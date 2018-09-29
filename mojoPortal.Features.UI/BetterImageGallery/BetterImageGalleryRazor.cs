using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BetterImageGalleryRazor : WebControl
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BetterImageGalleryRazor));
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected BIGConfig bimConfig = new BIGConfig();

		private int pageNumber = 1;
		private int totalPages = 1;
		private int pageSize = 4;
		private Module module = null;
		private SiteSettings siteSettings = null;
		private SiteUser currentUser = null;

		private IFileSystem fileSystem = null;
		private string rootPath = string.Empty;

		public int SiteId { get; private set; } = -1;
		public int PageId { get; set; } = -1;
		public int ModuleId { get; set; } = -1;
		public string SiteRoot { get; set; } = string.Empty;
		public string ImageSiteRoot { get; set; } = string.Empty;


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadSettings();

			if (module == null)
			{
				Visible = false;

				return;
			}

			if (Page.IsPostBack) return;
		}


		protected virtual void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			SiteId = siteSettings.SiteId;
			currentUser = SiteUtils.GetCurrentSiteUser();
			pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
			module = new Module(ModuleId);
			pageSize = bimConfig.PageSize;

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

			if (p == null)
			{
				//log.Error(string.Format(Resource.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider));
			}

			fileSystem = p.GetFileSystem();

			if (fileSystem == null)
			{
				//log.Error(string.Format(Resource.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider));
			}

			rootPath = $"{fileSystem.VirtualRoot}BetterImageGallery/{bimConfig.FolderPath}";

			if (!Directory.Exists(rootPath))
			{
				if (fileSystem.CountFolders() < fileSystem.Permission.MaxFolders)
				{
					fileSystem.CreateFolder(rootPath);
				}
			}
		}

		public static int CompareFileNames(FileInfo f1, FileInfo f2)
		{
			return f1.FullName.CompareTo(f2.FullName);
		}


		public static int CompareFolderNames(DirectoryInfo d1, DirectoryInfo d2)
		{
			return d1.FullName.CompareTo(d2.FullName);
		}


		protected override void Render(HtmlTextWriter writer)
		{
			RenderContents(writer);
		}


		protected override void RenderContents(HtmlTextWriter output)
		{
			//todo: look up images
			//todo: create gallerymodel
			//todo: populate model with images
			//var diskPath = HttpContext.Current.Server.MapPath(rootPath);
			//var di = new DirectoryInfo(diskPath);
			//var folders = di.GetDirectories("*", SearchOption.TopDirectoryOnly);
			//var pics = Web.ImageHelper.GetImageExtensions().SelectMany(ext => di.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();

			var images = fileSystem.GetFileList(rootPath).ToArray();
			var folders = fileSystem.GetFolderList(rootPath).ToArray();

			//Array.Sort(folders, CompareFolderNames);
			//Array.Sort(images, CompareFileNames);

			var picModels = new List<BIGImageModel>();

			foreach (var image in images)
			{
				picModels.Add(new BIGImageModel {
					Title = image.Name,
					ImageThumbUrl = image.VirtualPath.Replace("~", string.Empty), //go get the thumb url
					ImageUrl = image.VirtualPath.Replace("~", string.Empty)
				});
			}

			var folderModels = new List<BIGFolderModel>();

			foreach (var folder in folders)
			{
				folderModels.Add(new BIGFolderModel
				{
					Name = folder.Name,
					Path = folder.VirtualPath.Replace("~", string.Empty)
				});
			}

			BIGModel bigModel = new BIGModel {
				Folders = folderModels,
				Images = picModels
			};

			string text = string.Empty;

			try
			{
				text = RazorBridge.RenderPartialToString(bimConfig.Layout, bigModel, "BetterImageGallery");
			}
			catch (HttpException ex)
			{
				log.ErrorFormat(
					"Chosen layout ({0}) for _BetterImageGallery was not found in skin {1}. perhaps it is in a different skin. Error was: {2}",
					bimConfig.Layout,
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);

				text = RazorBridge.RenderPartialToString("_BetterImageGallery", bigModel, "BetterImageGallery");
			}

			output.Write(text);
		}

		private string FolderPath(string path, bool returnDiskPath = false, bool isFullPath = false)
		{
			if (string.IsNullOrEmpty(path)) return path;

			// Remove "../" or "\" to prevent hacks
			path = path.Replace("..", string.Empty).Replace("\\", string.Empty).Trim();

			string fullPath = !isFullPath ? rootPath + path : path;

			// Virtual path
			Regex onlyOneSlashRegEx = new Regex("(/)(?<=\\1\\1)");
			string cleanPath = onlyOneSlashRegEx.Replace(fullPath, string.Empty);

			if (returnDiskPath)
			{
				return HttpContext.Current.Server.MapPath(cleanPath);
			}
			else
			{
				return cleanPath;
			}
		}
	}
}