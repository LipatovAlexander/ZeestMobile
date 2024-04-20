namespace Services.Endpoints.Random;

public sealed class RandomEndpoint : IEndpoint<RandomRequest, RandomResponse>
{
    public Task<RandomResponse> HandleAsync(RandomRequest request)
    {
        var min = request.Min ?? int.MinValue;
        var max = request.Max ?? int.MaxValue;

        var response = new RandomResponse
        {
            Value = System.Random.Shared.Next(min, max)
        };

        return Task.FromResult(response);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/random", async ([AsParameters] RandomRequest request) => await HandleAsync(request));
    }
}