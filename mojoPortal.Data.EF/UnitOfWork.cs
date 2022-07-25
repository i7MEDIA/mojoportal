using mojoPortal.Core.EF;
using mojoPortal.Core.EF.Repositories;
using mojoPortal.Data.EF.Repositories;

namespace mojoPortal.Data.EF
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly mojoPortalDbContext context;
		private readonly bool readConnection = false;


		#region Public Repositories

		public IBannedIPAddressRepository BannedIPAddresses { get; private set; }

		#endregion


		public UnitOfWork(mojoPortalDbContext context)
		{
			this.context = context;


			#region Set Repositories

			BannedIPAddresses = new BannedIPAddressRepository(context);

			#endregion

		}


		public UnitOfWork(mojoPortalDbContext context, bool readConnection) : this(context) => this.readConnection = true;


		public IUnitOfWork ReadConnection() => new UnitOfWork(context.ReadConnection(), true);


		public int Complete() => readConnection ? 0 : context.SaveChanges();


		public void Dispose() => context.Dispose();
	}
}
