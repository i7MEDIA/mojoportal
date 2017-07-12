// Author:					
// Created:				    2007-02-22
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
using System.Collections.ObjectModel;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a tax rate associated with a geographic location
    /// </summary>
    public class TaxRate
    {

        #region Constructors

        public TaxRate(Guid siteGuid, Guid geoZoneGuid)
        {
            this.siteGuid = siteGuid;
            this.geoZoneGuid = geoZoneGuid;

        }


        public TaxRate(Guid guid)
        {
            GetTaxRate(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid geoZoneGuid = Guid.Empty;
        private Guid taxClassGuid = Guid.Empty;
        private int priority = 1;
        private decimal rate = 0;
        private string description;
        private DateTime created;
        private Guid createdBy = Guid.Empty;
        private DateTime lastModified;
        private Guid modifiedBy = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid GeoZoneGuid
        {
            get { return geoZoneGuid; }
            set { geoZoneGuid = value; }
        }
        public Guid TaxClassGuid
        {
            get { return taxClassGuid; }
            set { taxClassGuid = value; }
        }
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        public decimal Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public Guid ModifiedBy
        {
            get { return modifiedBy; }
            set { modifiedBy = value; }
        }

        #endregion

        #region Private Methods

        private void GetTaxRate(Guid guid)
        {
            using (IDataReader reader = DBTaxRate.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.geoZoneGuid = new Guid(reader["GeoZoneGuid"].ToString());
                    this.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    this.priority = Convert.ToInt32(reader["Priority"]);
                    this.rate = Convert.ToDecimal(reader["Rate"]);
                    this.description = reader["Description"].ToString();
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    if (
                        (reader["ModifiedBy"] != DBNull.Value)
                        && (reader["ModifiedBy"].ToString().Trim() != string.Empty)
                        )
                    {
                        this.modifiedBy = new Guid(reader["ModifiedBy"].ToString());
                    }

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBTaxRate.Create(
                this.guid,
                this.siteGuid,
                this.geoZoneGuid,
                this.taxClassGuid,
                this.priority,
                this.rate,
                this.description,
                this.created,
                this.createdBy,
                this.lastModified,
                this.modifiedBy);

            return (rowsAffected > 0);

        }



        private bool Update()
        {
            LogHistory(this.guid);
            return DBTaxRate.Update(
                this.guid,
                this.geoZoneGuid,
                this.taxClassGuid,
                this.priority,
                this.rate,
                this.description,
                this.lastModified,
                this.modifiedBy);

        }

        private static void LogHistory(Guid guid)
        {
            TaxRate previousVersion = new TaxRate(guid);
            if (previousVersion.Guid != Guid.Empty)
            {
                DBTaxRate.AddHistory(
                    Guid.NewGuid(),
                    previousVersion.Guid,
                    previousVersion.siteGuid,
                    previousVersion.GeoZoneGuid,
                    previousVersion.TaxClassGuid,
                    previousVersion.Priority,
                    previousVersion.Rate,
                    previousVersion.Description,
                    previousVersion.Created,
                    previousVersion.CreatedBy,
                    previousVersion.LastModified,
                    previousVersion.ModifiedBy,
                    DateTime.UtcNow);
            }


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
            LogHistory(guid);
            return DBTaxRate.Delete(guid);
        }

        public static DataTable GetAll(Guid siteGuid, Guid geoZoneGuid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("GeoZoneGuid", typeof(Guid));
            dataTable.Columns.Add("TaxClassGuid", typeof(Guid));
            dataTable.Columns.Add("Priority", typeof(int));
            dataTable.Columns.Add("Rate", typeof(decimal));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Created", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(Guid));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("ModifiedBy", typeof(Guid));


            using (IDataReader reader = DBTaxRate.GetTaxRates(siteGuid, geoZoneGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["SiteGuid"] = new Guid(reader["SiteGuid"].ToString());
                    row["GeoZoneGuid"] = new Guid(reader["GeoZoneGuid"].ToString());
                    row["TaxClassGuid"] = new Guid(reader["TaxClassGuid"].ToString());
                    row["Priority"] = reader["Priority"];
                    row["Rate"] = reader["Rate"];
                    row["Description"] = reader["Description"];
                    row["Created"] = reader["Created"];
                    row["CreatedBy"] = new Guid(reader["CreatedBy"].ToString());
                    row["LastModified"] = reader["LastModified"];

                    if (
                        (reader["ModifiedBy"] != DBNull.Value)
                        && (reader["ModifiedBy"].ToString().Trim() != string.Empty)
                        )
                    {
                        row["ModifiedBy"] = new Guid(reader["ModifiedBy"].ToString());
                    }
                    else
                    {
                        row["ModifiedBy"] = Guid.Empty;
                    }

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;

        }

        public static Collection<TaxRate> GetTaxRates(Guid siteGuid, Guid geoZoneGuid)
        {

            Collection<TaxRate> taxRates = new Collection<TaxRate>();

            using (IDataReader reader = DBTaxRate.GetTaxRates(siteGuid, geoZoneGuid))
            {
                while (reader.Read())
                {
                    TaxRate taxRate = new TaxRate(siteGuid, geoZoneGuid);

                    taxRate.guid = new Guid(reader["Guid"].ToString());
                    taxRate.description = reader["Description"].ToString();
                    taxRate.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    taxRate.priority = Convert.ToInt32(reader["Priority"]);
                    taxRate.rate = Convert.ToDecimal(reader["Rate"]);

                    taxRates.Add(taxRate);

                }

            }

            return taxRates;

        }


        



        #endregion


    }

}
