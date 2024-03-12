using Modulitis.Module;

namespace Modulitis.Internal;

public record ModuleRegistration(ModuleDefinition Definition, IModuleInterface Interface)
{
}