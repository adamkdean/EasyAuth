using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EasyAuth.Storage;

namespace EasyAuth.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        static IUserStore UserStore = EntityUserStore.Instance;
        
        protected void Application_Start()
        {
            Authentication.UserStore = UserStore;
            //Authentication.HashProviderType = typeof(SHA512HashProvider);

            // ASP.NET Generated code
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            Authentication.HttpContext = HttpContext.Current;
        }
    }
}