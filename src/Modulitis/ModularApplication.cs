using Modulitis.Internal;

namespace Modulitis;

public class ModularApplication
{
    private readonly ModularApplicationBuilder _builder;

    public ModularApplication(ModularApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        _builder = builder;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var runningModule = await RunAllModules(_builder.ModuleRegistrations, token);
    }

    public static ModularApplicationBuilder CreateBuilder(string[] args)
    {
        var options = new ModularApplicationOptions
        {
            Args = args
        };
        return new ModularApplicationBuilder(options);
    }

    private async Task<IEnumerable<RunningModule>> RunAllModules(IEnumerable<ModuleRegistration> moduleRegistrations, CancellationToken token = default)
    {
        var runningModules = new List<RunningModule>();
        foreach (var registration in moduleRegistrations)
        {
            var runner = registration.Interface;
            
            await runner.RunAsync(registration.Definition);
            var runningModule = new RunningModule(ModuleId.Create(), registration.Definition, registration.Interface);
            runningModules.Add(runningModule);
        }
        return runningModules;
    }
}