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
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;

using Resources;

namespace mojoPortal.Web.AdminUI
{
	public partial class ContentWorkflowPage : NonCmsBasePage
    {
		private static readonly ILog log = LogManager.GetLogger(typeof(ContentWorkflowPage));

		//private List<ContentAdminLink> model = new List<ContentAdminLink>();
		private Models.AdminMenuPage model;
		//private Models.BreadCrumbs breadModel;
		private ContentAdminLinksConfiguration supplementalLinks;
		private const string partialName = "_AdminMenu";
		protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			if (!WebUser.IsAdminOrContentAdminOrContentPublisher && !WebUser.IsInRoles(WebConfigSettings.RolesAllowedToUseWorkflowAdminPages))
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
				PageTitle = Resource.AdminMenuContentWorkflowLabel,
				PageHeading = Resource.AdminMenuContentWorkflowLabel
			};

			model.Links.AddRange(new List<ContentAdminLink>
			{
				new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AwaitingApprovalHeading",
					Url = SiteRoot + "/Admin/ContentAwaitingApproval.aspx",
					CssClass = "adminlink-workflow-needapproval",
					IconCssClass = "fa fa-thumbs-o-up",
					SortOrder = 10
				},
				new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "RejectedContentHeading",
					Url = SiteRoot + "/Admin/RejectedContent.aspx",
					CssClass = "adminlink-workflow-rejected",
					IconCssClass = "fa fa-thumbs-o-down",
					SortOrder = 20
				},
				new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "PendingPagesTitle",
					Url = SiteRoot + "/Admin/PendingPages.aspx",
					CssClass = "adminlink-workflow-pendingpages",
					IconCssClass = "fa fa-file-text-o",
					SortOrder = 20
				}
			});

			if (WebConfigSettings.Use3LevelContentWorkflow)
			{
				model.Links.Add(new ContentAdminLink
				{
					ResourceFile = "Resource",
					ResourceKey = "AwaitingPublishingHeading",
					Url = SiteRoot + "/Admin/ContentAwaitingPublishing.aspx",
					CssClass = "adminlink-workflow-needpublishing",
					IconCssClass = "fa fa-paper-plane-o",
					SortOrder = 15
				});
			}

			//Supplemental Links
			model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "workflow").ToList());

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
						Text = Resource.AdminMenuContentWorkflowLabel,
						Url = SiteRoot + "/Admin/ContentWorkflow.aspx",
						IsCurrent = true,
						SortOrder = 0,
						SystemName = "ContentWorkflow",
						Parent = "AdminMenu"
					}
				}
			};

			//skin style information
			//model.SkinConfig = new SkinConfig
			//{
			//	PanelOptions = new List<PanelOption>
			//	{
			//		new PanelOption { Name = "OuterWrapperPanel", Class = "outerwrap"},
			//		new PanelOption { Name = "InnerWrapperPanel", Class = "panelwrapper"},
			//		new PanelOption { Name = "OuterBodyPanel", Class = "outerbody"},
			//		new PanelOption { Name = "InnerBodyPanel", Class = "modulecontent"}
			//	}
			//};

			//Sort the whole thing (allows mixing Supplemental Links with system links instead of them always being at the bottom)
			model.Links.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
			model.BreadCrumbs.Crumbs.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));

		}

        private void PopulateLabels()
        {
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentWorkflowLabel);

			//heading.Text = Resource.AdminMenuContentWorkflowLabel;

			AddClassToBody("administration wfadmin admin-menu-workflow admin-workflow");
        }

		private void PopulateControls()
		{
			//not working yet
			ViewDataDictionary viewData = new ViewDataDictionary
			{
				["foo"] = "bar"
			};

			try
			{
				litMenu.Text = RazorBridge.RenderPartialToString(partialName, model, viewData, "Admin");
			}
			catch (System.Web.HttpException ex)
			{
				log.Error($"layout ({partialName}) was not found in skin {SiteUtils.GetSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
			}
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
