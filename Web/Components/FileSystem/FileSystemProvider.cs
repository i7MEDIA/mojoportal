using System.Configuration.Provider;


namespace mojoPortal.FileSystem
{
	public abstract class FileSystemProvider : ProviderBase
	{
		public abstract IFileSystem GetFileSystem();
		public abstract IFileSystem GetFileSystem(IFileSystemPermission permission);
		public abstract IFileSystem GetFileSystem(int siteId);
	}
}
