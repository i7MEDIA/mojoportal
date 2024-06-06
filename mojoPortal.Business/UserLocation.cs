using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// Represents a user location aka ip address for tracking user ip addresses
/// </summary>
public class UserLocation
{
	#region Constructors

	public UserLocation() { }


	public UserLocation(Guid rowID)
	{
		GetUserLocation(rowID);
	}

	public UserLocation(Guid userGuid, string ipv4Address)
	{
		UserGuid = userGuid;
		IPAddress = ipv4Address;
		IPAddressLong = IPAddressHelper.ConvertToLong(ipv4Address);
		GetUserLocation(UserGuid, IPAddressLong);
	}

	#endregion

	#region Properties

	public Guid RowID { get; set; } = Guid.Empty;
	public Guid UserGuid { get; set; } = Guid.Empty;
	public Guid SiteGuid { get; set; } = Guid.Empty;
	public string IPAddress { get; set; } = string.Empty;
	public long IPAddressLong { get; set; } = 0;
	public string Hostname { get; set; } = string.Empty;
	public double Longitude { get; set; } = 0;
	public double Latitude { get; set; } = 0;
	public string ISP { get; set; } = string.Empty;
	public string Continent { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string Region { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string TimeZone { get; set; } = string.Empty;
	public int CaptureCount { get; set; } = 1;
	public DateTime FirstCaptureUTC { get; set; } = DateTime.UtcNow;
	public DateTime LastCaptureUTC { get; set; } = DateTime.UtcNow;

	#endregion

	#region Private Methods

	/// <summary>
	/// Gets an instance of UserLocation.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	private void GetUserLocation(Guid rowID) => PopulateFromReader(DBUserLocation.GetOne(rowID));


	private void GetUserLocation(Guid userGuid, long iPAddressLong) => PopulateFromReader(DBUserLocation.GetOne(userGuid, iPAddressLong));


	private void PopulateFromReader(IDataReader reader)
	{
		if (reader.Read())
		{
			RowID = new Guid(reader["RowID"].ToString());
			UserGuid = new Guid(reader["UserGuid"].ToString());
			SiteGuid = new Guid(reader["SiteGuid"].ToString());
			IPAddress = reader["IPAddress"].ToString();
			IPAddressLong = Convert.ToInt64(reader["IPAddressLong"]);
			Hostname = reader["Hostname"].ToString();
			Longitude = Convert.ToDouble(reader["Longitude"]);
			Latitude = Convert.ToDouble(reader["Latitude"]);
			ISP = reader["ISP"].ToString();
			Continent = reader["Continent"].ToString();
			Country = reader["Country"].ToString();
			Region = reader["Region"].ToString();
			City = reader["City"].ToString();
			TimeZone = reader["TimeZone"].ToString();
			CaptureCount = Convert.ToInt32(reader["CaptureCount"]);
			FirstCaptureUTC = Convert.ToDateTime(reader["FirstCaptureUTC"]);
			LastCaptureUTC = Convert.ToDateTime(reader["LastCaptureUTC"]);
		}
	}

	/// <summary>
	/// Persists a new instance of UserLocation. Returns true on success.
	/// </summary>
	/// <returns></returns>
	private bool Create() => DBUserLocation.Create(
			Guid.NewGuid(),
			UserGuid,
			SiteGuid,
			IPAddress,
			IPAddressLong,
			Hostname,
			Longitude,
			Latitude,
			ISP,
			Continent,
			Country,
			Region,
			City,
			TimeZone,
			CaptureCount,
			DateTime.UtcNow,
			DateTime.UtcNow) > 0;


	/// <summary>
	/// Updates this instance of UserLocation. Returns true on success.
	/// </summary>
	/// <returns>bool</returns>
	private bool Update() => DBUserLocation.Update(
			RowID,
			UserGuid,
			SiteGuid,
			IPAddress,
			IPAddressLong,
			Hostname,
			Longitude,
			Latitude,
			ISP,
			Continent,
			Country,
			Region,
			City,
			TimeZone,
			CaptureCount + 1,
			DateTime.UtcNow);

	#endregion

	#region Public Methods

	/// <summary>
	/// Saves this instance of UserLocation. Returns true on success.
	/// </summary>
	/// <returns>bool</returns>
	public bool Save()
	{

		if (RowID != Guid.Empty)
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
	/// Deletes an instance of UserLocation. Returns true on success.
	/// </summary>
	/// <param name="rowID"> rowID </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowID) => DBUserLocation.Delete(rowID);

	public static bool DeleteByUser(Guid userGuid) => DBUserLocation.DeleteByUser(userGuid);


	/// <summary>
	/// Gets a count of UserLocation. 
	/// </summary>
	public static int GetCountByUser(Guid userGuid) => DBUserLocation.GetCountByUser(userGuid);

	/// <summary>
	/// Gets a count of UserLocation. 
	/// </summary>
	public static int GetCountBySite(Guid siteGuid) => DBUserLocation.GetCountBySite(siteGuid);

	private static List<UserLocation> LoadListFromReader(IDataReader reader)
	{
		List<UserLocation> userLocationList = new List<UserLocation>();

		try
		{
			while (reader.Read())
			{
				var userLocation = new UserLocation
				{
					RowID = new Guid(reader["RowID"].ToString()),
					UserGuid = new Guid(reader["UserGuid"].ToString()),
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					IPAddress = reader["IPAddress"].ToString(),
					IPAddressLong = Convert.ToInt64(reader["IPAddressLong"]),
					Hostname = reader["Hostname"].ToString(),
					Longitude = Convert.ToDouble(reader["Longitude"]),
					Latitude = Convert.ToDouble(reader["Latitude"]),
					ISP = reader["ISP"].ToString(),
					Continent = reader["Continent"].ToString(),
					Country = reader["Country"].ToString(),
					Region = reader["Region"].ToString(),
					City = reader["City"].ToString(),
					TimeZone = reader["TimeZone"].ToString(),
					CaptureCount = Convert.ToInt32(reader["CaptureCount"]),
					FirstCaptureUTC = Convert.ToDateTime(reader["FirstCaptureUTC"]),
					LastCaptureUTC = Convert.ToDateTime(reader["LastCaptureUTC"])
				};
				userLocationList.Add(userLocation);
			}
		}
		finally
		{
			reader.Close();
		}

		return userLocationList;
	}

	/// <summary>
	/// Gets an IList with all instances of UserLocation for the user.
	/// </summary>
	/// <param name="userGuid"> userGuid </param>
	public static List<UserLocation> GetByUser(Guid userGuid) => LoadListFromReader(DBUserLocation.GetByUser(userGuid));

	/// <summary>
	/// Gets an IList with all instances of UserLocation for the site.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	public static List<UserLocation> GetBySite(Guid siteGuid) => LoadListFromReader(DBUserLocation.GetBySite(siteGuid));

	/// <summary>
	/// Gets an IList with page of instances of UserLocation.
	/// </summary>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static List<UserLocation> GetPageByUser(
		Guid userGuid,
		int pageNumber,
		int pageSize,
		out int totalPages) => LoadListFromReader(DBUserLocation.GetPageByUser(userGuid, pageNumber, pageSize, out totalPages));

	/// <summary>
	/// Gets an IList with page of instances of UserLocation.
	/// </summary>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static List<UserLocation> GetPageBySite(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages) => LoadListFromReader(DBUserLocation.GetPageBySite(siteGuid, pageNumber, pageSize, out totalPages));

	/// <summary>
	/// Gets an IDataReader with rows from the mp_Users table which have the passed in IP Address
	/// </summary>
	public static IDataReader GetUsersByIPAddress(Guid siteGuid, string ipv4Address) => DBUserLocation.GetUsersByIPAddress(siteGuid, ipv4Address);


	#endregion

	#region Comparison Methods

	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByIPAddress(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.IPAddress.CompareTo(userLocation2.IPAddress);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByHostname(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.Hostname.CompareTo(userLocation2.Hostname);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByISP(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.ISP.CompareTo(userLocation2.ISP);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByContinent(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.Continent.CompareTo(userLocation2.Continent);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByCountry(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.Country.CompareTo(userLocation2.Country);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByRegion(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.Region.CompareTo(userLocation2.Region);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByCity(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.City.CompareTo(userLocation2.City);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByTimeZone(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.TimeZone.CompareTo(userLocation2.TimeZone);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByCaptureCount(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.CaptureCount.CompareTo(userLocation2.CaptureCount);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByFirstCaptureUTC(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.FirstCaptureUTC.CompareTo(userLocation2.FirstCaptureUTC);
	/// <summary>
	/// Compares 2 instances of UserLocation.
	/// </summary>
	public static int CompareByLastCaptureUTC(UserLocation userLocation1, UserLocation userLocation2) => userLocation1.LastCaptureUTC.CompareTo(userLocation2.LastCaptureUTC);

	#endregion


}
