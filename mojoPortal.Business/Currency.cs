// Author:					
// Created:				    2007-02-18
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
    ///
    /// </summary>
    public class Currency
    {

        #region Constructors

        public Currency()
        { }


        public Currency(Guid guid)
        {
            GetCurrency(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private string title = string.Empty;
        private string code = "USD";
        private string symbolLeft = "$";
        private string symbolRight = string.Empty;
        private string decimalPointChar = ".";
        private string thousandsPointChar = ",";
        private string decimalPlaces = string.Empty;
        private decimal value = 1;
        private DateTime lastModified = DateTime.UtcNow;
        private DateTime created = DateTime.UtcNow;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string SymbolLeft
        {
            get { return symbolLeft; }
            set { symbolLeft = value; }
        }
        public string SymbolRight
        {
            get { return symbolRight; }
            set { symbolRight = value; }
        }
        public string DecimalPointChar
        {
            get { return decimalPointChar; }
            set { decimalPointChar = value; }
        }
        public string ThousandsPointChar
        {
            get { return thousandsPointChar; }
            set { thousandsPointChar = value; }
        }
        public string DecimalPlaces
        {
            get { return decimalPlaces; }
            set { decimalPlaces = value; }
        }
        public decimal Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        #endregion

        #region Private Methods

        private void GetCurrency(Guid guid)
        {
            using (IDataReader reader = DBCurrency.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.title = reader["Title"].ToString();
                    this.code = reader["Code"].ToString();
                    this.symbolLeft = reader["SymbolLeft"].ToString();
                    this.symbolRight = reader["SymbolRight"].ToString();
                    this.decimalPointChar = reader["DecimalPointChar"].ToString();
                    this.thousandsPointChar = reader["ThousandsPointChar"].ToString();
                    this.decimalPlaces = reader["DecimalPlaces"].ToString();
                    this.value = Convert.ToDecimal(reader["Value"]);
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.created = Convert.ToDateTime(reader["Created"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBCurrency.Create(
                this.guid,
                this.title,
                this.code,
                this.symbolLeft,
                this.symbolRight,
                this.decimalPointChar,
                this.thousandsPointChar,
                this.decimalPlaces,
                this.value,
                this.lastModified,
                this.created);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBCurrency.Update(
                this.guid,
                this.title,
                this.code,
                this.symbolLeft,
                this.symbolRight,
                this.decimalPointChar,
                this.thousandsPointChar,
                this.decimalPlaces,
                this.value,
                this.lastModified);

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
            return DBCurrency.Delete(guid);
        }


        public static DataTable GetAll()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Code", typeof(string));
            dataTable.Columns.Add("SymbolLeft", typeof(string));
            dataTable.Columns.Add("SymbolRight", typeof(string));
            dataTable.Columns.Add("DecimalPointChar", typeof(string));
            dataTable.Columns.Add("ThousandsPointChar", typeof(string));
            dataTable.Columns.Add("DecimalPlaces", typeof(string));
            dataTable.Columns.Add("Value", typeof(decimal));
            dataTable.Columns.Add("LastModified", typeof(DateTime));
            dataTable.Columns.Add("Created", typeof(DateTime));

            using (IDataReader reader = DBCurrency.GetAll())
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = reader["Guid"];
                    row["Title"] = reader["Title"];
                    row["Code"] = reader["Code"];
                    row["SymbolLeft"] = reader["SymbolLeft"];
                    row["SymbolRight"] = reader["SymbolRight"];
                    row["DecimalPointChar"] = reader["DecimalPointChar"];
                    row["ThousandsPointChar"] = reader["ThousandsPointChar"];
                    row["DecimalPlaces"] = reader["DecimalPlaces"];
                    row["Value"] = reader["Value"];
                    row["LastModified"] = reader["LastModified"];
                    row["Created"] = reader["Created"];

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;
        }

        /*
        // TODO: uncomment and implement if needed
        public static DataTable GetPage(int pageNumber, int pageSize)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid",typeof(Guid));
            dataTable.Columns.Add("Title",typeof(string));
            dataTable.Columns.Add("Code",typeof(string));
            dataTable.Columns.Add("SymbolLeft",typeof(string));
            dataTable.Columns.Add("SymbolRight",typeof(string));
            dataTable.Columns.Add("DecimalPointChar",typeof(string));
            dataTable.Columns.Add("ThousandsPointChar",typeof(string));
            dataTable.Columns.Add("DecimalPlaces",typeof(string));
            dataTable.Columns.Add("Value",typeof(decimal));
            dataTable.Columns.Add("LastModified",typeof(DateTime));
            dataTable.Columns.Add("Created",typeof(DateTime));
            dataTable.Columns.Add("TotalPages", typeof(int));
		
            IDataReader reader = DBCurrency.GetCurrencyPage(pageNumber, pageSize);	
            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();
                row["Guid"] = reader["Guid"];
                row["Title"] = reader["Title"];
                row["Code"] = reader["Code"];
                row["SymbolLeft"] = reader["SymbolLeft"];
                row["SymbolRight"] = reader["SymbolRight"];
                row["DecimalPointChar"] = reader["DecimalPointChar"];
                row["ThousandsPointChar"] = reader["ThousandsPointChar"];
                row["DecimalPlaces"] = reader["DecimalPlaces"];
                row["Value"] = reader["Value"];
                row["LastModified"] = reader["LastModified"];
                row["Created"] = reader["Created"];
                row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
                dataTable.Rows.Add(row);
            }
		
            reader.Close();
		
            return dataTable;
		
        }
	
        */

        #endregion


    }

}
