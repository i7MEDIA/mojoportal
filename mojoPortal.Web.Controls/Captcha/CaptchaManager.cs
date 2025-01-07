using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.Web.Controls.Captcha;

public sealed class CaptchaManager
{
	private static CaptchaProvider defaultProvider;
	private static CaptchaProviderCollection providerCollection;


	static CaptchaManager() => Initialize();


	private static void Initialize()
	{
		var config = CaptchaConfiguration.GetConfig();

		if (
			config.DefaultProvider is null ||
			config.Providers is null ||
			config.Providers.Count < 1
		)
		{
			throw new ProviderException("You must specify a valid default provider.");
		}

		providerCollection = [];

		ProvidersHelper.InstantiateProviders(
			config.Providers,
			providerCollection,
			typeof(CaptchaProvider)
		);

		providerCollection.SetReadOnly();
		defaultProvider = providerCollection[config.DefaultProvider];
	}


	public static CaptchaProvider Provider => defaultProvider;

	public static CaptchaProviderCollection Providers => providerCollection;
}
