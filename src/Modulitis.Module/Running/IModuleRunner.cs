namespace Modulitis.Module.Running;

public interface IModuleRunner
{
    public Task<RunnedModule> RunAsync(ModuleDefinition moduleDefinition);
}