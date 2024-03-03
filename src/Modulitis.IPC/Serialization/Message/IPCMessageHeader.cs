using MessagePack;

namespace Modulitis.IPC.Serialization.Message;

[MessagePackObject]
internal sealed class IPCMessageHeader
{
    public IPCMessageHeader(string messageType)
    {
        MessageType = messageType;
    }
    
    [Key(0)]
    public string MessageType { get; private set; }
}