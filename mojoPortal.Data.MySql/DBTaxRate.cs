///							DBTaxRate.cs
/// Author:					
/// Created:				2008-06-25
/// Last Modified:			2012-07-20
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
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
using System.IO;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{
    public static class DBTaxRate
    {
        /// <summary>
        /// Inserts a row in the mp_TaxRate table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="priority"> priority </param>
        /// <param name="rate"> rate </param>
        /// <param name="description"> description </param>
        /// <param name="created"> created </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid modifiedBy)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaxRate (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("GeoZoneGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("Priority, ");
            sqlCommand.Append("Rate, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("ModifiedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?GeoZoneGuid, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?Priority, ");
            sqlCommand.Append("?Rate, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?ModifiedBy )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[11];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?GeoZoneGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = geoZoneGuid.ToString();

            arParams[3] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid.ToString();

            arParams[4] = new MySqlParameter("?Priority", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = priority;

            arParams[5] = new MySqlParameter("?Rate", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rate;

            arParams[6] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = created;

            arParams[8] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdBy.ToString();

            arParams[9] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            arParams[10] = new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = modifiedBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_TaxRate table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="priority"> priority </param>
        /// <param name="rate"> rate </param>
        /// <param name="description"> description </param>
        /// <param name="created"> created </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime lastModified,
            Guid modifiedBy)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaxRate ");
            sqlCommand.Append("SET  ");
  
            sqlCommand.Append("GeoZoneGuid = ?GeoZoneGuid, ");
            sqlCommand.Append("TaxClassGuid = ?TaxClassGuid, ");
            sqlCommand.Append("Priority = ?Priority, ");
            sqlCommand.Append("Rate = ?Rate, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("ModifiedBy = ?ModifiedBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?GeoZoneGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = geoZoneGuid.ToString();

            arParams[2] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = taxClassGuid.ToString();

            arParams[3] = new MySqlParameter("?Priority", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = priority;

            arParams[4] = new MySqlParameter("?Rate", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rate;

            arParams[5] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = description;

            arParams[6] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModified;

            arParams[7] = new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = modifiedBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_TaxRate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaxRate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaxRate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaxRate ");
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
        /// Gets an IDataReader with one row from the mp_TaxRate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetTaxRates(
            Guid siteGuid,
            Guid geoZoneGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaxRate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND GeoZoneGuid = ?GeoZoneGuid ");
            sqlCommand.Append("ORDER BY Priority ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?GeoZoneGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = geoZoneGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Inserts a row in the mp_TaxRateHistory table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="taxRateGuid"> taxRateGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="priority"> priority </param>
        /// <param name="rate"> rate </param>
        /// <param name="description"> description </param>
        /// <param name="created"> created </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <param name="logTime"> logTime </param>
        /// <returns>int</returns>
        public static int AddHistory(
            Guid guid,
            Guid taxRateGuid,
            Guid siteGuid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid modifiedBy,
            DateTime logTime)
        {

            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaxRateHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("TaxRateGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("GeoZoneGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("Priority, ");
            sqlCommand.Append("Rate, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("ModifiedBy, ");
            sqlCommand.Append("LogTime )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?TaxRateGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?GeoZoneGuid, ");
            sqlCommand.Append("?TaxClassGuid, ");
            sqlCommand.Append("?Priority, ");
            sqlCommand.Append("?Rate, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?ModifiedBy, ");
            sqlCommand.Append("?LogTime )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?TaxRateGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = taxRateGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?GeoZoneGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = geoZoneGuid.ToString();

            arParams[4] = new MySqlParameter("?TaxClassGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = taxClassGuid.ToString();

            arParams[5] = new MySqlParameter("?Priority", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = priority;

            arParams[6] = new MySqlParameter("?Rate", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = rate;

            arParams[7] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = description;

            arParams[8] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = created;

            arParams[9] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy.ToString();

            arParams[10] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModified;

            arParams[11] = new MySqlParameter("?ModifiedBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = modifiedBy.ToString();

            arParams[12] = new MySqlParameter("?LogTime", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = logTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        ///// <summary>
        ///// Gets a count of rows in the mp_TaxRate table.
        ///// </summary>
        //public static int GetCount()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  Count(*) ");
        //    sqlCommand.Append("FROM	mp_TaxRate ");
        //    sqlCommand.Append(";");

        //    return Convert.ToInt32(MySqlHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        null));
        //}

        ///// <summary>
        ///// Gets a page of data from the mp_TaxRate table.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static IDataReader GetPage(
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    int pageLowerBound = (pageSize * pageNumber) - pageSize;
        //    totalPages = 1;
        //    int totalRows = GetCount();

        //    if (pageSize > 0) totalPages = totalRows / pageSize;

        //    if (totalRows <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else
        //    {
        //        int remainder;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT	* ");
        //    sqlCommand.Append("FROM	mp_TaxRate  ");
        //    //sqlCommand.Append("WHERE  ");
        //    //sqlCommand.Append("ORDER BY  ");
        //    //sqlCommand.Append("  ");
        //    sqlCommand.Append("LIMIT ?PageSize ");

        //    if (pageNumber > 1)
        //    {
        //        sqlCommand.Append("OFFSET ?OffsetRows ");
        //    }

        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[2];

        //    arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = pageSize;

        //    arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = pageLowerBound;

        //    return MySqlHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);
        //}

    }
}
