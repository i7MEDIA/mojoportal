using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mojoPortal.Core.Extensions
{
	public class IndexedStringBuilder
	{
		private StringBuilder _stringBuilder;
		public string CurrentString => _stringBuilder.ToString();
		public int Length => _stringBuilder.Length;
		public int SortOrder1 { get; set; } = 0;
		public int SortOrder2 { get; set; } = 0;
		public string GroupName { get; set; } = string.Empty;
		public IndexedStringBuilder()
		{
			_stringBuilder = new StringBuilder();
		}
		public IndexedStringBuilder Append(string s)
		{
			_stringBuilder.Append(s);
			return this;
		}
		public IndexedStringBuilder Append(char c)
		{
			_stringBuilder.Append(c);
			return this;
		}

		public IndexedStringBuilder Append(object o)
		{
			_stringBuilder.Append(o);
			return this;
		}

		public IndexedStringBuilder Replace(string oldValue, string newValue)
		{
			_stringBuilder.Replace(oldValue, newValue);
			return this;
		}

		public IndexedStringBuilder Replace(char oldChar, char newChar)
		{
			_stringBuilder.Replace(oldChar, newChar);
			return this;
		}

		public IndexedStringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
		{
			_stringBuilder.Replace(oldValue, newValue, startIndex, count);
			return this;
		}

		public IndexedStringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
		{
			_stringBuilder.Replace(oldChar, newChar, startIndex, count);
			return this;
		}
		public static IndexedStringBuilder operator +(IndexedStringBuilder sb, string s) => sb.Append(s);

		public static IndexedStringBuilder operator +(IndexedStringBuilder sb, char c) => sb.Append(c);

		public static IndexedStringBuilder operator +(IndexedStringBuilder sb, object o) => sb.Append(o);

		public static implicit operator string(IndexedStringBuilder sb) => sb.CurrentString;

		public override string ToString() => CurrentString;

		public string ToString(int startIndex, int length) => _stringBuilder.ToString(startIndex, length);

	}
}
