using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using Newtonsoft.Json;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class GalleryCore
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BetterImageGalleryRazor));
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected BIGConfig bimConfig = new BIGConfig();

		private int siteID = -1;
		private SiteSettings siteSettings = null;
		private SiteUser currentUser = null;
		private IFileSystem fileSystem = null;

		private string mediaRootPath = string.Empty;
		private string galleryRootPath = string.Empty;
		private string galleryPath = string.Empty;
		private readonly string moduleThumbnailCachePath = "/Data/systemfiles/BetterImageGalleryCache";
		private readonly int thumbnailSize = 200;


		public GalleryCore()
		{
			LoadSettings();
			SetupThumbnails();
		}


		public BIGModel GetImages()
		{
			return GetImages(galleryPath);
		}


		public BIGModel GetImages(string path)
		{
			var folderList = fileSystem.GetFolderList(path).ToList();
			var imagesList = fileSystem.GetFileList(path).ToList();

			var model = new BIGModel();

			foreach (var folder in folderList)
			{
				model.Folders.Add(new BIGFolderModel
				{
					Name = folder.Name,
					Path = folder.Path
				});
			}

			foreach (var image in imagesList)
			{
				model.Images.Add(new BIGImageModel
				{
					Title = image.Name,
					ImageUrl = image.Path
				});
			}

			return model;
		}


		private void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			siteID = siteSettings.SiteId;
			currentUser = SiteUtils.GetCurrentSiteUser();

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

			// Media Folder
			mediaRootPath = fileSystem.VirtualRoot;
			// Gallery Module Folder
			galleryRootPath = mediaRootPath + "BetterImageGallery/";
			// Gallery Folder
			galleryPath = galleryRootPath + bimConfig.FolderPath;

			// Creates the Gallery Module Folder if it doesn't exist
			if (!Directory.Exists(galleryRootPath) && FolderCountUnderLimit())
			{
				fileSystem.CreateFolder(galleryRootPath);
			}

			// Creates module thumbnail cache folder if it doesn't exist
			if (!Directory.Exists(moduleThumbnailCachePath) && FolderCountUnderLimit())
			{
				fileSystem.CreateFolder(moduleThumbnailCachePath);
			}
		}


		private void SetupThumbnails()
		{
			var galleryDiskPath = HttpContext.Current.Server.MapPath(galleryPath);
			var dirInfo = new DirectoryInfo(galleryDiskPath);
			var images = Web.ImageHelper.GetImageExtensions()
				.SelectMany(ext => dirInfo.GetFiles(ext, SearchOption.AllDirectories))
				.ToArray();
			var thumbnailCachePath = moduleThumbnailCachePath + dirInfo.Name + "/";

			// Creates thumbnail cache folder if it doesn't exist, only should happen the first time this gallery instance is hit
			if (!Directory.Exists(thumbnailCachePath) && FolderCountUnderLimit())
			{
				fileSystem.CreateFolder(thumbnailCachePath);
				CreateThumbnailDataFile(images, thumbnailCachePath);
				CreateThumbnails(images, thumbnailCachePath);
			}
			else
			{
				var thumbnails = GetThumbnailDataFile(thumbnailCachePath);
				var imageNameList = images.Select(x => x.Name).ToList();

				// Finds what images are in the thumbnails data file, but not in the gallery folder
				//var missingThumbnailsList = thumbnails.Except(imageNameList).ToList();
				// Finds what images are in the galler folder, but not in the data file
				var missingImageNamesList = imageNameList.Except(thumbnails).ToList();

				// Creates missing thumbnail images
				if (missingImageNamesList.Count() > 0)
				{
					var missingImages = images.Where(i => missingImageNamesList.Contains(i.Name)).ToArray();

					CreateThumbnails(missingImages, thumbnailCachePath);
				}
			}
		}


		private void CreateThumbnailDataFile(FileInfo[] images, string thumbnailCachePath)
		{
			var mappedImages = images.Select(x => x.Name).ToList();
			var thumbnailCacheDiscPath = HttpContext.Current.Server.MapPath(thumbnailCachePath);

			File.WriteAllText(thumbnailCacheDiscPath + "data.config", JsonConvert.SerializeObject(mappedImages));
		}


		private List<string> GetThumbnailDataFile(string thumbnailCachePath)
		{
			var thumbnailCacheDiscPath = HttpContext.Current.Server.MapPath(thumbnailCachePath);
			var dataFile = File.ReadAllText(thumbnailCacheDiscPath + "data.config", Encoding.UTF8);

			return JsonConvert.DeserializeObject<List<string>>(dataFile);
		}


		private void CreateThumbnails(FileInfo[] images, string thumbnailCachePath)
		{
			foreach (var image in images)
			{
				using (Bitmap originalImage = LoadOriginalImage(image.FullName))
				{
					using (Bitmap newImage = CreateNewImage(originalImage, thumbnailSize))
					{
						newImage.Save(thumbnailCachePath + image.Name, ImageFormat.Jpeg);
					}
				}
			}
		}


		private static Bitmap LoadOriginalImage(string imageFile)
		{
			return (Bitmap)Image.FromFile(imageFile, false);
		}


		private static Bitmap CreateNewImage(Bitmap originalImage, int size)
		{
			int originalWidth = originalImage.Width;
			int originalHeight = originalImage.Height;
			int height = size;
			int width = size;
			int drawXOffset, drawYOffset, drawWidth, drawHeight;

			if (size > 0 && originalWidth >= originalHeight && originalWidth > size)
			{
				height = size;
				width = Convert.ToInt32(size * (double)originalWidth / originalHeight);
			}
			else if (size > 0 && originalHeight >= originalWidth && originalHeight > size)
			{
				height = Convert.ToInt32(size * (double)originalHeight / originalWidth);
				width = size;
			}
			else
			{
				width = originalWidth;
				height = originalHeight;
			}

			drawXOffset = 0;
			drawYOffset = 0;
			drawWidth = width;
			drawHeight = height;

			drawXOffset = (width - drawWidth) / 2;
			drawYOffset = (height - drawHeight) / 2;

			Bitmap newImage = new Bitmap(width, height);
			Graphics g = Graphics.FromImage(newImage);

			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			g.DrawImage(originalImage,
				drawXOffset,
				drawYOffset,
				drawWidth,
				drawHeight
			);

			g.Dispose();

			return newImage;
		}


		private static byte[] GetImageBytes(Bitmap image)
		{
			ImageCodecInfo codec = GetImageCodec();
			EncoderParameters encoderParams = GetImageEncoderParams();

			MemoryStream ms = new MemoryStream();
			image.Save(ms, codec, encoderParams);
			ms.Close();

			encoderParams.Dispose();
			return ms.ToArray();
		}


		private static ImageCodecInfo GetImageCodec()
		{
			ImageCodecInfo codec = null;
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

			foreach (ImageCodecInfo e in encoders)
			{
				if (e.MimeType == "image/jpeg")
				{
					codec = e;
					break;
				}
			}

			return codec;
		}


		private static EncoderParameters GetImageEncoderParams()
		{
			EncoderParameters encoderParams = new EncoderParameters();
			encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85);

			return encoderParams;
		}


		private bool FolderCountUnderLimit()
		{
			return fileSystem.CountFolders() < fileSystem.Permission.MaxFolders;
		}
	}
}