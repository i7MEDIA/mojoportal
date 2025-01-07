using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.Controls.Captcha;

public class CaptchaProviderCollection : ProviderCollection
{
	public new CaptchaProvider this[string name] => (CaptchaProvider)base[name];


	public override void Add(ProviderBase provider)
	{
		if (provider is null)
		{
			throw new ArgumentNullException("The provider parameter cannot be null.");
		}

		if (provider is not CaptchaProvider)
		{
			throw new ArgumentException("The provider parameter must be of type CaptchaProvider.");
		}

		base.Add(provider);
	}


	public void CopyTo(CaptchaProvider[] array, int index) => base.CopyTo(array, index);
}
