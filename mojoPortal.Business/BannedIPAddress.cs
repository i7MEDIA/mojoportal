using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using mojoPortal.Data;
using mojoPortal.Data.EF;

namespace mojoPortal.Business
{
	public class BannedIPAddress
	{
		private mojoPortalDbContext context = null;

		#region Constructors

		public BannedIPAddress()
		{
			context = new mojoPortalDbContext();
		}


		public BannedIPAddress(int rowId)
		{
			context = new mojoPortalDbContext();

			var item = context.BannedIPAddresses.Find(rowId);

			RowId = item.RowID;
			BannedIP = item.BannedIP;
			BannedUtc = item.BannedUTC;
			BannedReason = item.BannedReason;
			// GetBannedIPAddress(rowId);
		}

		#endregion


		public int RowId { get; set; }
		public string BannedIP { get; set; }
		public DateTime BannedUtc { get; set; }
		public string BannedReason { get; set; }

		#region Private Methods

		/// <summary>
		/// Gets an instance of BannedIPAddress.
		/// </summary>
		/// <param name="rowId"> rowId </param>
		//private void GetBannedIPAddress(int rowId)
		//      {
		//          using (IDataReader reader = DBBannedIP.GetOne(rowId))
		//          {
		//              if (reader.Read())
		//              {
		//                  this.RowId = Convert.ToInt32(reader["RowID"]);
		//                  this.BannedIP = reader["BannedIP"].ToString();
		//                  this.BannedUtc = Convert.ToDateTime(reader["BannedUTC"]);
		//                  this.BannedReason = reader["BannedReason"].ToString();

		//              }

		//          }
		//      }

		/// <summary>
		/// Persists a new instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			int newID = 0;

			newID = DBBannedIP.Add(
				this.BannedIP,
				this.BannedUtc,
				this.BannedReason);

			this.RowId = newID;

			return (newID > 0);

		}


		/// <summary>
		/// Updates this instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBBannedIP.Update(
				this.RowId,
				this.BannedIP,
				this.BannedUtc,
				this.BannedReason);

		}


		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.RowId > 0)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}




		#endregion

		#region Static Methods

		/// <summary>
		/// Deletes an instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <param name="rowId"> rowId </param>
		/// <returns>bool</returns>
		public static bool Delete(int rowId)
		{
			return DBBannedIP.Delete(rowId);
		}

		/// <summary>
		/// Returns true if the passed in address is banned
		/// </summary>
		/// <param name="ipAddress"> ipAddress </param>
		/// <returns>bool</returns>
		public static bool IsBanned(string ipAddress)
		{
			if (string.IsNullOrEmpty(ipAddress)) { return false; }
			return DBBannedIP.IsBanned(ipAddress);
		}

		/// <summary>
		/// Gets an IList with all instances of BannedIPAddress.
		/// </summary>
		public static List<BannedIPAddress> GetAll()
		{
			var context = new mojoPortalDbContext();

			return context.BannedIPAddresses.Select(x => new BannedIPAddress
			{
				RowId = x.RowID,
				BannedIP = x.BannedIP,
				BannedReason = x.BannedReason,
				BannedUtc = x.BannedUTC
			}).ToList();

			//List<BannedIPAddress> bannedIPAddressList
			//    = new List<BannedIPAddress>();

			//using (IDataReader reader = DBBannedIP.GetAll())
			//{
			//    while (reader.Read())
			//    {
			//        BannedIPAddress bannedIPAddress = new BannedIPAddress();
			//        bannedIPAddress.RowId = Convert.ToInt32(reader["RowID"]);
			//        bannedIPAddress.BannedIP = reader["BannedIP"].ToString();
			//        bannedIPAddress.BannedUtc = Convert.ToDateTime(reader["BannedUTC"]);
			//        bannedIPAddress.BannedReason = reader["BannedReason"].ToString();
			//        bannedIPAddressList.Add(bannedIPAddress);
			//    }
			//}

			//return bannedIPAddressList;
		}

		//public static List<String> GetAllBannedIPs()
		//{
		//    List<String> bannedIPAddressList
		//        = new List<String>();

		//    using (IDataReader reader = DBBannedIP.GetAll())
		//    {
		//        while (reader.Read())
		//        {

		//            bannedIPAddressList.Add(reader["BannedIP"].ToString());

		//        }
		//    }

		//    return bannedIPAddressList;

		//}


		/// <summary>
		/// Gets an IList with page of instances of BannedIPAddresse.
		/// </summary>
		public static List<BannedIPAddress> GetPage(
			int pageNumber,
			int pageSize,
			out int totalPages
		)
		{
			totalPages = 1;

			var context = new mojoPortalDbContext();
			var query = context.BannedIPAddresses;

			var results = query.OrderBy(b => b.ro)
				.Select(x => new
				{
					BannedIPAddress = new BannedIPAddress
					{
						RowId = x.RowID,
						BannedIP = x.BannedIP,
						BannedReason = x.BannedReason,
						BannedUtc = x.BannedUTC
					},
					TotalCount = query.Count()
				})
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList(); // query is executed once, here

			var totalCount = results.FirstOrDefault()?.TotalCount ?? query.Count();
			var bannedIPAddresses = results.Select(x => x.BannedIPAddress).ToList();

			totalPages = totalCount / pageSize;

			Math.DivRem(totalCount, pageSize, out int remainder);

			if (remainder > 0)
			{
				totalPages++;
			}

			return bannedIPAddresses;

			//        var items = context.BannedIPAddresses
			//            .Take(pageSize)
			//            .Skip((pageNumber - 1) * pageSize)
			//            .Select(x => new BannedIPAddress
			//{
			//                RowId = x.RowID,
			//                BannedIP = x.BannedIP,
			//                BannedReason = x.BannedReason,
			//                BannedUtc = x.BannedUTC
			//            })
			//            .ToList();

			//        totalPages = items.Count;




			//List<BannedIPAddress> bannedIPAddressList = new List<BannedIPAddress>();

			//using (IDataReader reader = DBBannedIP.GetPage(
			//    pageNumber,
			//    pageSize,
			//    out totalPages)
			//)
			//{
			//    while (reader.Read())
			//    {
			//        BannedIPAddress bannedIPAddress = new BannedIPAddress();
			//        bannedIPAddress.RowId = Convert.ToInt32(reader["RowID"]);
			//        bannedIPAddress.BannedIP = reader["BannedIP"].ToString();
			//        bannedIPAddress.BannedUtc = Convert.ToDateTime(reader["BannedUTC"]);
			//        bannedIPAddress.BannedReason = reader["BannedReason"].ToString();
			//        bannedIPAddressList.Add(bannedIPAddress);

			//    }
			//}

			//return bannedIPAddressList;
		}


		/// <summary>
		/// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
		/// </summary>
		/// <param name="ipAddress"> ipAddress </param>
		public static BannedIPAddress GeByIpAddress(string ipAddress)
		{
			var context = new mojoPortalDbContext();

			var item = context.BannedIPAddresses.Where(x => x.BannedIP == ipAddress).FirstOrDefault();

			return new BannedIPAddress
			{
				RowId = item.RowID,
				BannedIP = item.BannedIP,
				BannedUtc = item.BannedUTC,
				BannedReason = item.BannedReason
			};
			//return DBBannedIP.GeByIpAddress(ipAddress);
		}

		#endregion


		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of BannedIPAddresse.
		/// </summary>
		public static int CompareByRowId(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
		{
			return bannedIPAddress1.RowId.CompareTo(bannedIPAddress2.RowId);
		}
		/// <summary>
		/// Compares 2 instances of BannedIPAddresse.
		/// </summary>
		public static int CompareByBannedIP(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
		{
			return bannedIPAddress1.BannedIP.CompareTo(bannedIPAddress2.BannedIP);
		}
		/// <summary>
		/// Compares 2 instances of BannedIPAddresse.
		/// </summary>
		public static int CompareByBannedUtc(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
		{
			return bannedIPAddress1.BannedUtc.CompareTo(bannedIPAddress2.BannedUtc);
		}
		/// <summary>
		/// Compares 2 instances of BannedIPAddresse.
		/// </summary>
		public static int CompareByBannedReason(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
		{
			return bannedIPAddress1.BannedReason.CompareTo(bannedIPAddress2.BannedReason);
		}

		#endregion

	}
}
