using System.Web;
using System.Web.Optimization;

namespace SistemaOlcar
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/responsive.dataTables.css",
                      "~/Content/DataTables/css/buttons.dataTables.min.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/Site.css",
                      "~/Content/sweetalert.css"));

            bundles.Add(new Bundle("~/bundles/complementos").Include(
                       "~/Scripts/fontawesome/all.min.js",
                       "~/Scripts/scripts.js",
                       "~/Scripts/DataTables/jquery.dataTables.js",
                       "~/Scripts/DataTables/dataTables.buttons.min.js",
                       "~/Scripts/DataTables/dataTables.responsive.js",
                       "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/sweetalert.min.js"
                       //"~/Scripts/select2.min.js"//agregado
                       ));
        }
    }
}
