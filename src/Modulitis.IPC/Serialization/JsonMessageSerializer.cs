using System.Text.Json;

namespace Modulitis.IPC.Serialization;

internal sealed class JsonMessageSerializer: IPayloadSerializer
{
    public byte[] Serialize<T>(T message)
    {
        return JsonSerializer.SerializeToUtf8Bytes(message);
    }
}