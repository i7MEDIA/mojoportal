///		Author:				
///		Created:			2006-12-14
///		Last Modified:		2007-11-07

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    ///     This control is designed to be added to edit pages to keep
    ///     the session alive while editing. It calls ~/Services/SessionKeepAlive.aspx
    ///     before the session expires
    ///     
    /// </summary>
    public class SessionKeepAliveControl : WebControl
    {
        #region Constructors

        public SessionKeepAliveControl()
		{
			EnsureChildControls();

            
		}

		#endregion


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);
        }

        private void DoRender(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("iframe");
            writer.WriteAttribute("src", SiteUtils.GetNavigationSiteRoot() + "/Services/SessionKeepAlive.aspx");
            writer.WriteAttribute("frameborder", "0");
            writer.WriteAttribute("width", "0");
            writer.WriteAttribute("height", "0");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteEndTag("iframe");

        }

        protected override void OnPreRender(EventArgs e)
        {

            
            Initialize();
            base.OnPreRender(e);

        }

        private void Initialize()
        {
            

        }

        protected override void CreateChildControls()
        {
            


        }
    }
}
