/// Author:					
/// Created:				2007-11-05
/// Last Modified:			2009-05-14
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using DotNetOpenAuth.OpenId.RelyingParty;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.Web.UI
{

    public partial class UpdateOpenIdPage : NonCmsBasePage
    {
        private SiteUser siteUser;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (!(WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth))
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            LoadSettings();

            siteUser = SiteUtils.GetCurrentSiteUser();

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (siteUser == null) return;
            if (Page.IsPostBack) return;

            OpenIdLogin1.Text = siteUser.OpenIdUri;
          


        }


        private void PopulateLabels()
        {
            OpenIdLogin1.ButtonText = Resource.OpenIDUpdateButton;
            OpenIdLogin1.ButtonToolTip = Resource.OpenIDUpdateButton;
            OpenIdLogin1.ExamplePrefix = Resource.OpenIDExamplePrefix;
            OpenIdLogin1.ExampleUrl = Resource.OpenIDExampleUrl;

            lblLoginFailed.Text = Resource.OpenIDUpdateFailedMessage;
            lblLoginCanceled.Text = Resource.OpenIDUpdateCancelledMessage;

        }

        private void LoadSettings()
        {
            AddClassToBody("updateopenid");

        }

        protected void OpenIdLogin1_LoggedIn(object sender, OpenIdEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            // this prevents another login from happening
            e.Cancel = true;

            if (siteUser != null)
            {
                string oiduri = e.ClaimedIdentifier.ToString();
                Guid foundUserGuid
                    = SiteUser.GetUserGuidFromOpenId(
                    siteSettings.SiteId, oiduri);

                if (
                    (foundUserGuid == Guid.Empty)
                    || (foundUserGuid == siteUser.UserGuid)
                    )
                {
                    siteUser.OpenIdUri = e.ClaimedIdentifier.ToString();
                    siteUser.Save();
                }

            }
            WebUtils.SetupRedirect(this, "UserProfile.aspx");

        }

        void OpenIdLogin1_Failed(object sender, OpenIdEventArgs e)
        {
            lblLoginFailed.Visible = true;
        }

        
        protected void OpenIdLogin1_Canceled(object sender, OpenIdEventArgs e)
        {
            lblLoginCanceled.Visible = true;
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.OpenIdLogin1.LoggedIn += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_LoggedIn);
            //this.OpenIdLogin1.Error += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Error);
            OpenIdLogin1.Failed += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Failed);
            this.OpenIdLogin1.Canceled += new EventHandler<OpenIdEventArgs>(OpenIdLogin1_Canceled);

            SuppressMenuSelection();
            SuppressPageMenu();


        }

        

        #endregion
    }
}
