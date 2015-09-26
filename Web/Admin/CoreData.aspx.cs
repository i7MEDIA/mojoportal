/// Author:					Joe Audette
/// Created:				2008-06-22
/// Last Modified:			2011-03-21
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
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class CoreDataPage : NonCmsBasePage
    {
        private bool isAdmin = false;
        private bool isContentAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();

            if ((!isAdmin) && (!isContentAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (!siteSettings.IsServerAdminSite)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;

            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.CoreDataAdministrationHeading);

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkCurrentPage.Text = Resource.CoreDataAdministrationLink;
            lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

            heading.Text = Resource.CoreDataAdministrationHeading;

            liLanguageAdmin.Visible = siteSettings.IsServerAdminSite;
            lnkLanguageAdmin.Text = Resource.LanguageAdministrationLink;
            lnkLanguageAdmin.NavigateUrl = SiteRoot + "/Admin/AdminLanguage.aspx";
            // we're not supporting multiple language so this feature isn't useful, don't show it.
            liLanguageAdmin.Visible = false;

            liCurrencyAdmin.Visible = siteSettings.IsServerAdminSite;
            lnkCurrencyAdmin.Text = Resource.CurrencyAdministrationLink;
            lnkCurrencyAdmin.NavigateUrl = SiteRoot + "/Admin/AdminCurrency.aspx";

            liCountryAdmin.Visible = siteSettings.IsServerAdminSite;
            lnkCountryAdmin.Text = Resource.CountryAdministrationLink;
            lnkCountryAdmin.NavigateUrl = SiteRoot + "/Admin/AdminCountry.aspx";

            liGeoZoneAdmin.Visible = siteSettings.IsServerAdminSite;
            lnkGeoZone.Text = Resource.GeoZoneAdministrationLink;
            lnkGeoZone.NavigateUrl = SiteRoot + "/Admin/AdminGeoZone.aspx";

            liTaxClassAdmin.Visible = (isAdmin || isContentAdmin);
            lnkTaxClassAdmin.Text = Resource.TaxClassAdminLink;
            lnkTaxClassAdmin.NavigateUrl = SiteRoot + "/Admin/AdminTaxClass.aspx";

            liTaxRateAdmin.Visible = (isAdmin || isContentAdmin);
            lnkTaxRateAdmin.Text = Resource.TaxRateAdminLink;
            lnkTaxRateAdmin.NavigateUrl = SiteRoot + "/Admin/AdminTaxRate.aspx";

            

        }

        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;

            AddClassToBody("administration");
            AddClassToBody("coredata");
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