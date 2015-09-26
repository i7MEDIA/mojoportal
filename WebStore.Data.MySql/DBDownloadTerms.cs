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
    public static class DBDownloadTerms
    {

        public static int Add(
            Guid guid,
            Guid storeGuid,
            int downloadsAllowed,
            int expireAfterDays,
            bool countAfterDownload,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIPAddress,
            string name,
            string description)
        {
            #region Bit Conversion

            int intCountAfterDownload;
            if (countAfterDownload)
            {
                intCountAfterDownload = 1;
            }
            else
            {
                intCountAfterDownload = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_FullfillDownloadTerms (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("StoreGuid, ");
            sqlCommand.Append("DownloadsAllowed, ");
            sqlCommand.Append("ExpireAfterDays, ");
            sqlCommand.Append("CountAfterDownload, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifedBy, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("LastModifedFromIPAddress )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?StoreGuid, ");
            sqlCommand.Append("?DownloadsAllowed, ");
            sqlCommand.Append("?ExpireAfterDays, ");
            sqlCommand.Append("?CountAfterDownload, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifedBy, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?LastModifedFromIPAddress )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?StoreGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = storeGuid.ToString();

            arParams[2] = new MySqlParameter("?DownloadsAllowed", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = downloadsAllowed;

            arParams[3] = new MySqlParameter("?ExpireAfterDays", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = expireAfterDays;

            arParams[4] = new MySqlParameter("?CountAfterDownload", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intCountAfterDownload;

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

            arParams[9] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModifedBy.ToString();

            arParams[10] = new MySqlParameter("?LastModifedFromIPAddress", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModifedFromIPAddress;

            arParams[11] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = name;

            arParams[12] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = description;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            int downloadsAllowed,
            int expireAfterDays,
            bool countAfterDownload,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIPAddress,
            string name,
            string description)
        {

            #region Bit Conversion

            int intCountAfterDownload;
            if (countAfterDownload)
            {
                intCountAfterDownload = 1;
            }
            else
            {
                intCountAfterDownload = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_FullfillDownloadTerms ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("DownloadsAllowed = ?DownloadsAllowed, ");
            sqlCommand.Append("ExpireAfterDays = ?ExpireAfterDays, ");
            sqlCommand.Append("CountAfterDownload = ?CountAfterDownload, ");
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastModifedBy = ?LastModifedBy, ");
            sqlCommand.Append("LastModifedFromIPAddress = ?LastModifedFromIPAddress ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?DownloadsAllowed", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = downloadsAllowed;

            arParams[2] = new MySqlParameter("?ExpireAfterDays", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = expireAfterDays;

            arParams[3] = new MySqlParameter("?CountAfterDownload", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intCountAfterDownload;

            arParams[4] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastModified;

            arParams[5] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModifedBy.ToString();

            arParams[6] = new MySqlParameter("?LastModifedFromIPAddress", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModifedFromIPAddress;

            arParams[7] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = name;

            arParams[8] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = description;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTerms ");
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

        public static IDataReader Get(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_FullfillDownloadTerms  ");
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
            sqlCommand.Append("FROM	ws_FullfillDownloadTerms ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("StoreGuid = ?StoreGuid ");
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

        /// <summary>
        /// Gets a count of rows in the ws_FullfillDownloadTerms table.
        /// </summary>
        public static int GetCount(Guid storeGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_FullfillDownloadTerms ");
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


        public static IDataReader GetFullfillDownloadTermsPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            int totalPages = 1;
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
            sqlCommand.Append("SELECT  dt.*, ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages ");
            sqlCommand.Append("FROM	ws_FullfillDownloadTerms dt ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("dt.StoreGuid = ?StoreGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("dt.Name ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + ", ?PageSize ");
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

        


    }
}
