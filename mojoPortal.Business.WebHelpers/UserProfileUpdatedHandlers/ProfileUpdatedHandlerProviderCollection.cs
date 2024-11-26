//  Author:                     /Huw Reddick

using System;
using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;
public class ProfileUpdatedHandlerProviderCollection : ProviderCollection
{
	public override void Add(ProviderBase provider)
	{
		if (provider == null)
		{
			throw new ArgumentNullException("The provider parameter cannot be null.");
		}

		if (provider is not ProfileUpdatedHandlerProvider)
		{
			throw new ArgumentException("The provider parameter must be of type ProfileUpdatedHandlerProvider.");
		}

		base.Add(provider);
	}

	new public ProfileUpdatedHandlerProvider this[string name] => (ProfileUpdatedHandlerProvider)base[name];

	public void CopyTo(ProfileUpdatedHandlerProvider[] array, int index) => base.CopyTo(array, index);

}
