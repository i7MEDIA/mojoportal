/// Created:				2007-04-29
/// Last Modified:			2018-10-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Eclipse Public License 1.0 (https://opensource.org/licenses/eclipse-1.0)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Web.UI;
using mojoPortal.Web.Components;
using log4net;

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
		private List<ContentAdminLink> model = new List<ContentAdminLink>();
		private ContentAdminLinksConfiguration supplementalLinks;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadSettings();
			if (
				(!WebUser.IsAdminOrContentAdminOrRoleAdminOrNewsletterAdmin)
				&& (!isSiteEditor)
				&& (!isCommerceReportViewer)
				)
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
			//Site Settings
			if (IsAdminOrContentAdmin || isSiteEditor)
			{
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuSiteSettingsLink",
					Url = SiteRoot + "/Admin/SiteSettings.aspx",
					CssClass = "adminlink-sitesettings",
					IconCssClass = "fa fa-cog",
					SortOrder = -1
				});
			}

			//Site List
			if (WebConfigSettings.AllowMultipleSites && siteSettings.IsServerAdminSite && IsAdmin)
			{
				model.Add(new ContentAdminLink {
					ResourceFile = "Resource",
					ResourceKey = "SiteList",
					Url = SiteRoot + "/Admin/SiteList.aspx",
					CssClass = "adminlink-sitelist",
					IconCssClass = "fa fa-list",
					SortOrder = 10
				});
			}

			//Security Advisor
			if (IsAdmin && siteSettings.IsServerAdminSite)
			{
				bool needsAttention = !securityAdvisor.UsingCustomMachineKey();
				model.Add(new ContentAdminLink {
					ResourceFile = "Resource",
					ResourceKey = needsAttention ? "SecurityAdvisorNeedsAttention" : "SecurityAdvisor",
					Url = SiteRoot + "/Admin/SecurityAdvisor.aspx",
					CssClass = needsAttention ? "adminlink-securityadvisor text-danger" : "adminlink-securityadvisor",
					IconCssClass = needsAttention ? "fa fa-shield text-danger" : "fa fa-shield",
					SortOrder = 15
				});
			}

			//Role Admin
			if (WebUser.IsRoleAdmin || IsAdmin)
			{
				bool addLink = true;
				if (WebConfigSettings.UseRelatedSiteMode && WebConfigSettings.RelatedSiteModeHideRoleManagerInChildSites && siteSettings.SiteId != WebConfigSettings.RelatedSiteID)
				{
						addLink = false;
				}
				if (addLink)
				{
					model.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuRoleAdminLink",
						Url = SiteRoot + "/Admin/RoleManager.aspx",
						CssClass = "adminlink-rolemanager",
						IconCssClass = "fa fa-users",
						SortOrder = 20
					});
				}
			}

			//Site Permissions
			if (IsAdmin)
			{
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "SiteSettingsPermissionsTab",
					Url = SiteRoot + "/Admin/PermissionsMenu.aspx",
					CssClass = "adminlink-sitepermissions",
					IconCssClass = "fa fa-key",
					SortOrder = 25
				});
			}

			//Member List
			if (WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList) || WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers))
			{
				model.Add(new ContentAdminLink
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
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "MemberListAddUserLabel",
					Url = SiteRoot + "/Admin/ManageUsers.aspx?userId=-1",
					CssClass = "adminlink-adduser",
					IconCssClass = "fa fa-user-plus",
					SortOrder = 35
				});
			}

			//Page Manager
			if (IsAdminOrContentAdmin || isSiteEditor || SiteMapHelper.UserHasAnyCreatePagePermissions(siteSettings))
			{
				model.Add(new ContentAdminLink
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
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuContentManagerLink",
					Url = SiteRoot + "/Admin/ContentCatalog.aspx",
					CssClass = "adminlink-contentmanager",
					IconCssClass = "fa fa-hand-pointer-o",
					SortOrder = 45
				});
			}

			//Content Workflow
			if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow)
			{
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuContentWorkflowLabel",
					Url = SiteRoot + "/Admin/ContentWorkflow.aspx",
					CssClass = "adminlink-contentworkflow",
					IconCssClass = "fa fa-code-fork",
					SortOrder = 50
				});
			}

			//Content Templates/Styles
			if (IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates))
			{
				model.AddRange(new List<ContentAdminLink> {
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "ContentTemplates",
						Url = SiteRoot + "/Admin/ContentTemplates.aspx",
						CssClass = "adminlink-contenttemplates",
						IconCssClass = "fa fa-object-group",
						SortOrder = 55
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "ContentStyleTemplates",
						Url = SiteRoot + "/Admin/ContentStyles.aspx",
						CssClass = "adminlink-contentstyles",
						IconCssClass = "fa fa-code",
						SortOrder = 60
					}
				});
			}

			//Design Tools
			if (IsAdmin || WebUser.IsContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))
			{
				model.Add(new ContentAdminLink
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
				model.Add(new ContentAdminLink
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
				model.Add(new ContentAdminLink
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
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "CommerceReportsLink",
					Url = SiteRoot + "/Admin/SalesSummary.aspx",
					CssClass = "adminlink-commercereports",
					IconCssClass = "fa fa-shopping-basket",
					SortOrder = 80
				});
			}

			//Registration Agreement
			if (IsAdminOrContentAdmin)
			{
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "RegistrationAgreementLink",
					Url = SiteRoot + "/Admin/EditRegistrationAgreement.aspx",
					CssClass = "adminlink-registrationagreement",
					IconCssClass = "fa fa-handshake-o",
					SortOrder = 85
				});

				//Login Page Text
				if (!WebConfigSettings.DisableLoginInfo)
				{
					model.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "LoginPageContent",
						Url = SiteRoot + "/Admin/EditLoginInfo.aspx",
						CssClass = "adminlink-logininfo",
						IconCssClass = "fa fa-file-o",
						SortOrder = 90
					});
				}

				//Core Data
				if (siteSettings.IsServerAdminSite)
				{
					model.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "CoreDataAdministrationLink",
						Url = SiteRoot + "/Admin/CoreData.aspx",
						CssClass = "adminlink-coredata",
						IconCssClass = "fa fa-database",
						SortOrder = 95
					});
				}
			}

			//Adv. Tools
			if (IsAdminOrContentAdmin || isSiteEditor)
			{
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdvancedToolsLink",
					Url = SiteRoot + "/Admin/AdvancedTools.aspx",
					CssClass = "adminlink-advancedtools",
					IconCssClass = "fa fa-wrench",
					SortOrder = 100
				});
				
				//System Info
				if (siteSettings.IsServerAdminSite || WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu)
				{
					model.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuServerInfoLabel",
						Url = SiteRoot + "/Admin/ServerInformation.aspx",
						CssClass = "adminlink-serverinfo",
						IconCssClass = "fa fa-info-circle",
						SortOrder = 105
					});
				}
			}

			//Log Viewer
			if (IsAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableLogViewer)
			{
				model.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AdminMenuServerLogLabel",
					Url = SiteRoot + "/Admin/ServerLog.aspx",
					CssClass = "adminlink-log",
					IconCssClass = "fa fa-clipboard",
					SortOrder = 110
				});
			}

			//Supplemental Links
			model.AddRange(supplementalLinks.AdminLinks);

			//Sort the whole thing (allows mixing Supplemental Links with system links instead of them always being at the bottom)
			model.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
		}

		private void PopulateControls()
		{
			try
			{
				litMenu.Text = RazorBridge.RenderPartialToString("_AdminMenu", model, "Admin");
			}
			catch (System.Web.HttpException ex)
			{
				log.ErrorFormat(
					"layout for AdminMenu (_AdminMenu) was not found in skin {0}. perhaps it is in a different skin. Error was: {1}",
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);
			}
		}

		//private void BuildAdditionalMenuListItems()
		//{
		//	if (siteSettings == null) return;




		//	StringBuilder addedLinks = new StringBuilder();
		//	foreach (ContentAdminLink link in supplementalLinks.AdminLinks)
		//	{
		//		if (
		//			(link.VisibleToRoles.Length == 0)
		//			|| (WebUser.IsInRoles(link.VisibleToRoles))
		//			)
		//		{
		//			addedLinks.Append("\n<li>");
		//			addedLinks.Append("<a ");
		//			string title = ResourceHelper.GetResourceString(link.ResourceFile, link.ResourceKey);
		//			addedLinks.Append("title='" + title + "' ");
		//			addedLinks.Append("class='" + link.CssClass + "' ");
		//			string url = link.Url;
		//			if (url.StartsWith("~/"))
		//			{
		//				url = SiteRoot + "/" + url.Replace("~/", string.Empty);
		//			}
		//			addedLinks.Append("href='" + url + "'>" + title + "</a>");
		//			addedLinks.Append("</li>");
		//		}

		//	}

		//	litSupplementalLinks.Text = addedLinks.ToString();
		//}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuHeading);

			heading.Text = Resource.AdminMenuHeading;

			//liSiteSettings.Visible = IsAdminOrContentAdmin || isSiteEditor;
			//lnkSiteSettings.Text = Resource.AdminMenuSiteSettingsLink;
			//lnkSiteSettings.NavigateUrl = SiteRoot + "/Admin/SiteSettings.aspx";

			//liSiteList.Visible = ((WebConfigSettings.AllowMultipleSites) && (siteSettings.IsServerAdminSite) && IsAdmin);
			//lnkSiteList.Text = Resource.SiteList;
			//lnkSiteList.NavigateUrl = SiteRoot + "/Admin/SiteList.aspx";

			//lnkSecurityAdvisor.Text = Resource.SecurityAdvisor;
			//lnkSecurityAdvisor.NavigateUrl = SiteRoot + "/Admin/SecurityAdvisor.aspx";
			//if (IsAdmin && siteSettings.IsServerAdminSite)
			//{
			//	liSecurityAdvisor.Visible = true;
			//	if (!securityAdvisor.UsingCustomMachineKey())
			//	{
			//		liSecurityAdvisor.Attributes["class"] += " secwarning";
			//		lnkSecurityAdvisor.Text = Resource.SecurityAdvisorNeedsAttention;
			//	}
			//}

			//liCommerceReports.Visible = (isCommerceReportViewer && (commerceConfig != null) && (commerceConfig.IsConfigured));
			//lnkCommerceReports.Text = Resource.CommerceReportsLink;
			//lnkCommerceReports.NavigateUrl = SiteRoot + "/Admin/SalesSummary.aspx";

			//liContentManager.Visible = (IsAdminOrContentAdmin || isSiteEditor);
			//lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
			//lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";

			//liContentWorkFlow.Visible = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);
			//lnkContentWorkFlow.Visible = siteSettings.EnableContentWorkflow && WebUser.IsAdminOrContentAdminOrContentPublisher;
			//lnkContentWorkFlow.Text = Resource.AdminMenuContentWorkflowLabel;
			//lnkContentWorkFlow.NavigateUrl = SiteRoot + "/Admin/ContentWorkflow.aspx";

			//liContentTemplates.Visible = (IsAdminOrContentAdmin || isSiteEditor || (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)));
			//lnkContentTemplates.Text = Resource.ContentTemplates;
			//lnkContentTemplates.NavigateUrl = SiteRoot + "/Admin/ContentTemplates.aspx";

			//liStyleTemplates.Visible = (IsAdminOrContentAdmin || isSiteEditor || (WebUser.IsInRoles(siteSettings.RolesThatCanEditContentTemplates)));
			//lnkStyleTemplates.Text = Resource.ContentStyleTemplates;
			//lnkStyleTemplates.NavigateUrl = SiteRoot + "/Admin/ContentStyles.aspx";

			//liPageTree.Visible = (IsAdminOrContentAdmin || isSiteEditor || (SiteMapHelper.UserHasAnyCreatePagePermissions(siteSettings)));
			//lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
			//lnkPageTree.NavigateUrl = SiteRoot + WebConfigSettings.PageTreeRelativeUrl;

			//liRoleAdmin.Visible = (WebUser.IsRoleAdmin || IsAdmin);
			//lnkRoleAdmin.Text = Resource.AdminMenuRoleAdminLink;
			//lnkRoleAdmin.NavigateUrl = SiteRoot + "/Admin/RoleManager.aspx";

			//liPermissions.Visible = IsAdmin;
			//lnkPermissionAdmin.Text = Resource.SiteSettingsPermissionsTab;
			//lnkPermissionAdmin.NavigateUrl = SiteRoot + "/Admin/PermissionsMenu.aspx";

			//if ((WebConfigSettings.UseRelatedSiteMode) && (WebConfigSettings.RelatedSiteModeHideRoleManagerInChildSites))
			//{
			//	if (siteSettings.SiteId != WebConfigSettings.RelatedSiteID)
			//	{
			//		liRoleAdmin.Visible = false;
			//	}
			//}


			////
			//// File Manager Link
			////
			////liFileManager.Visible = IsAdminOrContentAdmin;

			////if ((!siteSettings.IsServerAdminSite) && (!WebConfigSettings.AllowFileManagerInChildSites))
			////{
			////	liFileManager.Visible = false;
			////}

			////if (WebConfigSettings.DisableFileManager)
			////{
			////	liFileManager.Visible = false;
			////}

			////lnkFileManager.Text = Resource.AdminMenuFileManagerLink;
			////lnkFileManager.NavigateUrl = SiteRoot + "/FileManager";


			////
			//// Member List
			////
			//lnkMemberList.Text = Resource.MemberListLink;
			//lnkMemberList.NavigateUrl = SiteRoot + WebConfigSettings.MemberListUrl;
			//lnkMemberList.Visible = ((WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList)) || (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)));

			//liAddUser.Visible = ((WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers)) || (WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)) || (WebUser.IsInRoles(siteSettings.RolesThatCanLookupUsers)));
			//lnkAddUser.Text = Resource.MemberListAddUserLabel;
			//lnkAddUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userId=-1";

			//if (WebConfigSettings.EnableNewsletter)
			//{
			//	liNewsletter.Visible = (IsAdmin || isSiteEditor || WebUser.IsNewsletterAdmin);
			//	lnkNewsletter.Text = Resource.AdminMenuNewsletterAdminLabel;
			//	lnkNewsletter.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

			//	//liTaskQueue.Visible = IsAdmin || WebUser.IsNewsletterAdmin;
			//	//lnkTaskQueue.Text = Resource.TaskQueueMonitorHeading;
			//	//lnkTaskQueue.NavigateUrl = SiteRoot + "/Admin/TaskQueueMonitor.aspx";

			//}
			//else
			//{
			//	liNewsletter.Visible = false;
			//	//liTaskQueue.Visible = false;
			//}

			//liRegistrationAgreement.Visible = (IsAdminOrContentAdmin);
			//lnkRegistrationAgreement.Text = Resource.RegistrationAgreementLink;
			//lnkRegistrationAgreement.NavigateUrl = SiteRoot + "/Admin/EditRegistrationAgreement.aspx";

			//liLoginInfo.Visible = (IsAdminOrContentAdmin) && !WebConfigSettings.DisableLoginInfo;
			//lnkLoginInfo.Text = Resource.LoginPageContent;
			//lnkLoginInfo.NavigateUrl = SiteRoot + "/Admin/EditLoginInfo.aspx";



			//liCoreData.Visible = (IsAdminOrContentAdmin && siteSettings.IsServerAdminSite);
			//lnkCoreData.Text = Resource.CoreDataAdministrationLink;
			//lnkCoreData.NavigateUrl = SiteRoot + "/Admin/CoreData.aspx";

			//liAdvancedTools.Visible = (IsAdminOrContentAdmin || isSiteEditor);
			//lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
			//lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";


			//liServerInfo.Visible = (IsAdminOrContentAdmin || isSiteEditor) && (siteSettings.IsServerAdminSite || WebConfigSettings.ShowSystemInformationInChildSiteAdminMenu);
			//lnkServerInfo.Text = Resource.AdminMenuServerInfoLabel;
			//lnkServerInfo.NavigateUrl = SiteRoot + "/Admin/ServerInformation.aspx";

			//liLogViewer.Visible = IsAdmin && siteSettings.IsServerAdminSite && WebConfigSettings.EnableLogViewer;
			//lnkLogViewer.Text = Resource.AdminMenuServerLogLabel;
			//lnkLogViewer.NavigateUrl = SiteRoot + "/Admin/ServerLog.aspx";

			//liDesignTools.Visible = IsAdmin || WebUser.IsContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins);
			//lnkDesignTools.Text = DevTools.DesignTools;
			//lnkDesignTools.NavigateUrl = SiteRoot + "/DesignTools/Default.aspx";

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