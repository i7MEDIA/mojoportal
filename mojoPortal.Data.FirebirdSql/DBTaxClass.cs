
namespace mojoPortal.Data
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using FirebirdSql.Data.FirebirdClient;

    /// <summary>
    ///							DBTaxClass.cs
    /// Author:					Joe Audette
    /// Created:				2008-06-25
    /// Last Modified:			2008-06-25
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public static class DBTaxClass
    {
        
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }


        /// <summary>
        /// Inserts a row in the mp_TaxClass table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            string title,
            string description,
            DateTime lastModified,
            DateTime created)
        {

            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@LastModified", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastModified;

            arParams[5] = new FbParameter("@Created", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = created;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaxClass (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("Created )");


            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@LastModified, ");
            sqlCommand.Append("@Created )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_TaxClass table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="created"> created </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string title,
            string description,
            DateTime lastModified,
            DateTime created)
        {
        
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaxClass ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("LastModified = @LastModified, ");
            sqlCommand.Append("Created = @Created ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new FbParameter("@LastModified", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastModified;

            arParams[4] = new FbParameter("@Created", FbDbType.TimeStamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = created;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes a row from the mp_TaxClass table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaxClass table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_TaxClass table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY Title ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_TaxClass table.
        /// </summary>
        public static int GetCountBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_TaxClass ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        

        /// <summary>
        /// Gets a page of data from the mp_TaxClass table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid siteGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountBySite(siteGuid);

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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append("	* ");
            sqlCommand.Append("FROM	mp_TaxClass  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY Title ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }
    }
}
