namespace Modulitis.IPC;

public interface IProcessSender: IAsyncDisposable, IDisposable
{
    Task SendAsync(byte[] bytes, CancellationToken token);
}