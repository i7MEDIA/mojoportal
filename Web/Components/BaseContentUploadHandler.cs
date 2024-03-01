using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using System;
using System.Web;

namespace mojoPortal.Web.UI;

/// <summary>
/// Provides common functionality for upload handlers
/// </summary>
public class BaseContentUploadHandler
{
	protected static readonly ILog log = LogManager.GetLogger(typeof(BaseContentUploadHandler));

	protected HttpRequest Request;
	protected HttpResponse Response;
	protected HttpServerUtility Server;
	protected IFileSystem FileSystem = null;
	protected int PageId = -1;
	protected int ModuleId = -1;
	protected SiteSettings CurrentSite = null;
	protected PageSettings CurrentPage = null;
	protected SiteUser CurrentUser = null;

	public bool UserIsAdmin { get; private set; } = false;

	public bool UserIsContentAdmin { get; private set; } = false;

	public bool UserIsSiteEditor { get; private set; } = false;

	public void Initialize(HttpContext context)
	{
		Request = context.Request;
		Response = context.Response;
		Server = context.Server;

		PageId = WebUtils.ParseInt32FromQueryString("pageid", PageId);
		ModuleId = WebUtils.ParseInt32FromQueryString("mid", ModuleId);
		CurrentSite = CacheHelper.GetCurrentSiteSettings();
		CurrentUser = SiteUtils.GetCurrentSiteUser();

		UserIsAdmin = WebUser.IsAdmin;

		if (!UserIsAdmin)
		{
			UserIsContentAdmin = WebUser.IsContentAdmin;
			if (!UserIsContentAdmin)
			{
				UserIsSiteEditor = SiteUtils.UserIsSiteEditor();
			}
		}

		FileSystem = FileSystemHelper.LoadFileSystem(WebConfigSettings.FileSystemProvider);
	}

	public bool UserCanEditModule(int moduleId, Guid featureGuid)
	{
		if (!Request.IsAuthenticated)
		{
			return false;
		}

		if (WebConfigSettings.FileServiceRejectFishyPosts)
		{
			if (SiteUtils.IsFishyPost(Request))
			{
				return false;
			}
		}

		CurrentPage ??= CacheHelper.GetCurrentPage();

		if (CurrentPage is null)
		{
			return false;
		}

		if (!UserIsAdmin && CurrentPage.EditRoles == "Admins;")
		{
			return false;
		}

		bool moduleFoundOnPage = false;
		string moduleEditRoles = string.Empty;

		foreach (Module m in CurrentPage.Modules)
		{
			if (m.ModuleId == moduleId && (featureGuid == Guid.Empty || m.FeatureGuid == featureGuid))
			{
				moduleFoundOnPage = true;
				moduleEditRoles = m.AuthorizedEditRoles;
			}
		}

		if (!moduleFoundOnPage)
		{
			return false;
		}

		if (!UserIsAdmin && moduleEditRoles == "Admins;")
		{
			return false;
		}

		if (UserIsAdmin || UserIsContentAdmin || UserIsSiteEditor)
		{
			return true;
		}

		if (WebUser.IsInRoles(moduleEditRoles) || WebUser.IsInRoles(CurrentPage.EditRoles))
		{
			return true;
		}

		return false;
	}


	public Module GetModule(int moduleId, Guid featureGuid)
	{
		CurrentPage ??= CacheHelper.GetCurrentPage();

		if (CurrentPage is null)
		{
			return null;
		}

		foreach (Module m in CurrentPage.Modules)
		{
			if (m.ModuleId == moduleId && (featureGuid == Guid.Empty || m.FeatureGuid == featureGuid))
			{
				return m;
			}
		}

		return null;
	}

	public bool UserCanViewPage(int moduleId, Guid featureGuid)
	{
		if (CurrentPage is null)
		{
			return false;
		}

		var module = GetModule(moduleId, featureGuid);

		if (module is null)
		{
			return false;
		}

		if (WebUser.IsAdmin)
		{
			return true;
		}

		if (WebUser.IsContentAdmin)
		{
			if (CurrentPage.AuthorizedRoles == "Admins;")
			{
				return false;
			}

			if (module.ViewRoles == "Admins;")
			{
				return false;
			}

			return true;
		}

		if (SiteUtils.UserIsSiteEditor())
		{
			if (CurrentPage.AuthorizedRoles == "Admins;")
			{
				return false;
			}

			if (module.ViewRoles == "Admins;")
			{
				return false;
			}

			return true;
		}

		if (!WebUser.IsInRoles(module.ViewRoles))
		{
			return false;
		}

		if (WebUser.IsInRoles(CurrentPage.AuthorizedRoles))
		{
			return true;
		}

		return false;
	}
}

public class UploadFilesResult
{
	public string Thumbnail_url { get; set; }
	public string FileUrl { get; set; }
	public string FullSizeUrl { get; set; }
	public string Name { get; set; }
	public int Length { get; set; }
	public string Type { get; set; }
	public string ReturnValue { get; set; }
	public string ErrorMessage { get; set; }
}