using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.Editor;

public class EditorProviderCollection : ProviderCollection
{
	public override void Add(ProviderBase provider)
	{
		if (provider == null)
			throw new ArgumentNullException("The provider parameter cannot be null.");

		if (!(provider is EditorProvider))
			throw new ArgumentException("The provider parameter must be of type EditorProvider.");

		base.Add(provider);
	}

	new public EditorProvider this[string name]
	{
		get { return (EditorProvider)base[name]; }
	}

	public void CopyTo(EditorProvider[] array, int index)
	{
		base.CopyTo(array, index);
	}
}