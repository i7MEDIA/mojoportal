//		Author:				
//		Created:			2006-12-13
//		Last Modified:		2008-08-15
//		
//     This control is really just designed for use in mojoPortal
//     and external projects that plug into mojoPortal.
//     

using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Globalization;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// This control is deprecated, you should use the portal:mojoHelpLink from the mojoPortal.Web project instead of this one.
    /// </summary>
    [Obsolete("This is a deprecated control, you should use the mojoHelpLink from the mojoPortal.Web project.")]
    public class HelpLinkButton : WebControl, INamingContainer
    {
        private bool render = true;

        #region Constructors

        public HelpLinkButton()
		{
			EnsureChildControls();

            
		}

		#endregion

        #region Control Declarations

        protected Literal litHelpLink;

        #endregion

        #region Private Properties

        private string helpKey = string.Empty;
        private string text = string.Empty;
        private string imageUrl = string.Empty;
        private string cssBaseUrl = "~/Data/style";
        private string navigateUrl = "~/Help.aspx";
        private int modalWidth = 400;
        private int modalHeight = 350;
        private int imageWidth = 0;
        private int imageHeight = 0;
        private string scriptDirectory = "~/ClientScript";

        #endregion

        #region Public Properties

        public String HelpKey
        {
            get {return helpKey; }
            set { helpKey = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl;}
            set { imageUrl = value; }
        }

        public string CssBaseUrl
        {
            get { return cssBaseUrl; }
            set { cssBaseUrl = value; }
        }

        public string NavigateUrl
        {
            get { return navigateUrl; }
            set { navigateUrl = value; }
        }

        public int ModalWidth
        {
            get { return modalWidth; }
            set { modalWidth = value; }
        }

        public int ModalHeight
        {
            get { return modalHeight; }
            set { modalHeight = value; }
        }

        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; }
        }

        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }

        [Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript")]
        public string ScriptDirectory
        {
            get { return scriptDirectory; }
            set { scriptDirectory = value; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EnableViewState = false;
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (render)
                {
                    this.litHelpLink.RenderControl(writer);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (ConfigurationManager.AppSettings["DisableHelpSystem"] != null)
            {
                
                //if (ConfigurationManager.AppSettings["DisableHelpSystem"].ToLower() == "true")
                if (string.Equals(ConfigurationManager.AppSettings["DisableHelpSystem"], "true", StringComparison.InvariantCultureIgnoreCase))
                {
                    render = false;
                }
            }

            if (render)
            {
                //SetupCss();
                RegisterScripts();
                Initialize();
            }

            base.OnPreRender(e);

        }

        private void Initialize()
        {
            if (this.ImageUrl != string.Empty)
            {
                //lnkHelp.ImageUrl = this.ImageUrl;
                litHelpLink.Text = "<a href='"
                    + GetNavigationUrl() + "' rel='"
                    + GetIBoxLink() + "'><img src='"
                    + Page.ResolveUrl(this.ImageUrl) + "' alt='"
                    + this.Text + "' /></a>";
               
            }
            else
            {
                litHelpLink.Text = "<a href='"
                    + GetNavigationUrl() + "' rel='"
                    + GetIBoxLink() + "'>" + this.Text + "</a>";
            }

        }

        protected override void CreateChildControls()
        {
            litHelpLink = new Literal();
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                //writer.Write("[" + this.ID + "]");
            }
            else
            {
                this.Controls.Add(litHelpLink);
            }
            
        }


        private string GetNavigationUrl()
        {
            string url = (this.NavigateUrl != string.Empty) ? ResolveUrl(NavigateUrl) : ResolveUrl("~/Help.aspx");
            url += (url.IndexOf("?") > -1) ? "&" : "?";
            url += "helpkey=" + this.HelpKey.ToLower(CultureInfo.InvariantCulture);
            return url;
        }

        private string GetIBoxLink()
        {
            string link = "ibox";
            if (this.ModalHeight > 0)
                link += "&amp;height=" + ModalHeight.ToString(CultureInfo.InvariantCulture);
            if (this.ModalWidth > 0)
                link += "&amp;width=" + ModalWidth.ToString(CultureInfo.InvariantCulture);

            return link;
        }

        
        private void SetupCss()
        {
            if (this.Site != null && this.Site.DesignMode)
            {

            }
            else
            {
                if (Page.Master != null)
                {
                    Control head = Page.Master.FindControl("Head1");
                    if (head != null)
                    {
                        try
                        {
                            // need to make sure we only load the css 1 time
                            if (head.FindControl("iboxcss") == null)
                            {
                                Literal cssLink = new Literal();
                                cssLink.ID = "iboxcss";
                                if (!CssBaseUrl.EndsWith("/"))
                                {
                                    CssBaseUrl += "/";
                                }

                                //if (CssBaseUrl.StartsWith("~"))
                                //{
                                cssLink.Text = "<style type=\"text/css\">@import url("
                                    + Page.ResolveUrl(CssBaseUrl + "ibox.css") + ");</style>\n";
                                //}
                                //else
                                //{
                                //    cssLink.Text = "<style type=\"text/css\">@import url("
                                //        + Page.ResolveUrl(CssBaseUrl + "ibox.css") + ");</style>\n";
                                //}

                                head.Controls.Add(cssLink);
                            }
                        }
                        catch (HttpException) { }

                    }

                }
            }

        }

        private void RegisterScripts()
        {

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "iboxscript", "<script src=\""
                + Page.ResolveUrl(ScriptDirectory + "/ibox.js.aspx") + "\" type=\"text/javascript\" ></script>");
        }


        public static void AddHelpLink(
            Panel parentControl,
            string helpkey,
            string siteNavigationRoot,
            string cssBaseUrl)
        {
            Literal litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

            HelpLinkButton helpLinkButton = new HelpLinkButton();
            helpLinkButton.ImageUrl = "~/Data/SiteImages/question.png";
            helpLinkButton.HelpKey = helpkey;
            if ((siteNavigationRoot != null) && (siteNavigationRoot.Length > 0))
            {
                helpLinkButton.NavigateUrl = siteNavigationRoot + "/Help.aspx";
            }
            helpLinkButton.CssBaseUrl = cssBaseUrl;

            parentControl.Controls.Add(helpLinkButton);

            litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

        }

    }
}
