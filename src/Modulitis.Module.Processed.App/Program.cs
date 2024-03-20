using CommandLine;
using Modulitis.Module.Processed.App;

Parser
    .Default
    .ParseArguments<CommandOptions>(args)
    .WithNotParsed(HandleParseError)
    .WithParsedAsync(Run);
    
static async Task Run(CommandOptions opts)
{
    var builder = WebApplication.CreateBuilder();
    builder.WebHost.ConfigureKestrel(
        opts => opts.ListenNamedPipe("my-test-pipe"));
    
    var app = builder.Build();

    await app.RunAsync();
}

static void HandleParseError(IEnumerable<Error> errs)
{
    //handle errors
}