//	Author:				
// Created:			    2005-11-19
// Last Modified:		2014-05-07

using System;
using System.IO;
using System.Web;
using System.Web.UI;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ContentUI 
{
	
	public partial class HtmlFragmentInclude : SiteModuleControl 
	{
        private string instanceCssClass = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }
        

        protected void Page_Load(object sender, EventArgs e)
		{
			Title1.EditUrl = SiteRoot + "/HtmlInclude/Edit.aspx";
            Title1.EditText = HtmlIncludeResources.HtmlFragmentIncludeEditLink;
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;

            }

			string includePath;

            if (WebConfigSettings.HtmlFragmentUseMediaFolder)
            {
                includePath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot()
                + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/htmlfragments");
            }
            else
            {
                includePath = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot()
                + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/htmlfragments");
            }
            

			string includeFileName = Settings["HtmlFragmentSourceSetting"].ToString();
			string includeContentFile = includePath + Path.DirectorySeparatorChar + includeFileName;
			
			if (includeFileName != null)
			{
				if  (File.Exists(includeContentFile)) 
				{
					FileInfo file = new FileInfo(includeContentFile);
                    using (StreamReader sr = file.OpenText())
                    {
                        this.lblInclude.Text = sr.ReadToEnd();
                        
                    }
				}
				else 
				{
					Controls.Add(new LiteralControl("<br /><span class='txterror'>File " + includeContentFile + " not found.<br />"));
				}
			}

            if (Settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = Settings["CustomCssClassSetting"].ToString();
            }
            if (instanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(instanceCssClass); }
		}

	}
}
