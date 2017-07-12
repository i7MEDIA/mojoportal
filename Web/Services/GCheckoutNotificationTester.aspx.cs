/// Author:					
/// Created:				2008-03-25
/// Last Modified:			2008-06-25
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class GCheckoutNotificationTesterPage : mojoBasePage
    {
        private CommerceConfiguration commerceConfig;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!WebUser.IsAdmin)||(!siteSettings.IsServerAdminSite))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;

            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (txtNotificationHandlerUrl.Text.Length == 0)
                txtNotificationHandlerUrl.Text
                    = SecureSiteRoot
                    + "/Services/GCheckoutNotificationHandler.ashx";

        }

        void btnPost_Click(object sender, EventArgs e)
        {
            if (commerceConfig == null) return;

            GNotificationTester tester = new GNotificationTester(
                commerceConfig.GoogleMerchantID,
                commerceConfig.GoogleMerchantKey,
                txtNotificationHandlerUrl.Text,
                txtXmlInput.Text,
                commerceConfig.DefaultTimeoutInMilliseconds);

            txtReponse.Text = tester.Test();



        }


        private void PopulateLabels()
        {
            btnPost.Text = Resource.GCheckoutTesterPostButton;
            txtReponse.Text = string.Empty;

        }

        private void LoadSettings()
        {
            commerceConfig = SiteUtils.GetCommerceConfig();


        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnPost.Click += new EventHandler(btnPost_Click);


        }



        #endregion

    }
}
