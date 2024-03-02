namespace Modulitis.IPC.NamedPipe.Extensions;

public static class NamedPipeExtensions
{
    public static NamedPipeBuilder AddNamedPipe(this IPCBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        return new NamedPipeBuilder(builder.Services);
    }
}