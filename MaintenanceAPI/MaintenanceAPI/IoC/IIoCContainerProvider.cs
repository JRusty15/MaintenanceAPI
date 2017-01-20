using Microsoft.Practices.Unity;

namespace MaintenanceAPI.IoC
{
    public interface IIoCContainerProvider
    {
        IUnityContainer GetIocContainer();
    }
}