using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.Commerce
{
	public class PaymentGatewayProviderCollection : ProviderCollection
	{
		#region Public Methods

		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("The provider parameter cannot be null.");
			}

			if (!(provider is PaymentGatewayProvider))
			{
				throw new ArgumentException("The provider parameter must be of type PaymentGatewayProvider.");
			}

			base.Add(provider);
		}


		public void CopyTo(PaymentGatewayProvider[] array, int index) => base.CopyTo(array, index);

		#endregion


		#region Indexer

		public new PaymentGatewayProvider this[string name] => (PaymentGatewayProvider)base[name];

		#endregion
	}
}