using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.Core.Extensions;

public static class UnitSeparatorExtensions
{
	public const char UNIT_SEPARATOR = '␟';

	/// <summary>
	/// Concatenates the elements of a sequence of strings, using the Unit Separator (ASCII 31) as the delimiter.
	/// </summary>
	/// <param name="items">The sequence of strings to concatenate. Cannot be <see langword="null"/>.</param>
	/// <returns>A single string that consists of the elements in <paramref name="items"/> delimited by the Unit Separator. Returns
	/// an empty string if <paramref name="items"/> is empty.</returns>
	public static string JoinUnitSeparator(this IEnumerable<string> items) => string.Join(UNIT_SEPARATOR.ToString(), items);

	/// <summary>
	/// Splits the input string into substrings based on the Unit Separator (ASCII 31) character, trimming whitespace from
	/// each substring.
	/// </summary>
	/// <param name="input">The input string to split. If <paramref name="input"/> is <see langword="null"/> or empty, an empty sequence is
	/// returned.</param>
	/// <returns>An <see cref="IEnumerable{T}"/> of substrings resulting from the split operation. If the input string is empty or
	/// <see langword="null"/>, the returned sequence is empty.</returns>
	public static IEnumerable<string> SplitUnitSeparator(this string input) => string.IsNullOrEmpty(input) ? Enumerable.Empty<string>() : input.SplitOnCharAndTrim(UNIT_SEPARATOR);
}
