using log4net;
using mojoPortal.Web;

namespace mojoPortal.FileSystem;

public static class FileSystemHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(FileSystemHelper));

	public static IFileSystem LoadFileSystem(string providerName = null)
	{
		providerName ??= WebConfigSettings.FileSystemProvider;

		FileSystemProvider p = FileSystemManager.Providers[providerName];

		if (p == null)
		{
			log.Fatal(string.Format($"FileSystemProvider Could Not Be Loaded: {providerName}"));
		}

		IFileSystem fileSystem = p.GetFileSystem();
		if (fileSystem == null)
		{
			log.Fatal(string.Format($"FileSystemProvider Could Not Be Loaded: {providerName}"));
		}
		return fileSystem;
	}
}