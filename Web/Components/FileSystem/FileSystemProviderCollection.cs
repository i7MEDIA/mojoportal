using System;
using System.Configuration.Provider;


namespace mojoPortal.FileSystem
{
	public class FileSystemProviderCollection : ProviderCollection
	{
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("The provider parameter cannot be null.");
			}

			if (!(provider is FileSystemProvider))
			{
				throw new ArgumentException("The provider parameter must be of type FileSystemProvider.");
			}

			base.Add(provider);
		}


		new public FileSystemProvider this[string name] => (FileSystemProvider)base[name];

		public void CopyTo(FileSystemProvider[] array, int index) => base.CopyTo(array, index);
	}
}
