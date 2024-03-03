using Modulitis.IPC.Serialization;

namespace Modulitis.IPC;

public class MessageSender: IMessageSender
{
    private readonly IMessageSerializer _messageSerializer;
    private readonly IProcessSender _processSender;

    public MessageSender(IMessageSerializer messageSerializer, IProcessSender processSender)
    {
        _messageSerializer = messageSerializer;
        _processSender = processSender;
    }

    public async Task SendAsync<T>(T message, CancellationToken token)
    {
        var bytes = await _messageSerializer.SerializeAsync(message, token);
        await _processSender.SendAsync(bytes, token);
    }

    public void Dispose()
    {
        _processSender.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _processSender.DisposeAsync();
    }
}