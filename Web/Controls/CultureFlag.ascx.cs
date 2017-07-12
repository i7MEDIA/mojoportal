/// Author:				
/// Created:			7/19/2006
///	Last Modified:		7/19/2006
///	
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Globalization;
using System.IO;

namespace mojoPortal.Web.UI
{
    
    public partial class CultureFlag : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String flagImagePath = "~/Data/SiteImages/flags/" 
                + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName
                + ".gif";

            if (File.Exists(Server.MapPath(flagImagePath)))
            {
                imgFlag.ImageUrl = Page.ResolveUrl(flagImagePath);
            }
            else
            {
                imgFlag.Visible = false;
            }




        }
    }
}