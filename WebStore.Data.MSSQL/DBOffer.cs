/// Author:				Joe Audette
/// Created:			2007-11-14
/// Last Modified:		2010-07-02
/// 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace WebStore.Data
{
    
    public static class DBOffer
    {
        

        #region Offer Main

        /// <summary>
        /// Inserts a row in the ws_Offer table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="isVisible"> isVisible </param>
        /// <param name="isSpecial"> isSpecial </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="url"> url </param>
        /// <param name="created"> created </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="lastModifiedBy"> lastModifiedBy </param>
        /// <param name="lastModifiedFromIP"> lastModifiedFromIP </param>
        /// <returns>int</returns>
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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Offer_Insert", 26);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@IsVisible", SqlDbType.Bit, ParameterDirection.Input, isVisible);
            sph.DefineSqlParameter("@IsSpecial", SqlDbType.Bit, ParameterDirection.Input, isSpecial);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            sph.DefineSqlParameter("@IsDonation", SqlDbType.Bit, ParameterDirection.Input, isDonation);

            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Abstract", SqlDbType.NVarChar, -1, ParameterDirection.Input, teaser);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, 15, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@ProductListName", SqlDbType.NVarChar, 255, ParameterDirection.Input, productListName);
            sph.DefineSqlParameter("@ShowDetailLink", SqlDbType.Bit, ParameterDirection.Input, showDetailLink);
            sph.DefineSqlParameter("@UserCanSetPrice", SqlDbType.Bit, ParameterDirection.Input, userCanSetPrice);
            sph.DefineSqlParameter("@MaxPerOrder", SqlDbType.Int, ParameterDirection.Input, maxPerOrder);
            sph.DefineSqlParameter("@SortRank1", SqlDbType.Int, ParameterDirection.Input, sortRank1);
            sph.DefineSqlParameter("@SortRank2", SqlDbType.Int, ParameterDirection.Input, sortRank1);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);

            int rowsAffected = sph.ExecuteNonQuery();
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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Offer_Update", 22);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsVisible", SqlDbType.Bit, ParameterDirection.Input, isVisible);
            sph.DefineSqlParameter("@IsSpecial", SqlDbType.Bit, ParameterDirection.Input, isSpecial);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            sph.DefineSqlParameter("@IsDonation", SqlDbType.Bit, ParameterDirection.Input, isDonation);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Abstract", SqlDbType.NVarChar, -1, ParameterDirection.Input, teaser);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, 15, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@ProductListName", SqlDbType.NVarChar, 255, ParameterDirection.Input, productListName);
            sph.DefineSqlParameter("@ShowDetailLink", SqlDbType.Bit, ParameterDirection.Input, showDetailLink);
            sph.DefineSqlParameter("@UserCanSetPrice", SqlDbType.Bit, ParameterDirection.Input, userCanSetPrice);
            sph.DefineSqlParameter("@MaxPerOrder", SqlDbType.Int, ParameterDirection.Input, maxPerOrder);
            sph.DefineSqlParameter("@SortRank1", SqlDbType.Int, ParameterDirection.Input, sortRank1);
            sph.DefineSqlParameter("@SortRank2", SqlDbType.Int, ParameterDirection.Input, sortRank1);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        

        public static bool Delete(
            Guid guid,
            DateTime deletedTime,
            Guid deletedBy,
            string deletedFromIP)
        {
            bool isDeleted = true;

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Offer_Delete", 5);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Offer table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
           
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the ws_Offer table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_GetCount", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        

        /// <summary>
        /// Gets a page of data from the ws_Offer table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_SelectPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        

        public static DataTable GetPublicPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_SelectPublicPage", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);

            DataTable dataTable = GetOfferEmptyTable();

            using (IDataReader reader = sph.ExecuteReader())
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

        /// <summary>
        /// Gets a count of rows in the ws_Offer table.
        /// </summary>
        public static int GetCountForProductList(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_GetCountForProductList", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);

            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static DataTable GetByProduct(Guid productGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_GetByProduct", 1);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);

            DataTable dataTable = GetEmptyTable();

            using(IDataReader reader = sph.ExecuteReader())
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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_GetListForPageOfProducts", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            

            DataTable dataTable = GetEmptyTable();

            using (IDataReader reader = sph.ExecuteReader())
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
            //dataTable.Columns.Add("SortRank1", typeof(int));
            //dataTable.Columns.Add("SortRank2", typeof(int));


            return dataTable;

        }

        /// <summary>
        /// Gets a page of data from the ws_Offer table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageForProductList(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCountForProductList(storeGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_SelectPageForProductList", 3);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        //public static IDataReader GetPage(
        //    Guid storeGuid,
        //    Guid languageGuid,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_Offer_SelectPage", 4);
        //    sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
        //    sph.DefineSqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, languageGuid);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();
        //}

        public static IDataReader GetTop10Specials(Guid storeGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_SelectTop10Specials", 1);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            
            return sph.ExecuteReader();

        }

        public static IDataReader GetByPage(int siteId, int pageId)
        {

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Offer_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();

        }

        #endregion

        //#region Offer Description

        //public static int AddOfferDescription(
        //    Guid offerGuid,
        //    Guid languageGuid,
        //    string shortDescription,
        //    string longDescription)
        //{

        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferDescription_Insert", 4);
        //    sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
        //    sph.DefineSqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, languageGuid);
        //    sph.DefineSqlParameter("@ShortDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, shortDescription);
        //    sph.DefineSqlParameter("@LongDescription", SqlDbType.NText, ParameterDirection.Input, longDescription);
            
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return rowsAffected;

        //}


        //public static bool UpdateOfferDescription(
        //    Guid offerGuid, Guid languageGuid,
        //    string shortDescription,
        //    string longDescription)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferDescription_Update", 4);
        //    sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
        //    sph.DefineSqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, languageGuid);
        //    sph.DefineSqlParameter("@ShortDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, shortDescription);
        //    sph.DefineSqlParameter("@LongDescription", SqlDbType.NText, ParameterDirection.Input, longDescription);

        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);

        //}

        //public static bool DeleteOfferDescription(
        //    Guid offerGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferDescription_Delete", 1);
        //    sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);

        //}

        //public static IDataReader GetOfferDescription(
        //    Guid offerGuid, Guid languageGuid)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "ws_OfferDescription_SelectOne", 2);
        //    sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
        //    sph.DefineSqlParameter("@LanguageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, languageGuid);
        //    return sph.ExecuteReader();

        //}


        //#endregion

        #region Offer History

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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_OfferHistory_Insert", 18);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@IsVisible", SqlDbType.Bit, ParameterDirection.Input, isVisible);
            sph.DefineSqlParameter("@IsSpecial", SqlDbType.Bit, ParameterDirection.Input, isSpecial);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@DeletedTime", SqlDbType.DateTime, ParameterDirection.Input, deletedTime);
            sph.DefineSqlParameter("@DeletedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, deletedBy);
            sph.DefineSqlParameter("@DeletedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, deletedFromIP);
            sph.DefineSqlParameter("@LastModified", SqlDbType.DateTime, ParameterDirection.Input, lastModified);
            sph.DefineSqlParameter("@LastModifiedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModifiedBy);
            sph.DefineSqlParameter("@LastModifiedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastModifiedFromIP);
            sph.DefineSqlParameter("@LogTime", SqlDbType.DateTime, ParameterDirection.Input, logTime);
            sph.DefineSqlParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, taxClassGuid);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            

        }

        #endregion

        

        

       



        


    }
}
