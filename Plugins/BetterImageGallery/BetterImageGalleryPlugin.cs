using System;
using System.Collections.Generic;
using System.Web.Http;
using mojoPortal.Web;
using mojoPortal.Web.Plugins;

namespace mojoPortal.Plugins.BetterImageGallery;

public class BetterImageGalleryPlugin : PluginBase
{
	public static readonly Guid FeatureGuid = new("DEB8288C-59D8-49C2-8405-D49E9CBDBF9F");

	public override Guid Id => FeatureGuid;
	public override string Name => "BetterImageGallery";
	public override string LegacyApplicationName => "BetterImageGallery";
	public override Version Version => new(1, 0, 0, 0);

	public override void RegisterWebApi(HttpConfiguration config)
	{
		if (!config.Routes.ContainsKey("BIG"))
		{
			config.Routes.MapHttpRoute(
				name: "BIG",
				routeTemplate: "api/BetterImageGallery/",
				defaults: new { controller = "BetterImageGallery" }
			);
		}
	}

	public override IEnumerable<string> GetViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/BetterImageGallery/Views/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public override IEnumerable<string> GetPartialViewLocationFormats(string pluginFolderName) =>
	[
		$"~/Plugins/{pluginFolderName}/Views/{{1}}/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/BetterImageGallery/Views/{{0}}.cshtml",
		$"~/Plugins/{pluginFolderName}/Views/Shared/{{0}}.cshtml"
	];

	public override IEnumerable<ContentFeature> GetFeatures()
	{
		var feature = new ContentFeature
		{
			FeatureGuid = FeatureGuid,
			SupportedDatabases = "MSSQL,MySQL,pgsql,SQLite,SqlAzure",
			ResourceFile = "BetterImageGalleryResources",
			FeatureNameReasourceKey = "FeatureName",
			ControlSource = "BetterImageGallery/BetterImageGalleryModule.ascx",
			SortOrder = 545,
			IsCacheable = true,
			DefaultCacheTime = 0,
			ExcludeFromFeatureList = false,
			Icon = "blank.gif"
		};

		feature.Settings.Add(new ContentFeatureSetting
		{
			ControlSrc = "~/BetterImageGallery/LayoutSelector.ascx",
			ControlType = "ISettingControl",
			DefaultValue = "_BetterImageGallery",
			RegexValidationExpression = string.Empty,
			ResourceFile = "BetterImageGalleryResources",
			ResourceKey = "Layout",
			SortOrder = 120
		});

		feature.Settings.Add(new ContentFeatureSetting
		{
			ControlSrc = "~/Controls/FilePathSetting.ascx",
			ControlType = "CustomField",
			DefaultValue = string.Empty,
			RegexValidationExpression = string.Empty,
			ResourceFile = "BetterImageGalleryResources",
			ResourceKey = "FolderGalleryPath",
			SortOrder = 130,
			Attributes = "type|folder;startFolder|BetterImageGallery;returnFullPath|false;"
		});

		return [feature];
	}
}
