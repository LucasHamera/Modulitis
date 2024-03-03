using MessagePack;
using Modulitis.IPC.Serialization.Message;

namespace Modulitis.IPC.Serialization;

internal class IPCMessageSerializer
{
    private readonly IPayloadSerializer _payloadSerializer;
    public IPCMessageSerializer(IPayloadSerializer payloadSerializer)
    {
        _payloadSerializer = payloadSerializer;
    }
    
    ReadOnlySpan<byte> Serialize<T>(T message)
    {
        var messageType = typeof(T).Name;
        var header = new IPCMessageHeader(messageType);

        var payload = _payloadSerializer.Serialize(message);

        var ipcMessage = new IPCMessage(header, payload);
        return MessagePackSerializer.Serialize(ipcMessage);
    }
}