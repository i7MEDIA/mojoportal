using AutoMapper;
using Ionic.Zip;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
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
		public dynamic FileManagerPost(FileService.RequestObject request)
		{
			var loadSettings = LoadSettings();

			if (!loadSettings.Result)
			{
				return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = loadSettings.Error });
			}

			if ((fileSystem == null) || (!fileSystem.UserHasUploadPermission))
			{
				return new FileService.ReturnObject(ReturnResult(OpResult.Denied));
			}

			virtualPath = fileSystem.VirtualRoot;
			StringBuilder returnErrors = new StringBuilder();
			Dictionary<string, FileService.ReturnMessage> returnMessages = new Dictionary<string, FileService.ReturnMessage>();

			switch (request.Action)
			{
				case "list":
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
					return ExtractItem(request.Item, request.Destination);

				default:
					return new FileService.ReturnObject(new FileService.ReturnMessage { Success = false, Error = "Whoops!  Something went bad!" });
			};
		}

		// GET: /fileservice
		[HttpGet]
		public HttpResponse Get([FromUri] FileService.RequestObject request)
		{
			LoadSettings();
			var loadSettings = LoadSettings();

			if (!loadSettings.Result)
			{
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}

			if ((fileSystem == null) || (!fileSystem.UserHasUploadPermission))
			{
				throw new HttpResponseException(HttpStatusCode.Unauthorized);
			}

			virtualPath = fileSystem.VirtualRoot;

			switch (request.Action)
			{
				case "download":
				default:
					return DownloadItem(request.Path);

				case "downloadMultiple":
					return DownloadMultiple(request.Items, request.ToFilename);
			}
		}


		private dynamic LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				return new FileService.ReturnMessage { Success = false, Error = "Site Setting not loaded" };
			}

			allowEditing = WebConfigSettings.AllowFileEditInFileManager;
			//logAllFileSystemActivity = WebConfigSettings.LogAllFileManagerActivity;

			overwriteExistingFiles = WebConfigSettings.FileManagerOverwriteFiles;

			if (WebConfigSettings.RequireFileSystemServiceToken)
			{
				//Guid fileSystemToken = WebUtils.ParseGuidFromQueryString("t", Guid.Empty);
				string fileSystemToken = Request.GetQueryNameValuePairs().Where(nv => nv.Key == "t").Select(nv => nv.Value).FirstOrDefault();

				if (fileSystemToken != Global.FileSystemToken.ToString())
				{
					log.Info("Invalid token received by FileService so blocking access");
					return new FileService.ReturnMessage { Success = false, Error = "Invalid token received by FileService so blocking access" };
				}
			}

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
				return new FileService.ReturnMessage { Success = false, Error = "Could not load file system provider " + WebConfigSettings.FileSystemProvider };
			}

			fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
				return new FileService.ReturnMessage { Success = false, Error = "Could not load file system from provider " + WebConfigSettings.FileSystemProvider };
			}

			////log.Info(context.Request.RawUrl);
			//if (WebConfigSettings.LogAllFileServiceRequests)
			//{
			//	log.Info("virtualPath = " + virtualPath + " virtualSourcePath = " + virtualSourcePath + " virtualTargetPath = " + virtualTargetPath);
			//}

			if ((WebUser.IsAdminOrContentAdmin) || (SiteUtils.UserIsSiteEditor()))
			{
				allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
			}
			else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
			{
				allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
			}
			else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
			{
				currentUser = SiteUtils.GetCurrentSiteUser();

				if (currentUser == null)
				{
					return new FileService.ReturnMessage { Success = false, Error = "User is unauthorized." };
				}

				allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
			}

			return new { Result = true };
		}


		private dynamic ListAllFilesFolders(string requestPath)
		{
			var files = fileSystem.GetFileList(FilePath(requestPath)).Select(Mapper.Map<WebFile, FileServiceDto>).ToList();
			var allowedFiles = new List<FileServiceDto>();
			var folders = fileSystem.GetFolderList(FilePath(requestPath)).Select(Mapper.Map<WebFolder, FileServiceDto>).ToList();
			var type = WebUtils.ParseStringFromQueryString("type", "file");

			foreach (var folder in folders)
			{
				folder.ContentType = "dir";
			}

			foreach (var file in files)
			{
				if ((type == "image") && !file.IsWebImageFile()) { continue; }
				if ((type == "media" || type == "audio" || type == "video") && !file.IsAllowedMediaFile()) { continue; }
				if ((type == "audio") && !file.IsAllowedFileType(WebConfigSettings.AudioFileExtensions)) { continue; }
				if ((type == "video") && !file.IsAllowedFileType(WebConfigSettings.VideoFileExtensions)) { continue; }
				if ((type == "file") && !file.IsAllowedFileType(allowedExtensions)) { continue; }

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
					string newFile = FilePath(newPath + "/" + CleanFileName(singleFileName));

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
						using (StreamWriter writer = File.CreateText(FilePath(item, true)))
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
				else
				{
					return new FileService.ReturnObject(ReturnResult(OpResult.FileTypeNotAllowed)); ;
				}
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
					else
					{
						return new { Result = Resource.FileSystemFileNotFound };
					}
				}
				else
				{
					return new { Result = Resource.FileTypeNotAllowed };
				}
			}

			return new { Result = Resource.FileEditInFileManagerNotAllowed };
		}


		private dynamic CreateFolder(string path)
		{
			OpResult result = OpResult.FolderLimitExceed;

			if (fileSystem.CountFolders() < fileSystem.Permission.MaxFolders)
			{
				string dir = VirtualPathUtility.GetDirectory(path);
				string newFolder = dir + CleanFileName(path);

				result = fileSystem.CreateFolder(FilePath(newFolder, false, true));

				if (result != OpResult.Succeed)
				{
					return new FileService.ReturnObject(ReturnResult(result));
				}
			}

			return new FileService.ReturnObject(ReturnResult(result));
		}


		private dynamic CompressItems(List<string> items, string destination, string compressedFilename, bool streamFile = false)
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
								zip.AddDirectory(path, CleanFileName(item));
								break;
							case "file":
								zip.AddFile(path, "");
								break;
						}
					}

					zip.TempFileFolder = Path.GetTempPath();


					if (!streamFile)
					{
						zip.Save(FilePath(destination + "/" + CleanFileName(compressedFilename), true));
						zip.Dispose();
					}
					else
					{
						HttpResponse response = HttpContext.Current.Response;
						response.Clear();
						response.ContentType = "application/zip";
						response.AddHeader("content-disposition", "filename=" + "file-manager.zip");
						zip.Save(response.OutputStream);
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


		private dynamic ExtractItem(string item, string destination)
		{
			try
			{
				using (ZipFile zip = ZipFile.Read(FilePath(item, true)))
				{
					foreach (ZipEntry e in zip)
					{
						e.Extract(FilePath(destination, true), ExtractExistingFileAction.OverwriteSilently);
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


		private HttpResponse DownloadItem(string path)
		{
			try
			{
				if (WebConfigSettings.DownloadScriptTimeout > -1)
				{
					HttpContext.Current.Server.ScriptTimeout = WebConfigSettings.DownloadScriptTimeout;
				}

				HttpResponse response = HttpContext.Current.Response;
				response.ClearContent();
				response.Clear();
				response.ContentType = "text/plain";
				response.AddHeader("Content-Disposition", "attachment; filename=" + CleanFileName(path) + ";");

				response.Buffer = false;
				response.BufferOutput = false;

				using (Stream stream = fileSystem.GetAsStream(FilePath(path)))
				{
					stream.CopyTo(response.OutputStream);
				}

				response.End();

				return response;
			}
			catch (System.Threading.ThreadAbortException) {
				throw new HttpResponseException(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw new HttpResponseException(HttpStatusCode.InternalServerError);
			}
		}


		private HttpResponse DownloadMultiple(List<string> items, string toFilename)
		{
			HttpResponse response = HttpContext.Current.Response;

			response.ClearContent();
			response.Clear();
			response.ContentType = "application/zip";
			response.AddHeader("Content-Disposition", "attachment; filename=" + toFilename);

			response.Buffer = false;
			response.BufferOutput = false;

			using (ZipFile zip = new ZipFile())
			{
				foreach (var item in items)
				{
					zip.AddFile(FilePath(item, true), "");
				}

				zip.TempFileFolder = Path.GetTempPath();

				zip.Save(response.OutputStream);
			}

			response.End();

			return response;
		}


		// First Tool
		private FileService.ReturnMessage MoveOrRename(string origin, string dest, bool move = false)
		{
			try
			{
				OpResult result = OpResult.NotFound;
				string dir = VirtualPathUtility.GetDirectory(dest);
				string destination = dir + Path.GetFileName(dest);

				if (!move)
				{
					switch(FolderOrFile(origin))
					{
						case "folder":
							result = fileSystem.MoveFolder(FilePath(origin, false, true), FilePath(destination, false, true));

							break;
						case "file":
							if (AllowedExtension(origin) && AllowedExtension(dest))
							{
								result = fileSystem.MoveFile(FilePath(origin), FilePath(destination), overwriteExistingFiles);
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
					switch(FolderOrFile(origin))
					{
						case "folder":
							string org = FilePath(origin, false, true);
							string des = FilePath(destination + "/" + CleanFileName(origin), false, true);
							result = fileSystem.MoveFolder(org, des);
							break;
						case "file":
							if (AllowedExtension(origin) && FolderOrFile(dest) == "folder")
							{
								result = fileSystem.MoveFile(FilePath(origin), FilePath(destination + "/" + CleanFileName(origin)), false);
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


		private string FilePath(string itemPath, bool returnDiskPath = false, bool appendTrailingSlash = false)
		{
			Regex removeDoubleSlash = new Regex("(/)(?<=\\1\\1)");

			if (string.IsNullOrEmpty(itemPath))
			{
				return itemPath;
			}

			// Remove "../" or "\" to prevent hacks 
			itemPath = itemPath.Replace("..", string.Empty).Replace("\\", string.Empty).Trim();
			string concatedPath = virtualPath + itemPath;
			// Clean virtual path
			string cleanPath = removeDoubleSlash.Replace(concatedPath, String.Empty);
			if (appendTrailingSlash)
				cleanPath = VirtualPathUtility.AppendTrailingSlash(cleanPath);
			// Clean disk path
			string discPath = HttpContext.Current.Server.MapPath(cleanPath);

			if (returnDiskPath)
				return discPath;
			else 
				return cleanPath;
		}

		private bool AllowedExtension(string item)
		{
			return fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(item));
		}

		private string CleanFileName(string item)
		{
			return Path.GetFileName(item).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
		}
	}
}
