using System;
using System.Linq;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class AdminMenuPage : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(AdminMenuPage));

	private bool IsAdminOrContentAdmin = false;
	private bool IsAdmin = false;
	private bool isSiteEditor = false;
	private bool isCommerceReportViewer = false;
	private CommerceConfiguration commerceConfig = null;
	SecurityAdvisor securityAdvisor = new SecurityAdvisor();
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

		if (!WebUser.IsAdminOrContentAdminOrRoleAdminOrNewsletterAdmin && !isSiteEditor && !isCommerceReportViewer)
		{
			WebUtils.SetupRedirect(this, "AccessDenied.aspx".ToLinkBuilder().ToString());

			return;
		}

		SecurityHelper.DisableBrowserCache();

		PopulateLabels();
		PopulateModel();
		PopulateControls();
	}


	private void PopulateModel()
	{
		model = new Models.AdminMenuPage
		{
			PageTitle = Resource.AdminMenuHeading,
			PageHeading = Resource.AdminMenuHeading
		};

		//Site Settings
		if (IsAdminOrContentAdmin || isSiteEditor)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuSiteSettingsLink",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/SiteSettings.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-sitesettings",
				IconCssClass = "fa fa-cog",
				SortOrder = -1
			});
		}

		//Site List
		if (WebConfigSettings.AllowMultipleSites && siteSettings.IsServerAdminSite && IsAdmin)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "SiteList",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/SiteList.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-sitelist",
				IconCssClass = "fa fa-list",
				SortOrder = 10
			});
		}

		//Security Advisor
		if (IsAdmin && siteSettings.IsServerAdminSite)
		{
			var needsAttention = !securityAdvisor.UsingCustomMachineKey() || securityAdvisor.DefaultAdmin().userExists;

			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = needsAttention ? "SecurityAdvisorNeedsAttention" : "SecurityAdvisor",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/SecurityAdvisor.aspx".ToLinkBuilder().ToString(),
				CssClass = needsAttention ? "adminlink-securityadvisor text-danger" : "adminlink-securityadvisor",
				IconCssClass = needsAttention ? "fa fa-shield text-danger" : "fa fa-shield",
				SortOrder = 15
			});
		}

		//Role Admin
		if (WebUser.IsRoleAdmin || IsAdmin)
		{
			var addLink = true;

			if (WebConfigSettings.UseRelatedSiteMode && WebConfigSettings.RelatedSiteModeHideRoleManagerInChildSites && siteSettings.SiteId != WebConfigSettings.RelatedSiteID)
			{
				addLink = false;
			}

			if (addLink)
			{
				model.Links.Add(new()
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuRoleAdminLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/RoleManager.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-rolemanager",
					IconCssClass = "fa fa-users",
					SortOrder = 20
				});
			}
		}

		//Site Permissions
		if (IsAdmin)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "SiteSettingsPermissionsTab",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/PermissionsMenu.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-sitepermissions",
				IconCssClass = "fa fa-key",
				SortOrder = 25
			});
		}

		//Member List
		if (WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList) || WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers))
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "MemberListLink",
				Url = WebConfigSettings.MemberListUrl.ToLinkBuilder().ToString(),
				CssClass = "adminlink-memberlist",
				IconCssClass = "fa fa-address-book-o",
				SortOrder = 30
			});
		}

		//Add User
		if (WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers + siteSettings.RolesThatCanManageUsers + siteSettings.RolesThatCanLookupUsers))
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "MemberListAddUserLabel",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/ManageUsers.aspx".ToLinkBuilder().AddParam("userId", -1).ToString(),
				CssClass = "adminlink-adduser",
				IconCssClass = "fa fa-user-plus",
				SortOrder = 35
			});
		}

		//Page Manager
		if (IsAdminOrContentAdmin || isSiteEditor || SiteMapHelper.UserHasAnyCreatePagePermissions(siteSettings))
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuPageTreeLink",
				Url = WebConfigSettings.PageTreeRelativeUrl.ToLinkBuilder().ToString(),
				CssClass = "adminlink-pagemanager",
				IconCssClass = "fa fa-sitemap",
				SortOrder = 40
			});
		}

		//Content Manager
		if (IsAdminOrContentAdmin || isSiteEditor)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuContentManagerLink",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/ContentCatalog.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-contentmanager",
				IconCssClass = "fa fa-hand-pointer-o",
				SortOrder = 45
			});
		}

		//Content Workflow
		if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuContentWorkflowLabel",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/ContentWorkflow.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-contentworkflow",
				IconCssClass = "fa fa-code-fork",
				SortOrder = 50
			});
		}

		//Content Templates/Styles
		if (IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates))
		{
			model.Links.AddRange([
				new () {
					ResourceFile = "Resource",
					ResourceKey = "ContentTemplates",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/ContentTemplates.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-contenttemplates",
					IconCssClass = "fa fa-object-group",
					SortOrder = 55
				},
				new () {
					ResourceFile = "Resource",
					ResourceKey = "ContentStyleTemplates",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/ContentStyles.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-contentstyles",
					IconCssClass = "fa fa-code",
					SortOrder = 60
				}
			]);
		}

		//Design Tools
		if (IsAdmin || WebUser.IsContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))
		{
			model.Links.Add(new()
			{
				ResourceFile = "DevTools",
				ResourceKey = "DesignTools",
				Url = "DesignTools/Default.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-designtools",
				IconCssClass = "fa fa-paint-brush",
				SortOrder = 65
			});
		}

		//File Manager
		//using the FileManagerLink to check if the link should render because it has a lot of criteria, best not to duplicate it
		var fileManagerLinkTest = new FileManagerLink();
		if (fileManagerLinkTest.ShouldRender())
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuFileManagerLink",
				Url = "FileManager".ToLinkBuilder().AddParam("view", "fullpage").ToString(),
				CssClass = "adminlink-filemanager",
				IconCssClass = "fa fa-folder-open",
				SortOrder = 70
			});
		}

		//Newsletter
		if (WebConfigSettings.EnableNewsletter && (IsAdmin || isSiteEditor || WebUser.IsNewsletterAdmin))
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuNewsletterAdminLabel",
				Url = "eletter/Admin.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-newsletter",
				IconCssClass = "fa fa-newspaper-o",
				SortOrder = 75
			});
		}

		//Commerce
		if (isCommerceReportViewer && commerceConfig != null && commerceConfig.IsConfigured)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "CommerceReportsLink",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/SalesSummary.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-commercereports",
				IconCssClass = "fa fa-shopping-basket",
				SortOrder = 80
			});
		}

		//Registration Agreement
		if (IsAdminOrContentAdmin)
		{
			model.Links.Add(new()
			{
				ResourceFile = "Resource",
				ResourceKey = "RegistrationAgreementLink",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/EditRegistrationAgreement.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-registrationagreement",
				IconCssClass = "fa fa-handshake-o",
				SortOrder = 85
			});

			//Login Page Text
			if (!WebConfigSettings.DisableLoginInfo)
			{
				model.Links.Add(new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "LoginPageContent",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/EditLoginInfo.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-logininfo",
					IconCssClass = "fa fa-file-o",
					SortOrder = 90
				});
			}

			//Core Data
			if (siteSettings.IsServerAdminSite)
			{
				model.Links.Add(new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "CoreDataAdministrationLink",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/CoreData.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-coredata",
					IconCssClass = "fa fa-database",
					SortOrder = 95
				});
			}
		}

		//Adv. Tools
		if (IsAdminOrContentAdmin || isSiteEditor)
		{
			model.Links.Add(new ()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdvancedToolsLink",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdvancedTools.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-advancedtools",
				IconCssClass = "fa fa-wrench",
				SortOrder = 100
			});

			//System Info
			if (siteSettings.IsServerAdminSite || WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu)
			{
				model.Links.Add(new ()
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuServerInfoLabel",
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/ServerInformation.aspx".ToLinkBuilder().ToString(),
					CssClass = "adminlink-serverinfo",
					IconCssClass = "fa fa-info-circle",
					SortOrder = 105
				});
			}
		}

		//Log Viewer
		if (IsAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableLogViewer)
		{
			model.Links.Add(new ()
			{
				ResourceFile = "Resource",
				ResourceKey = "AdminMenuServerLogLabel",
				Url = $"{WebConfigSettings.AdminDirectoryLocation}/ServerLog.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-log",
				IconCssClass = "fa fa-clipboard",
				SortOrder = 110
			});
		}

		//Supplemental Links
		model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "root").ToList());

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
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuHeading);
	}


	private void LoadSettings()
	{
		IsAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
		IsAdmin = WebUser.IsAdmin;
		isSiteEditor = SiteUtils.UserIsSiteEditor();
		isCommerceReportViewer = WebUser.IsInRoles(siteSettings.CommerceReportViewRoles);
		commerceConfig = SiteUtils.GetCommerceConfig();
		supplementalLinks = ContentAdminLinksConfiguration.GetConfig(siteSettings.SiteId);
		AddClassToBody("administration");
		AddClassToBody("adminmenu");
	}


	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);

		Load += new EventHandler(Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}
	#endregion
}