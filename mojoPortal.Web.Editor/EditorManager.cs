using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.Web.Editor;

public sealed class EditorManager
{
	public static EditorProvider Provider { get; private set; }

	public static EditorProviderCollection Providers { get; private set; }

	static EditorManager()
	{
		Initialize();
	}

	private static void Initialize()
	{
		EditorConfiguration editorConfig = EditorConfiguration.GetConfig();

		if (editorConfig.DefaultProvider == null
			|| editorConfig.Providers == null
			|| editorConfig.Providers.Count < 1
			)
		{
			throw new ProviderException("You must specify a valid default provider.");
		}

		Providers = [];

		ProvidersHelper.InstantiateProviders(editorConfig.Providers, Providers, typeof(EditorProvider));

		Providers.SetReadOnly();
		Provider = Providers[editorConfig.DefaultProvider];
	}
}