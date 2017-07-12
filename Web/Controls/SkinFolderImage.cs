/// Author:				    
/// Created:			    2008-04-23
///	Last Modified:		    2010-06-04
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using mojoPortal.Web;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// automatically resolves the base url for the current skin so you can just specify the ImageFileName for an image that exists in the skin folder.
    /// </summary>
    public class SkinFolderImage : Image
    {

        private string imageFileName = string.Empty;

        public string ImageFileName
        {
            get { return imageFileName; }
            set { imageFileName = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.imageFileName.Length == 0)
            {
                this.Visible = false;
                return;
            }

            this.ImageUrl = SiteUtils.DetermineSkinBaseUrl(true, WebConfigSettings.UseFullUrlsForSkins, Page) + this.imageFileName;

        }

    }
}
