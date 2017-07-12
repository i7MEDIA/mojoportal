// Author:					
// Created:				2008-01-04
// Last Modified:			2011-01-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a user location aka ip address for tracking user ip addresses
    /// </summary>
    public class UserLocation
    {

        #region Constructors

        public UserLocation()
        { }


        public UserLocation(Guid rowID)
        {
            GetUserLocation(rowID);
        }

        public UserLocation(Guid userGuid, string ipv4Address)
        {
            this.userGuid = userGuid;
            this.iPAddress = ipv4Address;
            this.iPAddressLong = IPAddressHelper.ConvertToLong(ipv4Address);
            GetUserLocation(this.userGuid, this.iPAddressLong);
        }

        #endregion

        #region Private Properties

        private Guid rowID = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string iPAddress = string.Empty;
        private long iPAddressLong = 0;
        private string hostname = string.Empty;
        private double longitude = 0;
        private double latitude = 0;
        private string iSP = string.Empty;
        private string continent = string.Empty;
        private string country = string.Empty;
        private string region = string.Empty;
        private string city = string.Empty;
        private string timeZone = string.Empty;
        private int captureCount = 1;
        private DateTime firstCaptureUTC = DateTime.UtcNow;
        private DateTime lastCaptureUTC = DateTime.UtcNow;

        #endregion

        #region Public Properties

        public Guid RowID
        {
            get { return rowID; }
            set { rowID = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public string IPAddress
        {
            get { return iPAddress; }
            set { iPAddress = value; }
        }
        public long IPAddressLong
        {
            get { return iPAddressLong; }
            set { iPAddressLong = value; }
        }
        public string Hostname
        {
            get { return hostname; }
            set { hostname = value; }
        }
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public string ISP
        {
            get { return iSP; }
            set { iSP = value; }
        }
        public string Continent
        {
            get { return continent; }
            set { continent = value; }
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        public string Region
        {
            get { return region; }
            set { region = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }
        public int CaptureCount
        {
            get { return captureCount; }
            set { captureCount = value; }
        }
        public DateTime FirstCaptureUTC
        {
            get { return firstCaptureUTC; }
            set { firstCaptureUTC = value; }
        }
        public DateTime LastCaptureUTC
        {
            get { return lastCaptureUTC; }
            set { lastCaptureUTC = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of UserLocation.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        private void GetUserLocation(Guid rowID)
        {
            using (IDataReader reader = DBUserLocation.GetOne(rowID))
            {
                PopulateFromReader(reader);
            }
        }


        private void GetUserLocation(Guid userGuid, long iPAddressLong)
        {
            using (IDataReader reader = DBUserLocation.GetOne(userGuid, iPAddressLong))
            {
                PopulateFromReader(reader);
            }
        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.rowID = new Guid(reader["RowID"].ToString());
                this.userGuid = new Guid(reader["UserGuid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.iPAddress = reader["IPAddress"].ToString();
                this.iPAddressLong = Convert.ToInt64(reader["IPAddressLong"]);
                this.hostname = reader["Hostname"].ToString();
                this.longitude = Convert.ToDouble(reader["Longitude"]);
                this.latitude = Convert.ToDouble(reader["Latitude"]);
                this.iSP = reader["ISP"].ToString();
                this.continent = reader["Continent"].ToString();
                this.country = reader["Country"].ToString();
                this.region = reader["Region"].ToString();
                this.city = reader["City"].ToString();
                this.timeZone = reader["TimeZone"].ToString();
                this.captureCount = Convert.ToInt32(reader["CaptureCount"]);
                this.firstCaptureUTC = Convert.ToDateTime(reader["FirstCaptureUTC"]);
                this.lastCaptureUTC = Convert.ToDateTime(reader["LastCaptureUTC"]);

            }

            


        }

        /// <summary>
        /// Persists a new instance of UserLocation. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            
            this.rowID = Guid.NewGuid(); ;

            int rowsAffected = DBUserLocation.Create(
                this.rowID,
                this.userGuid,
                this.siteGuid,
                this.iPAddress,
                this.iPAddressLong,
                this.hostname,
                this.longitude,
                this.latitude,
                this.iSP,
                this.continent,
                this.country,
                this.region,
                this.city,
                this.timeZone,
                this.captureCount,
                DateTime.UtcNow,
                DateTime.UtcNow);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of UserLocation. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBUserLocation.Update(
                this.rowID,
                this.userGuid,
                this.siteGuid,
                this.iPAddress,
                this.iPAddressLong,
                this.hostname,
                this.longitude,
                this.latitude,
                this.iSP,
                this.continent,
                this.country,
                this.region,
                this.city,
                this.timeZone,
                this.captureCount + 1,
                DateTime.UtcNow);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of UserLocation. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            
            if (this.rowID != Guid.Empty)
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
        public static bool Delete(Guid rowID)
        {
            return DBUserLocation.Delete(rowID);
        }

        public static bool DeleteByUser(Guid userGuid)
        {
            return DBUserLocation.DeleteByUser(userGuid);
        }


        /// <summary>
        /// Gets a count of UserLocation. 
        /// </summary>
        public static int GetCountByUser(Guid userGuid)
        {
            return DBUserLocation.GetCountByUser(userGuid);
        }

        /// <summary>
        /// Gets a count of UserLocation. 
        /// </summary>
        public static int GetCountBySite(Guid siteGuid)
        {
            return DBUserLocation.GetCountBySite(siteGuid);
        }

        private static List<UserLocation> LoadListFromReader(IDataReader reader)
        {
            List<UserLocation> userLocationList = new List<UserLocation>();

            try
            {
                while (reader.Read())
                {
                    UserLocation userLocation = new UserLocation();
                    userLocation.rowID = new Guid(reader["RowID"].ToString());
                    userLocation.userGuid = new Guid(reader["UserGuid"].ToString());
                    userLocation.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    userLocation.iPAddress = reader["IPAddress"].ToString();
                    userLocation.iPAddressLong = Convert.ToInt64(reader["IPAddressLong"]);
                    userLocation.hostname = reader["Hostname"].ToString();
                    userLocation.longitude = Convert.ToDouble(reader["Longitude"]);
                    userLocation.latitude = Convert.ToDouble(reader["Latitude"]);
                    userLocation.iSP = reader["ISP"].ToString();
                    userLocation.continent = reader["Continent"].ToString();
                    userLocation.country = reader["Country"].ToString();
                    userLocation.region = reader["Region"].ToString();
                    userLocation.city = reader["City"].ToString();
                    userLocation.timeZone = reader["TimeZone"].ToString();
                    userLocation.captureCount = Convert.ToInt32(reader["CaptureCount"]);
                    userLocation.firstCaptureUTC = Convert.ToDateTime(reader["FirstCaptureUTC"]);
                    userLocation.lastCaptureUTC = Convert.ToDateTime(reader["LastCaptureUTC"]);
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
        public static List<UserLocation> GetByUser(Guid userGuid)
        {
            IDataReader reader = DBUserLocation.GetByUser(userGuid);
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with all instances of UserLocation for the site.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        public static List<UserLocation> GetBySite(Guid siteGuid)
        {
            IDataReader reader = DBUserLocation.GetBySite(siteGuid);
            return LoadListFromReader(reader);

        }

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
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBUserLocation.GetPageByUser(userGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

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
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBUserLocation.GetPageBySite(siteGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Users table which have the passed in IP Address
        /// </summary>
        public static IDataReader GetUsersByIPAddress(Guid siteGuid, string ipv4Address)
        {
            return DBUserLocation.GetUsersByIPAddress(siteGuid, ipv4Address);
        }


        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByIPAddress(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.IPAddress.CompareTo(userLocation2.IPAddress);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByHostname(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.Hostname.CompareTo(userLocation2.Hostname);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByISP(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.ISP.CompareTo(userLocation2.ISP);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByContinent(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.Continent.CompareTo(userLocation2.Continent);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByCountry(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.Country.CompareTo(userLocation2.Country);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByRegion(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.Region.CompareTo(userLocation2.Region);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByCity(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.City.CompareTo(userLocation2.City);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByTimeZone(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.TimeZone.CompareTo(userLocation2.TimeZone);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByCaptureCount(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.CaptureCount.CompareTo(userLocation2.CaptureCount);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByFirstCaptureUTC(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.FirstCaptureUTC.CompareTo(userLocation2.FirstCaptureUTC);
        }
        /// <summary>
        /// Compares 2 instances of UserLocation.
        /// </summary>
        public static int CompareByLastCaptureUTC(UserLocation userLocation1, UserLocation userLocation2)
        {
            return userLocation1.LastCaptureUTC.CompareTo(userLocation2.LastCaptureUTC);
        }

        #endregion


    }

}
