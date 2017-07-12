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
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a State within a Country
    /// </summary>
    public class GeoZone
    {

        #region Constructors

        public GeoZone()
        { }

        public GeoZone(Guid guid)
        {
            GetGeoZone(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid countryGuid  = Guid.Empty;
        private string name;
        private string code;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid CountryGuid
        {
            get { return countryGuid; }
            set { countryGuid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        #endregion

        #region Private Methods

        private void GetGeoZone(Guid guid)
        {
            using (IDataReader reader = DBGeoZone.GetOne(guid))
            {
                LoadFromReader(this, reader);
            }

        }

        private static void LoadFromReader(GeoZone geoZone, IDataReader reader)
        {
            if (reader.Read())
            {
                geoZone.guid = new Guid(reader["Guid"].ToString());
                geoZone.countryGuid = new Guid(reader["CountryGuid"].ToString());
                geoZone.name = reader["Name"].ToString();
                geoZone.code = reader["Code"].ToString();

            }

        }

        private bool Create()
        {
            
            this.guid = Guid.NewGuid();

            int rowsAffected = DBGeoZone.Create(
                this.guid,
                this.countryGuid,
                this.name,
                this.code);

            return (rowsAffected > 0);

        }

        private bool Update()
        {

            return DBGeoZone.Update(
                this.guid,
                this.countryGuid,
                this.name,
                this.code);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if ((this.guid != null) && (this.guid != Guid.Empty))
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
            return DBGeoZone.Delete(guid);
        }


        public static IDataReader GetByCountry(Guid countryGuid)
        {
            return DBGeoZone.GetByCountry(countryGuid);
        }

        public static GeoZone GetByCode(Guid countryGuid, string code)
        {
            GeoZone geoZone = new GeoZone();
            using (IDataReader reader = DBGeoZone.GetByCode(countryGuid, code))
            {
                LoadFromReader(geoZone, reader);
            }

            if (geoZone.Guid == Guid.Empty)
            {
                return null;
            }
            return geoZone;


        }

        //public static DataTable GetPage(int pageNumber, int pageSize)
        //{
        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("Guid",typeof(Guid));
        //    dataTable.Columns.Add("CountryGuid",typeof(Guid));
        //    dataTable.Columns.Add("CountryName", typeof(string));
        //    dataTable.Columns.Add("Name",typeof(string));
        //    dataTable.Columns.Add("Code",typeof(string));
        //    dataTable.Columns.Add("TotalPages", typeof(int));

        //    IDataReader reader = DBGeoZone.GetGeoZonePage(pageNumber, pageSize);	
        //    while (reader.Read())
        //    {
        //        DataRow row = dataTable.NewRow();
        //        row["Guid"] = reader["Guid"];
        //        row["CountryGuid"] = reader["CountryGuid"];
        //        row["CountryName"] = reader["CountryName"];
        //        row["Name"] = reader["Name"];
        //        row["Code"] = reader["Code"];
        //        row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
        //        dataTable.Rows.Add(row);
        //    }

        //    reader.Close();

        //    return dataTable;

        //}

        public static DataTable GetPage(
            Guid countryGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("CountryGuid", typeof(Guid));
            dataTable.Columns.Add("CountryName", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Code", typeof(string));
            //dataTable.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = DBGeoZone.GetPage(countryGuid, pageNumber, pageSize, out totalPages))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["CountryGuid"] = reader["CountryGuid"];
                    row["CountryName"] = reader["CountryName"];
                    row["Name"] = reader["Name"];
                    row["Code"] = reader["Code"];
                    //row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;

        }



        #endregion


    }

}
