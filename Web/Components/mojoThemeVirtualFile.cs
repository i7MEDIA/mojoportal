using log4net;
using mojoPortal.Business.WebHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace mojoPortal.Web;

[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
public class mojoThemeVirtualFile : VirtualFile
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(mojoThemeVirtualFile));
	private const string _fallBackSkin = "~/App_Themes/default/theme.skin";

	private static readonly Dictionary<string, string> _legacyNamespaceToPluginAssembly = new(StringComparer.OrdinalIgnoreCase)
	{
		["mojoPortal.Web.ContentUI"] = "mojoPortal.Plugins.HtmlContent",
		["mojoPortal.Web.BlogUI"] = "mojoPortal.Plugins.Blog.UI",
		["mojoPortal.Web.ForumUI"] = "mojoPortal.Plugins.Forums.UI",
		["mojoPortal.Web.FeedUI"] = "mojoPortal.Plugins.FeedManager.UI",
		["mojoPortal.Web.SharedFilesUI"] = "mojoPortal.Plugins.SharedFiles.UI",
		["mojoPortal.Web.GalleryUI"] = "mojoPortal.Plugins.ImageGallery.UI",
		["mojoPortal.Web.ContactUI"] = "mojoPortal.Plugins.ContactForm.UI",
		["mojoPortal.Web.LinksUI"] = "mojoPortal.Plugins.List.UI",
		["mojoPortal.Web.EventCalendarUI"] = "mojoPortal.Plugins.EventCalendar.UI",
		["mojoPortal.MediaPlayerUI"] = "mojoPortal.Plugins.KDMediaPlayer.UI",
		["PollFeature.UI"] = "mojoPortal.Plugins.Poll.UI",
		["SurveyFeature.UI"] = "mojoPortal.Plugins.Survey.UI"
	};

	private static readonly (string ControlName, string AssemblyName, string Namespace, string TagPrefix)[] _migratedPortalControls =
	[
		("LoginModuleDisplaySettings", "mojoPortal.Plugins.Login", "mojoPortal.Plugins.Login", "mppluginlogin"),
		("RecentContentDisplaySettings", "mojoPortal.Plugins.RecentContent", "mojoPortal.Web.UI", "mppluginrc"),
		("SearchModuleDisplaySettings", "mojoPortal.Plugins.SiteSearch", "mojoPortal.Web.UI", "mppluginss")
	];

	private static readonly Regex _serverCommentRegex = new(@"<%--[\s\S]*?--%>", RegexOptions.Compiled);
	private static readonly Regex _registerDirectiveRegex = new(@"<%@\s*Register\b[^>]*?%>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	private static readonly Regex _attrRegex = new(@"(?<name>\w+)\s*=\s*[""'](?<val>[^""']*)[""']", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	private string _pathToFile;


	public mojoThemeVirtualFile(string virtualPath) : base(virtualPath)
	{
		_pathToFile = virtualPath;
	}


	public override Stream Open()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var currentPage = CacheHelper.GetCurrentPage();
		var siteSkinPath = _fallBackSkin;
		var skinSetBy = "site";

		if (siteSettings is not null)
		{
			siteSkinPath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.Skin}/theme.skin";
		}

		if (siteSettings is not null &&
			(
				siteSettings.AllowUserSkins ||
				WebConfigSettings.AllowEditingSkins &&
				WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
		{
			if (_pathToFile.Contains("App_Themes/userpersonal"))
			{
				var currentUser = SiteUtils.GetCurrentSiteUser();

				if (currentUser is not null && currentUser.Skin.Length > 0)
				{
					_pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{currentUser.Skin}/theme.skin";
					skinSetBy = $"userprofile userId=[{currentUser.UserId}]";
				}
			}
		}

		if (_pathToFile.Contains("App_Themes/mobile"))
		{
			if (siteSettings.MobileSkin.Length > 0)
			{
				_pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.MobileSkin}/theme.skin";
				skinSetBy = "sitemobile";
			}

			if (WebConfigSettings.MobilePhoneSkin.Length > 0)
			{
				_pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{WebConfigSettings.MobilePhoneSkin}/theme.skin";
				skinSetBy = "webconfigmobile";
			}
		}
		else if (
			currentPage is not null &&
			siteSettings.AllowPageSkins &&
			currentPage.Skin.Length > 0 &&
			_pathToFile.Contains("App_Themes/pageskin"))
		{
			_pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{currentPage.Skin}/theme.skin";
			skinSetBy = "page";
		}
		else if (_pathToFile.Contains("App_Themes/default"))
		{
			_pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.Skin}/theme.skin";
			skinSetBy = "site";
		}
		else if (_pathToFile.Contains("App_Themes/preview_"))
		{
			_pathToFile = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{SiteUtils.GetSkinPreviewParam(siteSettings)}/theme.skin";
			skinSetBy = "queryparam";
		}

		var filePath = HostingEnvironment.MapPath(_pathToFile);

		try
		{
			if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
			{
				return OpenFilteredSkinStream(filePath);
			}
		}
		catch (DirectoryNotFoundException ex)
		{
			_log.Error("\r\nError trying to set theme", ex);
		}
		catch (FileNotFoundException ex)
		{
			_log.Error("\r\nError trying to set theme", ex);
		}

		_log.Error($"could not find theme.skin in \"{filePath}\" set by \"{skinSetBy}\" setting. Will use site default and fallback to /App_Themes/default/theme.skin if site default doesn't work.");

		var mappedSiteSkinPath = HostingEnvironment.MapPath(siteSkinPath);

		if (!string.IsNullOrWhiteSpace(mappedSiteSkinPath) && File.Exists(mappedSiteSkinPath))
		{
			return OpenFilteredSkinStream(mappedSiteSkinPath);
		}

		return OpenFilteredSkinStream(HostingEnvironment.MapPath(_fallBackSkin));
	}

	private static Stream OpenFilteredSkinStream(string physicalPath)
	{
		var content = File.ReadAllText(physicalPath, Encoding.UTF8);
		var loadedAssemblies = new HashSet<string>(
			AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => !a.IsDynamic)
				.Select(a => a.GetName().Name)
				.Where(n => !string.IsNullOrWhiteSpace(n)),
			StringComparer.OrdinalIgnoreCase);

		// Preserve server-side comments (<%-- ... --%>) so commented-out directives are not altered
		var comments = new List<string>();
		var working = _serverCommentRegex.Replace(content, match =>
		{
			var index = comments.Count;

			comments.Add(match.Value);

			return $"__MP_SKIN_COMMENT_{index}__";
		});

		var unloadedTagPrefixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		working = _registerDirectiveRegex.Replace(working, match =>
		{
			var directive = match.Value;
			var attrs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach (Match attrMatch in _attrRegex.Matches(directive))
			{
				attrs[attrMatch.Groups["name"].Value] = attrMatch.Groups["val"].Value;
			}

			// UserControl registrations (Src="...") do not reference an assembly
			if (attrs.ContainsKey("Src"))
			{
				return directive;
			}

			if (!attrs.TryGetValue("Assembly", out var assemblyName) ||
				!attrs.TryGetValue("TagPrefix", out var tagPrefix))
			{
				return directive;
			}

			attrs.TryGetValue("Namespace", out var ns);

			if (!string.IsNullOrWhiteSpace(ns) &&
				_legacyNamespaceToPluginAssembly.TryGetValue(ns, out var targetPluginAssembly))
			{
				if (loadedAssemblies.Contains(targetPluginAssembly))
				{
					return Regex.Replace(
						directive,
						@"(\bAssembly\s*=\s*[""'])[^""']*([""'])",
						$"${{1}}{targetPluginAssembly}${{2}}",
						RegexOptions.IgnoreCase);
				}

				unloadedTagPrefixes.Add(tagPrefix);
				return string.Empty;
			}

			if (!loadedAssemblies.Contains(assemblyName))
			{
				unloadedTagPrefixes.Add(tagPrefix);
				return string.Empty;
			}

			return directive;
		});

		foreach (var prefix in unloadedTagPrefixes)
		{
			var escapedPrefix = Regex.Escape(prefix);
			// Remove paired tags <prefix:Control ...>...</prefix:Control> (allowing '>' inside quoted attribute values)
			working = Regex.Replace(
				working,
				$@"<{escapedPrefix}:(?<tag>\w+)\b(?:""[^""]*""|'[^']*'|[^""'>])*?>[\s\S]*?</{escapedPrefix}:\k<tag>\s*>",
				string.Empty,
				RegexOptions.IgnoreCase);

			// Remove self-closing tags <prefix:Control ... /> (allowing '>' inside quoted attribute values)
			working = Regex.Replace(
				working,
				$@"<{escapedPrefix}:\w+\b(?:""[^""]*""|'[^']*'|[^""'>])*?/>",
				string.Empty,
				RegexOptions.IgnoreCase);
		}

		// Handle DisplaySettings controls that historically used the global 'portal:' prefix
		var extraRegistrations = new StringBuilder();

		foreach (var (controlName, assemblyName, ns, tagPrefix) in _migratedPortalControls)
		{
			if (!working.Contains($"portal:{controlName}"))
			{
				continue;
			}

			if (loadedAssemblies.Contains(assemblyName))
			{
				extraRegistrations.AppendLine($@"<%@ Register Namespace=""{ns}"" Assembly=""{assemblyName}"" TagPrefix=""{tagPrefix}"" %>");
				working = Regex.Replace(
					working,
					$@"\bportal:{Regex.Escape(controlName)}\b",
					$"{tagPrefix}:{controlName}",
					RegexOptions.IgnoreCase);
			}
			else
			{
				working = Regex.Replace(
					working,
					$@"<portal:{Regex.Escape(controlName)}\b(?:""[^""]*""|'[^']*'|[^""'>])*?/>",
					string.Empty,
					RegexOptions.IgnoreCase);
			}
		}

		if (extraRegistrations.Length > 0)
		{
			working = extraRegistrations + working;
		}

		// Restore server-side comments
		for (var i = 0; i < comments.Count; i++)
		{
			working = working.Replace($"__MP_SKIN_COMMENT_{i}__", comments[i]);
		}

		return new MemoryStream(Encoding.UTF8.GetBytes(working));
	}
}
