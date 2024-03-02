using Microsoft.Extensions.DependencyInjection;

namespace Modulitis.IPC;

public class IPCBuilder
{
    public IPCBuilder(IServiceCollection services)
    {
        Services = services;
    }
    
    public IServiceCollection Services { get; }
}