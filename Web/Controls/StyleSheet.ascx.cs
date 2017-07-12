///		Author:			
///		Created:		2004-08-24
///		Last Modified:	2008-10-31
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// deprecated, use StyleSheetCombiner
    /// </summary>
	public partial class StyleSheet : UserControl
	{
        // the text of this literal is set in mojoBasePage
        // and in Default.aspx.cs
        // additional feature style sheets are also added externally

        public Literal LiteralStyleSheetLink
        {
            get { return litStyleSheet; }
        }
            

		protected void Page_Load(object sender, System.EventArgs e)
		{
            
		}

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                writer.Write("<link href='" + Page.ResolveUrl("~/Data/Sites/1/styleshout-techmania/style.css") 
                    + "' type='text/css' rel='stylesheet' />");
            }
            else
            {
                base.RenderControl(writer);

            }
        }

        

	}
}
