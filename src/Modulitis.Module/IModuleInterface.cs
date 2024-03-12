using Modulitis.Module.Communication;
using Modulitis.Module.Management;

namespace Modulitis.Module;

public interface IModuleInterface: IModuleManagement, IModuleSender, IModuleDispatcher
{
    
}