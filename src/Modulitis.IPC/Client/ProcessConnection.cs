namespace Modulitis.IPC.Client;

public class ProcessConnection
{
    private readonly IProcessSenderBuilder _senderBuilder;

    public ProcessConnection(IProcessSenderBuilder senderBuilder)
    {
        _senderBuilder = senderBuilder;
    }

    public async Task<IProcessSender> CreateSenderAsync(CancellationToken token)
        => await _senderBuilder.BuildAsync(token);
}