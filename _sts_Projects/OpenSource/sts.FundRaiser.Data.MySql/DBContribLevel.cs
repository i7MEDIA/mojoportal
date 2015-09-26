// Author:					Joe Audette
// Created:					2013-09-06
// Last Modified:			2013-09-06
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

namespace sts.FundRaiser.Data
{
    public static class DBContribLevel
    {
        public static int Create(
            Guid guid,
            Guid moduleGuid,
            Guid siteGuid,
            string title,
            string description,
            decimal thresholdAmt,
            int allowedContributors,
            int totalContributors,
            Guid createdBy,
            DateTime createdUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_FundContribLevel (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ThresholdAmt, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("AllowedContributors, ");
            sqlCommand.Append("TotalContributors )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?ThresholdAmt, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModBy, ");
            sqlCommand.Append("?LastModUtc, ");
            sqlCommand.Append("?AllowedContributors, ");
            sqlCommand.Append("?TotalContributors )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = title;

            arParams[4] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new MySqlParameter("?ThresholdAmt", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = thresholdAmt;

            arParams[6] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy.ToString();

            arParams[7] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdBy.ToString();

            arParams[9] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdUtc;

            arParams[10] = new MySqlParameter("?AllowedContributors", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = allowedContributors;

            arParams[11] = new MySqlParameter("?TotalContributors", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = totalContributors;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            string title,
            string description,
            decimal thresholdAmt,
            int allowedContributors,
            int totalContributors,
            Guid lastModBy,
            DateTime lastModUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundContribLevel ");
            sqlCommand.Append("SET  ");
           
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("ThresholdAmt = ?ThresholdAmt, ");
            
            sqlCommand.Append("LastModBy = ?LastModBy, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");
            sqlCommand.Append("AllowedContributors = ?AllowedContributors, ");
            sqlCommand.Append("TotalContributors = ?TotalContributors ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?ThresholdAmt", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = thresholdAmt;

            arParams[4] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastModBy.ToString();

            arParams[5] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModUtc;

            arParams[6] = new MySqlParameter("?AllowedContributors", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = allowedContributors;

            arParams[7] = new MySqlParameter("?TotalContributors", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = totalContributors;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContribLevel ");
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

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContribLevel ");
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

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContribLevel ");
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

        public static void UpdateContributorCount(Guid levelGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundContribLevel ");

            sqlCommand.Append("SET TotalContributors = COALESCE((");
            sqlCommand.Append("SELECT Count(*) FROM sts_ContribProfile WHERE LevelGuid = ?LevelGuid), 0) ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LevelGuid = ?LevelGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?LevelGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = levelGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void UpdateContributorCountByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundContribLevel ");

            sqlCommand.Append("SET TotalContributors = COALESCE((");
            sqlCommand.Append("SELECT COUNT(*) FROM sts_ContribProfile WHERE LevelGuid = sts_FundContribLevel.Guid), 0) ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_FundContribLevel ");
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

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContribLevel ");
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

        public static IDataReader GetByAmount(Guid moduleGuid, decimal contribAmount)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContribLevel ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ThresholdAmt <= ?ContribAmount ");
            sqlCommand.Append("ORDER BY ThresholdAmt DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?ContribAmount", MySqlDbType.Decimal);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = contribAmount;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByModuleAscending(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContribLevel ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("ORDER BY ThresholdAmt ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByModuleDescending(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContribLevel ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("ORDER BY ThresholdAmt DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
