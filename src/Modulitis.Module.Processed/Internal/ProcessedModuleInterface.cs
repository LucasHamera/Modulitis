using Modulitis.Module.Management;

namespace Modulitis.Module.Processed.Internal;

internal class ProcessedModuleInterface : IProcessedModuleInterface
{
    public Task RunAsync(ModuleDefinition moduleDefinition)
    {
        throw new NotImplementedException();
    }

    public Task TurnOffAsync()
    {
        throw new NotImplementedException();
    }

    public Task SendAsync<TRequest>(TRequest request, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DispatchAsync<TRequest>(TRequest request, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> DispatchAsync<TRequest, TResponse>(TRequest request, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}