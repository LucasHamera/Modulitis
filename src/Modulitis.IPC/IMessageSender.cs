namespace Modulitis.IPC;

public interface IMessageSender: IAsyncDisposable, IDisposable
{
    Task SendAsync<T>(T message, CancellationToken token);
}