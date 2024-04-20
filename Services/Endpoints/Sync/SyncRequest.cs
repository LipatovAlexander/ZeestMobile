using Services.Model;

namespace Services.Endpoints.Sync;

public class SyncRequest
{
    public required DateTime SyncedAt { get; set; }

    public required string Username { get; set; }
    
    public required List<TodoList> Lists { get; set; }
}