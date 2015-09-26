
// Author:					Joe Audette
// Created:					2009-03-03
// Last Modified:			2009-03-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

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
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Discount_Insert", 19);
            sph.DefineSqlParameter("@DiscountGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, discountGuid);
            sph.DefineSqlParameter("@DiscountCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, discountCode);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@StoreGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, storeGuid);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ValidityStartDate", SqlDbType.DateTime, ParameterDirection.Input, validityStartDate);
            if (validityEndDate == DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@ValidityEndDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@ValidityEndDate", SqlDbType.DateTime, ParameterDirection.Input, validityEndDate);
            }

            sph.DefineSqlParameter("@UseCount", SqlDbType.Int, ParameterDirection.Input, useCount);
            sph.DefineSqlParameter("@MaxCount", SqlDbType.Int, ParameterDirection.Input, maxCount);
            sph.DefineSqlParameter("@MinOrderAmount", SqlDbType.Decimal, ParameterDirection.Input, minOrderAmount);
            sph.DefineSqlParameter("@AbsoluteDiscount", SqlDbType.Decimal, ParameterDirection.Input, absoluteDiscount);
            sph.DefineSqlParameter("@PercentageDiscount", SqlDbType.Decimal, ParameterDirection.Input, percentageDiscount);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@AllowOtherDiscounts", SqlDbType.Bit, ParameterDirection.Input, allowOtherDiscounts);
            int rowsAffected = sph.ExecuteNonQuery();
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


            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Discount_Update", 14);
            sph.DefineSqlParameter("@DiscountGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, discountGuid);
            sph.DefineSqlParameter("@DiscountCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, discountCode);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            sph.DefineSqlParameter("@ValidityStartDate", SqlDbType.DateTime, ParameterDirection.Input, validityStartDate);
            if (validityEndDate == DateTime.MaxValue)
            {
                sph.DefineSqlParameter("@ValidityEndDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            }
            else
            {
                sph.DefineSqlParameter("@ValidityEndDate", SqlDbType.DateTime, ParameterDirection.Input, validityEndDate);
            }
            sph.DefineSqlParameter("@UseCount", SqlDbType.Int, ParameterDirection.Input, useCount);
            sph.DefineSqlParameter("@MaxCount", SqlDbType.Int, ParameterDirection.Input, maxCount);
            sph.DefineSqlParameter("@MinOrderAmount", SqlDbType.Decimal, ParameterDirection.Input, minOrderAmount);
            sph.DefineSqlParameter("@AbsoluteDiscount", SqlDbType.Decimal, ParameterDirection.Input, absoluteDiscount);
            sph.DefineSqlParameter("@PercentageDiscount", SqlDbType.Decimal, ParameterDirection.Input, percentageDiscount);
            sph.DefineSqlParameter("@LastModBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModBy);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@AllowOtherDiscounts", SqlDbType.Bit, ParameterDirection.Input, allowOtherDiscounts);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid discountGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Discount_Delete", 1);
            sph.DefineSqlParameter("@DiscountGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, discountGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Discount_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Discount_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the ws_Discount table. Returns true if row deleted.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOffer(Guid offerGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_Discount_DeleteByOffer", 1);
            sph.DefineSqlParameter("@OfferGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, offerGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Discount table.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        public static IDataReader GetOne(Guid discountGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Discount_SelectOne", 1);
            sph.DefineSqlParameter("@DiscountGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, discountGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Discount table.
        /// </summary>
        /// <param name="discountCode"> discountCode </param>
        public static IDataReader GetOne(Guid moduleGuid, string discountCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Discount_SelectByDiscountCode", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@DiscountCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, discountCode);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets an IDataReader with one row from the ws_Discount table.
        /// </summary>
        /// <param name="discountCode"> discountCode </param>
        public static IDataReader Find(Guid moduleGuid, string descriptionOrCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Discount_Find", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, 255, ParameterDirection.Input, descriptionOrCode);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the ws_Discount table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Discount_GetCount", 1);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            return Convert.ToInt32(sph.ExecuteScalar());

           

        }

        /// <summary>
        /// Gets a count of rows in the ws_Discount table.
        /// </summary>
        public static int GetCountOfActiveDiscountCodes(Guid moduleGuid, DateTime activeForDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Discount_GetCountOfActive", 2);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@CurrentDate", SqlDbType.DateTime, ParameterDirection.Input, activeForDate);
            return Convert.ToInt32(sph.ExecuteScalar());



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
            totalPages = 1;
            int totalRows
                = GetCount(moduleGuid);

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

            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_Discount_SelectPage", 3);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}
