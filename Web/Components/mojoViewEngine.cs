#nullable enable
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Plugins;
using System.Collections.Generic;
using System.Web.Mvc;

namespace mojoPortal.Web;

public class mojoViewEngine : RazorViewEngine
{
	private string _skin = "framework";
	private SiteSettings? _siteSettings;


	public mojoViewEngine()
	{
		var masterLocationFormats = new List<string>
		{
			"~/Data/Sites/$SiteId$/skins/$Skin$/Views/Shared/{0}.cshtml"
		};

		var partialViewLocationFormats = new List<string>
		{
			"~/Data/Sites/$SiteId$/skins/$Skin$/Views/{1}/{0}.cshtml",
			"~/Data/Sites/$SiteId$/skins/$Skin$/Views/Shared/{0}.cshtml"
		};

		var viewLocationFormats = new List<string>
		{
			"~/Data/Sites/$SiteId$/skins/$Skin$/Views/{1}/{0}.cshtml",
			"~/Data/Sites/$SiteId$/skins/$Skin$/Views/Shared/{0}.cshtml"
		};

		// Add plugin view/partial-view/master location formats in plugin Order sequence
		foreach (var descriptor in PluginLoader.EnabledPlugins)
		{
			if (descriptor.Instance is not null)
			{
				foreach (var fmt in descriptor.Instance.GetMasterLocationFormats(descriptor.FolderName) ?? [])
				{
					if (!string.IsNullOrWhiteSpace(fmt) && !masterLocationFormats.Contains(fmt))
					{
						masterLocationFormats.Add(fmt);
					}
				}

				foreach (var fmt in descriptor.Instance.GetPartialViewLocationFormats(descriptor.FolderName) ?? [])
				{
					if (!string.IsNullOrWhiteSpace(fmt) && !partialViewLocationFormats.Contains(fmt))
					{
						partialViewLocationFormats.Add(fmt);
					}
				}

				foreach (var fmt in descriptor.Instance.GetViewLocationFormats(descriptor.FolderName) ?? [])
				{
					if (!string.IsNullOrWhiteSpace(fmt) && !viewLocationFormats.Contains(fmt))
					{
						viewLocationFormats.Add(fmt);
					}
				}
			}
			else
			{
				var defaultViewFmt = $"~/Plugins/{descriptor.FolderName}/Views/{{1}}/{{0}}.cshtml";
				var defaultSharedFmt = $"~/Plugins/{descriptor.FolderName}/Views/Shared/{{0}}.cshtml";

				if (!masterLocationFormats.Contains(defaultSharedFmt))
				{
					masterLocationFormats.Add(defaultSharedFmt);
				}

				if (!partialViewLocationFormats.Contains(defaultViewFmt))
				{
					partialViewLocationFormats.Add(defaultViewFmt);
				}

				if (!partialViewLocationFormats.Contains(defaultSharedFmt))
				{
					partialViewLocationFormats.Add(defaultSharedFmt);
				}

				if (!viewLocationFormats.Contains(defaultViewFmt))
				{
					viewLocationFormats.Add(defaultViewFmt);
				}

				if (!viewLocationFormats.Contains(defaultSharedFmt))
				{
					viewLocationFormats.Add(defaultSharedFmt);
				}
			}
		}

		foreach (var fmt in MasterLocationFormats)
		{
			if (!masterLocationFormats.Contains(fmt))
			{
				masterLocationFormats.Add(fmt);
			}
		}

		foreach (var fmt in PartialViewLocationFormats)
		{
			if (!partialViewLocationFormats.Contains(fmt))
			{
				partialViewLocationFormats.Add(fmt);
			}
		}

		foreach (var fmt in ViewLocationFormats)
		{
			if (!viewLocationFormats.Contains(fmt))
			{
				viewLocationFormats.Add(fmt);
			}
		}

		MasterLocationFormats = [.. masterLocationFormats];
		PartialViewLocationFormats = [.. partialViewLocationFormats];
		ViewLocationFormats = [.. viewLocationFormats];
	}


	protected override IView CreatePartialView(ControllerContext ctx, string partialPath)
	{
		PopulateVariables();

		return base.CreatePartialView(ctx, OverriddenViewPath(ctx, partialPath));
	}


	protected override IView CreateView(ControllerContext ctx, string viewPath, string masterPath)
	{
		PopulateVariables();

		return base.CreateView(ctx, OverriddenViewPath(ctx, viewPath), OverriddenViewPath(ctx, masterPath));
	}


	protected override bool FileExists(ControllerContext ctx, string virtualPath)
	{
		PopulateVariables();

		var overriddenPath = OverriddenViewPath(ctx, virtualPath);
		var exists = base.FileExists(ctx, overriddenPath);

		return exists;
	}


	private string OverriddenViewPath(ControllerContext ctx, string viewPath)
	{
		return viewPath.Replace("$Skin$", _skin).Replace("$SiteId$", _siteSettings.SiteId.ToString());
	}


	private void PopulateVariables()
	{
		_siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (_siteSettings is null)
		{
			//not sure this would ever happen ...
			return;
		}

		_skin = _siteSettings.Skin;

		var currentPage = CacheHelper.GetCurrentPage();

		if (currentPage is not null && !string.IsNullOrWhiteSpace(currentPage.Skin))
		{
			_skin = currentPage.Skin;
		}
	}
}
