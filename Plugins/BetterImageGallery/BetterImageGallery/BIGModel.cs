using System.Collections.Generic;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BIGImageModel
	{
		public string Name { get; set; }
		public string Full { get; set; }
		public string Thumb { get; set; }
	}


	public class BIGFolderModel
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string ParentName { get; set; }
		public string ParentPath { get; set; }
	}


	public class BIGModel
	{
		public List<BIGFolderModel> Folders { get; set; } = new List<BIGFolderModel>();
		public List<BIGImageModel> Thumbnails { get; set; } = new List<BIGImageModel>();
		public int ModuleID { get; set; } = -1;
		public string GalleryFolder { get; set; }
	}

	public class BIGErrorResult
	{
		public string Type { get; set; }
		public string Message { get; set; }
	}
}