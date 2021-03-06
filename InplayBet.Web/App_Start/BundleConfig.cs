﻿
namespace InplayBet.Web
{
    using InplayBet.Web.Utilities;
    using System;
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
                    "~/Scripts/jquery.unobtrusive-ajax.min.js",
                    "~/Scripts/jquery.blockUI.js",
                    "~/Scripts/jquery.fs.boxer.min.js",
                    "~/Scripts/consolelog.min.js",
                    "~/Scripts/css3-mediaqueries.js",
                    "~/Scripts/easyTooltip.js",
                    "~/Scripts/AppScripts/UIScripts.js"
                );
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/MainScript", new JsMinify());
                scriptBundle.Include("~/Scripts/AppScripts/Common.js",
                    "~/Scripts/AppScripts/MainScript.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/RegistrationScript", new JsMinify());
                scriptBundle.Include("~/Scripts/cropbox-min.js",
                    "~/Scripts/AppScripts/RegistrationManager.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/ChallengeScript", new JsMinify());
                scriptBundle.Include("~/Scripts/AppScripts/ChallengeManager.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/Ranking", new JsMinify());
                scriptBundle.Include("~/Scripts/jquery.simplePagination.js",
                    "~/Scripts/AppScripts/RankingManager.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/AjaxValidation", new JsMinify());
                scriptBundle.Include("~/Scripts/jquery.validate.min.js",
                    "~/Scripts/jquery.validate.unobtrusive.min.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/NormalValidation", new JsMinify());
                scriptBundle.Include("~/Scripts/jquery.validate.min.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/Admin", new JsMinify());
                scriptBundle.Include("~/Areas/Admin/Scripts/jquery.jqGrid.min.js",
                    "~/Areas/Admin/Scripts/grid.locale-en.js",
                    "~/Areas/Admin/Scripts/jquery.linq-2.2.0.2.min.js",
                    "~/Areas/Admin/Scripts/grid-intregration.js",
                    "~/Areas/Admin/Scripts/AdminScript.js");
                BundleTable.Bundles.Add(scriptBundle);

                scriptBundle = new Bundle("~/Scripts/RangeSlider", new JsMinify());
                scriptBundle.Include("~/Scripts/ion.rangeSlider.min.js");
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
                styleBundle.Include("~/Styles/jquery-ui.min.css",
                    "~/Styles/jquery.fs.boxer.css",
                    "~/Styles/style.css",
                    "~/Styles/cropstyle.css");
                BundleTable.Bundles.Add(styleBundle);

                styleBundle = new Bundle("~/Styles/Ranking", new CssMinify());
                styleBundle.Include("~/Areas/Admin/Styles/jquery-ui-slider-pips.css",
                    "~/Styles/simplePagination.css");
                BundleTable.Bundles.Add(styleBundle);

                styleBundle = new Bundle("~/Styles/Admin", new CssMinify());
                styleBundle.Include("~/Areas/Admin/Styles/ui.jqgrid.css",
                    "~/Areas/Admin/Styles/AdminStyle.css");
                BundleTable.Bundles.Add(styleBundle);

                styleBundle = new Bundle("~/Styles/RangeSliderWeb", new CssMinify());
                styleBundle.Include("~/Styles/ion.rangeSlider.css",
                    "~/Styles/ion.rangeSlider.skinModern.css");
                BundleTable.Bundles.Add(styleBundle);

                styleBundle = new Bundle("~/Styles/RangeSliderMobile", new CssMinify());
                styleBundle.Include("~/Styles/ion.rangeSlider.css",
                    "~/Styles/ion.rangeSlider.skinHTML5.css");
                BundleTable.Bundles.Add(styleBundle);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(bundles);
            }
        }
    }
}