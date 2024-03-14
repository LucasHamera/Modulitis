using System.Diagnostics;

namespace Modulitis.Module.Processed.Internal;

internal sealed class ProcessedModuleInterface : IProcessedModuleInterface
{
    private const string PathToModuleProcessApp = "";
    
    private readonly Process _process= new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = PathToModuleProcessApp,
            UseShellExecute = true,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true
        }
    };

    private bool _isStarted = false;
    
    public ValueTask RunAsync(ModuleDefinition moduleDefinition)
    {
        var isStarted = _process.Start();
        if (!isStarted)
            throw new Exception();

        _isStarted = isStarted;
        return ValueTask.CompletedTask;
    }

    public ValueTask TurnOffAsync()
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