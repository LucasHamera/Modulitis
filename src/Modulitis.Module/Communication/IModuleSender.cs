namespace Modulitis.Module.Communication;

public interface IModuleSender
{
    Task SendAsync<TRequest>(TRequest request, CancellationToken token = default);
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken token = default);
}