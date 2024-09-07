using mojoPortal.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace mojoPortal.Core.Extensions;

public static class StringExtensions
{
	public static string ToInvariantString(this int i, string format = null) => format is null ? i.ToString(CultureInfo.InvariantCulture) : i.ToString(format, CultureInfo.InvariantCulture);

	public static string ToInvariantString(this float i, string format = null) => format is null ? i.ToString(CultureInfo.InvariantCulture) : i.ToString(format, CultureInfo.InvariantCulture);

	public static string ToInvariantString(this decimal i, string format = null) => format is null ? i.ToString(CultureInfo.InvariantCulture) : i.ToString(format, CultureInfo.InvariantCulture);

	// Reworked this to follow the .NET Core override method
	public static bool Contains(this string source, string value, StringComparison comparison)
	{
		return source.IndexOf(value, comparison) >= 0;
	}

	public static bool IsCaseInsensitiveMatch(this string str1, string str2) => string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);

	public static string ToSerialDate(this string s)
	{
		if (s.Length != 8)
		{
			return s;
		}

		return s.Substring(0, 4) + "-" + s.Substring(4, 2) + "-" + s.Substring(6, 2);
	}

	public static string EncodeHtml(this string s) => s?.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");

	public static string HtmlEscapeQuotes(this string s) => s?.Replace("'", "&#39;").Replace("\"", "&#34;");

	public static string RemoveCDataTags(this string s) => s.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty);

	public static string CsvEscapeQuotes(this string s) => s.Replace("\"", "\"\"");

	public static string RemoveAngleBrackets(this string s) => s.Remove(["<", ">"]);

	public static string RemovePunctuation(this string s) => s.Remove(".", ",", ":", "?", "!", ";", "&", "{", "}", "[", "]");

	public static string Remove(this string s, string str) => s.Remove([str]);

	public static string Remove(this string s, params char[] chars)
	{
		if (!string.IsNullOrWhiteSpace(s) && chars is not null)
		{
			foreach (var c in chars)
			{
				s = s.Replace(c.ToString(), string.Empty);
			}
		}
		return s;
	}

	public static string Remove(this string s, params string[] strings)
	{
		if (!string.IsNullOrWhiteSpace(s) && strings is not null)
		{
			foreach (var str in strings)
			{
				s = s.Replace(str, string.Empty);
			}
		}
		return s;
	}


	/// <summary>
	/// Evaluates the string and the argument (alt), returns the first one that is not null or whitespace.
	/// </summary>
	/// <param name="s"></param>
	/// <param name="alt"></param>
	/// <returns></returns>
	public static string Coalesce(this string s, string alt) => string.IsNullOrWhiteSpace(s) ? alt : s;


	public static string ToHtmlLineEndings(this string text)
	{
		text = HttpUtility.HtmlEncode(text);
		text = Regex.Replace(text, "\n\n", "<p />");
		text = Regex.Replace(text, "\n", "<br />");

		return text;
	}


	public static string ToTextLineEndings(this string text)
	{
		text = Regex.Replace(text, "<p>", "\n\n");
		text = Regex.Replace(text, "<br />", "\n");

		return text;
	}



	/// <summary>
	/// Converts a unicode string into its closest ascii equivalent
	/// </summary>
	/// <param name="s"></param>
	/// <returns></returns>
	public static string ToAscii(this string s)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			return s;
		}

		try
		{
			string normalized = s.Normalize(NormalizationForm.FormKD);

			Encoding ascii = Encoding.GetEncoding(
				  "us-ascii",
				  new EncoderReplacementFallback(string.Empty),
				  new DecoderReplacementFallback(string.Empty));

			byte[] encodedBytes = new byte[ascii.GetByteCount(normalized)];
			int numberOfEncodedBytes = ascii.GetBytes(normalized, 0, normalized.Length,
			encodedBytes, 0);

			return ascii.GetString(encodedBytes);
		}
		catch
		{
			return s;
		}
	}

	/// <summary>
	/// Converts a unicode string into its closest ascii equivalent.
	/// If the ascii encode string length is less than or equal to 1 returns the original string
	/// as this means the string is probably in a language with no ascii equivalents
	/// </summary>
	/// <param name="s"></param>
	/// <returns></returns>
	public static string ToAsciiIfPossible(this string s)
	{
		if (string.IsNullOrWhiteSpace(s)) { return s; }

		//http://www.mojoportal.com/Forums/Thread.aspx?thread=8974&mid=34&pageid=5&ItemID=9
		s = s.Replace("æ", "ae");
		s = s.Replace("Æ", "ae");
		s = s.Replace("å", "aa");
		s = s.Replace("Å", "aa");

		// based on:
		//http://www.mojoportal.com/Forums/Thread.aspx?thread=9176&mid=34&pageid=5&ItemID=9&pagenumber=1#post38114

		int len = s.Length;
		var sb = new StringBuilder(len);
		char c;

		for (int i = 0; i < len; i++)
		{
			c = s[i];

			if ((int)c >= 128)
			{
				sb.Append(StringHelper.RemapInternationalCharToAscii(c));

			}
			else
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}

	public static string RemoveNonNumeric(this string s)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			return s;
		}

		char[] result = new char[s.Length];
		int resultIndex = 0;
		foreach (char c in s)
		{
			if (char.IsNumber(c))
				result[resultIndex++] = c;
		}
		if (0 == resultIndex)
		{
			s = string.Empty;
		}
		else if (result.Length != resultIndex)
		{
			s = new string(result, 0, resultIndex);
		}

		return s;
	}

	public static string RemoveLineBreaks(this string s, string Replacement = "")
	{
		if (string.IsNullOrWhiteSpace(s)) { return s; }

		return s.Replace("\r\n", Replacement).Replace("\n", Replacement).Replace("\r", Replacement);
	}

	/// <summary>
	/// Removes Sequential Spaces in a string. RegEx isn't necessarily faster.
	/// </summary>
	/// <param name="s"></param>
	/// <param name="UseRegEx"></param>
	/// <returns></returns>
	public static string RemoveMultipleSpaces(this string s, bool UseRegEx = false)
	{
		if (string.IsNullOrWhiteSpace(s)) { return s; }

		if (UseRegEx)
		{
			var regex = new Regex("[ ]{2,}", RegexOptions.None);
			return regex.Replace(s, " ");
		}

		var sb = new StringBuilder(s.Length);

		int i = 0;
		foreach (char c in s)
		{
			if (c != ' ' || i == 0 || s[i - 1] != ' ')
			{
				sb.Append(c);
			}
			i++;
		}
		return sb.ToString();
	}

	public static string RemoveMarkup(this string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return s;
		}

		s = s.Replace("javascript", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
		s = Regex.Replace(s, @"&nbsp;", " ", RegexOptions.IgnoreCase);
		return Regex.Replace(s.Replace("  ", " "), @"<.+?>", "", RegexOptions.Singleline);
	}

	public static string RemoveQuotes(this string input) => input.Remove(["\"", "'"]);


	public static string EscapeXml(this string s)
	{
		string xml = s;
		if (!string.IsNullOrWhiteSpace(xml))
		{
			// replace literal values with entities
			xml = xml.Replace("&", "&amp;");
			xml = xml.Replace("&lt;", "&lt;");
			xml = xml.Replace("&gt;", "&gt;");
			xml = xml.Replace("\"", "&quot;");
			xml = xml.Replace("'", "&apos;");
		}
		return xml;
	}

	public static string UnescapeXml(this string s)
	{
		string unxml = s;
		if (!string.IsNullOrWhiteSpace(unxml))
		{
			// replace entities with literal values
			unxml = unxml.Replace("&apos;", "'");
			unxml = unxml.Replace("&quot;", "\"");
			unxml = unxml.Replace("&gt;", "&gt;");
			unxml = unxml.Replace("&lt;", "&lt;");
			unxml = unxml.Replace("&amp;", "&");
		}
		return unxml;
	}

	public static List<string> SplitOnChar(this string s, char c)
	{
		var list = new List<string>();
		if (string.IsNullOrWhiteSpace(s))
		{
			return list;
		}

		string[] a = s.Split(c);
		foreach (string item in a)
		{
			if (!string.IsNullOrWhiteSpace(item))
			{
				list.Add(item);
			}
		}

		return list;
	}

	public static List<string> SplitOnCharAndTrim(this string s, char c)
	{
		var list = new List<string>();
		if (string.IsNullOrWhiteSpace(s))
		{
			return list;
		}

		string[] a = s.Split(c);
		foreach (string item in a)
		{
			if (!string.IsNullOrWhiteSpace(item))
			{
				list.Add(item.Trim());
			}
		}

		return list;
	}

	public static List<int> SplitIntStringOnCharAndTrim(this string s, char c)
	{
		var list = new List<int>();
		if (string.IsNullOrWhiteSpace(s))
		{
			return list;
		}

		string[] a = s.Split(c);
		foreach (string item in a)
		{
			if (!string.IsNullOrWhiteSpace(item))
			{
				list.Add(Convert.ToInt32(item.Trim()));
			}
		}

		return list;
	}

	public static List<string> SplitOnNewLineAndTrim(this string s)
	{
		var list = new List<string>();
		if (string.IsNullOrWhiteSpace(s))
		{
			return list;
		}

		string[] a = s.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

		foreach (string b in a)
		{
			if (!string.IsNullOrWhiteSpace(b))
			{
				list.Add(b.Trim());
			}
		}
		return list;
	}

	/// <summary>
	/// Determines if a string is in an array of strings.
	/// </summary>
	/// <returns>bool</returns>
	/// <example>
	/// string color = "blue";
	/// string[] colors = {"blue", "green", "red"};
	/// bool validColor = false;
	/// validColor = color.IsIn(colors);
	/// </example>
	public static bool IsIn<T>(this T source, params T[] values)
	{
		return values.Contains(source);
	}

	public static List<string> SplitOnPipes(this string s)
	{
		var list = new List<string>();

		if (string.IsNullOrWhiteSpace(s))
		{
			return list;
		}

		string[] a = s.Split('|');
		foreach (string item in a)
		{
			if (!string.IsNullOrWhiteSpace(item))
			{
				list.Add(item.Trim());
			}
		}

		return list;
	}

	/// <summary>
	/// Combines two delimited strings avoiding duplicates
	/// </summary>
	/// <param name="s1"></param>
	/// <param name="s2"></param>
	/// <param name="delimiter"></param>
	/// <returns></returns>
	public static string Union(this string s1, string s2, char delimiter) => string.Join(delimiter.ToString(), s1.SplitOnChar(delimiter).Union(s2.SplitOnChar(delimiter)).ToList());

	public static string RelativePath(this HttpServerUtility srv, string path)
	{
		return path.Replace(HttpContext.Current.Server.MapPath("~/"), "~/").Replace(@"\", "/");
	}
}