namespace mojoPortal.Web.Models
{
	public class FileManager
	{
		public bool OverwriteFiles { get; set; }
		public string RootName { get; set; }
		public string FileSystemToken { get; set; }
		public string VirtualPath { get; set; }
		public bool ReturnFullPath { get; set; }
		public string UserFolder { get; set; }
		public string UserFolderName { get; set; }
		public string View { get; set; }
		public string Type { get; set; }
		public string Editor { get; set; }
		public string InputId { get; set; }
		public bool PickFolders { get; set; }
		public string CKEditorFuncNumber { get; set; }
		public QueryParams QueryString { get; set; }

		public string Upload { get; set; }
		public string Rename { get; set; }
		public string Move { get; set; }
		public string Copy { get; set; }
		public string Edit { get; set; }
		public string Compress { get; set; }
		public string CompressChooseName { get; set; }
		public string Extract { get; set; }
		public string Download { get; set; }
		public string DownloadMultiple { get; set; }
		public string Preview { get; set; }
		public string Remove { get; set; }
		public string CreateFolder { get; set; }

		public string PagePickerLinkText { get; set; }
		public string BackToFileManagerLinkText { get; set; }
		public string BackToWebsiteLinkText { get; set; }
	}

	public class QueryParams
	{
		public string type { get; set; }
		public string editor { get; set; }
		public string inputId { get; set; }
		public string CKEditor { get; set; }
		public string CKEditorFuncNum { get; set; }
		public string startFolder { get; set; }
		public bool returnFullPath { get; set; } = true;
	}
}