using Modulitis.Module;
using Modulitis.Internal;
using Modulitis.Module.Communication;
using Modulitis.Module.Management;

namespace Modulitis;

public class ModularApplicationBuilder
{
    internal ModularApplicationBuilder(ModularApplicationOptions options)
    {
        
    }

    public HashSet<ModuleRegistration> ModuleRegistrations { get; } = new HashSet<ModuleRegistration>();

    public void AddModule(ModuleDefinition definition, IModuleInterface moduleInterface)
        => ModuleRegistrations.Add(new ModuleRegistration(definition, moduleInterface));
    
    public ModularApplication Build()
    {
        return new ModularApplication(this);
    }
}