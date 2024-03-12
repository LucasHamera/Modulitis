namespace Modulitis.Module.Running;

public interface IModuleRunner
{
    public Task RunAsync(ModuleDefinition moduleDefinition);
}