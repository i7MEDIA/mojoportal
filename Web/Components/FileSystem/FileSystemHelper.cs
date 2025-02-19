using log4net;
using mojoPortal.Web;

namespace mojoPortal.FileSystem;

public static class FileSystemHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(FileSystemHelper));

	public static IFileSystem LoadFileSystem(int siteId, string providerName = null)
	{
		providerName ??= WebConfigSettings.FileSystemProvider;
		
		FileSystemProvider p = FileSystemManager.Providers[providerName];
		IFileSystem fileSystem;
		if (p == null)
		{
			log.Fatal(string.Format($"FileSystemProvider Could Not Be Loaded: {providerName}"));
		}

		if (siteId > 0)
		{
			fileSystem = p.GetFileSystem(siteId);
		}
		else
		{
			fileSystem = p.GetFileSystem();
		}

		if (fileSystem == null)
		{
			log.Fatal(string.Format($"FileSystemProvider Could Not Be Loaded: {providerName}"));
		}
		return fileSystem;
	}

	public static IFileSystem LoadFileSystem(string providerName = null)
	{
		return LoadFileSystem(-1, providerName);
	}
}