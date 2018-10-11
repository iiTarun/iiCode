using System.Web;
using System.Web.Optimization;

namespace Nom1Done.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                           "~/assets/js/main.js"
                           
                           ));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                                  "~/Scripts/jquery-2.1.4.min.js",                                                                      
                                  "~/Scripts/jquery-ui-1.12.1.min.js"
                                  ));

         

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                                 "~/assets/toastr/toastr.min.js"
                                 ));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                                "~/assets/select2/select2.min.js"
                                ));


            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                                 "~/Scripts/bootbox.min.js"
                                 ));


            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                               "~/Scripts/jquery.signalR-2.2.0.min.js",
                               "~/signalr/hubs"
                                ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                   "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.js",
                      "~/ Scripts/jquery.validate.unobtrusive.min.js"
                   ));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                               "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                       "~/Scripts/DataTables/jquery.bootstrap.js",
                       "~/Scripts/DataTables/jquery.dataTables.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/base/jquery-ui.min.css",
                          "~/Content/themes/base/jquery-ui.css",
                           "~/Content/bootstrap.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                          "~/Content/themes/base/jquery-ui.min.css",
                          "~/Content/themes/base/jquery-ui.css",
                          "~/Content/bootstrap.css",
                          "~/assets/css/font-awesome.min.css",
                          "~/assets/scss/style.css",
                          "~/assets/css/custome.css",
                          "~/Content/themes/base/jquery-ui.min.css",
                          "~/assets/css/lib/datatable/dataTables.bootstrap.min.css"
                    ));

            bundles.Add(new StyleBundle("~/Content/toastr/css").Include(
                    "~/assets/toastr/toastr.min.css"));

            bundles.Add(new StyleBundle("~/Content/datatable/css").Include(
                  "~/assets/css/jquery.new.dataTables.min.css"));

            bundles.Add(new StyleBundle("~/Content/select2/css").Include(
                   "~/assets/select2/select2.min.css"));
        }
    }
}
