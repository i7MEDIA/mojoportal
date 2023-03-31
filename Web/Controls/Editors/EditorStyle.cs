using mojoPortal.Core.Serializers.Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace mojoPortal.Web.Controls.Editors
{
	public class EditorStyle
	{
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "element")]
		[JsonConverter(typeof(SingleOrArrayConverter<string>))]
		public List<string> Element { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "widget")]
		public string Widget { get; set; }

		[JsonProperty(PropertyName = "group")]
		[JsonConverter(typeof(SingleOrArrayConverter<string>))]
		public List<string> Group { get; set; }

		[JsonProperty(PropertyName = "attributes")]
		public Dictionary<string, string> Attributes { get; set; }

		public bool ShouldSerializeAttributes()
		{
			return Attributes != null;
		}

		public bool ShouldSerializeElement()
		{
			return Element != null;
		}

		public bool ShouldSerializeType()
		{
			return Type != null;
		}

		public bool ShouldSerializeWidget()
		{
			return Widget != null;
		}

		public bool ShouldSerializeGroup()
		{
			return Group != null;
		}

		public static List<EditorStyle> GetEditorStyles(FileInfo file)
		{
			var styles = new List<EditorStyle>();
			if (file.Exists)
			{
				var content = File.ReadAllText(file.FullName);

				styles = JsonConvert.DeserializeObject<List<EditorStyle>>(content);

			}
			return styles;
		}
	}
}