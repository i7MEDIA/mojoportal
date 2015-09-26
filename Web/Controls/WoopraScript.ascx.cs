using System;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;


namespace mojoPortal.Web.UI
{
    public partial class WoopraScript : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EnableViewState = false;
            if (!WoopraIsEnabled())
            {
                this.Visible = false;
                return;
            }

        }

        private bool WoopraIsEnabled()
        {
            if (WebConfigSettings.EnableWoopraGlobally) { return true; }
            if (WebConfigSettings.DisableWoopraGlobally) { return false; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return false; }

            return siteSettings.EnableWoopra;

        }

        
    }
}