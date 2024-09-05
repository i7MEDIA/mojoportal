using System.Collections.Generic;

namespace mojoPortal.Core.Models;

public class SettingField<T, OptionType>
{
	public SettingField()
	{
		ReturnType = typeof(T).Name;
		FieldType = typeof(T).Name;
	}

	public string Name { get; set; }

	public string Label { get; set; }

	public string ReturnType { get; private set; }

	/// <summary>
	/// For setting custom field types, like "Image".
	/// Default is the same as return type
	/// </summary>
	public string FieldType { get; set; }
	public string RegexValidation { get; set; }

	public Dictionary<string, OptionType> Options { get; set; }
	public string Group { get; set; }
	public int Sort { get; set; } = 0;
	public string HelpKey { get; set; }
	public T Value { get; set; }

}

public class SettingField<T> : SettingField<T, string> { }
