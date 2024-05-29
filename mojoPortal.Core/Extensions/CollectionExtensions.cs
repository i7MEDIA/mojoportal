using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
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


	/// <summary>
	/// Creates a delimited string from a ListItemCollection
	/// </summary>
	/// <param name="list"></param>
	/// <param name="itemDelimiter"></param>
	/// <param name="addTrailingDelimiter"></param>
	/// <returns>A delimited string of values from the collection passed to it.</returns>
	public static string ToDelimitedString(this ListItemCollection list, string itemDelimiter = ",", bool addTrailingDelimiter = false)
	{
		if (list is null)
		{
			return string.Empty;
		}

		IEnumerable<ListItem> listItems = list.Cast<ListItem>().ToList();

		return string.Join(itemDelimiter, listItems.Where(x => x.Selected).Select(x => x.Value.ToString())) + (addTrailingDelimiter ? itemDelimiter : string.Empty);
	}


	public static DataTable ToDataTable<T>(this List<T> items)
	{
		DataTable dataTable = new(typeof(T).Name);
		//Get all the properties
		PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		foreach (PropertyInfo prop in Props)
		{
			//Setting column names as Property names
			dataTable.Columns.Add(prop.Name);
		}
		foreach (T item in items)
		{
			var values = new object[Props.Length];
			for (int i = 0; i < Props.Length; i++)
			{
				//inserting property values to datatable rows
				values[i] = Props[i].GetValue(item, null);
			}
			dataTable.Rows.Add(values);
		}
		//put a breakpoint here and check datatable
		return dataTable;
	}
}
