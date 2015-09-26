/// Author:					Joe Audette
/// Created:				2008-02-26
/// Last Modified:		    2015-04-13 (Joe Davis)
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
    public static class DBProduct
    {


        public static int Add(
            Guid guid,
            Guid storeGuid,
            Guid taxClassGuid,
            string modelNumber,
            byte status,
            byte fullfillmentType,
            decimal weight,
            int quantityOnHand,
            string imageFileName,
            byte[] imageFileBytes,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid lastModifedBy,
            string url,
            string name,
            string description,
            string teaser,
            bool showInProductList,
            bool enableRating,
            string metaDescription,
            string metaKeywords,
            int sortRank1,
            int sortRank2,
            string teaserFile,
            string teaserFileLink,
            string compiledMeta,
            decimal shippingAmount)
        {
            int intShowInProductList = 0;
            if (showInProductList)
            {
                intShowInProductList = 1;
            }
            
            int intEnableRating = 0;
            if (enableRating)
            {
                intEnableRating = 1;
            }
            

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_Product (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("ModelNumber, ");
            sqlCommand.Append("Status, ");
            sqlCommand.Append("FullfillmentType, ");
            sqlCommand.Append("Weight, ");
            sqlCommand.Append("QuantityOnHand, ");
            sqlCommand.Append("ImageFileName, ");
            sqlCommand.Append("ImageFileBytes, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("Url, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Abstract, ");

            sqlCommand.Append("MetaDescription, ");
            sqlCommand.Append("MetaKeywords, ");
            sqlCommand.Append("CompiledMeta, ");

            sqlCommand.Append("ShowInProductList, ");
            sqlCommand.Append("EnableRating, ");
            sqlCommand.Append("SortRank1, ");
            sqlCommand.Append("SortRank2, ");
            sqlCommand.Append("TeaserFile, ");
            sqlCommand.Append("TeaserFileLink, ");

            sqlCommand.Append("LastModifedBy, ");
            sqlCommand.Append("ShippingAmount ");
            sqlCommand.Append(" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?ModelNumber, ");
            sqlCommand.Append("?Status, ");
            sqlCommand.Append("?FullfillmentType, ");
            sqlCommand.Append("?Weight, ");
            sqlCommand.Append("?QuantityOnHand, ");
            sqlCommand.Append("?ImageFileName, ");
            sqlCommand.Append("?ImageFileBytes, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("?Url, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?Abstract, ");
            sqlCommand.Append("?MetaDescription, ");
            sqlCommand.Append("?MetaKeywords, ");
            sqlCommand.Append("?CompiledMeta, ");
            sqlCommand.Append("?ShowInProductList, ");
            sqlCommand.Append("?EnableRating, ");
            sqlCommand.Append("?SortRank1, ");
            sqlCommand.Append("?SortRank2, ");
            sqlCommand.Append("?TeaserFile, ");
            sqlCommand.Append("?TeaserFileLink, ");
            sqlCommand.Append("?LastModifedBy, ");
            sqlCommand.Append("?ShippingAmount ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[28];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            arParams[2] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = taxClassGuid.ToString();

            arParams[3] = new MySqlParameter("?ModelNumber", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = modelNumber;

            arParams[4] = new MySqlParameter("?Status", MySqlDbType.Int16);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = status;

            arParams[5] = new MySqlParameter("?FullfillmentType", MySqlDbType.Int16);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = fullfillmentType;

            arParams[6] = new MySqlParameter("?Weight", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = weight;

            arParams[7] = new MySqlParameter("?QuantityOnHand", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = quantityOnHand;

            arParams[8] = new MySqlParameter("?ImageFileName", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = imageFileName;

            arParams[9] = new MySqlParameter("?ImageFileBytes", MySqlDbType.LongBlob);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = imageFileBytes;

            arParams[10] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = created;

            arParams[11] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = createdBy.ToString();

            arParams[12] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lastModified;

            arParams[13] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lastModifedBy.ToString();

            arParams[14] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = url;

            arParams[15] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = name;

            arParams[16] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = description;

            arParams[17] = new MySqlParameter("?Abstract", MySqlDbType.Text);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = teaser;

            arParams[18] = new MySqlParameter("?ShowInProductList", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intShowInProductList;

            arParams[19] = new MySqlParameter("?EnableRating", MySqlDbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = intEnableRating;

            arParams[20] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = metaDescription;

            arParams[21] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = metaKeywords;

            arParams[22] = new MySqlParameter("?SortRank1", MySqlDbType.Int32);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = sortRank1;

            arParams[23] = new MySqlParameter("?SortRank2", MySqlDbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = sortRank2;

            arParams[24] = new MySqlParameter("?TeaserFile", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = teaserFile;

            arParams[25] = new MySqlParameter("?TeaserFileLink", MySqlDbType.VarChar, 255);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = teaserFileLink;

            arParams[26] = new MySqlParameter("?CompiledMeta", MySqlDbType.Text);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = compiledMeta;

            arParams[27] = new MySqlParameter("?ShippingAmount", MySqlDbType.Decimal);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = shippingAmount;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

           

         }

        public static bool Update(
            Guid guid,
            Guid taxClassGuid,
            string modelNumber,
            byte status,
            byte fullfillmentType,
            decimal weight,
            int quantityOnHand,
            string imageFileName,
            byte[] imageFileBytes,
            DateTime lastModified,
            Guid lastModifedBy,
            string url,
            string name,
            string description,
            string teaser,
            bool showInProductList,
            bool enableRating,
            string metaDescription,
            string metaKeywords,
            int sortRank1,
            int sortRank2,
            string teaserFile,
            string teaserFileLink,
            string compiledMeta,
            decimal shippingAmount)
        {

            int intShowInProductList;
            if (showInProductList)
            {
                intShowInProductList = 1;
            }
            else
            {
                intShowInProductList = 0;
            }

            int intEnableRating;
            if (enableRating)
            {
                intEnableRating = 1;
            }
            else
            {
                intEnableRating = 0;
            }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Product ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("TaxClassGuid = ?TaxClassGuid, ");
            sqlCommand.Append("ModelNumber = ?ModelNumber, ");
            sqlCommand.Append("Status = ?Status, ");
            sqlCommand.Append("FullfillmentType = ?FullfillmentType, ");
            sqlCommand.Append("Weight = ?Weight, ");
            sqlCommand.Append("QuantityOnHand = ?QuantityOnHand, ");
            sqlCommand.Append("ImageFileName = ?ImageFileName, ");
            sqlCommand.Append("ImageFileBytes = ?ImageFileBytes, ");
            sqlCommand.Append("Url = ?Url, ");
            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("Abstract = ?Abstract, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("MetaDescription = ?MetaDescription, ");
            sqlCommand.Append("MetaKeywords = ?MetaKeywords, ");
            sqlCommand.Append("CompiledMeta = ?CompiledMeta, ");
            sqlCommand.Append("ShowInProductList = ?ShowInProductList, ");
            sqlCommand.Append("EnableRating = ?EnableRating, ");
            sqlCommand.Append("SortRank1 = ?SortRank1, ");
            sqlCommand.Append("SortRank2 = ?SortRank2, ");
            sqlCommand.Append("TeaserFile = ?TeaserFile, ");
            sqlCommand.Append("TeaserFileLink = ?TeaserFileLink, ");

            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastModifedBy = ?LastModifedBy, ");
            sqlCommand.Append("ShippingAmount = ?ShippingAmount ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[25];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = taxClassGuid.ToString();

            arParams[2] = new MySqlParameter("?ModelNumber", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = modelNumber;

            arParams[3] = new MySqlParameter("?Status", MySqlDbType.Int16);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = status;

            arParams[4] = new MySqlParameter("?FullfillmentType", MySqlDbType.Int16);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = fullfillmentType;

            arParams[5] = new MySqlParameter("?Weight", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = weight;

            arParams[6] = new MySqlParameter("?QuantityOnHand", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = quantityOnHand;

            arParams[7] = new MySqlParameter("?ImageFileName", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = imageFileName;

            arParams[8] = new MySqlParameter("?ImageFileBytes", MySqlDbType.LongBlob);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = imageFileBytes;

            arParams[9] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            arParams[10] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModifedBy.ToString();

            arParams[11] = new MySqlParameter("?Url", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = url;

            arParams[12] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = name;

            arParams[13] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = description;

            arParams[14] = new MySqlParameter("?Abstract", MySqlDbType.Text);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = teaser;

            arParams[15] = new MySqlParameter("?ShowInProductList", MySqlDbType.Int32);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = intShowInProductList;

            arParams[16] = new MySqlParameter("?EnableRating", MySqlDbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intEnableRating;

            arParams[17] = new MySqlParameter("?MetaDescription", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = metaDescription;

            arParams[18] = new MySqlParameter("?MetaKeywords", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = metaKeywords;

            arParams[19] = new MySqlParameter("?SortRank1", MySqlDbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = sortRank1;

            arParams[20] = new MySqlParameter("?SortRank2", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = sortRank2;

            arParams[21] = new MySqlParameter("?TeaserFile", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = teaserFile;

            arParams[22] = new MySqlParameter("?TeaserFileLink", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = teaserFileLink;

            arParams[23] = new MySqlParameter("?CompiledMeta", MySqlDbType.Text);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = compiledMeta;

            arParams[24] = new MySqlParameter("?ShippingAmount", MySqlDbType.Decimal);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = shippingAmount;

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
            sqlCommand.Append("UPDATE ws_Product ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("IsDeleted = 1, ");
            sqlCommand.Append("DeletedBy = ?DeletedBy, ");
            sqlCommand.Append("DeletedFromIP = ?DeletedFromIP, ");
            sqlCommand.Append("DeletedTime = ?DeletedTime ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?DeletedBy", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = deletedBy.ToString();

            arParams[2] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = deletedFromIP;

            arParams[3] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = deletedTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
           

        }

        public static IDataReader Get(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
           
            sqlCommand.Append("FROM	ws_Product ");
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

        public static IDataReader GetAll(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            
            sqlCommand.Append("FROM	ws_Product ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND IsDeleted = false ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Name ");
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

        public static IDataReader GetForSiteMap(Guid siteGuid, Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("p.Guid, ");
            sqlCommand.Append("p.StoreGuid, ");
            sqlCommand.Append("p.Url, ");
            sqlCommand.Append("p.LastModified ");
           
            sqlCommand.Append("FROM	ws_Product p  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Store s ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("p.StoreGuid = s.Guid ");

            sqlCommand.Append("JOIN (");
            sqlCommand.Append("SELECT DISTINCT op.ProductGuid ");
            sqlCommand.Append("FROM ws_OfferProduct op ");
            sqlCommand.Append("JOIN ws_Offer o ");
            sqlCommand.Append("ON o.Guid = op.OfferGuid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("((o.StoreGuid = ?StoreGuid) OR (?StoreGuid = '00000000-0000-0000-0000-000000000000')) ");
            
            sqlCommand.Append("AND op.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsVisible = 1 ");

            sqlCommand.Append(") f ");
            sqlCommand.Append("ON f.ProductGuid = p.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ((p.StoreGuid = ?StoreGuid) OR (?StoreGuid = '00000000-0000-0000-0000-000000000000')) ");
            sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.SortRank1, p.SortRank2, p.Name ");

            
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the ws_Product table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Product p ");

            sqlCommand.Append("JOIN (");
            sqlCommand.Append("SELECT DISTINCT op.ProductGuid ");
            sqlCommand.Append("FROM ws_OfferProduct op ");
            sqlCommand.Append("JOIN ws_Offer o ");
            sqlCommand.Append("ON o.Guid = op.OfferGuid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.StoreGuid = ?StoreGuid ");

            sqlCommand.Append("AND op.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsVisible = 1 ");

            sqlCommand.Append(") f ");
            sqlCommand.Append("ON f.ProductGuid = p.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");
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
        /// Gets a count of rows in the ws_Product table.
        /// </summary>
        public static int GetCountForAdminList(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Product p ");

            //sqlCommand.Append("JOIN (");
            //sqlCommand.Append("SELECT DISTINCT op.ProductGuid ");
            //sqlCommand.Append("FROM ws_OfferProduct op ");
            //sqlCommand.Append("JOIN ws_Offer o ");
            //sqlCommand.Append("ON o.Guid = op.OfferGuid ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("o.StoreGuid = ?StoreGuid ");

            //sqlCommand.Append("AND op.IsDeleted = 0 ");
            //sqlCommand.Append("AND o.IsDeleted = 0 ");
            //sqlCommand.Append("AND o.IsVisible = 1 ");

            //sqlCommand.Append(") f ");
            //sqlCommand.Append("ON f.ProductGuid = p.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.StoreGuid = ?StoreGuid ");
            //sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");
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

        public static DataTable GetPageForAdminList(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountForAdminList(storeGuid);

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
            sqlCommand.Append("SELECT	p.* ");

            sqlCommand.Append("FROM	ws_Product p  ");


            //sqlCommand.Append("LEFT OUTER JOIN (");
            //sqlCommand.Append("SELECT DISTINCT op.ProductGuid ");
            //sqlCommand.Append("FROM ws_OfferProduct op ");
            //sqlCommand.Append("JOIN ws_Offer o ");
            //sqlCommand.Append("ON o.Guid = op.OfferGuid ");
            //sqlCommand.Append("WHERE ");
            //sqlCommand.Append("o.StoreGuid = ?StoreGuid ");

            //sqlCommand.Append("AND op.IsDeleted = 0 ");
            //sqlCommand.Append("AND o.IsDeleted = 0 ");
            //sqlCommand.Append("AND o.IsVisible = 1 ");

            //sqlCommand.Append(") f ");
            //sqlCommand.Append("ON f.ProductGuid = p.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.StoreGuid = ?StoreGuid ");
            //sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");



            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.Name ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            DataTable dataTable = GetProductEmptyTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    //row["StoreGuid"] = new Guid(reader["StoreGuid"].ToString());
                    //row["TaxClassGuid"] = new Guid(reader["TaxClassGuid"].ToString());
                    row["ModelNumber"] = reader["ModelNumber"];

                    // doh! actual field is mis-spelled as FullFillmentType
                    row["FulFillmentType"] = Convert.ToInt32(reader["FullFillmentType"]);

                    row["Weight"] = Convert.ToDecimal(reader["Weight"]);
                    //row["RetailPrice"] = Convert.ToDecimal(reader["RetailPrice"]);
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;



        }


        public static DataTable GetPage(
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
            sqlCommand.Append("SELECT	p.* ");
           
            sqlCommand.Append("FROM	ws_Product p  ");

            
            sqlCommand.Append("JOIN (");
            sqlCommand.Append("SELECT DISTINCT op.ProductGuid ");
            sqlCommand.Append("FROM ws_OfferProduct op ");
            sqlCommand.Append("JOIN ws_Offer o ");
            sqlCommand.Append("ON o.Guid = op.OfferGuid ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("o.StoreGuid = ?StoreGuid ");
            
            sqlCommand.Append("AND op.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsDeleted = 0 ");
            sqlCommand.Append("AND o.IsVisible = 1 ");

            sqlCommand.Append(") f ");
            sqlCommand.Append("ON f.ProductGuid = p.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND p.ShowInProductList = 1 ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.SortRank1, p.SortRank2, p.Name ");

            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            DataTable dataTable = GetProductEmptyTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    //row["StoreGuid"] = new Guid(reader["StoreGuid"].ToString());
                    //row["TaxClassGuid"] = new Guid(reader["TaxClassGuid"].ToString());
                    row["ModelNumber"] = reader["ModelNumber"];

                    // doh! actual field is mis-spelled as FullFillmentType
                    row["FulFillmentType"] = Convert.ToInt32(reader["FullFillmentType"]);

                    row["Weight"] = Convert.ToDecimal(reader["Weight"]);
                    //row["RetailPrice"] = Convert.ToDecimal(reader["RetailPrice"]);
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["EnableRating"] = Convert.ToBoolean(reader["EnableRating"]);
                    row["TeaserFile"] = reader["TeaserFile"];
                    row["TeaserFileLink"] = reader["TeaserFileLink"];

                    if (reader["ShippingAmount"] != DBNull.Value)
                    {
                        row["ShippingAmount"] = Convert.ToDecimal(reader["ShippingAmount"]);
                    }
                    else
                    {
                        row["ShippingAmount"] = 0;
                    }

                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;



        }

        private static DataTable GetProductEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));
            //dataTable.Columns.Add("StoreGuid", typeof(Guid));
            //dataTable.Columns.Add("TaxClassGuid", typeof(Guid));
            dataTable.Columns.Add("ModelNumber", typeof(string));
            // doh! actual field is mis-spelled as FullFillmentType
            dataTable.Columns.Add("FulFillmentType", typeof(int));
            dataTable.Columns.Add("Weight", typeof(decimal));
            //dataTable.Columns.Add("RetailPrice", typeof(decimal));
            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("EnableRating", typeof(bool));
            dataTable.Columns.Add("TeaserFile", typeof(string));
            dataTable.Columns.Add("TeaserFileLink", typeof(string));
            dataTable.Columns.Add("ShippingAmount", typeof(decimal));

            return dataTable;

        }

        private static DataTable GetOfferProductEmptyTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Guid", typeof(Guid));

            dataTable.Columns.Add("OfferGuid", typeof(Guid));
            dataTable.Columns.Add("ModelNumber", typeof(string));
            // doh! actual field is mis-spelled as FullFillmentType
            dataTable.Columns.Add("FulFillmentType", typeof(int));
            dataTable.Columns.Add("Weight", typeof(decimal));

            dataTable.Columns.Add("Url", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Abstract", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("EnableRating", typeof(bool));
            dataTable.Columns.Add("TeaserFile", typeof(string));
            dataTable.Columns.Add("TeaserFileLink", typeof(string));
            dataTable.Columns.Add("ShippingAmount", typeof(decimal));

            return dataTable;

        }


        public static DataTable GetListForPageOfOffers(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            //totalPages = 1;
            int totalRows = DBOffer.GetCount(storeGuid);

            //if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                //totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    //totalPages += 1;
                }
            }


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	p.*, ");
            sqlCommand.Append("op.OfferGuid ");

            sqlCommand.Append("FROM	ws_Product p  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_OfferProduct op ");
            sqlCommand.Append("ON p.Guid = op.ProductGuid ");

            sqlCommand.Append("JOIN ");

            sqlCommand.Append("(SELECT	Guid ");
            sqlCommand.Append("FROM	ws_Offer ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
            sqlCommand.Append("and IsDeleted = false ");
 
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString() + ", ?PageSize ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("SortRank1, SortRank2, Name  ");
            sqlCommand.Append(") o ");

            sqlCommand.Append("ON op.OfferGuid = o.Guid ");
            
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("p.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("AND p.IsDeleted = 0 ");

            //sqlCommand.Append("AND op.OfferGuid IN ");

            
            

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("p.SortRank1, p.SortRank2, p.Name ");

           
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = storeGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            DataTable dataTable = GetOfferProductEmptyTable();

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["OfferGuid"] = new Guid(reader["OfferGuid"].ToString());
                    row["ModelNumber"] = reader["ModelNumber"];

                    // doh! actual field is mis-spelled as FullFillmentType
                    row["FulFillmentType"] = Convert.ToInt32(reader["FullFillmentType"]);

                    row["Weight"] = Convert.ToDecimal(reader["Weight"]);
                    row["Url"] = reader["Url"];
                    row["Name"] = reader["Name"];
                    row["Abstract"] = reader["Abstract"];
                    row["Description"] = reader["Description"];
                    row["EnableRating"] = Convert.ToBoolean(reader["EnableRating"]);
                    row["TeaserFile"] = reader["TeaserFile"];
                    row["TeaserFileLink"] = reader["TeaserFileLink"];
                    if (reader["ShippingAmount"] != DBNull.Value)
                    {
                        row["ShippingAmount"] = Convert.ToDecimal(reader["ShippingAmount"]);
                    }
                    else
                    {
                        row["ShippingAmount"] = 0;
                    }
                    dataTable.Rows.Add(row);

                }
            }


            return dataTable;

        }



        public static IDataReader GetProductByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  pr.*, ");
            sqlCommand.Append("s.ModuleID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	ws_Product pr ");

            sqlCommand.Append("JOIN	ws_Store s ");
            sqlCommand.Append("ON s.Guid = pr.StoreGuid ");

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
            sqlCommand.Append("AND pr.IsDeleted = 0 ");
            sqlCommand.Append("AND pr.ShowInProductList = 1 ");

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

        public static int AddHistory(
            Guid guid,
            Guid productGuid,
            Guid storeGuid,
            Guid taxClassGuid,
            string sku,
            byte status,
            byte fullfillmentType,
            decimal weight,
            int quantityOnHand,
            string imageFileName,
            byte[] imageFileBytes,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid lastModifedBy,
            DateTime logTime,
            decimal shippingAmount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_ProductHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("Sku, ");
            sqlCommand.Append("Status, ");
            sqlCommand.Append("FullfillmentType, ");
            sqlCommand.Append("Weight, ");
            sqlCommand.Append("QuantityOnHand, ");
            sqlCommand.Append("ImageFileName, ");
            sqlCommand.Append("ImageFileBytes, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifedBy, ");
            sqlCommand.Append("LogTime, ");
            sqlCommand.Append("ShippingAmount )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?Sku, ");
            sqlCommand.Append("?Status, ");
            sqlCommand.Append("?FullfillmentType, ");
            sqlCommand.Append("?Weight, ");
            sqlCommand.Append("?QuantityOnHand, ");
            sqlCommand.Append("?ImageFileName, ");
            sqlCommand.Append("?ImageFileBytes, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifedBy, ");
            sqlCommand.Append("?LogTime, ");
            sqlCommand.Append("?ShippingAmount )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[17];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = productGuid.ToString();

            arParams[2] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = storeGuid.ToString();

            arParams[3] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid.ToString();

            arParams[4] = new MySqlParameter("?Sku", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = sku;

            arParams[5] = new MySqlParameter("?Status", MySqlDbType.Int16);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            arParams[6] = new MySqlParameter("?FullfillmentType", MySqlDbType.Int16);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = fullfillmentType;

            arParams[7] = new MySqlParameter("?Weight", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = weight;

            arParams[8] = new MySqlParameter("?QuantityOnHand", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = quantityOnHand;

            arParams[9] = new MySqlParameter("?ImageFileName", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = imageFileName;

            arParams[10] = new MySqlParameter("?ImageFileBytes", MySqlDbType.LongBlob);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = imageFileBytes;

            arParams[11] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = created;

            arParams[12] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = createdBy.ToString();

            arParams[13] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lastModified;

            arParams[14] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = lastModifedBy.ToString();

            arParams[15] = new MySqlParameter("?LogTime", MySqlDbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = logTime;

            arParams[16] = new MySqlParameter("?ShippingAmount", MySqlDbType.Decimal);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = shippingAmount;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


    }
}
