namespace Modulitis.IPC.Serialization;

public interface IMessageSerializer
{
    Task<byte[]> SerializeAsync<T>(T message, CancellationToken token);
}