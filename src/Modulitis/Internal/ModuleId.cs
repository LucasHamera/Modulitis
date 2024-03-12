namespace Modulitis.Internal;

internal record ModuleId(Guid Id)
{
    public static ModuleId Create()
        => new ModuleId(Guid.NewGuid());
}