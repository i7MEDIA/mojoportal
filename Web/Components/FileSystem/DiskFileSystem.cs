#nullable enable
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace mojoPortal.FileSystem;

public class DiskFileSystem : IFileSystem
{
	private readonly IFileSystemPermission _permission;
	private readonly string _fileBaseUrl;
	private readonly char _displayPathSeparator = '|';
	private long _size = -1;
	private int _folderCount = -1;
	private int _fileCount = -1;

	private DiskFileSystem(IFileSystemPermission permission)
	{
		_permission = permission;
		_fileBaseUrl = WebUtils.GetApplicationRoot();

		if (permission.VirtualRoot.Contains("userfiles"))
		{
			var userFolder = permission.VirtualRoot.Substring(0, permission.VirtualRoot.LastIndexOf("userfiles")) + "userfiles/";

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

	public static DiskFileSystem? GetFileSystem(IFileSystemPermission permission)
	{
		if (permission is null || string.IsNullOrEmpty(permission.VirtualRoot))
		{
			return null;
		}

		return new DiskFileSystem(permission);
	}

	private static string GuessMime(string filePath)
	{
		var mime = IOHelper.GetMimeType(Path.GetExtension(filePath).ToLower());

		return mime ?? "application/x-unknown-content-type";
	}

	private void UpdateInfo()
	{
		_size = 0;
		_fileCount = 0;
		_folderCount = 0;
		UpdateInfo(_permission.VirtualRoot);
	}

	private void UpdateInfo(string virtualPath)
	{
		var fullPath = HostingEnvironment.MapPath(virtualPath);
		var files = Directory.GetFiles(fullPath);

		_fileCount += files.Length;

		foreach (var filePath in files)
		{
			_size += new FileInfo(filePath).Length;
		}

		var subfolders = Directory.GetDirectories(fullPath);

		_folderCount += subfolders.Length;

		foreach (var folderPath in subfolders)
		{
			var virtualSubFolderPath = $"{virtualPath}{Path.GetFileName(folderPath)}/";

			UpdateInfo(virtualSubFolderPath);
		}
	}

	private bool IsAllowed(string path)
	{
		if (path.Contains(".svn"))
		{
			return false;
		}

		return true;
	}

	private string MapPath(string virtualPath)
	{
		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			throw new ArgumentNullException(nameof(virtualPath));
		}

		var fullPath = virtualPath.StartsWith("~")
			? HostingEnvironment.MapPath(virtualPath)
			: HostingEnvironment.MapPath("~" + virtualPath);

		var canonicalFullPath = Path.GetFullPath(fullPath);
		var dataRootPath = Path.GetFullPath(HostingEnvironment.MapPath("~/Data/"))
			.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
		var canonicalTargetWithSep = canonicalFullPath
			.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;

		if (!canonicalTargetWithSep.StartsWith(dataRootPath, StringComparison.OrdinalIgnoreCase))
		{
			throw new UnauthorizedAccessException("Attempted to access a path outside of the Data directory.");
		}

		return canonicalFullPath;
	}

	private string ResolvePathResult(string path)
	{
		if (Path.IsPathRooted(path))
		{
			var rootFolder = HostingEnvironment.MapPath(VirtualRoot);

			path = path.Substring(rootFolder.Length);
		}

		var builder = new StringBuilder(path.Length);

		foreach (var character in path)
		{
			if (character == Path.DirectorySeparatorChar || character == Path.AltDirectorySeparatorChar)
			{
				builder.Append(_displayPathSeparator);
			}
			else
			{
				builder.Append(character);
			}
		}

		return builder.ToString();
	}

	/// <summary>
	/// Copies the contents of input to output. Doesn't close either stream.
	/// </summary>
	private static void CopyStream(Stream input, Stream output)
	{
		var buffer = new byte[8 * 1024];

		int len;

		while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
		{
			output.Write(buffer, 0, len);
		}
	}

	#region IFileSystem Members

	public string FileBaseUrl => _fileBaseUrl;
	public IFileSystemPermission Permission => _permission;
	public bool UserHasUploadPermission => _permission.UserHasUploadPermission;
	public bool UserHasBrowsePermission => _permission.UserHasBrowsePermission || _permission.UserHasUploadPermission;
	public string VirtualRoot => _permission.VirtualRoot;

	public bool FileExists(string virtualPath)
	{
		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			return false;
		}

		return File.Exists(MapPath(virtualPath));
	}

	public bool FolderExists(string virtualPath)
	{
		if (string.IsNullOrWhiteSpace(virtualPath))
		{
			return false;
		}

		return Directory.Exists(MapPath(virtualPath));
	}

	public OpResult SaveFile(string virtualPath, Stream stream, string contentType, bool overWrite)
	{
		if (virtualPath is null)
		{
			throw new ArgumentNullException("virtualPath", "virtualPath cannot be null");
		}

		if (virtualPath.Length == 0)
		{
			throw new ArgumentException("virtualPath must have a length greater than 0.");
		}

		var fullPath = MapPath(virtualPath);

		if (File.Exists(fullPath))
		{
			if (overWrite)
			{
				File.Delete(fullPath);
			}
			else
			{
				return OpResult.AlreadyExist;
			}
		}

		using (stream)
		{
			using Stream file = File.OpenWrite(fullPath);

			CopyStream(stream, file);
		}

		return OpResult.Succeed;
	}

	public OpResult SaveFile(string virtualPath, HttpPostedFile file, bool overWrite)
	{
		if (virtualPath is null)
		{
			throw new ArgumentNullException("virtualPath", "virtualPath cannot be null");
		}

		if (virtualPath.Length == 0)
		{
			throw new ArgumentException("virtualPath must have a length greater than 0.");
		}

		var fullPath = MapPath(virtualPath);

		if (file is null)
		{
			throw new ArgumentNullException("file");
		}

		var filePath = Path.Combine(fullPath, Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));

		if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
		{
			return OpResult.FolderNotFound;
		}

		if (File.Exists(filePath))
		{
			if (overWrite)
			{
				File.Delete(filePath);
			}
			else
			{
				return OpResult.AlreadyExist;
			}
		}

		file.SaveAs(filePath);

		return OpResult.Succeed;
	}

	public Stream GetWritableStream(string virtualPath)
	{
		if (virtualPath is null)
		{
			throw new ArgumentNullException("virtualPath", "virtualPath cannot be null");
		}

		if (virtualPath.Length == 0)
		{
			throw new ArgumentException("virtualPath must have a length greater than 0.");
		}

		return File.OpenWrite(MapPath(virtualPath));
	}

	public Stream GetAsStream(string virtualPath)
	{
		if (virtualPath is null)
		{
			throw new ArgumentNullException("virtualPath", "virtualPath cannot be null");
		}

		if (virtualPath.Length == 0)
		{
			throw new ArgumentException("virtualPath must have a length greater than 0.");
		}

		return File.OpenRead(MapPath(virtualPath));
	}

	public WebFile? RetrieveFile(string virtualPath)
	{
		var info = new FileInfo(MapPath(virtualPath));

		if (info.Exists)
		{
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
		}

		// file not found
		return null;
	}

	public OpResult MoveFile(string virtualSourcePath, string virtualTargetPath, bool overWrite)
	{
		if (virtualSourcePath is null)
		{
			throw new ArgumentNullException("virtualSourcePath", "virtualSourcePath cannot be null");
		}

		if (virtualSourcePath.Length == 0)
		{
			throw new ArgumentException("virtualSourcePath must have a length greater than 0.");
		}

		if (virtualTargetPath is null)
		{
			throw new ArgumentNullException("virtualTargetPath", "virtualTargetPath cannot be null");
		}

		if (virtualTargetPath.Length == 0)
		{
			throw new ArgumentException("virtualTargetPath must have a length greater than 0.");
		}

		var srcPath = MapPath(virtualSourcePath);
		var destPath = MapPath(virtualTargetPath);

		if (!File.Exists(srcPath))
		{
			return OpResult.NotFound;
		}

		if (!Directory.Exists(Path.GetDirectoryName(destPath)))
		{
			return OpResult.FolderNotFound;
		}

		if (File.Exists(destPath))
		{
			if (overWrite)
			{
				File.Delete(destPath);
			}
			else
			{
				return OpResult.AlreadyExist;
			}
		}

		File.Move(srcPath, destPath);

		return OpResult.Succeed;
	}

	public OpResult CopyFile(string virtualSourcePath, string virtualTargetPath, bool overwrite)
	{
		if (virtualSourcePath is null)
		{
			throw new ArgumentNullException("virtualSourcePath", "virtualSourcePath cannot be null");
		}

		if (virtualSourcePath.Length == 0)
		{
			throw new ArgumentException("virtualSourcePath must have a length greater than 0.");
		}

		if (virtualTargetPath is null)
		{
			throw new ArgumentNullException("virtualTargetPath", "virtualTargetPath cannot be null");
		}

		if (virtualTargetPath.Length == 0)
		{
			throw new ArgumentException("virtualTargetPath must have a length greater than 0.");
		}

		var srcPath = MapPath(virtualSourcePath);
		var destPath = MapPath(virtualTargetPath);

		if (!File.Exists(srcPath))
		{
			return OpResult.NotFound;
		}

		if (!Directory.Exists(Path.GetDirectoryName(destPath)))
		{
			return OpResult.FolderNotFound;
		}

		if (File.Exists(destPath) && !overwrite)
		{
			return OpResult.AlreadyExist;
		}

		File.Copy(srcPath, destPath, overwrite);

		return OpResult.Succeed;
	}

	public OpResult DeleteFile(string virtualPath)
	{
		if (virtualPath is null)
		{
			throw new ArgumentNullException("virtualPath", "virtualPath cannot be null");
		}

		if (virtualPath.Length == 0)
		{
			throw new ArgumentException("virtualPath must have a length greater than 0.");
		}

		var fullPath = MapPath(virtualPath);

		if (!File.Exists(fullPath))
		{
			return OpResult.NotFound;
		}

		File.Delete(fullPath);

		return OpResult.Succeed;
	}

	public IEnumerable<WebFile>? GetFileList(string virtualPath)
	{
		if (virtualPath is null)
		{
			throw new ArgumentNullException("virtualPath", "virtualPath cannot be null");
		}

		var fullPath = MapPath(virtualPath);

		if (!Directory.Exists(fullPath))
		{
			return null;
		}

		var filePaths = Directory.GetFiles(fullPath);
		var files = new List<WebFile>(filePaths.Length);

		foreach (var filePath in filePaths)
		{
			var file = new FileInfo(filePath);

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
		if (_fileCount < 0)
		{
			UpdateInfo();
		}

		return _fileCount;
	}

	public long GetTotalSize()
	{
		if (_size < 0)
		{
			UpdateInfo();
		}

		return _size;
	}

	public OpResult CreateFolder(string virtualPath)
	{
		var fullPath = MapPath(virtualPath);

		if (Directory.Exists(fullPath))
		{
			return OpResult.AlreadyExist;
		}

		Directory.CreateDirectory(fullPath);

		return OpResult.Succeed;
	}

	public OpResult MoveFolder(string sourceVirtualPath, string targetVirtualPath)
	{
		var srcPath = MapPath(sourceVirtualPath);
		var destPath = MapPath(targetVirtualPath);

		if (!Directory.Exists(srcPath))
		{
			return OpResult.NotFound;
		}

		if (Directory.Exists(destPath))
		{
			return OpResult.AlreadyExist;
		}

		Directory.Move(srcPath, destPath);

		return OpResult.Succeed;
	}

	public OpResult DeleteFolder(string virtualPath)
	{
		var fullPath = MapPath(virtualPath);

		if (!Directory.Exists(fullPath))
		{
			return OpResult.NotFound;
		}

		Directory.Delete(fullPath, true);

		return OpResult.Succeed;
	}

	public IEnumerable<WebFolder>? GetAllFolders()
	{
		var fullPath = MapPath(_permission.VirtualRoot);

		if (!Directory.Exists(fullPath))
		{
			return null;
		}

		var directoryInfo = new DirectoryInfo(fullPath);

		return directoryInfo
			.GetDirectories("*", SearchOption.AllDirectories)
			.Where(folder => IsAllowed(folder.Name))
			.Select(p => new WebFolder()
			{
				Path = ResolvePathResult(p.FullName),
				VirtualPath = _permission.VirtualRoot + p.Name,
				Created = p.CreationTimeUtc,
				Modified = p.LastWriteTimeUtc,
				Name = p.Name
			});
	}

	public IEnumerable<WebFolder>? GetFolderList(string virtualPath)
	{
		var fullPath = MapPath(virtualPath);

		if (!Directory.Exists(fullPath))
		{
			return null;
		}

		var directoryInfo = new DirectoryInfo(fullPath);

		return directoryInfo
			.GetDirectories("*", SearchOption.TopDirectoryOnly)
			.Where(folder => IsAllowed(folder.Name))
			.Select(p => new WebFolder()
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
		if (_folderCount < 0)
		{
			UpdateInfo();
		}

		return _folderCount;
	}

	public string CombinePath(params string[] paths)
	{
		return Path.Combine(paths);
	}

	#endregion
}
