using System;
using System.Text;
using Newtonsoft.Json;

namespace mojoPortal.Core.Helpers;

public static class StringHelper
{
	public static string ToJsonString(object jsonObj)
	{
		//return JsonSerializer.Serialize(jsonObj);
		return JsonConvert.SerializeObject(jsonObj);
	}

	public static string DecodeBase64String(string base64String, Encoding encoding)
	{
		if (string.IsNullOrWhiteSpace(base64String))
		{
			return base64String;
		}

		byte[] encodedBytes = Convert.FromBase64String(base64String);
		return encoding.GetString(encodedBytes, 0, encodedBytes.Length);
	}

	public static string GetBase64EncodedUnicodeString(string unicodeString)
	{
		Encoding encoding = Encoding.Unicode;
		byte[] bytes = encoding.GetBytes(unicodeString);

		return Convert.ToBase64String(bytes);
	}

	public static string GetBase64EncodedAsciiString(string unicodeString)
	{
		Encoding encoding = Encoding.ASCII;
		byte[] bytes = encoding.GetBytes(unicodeString);

		return Convert.ToBase64String(bytes);
	}

	/// <summary>
	/// Remap International Chars To Ascii
	/// http://meta.stackoverflow.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
	/// </summary>
	/// <param name="c"></param>
	/// <returns></returns>
	public static string RemapInternationalCharToAscii(char c)
	{
		string s = c.ToString().ToLowerInvariant();
		if ("àåáâäãåą".Contains(s))
		{
			return "a";
		}
		else if ("èéêëę".Contains(s))
		{
			return "e";
		}
		else if ("ìíîïı".Contains(s))
		{
			return "i";
		}
		else if ("òóôõöøőð".Contains(s))
		{
			return "o";
		}
		else if ("ùúûüŭů".Contains(s))
		{
			return "u";
		}
		else if ("çćčĉ".Contains(s))
		{
			return "c";
		}
		else if ("żźž".Contains(s))
		{
			return "z";
		}
		else if ("śşšŝ".Contains(s))
		{
			return "s";
		}
		else if ("ñń".Contains(s))
		{
			return "n";
		}
		else if ("ýÿ".Contains(s))
		{
			return "y";
		}
		else if ("ğĝ".Contains(s))
		{
			return "g";
		}
		else if (c == 'ř')
		{
			return "r";
		}
		else if (c == 'ł')
		{
			return "l";
		}
		else if (c == 'đ')
		{
			return "d";
		}
		else if (c == 'ß')
		{
			return "ss";
		}
		else if (c == 'Þ')
		{
			return "th";
		}
		else if (c == 'ĥ')
		{
			return "h";
		}
		else if (c == 'ĵ')
		{
			return "j";
		}
		else
		{
			return string.Empty;
		}
	}
}