// Author:					Joe Audette
// Created:					2011-10-13
// Last Modified:			2012-05-29
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
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBTag
    {
        
        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["FirebirdWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["FirebirdWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }


        /// <summary>
        /// Inserts a row in the mp_Tag table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="tag"> tag </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid vocabularyGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            string tag,
            DateTime createdUtc,
            Guid createdBy)
        {

            FbParameter[] arParams = new FbParameter[10];


            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@VocabularyGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = vocabularyGuid.ToString();

            arParams[2] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new FbParameter("@FeatureGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid.ToString();

            arParams[4] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new FbParameter("@Tag", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = tag;

            arParams[6] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdUtc;

            arParams[7] = new FbParameter("@CreatedBy", FbDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdBy.ToString();

            arParams[8] = new FbParameter("@ModifiedUtc", FbDbType.TimeStamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdUtc;

            arParams[9] = new FbParameter("@ModifiedBy", FbDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Tag (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("VocabularyGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("Tag, ");
            sqlCommand.Append("ItemCount, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("ModifiedUtc, ");
            sqlCommand.Append("ModifiedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@VocabularyGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@Tag, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@ModifiedUtc, ");
            sqlCommand.Append("@ModifiedBy )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);


            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_Tag table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>  
        /// <param name="tag"> tag </param>
        /// <param name="modifiedUtc"> modifiedUtc </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid vocabularyGuid,
            string tag,
            DateTime modifiedUtc,
            Guid modifiedBy)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Tag ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("VocabularyGuid = @VocabularyGuid, ");
            sqlCommand.Append("Tag = @Tag, ");
            sqlCommand.Append("ModifiedUtc = @ModifiedUtc, ");
            sqlCommand.Append("ModifiedBy = @ModifiedBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@VocabularyGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = vocabularyGuid.ToString();

            arParams[2] = new FbParameter("@Tag", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = tag;

            arParams[3] = new FbParameter("@ModifiedUtc", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = modifiedUtc;

            arParams[4] = new FbParameter("@ModifiedBy", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = modifiedBy.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool UpdateItemCount(Guid guid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Tag ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ItemCount = ( ");
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("mp_TagItem ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("TagGuid = @Guid) ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes a row from the mp_Tag table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FeatureGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Tag table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("Tag ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_Tag table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_Tag ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null));

        }


        





        ///// <summary>
        ///// Gets an IDataReader with all rows in the mp_Tag table.
        ///// </summary>
        //public static IDataReader GetAll()
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  * ");
        //    sqlCommand.Append("FROM	mp_Tag ");
        //    sqlCommand.Append(";");

        //    return FBSqlHelper.ExecuteReader(
        //        GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //}


        ///// <summary>
        ///// Gets a page of data from the mp_Tag table.
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
        //    sqlCommand.Append("SELECT FIRST " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
        //    if (pageNumber > 1)
        //    {
        //        sqlCommand.Append("	SKIP " + pageLowerBound.ToString(CultureInfo.InvariantCulture) + " ");
        //    }
        //    sqlCommand.Append("	* ");
        //    sqlCommand.Append("FROM	mp_Tag  ");
        //    //sqlCommand.Append("WHERE   ");
        //    //sqlCommand.Append("ORDER BY  ");
        //    sqlCommand.Append("	; ");

        //    //FbParameter[] arParams = new FbParameter[1];

        //    //arParams[0] = new FbParameter("@CountryGuid", FbDbType.Char, 36);
        //    //arParams[0].Direction = ParameterDirection.Input;
        //    //arParams[0].Value = countryGuid.ToString();

        //    return FBSqlHelper.ExecuteReader(
        //        GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        null);

        //}
    }
}
