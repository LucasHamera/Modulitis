using System.IO.Pipes;
using Modulitis.IPC.Client;

namespace Modulitis.IPC.NamedPipe;

public sealed class NamedPipeSenderBuilder: IProcessSenderBuilder
{
    private readonly NamedPipeClientOptions _options;

    public NamedPipeSenderBuilder(NamedPipeClientOptions options)
    {
        _options = options;
    }

    public async Task<IProcessSender> BuildAsync(CancellationToken token)
    {
        var clientStream = new NamedPipeClientStream(_options.ServerName, _options.PipeName, _options.Direction, _options.Options, _options.ImpersonationLevel, _options.Inheritability);
        await clientStream.ConnectAsync(); 
        return new NamedPipeSender(clientStream);
    }
}