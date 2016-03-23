using System.Web;
using System.Web.UI;
using System.Web.Optimization;
using AspNet.ScriptManager.jQuery;

namespace mojoPortal.Web.Optimization
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //string jqueryVersion = "2.1.3";

            //ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            //{
            //    Path = "~/Scripts/jquery-" + jqueryVersion + ".min.js",
            //    DebugPath = "~/Scripts/jquery-" + jqueryVersion + ".js",
            //    CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + jqueryVersion + ".min.js",
            //    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + jqueryVersion + ".js",
            //    CdnSupportsSecureConnection = true,
            //    LoadSuccessExpression = "window.jQuery"
            //});

            //string jqueryUIVersion = "1.11.4";
            //ScriptManager.ScriptResourceMapping.AddDefinition("jquery.ui.combined", new ScriptResourceDefinition
            //{
            //    Path = "~/Scripts/jquery-ui-" + jqueryUIVersion + ".min.js",
            //    DebugPath = "~/Scripts/jquery-ui-" + jqueryUIVersion + ".js",
            //    CdnPath = "http://ajax.aspnetcdn.com/ajax/jquery.ui/" + jqueryUIVersion + "/jquery-ui.min.js",
            //    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jquery.ui/" + jqueryUIVersion + "/jquery-ui.js",
            //    CdnSupportsSecureConnection = true
            //});





            // var cssBundle = new StyleBundle("~/bundles/css");
            // cssBundle.Include("~/Content/Site.less", "~/Content/bootstrap/bootstrap.less");
            // //cssBundle.Transforms.Add(cssTransformer);
            // //cssBundle.Orderer = nullOrderer;
            // bundles.Add(cssBundle);

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //         "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            ScriptManager.ScriptResourceMapping.AddDefinition("WebFormsBundle", new ScriptResourceDefinition
            {
                Path = "~/bundles/WebFormsJs",
                CdnPath = "http://ajax.aspnetcdn.com/ajax/4.5/6/WebFormsBundle.js",
                LoadSuccessExpression = "window.WebForm_PostBackOptions",
                CdnSupportsSecureConnection = true
            });

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                  "~/Scripts/WebForms/WebForms.js",
                  "~/Scripts/WebForms/WebUIValidation.js",
                  "~/Scripts/WebForms/MenuStandards.js",
                  "~/Scripts/WebForms/Focus.js",
                  "~/Scripts/WebForms/GridView.js",
                  "~/Scripts/WebForms/DetailsView.js",
                  "~/Scripts/WebForms/TreeView.js",
                  "~/Scripts/WebForms/WebParts.js"));

            ScriptManager.ScriptResourceMapping.AddDefinition("MsAjaxBundle", new ScriptResourceDefinition
            {

                Path = "~/bundles/MsAjaxJs",
                CdnPath = "https://ajax.aspnetcdn.com/ajax/4.5/6/MsAjaxBundle.js",
                LoadSuccessExpression = "window.Sys",
                CdnSupportsSecureConnection = true

            });

            //PreApplicationStartCode.AddMsAjaxMapping("MicrosoftAjax.js", "window.Sys && Sys._Application && Sys.Observer");
            //PreApplicationStartCode.AddMsAjaxMapping("MicrosoftAjaxCore.js", "window.Type && Sys.Observer");
            //PreApplicationStartCode.AddMsAjaxMapping("MicrosoftAjaxGlobalization.js", "window.Sys && Sys.CultureInfo");
            //PreApplicationStartCode.AddMsAjaxMapping("MicrosoftAjaxSerialization.js", "window.Sys && Sys.Serialization");
            //PreApplicationStartCode.AddMsAjaxMapping("MicrosoftAjaxComponentModel.js", "window.Sys && Sys.CommandEventArgs");

            AddMsAjaxMapping("MicrosoftAjax.js", "window.Sys && Sys._Application && Sys.Observer");
            AddMsAjaxMapping("MicrosoftAjaxCore.js", "window.Type && Sys.Observer");
            AddMsAjaxMapping("MicrosoftAjaxGlobalization.js", "window.Sys && Sys.CultureInfo");
            AddMsAjaxMapping("MicrosoftAjaxSerialization.js", "window.Sys && Sys.Serialization");
            AddMsAjaxMapping("MicrosoftAjaxComponentModel.js", "window.Sys && Sys.CommandEventArgs");

            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            if (!WebConfigSettings.DisableAjaxToolkitBundlesAndScriptReferences)
            {
                var scripts = BundleTable.Bundles.GetBundleFor("~/Scripts/AjaxControlToolkit/Bundle");
                if (scripts != null)
                {
                    if (WebConfigSettings.AjaxToolkitUseCdnForBundle)
                    {
                        scripts.CdnPath = WebConfigSettings.AjaxToolkitJsBundleCdnUrl;
                    }
                }


                var styles = BundleTable.Bundles.GetBundleFor("~/Content/AjaxControlToolkit/Styles/Bundle");
                if (styles != null)
                {
                    if (WebConfigSettings.AjaxToolkitUseCdnForBundle)
                    {
                        styles.CdnPath = WebConfigSettings.AjaxToolkitCssBundleCdnUrl;
                    }
                }


            }



            BundleTable.EnableOptimizations = WebConfigSettings.BundlesForceOptimization;
            BundleTable.Bundles.UseCdn = WebConfigSettings.BundlesUseCdn;


        }

        private static void AddMsAjaxMapping(string name, string version
            //, string loadSuccessExpression
            )
        {

            ScriptManager.ScriptResourceMapping.AddDefinition(name, new ScriptResourceDefinition
            {
                Path = "~/Scripts/WebForms/MsAjax/" + name,
                CdnPath = "//ajax.aspnetcdn.com/ajax/" + version + "/" + name,
               // LoadSuccessExpression = loadSuccessExpression,
                CdnSupportsSecureConnection = true

            });
        }

    }
}
