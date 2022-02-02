using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mojoPortal.Data.EF
{
	public class mojoPortalDbConfiguration : DbConfiguration
	{
		public mojoPortalDbConfiguration()
		{
			SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
			SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
		}
	}
}
