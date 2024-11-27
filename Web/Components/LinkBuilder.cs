using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace mojoPortal.Web;

public class LinkBuilder
{
	private readonly string url;
	private readonly Dictionary<string, object> queries = [];
	private readonly bool includeSiteRoot;

	public LinkBuilder(string url, bool includeSiteRoot = true)
	{
		this.url = url;
		if (this.url.Contains("?"))
		{
			var query = HttpUtility.ParseQueryString(this.url);
			foreach (KeyValuePair<string, string> item in query)
			{
				queries[item.Key] = item.Value;
			}
			this.url = this.url.Substring(0, this.url.IndexOf('?'));
		}

		if (includeSiteRoot && this.url.Contains(SiteUtils.GetNavigationSiteRoot()))
		{
			includeSiteRoot = false;
		}

		this.includeSiteRoot = includeSiteRoot;
	}
	public LinkBuilder PageId(int id)
	{
		queries.Add("pageid", id);
		return this;
	}
	public LinkBuilder ModuleId(int id)
	{
		queries.Add("mid", id);
		return this;
	}
	public LinkBuilder SiteId(int id)
	{
		queries.Add("siteid", id);
		return this;
	}
	public LinkBuilder ItemId(int id)
	{
		queries.Add("itemid", id);
		return this;
	}
	public LinkBuilder PageNumber(int pageNumber)
	{
		queries.Add("pagenumber", pageNumber);
		return this;
	}
	public LinkBuilder PageNumber(string pageNumber)
	{
		queries.Add("pagenumber", pageNumber);
		return this;
	}
	public LinkBuilder ReturnUrl(string returnUrl)
	{
		queries.Add("returnurl", UrlEncode(returnUrl)); //UrlEncode prevents querystring from being used as vector for XSS
		return this;
	}

	public LinkBuilder AddParam(string key, object value)
	{
		queries.Add(key, UrlEncode(value.ToString())); //UrlEncode prevents querystring from being used as vector for XSS
		return this;
	}

	public LinkBuilder AddParams(Dictionary<string, object> @params)
	{
		foreach (var @param in @params)
		{
			queries.Add(@param.Key, UrlEncode(@param.Value.ToString())); //UrlEncode prevents querystring from being used as vector for XSS
		}
		return this;
	}

	/// <summary>
	/// Sets existing parameter to passed value if the parameter exists. If the parameter is not found, creates a new parameter with the passed value.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns>LinkBuilder</returns>
	public LinkBuilder SetParam(string key, object value)
	{
		if (queries.ContainsKey(key))
		{
			queries[key] = UrlEncode(value.ToString()); //UrlEncode prevents querystring from being used as vector for XSS
			return this;
		}
		else
		{
			return AddParam(key, value);
		}
	}

	public Uri ToUri()
	{
		var queryString = HttpUtility.ParseQueryString(string.Empty);

		foreach (var query in queries)
		{
			queryString.Add(query.Key, query.Value.ToString());
		}

		var path = url.TrimStart('~') + (queryString.Count > 0 ? $"?{queryString}" : string.Empty);

		if (includeSiteRoot)
		{
			return new Uri(new Uri(SiteUtils.GetNavigationSiteRoot(), UriKind.Absolute), new Uri(path, UriKind.Relative));
		}

		try
		{
			return new Uri(path);
		}
		catch (UriFormatException)
		{
			return new Uri(new Uri(SiteUtils.GetNavigationSiteRoot(), UriKind.Absolute), new Uri(path, UriKind.Relative));
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

		return ToUri().ToString();
	}
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