using Modulitis.Module.Processed.Internal;

namespace Modulitis.Module.Processed;

public static class ProcessedModuleExtensions
{
    public static ModularApplicationBuilder AddProcessedModule(this ModularApplicationBuilder builder, ModuleDefinition moduleDefinition)
    {
        builder.AddModule(moduleDefinition, new ProcessedModuleInterface());
        return builder;
    }
}