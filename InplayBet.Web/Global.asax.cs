

namespace InplayBet.Web
{
    using InplayBet.Web.Utilities;
    using System;
    using System.Collections.Generic;
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
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                string reqPath = Request.Url.AbsolutePath;
                if (reqPath.ToLower().Contains("home"))
                {
                    string newPath = string.Format("{0}{1}{2}", Request.Url.Scheme,
                        Uri.SchemeDelimiter, Request.Url.Authority);
                    Response.Redirect(newPath, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
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
        /// Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Session_Start(object sender, EventArgs e)
        {
            // event is raised each time a new session is created
            //CommonUtility.LogToFileWithStack(string.Format("New session has been started, Session ID {0}",
            //    this.Session.SessionID.AsString()));
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
            // event is raised when a session is abandoned or expires
            //CommonUtility.LogToFileWithStack(string.Format("Current session has been expired, Session ID {0}",
            //    this.Session.SessionID.AsString()));
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