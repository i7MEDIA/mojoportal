using System.Collections.Generic;
using System.Web;

namespace mojoPortal.Web;

public class LinkBuilder
{
	private readonly string _url;
	private readonly Dictionary<string, object> _queries = [];
	private readonly bool _includeSiteRoot;


	public LinkBuilder(string url, bool includeSiteRoot = true)
	{
		_url = url;

		if (_url.Contains("?"))
		{
			var query = HttpUtility.ParseQueryString(_url);

			foreach (KeyValuePair<string, string> item in query)
			{
				_queries[item.Key] = item.Value;
			}

			_url = _url.Substring(0, _url.IndexOf('?'));
		}

		if (includeSiteRoot && _url.Contains(SiteUtils.GetNavigationSiteRoot()))
		{
			includeSiteRoot = false;
		}

		_includeSiteRoot = includeSiteRoot;
	}


	public LinkBuilder PageId(int id)
	{
		_queries.Add("pageid", id);

		return this;
	}


	public LinkBuilder ModuleId(int id)
	{
		_queries.Add("mid", id);

		return this;
	}


	public LinkBuilder SiteId(int id)
	{
		_queries.Add("siteid", id);

		return this;
	}


	public LinkBuilder ItemId(int id)
	{
		_queries.Add("itemid", id);

		return this;
	}


	public LinkBuilder ReturnUrl(string returnUrl)
	{
		_queries.Add("returnurl", UrlEncode(returnUrl)); //UrlEncode prevents querystring from being used as vector for XSS

		return this;
	}


	public LinkBuilder AddParam(string key, object value)
	{
		_queries.Add(key, UrlEncode(value.ToString())); //UrlEncode prevents querystring from being used as vector for XSS

		return this;
	}


	public LinkBuilder AddParams(Dictionary<string, object> @params)
	{
		foreach (var @param in @params)
		{
			_queries.Add(@param.Key, UrlEncode(@param.Value.ToString())); //UrlEncode prevents querystring from being used as vector for XSS
		}

		return this;
	}


	public override string ToString()
	{
		string siteRoot = string.Empty;

		if (_includeSiteRoot)
		{
			siteRoot = SiteUtils.GetNavigationSiteRoot();
		}

		string qmark = _queries.Count > 0 ? "?" : string.Empty;

		//return string.Format(CultureInfo.InvariantCulture, $"{siteRoot}/{url.TrimStart('~', '/')}{qmark}{queries.ToDelimitedString()}".TrimStart('/'));
		return $"{siteRoot}/{_url?.TrimStart('~', '/')}{qmark}{_queries.ToDelimitedString()}".TrimStart('/');
	}
}


public static class LinkBuilderExtensions
{
	public static LinkBuilder ToLinkBuilder(this string str, bool includeSiteRoot = true)
	{
		return new LinkBuilder(str, includeSiteRoot);
	}
}