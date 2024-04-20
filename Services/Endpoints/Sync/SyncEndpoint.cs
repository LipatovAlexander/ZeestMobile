using Services.Model;

namespace Services.Endpoints.Sync;

public class SyncEndpoint : IEndpoint<SyncRequest, UserData>
{
    private static readonly Dictionary<Guid, UserData> _storage = new();
    
    public async Task<UserData> HandleAsync(SyncRequest request)
    {
        var userData = _storage.GetValueOrDefault(request.UserId, new UserData
        {
            SyncedAt = DateTime.MinValue,
            Lists = []
        });

        if (request.SyncedAt == userData.SyncedAt)
        {
            _storage[request.UserId] = new UserData
            {
                SyncedAt = DateTime.UtcNow,
                Lists = request.Lists
            };
        }

        return _storage[request.UserId];
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("sync", HandleAsync);
    }
}

public class UserData
{
    public required DateTime SyncedAt { get; set; }
    
    public required List<TodoList> Lists { get; set; }
}