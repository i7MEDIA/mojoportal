using System;
using System.IO;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Web;

[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
public class mojoThemeVirtualFile : VirtualFile
{
	private static readonly ILog log = LogManager.GetLogger(typeof(mojoThemeVirtualFile));
	private const string fallBackSkin = "~/App_Themes/default/theme.skin";

	private String pathToFile;
	public mojoThemeVirtualFile(String virtualPath)
		: base(virtualPath)
	{
		pathToFile = virtualPath;
	}

	public override Stream Open()
	{
		SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		PageSettings currentPage = CacheHelper.GetCurrentPage();

		string siteSkinPath = fallBackSkin;
		string skinSetBy = "site";

		if (siteSettings != null)
		{
			siteSkinPath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.Skin}/theme.skin";
		}


		if (
			siteSettings != null
			&& (siteSettings.AllowUserSkins || (WebConfigSettings.AllowEditingSkins && WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
			)
		{
			if (pathToFile.Contains("App_Themes/userpersonal"))
			{
				SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
				if (currentUser != null && currentUser.Skin.Length > 0)
				{
					pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{currentUser.Skin}/theme.skin";
					skinSetBy = $"userprofile userId=[{currentUser.UserId}]";
				}
			}
		}

		if (pathToFile.Contains("App_Themes/mobile"))
		{
			if (siteSettings.MobileSkin.Length > 0)
			{
				pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.MobileSkin}/theme.skin";
				skinSetBy = "sitemobile";

			}

			if (WebConfigSettings.MobilePhoneSkin.Length > 0)
			{
				pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{WebConfigSettings.MobilePhoneSkin}/theme.skin";
				skinSetBy = "webconfigmobile";
			}
		}
		else if (
			currentPage != null
			&& siteSettings.AllowPageSkins
			&& currentPage.Skin.Length > 0
			&& pathToFile.Contains("App_Themes/pageskin")
			)
		{
			pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{currentPage.Skin}/theme.skin";
			skinSetBy = "page";
		}
		else if (pathToFile.Contains("App_Themes/default"))
		{
			pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.Skin}/theme.skin";
			skinSetBy = "site";
		}
		else if (pathToFile.Contains("App_Themes/preview_"))
		{
			pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{SiteUtils.GetSkinPreviewParam(siteSettings)}/theme.skin";
			skinSetBy = "queryparam";
		}

		string filePath = HostingEnvironment.MapPath(pathToFile);

		try
		{
			return File.OpenRead(filePath);
		}
		catch (DirectoryNotFoundException ex)
		{
			log.Error("\r\nError trying to set theme", ex);
		}
		catch (FileNotFoundException ex)
		{
			log.Error("\r\nError trying to set theme", ex);
		}

		log.Error($"could not find theme.skin in \"{filePath}\" set by \"{skinSetBy}\" setting. Will use site default and fallback to /App_Themes/default/theme.skin if site default doesn't work.");

		if (File.Exists(HostingEnvironment.MapPath(siteSkinPath)))
		{
			return File.OpenRead(HostingEnvironment.MapPath(siteSkinPath));
		}

		return File.OpenRead(HostingEnvironment.MapPath(fallBackSkin));
	}
}