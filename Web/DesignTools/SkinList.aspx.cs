using mojoPortal.Business.WebHelpers;
using Resources;
using System;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace mojoPortal.Web.AdminUI;

public partial class SkinListPage : NonCmsBasePage
{
	private readonly string _newWindowMarkup = "onclick=\"window.open(this.href,'_blank');return false;\"";

	protected string PreviewText = string.Empty;
	protected string ManageText = string.Empty;
	protected bool allowEditing = false;


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();
		PopulateLabels();
		BindSkinList();
	}


	private void BindSkinList()
	{
		var skins = SiteUtils.GetSkinList(siteSettings);

		rptSkins.DataSource = skins;

		rptSkins.DataBind();
	}


	protected string BuildDownloadLink(string skinName)
	{
		return $"""
			<a href='{SiteRoot}/DesignTools/DownloadSkin.aspx?s={skinName}' title='{string.Format(CultureInfo.InvariantCulture, Resource.DownloadSkinFormat, skinName)}' class='skinzip' {_newWindowMarkup}>
				<img src='{ImageSiteRoot}/Data/SiteImages/Icons/zip.png' alt='{DevTools.DownloadSkin}' />
			</a>
			""";
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, DevTools.SkinManagement);

		heading.Text = DevTools.SkinManagement;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkDesignerTools.Text = DevTools.DesignTools;
		lnkDesignerTools.NavigateUrl = $"{SiteRoot}/DesignTools/Default.aspx";

		lnkThisPage.Text = DevTools.SkinManagement;
		lnkThisPage.NavigateUrl = $"{SiteRoot}/DesignTools/SkinList.aspx";

		PreviewText = Resource.View;
		ManageText = Resource.Manage;
	}


	private void LoadSettings()
	{
		allowEditing = WebConfigSettings.AllowEditingSkins && (AppConfig.MultiTenancy.AllowEditingSkins || siteSettings.IsServerAdminSite);
		ScriptConfig.IncludeColorBox = true;

		if (allowEditing)
		{
			var refreshFunction = "function refresh () { window.location.reload(true); } ";

			ScriptManager.RegisterClientScriptBlock(
				this,
				GetType(), "refresh",
				refreshFunction,
				true);
		}

		AddClassToBody("administration");
		AddClassToBody("designtools");
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
