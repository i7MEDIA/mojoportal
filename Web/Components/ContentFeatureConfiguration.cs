#nullable enable
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace mojoPortal.Web;

public class ContentFeatureConfiguration
{
	private static readonly ILog log = LogManager.GetLogger(typeof(ContentFeatureConfiguration));

	public List<ContentFeature> ContentFeatures { get; } = [];


	public ContentFeature? GetFeature(Guid featureGuid)
	{
		foreach (ContentFeature feature in ContentFeatures)
		{
			if (feature.FeatureGuid == featureGuid)
			{
				return feature;
			}
		}

		return null;
	}


	public static ContentFeatureConfiguration GetConfig(string applicationFolderName)
	{
		var contentFeatureConfig = new ContentFeatureConfiguration();
		var configFolderName = $"~/Setup/applications/{applicationFolderName}/FeatureDefinitions/";
		var pathToConfigFolder = HttpContext.Current.Server.MapPath(configFolderName);

		if (!Directory.Exists(pathToConfigFolder))
		{
			return contentFeatureConfig;
		}

		var directoryInfo = new DirectoryInfo(pathToConfigFolder);
		var featureFiles = directoryInfo.GetFiles("*.config");

		foreach (var fileInfo in featureFiles)
		{
			var featureConfigFile = XmlHelper.GetXmlDocument(fileInfo.FullName);

			LoadFeature(
				contentFeatureConfig,
				featureConfigFile.DocumentElement);
		}

		return contentFeatureConfig;
	}


	private static void LoadFeature(
		ContentFeatureConfiguration contentFeatureConfig,
		XmlNode node)
	{
		ContentFeature.LoadFeature(contentFeatureConfig, node);
	}
}
