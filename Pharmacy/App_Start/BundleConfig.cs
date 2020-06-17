using System.Web;
using System.Web.Optimization;

namespace Pharmacy
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                "~/Content/jquery-ui.min.css",
                "~/Content/AdminLTE/bootstrap.min.css",
                "~/Content/AdminLTE/font-awesome.min.css",
                "~/Content/AdminLTE/ionicons.min.css",
                "~/Content/AdminLTE/AdminLTE.min.css",
                "~/Content/AdminLTE/_all-skins.min.css",
                "~/Content/kendo.common.min.css",
                "~/Content/kendo.default.css"
                ));


            bundles.Add(new ScriptBundle("~/AdminLTE/js").Include(
                      "~/Scripts/AdminLTE/jquery-3.3.1.min.js",
                      "~/Scripts/AdminLTE/jquery-ui.min.js",
                       "~/Scripts/Kendo/kendo.all.min.js",
                       "~/Scripts/Kendo/kendo.aspnetmvc.js",
                       "~/Scripts/Common/jquery.validate.js",
                       "~/Scripts/Common/jquery.validate.unobtrusive.js",
                      "~/Scripts/AdminLTE/bootstrap.min.js",
                      "~/Scripts/AdminLTE/jquery.sparkline.min.js",
                      "~/Scripts/AdminLTE/jquery.knob.min.js",
                      "~/Scripts/AdminLTE/jquery.slimscroll.min.js",
                      "~/Scripts/AdminLTE/adminlte.min.js",
                      "~/Scripts/AdminLTE/demo.js"
                      ));
        }
    }
}
