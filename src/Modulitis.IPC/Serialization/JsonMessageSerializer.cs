using System.Text.Json;

namespace Modulitis.IPC.Serialization;

internal sealed class JsonMessageSerializer: IPayloadSerializer
{
    public Task<byte[]> SerializeAsync<T>(T message, CancellationToken token)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(message);
        return Task.FromResult(bytes);
    }
}