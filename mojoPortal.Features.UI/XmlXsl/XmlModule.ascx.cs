//	Author:					
// Last Modified:			2011-01-24

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.XmlUI
{
	
	public partial  class XmlModule : SiteModuleControl 
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(XmlModule));

        private XmlConfiguration config = new XmlConfiguration();

        protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;

        private string xmlBasePath = string.Empty;
        private string xslBasePath = string.Empty;
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        
        protected void Page_Load(object sender, EventArgs e) 
		{
            LoadSettings();

            PopulateControls();

            
		}

        private void PopulateControls()
        {
            Title1.EditUrl = SiteRoot + "/XmlXsl/XmlEdit.aspx";
            Title1.EditText = XmlResources.XmlEditButton;

            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
            }


            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            string xmlUrl = string.Empty;
            string xslUrl = string.Empty;
            if (config.XmlFileSource.Length > 0) { xmlUrl = WebUtils.ResolveServerUrl(xmlBasePath + config.XmlFileSource); }
            if (config.XslFileSource.Length > 0) { xslUrl = WebUtils.ResolveServerUrl(xslBasePath + config.XslFileSource); }

            if(config.XmlUrl.Length > 0) { xmlUrl = config.XmlUrl; }
            if(config.XslUrl.Length > 0) { xslUrl = config.XslUrl; }

            Literal litContent;
            if (config.TrustContent)
            {
                litContent = litTrustedContent;
            }
            else
            {
                litContent = litUnTrustedContent;
            }

            if((xmlUrl.Length > 0)&&(xslUrl.Length > 0))
            {
                try
                {
                    litContent.Text = Core.Helpers.XmlHelper.TransformXML(xmlUrl, xslUrl);
                    if (litContent.Text.Length == 0)
                    {
                        //probably exception swallowed by XmlHelper
                        litContent.Text = XmlResources.GenericError;
                    }
                }
                catch (Exception ex)
                {
                    log.Info("swallowed excpetion to keep from breaking the page", ex);
                    litContent.Text = XmlResources.GenericError;
                }
            }

        }

        private void LoadSettings()
        {
            config = new XmlConfiguration(Settings);

            if (WebConfigSettings.XmlUseMediaFolder)
            {
                xmlBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/xml/";
                xslBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/xsl/";
            }
            else
            {
                xmlBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/xml/";
                xslBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/xsl/";
            }
            

            if (config.AllowExternalImages) allowedImageUrlRegexPattern = SecurityHelper.RegexAnyImageUrlPatern;
            UntrustedContent1.TrustedImageUrlPattern = allowedImageUrlRegexPattern;

        }

    }
}
