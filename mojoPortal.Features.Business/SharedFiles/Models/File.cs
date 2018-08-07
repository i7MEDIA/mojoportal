using System;

namespace mojoPortal.Features.Business.SharedFiles.Models
{
	public class File
	{
		// Ordered liek the columns in the DB
		public int ID { get; set; }
		public int ModuleID { get; set; }
		public int UploadUserID { get; set; }
		public string Name { get; set; }
		public string OriginalFileName { get; set; }
		public string ServerFileName { get; set; }
		public int SizeInKB { get; set; }
		public DateTime UploadDate { get; set; }
		public int FolderID { get; set; }
		public Guid ItemGuid { get; set; }
		public Guid ModuleGuid { get; set; }
		public Guid UserGuid { get; set; }
		// FileBlob? This is missing in the business layer
		public Guid FolderGuid { get; set; }
		public string Description { get; set; }
		public int DownloadCount { get; set; }
		public string ViewRoles { get; set; }
	}
}