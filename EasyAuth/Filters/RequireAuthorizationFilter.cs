using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasyAuth
{
    public class RequireAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.RouteData.Values["action"];
            var allowUnauthorized = false;

            if (filterContext.Controller.GetType().GetMethods().Any(x => x.Name.Equals(actionName)))
            {
                MethodInfo method = filterContext.Controller.GetType().GetMethods().First(x => x.Name.Equals(actionName));
                var allowUnauthAttrs = method.GetCustomAttributes(typeof(AllowUnauthorized), true);
                allowUnauthorized = allowUnauthAttrs.Length > 0;
            }

            if (allowUnauthorized)
            {
                Debug.WriteLine("AllowUnauthorized");
            }
            else if (Authentication.IsAuthenticated())
            {
                Debug.WriteLine("RequireAuthorization, Authenticated");
            }
            else
            {
                Debug.WriteLine("RequireAuthorization, Unauthorized");
            }
        }
    }
}