using System.IO.Pipes;

namespace Modulitis.IPC.NamedPipe;

internal sealed class NamedPipeSender: IProcessSender
{
    private readonly NamedPipeClientStream _clientStream;

    public NamedPipeSender(NamedPipeClientStream clientStream)
    {
        _clientStream = clientStream;
    }

    public async Task SendAsync(byte[] bytes, CancellationToken token)
    {
        await _clientStream.WriteAsync(bytes, token);
        await _clientStream.FlushAsync();
    }

    public void Dispose()
    {
        _clientStream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _clientStream.DisposeAsync();
    }
}