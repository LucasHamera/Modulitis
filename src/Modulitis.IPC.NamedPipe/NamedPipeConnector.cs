using Modulitis.IPC.Client;

namespace Modulitis.IPC.NamedPipe;

public class NamedPipeConnector: IProcessConnector<NamedPipeClientOptions>
{
    public  Task<ProcessConnection> ConnectAsync(NamedPipeClientOptions options)
    {
        var senderBuilder = new NamedPipeSenderBuilder(options);
        var connection = new ProcessConnection(senderBuilder);
        return Task.FromResult(connection);
    }
}