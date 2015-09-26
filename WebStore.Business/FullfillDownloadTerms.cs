/// Author:					Joe Audette
/// Created:				2007-02-24
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
using System.Collections.Generic;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents the terms and duration of time by which a user can download a purchased product
    /// </summary>
    public class FullfillDownloadTerms
    {

        #region Constructors

        public FullfillDownloadTerms()
        { }

        public FullfillDownloadTerms(Guid guid)
        {
            GetFullfillDownloadTerms(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        //private Guid languageGuid = Guid.Empty;
        private string name = string.Empty;
        private string description = string.Empty;
        private int downloadsAllowed = 0;
        private int expireAfterDays = 0;
        private bool countAfterDownload = true;
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private string createdFromIP = string.Empty;
        private DateTime lastModified = DateTime.UtcNow;
        private Guid lastModifedBy = Guid.Empty;
        private string lastModifedFromIPAddress = string.Empty;

        

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }
        //public Guid LanguageGuid
        //{
        //    get { return languageGuid; }
        //}

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int DownloadsAllowed
        {
            get { return downloadsAllowed; }
            set { downloadsAllowed = value; }
        }
        public int ExpireAfterDays
        {
            get { return expireAfterDays; }
            set { expireAfterDays = value; }
        }
        public bool CountAfterDownload
        {
            get { return countAfterDownload; }
            set { countAfterDownload = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set 
            { 
                createdBy = value;
                if (lastModifedBy == Guid.Empty) lastModifedBy = value;
            }
        }
        public string CreatedFromIP
        {
            get { return createdFromIP; }
            set 
            { 
                createdFromIP = value;
                if (lastModifedFromIPAddress.Length == 0) lastModifedFromIPAddress = value;
            }
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
        public string LastModifedFromIPAddress
        {
            get { return lastModifedFromIPAddress; }
            set { lastModifedFromIPAddress = value; }
        }

        #endregion

        #region Private Methods

        private void GetFullfillDownloadTerms(Guid guid)
        {
            using (IDataReader reader = DBDownloadTerms.Get(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    this.downloadsAllowed = Convert.ToInt32(reader["DownloadsAllowed"]);
                    this.expireAfterDays = Convert.ToInt32(reader["ExpireAfterDays"]);
                    this.countAfterDownload = Convert.ToBoolean(reader["CountAfterDownload"]);
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.createdFromIP = reader["CreatedFromIP"].ToString();
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.lastModifedBy = new Guid(reader["LastModifedBy"].ToString());
                    this.lastModifedFromIPAddress = reader["LastModifedFromIPAddress"].ToString();

                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBDownloadTerms.Add(
                this.guid,
                this.storeGuid,
                this.downloadsAllowed,
                this.expireAfterDays,
                this.countAfterDownload,
                this.created,
                this.createdBy,
                this.createdFromIP,
                this.lastModified,
                this.lastModifedBy,
                this.lastModifedFromIPAddress,
                this.name,
                this.description);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBDownloadTerms.Update(
                this.guid,
                this.downloadsAllowed,
                this.expireAfterDays,
                this.countAfterDownload,
                this.lastModified,
                this.lastModifedBy,
                this.lastModifedFromIPAddress,
                this.name,
                this.description);

        }

        


        #endregion

        #region Public Methods


        public bool Save()
        {
            bool result = false;

            if (this.guid != Guid.Empty)
            {
                result = Update();
            }
            else
            {
                result = Create();
            }

            

            return result;

        }




        #endregion

        #region Static Methods

        public static bool Delete(Guid guid)
        {
            return DBDownloadTerms.Delete(guid);
        }

        private static List<FullfillDownloadTerms> LoadListFromReader(IDataReader reader)
        {
            List<FullfillDownloadTerms> fullfillDownloadTermList = new List<FullfillDownloadTerms>();
            try
            {
                while (reader.Read())
                {
                    FullfillDownloadTerms fullfillDownloadTerm = new FullfillDownloadTerms();
                    fullfillDownloadTerm.guid = new Guid(reader["Guid"].ToString());
                    fullfillDownloadTerm.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    fullfillDownloadTerm.downloadsAllowed = Convert.ToInt32(reader["DownloadsAllowed"]);
                    fullfillDownloadTerm.expireAfterDays = Convert.ToInt32(reader["ExpireAfterDays"]);
                    fullfillDownloadTerm.countAfterDownload = Convert.ToBoolean(reader["CountAfterDownload"]);
                    fullfillDownloadTerm.created = Convert.ToDateTime(reader["Created"]);
                    fullfillDownloadTerm.createdBy = new Guid(reader["CreatedBy"].ToString());
                    fullfillDownloadTerm.createdFromIP = reader["CreatedFromIP"].ToString();
                    fullfillDownloadTerm.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    fullfillDownloadTerm.lastModifedBy = new Guid(reader["LastModifedBy"].ToString());
                    fullfillDownloadTerm.lastModifedFromIPAddress = reader["LastModifedFromIPAddress"].ToString();
                    fullfillDownloadTermList.Add(fullfillDownloadTerm);

                    fullfillDownloadTerm.Description = reader["Description"].ToString();
                    fullfillDownloadTerm.name = reader["Name"].ToString();

                }
            }
            finally
            {
                reader.Close();
            }

            return fullfillDownloadTermList;

        }

        public static List<FullfillDownloadTerms> GetList(Guid storeGuid)
        {
            IDataReader reader = DBDownloadTerms.GetAll(storeGuid);
            return LoadListFromReader(reader);

        }

        public static IDataReader GetAll(Guid storeGuid)
        {
            return DBDownloadTerms.GetAll(storeGuid);
        }

        
        public static DataTable GetPage(
            Guid storeGuid,
            int pageNumber, 
            int pageSize)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid",typeof(Guid));
            dataTable.Columns.Add("StoreGuid",typeof(Guid));

            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            dataTable.Columns.Add("DownloadsAllowed",typeof(int));
            dataTable.Columns.Add("ExpireAfterDays",typeof(int));
            dataTable.Columns.Add("CountAfterDownload",typeof(bool));
            dataTable.Columns.Add("Created",typeof(DateTime));
            dataTable.Columns.Add("CreatedBy",typeof(Guid));
            dataTable.Columns.Add("CreatedFromIP",typeof(string));
            dataTable.Columns.Add("LastModified",typeof(DateTime));
            dataTable.Columns.Add("LastModifedBy",typeof(Guid));
            dataTable.Columns.Add("LastModifedFromIPAddress",typeof(string));
            dataTable.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = DBDownloadTerms.GetFullfillDownloadTermsPage(
                storeGuid,
                pageNumber,
                pageSize))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = reader["Guid"];
                    row["StoreGuid"] = reader["StoreGuid"];

                    row["Name"] = reader["Name"];
                    row["Description"] = reader["Description"];

                    row["DownloadsAllowed"] = reader["DownloadsAllowed"];
                    row["ExpireAfterDays"] = reader["ExpireAfterDays"];
                    row["CountAfterDownload"] = reader["CountAfterDownload"];
                    row["Created"] = reader["Created"];
                    row["CreatedBy"] = reader["CreatedBy"];
                    row["CreatedFromIP"] = reader["CreatedFromIP"];
                    row["LastModified"] = reader["LastModified"];
                    row["LastModifedBy"] = reader["LastModifedBy"];
                    row["LastModifedFromIPAddress"] = reader["LastModifedFromIPAddress"];
                    row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
                    dataTable.Rows.Add(row);
                }

            }
		
            return dataTable;
		
        }
	
        

        #endregion


    }

}
