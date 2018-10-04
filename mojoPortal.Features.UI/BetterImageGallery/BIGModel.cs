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
		public string Parent { get; set; }
	}


	public class BIGModel
	{
		public List<BIGFolderModel> Folders { get; set; } = new List<BIGFolderModel>();
		public List<BIGImageModel> Thumbnails { get; set; } = new List<BIGImageModel>();
	}
}