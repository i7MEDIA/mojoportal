//	Created:			    2010-03-19
//	Last Modified:		    2013-01-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class SiteSlogan : WebControl
    {
        private SiteSettings siteSettings = null;

        private string topMarkup = string.Empty;

        public string TopMarkup
        {
            get { return topMarkup; }
            set { topMarkup = value; }
        }

        private string bottomMarkup = string.Empty;

        public string BottomMarkup
        {
            get { return bottomMarkup; }
            set { bottomMarkup = value; }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnableViewState = false; //not needed
           
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null) { return; }
            if (siteSettings.Slogan.Length == 0) { return; }

            if (topMarkup.Length > 0)
            {
                writer.Write(topMarkup);
            }

            writer.Write(HttpUtility.HtmlAttributeEncode(siteSettings.Slogan));
            //base.Render(writer);

            if (bottomMarkup.Length > 0)
            {
                writer.Write(bottomMarkup);
            }
        }

    }
}
