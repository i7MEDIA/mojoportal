using mojoPortal.Web.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;

namespace mojoPortal.Web.Controls.Editors
{
	public class EditorTemplate
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "image")]
		public string Image { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "html")]
		public string Html { get; set; } = string.Empty;

		public bool ShouldSerializeDescription()
		{
			return Description != null;
		}
		public bool ShouldSerializeImage()
		{
			return Image != null;
		}
	}

	public class EditorTemplateCollection
	{
		[JsonProperty(PropertyName = "imagesPath")]
		public string ImagesPath { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "templates")]
		public List<EditorTemplate> Templates { get; set; }

		public EditorTemplateCollection()
		{
			Templates = new List<EditorTemplate>();
		}


		public EditorTemplateCollection(FileInfo file)
		{
			
			if (file.Exists)
			{
				var contents = File.ReadAllText(file.FullName);
				var collection = JsonConvert.DeserializeObject<EditorTemplateCollection>(contents);
				ImagesPath = collection.ImagesPath;
				Templates = collection.Templates;
			}
		}
	}
}