using mojoPortal.Core.EF.Domain;
using mojoPortal.Core.EF.Repositories;
using System;
using System.Linq;

namespace mojoPortal.Data.EF.Repositories
{
	public class BannedIPAddressRepository : Repository<BannedIPAddress>, IBannedIPAddressRepository
	{
		public mojoPortalDbContext context => Context as mojoPortalDbContext;


		public BannedIPAddressRepository(mojoPortalDbContext context) : base(context)
		{ }


		public bool IsBanned(BannedIPAddress bannedIPAddress) => IsBanned(bannedIPAddress.IPAddress);


		public bool IsBanned(string bannedIPAddress)
		{
			return context.BannedIPAddresses.Any(b => b.IPAddress.Equals(bannedIPAddress, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
