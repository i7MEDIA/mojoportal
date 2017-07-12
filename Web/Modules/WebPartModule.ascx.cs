/// Author:             	
/// Created:            	2006-06-02
/// Last Modified:			2007-12-15

#if !MONO

using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Controls;

namespace mojoPortal.Web.WebPartUI
{
    
    public partial class WebPartModule : SiteModuleControl
    {
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            Title1.Visible = !this.RenderInWebPartMode;

            LoadWebPart();
        }


        protected void LoadWebPart()
        {
            String partIDString = String.Empty;
            if (Settings.Contains("WebPartModuleWebPartSetting"))
            {
                partIDString = Settings["WebPartModuleWebPartSetting"].ToString();
            }

            if (partIDString.Length == 36)
            {
                Guid webPartID = new Guid(partIDString);
                WebPartContent webPartContent = new WebPartContent(webPartID);
                if (webPartContent.WebPartId != Guid.Empty)
                {
                    String path = HttpContext.Current.Server.MapPath("~/bin")
                        + Path.DirectorySeparatorChar + webPartContent.AssemblyName + ".dll";
                    Assembly assembly = Assembly.LoadFrom(path);
                    Type type = assembly.GetType(webPartContent.ClassName, true, true);
                    WebPart webPart = Activator.CreateInstance(type) as WebPart;
                    if (webPart != null)
                    {
                       this.Title = webPart.Title;
                       this.Description = webPart.Description;
                       this.ModuleConfiguration.ModuleTitle = webPart.Title;
                       this.pnlPlaceHolder.Controls.Add(webPart);
                    }
                    
                }
            }
        }

    }
}
#endif
