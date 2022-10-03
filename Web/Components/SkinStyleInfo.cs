using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace mojoPortal.Web
{
	public class SkinConfig
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("supportLink")]
		public string SupportLink { get; set; }

		[JsonProperty("compatibleWith")]
		public string CompatibleWith { get; set; }

		[JsonProperty("templates")]
		public List<SkinContentTemplate> Templates { get; set; }

		[JsonProperty("styles")]
		public List<SkinStyle> Styles { get; set; }

		[JsonProperty("panelOptions")]
		public List<PanelOption> PanelOptions { get; set; }
	}
	public class SkinContentTemplate
	{
		[JsonProperty("sysName")]
		public string SysName { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }
	}

	public class SkinStyle
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("place")]
		public string Place { get; set; }

		[JsonProperty("class")]
		public string @Class { get; set; }

		[JsonProperty("element")]
		public string Element { get; set; }
	}

	public class PanelOption
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("class")]
		public string @Class { get; set; }
	}
}