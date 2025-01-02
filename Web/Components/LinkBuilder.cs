using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web;

public class LinkBuilder
{
	#region Private Fields

	private static readonly ILog log = LogManager.GetLogger(typeof(LinkBuilder));
	private readonly string _url;
	private readonly NameValueCollection _queryCollection = HttpUtility.ParseQueryString(string.Empty);
	private readonly bool _includeSiteRoot;

	#endregion


	#region Constructors

	public LinkBuilder(string url, bool includeSiteRoot = true)
	{
		url = ParseAndRemoveQueryParamsFromUrlString(url);
		_url = CombinePaths(ParsePath(url));
		log.Debug($"Public CTOR Include In Site Root1?: {includeSiteRoot}");
		_includeSiteRoot = EnsureSiteRoot(includeSiteRoot);
		log.Debug($"Public CTOR URL: {url}");
		log.Debug($"Public CTOR Include In Site Root2?: {_includeSiteRoot}");
	}


	private LinkBuilder(string[] urls, bool includeSiteRoot = true)
	{
		// Clean out any query parameters
		urls = urls.Select(x => x.Contains("?") ? x.Remove(x.IndexOf('?'), x.IndexOf('?') - x.Length) : x).ToArray();

		(_, var paths) = ParsePaths(urls);

		_url = CombinePaths(paths);
		log.Debug($"Private CTOR Include In Site Root1?: {includeSiteRoot}");
		_includeSiteRoot = EnsureSiteRoot(includeSiteRoot);
		log.Debug($"Private CTOR URL: {_url}");
		log.Debug($"Private CTOR Include In Site Root2?: {_includeSiteRoot}");
	}

	#endregion


	#region Query Methods

	public LinkBuilder PageId(int id)
	{
		_queryCollection.Set("pageid", id.ToString());

		return this;
	}


	public LinkBuilder ModuleId(int id)
	{
		_queryCollection.Set("mid", id.ToString());

		return this;
	}


	public LinkBuilder SiteId(int id)
	{
		_queryCollection.Set("siteid", id.ToString());

		return this;
	}


	public LinkBuilder ItemId(int id)
	{
		_queryCollection.Set("itemid", id.ToString());

		return this;
	}


	public LinkBuilder PageNumber(int pageNumber)
	{
		_queryCollection.Set("pagenumber", pageNumber.ToString());

		return this;
	}


	public LinkBuilder PageNumber(string pageNumber)
	{
		_queryCollection.Set("pagenumber", pageNumber);

		return this;
	}


	public LinkBuilder ReturnUrl(string returnUrl)
	{
		_queryCollection.Set("returnurl", returnUrl);

		return this;
	}


	public LinkBuilder AddParam(string key, object value)
	{
		_queryCollection.Add(key, value.ToString());

		return this;
	}


	public LinkBuilder AddParams(Dictionary<string, object> @params, bool @override = false)
	{
		foreach (var @param in @params)
		{
			if (@override)
			{
				_queryCollection.Set(@param.Key, @param.Value.ToString());
			}
			else
			{
				_queryCollection.Add(@param.Key, @param.Value.ToString());
			}
		}

		return this;
	}


	/// <summary>
	/// Sets query parameter to passed value. This will override any previously set value.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns>LinkBuilder</returns>
	public LinkBuilder SetParam(string key, object value)
	{
		_queryCollection.Set(key, value.ToString());

		return this;
	}

	#endregion


	public Uri ToUri()
	{
		try
		{
			log.Debug($"Site Root: {SiteUtils.GetNavigationSiteRoot()}");
			log.Debug($"Include Site Root?: {_includeSiteRoot}");

			if (
				_includeSiteRoot &&
				Uri.TryCreate(SiteUtils.GetNavigationSiteRoot(), UriKind.Absolute, out var baseUrl) &&
				Uri.TryCreate(_url, UriKind.Relative, out var urlPath) &&
				Uri.TryCreate(baseUrl, urlPath + GetQueryString(), out var fullSiteUri)
			)
			{
				log.Debug($"Full Site URL BaseURL: {baseUrl}");
				log.Debug($"Full Site URL URLPath: {urlPath}");
				log.Debug($"Full Site URL: {fullSiteUri}");

				return new UriBuilder(fullSiteUri)
				{
					Query = _queryCollection.ToString(),
				}.Uri;
			}

			_ = Uri.TryCreate(_url + GetQueryString(), UriKind.Absolute, out Uri result) ||
				Uri.TryCreate(_url + GetQueryString(), UriKind.Relative, out result);

			log.Debug($"Relative or External URL: {result}");

			return result;
		}
		catch (Exception e)
		{
			log.Error(e);
			return null;
		}
	}


	public override string ToString()
	{
		//string siteRoot = string.Empty;

		//if (includeSiteRoot)
		//{
		//	siteRoot = SiteUtils.GetNavigationSiteRoot();
		//}

		//string qmark = queries.Count > 0 ? "?" : string.Empty;
		////return string.Format(CultureInfo.InvariantCulture, $"{siteRoot}/{url.TrimStart('~', '/')}{qmark}{queries.ToDelimitedString()}".TrimStart('/'));
		//return $"{siteRoot}/{url?.TrimStart('~', '/')}{qmark}{queries.ToDelimitedString()}".TrimStart('/');

		return ToUri()?.ToString() ?? string.Empty;
	}


	/// <summary>
	/// Creates a full path for files in the selected skin
	/// </summary>
	/// <param name="path">Path to file relative to the current skin</param>
	public static LinkBuilder SkinUrl(string path, Page page, bool includeSkinVersion = true)
	{
		var skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(true, page);
		var builder = new LinkBuilder([skinBaseUrl, path]);

		if (includeSkinVersion)
		{
			var skinVersion = Guid.Empty;

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				skinVersion = siteSettings.SkinVersion;
			}

			builder.SetParam("v", skinVersion);
		}

		return builder;
	}


	#region Private Utilities

	private bool EnsureSiteRoot(bool includeSiteRoot)
	{
		var siteRoot = SiteUtils.GetNavigationSiteRoot();
		var urlContainsSiteRoot = _url.Contains(siteRoot);

		log.Debug($"EnsureSiteRoot SiteRoot: {siteRoot}");
		log.Debug($"EnsureSiteRoot UrlContainsSiteRoot: {urlContainsSiteRoot}");

		if (includeSiteRoot && urlContainsSiteRoot)
		{
			includeSiteRoot = false;
		}

		return includeSiteRoot;
	}


	private string ParseAndRemoveQueryParamsFromUrlString(string url, bool removeDuplicates = true)
	{
		var queryIndex = url.IndexOf("?");

		if (queryIndex > -1)
		{
			var queryString = url.Substring(queryIndex + 1, url.Length - (queryIndex + 1));
			var queryCollection = HttpUtility.ParseQueryString(queryString);

			if (removeDuplicates)
			{
				foreach (KeyValuePair<string, string> item in queryCollection)
				{
					_queryCollection.Set(item.Key, item.Value);
				}
			}
			else
			{
				foreach (KeyValuePair<string, string> item in queryCollection)
				{
					_queryCollection.Add(item.Key, item.Value);
				}
			}

			url = url.Substring(0, url.IndexOf('?'));
		}

		return url;
	}


	private string GetQueryString()
	{
		if (_queryCollection.Count > 0)
		{
			return "?" + _queryCollection.ToString();
		}

		return string.Empty;
	}


	/// <summary>
	/// Parses a string array of paths.
	/// </summary>
	/// <param name="paths">Array of paths to parse</param>
	/// <returns>Returns the first base URL in the array or an empty string, and a flattened string array of path directories.</returns>
	private static (string, string[]) ParsePaths(params string[] paths)
	{
		var urlBase = string.Empty;
		var pathList = new List<string>();

		foreach (var path in paths)
		{
			var (_urlBase, _paths) = ParsePath(path);

			pathList.AddRange(_paths);

			if (string.IsNullOrWhiteSpace(urlBase) && !string.IsNullOrWhiteSpace(_urlBase))
			{
				urlBase = _urlBase;
			}
		}

		return (urlBase, pathList.ToArray());
	}

	/// <summary>
	/// Parses a path string.
	/// </summary>
	/// <param name="path"></param>
	/// <returns>Returns the base URL or an empty string, and a string array of path directories.</returns>
	private static (string, string[]) ParsePath(string path)
	{
		var urlBase = string.Empty;

		// double leading slashes causes Uri to create a file URI e.g.: "file://something", so trim leading slashes
		if (Uri.TryCreate(path.TrimStart('/'), UriKind.Absolute, out var uri))
		{
			var _urlBase = uri.GetLeftPart(UriPartial.Authority);

			if (!string.IsNullOrWhiteSpace(_urlBase))
			{
				var index = path.IndexOf(path);

				path = path.Remove(index, _urlBase.Length);
				urlBase = _urlBase;
			}
		}

		return (urlBase, path.Split(new char[] { '/', '\\', '~' }, StringSplitOptions.RemoveEmptyEntries));
	}

	/// <summary>
	/// Turns a tuple of a base URL and a string array into a URL path.
	/// </summary>
	/// <param name="s"></param>
	/// <returns>Returns a URL if a base URL was provided, or a URL path with a leading slash if not.</returns>
	private static string CombinePaths((string urlBase, string[] paths) s) => CombinePaths(s.urlBase, s.paths);

	/// <summary>
	/// Turns a base URL and a string array into a URL path.
	/// </summary>
	/// <param name="urlBase">A base URL string, E.G.: "https://google.com"</param>
	/// <param name="paths">A string array of path directories to be combined.</param>
	/// <returns>Returns a URL if a base URL was provided, or a URL path with a leading slash if not.</returns>
	private static string CombinePaths(string urlBase, string[] paths) => $"{urlBase}/{CombinePaths(paths).TrimStart('/')}";

	/// <summary>
	/// Turns a string array into a URL path.
	/// </summary>
	/// <param name="paths">A string array of path directories to be combined.</param>
	/// <returns>Returns a URL path with a leading slash.</returns>
	private static string CombinePaths(params string[] paths) => paths.Aggregate(string.Empty, (c, p) => $"{c.TrimEnd('/')}/{p.TrimStart('/')}");

	#endregion
}


public static class LinkBuilderExtensions
{
	public static LinkBuilder ToLinkBuilder(this string str, bool includeSiteRoot = true)
	{
		return new LinkBuilder(str, includeSiteRoot);
	}

	public static LinkBuilder ToLinkBuilder(this Uri uri, bool includeSiteRoot = false)
	{
		return new LinkBuilder(uri.AbsolutePath, includeSiteRoot)
			.AddParams((Dictionary<string, object>)uri.ParseQueryString().ToDictionary());
	}
}
