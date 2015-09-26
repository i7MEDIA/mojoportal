using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    ///	Created:			    2006/04/20
    ///	Last Modified:		    2007/04/20
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.	
    /// 
    /// The purpose of this control is just for displaying a separator between
    /// links in the mojoportal skins either as text or image, with option to
    /// hide when authenticated or hide when anonymous.
    /// 
    /// </summary>
    public class SeparatorControl : WebControl
    {
        private bool visibleWhenAnonymous = true;
        public bool VisibleWhenAnonymous
        {
            get { return visibleWhenAnonymous; }
            set { visibleWhenAnonymous = value; }
        }

        private bool visibleWhenAuthenticated = true;
        public bool VisibleWhenAuthenticated
        {
            get { return visibleWhenAuthenticated; }
            set { visibleWhenAuthenticated = value; }
        }

        private string separatorImageUrl = string.Empty;
        public string SeparatorImageUrl
        {
            get { return separatorImageUrl; }
            set { separatorImageUrl = value; }
        }

        private string separatorText = "|";
        public string SeparatorText
        {
            get { return separatorText; }
            set { separatorText = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (
                ((!Page.Request.IsAuthenticated)&&(visibleWhenAnonymous))
                || ((Page.Request.IsAuthenticated) && (visibleWhenAuthenticated))
                )
            {
                if (CssClass.Length == 0) CssClass = "accent";

                if (separatorImageUrl.Length > 0)
                {
                    writer.Write("<img class='" + CssClass + "' src='" + Page.ResolveUrl(separatorImageUrl) + "' border='0' />");
                }
                else
                {
                    writer.Write("<span class='" + CssClass + "'>" + separatorText + "</span>");
                }
                
            }
        }

    }
}
