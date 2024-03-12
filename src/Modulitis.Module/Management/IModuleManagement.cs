namespace Modulitis.Module.Management;

public interface IModuleManagement
{
    public Task RunAsync(ModuleDefinition moduleDefinition);
    public Task TurnOffAsync();
}