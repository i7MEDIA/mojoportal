#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Ionic.Zip;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Dtos;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Models;
using Resources;

namespace mojoPortal.Web.Controllers;

public class FileServiceController : ApiController
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(FileServiceController));

	private IFileSystem? _fileSystem = null;
	private bool _allowEditing = false;
	private SiteUser? _currentUser = null;
	private SiteSettings? _siteSettings = null;
	private string _virtualPath = string.Empty;
	private string _allowedExtensions = string.Empty;
	private bool _overwriteExistingFiles = false;


	// POST: /fileservice
	[HttpPost]
	public dynamic FileManagerPost([FromBody] FileService.RequestObject request, [FromUri] string t)
	{
		var loadSettings = LoadSettings(request, t);

		if (!loadSettings.Success)
		{
			return new FileService.ReturnObject(loadSettings);
		}

		if (WebConfigSettings.FileServiceRejectFishyPosts)
		{
			if (SiteUtils.IsFishyPost(Request))
			{
				return new FileService.ReturnObject(ReturnResult(OpResult.Denied));
			}
		}

		if (
			_siteSettings is null ||
			_fileSystem is null ||
			!_fileSystem.UserHasBrowsePermission)
		{
			return new FileService.ReturnObject(ReturnResult(OpResult.Denied));
		}

		StringBuilder returnErrors = new StringBuilder();
		Dictionary<string, FileService.ReturnMessage> returnMessages = new Dictionary<string, FileService.ReturnMessage>();

		var localizedUserFolder = "/" + Resource.UserFolder;

		request.Path = request.Path?.Replace(localizedUserFolder, _fileSystem.Permission.UserFolder);
		request.NewItemPath = request.NewItemPath?.Replace(localizedUserFolder, _fileSystem.Permission.UserFolder);
		request.NewPath = request.NewPath?.Replace(localizedUserFolder, _fileSystem.Permission.UserFolder);
		request.Item = request.Item?.Replace(localizedUserFolder, _fileSystem.Permission.UserFolder);

		if (request.Items != null && request.Items.Count > 0)
		{
			request.Items = request.Items.Select(s => s?.Replace(localizedUserFolder, _fileSystem.Permission.UserFolder)).ToList();
		}

		var canUpload = _fileSystem.UserHasUploadPermission;
		var canDelete = canUpload && (
			WebUser.IsAdminOrContentAdmin ||
			WebUser.IsInRoles(_siteSettings.RolesThatCanDeleteFilesInEditor) ||
			!string.IsNullOrWhiteSpace(_fileSystem.Permission.UserFolder) &&
			_fileSystem.Permission.UserFolder == _fileSystem.VirtualRoot
		);

		return request.Action switch
		{
			"rename" => canUpload ? RenameItem(request.Item, request.NewItemPath) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			"move" => canUpload ? MoveItem(request.Items, request.NewPath, returnMessages, returnErrors) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			"copy" => canUpload ? CopyItem(request.Items, request.SingleFileName, request.NewPath, returnMessages, returnErrors) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			"remove" => canDelete ? RemoveItem(request.Items, returnMessages, returnErrors) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			"edit" => canUpload ? EditItem(request.Item, request.Content) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			"getContent" => GetFileContent(request.Item),
			"createFolder" => canUpload ? CreateFolder(request.NewPath) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			// There is no infrastructure to edit permissions... yet.
			//case "changePermissions":
			"compress" => canUpload ? CompressItems(request.Items, request.Destination, request.CompressedFilename) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			"extract" => canUpload ? ExtractItems(request.Item, request.Destination, request.FolderName) : new FileService.ReturnObject(ReturnResult(OpResult.Denied)),
			_ => ListAllFilesFolders(request.Path),
		};
	}

	// GET: /fileservice
	[HttpGet]
	public HttpResponseMessage Get([FromUri] FileService.RequestObject request, string t)
	{
		var loadSettings = LoadSettings(request, t);

		if (!loadSettings.Success)
		{
			_log.Info(loadSettings.Error);
			throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}

		if (_fileSystem == null || !_fileSystem.UserHasBrowsePermission)
		{
			throw new HttpResponseException(HttpStatusCode.Unauthorized);
		}

		return request.Action switch
		{
			"downloadMultiple" => DownloadMultiple(request.Items, request.ToFilename),
			_ => DownloadItem(request.Path),
		};
	}


	private FileService.ReturnMessage LoadSettings(FileService.RequestObject request, string t)
	{
		_allowEditing = WebConfigSettings.AllowFileEditInFileManager;
		_overwriteExistingFiles = WebConfigSettings.FileManagerOverwriteFiles;

		_siteSettings = CacheHelper.GetCurrentSiteSettings();
		if (_siteSettings == null)
		{
			return new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemSiteSettingsNotLoaded };
		}

		if (WebConfigSettings.RequireFileSystemServiceToken && t != Global.FileSystemToken.ToString())
		{
			_log.Info(Resource.FileSystemInvalidToken);
			return new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemInvalidToken };
		}

		var p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

		if (p == null)
		{
			_log.Error(string.Format(Resource.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider));
			return new FileService.ReturnMessage { Success = false, Error = string.Format(Resource.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider) };
		}

		_fileSystem = p.GetFileSystem();
		if (_fileSystem == null)
		{
			_log.Error(string.Format(Resource.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider));
			return new FileService.ReturnMessage { Success = false, Error = string.Format(Resource.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider) };
		}

		_virtualPath = _fileSystem.VirtualRoot;

		if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor() || WebUser.IsInRoles(_siteSettings.GeneralBrowseAndUploadRoles + _siteSettings.GeneralBrowseRoles))
		{
			_allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
		}
		else if (WebUser.IsInRoles(_siteSettings.UserFilesBrowseAndUploadRoles))
		{
			_currentUser = SiteUtils.GetCurrentSiteUser();

			if (_currentUser == null)
			{
				return new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemUserNotAuthorized };
			}

			_allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
		}

		if (WebConfigSettings.LogAllFileServiceRequests)
		{
			var message = new StringBuilder();

			message.AppendLine("\nFile Manager Activity:");
			message.AppendFormat("Request Action: {0}\n", request.Action);

			if (request.CompressedFilename != null)
				message.AppendFormat("CompressedFilename: {0}\n", request.CompressedFilename);
			if (request.Content != null)
				message.AppendFormat("Content: {0}\n", request.Content);
			if (request.Destination != null)
				message.AppendFormat("Destination: {0}\n", request.Destination);
			if (request.Item != null)
				message.AppendFormat("Item: {0}\n", request.Item);
			if (request.Items != null)
				message.AppendFormat("Items: {0}\n", request.Items);
			if (request.NewItemPath != null)
				message.AppendFormat("NewItemPath: {0}\n", request.NewItemPath);
			if (request.NewPath != null)
				message.AppendFormat("NewPath: {0}\n", request.NewPath);
			if (request.FolderName != null)
				message.AppendFormat("FolderName: {0}\n", request.FolderName);
			if (request.Path != null)
				message.AppendFormat("Path: {0}\n", request.Path);
			if (request.Perms != null)
				message.AppendFormat("Perms: {0}\n", request.Perms);
			if (request.PermsCode != null)
				message.AppendFormat("PermsCode: {0}\n", request.PermsCode);
			if (request.Recursive != null)
				message.AppendFormat("Recursive: {0}\n", request.Recursive);
			if (request.SingleFileName != null)
				message.AppendFormat("SingleFileName: {0}\n", request.SingleFileName);
			if (request.ToFilename != null)
				message.AppendFormat("ToFilename: {0}\n", request.ToFilename);

			_log.Info(message);
		}

		return new FileService.ReturnMessage { Success = true };
	}


	private dynamic ListAllFilesFolders(string requestPath)
	{
		var filePath = FilePath(requestPath);
		var files = _fileSystem.GetFileList(filePath).Select(x => x.ToDto()).ToList();
		var folders = _fileSystem.GetFolderList(filePath).Select(x => x.ToDto()).ToList();
		var allowedFiles = new List<FileServiceDto>();

		if (
			requestPath == "/" &&
			!string.IsNullOrWhiteSpace(_fileSystem.Permission.UserFolder) &&
			_fileSystem.Permission.UserFolder != _fileSystem.VirtualRoot
		)
		{
			folders.Add(new WebFolder
			{
				VirtualPath = _fileSystem.Permission.UserFolder,
				Path = _fileSystem.Permission.UserFolder,
				Created = DateTime.Now,
				Modified = DateTime.Now,
				Name = Resource.UserFolder
			}.ToDto());
		}

		var type = WebUtils.ParseStringFromQueryString("type", "file");

		foreach (var file in files)
		{
			// Elijah Fowler - 2025-08-24
			// I believe that this is if the listing function needs to filter files based on their type.
			// E.g. I need a list of all PDFs only.
			// If there's no "type" specified in the query params, all files are shown.
			// TODO: Not sure if this is good, we should re-evaluate at at later time.
			if (
				type == "image" && !file.IsWebImageFile() ||
				(type == "media" || type == "audio" || type == "video") && !file.IsAllowedMediaFile() ||
				type == "audio" && !file.IsAllowedFileType(WebConfigSettings.AudioFileExtensions) ||
				type == "video" && !file.IsAllowedFileType(WebConfigSettings.VideoFileExtensions) ||
				type == "file" && !file.IsAllowedFileType(_allowedExtensions)
			)
			{
				continue;
			}

			allowedFiles.Add(file);
		}

		return new { result = folders.Concat(allowedFiles) };
	}


	private dynamic RenameItem(string item, string newItemPath)
	{
		return new FileService.ReturnObject(MoveOrRename(item, newItemPath));
	}


	private dynamic MoveItem(List<string> items, string newPath, Dictionary<string, FileService.ReturnMessage> returnMessages, StringBuilder sbErrors)
	{
		foreach (string item in items)
		{
			FileService.ReturnMessage returnMessage = MoveOrRename(item, newPath, true);

			if (!returnMessage.Success)
			{
				returnMessages.Add(item, returnMessage);
			}
		}

		return ReturnMessages(returnMessages, sbErrors);
	}


	private dynamic CopyItem(List<string> items, string singleFileName, string newPath, Dictionary<string, FileService.ReturnMessage> returnMessages, StringBuilder sbErrors)
	{
		foreach (string item in items)
		{
			OpResult result = OpResult.FileTypeNotAllowed;
			string file = FilePath(item);

			if (AllowedExtension(file))
			{
				string newFile = FilePath(newPath + "/" + CleanFileName(singleFileName, "file"));

				result = _fileSystem.CopyFile(file, newFile, _overwriteExistingFiles);

				if (result != OpResult.Succeed)
				{
					returnMessages.Add(file, ReturnResult(result));
				}
			}
			else
			{
				returnMessages.Add(file, ReturnResult(result));
			}
		}

		return ReturnMessages(returnMessages, sbErrors);
	}


	private dynamic RemoveItem(List<string> items, Dictionary<string, FileService.ReturnMessage> returnMessages, StringBuilder sbErrors)
	{
		foreach (var item in items)
		{
			OpResult result = OpResult.FileTypeNotAllowed;

			switch (FolderOrFile(item))
			{
				case "file":
					if (AllowedExtension(item))
					{
						result = _fileSystem.DeleteFile(FilePath(item));
					}
					else
					{
						returnMessages.Add(item, ReturnResult(result));
					}
					break;
				case "folder":
					result = _fileSystem.DeleteFolder(FilePath(item, false, true));
					break;
			}

			if (result != OpResult.Succeed)
			{
				returnMessages.Add(item, ReturnResult(result));
			}
		}

		return ReturnMessages(returnMessages, sbErrors);
	}


	private dynamic EditItem(string item, string content)
	{
		if (_allowEditing)
		{
			if (AllowedExtension(item))
			{
				try
				{
					string diskFilePath = FilePath(item, true);
					if (Path.GetExtension(diskFilePath).Equals(".svg", StringComparison.OrdinalIgnoreCase))
					{
						content = Core.Helpers.XmlSanitizer.RemoveScripts(content);
					}

					using (StreamWriter writer = File.CreateText(diskFilePath))
					{
						writer.Write(content);
					}

					return new FileService.ReturnObject(ReturnResult(OpResult.Succeed));
				}
				catch (Exception ex)
				{
					_log.Error(ex);
					return new FileService.ReturnObject(ReturnResult(OpResult.Error));
				}
			}

			return new FileService.ReturnObject(ReturnResult(OpResult.FileTypeNotAllowed));
		}

		return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.FileEditInFileManagerNotAllowed });
	}


	private dynamic GetFileContent(string item)
	{
		if (_allowEditing)
		{
			if (AllowedExtension(item))
			{
				string filePath = FilePath(item, true);

				if (File.Exists(filePath))
				{
					string fileBody = File.ReadAllText(filePath, Encoding.UTF8);

					return new { Result = fileBody };
				}

				return new { Result = Resource.FileSystemFileNotFound };
			}

			return new { Result = Resource.FileTypeNotAllowed };
		}

		return new { Result = Resource.FileEditInFileManagerNotAllowed };
	}


	private dynamic CreateFolder(string path)
	{
		OpResult result = OpResult.FolderLimitExceed;

		if (_fileSystem.CountFolders() < _fileSystem.Permission.MaxFolders)
		{
			string dir = VirtualPathUtility.GetDirectory(path);
			string newFolder = dir + CleanFileName(path, "folder");

			result = _fileSystem.CreateFolder(FilePath(newFolder, false, true));

			if (result != OpResult.Succeed)
			{
				return new FileService.ReturnObject(ReturnResult(result));
			}
		}

		return new FileService.ReturnObject(ReturnResult(result));
	}


	private FileService.ReturnObject CompressItems(List<string> items, string destination, string compressedFilename)
	{
		try
		{
			using (ZipFile zip = new ZipFile())
			{
				foreach (var item in items)
				{
					string path = FilePath(item, true);

					switch (FolderOrFile(item))
					{
						case "folder":
							zip.AddDirectory(path, CleanFileName(item, "folder"));
							break;
						case "file":
							if (AllowedExtension(item))
							{
								zip.AddFile(path, "");
							}
							break;
					}
				}

				if (!compressedFilename.EndsWith(".zip"))
				{
					compressedFilename += ".zip";
				}

				zip.Save(FilePath(destination + "/" + CleanFileName(compressedFilename, "file"), true));
				zip.Dispose();
			}

			return new FileService.ReturnObject(ReturnResult(OpResult.Succeed));
		}
		catch (Exception ex)
		{
			_log.Error(ex);
			return new FileService.ReturnObject(ReturnResult(OpResult.Error));
		}
	}


	private dynamic ExtractItems(string item, string destination, string folderName)
	{
		try
		{
			if (!AllowedExtension(item) || !Path.GetExtension(item).Equals(".zip", StringComparison.OrdinalIgnoreCase))
			{
				return new FileService.ReturnObject(ReturnResult(OpResult.FileTypeNotAllowed));
			}

			string cleanFolderName = CleanFileName(folderName, "folder");
			string targetExtractDir = FilePath(destination + "/" + cleanFolderName, true);

			using (ZipFile zip = ZipFile.Read(FilePath(item, true)))
			{

				foreach (ZipEntry e in zip.EntriesSorted)
				{
					if (e.IsDirectory)
					{
						List<string> dirs = e.FileName.SplitOnCharAndTrim('/');
						List<string> cleanDirs = new List<string>();

						foreach (string d in dirs)
						{
							cleanDirs.Add(d.ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation));
						}

						e.FileName = string.Join("/", cleanDirs);
					}
					else
					{
						if (!AllowedExtension(e.FileName))
						{
							continue;
						}

						string file = e.FileName.Contains("/") ? e.FileName.Substring(e.FileName.LastIndexOf('/')) : e.FileName;
						string dir = e.FileName.Substring(0, e.FileName.Contains("/") ? e.FileName.LastIndexOf('/') : 0);
						List<string> dirs = dir.SplitOnCharAndTrim('/');
						List<string> cleanDirs = new List<string>();

						foreach (string d in dirs)
						{
							cleanDirs.Add(d.ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation));
						}

						file = file.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

						e.FileName = string.Join("/", cleanDirs) + "/" + file;
					}

					if (!IOHelper.IsSafeZipEntryPath(targetExtractDir, e.FileName))
					{
						continue;
					}

					e.Extract(targetExtractDir, _overwriteExistingFiles ? ExtractExistingFileAction.OverwriteSilently : ExtractExistingFileAction.DoNotOverwrite);

					if (!e.IsDirectory && Path.GetExtension(e.FileName).Equals(".svg", StringComparison.OrdinalIgnoreCase))
					{
						string extractedFilePath = Path.GetFullPath(Path.Combine(targetExtractDir, e.FileName.TrimStart('/', '\\')));
						if (File.Exists(extractedFilePath))
						{
							string sanitizedSvg = Core.Helpers.XmlSanitizer.RemoveScripts(File.ReadAllText(extractedFilePath, Encoding.UTF8));
							File.WriteAllText(extractedFilePath, sanitizedSvg, Encoding.UTF8);
						}
					}
				}
			}

			return new FileService.ReturnObject(ReturnResult(OpResult.Succeed));
		}
		catch (Exception ex)
		{
			_log.Error(ex);
			return new FileService.ReturnObject(ReturnResult(OpResult.Error));
		}
	}


	private HttpResponseMessage DownloadItem(string path)
	{
		var newpath = path.Replace($"/{Resource.UserFolder}", _fileSystem.Permission.UserFolder);

		if (!AllowedExtension(newpath))
		{
			throw new HttpResponseException(HttpStatusCode.Forbidden);
		}

		try
		{
			var result = new HttpResponseMessage(HttpStatusCode.OK);
			var stream = new FileStream(FilePath(newpath, true), FileMode.Open, FileAccess.Read, FileShare.Read);

			result.Content = new StreamContent(stream);
			result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(path));

			result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
			{
				FileName = VirtualPathUtility.GetFileName(newpath)
			};

			return result;
		}
		catch (Exception ex)
		{
			_log.Error(ex);
			throw new HttpResponseException(HttpStatusCode.InternalServerError);
		}
	}


	public HttpResponseMessage DownloadMultiple(List<string> items, string toFilename)
	{
		using (var zipFile = new ZipFile())
		{
			foreach (var item in items)
			{
				var resolvedItem = item.Replace($"/{Resource.UserFolder}", _fileSystem.Permission.UserFolder);
				if (AllowedExtension(resolvedItem))
				{
					zipFile.AddFile(FilePath(resolvedItem, true), "");
				}
			}

			return ZipContentResult(zipFile, toFilename);
		}
	}


	// Utilities
	private HttpResponseMessage ZipContentResult(ZipFile zipFile, string fileName)
	{
		var pushStreamContent = new PushStreamContent((stream, content, context) =>
		{
			zipFile.Save(stream);
			stream.Close(); // After save we close the stream to signal that we are done writing.
		}, "application/zip");

		var result = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = pushStreamContent,
		};

		result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
		{
			FileName = fileName
		};

		return result;
	}

	private FileService.ReturnMessage MoveOrRename(string origin, string dest, bool move = false)
	{
		try
		{
			OpResult result = OpResult.NotFound;
			string dir = VirtualPathUtility.GetDirectory(dest);
			string destination = dir + Path.GetFileName(dest);

			if (!move)
			{
				switch (FolderOrFile(origin))
				{
					case "folder":
						result = _fileSystem.MoveFolder(FilePath(origin, false, true), FilePath(dir + "/" + CleanFileName(destination, "folder"), false, true));

						break;

					case "file":
						if (AllowedExtension(origin) && AllowedExtension(dest))
						{
							result = _fileSystem.MoveFile(FilePath(origin), FilePath(dir + "/" + CleanFileName(dest, "file")), _overwriteExistingFiles);
						}
						else
						{
							result = OpResult.FileTypeNotAllowed;
						}

						break;
				}
			}
			else
			{
				switch (FolderOrFile(origin))
				{
					case "folder":
						string org = FilePath(origin, false, true);
						string des = FilePath(destination + "/" + CleanFileName(origin, "folder"), false, true);
						result = _fileSystem.MoveFolder(org, des);

						break;

					case "file":
						if (AllowedExtension(origin) && FolderOrFile(dest) == "folder")
						{
							result = _fileSystem.MoveFile(FilePath(origin), FilePath(destination + "/" + CleanFileName(origin, "file")), false);
						}
						else
						{
							result = OpResult.FileTypeNotAllowed;
						}

						break;
				}
			}

			return ReturnResult(result);
		}
		catch (Exception ex)
		{
			_log.Error(ex);
			return new FileService.ReturnMessage { Success = false, Error = ex.ToString() };
		}
	}


	private FileService.ReturnMessage ReturnResult(OpResult result)
	{
		if (result != OpResult.Succeed)
		{
			string resultText = string.Empty;

			switch (result)
			{
				case OpResult.AlreadyExist:
					resultText = Resource.FileSystemFileExists;
					break;
				case OpResult.Denied:
					resultText = Resource.AccessDenied;
					break;
				case OpResult.Error:
				default:
					resultText = Resource.GenericError;
					break;
				case OpResult.FileLimitExceed:
					resultText = Resource.FileSystemFileLimitReached;
					break;
				case OpResult.FileSizeLimitExceed:
					resultText = Resource.FileSystemFileSizeLimitReached;
					break;
				case OpResult.FileTypeNotAllowed:
					resultText = Resource.FileTypeNotAllowed;
					break;
				case OpResult.FolderLimitExceed:
					resultText = Resource.FileSystemFolderLimitReached;
					break;
				case OpResult.FolderNotFound:
					resultText = Resource.FileSystemFolderNotFound;
					break;
				case OpResult.NotFound:
					resultText = Resource.FileSystemFileNotFound;
					break;
				case OpResult.QuotaExceed:
					resultText = Resource.FileSystemQuotaExceeded;
					break;
			}

			return new FileService.ReturnMessage { Success = false, Error = resultText };
		}
		else
		{
			return new FileService.ReturnMessage { Success = true };
		}
	}


	private FileService.ReturnObject ReturnMessages(Dictionary<string, FileService.ReturnMessage> returnMessages, StringBuilder sbErrors)
	{
		foreach (var msg in returnMessages)
		{
			sbErrors.AppendLine(string.Format("{0}, {1}", msg.Key, msg.Value.Error));
		}

		if (returnMessages.Count > 0)
		{
			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = sbErrors.ToString() });
		}

		return new FileService.ReturnObject(new FileService.ReturnMessage { Success = true });
	}


	private string FolderOrFile(string item)
	{
		bool dir = Directory.Exists(FilePath(item, true));
		bool file = File.Exists(FilePath(item, true));
		string result = string.Empty;

		if (dir)
		{
			result = "folder";
		}

		if (file)
		{
			result = "file";
		}

		return result;
	}


	private string FilePath(string itemPath, bool returnDiskPath = false, bool appendTrailingSlash = false, bool isFullPath = false)
	{
		Regex onlyOneSlashRegEx = new Regex("(/)(?<=\\1\\1)");

		if (string.IsNullOrEmpty(itemPath))
		{
			return itemPath;
		}

		// Normalize backslashes first, then repeatedly strip ".." to prevent order-of-operations bypasses
		itemPath = itemPath.Replace("\\", "/").Trim();
		while (itemPath.Contains(".."))
		{
			itemPath = itemPath.Replace("..", string.Empty);
		}

		var userFolder = _fileSystem.Permission.UserFolder;

		if (!string.IsNullOrWhiteSpace(userFolder) && itemPath.StartsWith(userFolder, StringComparison.OrdinalIgnoreCase))
		{
			if (!_fileSystem.FolderExists(_fileSystem.Permission.UserFolder))
			{
				_fileSystem.CreateFolder(_fileSystem.Permission.UserFolder);
			}

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

		if (!isWithinAllowedRoot && !string.IsNullOrWhiteSpace(userFolder))
		{
			string allowedUserDiskPath = Path.GetFullPath(HttpContext.Current.Server.MapPath(userFolder))
				.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			isWithinAllowedRoot = diskPathWithSep.StartsWith(allowedUserDiskPath, StringComparison.OrdinalIgnoreCase);
		}

		if (!isWithinAllowedRoot)
		{
			throw new UnauthorizedAccessException("Attempted to access a path outside of the allowed file manager root.");
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

	private bool AllowedExtension(string item)
	{
		return _fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(item));
	}

	private string CleanFileName(string item, string type)
	{
		switch (type)
		{
			case "folder":
				return Path.GetFileName(item).ToCleanFolderName(WebConfigSettings.ForceLowerCaseForFolderCreation);
			case "file":
			default:
				return Path.GetFileName(item).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
		}
	}
}
