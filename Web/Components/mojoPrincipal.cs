using mojoPortal.Business.WebHelpers;
using System.Security.Principal;

namespace mojoPortal.Web.Security
{
	public class mojoPrincipal : IPrincipal
	{
		private readonly IPrincipal innerPrincipal;
		private readonly mojoIdentity identity = new mojoIdentity();


		public mojoPrincipal(IPrincipal innerPricipal)
		{
			innerPrincipal = innerPricipal;
			identity = new mojoIdentity(innerPrincipal.Identity);
		}


		public bool IsInRole(string role)
		{
			var result = false;

			if (innerPrincipal != null)
			{
				result = innerPrincipal.IsInRole(role);
			}

			if (WebConfigSettings.UseFolderBasedMultiTenants)
			{
				string virtualFolder = VirtualFolderEvaluator.VirtualFolderName();

				if (virtualFolder.Length > 0)
				{
					result = false;
				}
			}

			return result;
		}

		public IIdentity Identity
		{
			get { return identity; }
		}
	}
}
