//	Author:					
//	Created:				2011-07-10
//	Last Modified:		    2011-07-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.		

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// you could just add the apple touch link directly in layout.master
    /// but this control makes it easier to have different icons per skin
    /// whereas if you just use the one(s) in the root of the installation then all the sites in the installation would have the same icon
    /// so this control could be used in layout.master in the head element and you put the .png file in your skin folder and set the fileName
    /// to the name of the .png file
    /// by doing this each site could have different icons
    /// 
    /// http://developer.apple.com/library/safari/#documentation/appleapplications/reference/safariwebcontent/ConfiguringWebApplications/ConfiguringWebApplications.html
    /// http://stackoverflow.com/questions/2997437/what-size-should-apple-touch-icon-png-be-for-ipad-and-iphone-4
    /// 
    /// </summary>
    public class AppleTouchIcon : WebControl
    {
        private string fileName = string.Empty;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string sizes = string.Empty;

        /// <summary>
        /// valid options are leave it blank, 72x72, 114x114
        /// </summary>
        public string Sizes
        {
            get { return sizes; }
            set { sizes = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (fileName.Length == 0) { return; }

            if (sizes.Length > 0)
            {
                writer.Write("\n<link rel='apple-touch-icon' sizes=\"{0}\" href='{1}{2}' />",
                   sizes,
                  SiteUtils.DetermineSkinBaseUrl(true, false, Page),
                  fileName
                  );
            }
            else
            {
                writer.Write("\n<link rel='apple-touch-icon' href='{0}{1}' />",
                   SiteUtils.DetermineSkinBaseUrl(true, false, Page),
                   fileName
                   );
               
            }
            
           


              
        }
    }
}