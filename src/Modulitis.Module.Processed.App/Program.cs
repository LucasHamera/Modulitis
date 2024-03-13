using CommandLine;
using Modulitis.Module.Processed.App;

Parser
    .Default
    .ParseArguments<CommandOptions>(args)
    .WithNotParsed(HandleParseError)
    .WithParsedAsync(Run);
    
static async Task Run(CommandOptions opts)
{
    //handle options
}

static void HandleParseError(IEnumerable<Error> errs)
{
    //handle errors
}