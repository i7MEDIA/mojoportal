using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.FileSystem
{
	public sealed class FileSystemManager
	{
		public static FileSystemProviderCollection Providers { get; private set; }
		public static FileSystemProvider Provider { get; private set; }


		static FileSystemManager()
		{
			Initialize();
		}


		private static void Initialize()
		{
			var config = FileSystemConfiguration.GetConfig();

			if (
				config.DefaultProvider == null ||
				config.Providers == null ||
				config.Providers.Count < 1
			)
			{
				throw new ProviderException("You must specify a valid default provider.");
			}

			Providers = new FileSystemProviderCollection();

			ProvidersHelper.InstantiateProviders(
				config.Providers,
				Providers,
				typeof(FileSystemProvider)
			);

			Providers.SetReadOnly();
			Provider = Providers[config.DefaultProvider];
		}
	}
}
