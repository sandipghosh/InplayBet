﻿

namespace InplayBet.Web
{
    using InplayBet.Web.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            InjectorInitializer.Initialize();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        /// <summary>
        /// Handles the PreSendRequestHeaders event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("X-Frame-Options");
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is System.Threading.ThreadAbortException)
                return;

            ex.ExceptionValueTracker(new Dictionary<string, object>() { 
                {"Is AJAX Request",HttpContext.Current.Request.IsAjaxRequest().ToString()},
                {"Request URL",HttpContext.Current.Request.Url},
                {"Request Type", HttpContext.Current.Request.HttpMethod},
                {"Request Context", HttpContext.Current.Request.Form}
            });

            if (!HttpContext.Current.Request.IsAjaxRequest())
            {
                HttpContext.Current.Response.Redirect(string.Format("{0}/Error/Index",
                    CommonUtility.GetConfigData<string>("VirtualDirectory")));
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 500;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine(string.Format("Error Source: {0}", ex.InnerException.Source));
                sb.AppendLine(string.Format("Error Message: {0}", ex.InnerException.Message));
                sb.AppendLine(string.Format("Error Stack: {0}", ex.InnerException.StackTrace));
                HttpContext.Current.Response.Write(sb.ToString());
            }
        }
    }
}