// Author:					
// Created:				    2009-05-01
// Last Modified:			2017-10-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
    public class mojoHelpLink : HyperLink
    {
        private string helpKey = string.Empty;
        
        private bool helpIsEnabled = true;
        //private SiteSettings siteSettings = null;


        public String HelpKey
        {
            get { return helpKey; }
            set { helpKey = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (HttpContext.Current == null) { return; }
            EnableViewState = false;
            helpIsEnabled = (!WebConfigSettings.DisableHelpSystem);

            if (!helpIsEnabled) { return; }

            //siteSettings = CacheHelper.GetCurrentSiteSettings();
            //if ((siteSettings == null) || (siteSettings.SiteGuid == Guid.Empty))
            //{
            //    helpIsEnabled = false;
            //    return;
            //}

            this.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/Help.aspx?helpkey=" + Page.Server.UrlEncode(helpKey);
            this.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/question.png");
            this.ToolTip = Resource.HelpLink;
            this.Text = Resource.HelpLink;
            
            this.CssClass += " mhelp cblink";

            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null) { basePage.ScriptConfig.IncludeColorBox = true; }

            string initScript = "$('a.mhelp').colorbox({width:'85%', height:'85%', iframe:true, maxWidth:'95%',maxHeight:'95%'});";

            ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "helplink", "\n<script type=\"text/javascript\" >"
                   + initScript.ToString() + "</script>", false);
           
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

           
            if ((helpIsEnabled)&&(helpKey.Length > 0))
            {
                base.Render(writer);
            }
           
        }

        public static void AddHelpLink(
            Panel parentControl,
            string helpkey)
        {
            Literal litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

            mojoHelpLink helpLinkButton = new mojoHelpLink();
            helpLinkButton.HelpKey = helpkey;
            parentControl.Controls.Add(helpLinkButton);

            litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

        }

		public static Control GetHelpLinkControl(string helpKey)
		{
			mojoHelpLink helpLinkButton = new mojoHelpLink();
			helpLinkButton.HelpKey = helpKey;

			return helpLinkButton;
		}

    }
}
