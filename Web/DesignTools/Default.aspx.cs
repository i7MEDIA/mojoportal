using System;
using System.Linq;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class DesignerToolsPage : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(DesignerToolsPage));

	private Models.AdminMenuPage model;
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

		model.Links.AddRange(
		[
			new() {
				ResourceFile = "DevTools",
				ResourceKey = "SkinManagement",
				Url = "DesignTools/SkinList.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-design-skins",
				IconCssClass = "fa fa-image",
				SortOrder = 10
			},
			new() {
				ResourceFile = "DevTools",
				ResourceKey = "CacheTool",
				Url = "DesignTools/CacheTool.aspx".ToLinkBuilder().ToString(),
				CssClass = "adminlink-design-cache",
				IconCssClass = "fa fa-floppy-o",
				SortOrder = 15
			},
		]);

		//Supplemental Links
		model.Links.AddRange(supplementalLinks.AdminLinks.Where(l => l.Parent.ToLower() == "designtools").ToList());

		//BreadCrumbs

		model.BreadCrumbs = new Models.BreadCrumbs
		{
			CrumbArea = Models.BreadCrumbArea.Admin,
			Crumbs =
			{
				new()
				{
					Text = Resource.AdminMenuLink,
					Url = $"{WebConfigSettings.AdminDirectoryLocation}/AdminMenu.aspx".ToLinkBuilder().ToString(),
					SortOrder = -1,
					SystemName = "AdminMenu",
					Parent = "root"
				},
				new()
				{
					Text = DevTools.DesignTools,
					Url = "DesignTools/Default.aspx".ToLinkBuilder().ToString(),
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
			log.Error($"layout ({partialName}) was not found in skin {SiteUtils.DetermineSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
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