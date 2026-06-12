using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace mojoPortal.Web.Components;

public static class StringSlugger
{
	private static readonly Regex _wordDelimiters = new(@"[\s—–_]", RegexOptions.Compiled);
	private static readonly Regex _invalidChars = new(@"[^a-z0-9\-]", RegexOptions.Compiled);
	private static readonly Regex _multipleHyphens = new(@"-{2,}", RegexOptions.Compiled);

	public static string ToSlug(string value)
	{
		value = value.ToLowerInvariant();
		value = RemoveDiacritics(value);
		value = _wordDelimiters.Replace(value, "-");
		value = _invalidChars.Replace(value, "");
		value = _multipleHyphens.Replace(value, "-");

		return value.Trim('-');
	}

	private static string RemoveDiacritics(string value)
	{
		var nomalizedValue = value.Normalize(NormalizationForm.FormD);
		var result = string.Empty;

		foreach (var _char in nomalizedValue)
		{
			var uc = CharUnicodeInfo.GetUnicodeCategory(_char);

			if (uc != UnicodeCategory.NonSpacingMark)
			{
				result += _char;
			}
		}

		return result.Normalize(NormalizationForm.FormC);
	}
}
