/// Author:				    
/// Created:			    2006-10-01
///	Last Modified:		    2007-07-08
/// 
/// 03/13/2007   Alexander Yushchenko: moved all the control logic to Render() to simplify it.
/// 13/04/2007   Alexander Yushchenko: made it WebControl instead of UserControl.
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
using Resources;

namespace mojoPortal.Web.UI
{
    public class SkipLink : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
               
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                writer.Write("<a href='#startcontent' class='skiplink'>{0}</a>", Resource.SkipLink);
            }
        }
    }
}
