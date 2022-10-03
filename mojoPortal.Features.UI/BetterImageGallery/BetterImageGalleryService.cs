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
using Resources;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BetterImageGalleryService
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BetterImageGalleryRazor));
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected BIGConfig bigConfig = new BIGConfig();
		private int moduleID = -1;
		private int siteID = -1;
		private SiteSettings siteSettings = null;
		private SiteUser currentUser = null;
		private IFileSystem fileSystem = null;

		private string siteRoot = string.Empty;
		private string mediaRootPath = string.Empty;
		private string galleryRootPath = string.Empty;
		private string galleryPath = string.Empty;
		private readonly string moduleThumbnailCachePath = "~/Data/systemfiles/BetterImageGalleryCache/";
		private readonly int thumbnailSize = 200;

		public BIGErrorResult Error { get; set; } = null;


		public BetterImageGalleryService(int moduleId)
		{
			moduleID = moduleId;
			LoadSettings();
		}


		public void Setup()
		{
			if (Error == null)
			{
				SetupThumbnails();
			}
		}


		public BIGModel GetImages()
		{
			return GetImages(galleryPath);
		}


		public BIGModel GetImages(string path)
		{
			var model = new BIGModel
			{
				ModuleID = moduleID,
				GalleryFolder = bigConfig.FolderPath
			};

			if (string.IsNullOrWhiteSpace(bigConfig.FolderPath)) return model;

			if (!path.Contains(galleryRootPath.Replace("~", String.Empty)))
			{
				path = galleryRootPath + path;
			}

			var folderList = fileSystem.GetFolderList(path).ToList();
			var imagesList = fileSystem.GetFileList(path).ToList();

			foreach (var folder in folderList)
			{
				model.Folders.Add(new BIGFolderModel
				{
					Name = folder.Name,
					Path = Uri.EscapeUriString(folderPath(folder.Path)),
					ParentName = parentName(folder.Name, folder.Path),
					ParentPath = Uri.EscapeUriString(parentPath(folder.Name, folder.Path))
				});
			}

			foreach (var image in imagesList)
			{
				var galleryPathFolder = galleryPath.Substring(galleryPath.LastIndexOf('/') + 1);
				
				model.Thumbnails.Add(new BIGImageModel
				{
					Name = Path.GetFileNameWithoutExtension(image.Name),
					Full = Uri.EscapeUriString(siteRoot + image.VirtualPath.Replace("~", string.Empty).Replace("\\", "/")),
					Thumb = Uri.EscapeUriString(siteRoot + $"/api/BetterImageGallery/imagehandler?path={galleryPathFolder}/{FileWithFolderAndJpegExt(image.Path)}")
				});
			}

			string folderPath(string str)
			{
				return str.Replace("|", "/").Replace("BetterImageGallery/", String.Empty);
			}

			string parentName(string name, string str)
			{
				str = parentPath(name, str);
				return str.Substring(str.LastIndexOf('/') + 1);
			}

			string parentPath(string name, string str)
			{
				return folderPath(str).Replace("/" + name, String.Empty);
			}

			return model;
		}


		protected virtual void LoadSettings()
		{
			siteRoot = SiteUtils.GetNavigationSiteRoot();
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			siteID = siteSettings.SiteId;
			currentUser = SiteUtils.GetCurrentSiteUser();

			var moduleSettings = ModuleSettings.GetModuleSettings(moduleID);

			if (moduleSettings != null)
			{
				bigConfig = new BIGConfig(moduleSettings);
			}

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

			if (p == null)
			{
				log.Error(string.Format(BetterImageGalleryResources.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider));
			}

			fileSystem = p.GetFileSystem();

			if (fileSystem == null)
			{
				log.Error(string.Format(BetterImageGalleryResources.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider));
			}

			// Media Folder
			//VirtualRoot isn't always media folder, depending on how the site is configured, so we'll make sure to use media folder
			
			mediaRootPath = fileSystem.VirtualRoot;
			if (!mediaRootPath.TrimEnd('/').EndsWith("media"))
			{
				mediaRootPath = mediaRootPath.TrimEnd('/') + "/media/";
			}

			// Gallery Module Folder
			galleryRootPath = mediaRootPath + "BetterImageGallery/";
			// Gallery Folder
			galleryPath = galleryRootPath + bigConfig.FolderPath.TrimEnd('/');

			// Creates the Gallery Module Folder if it doesn't exist
			if (!fileSystem.FolderExists(galleryRootPath))
			{
				fileSystem.CreateFolder(galleryRootPath);
			}

			// Creates the Gallery Module Folder if it doesn't exist
			if (!fileSystem.FolderExists(galleryPath))
			{
				Error = new BIGErrorResult
				{
					Type = "FolderNotFound",
					Message = BetterImageGalleryResources.FolderNotFound
				};
			}

			// Creates module thumbnail cache folder if it doesn't exist
			if (!fileSystem.FolderExists(moduleThumbnailCachePath))
			{
				fileSystem.CreateFolder(moduleThumbnailCachePath);
			}
		}


		private void SetupThumbnails()
		{
			if (bigConfig.FolderPath == string.Empty) return;

			var galleryDiskPath = HttpContext.Current.Server.MapPath(galleryPath);
			var dirInfo = new DirectoryInfo(galleryDiskPath);
			var images = Web.ImageHelper.GetImageExtensions()
				.SelectMany(ext => dirInfo.GetFiles(ext, SearchOption.AllDirectories))
				.ToArray();
			var thumbnailCachePath = moduleThumbnailCachePath + dirInfo.Name + "/";

			// Creates thumbnail cache folder if it doesn't exist, should only happen
			// the first time this gallery instance is hit.
			if (!fileSystem.FolderExists(thumbnailCachePath))
			{
				fileSystem.CreateFolder(thumbnailCachePath);
				//CreateThumbnailDataFile(images, thumbnailCachePath);
				CreateThumbnails(images, thumbnailCachePath);
			}
			else
			{
				//var thumbnails = GetThumbnailDataFile(thumbnailCachePath);
				//var imageNameList = images.Select(x => FileNameWithJpegExt(x.Name)).ToList();

				//// Finds what images are in the thumbnails data file, but not in the gallery folder
				////var missingThumbnailsList = thumbnails.Except(imageNameList).ToList();
				//// Finds what images are in the galler folder, but not in the data file
				//var missingImageNamesList = imageNameList.Except(thumbnails).ToList();

				//// Creates missing thumbnail images
				//if (missingImageNamesList.Count() > 0)
				//{
				//	var missingImages = images.Where(i => missingImageNamesList.Contains(Path.GetFileNameWithoutExtension(i.Name) + ".jpg")).ToArray();

				//	CreateThumbnailDataFile(images, thumbnailCachePath);
				//	CreateThumbnails(missingImages, thumbnailCachePath);
				//}

				var thumbnailCacheDiscPath = HttpContext.Current.Server.MapPath(thumbnailCachePath);
				var cacheDirInfo = new DirectoryInfo(thumbnailCacheDiscPath);
				var cacheImages = Web.ImageHelper.GetImageExtensions()
					.SelectMany(ext => cacheDirInfo.GetFiles(ext, SearchOption.AllDirectories))
					.ToArray();

				var imageNameList = images.Select(x => FileNameWithJpegExt(x.Name)).ToList();
				var thumbnailNameList = cacheImages.Select(x => FileNameWithJpegExt(x.Name)).ToList();
				var missingImageNamesList = imageNameList.Except(thumbnailNameList).ToList();

				// Creates missing thumbnail images
				if (missingImageNamesList.Count() > 0)
				{
					var missingImages = images.Where(i => missingImageNamesList.Contains(Path.GetFileNameWithoutExtension(i.Name) + ".jpg")).ToArray();

					CreateThumbnails(missingImages, thumbnailCachePath);
				}
			}
		}


		private void CheckThumbnails(string imageCachePath)
		{

		}


		//private void CreateThumbnailDataFile(FileInfo[] images, string thumbnailCachePath)
		//{
		//	var mappedImages = images.Select(x => FileNameWithJpegExt(x.Name)).ToList();
		//	var thumbnailCacheDiscPath = HttpContext.Current.Server.MapPath(thumbnailCachePath);

		//	File.WriteAllText(thumbnailCacheDiscPath + "data.config", JsonConvert.SerializeObject(mappedImages));
		//}


		//private List<string> GetThumbnailDataFile(string thumbnailCachePath)
		//{
		//	var thumbnailCacheDiscPath = HttpContext.Current.Server.MapPath(thumbnailCachePath);
		//	var dataFile = File.ReadAllText(thumbnailCacheDiscPath + "data.config", Encoding.UTF8);

		//	return JsonConvert.DeserializeObject<List<string>>(dataFile);
		//}


		private void CreateThumbnails(FileInfo[] images, string thumbnailCachePath)
		{
			foreach (var image in images)
			{
				using (Bitmap originalImage = LoadOriginalImage(image.FullName))
				{
					using (Bitmap newImage = CreateNewImage(originalImage, thumbnailSize))
					{
						var imageRelativePath = image.FullName.Replace(HttpContext.Current.Server.MapPath(galleryPath), string.Empty).Replace("\\", "/");
						var imageName = FileWithFolderAndJpegExt(image.FullName);
						var imageFolder = imageRelativePath.Replace(Path.GetFileName(imageRelativePath), string.Empty);
						var thumbnailDiscPath = HttpContext.Current.Server.MapPath(thumbnailCachePath + imageName);

						if (imageFolder != "/")
						{
							fileSystem.CreateFolder(thumbnailCachePath + imageFolder);
						}

						newImage.Save(thumbnailDiscPath, ImageFormat.Jpeg);
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

			if (originalWidth >= originalHeight && originalWidth > size)
			{
				height = size;
				width = Convert.ToInt32(size * (double)originalWidth / originalHeight);
			}
			else if (originalHeight >= originalWidth && originalHeight > size)
			{
				height = Convert.ToInt32(size * (double)originalHeight / originalWidth);
				width = size;
			}

			Bitmap newImage = new Bitmap(size, size);
			Graphics g = Graphics.FromImage(newImage);

			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			g.DrawImage(originalImage,
				(size - width) / 2,
				(size - height) / 2,
				width,
				height
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


		//private bool FolderCountUnderLimit()
		//{
		//	return fileSystem.CountFolders() < fileSystem.Permission.MaxFolders;
		//}

		private string FileNameWithJpegExt(string str)
		{
			return Path.GetFileNameWithoutExtension(str) + ".jpg";
		}

		private string FileWithFolderAndJpegExt(string str)
		{
			var imageRelativePath = str.Replace(HttpContext.Current.Server.MapPath(galleryPath), string.Empty).Replace("\\", "/");
			var imageName = Path.GetFileName(imageRelativePath);
			return imageRelativePath.Replace(imageName, Path.GetFileNameWithoutExtension(imageName) + ".jpg");
		}
	}
}