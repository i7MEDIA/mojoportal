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
                SiteUtils.RedirectToSiteRoot();
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
