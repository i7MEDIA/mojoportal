#nullable enable
using mojoPortal.Business.WebHelpers;
using System.Security.Principal;

namespace mojoPortal.Web.Security;

public class mojoPrincipal : IPrincipal
{
	private readonly IPrincipal _innerPrincipal;
	private readonly mojoIdentity identity;

	public mojoPrincipal(IPrincipal innerPrincipal)
	{
		_innerPrincipal = innerPrincipal;
		identity = new(innerPrincipal.Identity);
	}

	public mojoPrincipal(string username)
	{
		identity = new mojoIdentity(username);
	}

	public IIdentity Identity => identity!;


	public bool IsInRole(string role)
	{
		var result = false;

		if (_innerPrincipal != null)
		{
			result = _innerPrincipal.IsInRole(role);
		}

		if (WebConfigSettings.UseFolderBasedMultiTenants)
		{
			var virtualFolder = VirtualFolderEvaluator.VirtualFolderName();

			if (!string.IsNullOrWhiteSpace(virtualFolder))
			{
				result = false;
			}
		}

		return result;
	}
}
