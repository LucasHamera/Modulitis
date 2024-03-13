using CommandLine;

namespace Modulitis.Module.Processed.App;

public class CommandOptions
{
    [Option('i', "id", Required = true, HelpText = "Module identificator.")]
    public Guid ModuleId { get; set; }
    
    [Option('n', "name", Required = true, HelpText = "Module name.")]
    public string ModuleName { get; set; }
}