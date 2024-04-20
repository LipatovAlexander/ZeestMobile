using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Model;

namespace ZeestMobile.Services;

public class Synchronizer(ApplicationContext applicationContext)
{
    public async Task SyncAsync()
    {
        var lists = await applicationContext.TodoLists
            .ToListAsync();

        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync("http://localhost:5015/api/sync", new
        {
            syncedAt = Preferences.Get("synced_at", DateTime.MinValue).ToString("O"),
            lists,
            username = Preferences.Get("username", null)
        });

        var c =await response.Content.ReadAsStringAsync();

        Console.WriteLine(c);
        response.EnsureSuccessStatusCode();
        var r = await response.Content.ReadFromJsonAsync<Response>();
        
        Preferences.Set("synced_at", r!.SyncedAt);

        applicationContext.TodoLists.RemoveRange(lists);
        await applicationContext.SaveChangesAsync();

        applicationContext.TodoLists.AddRange(r.Lists);
        await applicationContext.SaveChangesAsync();
    }
}

public class Response
{
    public required DateTime SyncedAt { get; set; }
    
    public required List<TodoList> Lists { get; set; }
}