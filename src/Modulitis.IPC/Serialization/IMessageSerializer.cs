namespace Modulitis.IPC.Serialization;

public interface IMessageSerializer
{
    byte[] Serialize<T>(T message);
}