using mojoPortal.Business.WebHelpers.PaymentGateway;
using System.Configuration.Provider;

namespace mojoPortal.Web.Commerce
{
	public abstract class PaymentGatewayProvider : ProviderBase
	{
		public abstract IPaymentGateway GetPaymentGateway();
	}
}
