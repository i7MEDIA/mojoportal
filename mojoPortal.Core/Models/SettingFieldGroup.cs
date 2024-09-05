using System.Collections.Generic;

namespace mojoPortal.Core.Models;
public class SettingFieldGroup
{
	public string Label { get; set; }

	public List<object> Fields { get; set; } = [];
}
