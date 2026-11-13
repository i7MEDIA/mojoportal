#nullable enable
using log4net;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;
using System.Xml;

namespace mojoPortal.Web;

/// <summary>
/// Defines a Feature that can plug into the CMS
/// </summary>
public class ContentFeature
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(ContentFeature));

	public Guid FeatureGuid { get; set; } = Guid.Empty;
	public string SupportedDatabases { get; set; } = "MSSQL,MySQL,pgsql,SQLite";
	public string ResourceFile { get; set; } = string.Empty;
	public string FeatureNameReasourceKey { get; set; } = string.Empty;
	public string SearchListNameResourceKey { get; set; } = string.Empty;
	public bool IsSearchable { get; set; } = false;
	public bool SupportsPageReuse { get; set; } = true;
	public string DeleteProvider { get; set; } = string.Empty;
	public string PartialView { get; set; } = string.Empty;
	public string ControlSource { get; set; } = string.Empty;
	public int SortOrder { get; set; } = 300;
	public bool IsCacheable { get; set; } = false;
	public int DefaultCacheTime { get; set; } = 0;
	public string Icon { get; set; } = "blank.gif";
	public bool ExcludeFromFeatureList { get; set; } = false;
	public string SkinFileName { get; set; } = string.Empty;

	private Collection<ContentFeatureSetting> _featureSettings = [];
	public Collection<ContentFeatureSetting> Settings
	{
		get
		{
			return _featureSettings;
		}
		set
		{
			_featureSettings = value ?? [];
		}
	}


	public ContentFeature()
	{ }


	public static void LoadFeature(
		ContentFeatureConfiguration contentFeatureConfig,
		XmlNode documentElement)
	{
		if (HttpContext.Current == null || documentElement.Name != "featureDefinitions")
		{
			return;
		}

		foreach (XmlNode node in documentElement.ChildNodes)
		{
			if (node.Name == "featureDefinition")
			{
				var feature = new ContentFeature();

				var attributeCollection = node.Attributes;

				if (attributeCollection["featureGuid"] is not null)
				{
					feature.FeatureGuid = new Guid(attributeCollection["featureGuid"].Value);
				}

				if (attributeCollection["supportedDatabases"] is not null)
				{
					feature.SupportedDatabases = attributeCollection["supportedDatabases"].Value;
				}

				if (attributeCollection["resourceFile"] is not null)
				{
					feature.ResourceFile = attributeCollection["resourceFile"].Value;
				}

				if (attributeCollection["excludeFromFeatureList"] is not null)
				{
					try
					{
						feature.ExcludeFromFeatureList = Convert.ToBoolean(attributeCollection["excludeFromFeatureList"].Value);
					}
					catch { }
				}

				if (attributeCollection["featureNameReasourceKey"] is not null)
				{
					feature.FeatureNameReasourceKey = attributeCollection["featureNameReasourceKey"].Value;
				}

				if (attributeCollection["isSearchable"] is not null)
				{
					try
					{
						feature.IsSearchable = Convert.ToBoolean(attributeCollection["isSearchable"].Value);
					}
					catch { }
				}

				if (attributeCollection["supportsPageReuse"] is not null)
				{
					try
					{
						feature.SupportsPageReuse = Convert.ToBoolean(attributeCollection["supportsPageReuse"].Value);
					}
					catch { }
				}

				if (attributeCollection["isCacheable"] is not null)
				{
					try
					{
						feature.IsCacheable = Convert.ToBoolean(attributeCollection["isCacheable"].Value);
					}
					catch { }
				}

				if (attributeCollection["deleteProvider"] is not null)
				{
					feature.DeleteProvider = attributeCollection["deleteProvider"].Value;
				}

				if (attributeCollection["partialView"] is not null)
				{
					feature.PartialView = attributeCollection["partialView"].Value;
				}

				if (attributeCollection["skinFileName"] is not null)
				{
					feature.SkinFileName = attributeCollection["skinFileName"].Value;
				}


				if (attributeCollection["searchListNameResourceKey"] is not null)
				{
					feature.SearchListNameResourceKey = attributeCollection["searchListNameResourceKey"].Value;
				}

				if (attributeCollection["controlSource"] is not null)
				{
					feature.ControlSource = attributeCollection["controlSource"].Value;
				}

				if (!feature.ControlSource.EndsWith(".ascx"))
				{
					_log.Error($"could not install feature {feature.FeatureNameReasourceKey}. Invalid ControlSource Setting");

					return;
				}

				if (attributeCollection["icon"] is not null)
				{
					feature.Icon = attributeCollection["icon"].Value;
				}

				if (attributeCollection["sortOrder"] is not null)
				{
					if (int.TryParse(attributeCollection["sortOrder"].Value, out int sort))
					{
						feature.SortOrder = sort;
					}
				}

				if (attributeCollection["defaultCacheTime"] is not null)
				{
					if (int.TryParse(attributeCollection["defaultCacheTime"].Value, out int cacheTime))
					{
						feature.DefaultCacheTime = cacheTime;
					}
				}

				foreach (XmlNode featureSettingNode in node.ChildNodes)
				{
					if (featureSettingNode.Name == "featureSetting")
					{
						ContentFeatureSetting.LoadFeatureSetting(
							feature,
							featureSettingNode);
					}
				}

				if (feature.FeatureGuid == Guid.Empty)
				{
					_log.Error($"could not install feature {feature.FeatureNameReasourceKey}. Invalid FeatureGuid.");

					return;
				}

				var controlPath = feature.ControlSource;

				if (!controlPath.StartsWith("/"))
				{
					controlPath = "~/" + controlPath;
				}
				else
				{
					controlPath = "~" + controlPath;
				}

				if (File.Exists(HttpContext.Current.Server.MapPath(controlPath)) || Plugins.PluginFileResolver.FileExists(controlPath))
				{
					contentFeatureConfig.ContentFeatures.Add(feature);
				}
				else
				{
					_log.Error($"could not install feature {feature.FeatureNameReasourceKey}. Invalid ControlSource Setting. Filenot found: {controlPath}");
				}
			}
		}
	}
}
