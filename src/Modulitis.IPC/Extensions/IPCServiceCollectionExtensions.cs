using Microsoft.Extensions.DependencyInjection;

namespace Modulitis.IPC.Extensions;

public static class IPCServiceCollectionExtensions
{
    public static IPCBuilder AddIPC(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        
        return new IPCBuilder(serviceCollection);
    }
}