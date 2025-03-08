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

	private readonly Uri _uri = new(string.Empty, UriKind.RelativeOrAbsolute);
	private readonly NameValueCollection _queryCollection = HttpUtility.ParseQueryString(string.Empty);

	#endregion


	#region Constructors

	public LinkBuilder(string url, bool includeSiteRoot = true)
	{
		url = ParseAndRemoveQueryParamsFromUrlString(url.Trim());

		var urlParsed = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri);

		if (!urlParsed)
		{
			return;
		}

		(var urlAuthority, var urlPaths) = ParsePath(url);
		var siteRootPaths = Array.Empty<string>();

		if (includeSiteRoot && !uri.IsAbsoluteUri)
		{
			var siteRoot = SiteUtils.GetNavigationSiteRoot();
			var siteRootUri = new Uri(siteRoot);
			urlAuthority = siteRootUri.GetLeftPart(UriPartial.Authority);
			(_, siteRootPaths) = ParsePath(siteRootUri.AbsolutePath);
		}

		if (!includeSiteRoot)
		{
			urlAuthority = string.Empty;
		}

		var cleanUrl = CombinePaths(urlAuthority, [.. siteRootPaths, .. urlPaths]);

		_uri = new(cleanUrl, UriKind.RelativeOrAbsolute);
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


	public LinkBuilder AddParams(NameValueCollection collection)
	{
		foreach (KeyValuePair<string, string> item in collection)
		{
			_queryCollection.Add(item.Key, item.Value);
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


	#region Public Methods

	public Uri ToUri()
	{
		return new(
			_uri.IsAbsoluteUri ?
				_uri.AbsoluteUri + GetQueryString() :
				_uri.OriginalString + GetQueryString()
			,
			UriKind.RelativeOrAbsolute
		);
	}


	public override string ToString()
	{
		return ToUri()?.ToString() ?? string.Empty;
	}


	/// <summary>
	/// Creates a full path for files in the selected skin
	/// </summary>
	/// <param name="path">Path to file relative to the current skin</param>
	public static LinkBuilder SkinUrl(string path, Page page, bool includeSkinVersion = true)
	{
		var skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(true, page);
		string[] paths = [skinBaseUrl, path];

		static string RemoveQueryString(string path)
		{
			if (path.Contains("?"))
			{
				return path.Remove(path.IndexOf('?'), path.IndexOf('?') - path.Length);
			}

			return path;
		}

		// Clean out any query parameters
		paths = paths.Select(RemoveQueryString).ToArray();

		var url = CombinePaths(ParsePaths(paths));
		var builder = new LinkBuilder(url);

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

	#endregion


	#region Private Utilities

	private string ParseAndRemoveQueryParamsFromUrlString(string url, bool removeDuplicates = true)
	{
		var queryIndex = url.IndexOf('?');

		if (queryIndex > -1)
		{
			var queryString = url.Substring(queryIndex + 1, url.Length - (queryIndex + 1));
			var queryCollection = HttpUtility.ParseQueryString(queryString);

			if (removeDuplicates)
			{
				foreach (var item in queryCollection.AllKeys)
				{
					_queryCollection.Set(item, queryCollection[item]);
				}
			}
			else
			{
				foreach (var item in queryCollection.AllKeys)
				{
					_queryCollection.Add(item, queryCollection[item]);
				}
			}

			url = url.Substring(0, queryIndex);
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

		path = path
			.Replace("../", string.Empty)
			.Replace("..\\", string.Empty);

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

		return (urlBase, path.Split(['/', '\\', '~'], StringSplitOptions.RemoveEmptyEntries));
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
		return new LinkBuilder(uri.AbsoluteUri, includeSiteRoot)
			.AddParams(uri.ParseQueryString());
	}
}
