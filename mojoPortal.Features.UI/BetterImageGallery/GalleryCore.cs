using System;
using System.Collections;
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

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class GalleryCore
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BetterImageGalleryRazor));
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected BIGConfig bigConfig = new BIGConfig();
		private Hashtable moduleSettings;

		private int siteID = -1;
		private SiteSettings siteSettings = null;
		private SiteUser currentUser = null;
		private IFileSystem fileSystem = null;

		private string siteRoot = string.Empty;
		private string mediaRootPath = string.Empty;
		private string galleryRootPath = string.Empty;
		private string galleryPath = string.Empty;
		private readonly string moduleThumbnailCachePath = "/Data/systemfiles/BetterImageGalleryCache/";
		private readonly int thumbnailSize = 200;


		public GalleryCore()
		{
			LoadSettings();
		}


		public GalleryCore(BIGConfig config)
		{
			bigConfig = config;
			LoadSettings();
		}

		public GalleryCore(Hashtable settings)
		{
			moduleSettings = settings;
			LoadSettings();
		}

		public void Setup()
		{
			SetupThumbnails();
		}


		public BIGModel GetImages()
		{
			return GetImages(galleryPath);
		}


		public BIGModel GetImages(string path)
		{
			if (!path.Contains(galleryRootPath.Replace("~", String.Empty)))
				path = galleryRootPath + path;

			var folderList = fileSystem.GetFolderList(path).ToList();
			var imagesList = fileSystem.GetFileList(path).ToList();

			var model = new BIGModel();

			foreach (var folder in folderList)
			{
				model.Folders.Add(new BIGFolderModel
				{
					Name = folder.Name,
					Path = folderPath(folder.Path),
					Parent = parentPath(folder.Name, folder.Path)
				});
			}

			foreach (var image in imagesList)
			{
				model.Thumbnails.Add(new BIGImageModel
				{
					Name = @Path.GetFileNameWithoutExtension(image.Name),
					Full = Uri.EscapeUriString(siteRoot + image.VirtualPath.Replace("~", string.Empty).Replace("\\", "/")),
					Thumb = Uri.EscapeUriString(siteRoot + $"/api/BetterImageGallery/imagehandler?path={bigConfig.FolderPath}/{FileNameWithJpegExt(image.Name)}")
				});
			}

			string folderPath(string str)
			{
				return Uri.EscapeUriString(str.Replace("|", "/").Replace("BetterImageGallery/", String.Empty));
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
			mediaRootPath = fileSystem.VirtualRoot;
			// Gallery Module Folder
			galleryRootPath = mediaRootPath + "BetterImageGallery/";
			// Gallery Folder
			galleryPath = galleryRootPath + bigConfig.FolderPath;

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
			if (bigConfig.FolderPath == string.Empty) return;

			var galleryDiskPath = HttpContext.Current.Server.MapPath(galleryPath);
			var dirInfo = new DirectoryInfo(galleryDiskPath);
			var images = Web.ImageHelper.GetImageExtensions()
				.SelectMany(ext => dirInfo.GetFiles(ext, SearchOption.AllDirectories))
				.ToArray();
			var thumbnailCachePath = moduleThumbnailCachePath + dirInfo.Name + "/";

			// Creates thumbnail cache folder if it doesn't exist, should only happen
			// the first time this gallery instance is hit.
			if (!Directory.Exists(thumbnailCachePath) && FolderCountUnderLimit())
			{
				fileSystem.CreateFolder(thumbnailCachePath);
				CreateThumbnailDataFile(images, thumbnailCachePath);
				CreateThumbnails(images, thumbnailCachePath);
			}
			else
			{
				var thumbnails = GetThumbnailDataFile(thumbnailCachePath);
				var imageNameList = images.Select(x => FileNameWithJpegExt(x.Name)).ToList();

				// Finds what images are in the thumbnails data file, but not in the gallery folder
				//var missingThumbnailsList = thumbnails.Except(imageNameList).ToList();
				// Finds what images are in the galler folder, but not in the data file
				var missingImageNamesList = imageNameList.Except(thumbnails).ToList();

				// Creates missing thumbnail images
				if (missingImageNamesList.Count() > 0)
				{
					var missingImages = images.Where(i => missingImageNamesList.Contains(i.Name)).ToArray();

					CreateThumbnailDataFile(images, thumbnailCachePath);
					CreateThumbnails(missingImages, thumbnailCachePath);
				}
			}
		}


		private void CreateThumbnailDataFile(FileInfo[] images, string thumbnailCachePath)
		{
			var mappedImages = images.Select(x => FileNameWithJpegExt(x.Name)).ToList();
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
						var thumbnailDiscPath = HttpContext.Current.Server
							.MapPath(thumbnailCachePath + FileNameWithJpegExt(image.Name));

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


		private bool FolderCountUnderLimit()
		{
			return fileSystem.CountFolders() < fileSystem.Permission.MaxFolders;
		}

		private string FileNameWithJpegExt(string str)
		{
			return Path.GetFileNameWithoutExtension(str) + ".jpg";
		}
	}
}