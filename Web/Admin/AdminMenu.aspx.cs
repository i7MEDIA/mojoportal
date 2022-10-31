using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.Web.AdminUI
{
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
				WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");

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
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuSiteSettingsLink",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/SiteSettings.aspx",
					CssClass = "adminlink-sitesettings",
					IconCssClass = "fa fa-cog",
					SortOrder = -1
				});
			}

			//Site List
			if (WebConfigSettings.AllowMultipleSites && siteSettings.IsServerAdminSite && IsAdmin)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "SiteList",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/SiteList.aspx",
					CssClass = "adminlink-sitelist",
					IconCssClass = "fa fa-list",
					SortOrder = 10
				});
			}

			//Security Advisor
			if (IsAdmin && siteSettings.IsServerAdminSite)
			{
				var needsAttention = !securityAdvisor.UsingCustomMachineKey() || securityAdvisor.DefaultAdmin().userExists;

				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = needsAttention ? "SecurityAdvisorNeedsAttention" : "SecurityAdvisor",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/SecurityAdvisor.aspx",
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
					model.Links.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuRoleAdminLink",
						Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/RoleManager.aspx",
						CssClass = "adminlink-rolemanager",
						IconCssClass = "fa fa-users",
						SortOrder = 20
					});
				}
			}

			//Site Permissions
			if (IsAdmin)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "SiteSettingsPermissionsTab",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/PermissionsMenu.aspx",
					CssClass = "adminlink-sitepermissions",
					IconCssClass = "fa fa-key",
					SortOrder = 25
				});
			}

			//Member List
			if (WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList) || WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers))
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "MemberListLink",
					Url = SiteRoot + WebConfigSettings.MemberListUrl,
					CssClass = "adminlink-memberlist",
					IconCssClass = "fa fa-address-book-o",
					SortOrder = 30
				});
			}

			//Add User
			if (WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers + siteSettings.RolesThatCanManageUsers + siteSettings.RolesThatCanLookupUsers))
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "MemberListAddUserLabel",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ManageUsers.aspx?userId=-1",
					CssClass = "adminlink-adduser",
					IconCssClass = "fa fa-user-plus",
					SortOrder = 35
				});
			}

			//Page Manager
			if (IsAdminOrContentAdmin || isSiteEditor || SiteMapHelper.UserHasAnyCreatePagePermissions(siteSettings))
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuPageTreeLink",
					Url = SiteRoot + WebConfigSettings.PageTreeRelativeUrl,
					CssClass = "adminlink-pagemanager",
					IconCssClass = "fa fa-sitemap",
					SortOrder = 40
				});
			}

			//Content Manager
			if (IsAdminOrContentAdmin || isSiteEditor)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuContentManagerLink",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ContentCatalog.aspx",
					CssClass = "adminlink-contentmanager",
					IconCssClass = "fa fa-hand-pointer-o",
					SortOrder = 45
				});
			}

			//Content Workflow
			if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuContentWorkflowLabel",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ContentWorkflow.aspx",
					CssClass = "adminlink-contentworkflow",
					IconCssClass = "fa fa-code-fork",
					SortOrder = 50
				});
			}

			//Content Templates/Styles
			if (IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates))
			{
				model.Links.AddRange(new List<ContentAdminLink> {
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "ContentTemplates",
						Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ContentTemplates.aspx",
						CssClass = "adminlink-contenttemplates",
						IconCssClass = "fa fa-object-group",
						SortOrder = 55
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "ContentStyleTemplates",
						Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ContentStyles.aspx",
						CssClass = "adminlink-contentstyles",
						IconCssClass = "fa fa-code",
						SortOrder = 60
					}
				});
			}

			//Design Tools
			if (IsAdmin || WebUser.IsContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "DevTools",
					ResourceKey = "DesignTools",
					Url = SiteRoot + "/DesignTools/Default.aspx",
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
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuFileManagerLink",
					Url = SiteRoot + "/FileManager?view=fullpage",
					CssClass = "adminlink-filemanager",
					IconCssClass = "fa fa-folder-open",
					SortOrder = 70
				});
			}

			//Newsletter
			if (WebConfigSettings.EnableNewsletter && (IsAdmin || isSiteEditor || WebUser.IsNewsletterAdmin))
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuNewsletterAdminLabel",
					Url = SiteRoot + "/eletter/Admin.aspx",
					CssClass = "adminlink-newsletter",
					IconCssClass = "fa fa-newspaper-o",
					SortOrder = 75
				});
			}

			//Commerce
			if (isCommerceReportViewer && commerceConfig != null && commerceConfig.IsConfigured)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "CommerceReportsLink",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/SalesSummary.aspx",
					CssClass = "adminlink-commercereports",
					IconCssClass = "fa fa-shopping-basket",
					SortOrder = 80
				});
			}

			//Registration Agreement
			if (IsAdminOrContentAdmin)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "RegistrationAgreementLink",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/EditRegistrationAgreement.aspx",
					CssClass = "adminlink-registrationagreement",
					IconCssClass = "fa fa-handshake-o",
					SortOrder = 85
				});

				//Login Page Text
				if (!WebConfigSettings.DisableLoginInfo)
				{
					model.Links.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "LoginPageContent",
						Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/EditLoginInfo.aspx",
						CssClass = "adminlink-logininfo",
						IconCssClass = "fa fa-file-o",
						SortOrder = 90
					});
				}

				//Core Data
				if (siteSettings.IsServerAdminSite)
				{
					model.Links.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "CoreDataAdministrationLink",
						Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/CoreData.aspx",
						CssClass = "adminlink-coredata",
						IconCssClass = "fa fa-database",
						SortOrder = 95
					});
				}
			}

			//Adv. Tools
			if (IsAdminOrContentAdmin || isSiteEditor)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdvancedToolsLink",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/AdvancedTools.aspx",
					CssClass = "adminlink-advancedtools",
					IconCssClass = "fa fa-wrench",
					SortOrder = 100
				});

				//System Info
				if (siteSettings.IsServerAdminSite || WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu)
				{
					model.Links.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuServerInfoLabel",
						Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ServerInformation.aspx",
						CssClass = "adminlink-serverinfo",
						IconCssClass = "fa fa-info-circle",
						SortOrder = 105
					});
				}
			}

			//Log Viewer
			if (IsAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableLogViewer)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuServerLogLabel",
					Url = SiteRoot + $"{WebConfigSettings.AdminDirectoryLocation}/ServerLog.aspx",
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
				log.Error($"layout ({partialName}) was not found in skin {SiteUtils.GetSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
			}
		}

		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuHeading);

			//heading.Text = Resource.AdminMenuHeading;
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

			this.Load += new EventHandler(this.Page_Load);

			SuppressMenuSelection();
			SuppressPageMenu();
		}
		#endregion
	}
}