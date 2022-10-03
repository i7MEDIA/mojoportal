using mojoPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Business
{
	public class BannedIPAddress
	{

		#region Constructors

		public BannedIPAddress() { }


		public BannedIPAddress(int rowId)
		{
			GetBannedIPAddress(rowId);
		}

		#endregion


		#region Public Properties

		public int RowId { get; set; }
		public string BannedIP { get; set; }
		public DateTime BannedUtc { get; set; }
		public string BannedReason { get; set; }

		#endregion


		#region Private Methods

		/// <summary>
		/// Gets an instance of BannedIPAddress.
		/// </summary>
		/// <param name="rowId"> rowId </param>
		private void GetBannedIPAddress(int rowId)
		{
			using (IDataReader reader = DBBannedIP.GetOne(rowId))
			{
				if (reader.Read())
				{
					RowId = Convert.ToInt32(reader["RowID"]);
					BannedIP = reader["BannedIP"].ToString();
					BannedUtc = Convert.ToDateTime(reader["BannedUTC"]);
					BannedReason = reader["BannedReason"].ToString();
				}
			}
		}


		/// <summary>
		/// Persists a new instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			RowId = DBBannedIP.Add(
				BannedIP,
				BannedUtc,
				BannedReason
			);

			return RowId > 0;
		}


		/// <summary>
		/// Updates this instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{
			return DBBannedIP.Update(
				RowId,
				BannedIP,
				BannedUtc,
				BannedReason
			);
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Saves this instance of BannedIPAddress. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save() => RowId > 0 ? Update() : Create();

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
			if (string.IsNullOrEmpty(ipAddress))
			{
				return false;
			}

			return DBBannedIP.IsBanned(ipAddress);
		}


		/// <summary>
		/// Gets an IList with all instances of BannedIPAddress.
		/// </summary>
		public static List<BannedIPAddress> GetAll()
		{
			var bannedIPAddressList = new List<BannedIPAddress>();

			using (IDataReader reader = DBBannedIP.GetAll())
			{
				while (reader.Read())
				{
					BannedIPAddress bannedIPAddress = new BannedIPAddress
					{
						RowId = Convert.ToInt32(reader["RowID"]),
						BannedIP = reader["BannedIP"].ToString(),
						BannedUtc = Convert.ToDateTime(reader["BannedUTC"]),
						BannedReason = reader["BannedReason"].ToString()
					};

					bannedIPAddressList.Add(bannedIPAddress);
				}
			}

			return bannedIPAddressList;
		}


		public static List<string> GetAllBannedIPs()
		{
			var bannedIPAddressList = new List<string>();

			using (IDataReader reader = DBBannedIP.GetAll())
			{
				while (reader.Read())
				{
					bannedIPAddressList.Add(reader["BannedIP"].ToString());
				}
			}

			return bannedIPAddressList;
		}


		/// <summary>
		/// Gets an IList with page of instances of BannedIPAddresse.
		/// </summary>
		public static List<BannedIPAddress> GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			var bannedIPAddressList = new List<BannedIPAddress>();

			using (IDataReader reader = DBBannedIP.GetPage(pageNumber, pageSize, out totalPages))
			{
				while (reader.Read())
				{
					BannedIPAddress bannedIPAddress = new BannedIPAddress
					{
						RowId = Convert.ToInt32(reader["RowID"]),
						BannedIP = reader["BannedIP"].ToString(),
						BannedUtc = Convert.ToDateTime(reader["BannedUTC"]),
						BannedReason = reader["BannedReason"].ToString()
					};

					bannedIPAddressList.Add(bannedIPAddress);
				}
			}

			return bannedIPAddressList;
		}


		/// <summary>
		/// Gets an IDataReader with rows from the mp_BannedIPAddresses table.
		/// </summary>
		/// <param name="ipAddress"> ipAddress </param>
		public static IDataReader GeByIpAddress(string ipAddress)
		{
			return DBBannedIP.GeByIpAddress(ipAddress);
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
