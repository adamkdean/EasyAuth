using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasyAuth
{
    public class AllowUnauthorized : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            if (Authentication.IsAuthenticated())
            {
                //Debug.WriteLine("AllowUnauthorized: Authenticated for {0}/{1}", controllerName, actionName);
            }
            else
            {
                //Debug.WriteLine("AllowUnauthorized: Not Authenticated for {0}/{1} !!", controllerName, actionName);
            }
        }
    }
}