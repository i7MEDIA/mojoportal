using mojoPortal.Data;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Business;

public class RedirectInfo
{
	#region Public Properties

	public Guid RowGuid { get; set; } = Guid.Empty;
	public Guid SiteGuid { get; set; } = Guid.Empty;
	public int SiteId { get; set; } = -1;
	public string OldUrl { get; set; } = string.Empty;
	public string NewUrl { get; set; } = string.Empty;
	public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
	/// <summary>
	/// The date that the old url expired or expires
	/// </summary>
	public DateTime ExpireUtc { get; set; } = DateTime.UtcNow;

	#endregion


	#region Constructors

	public RedirectInfo()
	{ }


	public RedirectInfo(Guid rowGuid)
	{
		GetRedirectList(rowGuid);
	}

	#endregion


	#region Private Methods

	/// <summary>
	/// Gets an instance of RedirectList.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	private void GetRedirectList(Guid rowGuid)
	{
		using IDataReader reader = DBRedirectList.GetOne(rowGuid);
		PopulateFromReader(reader);
	}


	private void PopulateFromReader(IDataReader reader)
	{
		if (reader.Read())
		{
			RowGuid = new Guid(reader["RowGuid"].ToString());
			SiteGuid = new Guid(reader["SiteGuid"].ToString());
			SiteId = Convert.ToInt32(reader["SiteID"]);
			OldUrl = reader["OldUrl"].ToString();
			NewUrl = reader["NewUrl"].ToString();
			CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
			ExpireUtc = Convert.ToDateTime(reader["ExpireUtc"]);
		}
	}


	/// <summary>
	/// Persists a new instance of RedirectList. Returns true on success.
	/// </summary>
	/// <returns></returns>
	private bool Create()
	{
		RowGuid = Guid.NewGuid();

		//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11174~1#post46572
		if (string.IsNullOrWhiteSpace(OldUrl))
		{
			return false;
		}

		int rowsAffected = DBRedirectList.Create(
			RowGuid,
			SiteGuid,
			SiteId,
			OldUrl,
			NewUrl,
			CreatedUtc,
			ExpireUtc
		);

		return rowsAffected > 0;
	}


	/// <summary>
	/// Updates this instance of RedirectList. Returns true on success.
	/// </summary>
	/// <returns>bool</returns>
	private bool Update()
	{
		//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11174~1#post46572
		if (string.IsNullOrWhiteSpace(OldUrl))
		{
			return false;
		}

		return DBRedirectList.Update(
			RowGuid,
			OldUrl,
			NewUrl,
			ExpireUtc
		);
	}

	#endregion


	#region Public Methods

	/// <summary>
	/// Saves this instance of RedirectList. Returns true on success.
	/// </summary>
	/// <returns>bool</returns>
	public bool Save() => RowGuid != Guid.Empty ? Update() : Create();

	#endregion


	#region Static Methods

	/// <summary>
	/// Deletes an instance of RedirectList. Returns true on success.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		return DBRedirectList.Delete(rowGuid);
	}


	/// <summary>
	/// Gets a count of RedirectList. 
	/// </summary>
	public static int GetCount(int siteId)
	{
		return DBRedirectList.GetCount(siteId);
	}


	private static List<RedirectInfo> LoadListFromReader(IDataReader reader)
	{
		List<RedirectInfo> redirectListList = [];

		try
		{
			while (reader.Read())
			{
				RedirectInfo redirectList = new RedirectInfo
				{
					RowGuid = new Guid(reader["RowGuid"].ToString()),
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					SiteId = Convert.ToInt32(reader["SiteID"]),
					OldUrl = reader["OldUrl"].ToString(),
					NewUrl = reader["NewUrl"].ToString(),
					CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]),
					ExpireUtc = Convert.ToDateTime(reader["ExpireUtc"])
				};

				redirectListList.Add(redirectList);
			}
		}
		finally
		{
			reader.Close();
		}

		return redirectListList;
	}


	/// <summary>
	/// Gets an IList with page of instances of RedirectList.
	/// </summary>
	//public static List<RedirectInfo> GetPage(int siteId, int pageNumber, int pageSize, out int totalPages)
	//{
	//	totalPages = 1;
	//	IDataReader reader = DBRedirectList.GetPage(siteId, pageNumber, pageSize, out totalPages);
	//	return LoadListFromReader(reader);
	//}

	/// <summary>
	/// Gets an IList with page of instances of RedirectList with search term.
	/// </summary>
	public static List<RedirectInfo> GetPage(
		int siteId,
		int pageNumber,
		int pageSize,
		out int totalPages,
		string searchTerm = ""
	)
	{
		var reader = DBRedirectList.GetPage(siteId, pageNumber, pageSize, out totalPages, searchTerm);

		return LoadListFromReader(reader);
	}


	/// <summary>
	/// Gets an IDataReader with one row from the mp_RedirectList table.
	/// </summary>
	public static IDataReader GetBySiteAndUrl(int siteId, string oldUrl)
	{
		return DBRedirectList.GetBySiteAndUrl(siteId, oldUrl);
	}


	/// <summary>
	/// returns true if the record exists
	/// </summary>
	public static bool Exists(int siteId, string oldUrl)
	{
		return DBRedirectList.Exists(siteId, oldUrl);
	}

	#endregion


	#region Comparison Methods

	/// <summary>
	/// Compares 2 instances of RedirectList.
	/// </summary>
	public static int CompareBySiteID(RedirectInfo redirectList1, RedirectInfo redirectList2)
	{
		return redirectList1.SiteId.CompareTo(redirectList2.SiteId);
	}


	/// <summary>
	/// Compares 2 instances of RedirectList.
	/// </summary>
	public static int CompareByOldUrl(RedirectInfo redirectList1, RedirectInfo redirectList2)
	{
		return redirectList1.OldUrl.CompareTo(redirectList2.OldUrl);
	}


	/// <summary>
	/// Compares 2 instances of RedirectList.
	/// </summary>
	public static int CompareByNewUrl(RedirectInfo redirectList1, RedirectInfo redirectList2)
	{
		return redirectList1.NewUrl.CompareTo(redirectList2.NewUrl);
	}


	/// <summary>
	/// Compares 2 instances of RedirectList.
	/// </summary>
	public static int CompareByCreatedUtc(RedirectInfo redirectList1, RedirectInfo redirectList2)
	{
		return redirectList1.CreatedUtc.CompareTo(redirectList2.CreatedUtc);
	}


	/// <summary>
	/// Compares 2 instances of RedirectList.
	/// </summary>
	public static int CompareByExpireUtc(RedirectInfo redirectList1, RedirectInfo redirectList2)
	{
		return redirectList1.ExpireUtc.CompareTo(redirectList2.ExpireUtc);
	}

	#endregion
}
