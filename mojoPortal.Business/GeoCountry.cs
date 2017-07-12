// Author:					
// Created:				    2007-02-16
// Last Modified:			2009-02-01
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
    /// Represents a geographic country
    /// </summary>
    public class GeoCountry
    {

        #region Constructors

        public GeoCountry()
        { }


        public GeoCountry(Guid guid)
        {
            GetGeoCountry(guid);
        }

        public GeoCountry(string countryISOCode2)
        {
            GetGeoCountry(countryISOCode2);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private string name;
        private string iSOCode2;
        private string iSOCode3;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string IsoCode2
        {
            get { return iSOCode2; }
            set { iSOCode2 = value; }
        }
        public string IsoCode3
        {
            get { return iSOCode3; }
            set { iSOCode3 = value; }
        }

        #endregion

        #region Private Methods

        private void GetGeoCountry(Guid guid)
        {
            using (IDataReader reader = DBGeoCountry.GetOne(guid))
            {
                GetGeoCountry(reader);
            }
        }

        private void GetGeoCountry(string countryISOCode2)
        {
            using (IDataReader reader = DBGeoCountry.GetByISOCode2(countryISOCode2))
            {
                GetGeoCountry(reader);
            }
        }

        private void GetGeoCountry(IDataReader reader)
        {
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.name = reader["Name"].ToString();
                this.iSOCode2 = reader["ISOCode2"].ToString();
                this.iSOCode3 = reader["ISOCode3"].ToString();

            }
        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBGeoCountry.Create(
                this.guid,
                this.name,
                this.iSOCode2,
                this.iSOCode3);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBGeoCountry.Update(
                this.guid,
                this.name,
                this.iSOCode2,
                this.iSOCode3);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.guid != Guid.Empty)
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

        public static bool Delete(Guid guid)
        {
            return DBGeoCountry.Delete(guid);

        }

        public static IDataReader GetAllGeoCountry()
        {
            return DBGeoCountry.GetAll();
        }

        public static DataTable GetList()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Name", typeof(String));
            dataTable.Columns.Add("ISOCode2", typeof(String));
            dataTable.Columns.Add("ISOCode3", typeof(String));

            using (IDataReader reader = DBGeoCountry.GetAll())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = reader["Guid"];
                    row["Name"] = reader["Name"].ToString();
                    row["ISOCode2"] = reader["ISOCode2"].ToString();
                    row["ISOCode3"] = reader["ISOCode3"].ToString();
                    dataTable.Rows.Add(row);

                }
            }

            return dataTable;
        }

        public static List<GeoCountry> GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            
            List<GeoCountry> geoCountryCollection
                = new List<GeoCountry>();

            using (IDataReader reader = DBGeoCountry.GetPage(pageNumber, pageSize, out totalPages))
            {
                while (reader.Read())
                {
                    GeoCountry geoCountry = new GeoCountry();
                    geoCountry.guid = new Guid(reader["Guid"].ToString());
                    geoCountry.name = reader["Name"].ToString();
                    geoCountry.iSOCode2 = reader["ISOCode2"].ToString();
                    geoCountry.iSOCode3 = reader["ISOCode3"].ToString();
                    geoCountryCollection.Add(geoCountry);
                    //totalPages = Convert.ToInt32(reader["TotalPages"]);
                }
            }

            return geoCountryCollection;

        }

        public static int CompareByName(GeoCountry country1, GeoCountry country2)
        {
            return country1.Name.CompareTo(country2.Name);

        }

        public static int CompareByIsoCode2(GeoCountry country1, GeoCountry country2)
        {
            return country1.IsoCode2.CompareTo(country2.IsoCode2);
        }

        public static int CompareByIsoCode3(GeoCountry country1, GeoCountry country2)
        {
            return country1.IsoCode3.CompareTo(country2.IsoCode3);

        }



        


        #endregion


    }

}