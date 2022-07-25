using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace mojoPortal.Data.EF
{
	public class mojoPortalDbConfiguration : DbConfiguration
	{
		public mojoPortalDbConfiguration()
		{
			SetDefaultConnectionFactory(connectionFactory: new SqlConnectionFactory());
			SetProviderServices(providerInvariantName: "System.Data.SqlClient", provider: SqlProviderServices.Instance);
		}
	}
}
