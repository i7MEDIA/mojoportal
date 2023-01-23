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
        private bool helpIsEnabled = true;

        public string HelpKey { get; set; } = string.Empty;

        public bool ShowText { get; set; } = false;

        public bool UseImage { get; set; } = true;

        public bool CreateJSHelpObject { get; set; } = false;

        public string ModalSize { get; set; } = "fluid-large";
        
		protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (HttpContext.Current == null) { return; }
            EnableViewState = false;
            helpIsEnabled = (!WebConfigSettings.DisableHelpSystem);

            if (!helpIsEnabled) { return; }

            this.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/Help.aspx?helpkey=" + Page.Server.UrlEncode(HelpKey);

            if (UseImage)
            {
                this.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/question.png");
            }

            if (string.IsNullOrWhiteSpace(ToolTip))
            {
                ToolTip = Resource.HelpLink;
            }

            if (string.IsNullOrWhiteSpace(Text) && ShowText)
            {
                Text = Resource.HelpLink;
            }

            if (!string.IsNullOrWhiteSpace(ModalSize))
            {
                this.Attributes.Add("data-size", ModalSize);
            }

            this.Attributes.Add("data-modal", string.Empty);
            this.Attributes.Add("data-close-text", Resource.CloseDialogButton);
            this.Attributes.Add("data-modal-type", "iframe");

            CssClass += " mhelp";

		}

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

			if (Page is mojoBasePage basePage)
			{
				if (string.IsNullOrWhiteSpace(Global.SkinConfig.ModalTemplatePath) || string.IsNullOrWhiteSpace(Global.SkinConfig.ModalScriptPath))
				{
					basePage.ScriptConfig.IncludeColorBox = true;
					ScriptManager.RegisterStartupScript(this, typeof(Page), "mojoHelpLink", $"<script data-loader=\"helplink\" src=\"{Global.SkinConfig.HelpLinkScriptPath}\"></script>", false);
					this.CssClass += " cblink";
				}
				else
				{
					basePage.EnsureDefaultModal();
				}
			}

			if (helpIsEnabled && HelpKey.Length > 0)
            {
                base.Render(writer);
            }
           
        }

        public static void AddHelpLink(Panel parentControl, string helpkey)
        {
            Literal litSpace = new() { Text = "&nbsp;" };

            parentControl.Controls.Add(litSpace);
            mojoHelpLink helpLink = new() { HelpKey = helpkey };
            parentControl.Controls.Add(helpLink);

            parentControl.Controls.Add(litSpace);
        }

		public static Control GetHelpLinkControl(string helpKey)
		{
            return new mojoHelpLink { HelpKey = helpKey };
		}

    }
}
