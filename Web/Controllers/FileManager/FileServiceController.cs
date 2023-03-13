using AutoMapper;
using DotNetOpenAuth.OpenId.Provider;
using Ionic.Zip;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Mappers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Dtos;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Models;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace mojoPortal.Web.Controllers
{
	public class FileServiceController : ApiController
	{
		IFileSystem fileSystem = null;
		private bool allowEditing = false;
		private SiteUser currentUser = null;
		private SiteSettings siteSettings = null;
		private string virtualPath = string.Empty;
		protected bool overwriteExistingFiles = false;
		private string allowedExtensions = string.Empty;
		private static readonly ILog log = LogManager.GetLogger(typeof(FileServiceController));


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

			if ((fileSystem == null) || (!fileSystem.UserHasBrowsePermission))
			{
				return new FileService.ReturnObject(ReturnResult(OpResult.Denied));
			}

			StringBuilder returnErrors = new StringBuilder();
			Dictionary<string, FileService.ReturnMessage> returnMessages = new Dictionary<string, FileService.ReturnMessage>();

			var localizedUserFolder = "/" + Resource.UserFolder;

			request.Path = request.Path.Replace(localizedUserFolder, fileSystem.Permission.UserFolder);
			request.NewItemPath = request.NewItemPath.Replace(localizedUserFolder, fileSystem.Permission.UserFolder);
			request.NewPath = request.NewPath.Replace(localizedUserFolder, fileSystem.Permission.UserFolder);
			request.Item = request.Item.Replace(localizedUserFolder, fileSystem.Permission.UserFolder);

			if (request.Items.Count > 0)
			{
				request.Items = request.Items.Select(s => s.Replace(localizedUserFolder, fileSystem.Permission.UserFolder)).ToList();
			}

			switch (request.Action)
			{
				case "list":
				default:
					return ListAllFilesFolders(request.Path);

				case "rename":
					return RenameItem(request.Item, request.NewItemPath);

				case "move":
					return MoveItem(request.Items, request.NewPath, returnMessages, returnErrors);

				case "copy":
					return CopyItem(request.Items, request.SingleFileName, request.NewPath, returnMessages, returnErrors);

				case "remove":
					return RemoveItem(request.Items, returnMessages, returnErrors);

				case "edit":
					return EditItem(request.Item, request.Content);

				case "getContent":
					return GetFileContent(request.Item);

				case "createFolder":
					return CreateFolder(request.NewPath);

				// There is no infrastructure to edit permissions... yet.
				//case "changePermissions":

				case "compress":
					return CompressItems(request.Items, request.Destination, request.CompressedFilename);

				case "extract":
					return ExtractItems(request.Item, request.Destination, request.FolderName);
			};
		}

		// GET: /fileservice
		[HttpGet]
		public HttpResponseMessage Get([FromUri] FileService.RequestObject request, string t)
		{
			var loadSettings = LoadSettings(request, t);

			if (!loadSettings.Success)
			{
				log.Info(loadSettings.Error);
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			if ((fileSystem == null) || (!fileSystem.UserHasBrowsePermission))
			{
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}

			switch (request.Action)
			{
				case "download":
				default:
					return DownloadItem(request.Path);

				case "downloadMultiple":
					return DownloadMultiple(request.Items, request.ToFilename);
			}
		}


		private FileService.ReturnMessage LoadSettings(FileService.RequestObject request, string t)
		{
			allowEditing = WebConfigSettings.AllowFileEditInFileManager;
			overwriteExistingFiles = WebConfigSettings.FileManagerOverwriteFiles;

			siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null)
			{
				return new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemSiteSettingsNotLoaded };
			}

			if (WebConfigSettings.RequireFileSystemServiceToken && (t != Global.FileSystemToken.ToString()))
			{
				log.Info(Resource.FileSystemInvalidToken);
				return new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemInvalidToken };
			}

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error(string.Format(Resource.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider));
				return new FileService.ReturnMessage { Success = false, Error = string.Format(Resource.FileSystemProviderNotLoaded, WebConfigSettings.FileSystemProvider) };
			}

			fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error(string.Format(Resource.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider));
				return new FileService.ReturnMessage { Success = false, Error = string.Format(Resource.FileSystemNotLoadedFromProvider, WebConfigSettings.FileSystemProvider) };
			}

			virtualPath = fileSystem.VirtualRoot;

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor() || WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles + siteSettings.GeneralBrowseRoles))
			{
				allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
			}
			else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
			{
				currentUser = SiteUtils.GetCurrentSiteUser();

				if (currentUser == null)
				{
					return new FileService.ReturnMessage { Success = false, Error = Resource.FileSystemUserNotAuthorized };
				}

				allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
			}

			if (WebConfigSettings.LogAllFileServiceRequests)
			{
				StringBuilder message = new StringBuilder();

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

				log.Info(message);
			}

			return new FileService.ReturnMessage { Success = true };
		}


		private dynamic ListAllFilesFolders(string requestPath)
		{
			var filePath = FilePath(requestPath);
			var files = fileSystem.GetFileList(filePath).Select(AutoMapperAdapter.Mapper.Map<WebFile, FileServiceDto>).ToList();
			var folders = fileSystem.GetFolderList(filePath).Select(AutoMapperAdapter.Mapper.Map<WebFolder, FileServiceDto>).ToList();
			var allowedFiles = new List<FileServiceDto>();


			if (
				requestPath == "/" &&
				!string.IsNullOrWhiteSpace(fileSystem.Permission.UserFolder) &&
				fileSystem.Permission.UserFolder != fileSystem.VirtualRoot
			)
			{
				var userFolder = new List<WebFolder>() {
					new WebFolder {
						VirtualPath = fileSystem.Permission.UserFolder,
						Path = fileSystem.Permission.UserFolder,
						Created = DateTime.Now,
						Modified = DateTime.Now,
						Name = Resource.UserFolder
					}
				};

				folders.AddRange(userFolder.Select(AutoMapperAdapter.Mapper.Map<WebFolder, FileServiceDto>).ToList());
			}

			var type = WebUtils.ParseStringFromQueryString("type", "file");

			foreach (var folder in folders)
			{
				folder.ContentType = "dir";
			}

			foreach (var file in files)
			{
				if ((type == "image") && !file.IsWebImageFile())
				{ continue; }
				if ((type == "media" || type == "audio" || type == "video") && !file.IsAllowedMediaFile())
				{ continue; }
				if ((type == "audio") && !file.IsAllowedFileType(WebConfigSettings.AudioFileExtensions))
				{ continue; }
				if ((type == "video") && !file.IsAllowedFileType(WebConfigSettings.VideoFileExtensions))
				{ continue; }
				if ((type == "file") && !file.IsAllowedFileType(allowedExtensions))
				{ continue; }

				file.ContentType = "file";
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

					result = fileSystem.CopyFile(file, newFile, overwriteExistingFiles);

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
							result = fileSystem.DeleteFile(FilePath(item));
						}
						else
						{
							returnMessages.Add(item, ReturnResult(result));
						}
						break;
					case "folder":
						result = fileSystem.DeleteFolder(FilePath(item, false, true));
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
			item = FilePath(item);

			if (allowEditing)
			{
				if (AllowedExtension(item))
				{
					try
					{
						using (StreamWriter writer = File.CreateText(FilePath(item, true, false, true)))
						{
							writer.Write(content);
						}

						return new FileService.ReturnObject(ReturnResult(OpResult.Succeed));
					}
					catch (Exception ex)
					{
						log.Error(ex);
						return new FileService.ReturnObject(ReturnResult(OpResult.Error));
					}
				}

				return new FileService.ReturnObject(ReturnResult(OpResult.FileTypeNotAllowed));
				;
			}

			return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = Resource.FileEditInFileManagerNotAllowed });
		}


		private dynamic GetFileContent(string item)
		{
			if (allowEditing)
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

			if (fileSystem.CountFolders() < fileSystem.Permission.MaxFolders)
			{
				string dir = VirtualPathUtility.GetDirectory(path);
				string newFolder = dir + CleanFileName(path, "folder");

				result = fileSystem.CreateFolder(FilePath(newFolder, false, true));

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
								zip.AddFile(path, "");
								break;
						}
					}

					zip.Save(FilePath(destination + "/" + CleanFileName(compressedFilename, "file"), true));
					zip.Dispose();
				}

				return new FileService.ReturnObject(ReturnResult(OpResult.Succeed));
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return new FileService.ReturnObject(ReturnResult(OpResult.Error));
			}
		}


		private dynamic ExtractItems(string item, string destination, string folderName)
		{
			try
			{
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

						e.Extract(FilePath(destination + "/" + folderName, true), overwriteExistingFiles ? ExtractExistingFileAction.OverwriteSilently : ExtractExistingFileAction.DoNotOverwrite);

					}
				}

				return new FileService.ReturnObject(ReturnResult(OpResult.Succeed));
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return new FileService.ReturnObject(ReturnResult(OpResult.Error));
			}
		}


		private HttpResponseMessage DownloadItem(string path)
		{
			var newpath = path.Replace($"/{Resource.UserFolder}", fileSystem.Permission.UserFolder);

			try
			{
				var result = new HttpResponseMessage(HttpStatusCode.OK);
				var stream = new FileStream(FilePath(newpath, true), FileMode.Open);

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
				log.Error(ex);
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}
		}


		public HttpResponseMessage DownloadMultiple(List<string> items, string toFilename)
		{
			using (var zipFile = new ZipFile())
			{
				foreach (var item in items)
				{
					zipFile.AddFile(FilePath(item, true), "");
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
							result = fileSystem.MoveFolder(FilePath(origin, false, true), FilePath(dir + "/" + CleanFileName(destination, "folder"), false, true));

							break;

						case "file":
							if (AllowedExtension(origin) && AllowedExtension(dest))
							{
								result = fileSystem.MoveFile(FilePath(origin), FilePath(dir + "/" + CleanFileName(dest, "file")), overwriteExistingFiles);
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
							result = fileSystem.MoveFolder(org, des);

							break;

						case "file":
							if (AllowedExtension(origin) && FolderOrFile(dest) == "folder")
							{
								result = fileSystem.MoveFile(FilePath(origin), FilePath(destination + "/" + CleanFileName(origin, "file")), false);
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
				log.Error(ex);
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

			var userFolder = fileSystem.Permission.UserFolder;

			if (!string.IsNullOrWhiteSpace(userFolder) && itemPath.Contains(userFolder))
			{
				if (!Directory.Exists(fileSystem.Permission.UserFolder))
					fileSystem.CreateFolder(fileSystem.Permission.UserFolder);

				itemPath = itemPath.Replace(userFolder, fileSystem.Permission.UserFolder);
				isFullPath = true;
			}

			// Remove "../" or "\" to prevent hacks 
			itemPath = itemPath.Replace("..", string.Empty).Replace("\\", string.Empty).Trim();
			string fullPath = !isFullPath ? virtualPath + itemPath : itemPath;
			// Clean virtual path
			string cleanPath = onlyOneSlashRegEx.Replace(fullPath, string.Empty);
			if (appendTrailingSlash)
				cleanPath = VirtualPathUtility.AppendTrailingSlash(cleanPath);
			// Clean disk path
			string diskPath = HttpContext.Current.Server.MapPath(cleanPath);

			if (returnDiskPath)
				return diskPath;
			else
				return cleanPath;
		}

		private bool AllowedExtension(string item)
		{
			return fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(item));
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
}
