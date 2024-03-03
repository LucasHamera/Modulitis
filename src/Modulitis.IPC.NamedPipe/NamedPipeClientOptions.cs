using System.IO.Pipes;
using System.Security.Principal;

namespace Modulitis.IPC.NamedPipe;

public class NamedPipeClientOptions
{
    public string ServerName { get; init; } = ".";
    public required string PipeName { get; init; }
    public PipeDirection Direction { get; init; } = PipeDirection.InOut;
    public PipeOptions Options { get; init; } = PipeOptions.None;
    public TokenImpersonationLevel ImpersonationLevel { get; init; } = TokenImpersonationLevel.None;
    public HandleInheritability Inheritability { get; init; } = HandleInheritability.None;
}