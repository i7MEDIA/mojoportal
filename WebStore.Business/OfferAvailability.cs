/// Author:					Joe Audette
/// Created:				2007-03-01
/// Last Modified:			2009-02-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents the availability of an offer
    /// </summary>
    public class OfferAvailability
    {
        
        #region Constructors

        public OfferAvailability()
        { }


        public OfferAvailability(Guid guid)
        {
            GetOfferAvailability(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        private DateTime beginUTC;
        private DateTime endUTC;
        private bool requiresOfferCode;
        private string offerCode;
        private int maxAllowedPerCustomer;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private string createdFromIP;
        private bool isDeleted;
        private Guid deletedBy = Guid.Empty;
        private DateTime deletedTime;
        private string deletedFromIP;
        private DateTime lastModified = DateTime.UtcNow;
        private Guid lastModifedBy = Guid.Empty;
        private string lastModifedFromIP;

        

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid OfferGuid
        {
            get { return offerGuid; }
            set { offerGuid = value; }
        }
        public DateTime BeginUtc
        {
            get { return beginUTC; }
            set { beginUTC = value; }
        }
        public DateTime EndUtc
        {
            get { return endUTC; }
            set { endUTC = value; }
        }
        public bool RequiresOfferCode
        {
            get { return requiresOfferCode; }
            set { requiresOfferCode = value; }
        }
        public string OfferCode
        {
            get { return offerCode; }
            set { offerCode = value; }
        }
        public int MaxAllowedPerCustomer
        {
            get { return maxAllowedPerCustomer; }
            set { maxAllowedPerCustomer = value; }
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
        public string CreatedFromIP
        {
            get { return createdFromIP; }
            set { createdFromIP = value; }
        }
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
        public Guid DeletedBy
        {
            get { return deletedBy; }
            set { deletedBy = value; }
        }
        public DateTime DeletedTime
        {
            get { return deletedTime; }
            set { deletedTime = value; }
        }
        public string DeletedFromIP
        {
            get { return deletedFromIP; }
            set { deletedFromIP = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public Guid LastModifedBy
        {
            get { return lastModifedBy; }
            set { lastModifedBy = value; }
        }
        public string LastModifedFromIP
        {
            get { return lastModifedFromIP; }
            set { lastModifedFromIP = value; }
        }

        #endregion

        #region Private Methods

        

        private void GetOfferAvailability(Guid guid)
        {
            using (IDataReader reader = DBOfferAvailability.Get(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.offerGuid = new Guid(reader["OfferGuid"].ToString());
                    this.beginUTC = Convert.ToDateTime(reader["BeginUTC"]);
                    this.endUTC = Convert.ToDateTime(reader["EndUTC"]);
                    this.requiresOfferCode = Convert.ToBoolean(reader["RequiresOfferCode"]);
                    this.offerCode = reader["OfferCode"].ToString();
                    this.maxAllowedPerCustomer = Convert.ToInt32(reader["MaxAllowedPerCustomer"]);
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.createdFromIP = reader["CreatedFromIP"].ToString();
                    this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    if (reader["DeletedBy"] != DBNull.Value)
                    {
                        this.deletedBy = new Guid(reader["DeletedBy"].ToString());
                    }
                    if (reader["DeletedTime"] != DBNull.Value)
                    {
                        this.deletedTime = Convert.ToDateTime(reader["DeletedTime"]);
                    }
                    this.deletedFromIP = reader["DeletedFromIP"].ToString();
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.lastModifedBy = new Guid(reader["LastModifedBy"].ToString());
                    this.lastModifedFromIP = reader["LastModifedFromIP"].ToString();

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBOfferAvailability.Add(
                this.guid,
                this.offerGuid,
                this.beginUTC,
                this.endUTC,
                this.requiresOfferCode,
                this.offerCode,
                this.maxAllowedPerCustomer,
                this.created,
                this.createdBy,
                this.createdFromIP,
                this.lastModified,
                this.lastModifedBy,
                this.lastModifedFromIP);

            return (rowsAffected > 0);

        }



        private bool Update()
        {
            OfferAvailability availability = new OfferAvailability(this.guid);
            DBOfferAvailability.AddHistory(
                Guid.NewGuid(),
                availability.Guid,
                availability.OfferGuid,
                availability.BeginUtc,
                availability.EndUtc,
                availability.RequiresOfferCode,
                availability.OfferCode,
                availability.MaxAllowedPerCustomer,
                availability.Created,
                availability.CreatedBy,
                availability.CreatedFromIP,
                availability.IsDeleted,
                availability.DeletedBy,
                availability.DeletedTime,
                availability.DeletedFromIP,
                availability.LastModified,
                availability.LastModifedBy,
                availability.LastModifedFromIP,
                DateTime.UtcNow);

            return DBOfferAvailability.Update(
                this.guid,
                this.beginUTC,
                this.endUTC,
                this.requiresOfferCode,
                this.offerCode,
                this.maxAllowedPerCustomer,
                this.lastModified,
                this.lastModifedBy,
                this.lastModifedFromIP);

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

        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP)
        {
            OfferAvailability availability = new OfferAvailability(guid);
            DBOfferAvailability.AddHistory(
                Guid.NewGuid(),
                availability.Guid,
                availability.OfferGuid,
                availability.BeginUtc,
                availability.EndUtc,
                availability.RequiresOfferCode,
                availability.OfferCode,
                availability.MaxAllowedPerCustomer,
                availability.Created,
                availability.CreatedBy,
                availability.CreatedFromIP,
                availability.IsDeleted,
                availability.DeletedBy,
                availability.DeletedTime,
                availability.DeletedFromIP,
                availability.LastModified,
                availability.LastModifedBy,
                availability.LastModifedFromIP,
                DateTime.UtcNow);

            return DBOfferAvailability.Delete(
                guid,
                deletedBy,
                deletedTime,
                deletedFromIP);
        }


        

        public static DataTable GetByOffer(Guid offerGuid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("OfferGuid", typeof(Guid));
            dataTable.Columns.Add("BeginUTC", typeof(DateTime));
            dataTable.Columns.Add("EndUTC", typeof(DateTime));
            dataTable.Columns.Add("RequiresOfferCode", typeof(bool));
            dataTable.Columns.Add("OfferCode", typeof(string));
            dataTable.Columns.Add("MaxAllowedPerCustomer", typeof(int));

            using (IDataReader reader = DBOfferAvailability.GetByOffer(offerGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = reader["Guid"];
                    row["OfferGuid"] = reader["OfferGuid"];
                    row["BeginUTC"] = reader["BeginUTC"];
                    row["EndUTC"] = reader["EndUTC"];
                    row["RequiresOfferCode"] = reader["RequiresOfferCode"];
                    row["OfferCode"] = reader["OfferCode"];
                    row["MaxAllowedPerCustomer"] = reader["MaxAllowedPerCustomer"];
                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;

        }

        

        #endregion


    }

}
