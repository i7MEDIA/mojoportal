using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace mojoPortal.Web.Framework;

public class CombineScripts
{
	private static readonly Regex _findScriptTags = new(@"<script\s*src\s*=\s*""(?<url>.[^""]+)"".[^>]*>\s*</script>", RegexOptions.Compiled);


	/// <summary>
	/// Combine script references using file sets defined in a configuration file.
	/// It will replace multiple script references using one 
	/// </summary>
	public static string CombineScriptBlocks(string scripts)
	{
		var sets = LoadSets();
		var output = scripts;

		foreach (var mapSet in sets)
		{
			var setStartPos = -1;
			var names = new List<string>();

			output = _findScriptTags.Replace(output, new MatchEvaluator(delegate (Match match)
			{
				var url = match.Groups["url"].Value;

				var urlMatch = mapSet.Urls.Find(
					new Predicate<UrlMap>(
						delegate (UrlMap map)
						{
							return map.Url == url;
						}));

				if (null != urlMatch)
				{
					// Remember the first script tag that matched in this UrlMapSet because
					// this is where the combined script tag will be inserted
					if (setStartPos < 0)
					{
						setStartPos = match.Index;
					}

					names.Add(urlMatch.Name);

					return string.Empty;
				}
				else
				{
					return match.Value;
				}

			}));

			if (setStartPos >= 0)
			{
				var setName = string.Empty;

				// if the set says always include all urls within it whenever a single match is found,
				// then generate the full set
				if (mapSet.IsIncludeAll)
				{
					// No need send the individual url names when the full set needs to be included
					setName = string.Empty;
				}
				else
				{
					names.Sort();
					setName = string.Join(",", names.ToArray());
				}

				var urlPrefix = HttpContext.Current.Request.Path.Substring(0, HttpContext.Current.Request.Path.LastIndexOf('/') + 1);
				var newScriptTag = $"<script src=\"scripthandler.ashx?{HttpUtility.UrlEncode(mapSet.Name)}={HttpUtility.UrlEncode(setName)}&amp;{HttpUtility.UrlEncode(urlPrefix)}\"></script>";

				output = output.Insert(setStartPos, newScriptTag);
			}
		}

		return output;
	}

	public static List<UrlMapSet> LoadSets()
	{
		var sets = new List<UrlMapSet>();
		var fullPathToFile = HttpContext.Current.Server.MapPath("~/App_Data/ScriptFileSets.xml");

		if (!File.Exists(fullPathToFile))
		{
			return sets;
		}

		using var streamReader = new StreamReader(fullPathToFile);
		using var reader = XmlHelper.CreateXmlReader(streamReader);

		reader.MoveToContent();

		while (reader.Read())
		{
			if ("set" == reader.Name)
			{
				string setName = reader.GetAttribute("name");
				string isIncludeAll = reader.GetAttribute("includeAll");

				var mapSet = new UrlMapSet
				{
					Name = setName
				};

				if (isIncludeAll == "true")
				{
					mapSet.IsIncludeAll = true;
				}

				while (reader.Read())
				{
					if ("url" == reader.Name)
					{
						var urlName = reader.GetAttribute("name");
						var url = reader.ReadElementContentAsString();

						mapSet.Urls.Add(new UrlMap(urlName, url));
					}
					else if ("set" == reader.Name)
					{
						break;
					}
				}

				sets.Add(mapSet);
			}
		}

		return sets;
	}
}

public class UrlMapSet
{
	public string Name;
	public bool IsIncludeAll;
	public List<UrlMap> Urls = new List<UrlMap>();
}

public class UrlMap
{
	public string Name;
	public string Url;

	public UrlMap(string name, string url)
	{
		this.Name = name;
		this.Url = url;
	}
}
