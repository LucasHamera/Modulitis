using Modulitis.Module;
using Modulitis.Internal;
using Modulitis.Module.Running;
using Modulitis.Module.Communication;

namespace Modulitis;

public class ModularApplicationBuilder
{
    internal ModularApplicationBuilder(ModularApplicationOptions options)
    {
        
    }

    public HashSet<ModuleRegistration> ModuleRegistrations { get; } = new HashSet<ModuleRegistration>();
    
    public void AddModule(ModuleDefinition definition, IModuleSender sender)
        => ModuleRegistrations.Add(new ModuleRegistration(definition, sender));

    public void AddModule(ModuleDefinition definition, IModuleSender sender, IModuleRunner runner)
        => ModuleRegistrations.Add(new ModuleRegistration(definition, sender, runner));
    
    public ModularApplication Build()
    {
        return new ModularApplication(this);
    }
}