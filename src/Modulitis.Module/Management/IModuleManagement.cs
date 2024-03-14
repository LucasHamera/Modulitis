namespace Modulitis.Module.Management;

public interface IModuleManagement
{
    public ValueTask RunAsync(ModuleDefinition moduleDefinition);
    public ValueTask TurnOffAsync();
}