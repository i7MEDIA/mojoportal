using System;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-02-18
    /// Last Modified:			2008-04-06
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public class OrderStatus
    {
        const string OrderStatusReceivedId = "0db28432-d9a9-423e-84f2-8a94db434643";
        const string OrderStatusFulfillableId = "70443443-f665-42c9-b69f-48cbf011a14b";
        const string OrderStatusFulfilledId = "67e92035-e8d0-4700-822b-a4002f2f1a15";

        public static Guid OrderStatusReceivedGuid
        {
            get { return new Guid(OrderStatusReceivedId); }
        }

        public static Guid OrderStatusFulfillableGuid
        {
            get { return new Guid(OrderStatusFulfillableId); }
        }

        public static Guid OrderStatusFulfilledGuid
        {
            get { return new Guid(OrderStatusFulfilledId); }
        }


        #region Constructors

        public OrderStatus(Guid languageGuid)
        {
            langGuid = languageGuid;
        }


        public OrderStatus(Guid statusGuid, Guid languageGuid)
        {
            
            GetOrderStatus(statusGuid);
            langGuid = languageGuid;
            GetDescription(statusGuid, languageGuid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private int sort;
        private Guid langGuid = Guid.Empty;
        private Guid descriptionGuid = Guid.Empty;
        private string description = string.Empty;
        private Guid guidToUse = Guid.NewGuid();

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            //set { guid = value; }
        }

        public Guid GuidToUse
        {
            get { return guidToUse; }
            set { guidToUse = value; }
        }
        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }

        public Guid LanguageGuid
        {
            get { return langGuid; }
            set { langGuid = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region Private Methods

        private void GetOrderStatus(Guid guid)
        {
            IDataReader reader = DBOrderStatus.Get(guid);

            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.sort = Convert.ToInt32(reader["Sort"]);

            }

            reader.Close();

        }

        private void GetDescription(
            Guid statusGuid,
            Guid languageGuid)
        {
            IDataReader reader = DBOrderStatus.GetDescription(
                statusGuid,
                languageGuid);

            if (reader.Read())
            {
                this.langGuid = new Guid(reader["LanguageGuid"].ToString());
                this.description = reader["Description"].ToString();

            }

            reader.Close();

        }

        private bool Create()
        {
            
            this.guid = guidToUse;

            int rowsAffected = DBOrderStatus.Add(
                this.guid,
                this.sort);

            return (rowsAffected > 0);

        }



        private bool Update()
        {
            return DBOrderStatus.Update(
                this.guid,
                this.sort);

        }

        private bool LanguageDescriptionExists()
        {
            bool result = false;

            IDataReader reader = DBOrderStatus.GetDescription(
                guid, langGuid);
            if (reader.Read())
            {
                result = true;
            }
            reader.Close();


            return result;
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

            if (result)
            {
                if (LanguageDescriptionExists())
                {
                    DBOrderStatus.UpdateDescription(
                        guid,
                        langGuid,
                        description);
                }
                else
                {
                    DBOrderStatus.AddDescription(
                        guid,
                        langGuid,
                        Description);
                }
            }

            return result;
        }

        public bool Delete()
        {
            // TODO: check if any order have this status first
            //bool result = DBOrderStatus.DeleteOrderStatusDescription(guid,langGuid);
            bool result = DBOrderStatus.DeleteDescription(guid);

            result = DBOrderStatus.Delete(guid);

            return result;
        }


        #endregion

        #region Static Methods

        


        //public static IDataReader GetAll(Guid languageGuid)
        //{
        //    return DBOrderStatus.GetAll(languageGuid);
        //}

        public static DataTable GetAll(Guid languageGuid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Sort", typeof(int));
            dataTable.Columns.Add("Description", typeof(string));
           // dataTable.Columns.Add("TotalPages", typeof(int));

            IDataReader reader = DBOrderStatus.GetAll(languageGuid);
            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();
                row["Guid"] = reader["Guid"];
                row["Sort"] = reader["Sort"];
                row["Description"] = reader["Description"];
                //row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
                dataTable.Rows.Add(row);
            }

            reader.Close();

            return dataTable;

        }

        

        #endregion


    }

}
