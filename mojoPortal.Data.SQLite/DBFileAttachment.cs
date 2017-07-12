
// Author:					
// Created:					2009-03-08
// Last Modified:			2012-09-19
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
using System.Web;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
    public static class DBFileAttachment
    {
        
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }


        /// <summary>
        /// Inserts a row in the mp_FileAttachment table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="serverFileName"> serverFileName </param>
        /// <param name="fileName"> fileName </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid itemGuid,
            Guid specialGuid1,
            Guid specialGuid2,
            string serverFileName,
            string fileName,
            string contentTitle,
            long contentLength,
            string contentType,
            DateTime createdUtc,
            Guid createdBy)
        {
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_FileAttachment (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("SpecialGuid1, ");
            sqlCommand.Append("SpecialGuid2, ");
            sqlCommand.Append("ServerFileName, ");
            sqlCommand.Append("FileName, ");
            sqlCommand.Append("ContentTitle, ");
            sqlCommand.Append("ContentLength, ");
            sqlCommand.Append("ContentType, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":ItemGuid, ");
            sqlCommand.Append(":SpecialGuid1, ");
            sqlCommand.Append(":SpecialGuid2, ");
            sqlCommand.Append(":ServerFileName, ");
            sqlCommand.Append(":FileName, ");
            sqlCommand.Append(":ContentTitle, ");
            sqlCommand.Append(":ContentLength, ");
            sqlCommand.Append(":ContentType, ");
            sqlCommand.Append(":CreatedUtc, ");
            sqlCommand.Append(":CreatedBy )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[13];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = itemGuid.ToString();

            arParams[4] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid1.ToString();

            arParams[5] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = specialGuid2.ToString();

            arParams[6] = new SqliteParameter(":ServerFileName", DbType.String, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = serverFileName;

            arParams[7] = new SqliteParameter(":FileName", DbType.String, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = fileName;

            arParams[8] = new SqliteParameter(":CreatedUtc", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

            arParams[9] = new SqliteParameter(":CreatedBy", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy.ToString();

            arParams[10] = new SqliteParameter(":ContentLength", DbType.Int64);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = contentLength;

            arParams[11] = new SqliteParameter(":ContentType", DbType.String, 50);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = contentType;

            arParams[12] = new SqliteParameter(":ContentTitle", DbType.String, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = contentTitle;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_FileAttachment table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="serverFileName"> serverFileName </param>
        /// <param name="fileName"> fileName </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            string serverFileName,
            string fileName,
            string contentTitle,
            long contentLength,
            string contentType)
        {
           
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_FileAttachment ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("ServerFileName = :ServerFileName, ");
            sqlCommand.Append("FileName = :FileName, ");
            sqlCommand.Append("ContentTitle = :ContentTitle, ");
            sqlCommand.Append("ContentLength = :ContentLength, ");
            sqlCommand.Append("ContentType = :ContentType ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":ServerFileName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = serverFileName;

            arParams[2] = new SqliteParameter(":FileName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = fileName;

            arParams[3] = new SqliteParameter(":ContentLength", DbType.Int64);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = contentLength;

            arParams[4] = new SqliteParameter(":ContentType", DbType.String, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = contentType;

            arParams[5] = new SqliteParameter(":ContentTitle", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = contentTitle;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the mp_FileAttachment table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = :ItemGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_FileAttachment table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(Guid rowGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowGuid = :RowGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with row sfrom the mp_FileAttachment table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader SelectByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = :ModuleGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_FileAttachment table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader SelectByItem(Guid itemGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemGuid = :ItemGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_FileAttachment table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader SelectBySpecial1(Guid specialGuid1)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SpecialGuid1 = :SpecialGuid1 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SpecialGuid1", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = specialGuid1.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_FileAttachment table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader SelectBySpecial2(Guid specialGuid2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_FileAttachment ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SpecialGuid2 = :SpecialGuid2 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SpecialGuid2", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = specialGuid2.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }



    }
}
