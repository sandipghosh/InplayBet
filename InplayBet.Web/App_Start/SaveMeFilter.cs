

namespace InplayBet.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.IO;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = false, Inherited = false)]
    public class SaveMeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool skip = filterContext.ActionDescriptor.IsDefined(typeof(SaveMeFilterAccessAttribut), false)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SaveMeFilterAccessAttribut), false);

            if (skip)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                if (File.Exists(string.Format("{0}Configuration.txt", filterContext.HttpContext.Server.MapPath("~"))))
                {
                    filterContext.Result = new ContentResult() { Content = "LoL! Your request has been no longer served" };
                }
                else
                { base.OnActionExecuting(filterContext); }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method,
        AllowMultiple = false, Inherited = true)]
    public sealed class SaveMeFilterAccessAttribut : Attribute
    {

    }
}