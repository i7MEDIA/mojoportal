using System;
using System.Linq;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;

namespace mojoPortal.Web.AdminUI;

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
			model.Links.AddRange(
			[
				new() {
					ResourceFile = "Resource",
					ResourceKey = "LanguageAdministrationLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminLanguage.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata-language",
					IconCssClass = "fa fa-language",
					SortOrder = 10
				},
				new() {
					ResourceFile = "Resource",
					ResourceKey = "CurrencyAdministrationLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminCurrency.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata-currency",
					IconCssClass = "fa fa-usd",
					SortOrder = 10
				},
				new() {
					ResourceFile = "Resource",
					ResourceKey = "CountryAdministrationLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminCountry.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata-country",
					IconCssClass = "fa fa-flag",
					SortOrder = 15
				},
				new() {
					ResourceFile = "Resource",
					ResourceKey = "GeoZoneAdministrationLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminGeoZone.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata-geozone",
					IconCssClass = "fa fa-globe",
					SortOrder = 20
				},
			]);
		}

		if (isAdmin || isContentAdmin)
		{
			model.Links.AddRange(
			[
				new() {
					ResourceFile = "Resource",
					ResourceKey = "TaxClassAdminLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminTaxClass.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata-taxclass",
					IconCssClass = "fa fa-balance-scale",
					SortOrder = 25
				},
				new() {
					ResourceFile = "Resource",
					ResourceKey = "TaxRateAdminLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminTaxRate.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata-taxrate",
					IconCssClass = "fa fa-percent",
					SortOrder = 30
				},
			]);
		}

		//Supplemental Links
		model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "coredata").ToList());

		//BreadCrumbs

		model.BreadCrumbs = new Models.BreadCrumbs
		{
			CrumbArea = Models.BreadCrumbArea.Admin,
			Crumbs =
			{
				new () {
					Text = Resource.AdminMenuLink,
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminMenu.aspx".ToLinkBuilder().ToString(),
					SortOrder = -1,
					SystemName = "AdminMenu",
					Parent = "root"
				},
				new () {
					Text = Resource.CoreDataAdministrationLink,
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/CoreData.aspx".ToLinkBuilder().ToString(),
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
			log.Error($"layout ({partialName}) was not found in skin {SiteUtils.DetermineSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
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