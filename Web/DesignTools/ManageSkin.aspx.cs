using System;
using System.Globalization;
using System.IO;
using System.Text;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI;

public partial class ManageSkinPage : NonCmsBasePage
{
	protected string skinName = string.Empty;
	private string skinBasePath = string.Empty;
	protected bool allowEditing = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		LoadSettings();

		if (!allowEditing)
		{
			WebUtils.SetupRedirect(this, $"{SiteRoot}/DesignTools/SkinList.aspx");
			return;
		}

		if (!Directory.Exists(Server.MapPath(skinBasePath + skinName)))
		{   //skin doesn't exist go back to skin list
			WebUtils.SetupRedirect(this, $"{SiteRoot}/DesignTools/SkinList.aspx");
			return;
		}

		PopulateLabels();
		BindSkin();

	}

	private void BindSkin()
	{

		var title = string.Format(CultureInfo.InvariantCulture, Resource.ManageSkinFormat, skinName);
		Title = SiteUtils.FormatPageTitle(siteSettings, title);

		heading.Text = title;
		lnkThisPage.Text = skinName;
		string skinFolderPath = Server.MapPath(skinBasePath + skinName);
		var files = SkinHelper.GetCssFileList(skinFolderPath: skinFolderPath, recursive: true);
		var sb = new StringBuilder();

		foreach (var file in files)
		{
			//we want to have just the name and the directory starting at the skin path
			string thisPath = file.FullName.Replace(skinFolderPath, "").TrimStart('/').TrimStart('\\').Replace('\\', '/');
			sb.Append($"<li class='simplelist'><a href='{SiteRoot}/DesignTools/CssEditor.aspx?s={skinName}&f={thisPath}'>{thisPath}</a></li>");
		}

		litCssFiles.Text = $"<ul class='simplelist'>{sb}</ul>";
	}

	void btnCopy_Click(object sender, EventArgs e)
	{
		string newSkinName = txtCopyAs.Text.ToCleanFolderName(true);
		if (!string.IsNullOrWhiteSpace(newSkinName) && newSkinName != skinName)
		{
			SkinHelper.CopySkin(Server.MapPath(skinBasePath + skinName), Server.MapPath(skinBasePath + newSkinName));

			WebUtils.SetupRedirect(this, $"{SiteRoot}/DesignTools/ManageSkin.aspx?s={newSkinName}");
			return;
		}
		else
		{
			WebUtils.SetupRedirect(this, Request.RawUrl);
			return;
		}
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ManageSkin);

		heading.Text = Resource.ManageSkin;

		lnkAdminMenu.Text = Resource.AdminMenuLink;
		lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

		lnkDesignerTools.Text = DevTools.DesignTools;
		lnkDesignerTools.NavigateUrl = $"{SiteRoot}/DesignTools/Default.aspx";

		lnkSkinList.Text = DevTools.SkinManagement;
		lnkSkinList.NavigateUrl = $"{SiteRoot}/DesignTools/SkinList.aspx";

		lnkThisPage.Text = Resource.ManageSkin;
		lnkThisPage.NavigateUrl = Request.RawUrl;

		btnCopy.Text = Resource.CopySkinAs;
	}

	private void LoadSettings()
	{
		skinBasePath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/";
		skinName = WebUtils.ParseStringFromQueryString("s", string.Empty);

		allowEditing = WebConfigSettings.AllowEditingSkins
			&& (WebConfigSettings.AllowEditingSkinsInChildSites || siteSettings.IsServerAdminSite);

		AddClassToBody("administration");
		AddClassToBody("designtools");
	}

	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		btnCopy.Click += new EventHandler(btnCopy_Click);

		SuppressMenuSelection();
		SuppressPageMenu();
	}
	#endregion
}