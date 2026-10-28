#nullable enable
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Models;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace mojoPortal.Web.Controllers.FileManager;

public class FileUploadController : ApiController
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(FileServiceController));

	private readonly SiteSettings _siteSettings;
	private readonly bool _overwriteExistingFiles;
	private readonly string _siteRoot;
	private readonly IFileSystem? _fileSystem;
	private readonly string _virtualPath = string.Empty;


	#region Constructors

	public FileUploadController()
	{
		_siteSettings = CacheHelper.GetCurrentSiteSettings();
		_overwriteExistingFiles = WebConfigSettings.FileManagerOverwriteFiles;
		_siteRoot = SiteUtils.GetNavigationSiteRoot();

		var fileSystemProvider = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

		if (fileSystemProvider is null)
		{
			_log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
			return;
		}

		_fileSystem = fileSystemProvider.GetFileSystem();

		if (_fileSystem is null)
		{
			_log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
			return;
		}

		_virtualPath = _fileSystem.VirtualRoot;

		if (_virtualPath.StartsWith("/"))
		{
			_virtualPath = "~" + _virtualPath;
		}

		if (!_virtualPath.EndsWith("/"))
		{
			_virtualPath += "/";
		}

		if (!_virtualPath.StartsWith(_fileSystem.VirtualRoot))
		{
			_log.Info("returning 404 for upload request to " + _virtualPath);
			return;
		}
	}

	#endregion


	[HttpPost]
	public FileService.ReturnObject UploadFiles()
	{
		if (!AppConfig.EnableUploads)
		{
			_log.Info(Resource.UploadDisabledMessage);
			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.UploadDisabledMessage });
		}

		var context = HttpContext.Current;
		HttpFileCollection? files = context.Request.Files.Count > 0 ? context.Request.Files : null;

		if (files is null || files.Count is 0)
		{
			_log.Info(Resource.NoFileSelectedWarning);
			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.NoFileSelectedWarning });
		}

		if (_siteSettings is null || _fileSystem is null)
		{
			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemSiteSettingsNotLoaded });
		}

		OpResult results = OpResult.Error;
		StringBuilder errors = new();
		string uploadPath = _virtualPath;
		bool canUpload = (
			WebUser.IsAdminOrContentAdmin
			|| SiteUtils.UserIsSiteEditor()
			|| WebUser.IsInRoles(_siteSettings.GeneralBrowseAndUploadRoles)
			|| WebUser.IsInRoles(_siteSettings.UserFilesBrowseAndUploadRoles)
			|| WebUser.IsInRoles(_siteSettings.RolesThatCanDeleteFilesInEditor)
		);

		if (!canUpload)
		{
			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.AccessDenied });
		}

		if (context.Request.Form.Get("destination") != null)
		{
			uploadPath = FilePath(VirtualPathUtility.AppendTrailingSlash(context.Request.Form.Get("destination")));
		}

		if (files != null)
		{
			var fileUploadsRemaining = _fileSystem.Permission.MaxFiles - _fileSystem.CountAllFiles();

			if (fileUploadsRemaining < files.Count)
			{
				_log.Warn("upload rejected due to _fileSystem.Permission.MaxFiles");

				string errorMessage;

				if (fileUploadsRemaining == 0)
				{
					errorMessage = Resource.FileSystemFileLimitReached;
				}
				else
				{
					errorMessage = string.Format(Resource.FileSystemFileLimitRemainder, fileUploadsRemaining);
				}

				return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = errorMessage });
			}

			for (int f = 0; f < files.Count; f++)
			{
				HttpPostedFile file = files[f];
				bool doUpload = true;

				if (file.ContentLength > _fileSystem.Permission.MaxSizePerFile)
				{
					_log.Info(Resource.FileSystemFileTooLargeError);
					errors.AppendLine(Resource.FileSystemFileTooLargeError);
					doUpload = false;
				}
				else if (_fileSystem.GetTotalSize() + file.ContentLength >= _fileSystem.Permission.Quota)
				{
					_log.Info(Resource.FileSystemStorageQuotaError);
					errors.AppendLine(Resource.FileSystemStorageQuotaError);
					doUpload = false;
				}

				if (!_fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(file.FileName)))
				{
					_log.Info(Resource.FileTypeNotAllowed);
					errors.AppendLine(Resource.FileTypeNotAllowed);
					doUpload = false;
				}

				string destPath = VirtualPathUtility.Combine(uploadPath, Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));

				string ext = Path.GetExtension(file.FileName);
				string mimeType = file.ContentType;

				if (doUpload)
				{
					using (Stream s = file.InputStream)
					{
						if (Path.GetExtension(file.FileName).ToLower() == ".svg")
						{
							results = _fileSystem.SaveFile(destPath, XmlSanitizer.RemoveScripts(s), mimeType, true);
						}
						else
						{
							results = _fileSystem.SaveFile(destPath, s, mimeType, true);
						}
					}

					if (results != OpResult.Succeed)
					{
						errors.AppendLine(results.ToString());
					}
				}
			}

			if (errors.Length > 0)
			{
				return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = errors.ToString() });
			}

			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = true });
		}
		else
		{
			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.NoFileSelectedWarning });
		}
	}


	/// <summary>
	/// Written as a drop in replacement for FileService.ashx
	/// </summary>
	/// <param name="files"></param>
	/// <param name="t"></param>
	/// <param name="maxW"></param>
	/// <param name="maxH"></param>
	/// <param name="rz"></param>
	/// <param name="ko"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("api/FileUpload/UploadFromEditor")]
	public async Task<IHttpActionResult> UploadFromEditor(
		Guid? t,
		int? maxW = null,
		int? maxH = null,
		bool rz = false,
		bool? ko = false)
	{
		var files = HttpContext.Current.Request.Files;

		if (
			files is null ||
			files.Count == 0 ||
			_siteSettings is null ||
			_fileSystem is null ||
			(!WebUser.IsInRoles(_siteSettings.GeneralBrowseAndUploadRoles) &&
			!WebUser.IsInRoles(_siteSettings.UserFilesBrowseAndUploadRoles) &&
			!WebUser.IsAdminOrContentAdmin &&
			!SiteUtils.UserIsSiteEditor()) ||
			(WebConfigSettings.RequireFileSystemServiceToken &&
			(t is null || t != Global.FileSystemToken)))
		{
			return NotFound();
		}

		var resizeWidth = maxW ?? WebConfigSettings.ResizeImageDefaultMaxWidth;
		var resizeHeight = maxH ?? WebConfigSettings.ResizeImageDefaultMaxHeight;
		var resize = rz;
		var keepOriginalSize = ko ?? WebConfigSettings.KeepFullSizeEditorUploadedImages;

		var doUpload = true;
		var file = files[0];
		var contentLength = file.ContentLength;

		if (contentLength > _fileSystem.Permission.MaxSizePerFile)
		{
			_log.Info("upload rejected due to _fileSystem.Permission.MaxSizePerFile");
			doUpload = false;
		}
		else if (_fileSystem.CountAllFiles() >= _fileSystem.Permission.MaxFiles)
		{
			_log.Info("upload rejected due to _fileSystem.Permission.MaxFiles");
			doUpload = false;
		}
		else if (_fileSystem.GetTotalSize() + contentLength >= _fileSystem.Permission.Quota)
		{
			_log.Info("upload rejected due to _fileSystem.Permission.Quota");
			doUpload = false;
		}

		if (!_fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(file.FileName)))
		{
			_log.Info("upload rejected due to not allowed file extension");
			doUpload = false;
		}

		var newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
		var destPath = VirtualPathUtility.Combine(_virtualPath, newFileName);
		var ext = Path.GetExtension(file.FileName).ToLower();
		var mimeType = IOHelper.GetMimeType(ext);
		var fullSizeFilePath = destPath.Replace(ext, "-fs" + ext);

		if (!doUpload)
		{
			return NotFound();
		}

		var fileResults = new List<UploadFilesResult>();

		try
		{
			using (Stream s = file.InputStream)
			{
				if (ext == ".svg")
				{
					_fileSystem.SaveFile(destPath, Core.Helpers.XmlSanitizer.RemoveScripts(s), mimeType, true);
				}
				else
				{
					_fileSystem.SaveFile(destPath, s, mimeType, true);
				}
			}

			if (resize && keepOriginalSize)
			{
				_fileSystem.CopyFile(destPath, fullSizeFilePath, true);
			}

			if (SiteUtils.IsImageFileExtension(ext))
			{
				if (resize)
				{
					ImageHelper.ResizeImage(
						destPath,
						mimeType,
						resizeWidth,
						resizeHeight,
						WebConfigSettings.DefaultResizeBackgroundColor);
				}
			}
		}
		catch (Exception ex)
		{
			_log.Error(ex);

			return NotFound();
		}

		if (resize && keepOriginalSize)
		{
			fileResults.Add(new UploadFilesResult()
			{
				FileUrl = WebUtils.ResolveUrl(destPath),
				FullSizeUrl = WebUtils.ResolveUrl(fullSizeFilePath),
				Name = newFileName,
				Length = contentLength,
				Type = mimeType
			});

		}
		else
		{
			fileResults.Add(new UploadFilesResult()
			{
				FileUrl = WebUtils.ResolveUrl(destPath),
				Name = newFileName,
				Length = contentLength,
				Type = mimeType
			});
		}

		var uploadedFiles = new
		{
			files = fileResults.ToArray()
		};

		return Json(uploadedFiles);
	}


	private string FilePath(string itemPath, bool returnDiskPath = false, bool appendTrailingSlash = false, bool isFullPath = false)
	{
		Regex onlyOneSlashRegEx = new Regex("(/)(?<=\\1\\1)");

		if (string.IsNullOrEmpty(itemPath) || _fileSystem is null)
		{
			return itemPath;
		}

		// Normalize backslashes first, then repeatedly strip ".." to prevent order-of-operations bypasses
		itemPath = itemPath.Replace("\\", "/").Trim();
		while (itemPath.Contains(".."))
		{
			itemPath = itemPath.Replace("..", string.Empty);
		}

		var userFolder = "/" + Resource.UserFolder;

		if (!string.IsNullOrWhiteSpace(_fileSystem.Permission.UserFolder) && itemPath.StartsWith(userFolder, StringComparison.OrdinalIgnoreCase))
		{
			if (!_fileSystem.FolderExists(_fileSystem.Permission.UserFolder))
			{
				_fileSystem.CreateFolder(_fileSystem.Permission.UserFolder);
			}

			itemPath = _fileSystem.Permission.UserFolder + itemPath.Substring(userFolder.Length);
			isFullPath = true;
		}

		string fullPath = !isFullPath ? _virtualPath + itemPath : itemPath;
		// Clean virtual path
		string cleanPath = onlyOneSlashRegEx.Replace(fullPath, string.Empty);

		if (appendTrailingSlash)
		{
			cleanPath = VirtualPathUtility.AppendTrailingSlash(cleanPath);
		}

		// Clean and verify disk path containment
		string diskPath = Path.GetFullPath(HttpContext.Current.Server.MapPath(cleanPath));
		string allowedRootDiskPath = Path.GetFullPath(HttpContext.Current.Server.MapPath(_virtualPath))
			.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
		string diskPathWithSep = diskPath
			.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;

		bool isWithinAllowedRoot = diskPathWithSep.StartsWith(allowedRootDiskPath, StringComparison.OrdinalIgnoreCase);

		if (!isWithinAllowedRoot && !string.IsNullOrWhiteSpace(_fileSystem.Permission.UserFolder))
		{
			string allowedUserDiskPath = Path.GetFullPath(HttpContext.Current.Server.MapPath(_fileSystem.Permission.UserFolder))
				.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			isWithinAllowedRoot = diskPathWithSep.StartsWith(allowedUserDiskPath, StringComparison.OrdinalIgnoreCase);
		}

		if (!isWithinAllowedRoot)
		{
			throw new UnauthorizedAccessException("Attempted to upload to a path outside of the allowed file manager root.");
		}

		if (returnDiskPath)
		{
			return diskPath;
		}
		else
		{
			return cleanPath;
		}
	}
}
