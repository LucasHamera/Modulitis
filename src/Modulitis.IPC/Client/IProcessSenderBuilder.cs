namespace Modulitis.IPC.Client;

public interface IProcessSenderBuilder
{
    Task<IProcessSender> BuildAsync(CancellationToken token);
}