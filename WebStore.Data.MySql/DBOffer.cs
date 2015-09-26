/// Author:					Joe Audette
/// Created:				2008-02-24
/// Last Modified:		    2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace WebStore.Data
{
    
    public static class DBOffer
    {
        

        public static int Create(
            Guid guid,
            Guid storeGuid,
            bool isVisible,
            bool isSpecial,
            Guid taxClassGuid,
            string url,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP,
            bool isDonation,
            string name,
            string description,
            string teaser,
            decimal price,
            string productListName,
            bool showDetailLink,
            bool userCanSetPrice,
            int maxPerOrder,
            int sortRank1,
            int sortRank2,
            string metaDescription,
            string metaKeywords,
            string compiledMeta)
        {
            #region Bit Conversion

            int intUserCanSetPrice = 0;
            if (userCanSetPrice)
            {
                intUserCanSetPrice = 1;
            }
            
            int intIsVisible = 0;
            if (isVisible)
            {
                intIsVisible = 1;
            }
            
            int intIsSpecial = 0;
            if (isSpecial)
            {
                intIsSpecial = 1;
            }
            
            int intIsDonation = 0;
            if (isDonation)
            {
                intIsDonation = 1;
            }
           
            int intShowDetailLink = 0;
            if (showDetailLink)
            {
                intShowDetailLink = 1;
            }
           

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_Offer (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("IsVisible, ");
            sqlCommand.Append("IsSpecial, ");
            sqlCommand.Append("IsDonation, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifiedBy, ");
            sqlCommand.Append("LastModifiedFromIP, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("ProductListName, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Abstract, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("ShowDetailLink, ");
            sqlCommand.Append("UserCanSetPrice, ");
            sqlCommand.Append("MaxPerOrder, ");
            sqlCommand.Append("MetaDescription, ");
            sqlCommand.Append("MetaKeywords, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("SortRank1, ");
            sqlCommand.Append("SortRank2, ");

            sqlCommand.Append("Url )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?IsVisible, ");
            sqlCommand.Append("?IsSpecial, ");
            sqlCommand.Append("?IsDonation, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifiedBy, ");
            sqlCommand.Append("?LastModifiedFromIP, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?ProductListName, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?Abstract, ");
            sqlCommand.Append("?Price, ");
            sqlCommand.Append("?ShowDetailLink, ");
            sqlCommand.Append("?UserCanSetPrice, ");
            sqlCommand.Append("?MaxPerOrder, ");
            sqlCommand.Append("?MetaDescription, ");
            sqlCommand.Append("?MetaKeywords, ");
            sqlCommand.Append("CompiledMeta, ");
            sqlCommand.Append("?SortRank1, ");
            sqlCommand.Append("?SortRank2, ");
            sqlCommand.Append("?Url )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[26];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            arParams[2] = new MySqlParameter("?IsVisible", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intIsVisible;

            arParams[3] = new MySqlParameter("?IsSpecial", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intIsSpecial;

            arParams[4] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = taxClassGuid.ToString();

            arParams[5] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = created;

            arParams[6] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy.ToString();

            arParams[7] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdFromIP;

            arParams[8] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModified;

            arParams[9] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModifiedBy.ToString();

            arParams[10] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModifiedFromIP;

            arParams[11] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = url;

            arParams[12] = new MySqlParameter("?IsDonation", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intIsDonation;

            arParams[13] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = name;

            arParams[14] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = description;

            arParams[15] = new MySqlParameter("?Abstract", MySqlDbType.Text);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = teaser;

            arParams[16] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = price;

            arParams[17] = new MySqlParameter("?ProductListName", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = productListName;

            arParams[18] = new MySqlParameter("?ShowDetailLink", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intShowDetailLink;

            arParams[19] = new MySqlParameter("?UserCanSetPrice", MySqlDbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intUserCanSetPrice;

            arParams[20] = new MySqlParameter("?MaxPerOrder", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = maxPerOrder;

            arParams[21] = new MySqlParameter("?SortRank1", MySqlDbType.Int32);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = sortRank1;

            arParams[22] = new MySqlParameter("?SortRank2", MySqlDbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = sortRank2;

            arParams[23] = new MySqlParameter("?CompiledMeta", MySqlDbType.Text);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = compiledMeta;

            arParams[24] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = metaDescription;

            arParams[25] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = metaKeywords;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the ws_Offer table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="isVisible"> isVisible </param>
        /// <param name="isSpecial"> isSpecial </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="url"> url </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="lastModifiedBy"> lastModifiedBy </param>
        /// <param name="lastModifiedFromIP"> lastModifiedFromIP </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            bool isVisible,
            bool isSpecial,
            Guid taxClassGuid,
            string url,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP,
            bool isDonation,
            string name,
            string description,
            string teaser,
            decimal price,
            string productListName,
            bool showDetailLink,
            bool userCanSetPrice,
            int maxPerOrder,
            int sortRank1,
            int sortRank2,
            string metaDescription,
            string metaKeywords,
            string compiledMeta)
        {
            #region Bit Conversion

            int intUserCanSetPrice;
            if (userCanSetPrice)
            {
                intUserCanSetPrice = 1;
            }
            else
            {
                intUserCanSetPrice = 0;
            }

            int intIsVisible;
            if (isVisible)
            {
                intIsVisible = 1;
            }
            else
            {
                intIsVisible = 0;
            }

            int intIsSpecial;
            if (isSpecial)
            {
                intIsSpecial = 1;
            }
            else
            {
                intIsSpecial = 0;
            }

            int intIsDonation;
            if (isDonation)
            {
                intIsDonation = 1;
            }
            else
            {
                intIsDonation = 0;
            }

            int intShowDetailLink;
            if (showDetailLink)
            {
                intShowDetailLink = 1;
            }
            else
            {
                intShowDetailLink = 0;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Offer ");
            sqlCommand.Append("SET  ");
     
            sqlCommand.Append("IsVisible = ?IsVisible, ");
            sqlCommand.Append("IsSpecial = ?IsSpecial, ");
            sqlCommand.Append("IsDonation = ?IsDonation, ");
            sqlCommand.Append("TaxClassGuid = ?TaxClassGuid, ");
            
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastModifiedBy = ?LastModifiedBy, ");
            sqlCommand.Append("LastModifiedFromIP = ?LastModifiedFromIP, ");

            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("ProductListName = ?ProductListName, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("Abstract = ?Abstract, ");
            sqlCommand.Append("Price = ?Price, ");
            sqlCommand.Append("ShowDetailLink = ?ShowDetailLink, ");
            sqlCommand.Append("UserCanSetPrice = ?UserCanSetPrice, ");
            sqlCommand.Append("MaxPerOrder = ?MaxPerOrder, ");
            sqlCommand.Append("SortRank1 = ?SortRank1, ");
            sqlCommand.Append("SortRank2 = ?SortRank2, ");
            sqlCommand.Append("MetaDescription = ?MetaDescription, ");
            sqlCommand.Append("MetaKeywords = ?MetaKeywords, ");
            sqlCommand.Append("CompiledMeta = ?CompiledMeta, ");
            sqlCommand.Append("Url = ?Url ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[22];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?IsVisible", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIsVisible;

            arParams[2] = new MySqlParameter("?IsSpecial", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intIsSpecial;

            arParams[3] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid.ToString();

            arParams[4] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastModified;

            arParams[5] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModifiedBy.ToString();

            arParams[6] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModifiedFromIP;

            arParams[7] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = url;

            arParams[8] = new MySqlParameter("?IsDonation", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intIsDonation;

            arParams[9] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = name;

            arParams[10] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = description;

            arParams[11] = new MySqlParameter("?Abstract", MySqlDbType.Text);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = teaser;

            arParams[12] = new MySqlParameter("?Price", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = price;

            arParams[13] = new MySqlParameter("?ProductListName", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = productListName;

            arParams[14] = new MySqlParameter("?ShowDetailLink", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intShowDetailLink;

            arParams[15] = new MySqlParameter("?UserCanSetPrice", MySqlDbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intUserCanSetPrice;

            arParams[16] = new MySqlParameter("?MaxPerOrder", MySqlDbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = maxPerOrder;

            arParams[17] = new MySqlParameter("?SortRank1", MySqlDbType.Int32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = sortRank1;

            arParams[18] = new MySqlParameter("?SortRank2", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = sortRank2;

            arParams[19] = new MySqlParameter("?CompiledMeta", MySqlDbType.Text);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = compiledMeta;

            arParams[20] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = metaDescription;

            arParams[21] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = metaKeywords;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }


        public static bool Delete(
            Guid guid,
            DateTime deletedTime,
            Guid deletedBy,
            string deletedFromIP)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Offer ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("IsDeleted = 1, ");
            sqlCommand.Append("DeletedTime = ?DeletedTime, ");
            sqlCommand.Append("DeletedBy = ?DeletedBy, ");
            sqlCommand.Append("DeletedFromIP = ?DeletedFromIP ");
           
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = deletedTime;

            arParams[2] = new MySqlParameter("?DeletedBy", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = deletedBy.ToString();

            arParams[3] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = deletedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Offer  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }


        /// <summary>
        /// Gets a count of rows in the ws_Offer table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Offer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a count of rows in the ws_Offer table.
        /// </summary>
        public static int GetCountForProductList(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Offer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND IsVisible = 1 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static DataTable GetByProduct(Guid productGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT o.*, ");
            sqlCommand.Append("op.ProductGuid ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Offer o ");

            sqlCommand.Append("JOIN ws_OfferProduct op ");
            sqlCommand.Append("ON o.Guid = op.OfferGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("op.ProductGuid = ?ProductGuid ");
            sqlCommand.Append("AND o.IsVisible = 1 ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND op.IsDeleted = 0 ");
            sqlCommand.Append("ORDER BY o.SortRank1, o.SortRank2, o.Price ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            DataTable dataTable = GetEmptyTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["ProductGuid"] = new Guid(reader["ProductGuid"].ToString());
                    row["Price"] = Convert.ToDecimal(reader["Price"]);
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["ProductListName"] = reader["ProductListName"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["ShowDetailLink"] = Convert.ToBoolean(reader["ShowDetailLink"]);

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;

        }

        public static DataTable GetListForPageOfProducts(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT o.*, ");
            sqlCommand.Append("op.ProductGuid ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("ws_Offer o ");

            sqlCommand.Append("JOIN ws_OfferProduct op ");
            sqlCommand.Append("ON o.Guid = op.OfferGuid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("(SELECT	p.Guid ");
            sqlCommand.Append("FROM	ws_Product p  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");
            sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append("AND p.Guid IN ( ");
            sqlCommand.Append("SELECT op.ProductGuid ");
            sqlCommand.Append("FROM ws_OfferProduct op ");
            sqlCommand.Append("JOIN ws_Offer o ");
            sqlCommand.Append("ON	op.OfferGuid = o.Guid ");
            sqlCommand.Append("WHERE op.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsVisible = 1 ");
            sqlCommand.Append(")");


            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.SortRank1, p.SortRank2, p.Name ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(") f ");

            sqlCommand.Append("ON f.Guid = op.ProductGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.IsVisible = 1 ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND op.IsDeleted = 0 ");
            sqlCommand.Append("ORDER BY o.SortRank1, o.SortRank2, o.Price ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            DataTable dataTable = GetEmptyTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["ProductGuid"] = new Guid(reader["ProductGuid"].ToString());
                    row["Price"] = Convert.ToDecimal(reader["Price"]);
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["ProductListName"] = reader["ProductListName"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["ShowDetailLink"] = Convert.ToBoolean(reader["ShowDetailLink"]);

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;

        }

        private static DataTable GetEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("ProductGuid", typeof(Guid));
            //dataTable.Columns.Add("StoreGuid", typeof(Guid));
            //dataTable.Columns.Add("TaxClassGuid", typeof(Guid));
            dataTable.Columns.Add("Price", typeof(decimal));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("ProductListName", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ShowDetailLink", typeof(bool));


            return dataTable;

        }

        public static IDataReader GetPageForProductList(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountForProductList(storeGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            
            sqlCommand.Append("FROM	ws_Offer   ");

            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND IsVisible = 1 ");

            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("Name  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }


        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
		{
			int pageLowerBound = (pageSize * pageNumber) - pageSize;
			totalPages = 1;
			int totalRows = GetCount(storeGuid);
			
			if (pageSize > 0) totalPages = totalRows / pageSize;
			
			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				int remainder;
				Math.DivRem(totalRows, pageSize, out remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	ws_Offer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("and IsDeleted = false " );
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("Name  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static DataTable GetPublicPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountForProductList(storeGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	ws_Offer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("and IsDeleted = false ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SortRank1, SortRank2, Name  ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            DataTable dataTable = GetOfferEmptyTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["ProductListName"] = reader["ProductListName"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["Price"] = reader["Price"];

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;


        }

        private static DataTable GetOfferEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("ProductListName", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));



            return dataTable;

        }

        public static IDataReader GetTop10Specials(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	ws_Offer  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND IsDeleted = 0 ");
            sqlCommand.Append("AND IsVisible = 1 ");
            sqlCommand.Append("AND IsSpecial = 1 ");
           
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SortRank1, SortRank2, Name  ");
            sqlCommand.Append("LIMIT 10 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  o.*, ");
            sqlCommand.Append("s.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	ws_Offer o ");

            sqlCommand.Append("JOIN	ws_Store s ");
            sqlCommand.Append("ON s.Guid = o.StoreGuid ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON s.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = ?SiteID ");
            sqlCommand.Append("AND pm.PageID = ?PageID ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsVisible = 1 ");

            sqlCommand.Append("AND o.Guid IN (");
            sqlCommand.Append("SELECT op.OfferGuid ");
            sqlCommand.Append("FROM ws_OfferProduct op ");
            sqlCommand.Append("JOIN ws_Product p ");
            sqlCommand.Append("ON op.ProductGuid = p.Guid ");
            sqlCommand.Append("WHERE op.IsDeleted = 0 ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");
            sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append(")");


            sqlCommand.Append(" ; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        //public static int AddOfferDescription(
        //    Guid offerGuid,
        //    Guid languageGuid,
        //    string shortDescription,
        //    string longDescription)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("INSERT INTO ws_OfferDescription (");
        //    sqlCommand.Append("OfferGuid, ");
        //    sqlCommand.Append("LanguageGuid, ");
        //    sqlCommand.Append("ShortDescription, ");
        //    sqlCommand.Append("LongDescription )");

        //    sqlCommand.Append(" VALUES (");
        //    sqlCommand.Append("?OfferGuid, ");
        //    sqlCommand.Append("?LanguageGuid, ");
        //    sqlCommand.Append("?ShortDescription, ");
        //    sqlCommand.Append("?LongDescription )");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[4];

        //    arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = offerGuid.ToString();

        //    arParams[1] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = languageGuid.ToString();

        //    arParams[2] = new MySqlParameter("?ShortDescription", MySqlDbType.VarChar, 255);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = shortDescription;

        //    arParams[3] = new MySqlParameter("?LongDescription", MySqlDbType.Text);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = longDescription;

        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);
        //    return rowsAffected;

           
        //}


        //public static bool UpdateOfferDescription(
        //    Guid offerGuid, Guid languageGuid,
        //    string shortDescription,
        //    string longDescription)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE ws_OfferDescription ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("ShortDescription = ?ShortDescription, ");
        //    sqlCommand.Append("LongDescription = ?LongDescription ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("OfferGuid = ?OfferGuid AND ");
        //    sqlCommand.Append("LanguageGuid = ?LanguageGuid ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[4];

        //    arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = offerGuid.ToString();

        //    arParams[1] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = languageGuid.ToString();

        //    arParams[2] = new MySqlParameter("?ShortDescription", MySqlDbType.VarChar, 255);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = shortDescription;

        //    arParams[3] = new MySqlParameter("?LongDescription", MySqlDbType.Text);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = longDescription;

        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);


        //}

        //public static bool DeleteOfferDescription(Guid offerGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("DELETE FROM ws_OfferDescription ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("OfferGuid = ?OfferGuid ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = offerGuid.ToString();

        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > 0);
        //}

        //public static IDataReader GetOfferDescription(
        //    Guid offerGuid, 
        //    Guid languageGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	ws_OfferDescription ");
        //    sqlCommand.Append("WHERE ");
        //    sqlCommand.Append("OfferGuid = ?OfferGuid AND ");
        //    sqlCommand.Append("LanguageGuid = ?LanguageGuid ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = offerGuid.ToString();

        //    arParams[1] = new MySqlParameter("?LanguageGuid", MySqlDbType.VarChar, 36);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = languageGuid.ToString();

        //    return MySqlHelper.ExecuteReader(
        //        GetConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //}


        public static int AddOfferHistory(
            Guid guid,
            Guid offerGuid,
            Guid storeGuid,
            bool isVisible,
            bool isSpecial,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            bool isDeleted,
            DateTime deletedTime,
            Guid deletedBy,
            string deletedFromIP,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP,
            DateTime logTime,
            Guid taxClassGuid,
            string url)
        {
            #region Bit Conversion

            int intIsVisible;
            if (isVisible)
            {
                intIsVisible = 1;
            }
            else
            {
                intIsVisible = 0;
            }

            int intIsSpecial;
            if (isSpecial)
            {
                intIsSpecial = 1;
            }
            else
            {
                intIsSpecial = 0;
            }

            int intIsDeleted;
            if (isDeleted)
            {
                intIsDeleted = 1;
            }
            else
            {
                intIsDeleted = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OfferHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("IsVisible, ");
            sqlCommand.Append("IsSpecial, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("DeletedTime, ");
            sqlCommand.Append("DeletedBy, ");
            sqlCommand.Append("DeletedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifiedBy, ");
            sqlCommand.Append("LastModifiedFromIP, ");
            sqlCommand.Append("LogTime, ");
            sqlCommand.Append("Url )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?IsVisible, ");
            sqlCommand.Append("?IsSpecial, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?IsDeleted, ");
            sqlCommand.Append("?DeletedTime, ");
            sqlCommand.Append("?DeletedBy, ");
            sqlCommand.Append("?DeletedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifiedBy, ");
            sqlCommand.Append("?LastModifiedFromIP, ");
            sqlCommand.Append("?LogTime, ");
            sqlCommand.Append("?Url )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[18];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = offerGuid.ToString();

            arParams[2] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = storeGuid.ToString();

            arParams[3] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid.ToString();

            arParams[4] = new MySqlParameter("?IsVisible", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsVisible;

            arParams[5] = new MySqlParameter("?IsSpecial", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsSpecial;

            arParams[6] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = created;

            arParams[7] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdBy.ToString();

            arParams[8] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIP;

            arParams[9] = new MySqlParameter("?IsDeleted", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intIsDeleted;

            arParams[10] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = deletedTime;

            arParams[11] = new MySqlParameter("?DeletedBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = deletedBy.ToString();

            arParams[12] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deletedFromIP;

            arParams[13] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lastModified;

            arParams[14] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = lastModifiedBy.ToString();

            arParams[15] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = lastModifiedFromIP;

            arParams[16] = new MySqlParameter("?LogTime", MySqlDbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = logTime;

            arParams[17] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = url;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }



    }
}
