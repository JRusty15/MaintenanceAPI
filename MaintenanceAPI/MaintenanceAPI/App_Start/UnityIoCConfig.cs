using MaintenanceAPI.Repository;
using MaintenanceAPI.Repository.Interface;
using Microsoft.Practices.Unity;

namespace MaintenanceAPI.App_Start
{
    public static class UnityIoCConfig
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVehicleRepository, VehicleRepository>(new HierarchicalLifetimeManager());
        }
    }
}