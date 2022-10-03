// Author:					
// Created:					2010-12-09
// Last Modified:			2018-11-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web;
using Resources;


namespace mojoPortal.Web.AdminUI
{
    public partial class DesignerToolsPage : NonCmsBasePage
    {
		private static readonly ILog log = LogManager.GetLogger(typeof(DesignerToolsPage));

		private mojoPortal.Web.Models.AdminMenuPage model;
		private ContentAdminLinksConfiguration supplementalLinks;
		private const string partialName = "_AdminMenu";
		protected void Page_Load(object sender, EventArgs e)
        {

            if ((!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))) 
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
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
				PageTitle = DevTools.DesignTools,
				PageHeading = DevTools.DesignTools
			};

			model.Links.AddRange(new List<ContentAdminLink>
			{
				new ContentAdminLink
				{
					ResourceFile = "DevTools",
					ResourceKey = "SkinManagement",
					Url = SiteRoot + "/DesignTools/SkinList.aspx",
					CssClass = "adminlink-design-skins",
					IconCssClass = "fa fa-image",
					SortOrder = 10
				},
				new ContentAdminLink
				{
					ResourceFile = "DevTools",
					ResourceKey = "CacheTool",
					Url = SiteRoot + "/DesignTools/CacheTool.aspx",
					CssClass = "adminlink-design-cache",
					IconCssClass = "fa fa-floppy-o",
					SortOrder = 15
				},
			});


			//Supplemental Links
			model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "designtools").ToList());

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
						Text = DevTools.DesignTools,
						Url = SiteRoot + "/DesignTools/Default.aspx",
						IsCurrent = true,
						SortOrder = 0,
						SystemName = "DesignTools",
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
            Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.DesignTools);

			AddClassToBody("administration wfadmin admin-menu-design admin-design");

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
