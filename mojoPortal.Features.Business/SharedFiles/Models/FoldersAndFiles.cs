using System.Collections.Generic;

namespace mojoPortal.Features.Business.SharedFiles.Models
{
	public class FoldersAndFiles
	{
		public List<Folder> Folders { get; set; } = new List<Folder>();
		public List<File> Files { get; set; } = new List<File>();
	}
}