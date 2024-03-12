using Modulitis.Module;
using Modulitis.Module.Running;
using Modulitis.Module.Communication;

namespace Modulitis.Internal;

public record ModuleRegistration(ModuleDefinition Definition, IModuleSender Sender, IModuleRunner? Runner = default)
{
}