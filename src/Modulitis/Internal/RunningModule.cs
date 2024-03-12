using Modulitis.Module;
using Modulitis.Module.Communication;

namespace Modulitis.Internal;

internal record RunningModule(ModuleId Id, ModuleDefinition Definition, IModuleSender Sender);