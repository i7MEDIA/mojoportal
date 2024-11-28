using System;
using System.Linq;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class AdvnacedToolsPage : NonCmsBasePage
{
	private bool isContentAdmin = false;
	private bool isAdmin = false;
	private bool isSiteEditor = false;
	private static readonly ILog log = LogManager.GetLogger(typeof(AdvnacedToolsPage));

	private Models.AdminMenuPage model;
	private ContentAdminLinksConfiguration supplementalLinks;
	private const string partialName = "_AdminMenu";
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		LoadSettings();

		if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor))
		{
			WebUtils.SetupRedirect(this, "AccessDenied.aspx".ToLinkBuilder().ToString());
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
			PageTitle = Resource.AdvancedToolsHeading,
			PageHeading = Resource.AdvancedToolsHeading
		};

		if (isAdmin || isContentAdmin || isSiteEditor)
		{
			model.Links.AddRange(
			[
				new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuUrlManagerLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/UrlManager.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-urls",
					IconCssClass = "fa fa-link",
					SortOrder = 10
				},
				new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "RedirectManagerLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/RedirectManager.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-redirects",
					IconCssClass = "fa fa-reply",
					SortOrder = 15
				},
				new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminIndexBrowser",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/IndexBrowser.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-indexbrowser",
					IconCssClass = "fa fa-search",
					SortOrder = 18
				},
			]);
		}

		if (isAdmin && siteSettings.IsServerAdminSite)
		{
			model.Links.AddRange(
			[
				new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuBannedIPAddressesLabel",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/BannedIPAddresses.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-bannedip",
					IconCssClass = "fa fa-ban",
					SortOrder = 20
				},
				new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuFeatureModulesLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/ModuleAdmin.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-modules",
					IconCssClass = "fa fa-star",
					SortOrder = 25
				},
			]);

			if ((!WebConfigSettings.DisableTaskQueue) && (isAdmin || WebUser.IsNewsletterAdmin))
			{
				model.Links.Add(new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "TaskQueueMonitorHeading",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/TaskQueueMonitor.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-queue",
					IconCssClass = "fa fa-clock-o",
					SortOrder = 30
				});
			}

			if (WebConfigSettings.EnableDeveloperMenuInAdminMenu)
			{
				model.Links.Add(new ()
				{
					ResourceFile = "DevTools",
					ResourceKey = "DevToolsHeading",
					Url = $"DevAdmin/Default.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-advanced-devtools",
					IconCssClass = "fa fa-wrench",
					SortOrder = 35
				});
			}
		}

		//Supplemental Links
		model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "advancedtools").ToList());

		//BreadCrumbs

		model.BreadCrumbs = new Models.BreadCrumbs
		{
			CrumbArea = Models.BreadCrumbArea.Admin,
			Crumbs =
			{
				new ()
				{
					Text = Resource.AdminMenuLink,
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminMenu.aspx".ToLinkBuilder().ToString(),
					SortOrder = -1,
					SystemName = "AdminMenu",
					Parent = "root"
				},
				new ()
				{
					Text = Resource.AdvancedToolsLink,
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdvancedTools.aspx".ToLinkBuilder().ToString(),
					IsCurrent = true,
					SortOrder = 0,
					SystemName = "AdvancedTools",
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
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdvancedToolsHeading);
	}

	private void LoadSettings()
	{
		isAdmin = WebUser.IsAdmin;
		isContentAdmin = WebUser.IsContentAdmin;
		isSiteEditor = SiteUtils.UserIsSiteEditor();

		AddClassToBody("administration admin-menu-advanced admin-advanced");
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
