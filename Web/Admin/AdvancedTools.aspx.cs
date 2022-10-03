/// Author:					
/// Created:				2008-06-14
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
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using Resources;
using log4net;
using System.Collections.Generic;
using mojoPortal.Web.Components;
using System.Linq;

namespace mojoPortal.Web.AdminUI
{
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

            if ((!isAdmin)&&(!isContentAdmin)&&(!isSiteEditor))
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/AccessDenied.aspx");
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
				model.Links.AddRange(new List<ContentAdminLink>
				{
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuUrlManagerLink",
						Url = SiteRoot + "/Admin/UrlManager.aspx",
						CssClass = "adminlink-advanced-urls",
						IconCssClass = "fa fa-link",
						SortOrder = 10
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "RedirectManagerLink",
						Url = SiteRoot + "/Admin/RedirectManager.aspx",
						CssClass = "adminlink-advanced-redirects",
						IconCssClass = "fa fa-reply",
						SortOrder = 15
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminIndexBrowser",
						Url = SiteRoot + "/Admin/IndexBrowser.aspx",
						CssClass = "adminlink-advanced-indexbrowser",
						IconCssClass = "fa fa-search",
						SortOrder = 18
					},
				});
			}

			if (isAdmin && siteSettings.IsServerAdminSite)
			{
				model.Links.AddRange(new List<ContentAdminLink>
				{
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuBannedIPAddressesLabel",
						Url = SiteRoot + "/Admin/BannedIPAddresses.aspx",
						CssClass = "adminlink-advanced-bannedip",
						IconCssClass = "fa fa-ban",
						SortOrder = 20
					},
					new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "AdminMenuFeatureModulesLink",
						Url = SiteRoot + "/Admin/ModuleAdmin.aspx",
						CssClass = "adminlink-advanced-modules",
						IconCssClass = "fa fa-star",
						SortOrder = 25
					},
				});

				if ((!WebConfigSettings.DisableTaskQueue) && (isAdmin || WebUser.IsNewsletterAdmin))
				{
					model.Links.Add(new ContentAdminLink
					{
						ResourceFile = "Resource",
						ResourceKey = "TaskQueueMonitorHeading",
						Url = SiteRoot + "/Admin/TaskQueueMonitor.aspx",
						CssClass = "adminlink-advanced-queue",
						IconCssClass = "fa fa-clock-o",
						SortOrder = 30
					});
				}

				if (WebConfigSettings.EnableDeveloperMenuInAdminMenu)
				{
					model.Links.Add(new ContentAdminLink
					{
						ResourceFile = "DevTools",
						ResourceKey = "DevToolsHeading",
						Url = SiteRoot + "/DevAdmin/Default.aspx",
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
						Text = Resource.AdvancedToolsLink,
						Url = SiteRoot + "/Admin/AdvancedTools.aspx",
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
				log.Error($"layout ({partialName}) was not found in skin {SiteUtils.GetSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
			}
		}


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings,Resource.AdvancedToolsHeading);

            //lnkAdminMenu.Text = Resource.AdminMenuLink;
            //lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            //lnkCurrentPage.Text = Resource.AdvancedToolsLink;
            //lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            //heading.Text = Resource.AdvancedToolsHeading;

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
}
