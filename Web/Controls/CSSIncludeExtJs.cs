///	Created:			    2007-10-29
///	Last Modified:		    2007-11-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    public class CssIncludeExtJS : WebControl
    {
        private string extJsBasePath = "~/ClientScript/ext-2.0-beta1/";

        public string ExtJSBasePath
        {
            get { return extJsBasePath; }
            set { extJsBasePath = value; }
        }

        private string extJsDefaultTheme = string.Empty;

        public string ExtJSDefaultTheme
        {
            get { return extJsDefaultTheme; }
            set { extJsDefaultTheme = value; }
        }

        private bool includeSkinOverride = true;

        public bool IncludeSkinOverride
        {
            get { return includeSkinOverride; }
            set { includeSkinOverride = value; }
        }


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (ConfigurationManager.AppSettings["ExtJsBasePath"] != null)
            {
                extJsBasePath = ConfigurationManager.AppSettings["ExtJsBasePath"] ;
            }

            if (extJsDefaultTheme.Length == 0)
            {
                if (ConfigurationManager.AppSettings["ExtJsDefaultTheme"] != null)
                {
                    extJsDefaultTheme = ConfigurationManager.AppSettings["ExtJsDefaultTheme"];
                }
            }

            

            SetupCss();
            this.Visible = false;
        }

        private void SetupCss()
        {
            if (Page.Master != null)
            {
                Control head = Page.Master.FindControl("Head1");
                LoadCss(head);

            }
            else
            {
                Control head = Page.FindControl("Head1");
                LoadCss(head);
            }


        }

        private void LoadCss(Control head)
        {
            if (head != null)
            {
                try
                {
                    if (head.FindControl("ext-all") == null)
                    {
                        Literal cssLink = new Literal();
                        cssLink.ID = "ext-all";
                        cssLink.Text = "<style type=\"text/css\">@import url('"
                            + Page.ResolveUrl(extJsBasePath + "resources/css/ext-all.css")
                             + "');</style>\n";

                        head.Controls.Add(cssLink);


                    }

                    if ((extJsDefaultTheme.Length > 0) && (head.FindControl("ext-theme") == null))
                    {
                        Literal cssLink = new Literal();
                        cssLink.ID = "ext-theme";
                        cssLink.Text = "<style type=\"text/css\">@import url('"
                            + Page.ResolveUrl(ExtJSBasePath + extJsDefaultTheme)
                             + "');</style>\n";

                        head.Controls.Add(cssLink);


                    }

                    if ((IncludeSkinOverride) && (head.FindControl("ext-override") == null))
                    {
                        Literal cssLink = new Literal();
                        cssLink.ID = "ext-override";
                        cssLink.Text = "<style type=\"text/css\">@import url('"
                            + SiteUtils.GetSkinBaseUrl(Page) + "style-ext-override.css"
                             + "');</style>\n";

                        head.Controls.Add(cssLink);


                    }
                }
                catch (HttpException) { }


            }


        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                // don't render anything
            }

        }
    }
}
