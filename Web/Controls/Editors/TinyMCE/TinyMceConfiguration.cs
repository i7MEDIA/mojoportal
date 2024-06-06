using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web.UI;
using System.Xml;
using log4net;
using mojoPortal.Core.Extensions;
using Newtonsoft.Json;

namespace mojoPortal.Web.Editor;

//todo: consolidate TinyMceConfiguration and TinyMceSettings
public class TinyMceConfiguration
{
	private static readonly ILog log = LogManager.GetLogger(typeof(TinyMceConfiguration));

	private TinyMceConfiguration() { }

	private static readonly List<TinyMceSettings> EditorDefinitions = [];

	public static TinyMceConfiguration GetConfig()
	{
		var config = new TinyMceConfiguration();

		var configPath = ConfigHelper.GetStringProperty("TinyMCE:ConfigFile", "~/tinymce.json");

		try
		{
			if (HttpRuntime.Cache["mojoTinyConfiguration"] is not null and TinyMceConfiguration)
			{
				return (TinyMceConfiguration)HttpRuntime.Cache["mojoTinyConfiguration"];
			}

			if (Global.SkinConfig.EditorConfig.TryGetValue("TinyMCE", out var editorConfig))
			{
				var page = HttpContext.Current?.Handler as Page;
				configPath = editorConfig.ConfigPath
					.Coalesce(configPath)
					.Replace("$SkinPath$", SiteUtils.DetermineSkinBaseUrl(true, page));
			}

			try
			{
				var configFile = new FileInfo(HostingEnvironment.MapPath(configPath));

				if (configFile.Exists)
				{
					var content = File.ReadAllText(configFile.FullName);
					JsonConvert.PopulateObject(content, EditorDefinitions);
				}

			}
			catch(Exception ex)
			{
				log.Error($"Could not parse tinymce configuration from {configPath}. Error was {ex}");
			}

			if (Global.SkinConfig.EditorConfig.TryGetValue("all", out var allEditorConfig))
			{
				foreach (var editor in EditorDefinitions)
				{
					var cssClassess = editor.EditorBodyCssClass.SplitOnChar(' ');
					var allEditorCssClasses = allEditorConfig.ContainerCssClass.SplitOnChar(' ');

					editor.EditorBodyCssClass = editor.EditorBodyCssClass.Union(allEditorConfig.ContainerCssClass,' ');
				}
			}

			var aggregateCacheDependency = new AggregateCacheDependency();

			var pathToWebConfig = HostingEnvironment.MapPath("~/Web.config");

			aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

			HttpRuntime.Cache.Insert(
				"mojoTinyConfiguration",
				config,
				aggregateCacheDependency,
				DateTime.Now.AddYears(1),
				TimeSpan.Zero,
				CacheItemPriority.Default,
				null
			);

			return (TinyMceConfiguration)HttpRuntime.Cache["mojoTinyConfiguration"];
		}

		catch (HttpException ex)
		{
			log.Error(ex);
		}
		catch (XmlException ex)
		{
			log.Error(ex);
		}
		catch (ArgumentException ex)
		{
			log.Error(ex);
		}
		catch (NullReferenceException ex)
		{
			log.Error(ex);
		}

		return config;
	}

	public TinyMceSettings GetEditorSettings(string name)
	{
		foreach (TinyMceSettings t in EditorDefinitions)
		{
			if (t.Name == name) { return t; }
		}

		log.Error($"could not load requested editor settings for editor definition {name}");
		return new TinyMceSettings();
	}

	internal static void ClearCache()
	{
		HttpRuntime.Cache.Remove("mojoTinyConfiguration");
	}
}