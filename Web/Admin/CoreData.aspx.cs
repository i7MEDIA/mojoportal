/// Author:					
/// Created:				2008-06-22
/// Last Modified:			2018-11-12
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;

namespace mojoPortal.Web.AdminUI
{

	public partial class CoreDataPage : NonCmsBasePage
    {
		private static readonly ILog log = LogManager.GetLogger(typeof(CoreDataPage));

		private Models.AdminMenuPage model;
		private ContentAdminLinksConfiguration supplementalLinks;
		private bool isAdmin = false;
        private bool isContentAdmin = false;
		private const string partialName = "_AdminMenu";
		protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
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
			supplementalLinks = ContentAdminLinksConfiguration.GetConfig(siteSettings.SiteId);
			PopulateModel();

			PopulateControls();

        }

		private void PopulateModel()
		{
			model = new Models.AdminMenuPage
			{
				PageTitle = Resource.CoreDataAdministrationHeading,
				PageHeading = Resource.CoreDataAdministrationHeading
			};
			if (siteSettings.IsServerAdminSite)
			{
				model.Links.AddRange(new List<ContentAdminLink>
				{
					// don't have multi-language tools yet so this is not needed
					//new ContentAdminLink
					//{
					//	ResourceFile = "Resource",
					//	ResourceKey = "LanguageAdministrationLink",
					//	Url = SiteRoot + "/Admin/AdminLanguage.aspx",
					//	CssClass = "adminlink-coredata-language",
					//	IconCssClass = "fa fa-language",
					//	SortOrder = 10
					//},

					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "CurrencyAdministrationLink",
						Url = SiteRoot + "/Admin/AdminCurrency.aspx",
						CssClass = "adminlink-coredata-currency",
						IconCssClass = "fa fa-usd",
						SortOrder = 10
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "CountryAdministrationLink",
						Url = SiteRoot + "/Admin/AdminCountry.aspx",
						CssClass = "adminlink-coredata-country",
						IconCssClass = "fa fa-flag",
						SortOrder = 15
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "GeoZoneAdministrationLink",
						Url = SiteRoot + "/Admin/AdminGeoZone.aspx",
						CssClass = "adminlink-coredata-geozone",
						IconCssClass = "fa fa-globe",
						SortOrder = 20
					},
				});
			}

			if (isAdmin || isContentAdmin)
			{
				model.Links.AddRange(new List<ContentAdminLink>
				{
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "TaxClassAdminLink",
						Url = SiteRoot + "/Admin/AdminTaxClass.aspx",
						CssClass = "adminlink-coredata-taxclass",
						IconCssClass = "fa fa-balance-scale",
						SortOrder = 25
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "TaxRateAdminLink",
						Url = SiteRoot + "/Admin/AdminTaxRate.aspx",
						CssClass = "adminlink-coredata-taxrate",
						IconCssClass = "fa fa-percent",
						SortOrder = 30
					},
				});
			}

			//Supplemental Links
			model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "coredata").ToList());

			//BreadCrumbs

			model.BreadCrumbs = new Models.BreadCrumbs
			{
				CrumbArea = Models.BreadCrumbArea.Admin,
				Crumbs =
				{
					new BreadCrumb
					{
						Text = Resource.AdminMenuLink,
						Url = SiteRoot + "/Admin/AdminMenu.aspx",
						SortOrder = -1,
						SystemName = "AdminMenu",
						Parent = "root"
					},
					new BreadCrumb
					{
						Text = Resource.CoreDataAdministrationLink,
						Url = SiteRoot + "/Admin/CoreData.aspx",
						IsCurrent = true,
						SortOrder = 0,
						SystemName = "CoreData",
						Parent = "AdminMenu"
					}
				}
			};

			//Sort the whole thing (allows mixing Supplemental Links with system links instead of them always being at the bottom)
			model.Links.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
		}
		private void PopulateControls()
        {
			try
			{
				litMenu.Text = RazorBridge.RenderPartialToString(partialName, model, "Admin");
			}
			catch (System.Web.HttpException ex)
			{
				log.Error($"layout ({partialName}) was not found in skin {SiteUtils.GetSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
			}
		}


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.CoreDataAdministrationHeading);
        }

        private void LoadSettings()
        {
            isAdmin = WebUser.IsAdmin;
            isContentAdmin = WebUser.IsContentAdmin;

            AddClassToBody("administration admin-menu-coredata admin-coredata");
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