// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2012-08-30
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
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBEmailTemplate
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        /// <summary>
        /// Inserts a row in the mp_EmailTemplate table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="specialGuid1"> specialGuid1 </param>
        /// <param name="specialGuid2"> specialGuid2 </param>
        /// <param name="name"> name </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="hasHtml"> hasHtml </param>
        /// <param name="isEditable"> isEditable </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid specialGuid1,
            Guid specialGuid2,
            string name,
            string subject,
            string textBody,
            string htmlBody,
            bool hasHtml,
            bool isEditable,
            DateTime createdUtc,
            Guid lastModBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_EmailTemplate ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SpecialGuid1, ");
            sqlCommand.Append("SpecialGuid2, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Subject, ");
            sqlCommand.Append("TextBody, ");
            sqlCommand.Append("HtmlBody, ");
            sqlCommand.Append("HasHtml, ");
            sqlCommand.Append("IsEditable, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FeatureGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@SpecialGuid1, ");
            sqlCommand.Append("@SpecialGuid2, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@Subject, ");
            sqlCommand.Append("@TextBody, ");
            sqlCommand.Append("@HtmlBody, ");
            sqlCommand.Append("@HasHtml, ");
            sqlCommand.Append("@IsEditable, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@LastModBy ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[15];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = featureGuid;

            arParams[3] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid;

            arParams[4] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid1;

            arParams[5] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = specialGuid2;

            arParams[6] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = name;

            arParams[7] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = subject;

            arParams[8] = new SqlCeParameter("@TextBody", SqlDbType.NText);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = textBody;

            arParams[9] = new SqlCeParameter("@HtmlBody", SqlDbType.NText);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = htmlBody;

            arParams[10] = new SqlCeParameter("@HasHtml", SqlDbType.Bit);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = hasHtml;

            arParams[11] = new SqlCeParameter("@IsEditable", SqlDbType.Bit);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = isEditable;

            arParams[12] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = createdUtc;

            arParams[13] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = lastModBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the mp_EmailTemplate table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="name"> name </param>
        /// <param name="subject"> subject </param>
        /// <param name="textBody"> textBody </param>
        /// <param name="htmlBody"> htmlBody </param>
        /// <param name="hasHtml"> hasHtml </param>
        /// <param name="isEditable"> isEditable </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            string name,
            string subject,
            string textBody,
            string htmlBody,
            bool hasHtml,
            bool isEditable,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_EmailTemplate ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Name = @Name, ");
            sqlCommand.Append("Subject = @Subject, ");
            sqlCommand.Append("TextBody = @TextBody, ");
            sqlCommand.Append("HtmlBody = @HtmlBody, ");
            sqlCommand.Append("HasHtml = @HasHtml, ");
            sqlCommand.Append("IsEditable = @IsEditable, ");
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("LastModBy = @LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[9];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new SqlCeParameter("@Subject", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subject;

            arParams[3] = new SqlCeParameter("@TextBody", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = textBody;

            arParams[4] = new SqlCeParameter("@HtmlBody", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = htmlBody;

            arParams[5] = new SqlCeParameter("@HasHtml", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = hasHtml;

            arParams[6] = new SqlCeParameter("@IsEditable", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isEditable;

            arParams[7] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModUtc;

            arParams[8] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_EmailTemplate table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySpecial1(Guid specialGuid1)
        {
            if (specialGuid1 == Guid.Empty) { return false; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SpecialGuid1 = @SpecialGuid1 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = specialGuid1;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool DeleteBySpecial2(Guid specialGuid2)
        {
            if (specialGuid2 == Guid.Empty) { return false; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SpecialGuid2 = @SpecialGuid2 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = specialGuid2;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_EmailTemplate table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader Get(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND (@FeatureGuid = '00000000-0000-0000-0000-000000000000' OR FeatureGuid = @FeatureGuid) ");
            sqlCommand.Append("AND (@ModuleGuid = '00000000-0000-0000-0000-000000000000' OR ModuleGuid = @ModuleGuid) ");
            sqlCommand.Append("AND (@SpecialGuid1 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid1 = @SpecialGuid1) ");
            sqlCommand.Append("AND (@SpecialGuid2 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid2 = @SpecialGuid2) ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = specialGuid1;

            arParams[4] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid2;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByModule(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SpecialGuid1 = @SpecialGuid1 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SpecialGuid2 = @SpecialGuid2 ");


            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = specialGuid1;

            arParams[2] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = specialGuid2;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByFeature(Guid siteGuid, Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
           
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static int GetCount(Guid siteGuid, Guid featureGuid, Guid moduleGuid, Guid specialGuid1, Guid specialGuid2)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND (@FeatureGuid = '00000000-0000-0000-0000-000000000000' OR FeatureGuid = @FeatureGuid) ");
            sqlCommand.Append("AND (@ModuleGuid = '00000000-0000-0000-0000-000000000000' OR ModuleGuid = @ModuleGuid) ");
            sqlCommand.Append("AND (@SpecialGuid1 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid1 = @SpecialGuid1) ");
            sqlCommand.Append("AND (@SpecialGuid2 = '00000000-0000-0000-0000-000000000000' OR SpecialGuid2 = @SpecialGuid2) ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            arParams[2] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid;

            arParams[3] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = specialGuid1;

            arParams[4] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = specialGuid2;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountByModuleAndName(Guid moduleGuid, string name)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("[Name] = @Name ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountByModuleSpecialAndName(Guid moduleGuid, Guid specialGuid1, Guid specialGuid2, string name)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = @ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SpecialGuid1 = @SpecialGuid1 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SpecialGuid2 = @SpecialGuid2 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("[Name] = @Name ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid;

            arParams[1] = new SqlCeParameter("@SpecialGuid1", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = specialGuid1;

            arParams[2] = new SqlCeParameter("@SpecialGuid2", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = specialGuid2;

            arParams[3] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = name;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static int GetCountByFeature(Guid siteGuid, Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_EmailTemplate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");
            
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPageByFeature(
            Guid siteGuid,
            Guid featureGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByFeature(siteGuid, featureGuid);

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

            int offset = 0;
            if (pageNumber > 1) { offset = (pageSize * pageNumber) - pageSize; }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_EmailTemplate  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("FeatureGuid = @FeatureGuid ");

            sqlCommand.Append("ORDER BY [Name]  "); 
            //order by is required if using fetch and offset or an error will occur, uncomment it and put at least one column to sort by

            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");

            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@FeatureGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

    }
}
