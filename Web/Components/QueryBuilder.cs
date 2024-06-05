using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Web;

namespace mojoPortal.Web;

public class QueryBuilder
{
	private string url;
	private Dictionary<string, object> queries = [];
	private bool includeSiteRoot;

	public QueryBuilder(string url, bool includeSiteRoot = true)
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
	public QueryBuilder PageId(int id)
	{
		queries.Add("pageid", id);
		return this;
	}
	public QueryBuilder ModuleId(int id)
	{
		queries.Add("mid", id);
		return this;
	}
	public QueryBuilder SiteId(int id)
	{
		queries.Add("siteid", id);
		return this;
	}
	public QueryBuilder ItemId(int id)
	{
		queries.Add("itemid", id);
		return this;
	}

	public QueryBuilder AddParam(string key, object value)
	{
		queries.Add(key, value);
		return this;
	}

	public QueryBuilder AddParams(Dictionary<string, object> @params)
	{
		foreach (var @param in @params)
		{
			queries.Add(@param.Key, @param.Value);
		}
		return this;
	}

	public override string ToString()
	{
		string siteRoot = string.Empty;

		if (includeSiteRoot)
		{
			siteRoot = SiteUtils.GetNavigationSiteRoot();
		}

		string qmark = queries.Count > 0 ? "?" : string.Empty;
		//return string.Format(CultureInfo.InvariantCulture, $"{siteRoot}/{url.TrimStart('~', '/')}{qmark}{queries.ToDelimitedString()}".TrimStart('/'));
		return $"{siteRoot}/{url?.TrimStart('~', '/')}{qmark}{queries.ToDelimitedString()}".TrimStart('/');
	}
}

public static class QueryBuilderExtensions
{
	public static QueryBuilder ToQueryBuilder(this string str, bool includeSiteRoot = true)
	{
		return new QueryBuilder(str, includeSiteRoot);
	}
}