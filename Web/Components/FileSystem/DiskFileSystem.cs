// Author:					
// Created:				    2009-12-30
// Last Modified:			2017-09-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

// caution!!! I would not visit this link with javascript enabled, disable it 
// or use the NoScript plugin in Firefox to read the security bulletin
// 2010-09-16 fixed issue posted here: http://www.exploit-db.com/exploits/15018/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using mojoPortal.Web;
using mojoPortal.Web.Framework;


namespace mojoPortal.FileSystem
{
	public class DiskFileSystem : IFileSystem
	{
		private DiskFileSystem(IFileSystemPermission permission) 
		{
			this.permission = permission;
			fileBaseUrl = WebUtils.GetApplicationRoot();

			if (permission.VirtualRoot.Contains("userfiles"))
			{
				string userFolder = permission.VirtualRoot.Substring(0, permission.VirtualRoot.LastIndexOf("userfiles")) + "userfiles/";

				if (!FolderExists(userFolder))
				{
					CreateFolder(userFolder);
				}
			}

			if (!FolderExists(permission.VirtualRoot))
			{
				CreateFolder(permission.VirtualRoot);
			}



		}

		private IFileSystemPermission permission = null;
		private char displayPathSeparator = '|';
		private long size = -1;
		private int folderCount = -1;
		private int fileCount = -1;
		private string fileBaseUrl = string.Empty;

		public static DiskFileSystem GetFileSystem(IFileSystemPermission permission)
		{
			if (permission == null) { return null; }
			if(string.IsNullOrEmpty(permission.VirtualRoot)) { return null; }

			DiskFileSystem fs = new DiskFileSystem(permission);
			return fs;
		}

	   
		

		private static string GuessMime(string filePath)
		{
			var mime = IOHelper.GetMimeType(Path.GetExtension(filePath).ToLower());
			return mime ?? "application/x-unknown-content-type";
		}

		private void UpdateInfo()
		{
			size = 0;
			fileCount = 0;
			folderCount = 0;
			UpdateInfo(permission.VirtualRoot);
		}

		private void UpdateInfo(string virtualPath)
		{
			string fullPath = HostingEnvironment.MapPath(virtualPath);
			var files = Directory.GetFiles(fullPath);
			fileCount += files.Length;
			foreach (var filePath in files)
			{
				size += new FileInfo(filePath).Length;
			}
			var subfolders = Directory.GetDirectories(fullPath);
			folderCount += subfolders.Length;
			foreach (var folderPath in subfolders)
			{
				string virtualSubFolderPath = virtualPath + Path.GetFileName(folderPath) + "/";
				UpdateInfo(virtualSubFolderPath);
			}
		}

		private bool IsAllowed(string path)
		{
			if (path.Contains(".svn")) { return false; }

			return true;
		}

		private string MapPath(string virtualPath)
		{
			string fullPath;
			if (virtualPath.StartsWith("~"))
			{
				fullPath = HostingEnvironment.MapPath(virtualPath);
			}
			else
			{
				fullPath = HostingEnvironment.MapPath("~" + virtualPath);
			}

			return fullPath;
		}
		

		private string ResolvePathResult(string path)
		{
			if (Path.IsPathRooted(path))
			{
				string rootFolder = HostingEnvironment.MapPath(VirtualRoot);
				path = path.Substring(rootFolder.Length);
			}

			StringBuilder builder = new StringBuilder(path.Length);
			foreach (var c in path)
			{
				if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
					builder.Append(displayPathSeparator);
				else
					builder.Append(c);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Copies the contents of input to output. Doesn't close either stream.
		/// </summary>
		private static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[8 * 1024];
			int len;
			while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				output.Write(buffer, 0, len);
			}
		}
		
		#region IFileSystem Members

		public string FileBaseUrl
		{
			get { return fileBaseUrl; }
		}

		public IFileSystemPermission Permission
		{
			get { return permission; }
		}

		public bool UserHasUploadPermission
		{
			get { return permission.UserHasUploadPermission; }
		}

		public bool UserHasBrowsePermission
		{
			get { return permission.UserHasBrowsePermission || permission.UserHasUploadPermission; }
		}

		public string VirtualRoot
		{
			get { return permission.VirtualRoot; }
		}

		public bool FileExists(string virtualPath)
		{
			return File.Exists(MapPath(virtualPath));
		}

		public bool FolderExists(string virtualPath)
		{
			return Directory.Exists(MapPath(virtualPath));
		}

		public OpResult SaveFile(string virtualPath, Stream stream, string contentType, bool overWrite)
		{
			string fullPath = MapPath(virtualPath);
			if (File.Exists(fullPath))
			{
				if (overWrite)
					File.Delete(fullPath);
				else
					return OpResult.AlreadyExist;
			}

			using (stream)
			{
				using (Stream file = File.OpenWrite(fullPath))
				{
					CopyStream(stream, file);
				}
			}

			return OpResult.Succeed;
		}

		public OpResult SaveFile(string virtualPath, HttpPostedFile file, bool overWrite)
		{
			string fullPath = MapPath(virtualPath);
			if (file == null) { throw new ArgumentNullException("file"); }

			string filePath = Path.Combine(fullPath, Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));

			if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
				return OpResult.FolderNotFound;

			if (File.Exists(filePath))
			{
				if (overWrite)
					File.Delete(filePath);
				else
					return OpResult.AlreadyExist;
			}

			file.SaveAs(filePath);
			return OpResult.Succeed;
		}

		public Stream GetWritableStream(string virtualPath)
		{
			string fullPath = MapPath(virtualPath);
			return File.OpenWrite(fullPath);
		}

		
		public Stream GetAsStream(string virtualPath)
		{
			return File.OpenRead(MapPath(virtualPath));
		}

		public WebFile RetrieveFile(string virtualPath)
		{
			//string fullPath = GetPath(path);
			FileInfo info = new FileInfo(MapPath(virtualPath));
			if (info.Exists)
				return new WebFile()
				{
					VirtualPath = virtualPath,
					Path = info.FullName,
					FolderVirtualPath = virtualPath.Substring(0, virtualPath.LastIndexOf('/')),
					Name = info.Name,
					Size = info.Length,
					ContentType = GuessMime(info.Name),
					Created = info.CreationTimeUtc,
					Modified = info.LastWriteTimeUtc
				};

			// file not found
			return null;
		}

		public OpResult MoveFile(string virtualSourcePath, string virtualTargetPath, bool overWrite)
		{
			string srcPath = MapPath(virtualSourcePath);
			string destPath = MapPath(virtualTargetPath);

			if (!File.Exists(srcPath))
				return OpResult.NotFound;

			if (!Directory.Exists(Path.GetDirectoryName(destPath)))
				return OpResult.FolderNotFound;

			if (File.Exists(destPath))
			{
				if (overWrite)
					File.Delete(destPath);
				else
					return OpResult.AlreadyExist;
			}

			File.Move(srcPath, destPath);
			return OpResult.Succeed;
		}

		public OpResult CopyFile(string virtualSourcePath, string virtualTargetPath, bool overwrite)
		{
			string srcPath = MapPath(virtualSourcePath);
			string destPath = MapPath(virtualTargetPath);

			if (!File.Exists(srcPath))
				return OpResult.NotFound;

			if (!Directory.Exists(Path.GetDirectoryName(destPath)))
				return OpResult.FolderNotFound;

			if (File.Exists(destPath) && !overwrite)
			{
				return OpResult.AlreadyExist;
			}

			File.Copy(srcPath, destPath, overwrite);
			return OpResult.Succeed;
		}


		public OpResult DeleteFile(string virtualPath)
		{
			string fullPath = MapPath(virtualPath);
			if (!File.Exists(fullPath))
				return OpResult.NotFound;

			File.Delete(fullPath);
			return OpResult.Succeed;
		}

		public IEnumerable<WebFile> GetFileList(string virtualPath)
		{
			string fullPath = MapPath(virtualPath);
			
			if (!Directory.Exists(fullPath)) { return null; }
			int fullPathLen = fullPath.Length + 1;

			var filePaths = Directory.GetFiles(fullPath);
			var files = new List<WebFile>(filePaths.Length);
			foreach (var filePath in filePaths)
			{
				FileInfo file = new FileInfo(filePath);
				files.Add(new WebFile()
				{
					VirtualPath = Path.Combine(virtualPath, file.Name),
					Path = file.FullName,
					FolderVirtualPath = virtualPath.Substring(0, virtualPath.LastIndexOf('/')),
					Name = file.Name,
					Size = file.Length,
					ContentType = GuessMime(file.Name),
					Created = file.CreationTimeUtc,
					Modified = file.LastWriteTimeUtc
				});

			}
			return files;
		}

		public int CountAllFiles()
		{
			if (fileCount < 0)
				UpdateInfo();
			return fileCount;
		}

		public long GetTotalSize()
		{
			if (size < 0)
				UpdateInfo();
			return size;
		}

		public OpResult CreateFolder(string virtualPath)
		{
			string fullPath = MapPath(virtualPath);
			if (Directory.Exists(fullPath))
				return OpResult.AlreadyExist;
			Directory.CreateDirectory(fullPath);
			return OpResult.Succeed;
		}

		public OpResult MoveFolder(string sourceVirtualPath, string targetVirtualPath)
		{
			string srcPath = MapPath(sourceVirtualPath);
			string destPath = MapPath(targetVirtualPath);

			if (!Directory.Exists(srcPath))
				return OpResult.NotFound;

			if (Directory.Exists(destPath))
				return OpResult.AlreadyExist;

			Directory.Move(srcPath, destPath);
			return OpResult.Succeed;
		}

		public OpResult DeleteFolder(string virtualPath)
		{
			string fullPath = MapPath(virtualPath);
			if (!Directory.Exists(fullPath))
				return OpResult.NotFound;

			Directory.Delete(fullPath, true);
			return OpResult.Succeed;
		}

		public IEnumerable<WebFolder> GetAllFolders()
		{
			string fullPath = MapPath(permission.VirtualRoot);
			if (!Directory.Exists(fullPath)) { return null; }
			DirectoryInfo di = new DirectoryInfo(fullPath);

			//var folders = from folder in Directory.GetDirectories(permission.RootFolder, "*", SearchOption.AllDirectories)
			//              where IsAllowed(folder)
			//              select folder;

			var folders = from folder in di.GetDirectories("*", SearchOption.AllDirectories)
						  where IsAllowed(folder.Name)
						  select folder;

			return folders.Select(p => new WebFolder() 
			{ 
				Path = ResolvePathResult(p.FullName), 
				VirtualPath = permission.VirtualRoot + p.Name,
				Created = p.CreationTimeUtc,
				Modified = p.LastWriteTimeUtc,
				Name = p.Name 
			});

		}

		public IEnumerable<WebFolder> GetFolderList(string virtualPath)
		{
			string fullPath = MapPath(virtualPath); ;
			
			if (!Directory.Exists(fullPath)) { return null; }

			DirectoryInfo di = new DirectoryInfo(fullPath);
			
			var folders = from folder in di.GetDirectories("*", SearchOption.TopDirectoryOnly)
						  where IsAllowed(folder.Name)
						  select folder;

			return folders.Select(p => new WebFolder() 
				{   
					VirtualPath = virtualPath + p.Name,
					Path = ResolvePathResult(p.FullName), 
					Created = p.CreationTimeUtc,
					Modified = p.LastWriteTimeUtc,
					Name = p.Name 
				});

		}

		

		public int CountFolders()
		{
			if (folderCount < 0)
				UpdateInfo();
			return folderCount;
		}


		//public string CombinePath(string path1, string path2)
		//{
		//	return Path.Combine(path1, path2);
		//}

		public string CombinePath(params string[] paths)
		{
			return Path.Combine(paths);
		}
		#endregion


	}
}
