using MessagePack;
using Modulitis.IPC.Serialization.Message;

namespace Modulitis.IPC.Serialization;

internal class IPCMessageSerializer: IMessageSerializer
{
    private readonly IPayloadSerializer _payloadSerializer;
    public IPCMessageSerializer(IPayloadSerializer payloadSerializer)
    {
        _payloadSerializer = payloadSerializer;
    }
    
    public async Task<byte[]> SerializeAsync<T>(T message, CancellationToken token)
    {
        var messageType = typeof(T).Name;
        var header = new IPCMessageHeader(messageType);

        var payload = await _payloadSerializer.SerializeAsync(message, token);

        var ipcMessage = new IPCMessage(header, payload);
        var bytes = MessagePackSerializer.Serialize(ipcMessage);
        
        return bytes;
    }
}