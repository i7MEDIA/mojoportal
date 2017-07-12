// Author:					
// Created:					2011-10-07
// Last Modified:			2011-12-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using Resources;

namespace mojoPortal.Web.UI.Pages
{

    public partial class ClosedPage : NonCmsBasePage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!siteSettings.SiteIsClosed)
            {
                SiteUtils.RedirectToDefault();
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            litSiteClosedMessage.Text = siteSettings.SiteIsClosedMessage;

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SiteClosedPageTitle);
            
        }

        private void LoadSettings()
        {

            AddClassToBody("closedpage");

        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            if (WebConfigSettings.HideAllMenusOnSiteClosedPage)
            {
                SuppressAllMenus();
            }


        }

        #endregion
    }
}
