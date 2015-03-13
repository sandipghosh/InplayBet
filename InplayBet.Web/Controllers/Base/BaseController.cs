

namespace InplayBet.Web.Controllers.Base
{
    using System.Web.Mvc;
    using InplayBet.Web.Utilities;
    using Microsoft.Ajax.Utilities;

    public class BaseController : Controller
    {
        public ActionResult AppConfigData()
        { 
            string js = string.Format(@"var appData = {0}",CommonUtility.AppsettingsToJson());

            Minifier minifier = new Minifier();
            string minifiedJs = minifier.MinifyJavaScript(js, new CodeSettings
            {
                EvalTreatment = EvalTreatment.MakeImmediateSafe,
                PreserveImportantComments = false
            });
            return JavaScript(js);
        }
    }
}
