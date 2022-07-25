using mojoPortal.Core.EF.Repositories;
using System;

namespace mojoPortal.Core.EF
{
	public interface IUnitOfWork : IDisposable
	{
		IBannedIPAddressRepository BannedIPAddresses { get; }

		int Complete();
	}
}
