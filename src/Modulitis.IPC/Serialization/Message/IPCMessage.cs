using MessagePack;
using Modulitis.IPC.Serialization.Message;

namespace Modulitis.IPC.Serialization;

[MessagePackObject]
internal sealed class IPCMessage
{
    public IPCMessage(IPCMessageHeader header, byte[] payload)
    {
        Header = header;
        Payload = payload;
    }
    
    [Key(0)]
    public IPCMessageHeader Header { get; private set; }
    [Key(1)]
    public byte[] Payload { get; }
}