using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;

namespace mojoPortal.Core.Extensions;

public static class CollectionExtensions
{
	/// <summary>
	///     A NameValueCollection extension method that converts the @this to a dictionary.
	/// </summary>
	/// <param name="this">The @this to act on.</param>
	/// <returns>@this as an IDictionary&lt;string,object&gt;</returns>
	public static IDictionary<string, object> ToDictionary(this NameValueCollection @this)
	{
		var dict = new Dictionary<string, object>();

		if (@this != null)
		{
			foreach (string key in @this.AllKeys)
			{
				dict.Add(key, @this[key]);
			}
		}

		return dict;
	}


	public static string ToDelimitedString<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, string kvpDelimiter = "=", string itemDelimiter = "&")
	{
		if (dict == null)
		{
			return string.Empty;
		}

		return string.Join(itemDelimiter, dict.Select(x => x.Key.ToString() + kvpDelimiter + x.Value.ToString()));
	}


	public static string ToDelimitedString(this ListItemCollection list, string itemDelimiter = ",", bool addTrailingDelimiter = false)
	{
		if (list == null)
		{
			return string.Empty;
		}

		IEnumerable<ListItem> listItems = list.Cast<ListItem>().ToList();

		return string.Join(itemDelimiter, listItems.Where(x => x.Selected).Select(x => x.Value.ToString())) + (addTrailingDelimiter ? itemDelimiter : string.Empty);
	}
}
