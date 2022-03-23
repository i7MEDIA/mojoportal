using System.Configuration.Provider;
using System.Web.Configuration;

namespace mojoPortal.Web.Commerce
{
	public sealed class PaymentGatewayManager
	{
		#region Properties

		public static PaymentGatewayProvider Provider { get; private set; }
		public static PaymentGatewayProviderCollection Providers { get; private set; }

		#endregion


		#region Constructors

		static PaymentGatewayManager()
		{
			Initialize();
		}

		#endregion


		#region Private Methods

		private static void Initialize()
		{
			PaymentGatewayConfiguration config = PaymentGatewayConfiguration.GetConfig();

			if (
				config.DefaultProvider == null ||
				config.Providers == null ||
				config.Providers.Count < 1
			)
			{
				throw new ProviderException("You must specify a valid default provider.");
			}

			Providers = new PaymentGatewayProviderCollection();

			ProvidersHelper.InstantiateProviders(
				config.Providers,
				Providers,
				typeof(PaymentGatewayProvider)
			);

			Providers.SetReadOnly();
			Provider = Providers[config.DefaultProvider];
		}

		#endregion
	}
}
