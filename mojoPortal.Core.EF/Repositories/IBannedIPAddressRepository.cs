using mojoPortal.Core.EF.Domain;

namespace mojoPortal.Core.EF.Repositories
{
	public interface IBannedIPAddressRepository : IRepository<BannedIPAddress>
	{
		bool IsBanned(BannedIPAddress bannedIPAddess);
		bool IsBanned(string bannedIPAddess);
	}
}
