using System;
using System.Collections;
using System.Globalization;

namespace mojoPortal.Core.Extensions;

public static class HashtableExtensions
{
	public static string ParseString(this Hashtable table, string key, string defaultIfNotFound = "")
	{
		if (table.Contains(key))
		{
			return table[key].ToString().Trim();
		}
		return defaultIfNotFound;
	}

	public static int ParseInt32(this Hashtable table, string key, int defaultIfNotFound)
	{
		int returnValue = defaultIfNotFound;
		if (table is null) { return returnValue; }

		if ((key is not null)
			&& table.Contains(key)
			&& (!int.TryParse(table[key].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out returnValue))
			)
		{
			returnValue = defaultIfNotFound;
		}

		return returnValue;
	}

	public static double ParseDouble(this Hashtable table, string key, double defaultIfNotFound)
	{
		double returnValue = defaultIfNotFound;

		if ((key is not null)
			&& table.Contains(key)
			&& (!double.TryParse(
			table[key].ToString(),
			NumberStyles.Any,
			CultureInfo.InvariantCulture,
			out returnValue))
			)
		{
			returnValue = defaultIfNotFound;
		}

		return returnValue;
	}

	public static bool ParseBool(this Hashtable table, string key, bool defaultIfNotFound)
	{
		bool returnValue = defaultIfNotFound;

		if (
			(table is not null)
			&& (key is not null)
			&& table.Contains(key)
			)
		{
			if (string.Equals(table[key].ToString(), "true", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}

		return returnValue;
	}

	public static Guid ParseGuidFromHashTable(this Hashtable table, string key, Guid defaultIfNotFoundOrInvalid)
	{
		Guid returnValue = defaultIfNotFoundOrInvalid;

		if (
			(table is not null)
			&& (key is not null)
			&& table.Contains(key)
			)
		{
			string foundSetting = table[key].ToString();
			if (foundSetting.Length == 36)
			{
				returnValue = new Guid(foundSetting);
			}
		}
		return returnValue;
	}
}
