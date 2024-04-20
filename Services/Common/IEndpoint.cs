namespace Services.Common;

public interface IEndpoint
{
    void AddRoute(IEndpointRouteBuilder app);
}

public interface IEndpoint<TResult> : IEndpoint
{
    Task<TResult> HandleAsync();
}

public interface IEndpoint<in TRequest, TResult> : IEndpoint
{
    Task<TResult> HandleAsync(TRequest request);
}