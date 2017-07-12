/// Author:					
/// Created:				2010-03-21
/// Last Modified:			2010-06-15
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.		

using System.Web.UI.WebControls;
using log4net;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// update 2011-03-17 this menu is now obsolete and only kept for backward comaptibility
    /// the functionality provided by this control can be replicated using mojoTreeView and setting configuration from theme.skin
    /// 
    /// </summary>
    public class ArtisteerTreeView : mojoTreeView
    {
        

        private bool renderNavigationHeader = false;

        new public bool RenderNavigationHeader
        {
            get { return renderNavigationHeader; }
            set { renderNavigationHeader = value; }
        }

        private string navigationHeaderText = string.Empty;
        new public string NavigationHeaderText
        {
            get { return navigationHeaderText; }
            set { navigationHeaderText = value; }
        }

    }
}
