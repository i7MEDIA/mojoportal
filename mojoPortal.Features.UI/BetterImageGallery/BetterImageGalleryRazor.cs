using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BetterImageGalleryRazor : WebControl
	{


		private static readonly ILog log = LogManager.GetLogger(typeof(BetterImageGalleryRazor));
		private int pageNumber = 1;
		private int totalPages = 1;
		private int pageSize = 4;
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		private Module module = null;
		protected BIGConfig bimConfig = new BIGConfig();
		
		private SiteSettings siteSettings = null;


		private SiteUser currentUser = null;
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

			if (Page.IsPostBack)
			{
				return;
			}
		}

		protected virtual void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			SiteId = siteSettings.SiteId;
			currentUser = SiteUtils.GetCurrentSiteUser();


			pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

			module = new Module(ModuleId);



		
			pageSize = bimConfig.PageSize;

		}
		public static int CompareFileNames(FileInfo f1, FileInfo f2)
		{
			return f1.FullName.CompareTo(f2.FullName);
		}
		public static int CompareFolderNames(DirectoryInfo d1, DirectoryInfo d2)
		{
			return d1.FullName.CompareTo(d2.FullName);
		}
		protected override void RenderContents(HtmlTextWriter output)
		{
			//todo: look up images
			//todo: create gallerymodel
			//todo: populate model with images

			var di = new DirectoryInfo(bimConfig.FolderPath);
			var imgExt = Web.ImageHelper.GetImageExtensions();
			var pics = imgExt.SelectMany(ext => di.GetFiles(ext, SearchOption.TopDirectoryOnly)).ToArray();
			var folders = di.GetDirectories("*", SearchOption.TopDirectoryOnly);

			Array.Sort(pics, CompareFileNames);
			Array.Sort(folders, CompareFolderNames);

			var picModels = new List<BIGImageModel>();

			foreach (var pic in pics)
			{
				picModels.Add(new BIGImageModel {
					Title = string.Empty,
					ImageThumbUrl = string.Empty, //go get the thumb url
					ImageUrl = pic.FullName
				});
			}

			var folderModels = new List<BIGFolderModel>();

			foreach (var folder in folders)
			{
				folderModels.Add(new BIGFolderModel
				{
					Name = folder.Name,
					Path = folder.FullName,
					ParentName = folder.Parent.Name,
					ParentFolderPath = folder.Parent.FullName
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
			catch (System.Web.HttpException ex)
			{
				log.ErrorFormat(
					"chosen layout ({0}) for _BetterImageGallery was not found in skin {1}. perhaps it is in a different skin. Error was: {2}",
					bimConfig.Layout,
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);

				text = RazorBridge.RenderPartialToString("_BetterImageGallery", bigModel, "BetterImageGallery");
			}

			output.Write(text);
		}
	}
}