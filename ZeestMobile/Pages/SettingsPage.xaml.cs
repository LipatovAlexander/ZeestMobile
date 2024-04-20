using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Model;

namespace ZeestMobile.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly ApplicationContext _applicationContext;

    public SettingsPage(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
        InitializeComponent();
    }

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        var lists = await _applicationContext.TodoLists
            .Include(td => td.Items)
            .ToListAsync();

        using var httpClient = new HttpClient();
        var response = await httpClient.PostAsJsonAsync("http://localhost:5015/api/sync", new
        {
            syncedAt = Preferences.Get("synced_at", null),
            lists,
            userId = Preferences.Get("user_id", null)
        });

        response.EnsureSuccessStatusCode();
        var r = await response.Content.ReadFromJsonAsync<Response>();
        
        Preferences.Set("synced_at", r!.SyncedAt);

        _applicationContext.TodoLists.RemoveRange(lists);
        
        _applicationContext.TodoLists.AddRange(r.Lists);

        await _applicationContext.SaveChangesAsync();
    }
}

public class Response
{
    public required DateTime SyncedAt { get; set; }
    
    public required List<TodoList> Lists { get; set; }
}