using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BIGImageModel
	{
		public string Title { get; set; }
		public string ImageUrl { get; set; }
		public string ImageThumbUrl { get; set; }
	}
	public class BIGFolderModel
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string ParentName { get; set; }
		public string ParentFolderPath { get; set; }
	}
	public class BIGModel
	{
		public List<BIGImageModel> Images { get; set; } = new List<BIGImageModel>();
		public List<BIGFolderModel> Folders { get; set; } = new List<BIGFolderModel>();
	}
}