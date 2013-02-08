using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasyAuth
{
    public class RequireUnauthorized : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            if (Authentication.IsAuthenticated())
            {
                //Debug.WriteLine("RequireUnauthorized: Authenticated for {0}/{1}", controllerName, actionName);
            }
            else
            {
                //Debug.WriteLine("RequireUnauthorized: Not Authenticated for {0}/{1} !!", controllerName, actionName);
            }
        }
    }
}