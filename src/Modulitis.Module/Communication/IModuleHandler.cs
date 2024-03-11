namespace Modulitis.Module.Communication;

public interface IModuleHandler<in TRequest>
{
    Task Handle(TRequest request, CancellationToken token = default);
}

public interface IModuleHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken token = default);
}