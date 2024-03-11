namespace Modulitis.Module.Communication;

public interface IModuleDispatcher
{
    Task DispatchAsync<TRequest>(TRequest request, CancellationToken token = default);
    Task<TResponse> DispatchAsync<TRequest, TResponse>(TRequest request, CancellationToken token = default);
}