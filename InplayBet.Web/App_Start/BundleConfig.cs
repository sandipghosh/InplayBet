
namespace InplayBet.Web
{
    using System;
    using InplayBet.Web.Utilities;
    using System.Web.Optimization;

    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            try
            {
                RegisterStyleBundles(bundles);
                RegisterScriptBundles(bundles);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bundles);
            }
        }

        /// <summary>
        /// Registers the script bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterScriptBundles(BundleCollection bundles)
        {
            try
            {
                Bundle scriptBundle = new Bundle("~/Scripts/CommonScript", new JsMinify());
                scriptBundle.Include("~/Scripts/jquery-2.1.3.min.js",
                    "~/Scripts/jquery-migrate-1.2.1.min.js",
                    "~/Scripts/jquery-ui.min.js",
                    "~/Scripts/consolelog.min.js",
                    "~/Scripts/cropbox-min.js",
                    "~/Scripts/AppScripts/Common.js",
                    "~/Scripts/AppScripts/MainScript.js"
                );
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/AjaxValidation", new JsMinify());
                scriptBundle.Include("~/Scripts/ChallengeManager.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/ChallengeScript", new JsMinify());
                scriptBundle.Include("~/Scripts/jquery.unobtrusive-ajax.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/jquery.validate.unobtrusive.min.js");
                BundleTable.Bundles.Add(scriptBundle);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bundles);
            }
        }

        /// <summary>
        /// Registers the style bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            try
            {
                Bundle styleBundle = new Bundle("~/Styles/CommonStyle", new CssMinify());
                styleBundle.Include("~/Styles/jquery-ui.min.css", "~/Styles/style.css");
                BundleTable.Bundles.Add(styleBundle);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bundles);
            }
        }
    }
}