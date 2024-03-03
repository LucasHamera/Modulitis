namespace Modulitis.IPC.Client;

public interface IProcessConnector<in TOption>
{
    Task<ProcessConnection> ConnectAsync(TOption options);
}