using System.Web.Http;

namespace MaintenanceAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new AuthorizeAttribute());//<=HTTP not MVC

            // Add Global E.H. here as well
        }
    }
}
