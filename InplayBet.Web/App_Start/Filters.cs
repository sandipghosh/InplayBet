using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InplayBet.Web.Utilities;
namespace InplayBet.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = false, Inherited = false)]
    public class UserAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAccess), false)
                    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAccess), false);

                if (skipAuthorization)
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    //RedirectToRouteResult redirect = new RedirectToRouteResult(new RouteValueDictionary{
                    //{ "action", "ShowPopup" },
                    //{ "controller", "Base" },
                    //{ "area", "" });

                    if (filterContext.RequestContext.HttpContext.Session == null)
                        //filterContext.Result = redirect;
                        return;
                    else
                    {
                        if (filterContext.RequestContext.HttpContext.Session[SessionVeriables.UserKey] == null)
                            //filterContext.Result = redirect;
                            return;
                        else
                            base.OnActionExecuting(filterContext);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(filterContext);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAccess : Attribute
    {

    }
}