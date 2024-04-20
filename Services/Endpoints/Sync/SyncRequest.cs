using Services.Model;

namespace Services.Endpoints.Sync;

public class SyncRequest
{
    public required DateTime SyncedAt { get; set; }

    public required Guid UserId { get; set; }
    
    public required List<TodoList> Lists { get; set; }
}