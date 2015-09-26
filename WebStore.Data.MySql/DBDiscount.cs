// Author:					Joe Audette
// Created:					2009-03-03
// Last Modified:			2012-07-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using mojoPortal.Data;


namespace WebStore.Data
{

    public static class DBDiscount
    {
        

        /// <summary>
        /// Inserts a row in the ws_Discount table. Returns rows affected count.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <param name="discountCode"> discountCode </param>
        /// <param name="description"> description </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="storeGuid"> storeGuid </param>
        /// <param name="offerGuid"> offerGuid </param>
        /// <param name="validityStartDate"> validityStartDate </param>
        /// <param name="validityEndDate"> validityEndDate </param>
        /// <param name="useCount"> useCount </param>
        /// <param name="maxCount"> maxCount </param>
        /// <param name="minOrderAmount"> minOrderAmount </param>
        /// <param name="absoluteDiscount"> absoluteDiscount </param>
        /// <param name="percentageDiscount"> percentageDiscount </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid discountGuid,
            string discountCode,
            string description,
            Guid siteGuid,
            Guid moduleGuid,
            Guid storeGuid,
            Guid offerGuid,
            DateTime validityStartDate,
            DateTime validityEndDate,
            int useCount,
            int maxCount,
            decimal minOrderAmount,
            decimal absoluteDiscount,
            decimal percentageDiscount,
            bool allowOtherDiscounts,
            Guid createdBy,
            DateTime createdUtc)
        {
            int intallowOtherDiscounts = 0;
            if (allowOtherDiscounts) { intallowOtherDiscounts = 1; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_Discount (");
            sqlCommand.Append("DiscountGuid, ");
            sqlCommand.Append("DiscountCode, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("ValidityStartDate, ");
            sqlCommand.Append("ValidityEndDate, ");
            sqlCommand.Append("AllowOtherDiscounts, ");
            sqlCommand.Append("UseCount, ");
            sqlCommand.Append("MaxCount, ");
            sqlCommand.Append("MinOrderAmount, ");
            sqlCommand.Append("AbsoluteDiscount, ");
            sqlCommand.Append("PercentageDiscount, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("LastModUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?DiscountGuid, ");
            sqlCommand.Append("?DiscountCode, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?ValidityStartDate, ");
            if (validityEndDate == DateTime.MaxValue)
            {
                sqlCommand.Append("NULL, ");
            }
            else
            {
                sqlCommand.Append("?ValidityEndDate, ");
            }
            sqlCommand.Append("?AllowOtherDiscounts, ");
            sqlCommand.Append("?UseCount, ");
            sqlCommand.Append("?MaxCount, ");
            sqlCommand.Append("?MinOrderAmount, ");
            sqlCommand.Append("?AbsoluteDiscount, ");
            sqlCommand.Append("?PercentageDiscount, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModBy, ");
            sqlCommand.Append("?LastModUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[19];

            arParams[0] = new MySqlParameter("?DiscountGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = discountGuid.ToString();

            arParams[1] = new MySqlParameter("?DiscountCode", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = discountCode;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = siteGuid.ToString();

            arParams[4] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = storeGuid.ToString();

            arParams[6] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = offerGuid.ToString();

            arParams[7] = new MySqlParameter("?ValidityStartDate", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = validityStartDate;

            arParams[8] = new MySqlParameter("?ValidityEndDate", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = validityEndDate;

            arParams[9] = new MySqlParameter("?UseCount", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = useCount;

            arParams[10] = new MySqlParameter("?MaxCount", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = maxCount;

            arParams[11] = new MySqlParameter("?MinOrderAmount", MySqlDbType.Decimal);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = minOrderAmount;

            arParams[12] = new MySqlParameter("?AbsoluteDiscount", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = absoluteDiscount;

            arParams[13] = new MySqlParameter("?PercentageDiscount", MySqlDbType.Decimal);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = percentageDiscount;

            arParams[14] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdBy.ToString();

            arParams[15] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = createdUtc;

            arParams[16] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdBy.ToString();

            arParams[17] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = createdUtc;

            arParams[18] = new MySqlParameter("?AllowOtherDiscounts", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intallowOtherDiscounts;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the ws_Discount table. Returns true if row updated.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <param name="discountCode"> discountCode </param>
        /// <param name="description"> description </param>
        /// <param name="offerGuid"> offerGuid </param>
        /// <param name="validityStartDate"> validityStartDate </param>
        /// <param name="validityEndDate"> validityEndDate </param>
        /// <param name="useCount"> useCount </param>
        /// <param name="maxCount"> maxCount </param>
        /// <param name="minOrderAmount"> minOrderAmount </param>
        /// <param name="absoluteDiscount"> absoluteDiscount </param>
        /// <param name="percentageDiscount"> percentageDiscount </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid discountGuid,
            string discountCode,
            string description,
            Guid offerGuid,
            DateTime validityStartDate,
            DateTime validityEndDate,
            int useCount,
            int maxCount,
            decimal minOrderAmount,
            decimal absoluteDiscount,
            decimal percentageDiscount,
            bool allowOtherDiscounts,
            Guid lastModBy,
            DateTime lastModUtc)
        {
            int intallowOtherDiscounts = 0;
            if (allowOtherDiscounts) { intallowOtherDiscounts = 1; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Discount ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("DiscountCode = ?DiscountCode, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("OfferGuid = ?OfferGuid, ");
            sqlCommand.Append("ValidityStartDate = ?ValidityStartDate, ");
            if (validityEndDate == DateTime.MaxValue)
            {
                sqlCommand.Append("ValidityEndDate = NULL, ");
            }
            else
            {
                sqlCommand.Append("ValidityEndDate = ?ValidityEndDate, ");
            }
            sqlCommand.Append("AllowOtherDiscounts = ?AllowOtherDiscounts, ");
            sqlCommand.Append("UseCount = ?UseCount, ");
            sqlCommand.Append("MaxCount = ?MaxCount, ");
            sqlCommand.Append("MinOrderAmount = ?MinOrderAmount, ");
            sqlCommand.Append("AbsoluteDiscount = ?AbsoluteDiscount, ");
            sqlCommand.Append("PercentageDiscount = ?PercentageDiscount, ");
            sqlCommand.Append("LastModBy = ?LastModBy, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("DiscountGuid = ?DiscountGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[14];

            arParams[0] = new MySqlParameter("?DiscountGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = discountGuid.ToString();

            arParams[1] = new MySqlParameter("?DiscountCode", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = discountCode;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = offerGuid.ToString();

            arParams[4] = new MySqlParameter("?ValidityStartDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = validityStartDate;

            arParams[5] = new MySqlParameter("?ValidityEndDate", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = validityEndDate;

            arParams[6] = new MySqlParameter("?UseCount", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = useCount;

            arParams[7] = new MySqlParameter("?MaxCount", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = maxCount;

            arParams[8] = new MySqlParameter("?MinOrderAmount", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = minOrderAmount;

            arParams[9] = new MySqlParameter("?AbsoluteDiscount", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = absoluteDiscount;

            arParams[10] = new MySqlParameter("?PercentageDiscount", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = percentageDiscount;

            arParams[11] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = lastModBy.ToString();

            arParams[12] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lastModUtc;

            arParams[13] = new MySqlParameter("?AllowOtherDiscounts", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intallowOtherDiscounts;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid discountGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("DiscountGuid = ?DiscountGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?DiscountGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = discountGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOffer(Guid offerGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OfferGuid = ?OfferGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = offerGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Discount table.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        public static IDataReader GetOne(Guid discountGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("DiscountGuid = ?DiscountGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?DiscountGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = discountGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Discount table.
        /// </summary>
        /// <param name="discountCode"> discountCode </param>
        public static IDataReader GetOne(Guid moduleGuid, string discountCode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("DiscountCode = ?DiscountCode ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?DiscountCode", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = discountCode;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Discount table.
        /// </summary>
        /// <param name="discountCode"> discountCode </param>
        public static IDataReader Find(Guid moduleGuid, string descriptionOrCode)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND (");
            sqlCommand.Append("(DiscountCode LIKE ?DescriptionOrCode) ");
            sqlCommand.Append("OR (Description LIKE ?DescriptionOrCode) ");
            sqlCommand.Append(") ");
            sqlCommand.Append("ORDER BY Description ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?DescriptionOrCode", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = descriptionOrCode;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the ws_Discount table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a count of rows in the ws_Discount table.
        /// </summary>
        public static int GetCountOfActiveDiscountCodes(Guid moduleGuid, DateTime activeForDate)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_Discount ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND (ValidityStartDate <= ?ActiveForDate) ");
            sqlCommand.Append("AND ((ValidityEndDate IS NULL) OR (ValidityEndDate >= ?ActiveForDate)) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?ActiveForDate", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = activeForDate;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        /// <summary>
        /// Gets a page of data from the ws_Discount table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleGuid);

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
            sqlCommand.Append("FROM	ws_Discount  ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("Description  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
