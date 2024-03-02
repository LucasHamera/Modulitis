using Microsoft.Extensions.DependencyInjection;

namespace Modulitis.IPC.NamedPipe;

public class NamedPipeBuilder
{
    public NamedPipeBuilder(IServiceCollection services)
    {
        Services = services;
    }
    
    public IServiceCollection Services { get; }
}