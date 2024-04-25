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
        var response = await httpClient.PostAsJsonAsync("https://0280-217-113-12-12.ngrok-free.app:5015/api/sync", new
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

        var serverListIds = r.Lists.Select(l => l.Id).ToHashSet();
        applicationContext.TodoLists.RemoveRange(lists.Where(l => !serverListIds.Contains(l.Id)));

        // Обновление существующих листов и добавление новых
        foreach (var list in r.Lists)
        {
            var existingList = await applicationContext.TodoLists.FindAsync(list.Id);
            if (existingList != null)
            {
                existingList.ToDoItems = list.ToDoItems;
                existingList.Name = list.Name;
            }
            else
            {
                applicationContext.TodoLists.Add(list);
            }
        }

        // Сохранение изменений в БД
        await applicationContext.SaveChangesAsync();
        
        applicationContext.ChangeTracker.Clear();
    }
}

public class Response
{
    public required DateTime SyncedAt { get; set; }
    
    public required List<TodoList> Lists { get; set; }
}