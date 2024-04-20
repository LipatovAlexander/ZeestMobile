using Microsoft.AspNetCore.Mvc;
using Services.Model;

namespace Services.Endpoints.Sync;

public class SyncEndpoint : IEndpoint<SyncRequest, UserData>
{
    private static readonly Dictionary<string, UserData> _storage = new();
    
    public async Task<UserData> HandleAsync([FromBody] SyncRequest request)
    {
        var userData = _storage.GetValueOrDefault(request.Username, new UserData
        {
            SyncedAt = DateTime.MinValue,
            Lists = []
        });
        
        if (request.SyncedAt == userData.SyncedAt)
        {
            _storage[request.Username] = new UserData
            {
                SyncedAt = DateTime.UtcNow,
                Lists = request.Lists
            };
        }

        return _storage.GetValueOrDefault(request.Username, new UserData
        {
            SyncedAt = DateTime.UtcNow,
            Lists = []
        });
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