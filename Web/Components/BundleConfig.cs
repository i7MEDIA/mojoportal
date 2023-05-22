using System.Web;
using System.Web.UI;
using System.Web.Optimization;
using AspNet.ScriptManager.jQuery;
using Microsoft.Ajax.Utilities;
using System.Web.Configuration;

namespace mojoPortal.Web.Optimization
{
    public class BundleConfig
    {
        private static string protocol = "http";
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            protocol = Core.Helpers.WebHelper.IsSecureRequest() ? "https" : protocol;

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                  "~/Scripts/WebForms/WebForms.js",
                  "~/Scripts/WebForms/WebUIValidation.js",
                  "~/Scripts/WebForms/MenuStandards.js",
                  "~/Scripts/WebForms/Focus.js",
                  "~/Scripts/WebForms/GridView.js",
                  "~/Scripts/WebForms/DetailsView.js",
                  "~/Scripts/WebForms/TreeView.js",
                  "~/Scripts/WebForms/WebParts.js"
                  ));

            ScriptManager.ScriptResourceMapping.AddDefinition("WebFormsBundle", new ScriptResourceDefinition
            {
                Path = "~/bundles/WebFormsJs",
                CdnPath = $"{protocol}://ajax.aspnetcdn.com/ajax/4.5.1/1/WebFormsBundle.js",
                LoadSuccessExpression = "window.WebForm_PostBackOptions",
                CdnSupportsSecureConnection = true,
            });
            
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            ScriptManager.ScriptResourceMapping.AddDefinition("MsAjaxBundle", new ScriptResourceDefinition
            {
                Path = "~/bundles/MsAjaxJs",
                CdnPath = $"{protocol}://ajax.aspnetcdn.com/ajax/4.5.1/1/MsAjaxBundle.js",
                LoadSuccessExpression = "window.Sys",
                CdnSupportsSecureConnection = true

            });

            var ajaxVersion = "4.5.1/1";

            AddMsAjaxMapping("MicrosoftAjax.js", ajaxVersion, "window.Sys && Sys._Application && Sys.Observer");
            AddMsAjaxMapping("MicrosoftAjaxCore.js", ajaxVersion, "window.Type && Sys.Observer");
            AddMsAjaxMapping("MicrosoftAjaxGlobalization.js", ajaxVersion, "window.Sys && Sys.CultureInfo");
            AddMsAjaxMapping("MicrosoftAjaxSerialization.js", ajaxVersion, "window.Sys && Sys.Serialization");
            AddMsAjaxMapping("MicrosoftAjaxComponentModel.js", ajaxVersion, "window.Sys && Sys.CommandEventArgs");

            if (!WebConfigSettings.DisableAjaxToolkitBundlesAndScriptReferences)
            {

                ScriptManager.ScriptResourceMapping.AddDefinition("AjaxToolkitBundle", new ScriptResourceDefinition
                {
                    Path = "~/Scripts/AjaxControlToolkit/Bundle",
                    CdnPath = $"{protocol}://ajax.aspnetcdn.com/ajax/act/20_1_0/Scripts/AjaxControlToolkit/Bundle.js",
                    CdnSupportsSecureConnection = true
                });

                // for some reason could not get it to use the cdn without doing this
                var scripts = BundleTable.Bundles.GetBundleFor("~/bundles/WebFormsJs");
                if (scripts != null)
                { 
                    scripts.CdnPath = $"{protocol}://ajax.aspnetcdn.com/ajax/4.5.1/1/WebFormsBundle.js";  
                }
                scripts = BundleTable.Bundles.GetBundleFor("~/bundles/MsAjaxJs");
                if (scripts != null)
                {
                    scripts.CdnPath = $"{protocol}://ajax.aspnetcdn.com/ajax/4.5.1/1/MsAjaxBundle.js";
                }


                //var styles = BundleTable.Bundles.GetBundleFor("~/Content/AjaxControlToolkit/Styles/Bundle");
                //if (styles != null)
                //{
                //    if (WebConfigSettings.AjaxToolkitUseCdnForBundle)
                //    {
                //        styles.CdnPath = WebConfigSettings.AjaxToolkitCssBundleCdnUrl;
                //    }
                //}


            }



            BundleTable.EnableOptimizations = WebConfigSettings.BundlesForceOptimization;
            BundleTable.Bundles.UseCdn = WebConfigSettings.BundlesUseCdn;
            


        }

        private static void AddMsAjaxMapping(
            string name, 
            string version, 
            string loadSuccessExpression
            )
        {

            ScriptManager.ScriptResourceMapping.AddDefinition(name, new ScriptResourceDefinition
            {
                Path = "~/Scripts/WebForms/MsAjax/" + name,
                CdnPath = $"{protocol}://ajax.aspnetcdn.com/ajax/" + version + "/" + name,
                LoadSuccessExpression = loadSuccessExpression,
                CdnSupportsSecureConnection = true

            });
        }

    }
}
