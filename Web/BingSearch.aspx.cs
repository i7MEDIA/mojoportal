// Author:					
// Created:					2010-06-08
// Last Modified:			2010-06-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.UI
{

    public partial class BingSearchPage : NonCmsBasePage
	{
        string bingApiId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (string.IsNullOrEmpty(bingApiId))
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/SearchResults.aspx");
                return;

            }

            PopulateLabels();
            
        }

        private void PopulateLabels()
        {
            if (siteSettings == null) return;

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SearchPageTitle);

            heading.Text = Resource.SearchPageTitle;
        }

        private void LoadSettings()
        {
            bingApiId = SiteUtils.GetBingApiId();

            AddClassToBody("bingsearch");

        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        #endregion
    }
}