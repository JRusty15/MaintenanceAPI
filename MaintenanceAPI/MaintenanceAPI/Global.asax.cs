using MaintenanceAPI.IoC;
using MaintenanceAPI.Repository;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MaintenanceAPI.App_Start;

namespace MaintenanceAPI
{
    public class WebApiApplication : HttpApplication, IIoCContainerProvider
    {
        protected void Application_Start()
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;

            config.DependencyResolver = new UnityResolver(((IIoCContainerProvider)this).GetIocContainer());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string userName;
            string password;
            GetBasicAuthorizationHeaderUserCredential(out userName, out password);
            var userRepository = new UserRepository();
            var user = userRepository.Login(userName, password);

            if(user != null && user.UserID > 0)
            {
                SetPrincipal(user);
            }
        }

        private static void SetPrincipal(System.Security.Principal.IPrincipal principal)
        {
            System.Threading.Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        /// <summary>
        /// Returns a tupple containing the Username and password from a basic authorization token in the HTTP Header.
        /// If the header token is not there or is not basic, a null, null tupple will be returned.
        /// </summary>
        /// <returns></returns>
        private static void GetBasicAuthorizationHeaderUserCredential(out string userName, out string password)
        {
            userName = null;
            password = null;
            var auth = HttpContext.Current.Request.Headers["Authorization"];
            if (auth != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(auth);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic",
                        StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(authHeaderVal.Parameter));

                    int separator = credentials.IndexOf(':');
                    userName = credentials.Substring(0, separator);
                    password = credentials.Substring(separator + 1);
                }
            }
        }

        IUnityContainer IIoCContainerProvider.GetIocContainer()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                UnityIoCConfig.RegisterTypes(_container);
            }
            return _container;
        }
        IUnityContainer _container;
    }
}
