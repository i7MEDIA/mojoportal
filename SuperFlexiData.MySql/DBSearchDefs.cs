// Created:					2017-07-17
// Last Modified:			2017-07-18

using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace SuperFlexiData
{
    public static class DBSearchDefs
    {
        public static bool Create(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("INSERT INTO i7_sflexi_searchdefs ({0}) VALUES ({1});",
                "Guid,"
                + "SiteGuid,"
                + "FeatureGuid,"
                + "FieldDefinitionGuid,"
                + "Title,"
                + "Keywords,"
                + "Description,"
                + "Link,"
                + "LinkQueryAddendum"
                , "?Guid,"
                + "?SiteGuid,"
                + "?FeatureGuid,"
                + "?FieldDefinitionGuid,"
                + "?Title,"
                + "?Keywords,"
                + "?Description,"
                + "?Link,"
                + "?LinkQueryAddendum");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?FeatureGuid", MySqlDbType.Guid);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            arParams[2] = new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = definitionGuid;

            arParams[3] = new MySqlParameter("?Guid", MySqlDbType.Guid)
            {
                Direction = ParameterDirection.Input,
                Value = guid
            };
            arParams[4] = new MySqlParameter("?Title", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = title;

            arParams[5] = new MySqlParameter("?Keywords", MySqlDbType.VarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = keywords;

            arParams[6] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new MySqlParameter("?Link", MySqlDbType.LongText);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = link;

            arParams[8] = new MySqlParameter("?LinkQueryAddendum", MySqlDbType.VarChar, 16);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = linkQueryAddendum;

            int rowsAffected = Convert.ToInt32(MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return rowsAffected > 0;
        }

        public static bool Update(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("UPDATE i7_sflexi_searchdefs SET {0} WHERE Guid = ?Guid;",
                "SiteGuid = ?SiteGuid,"
                + "FeatureGuid = ?FeatureGuid,"
                + "FieldDefinitionGuid = ?FieldDefinitionGuid,"
                + "Title = ?Title,"
                + "Keywords = ?Keywords,"
                + "Description = ?Description,"
                + "Link = ?Link,"
                + "LinkQueryAddendum = ?LinkQueryAddendum");

            MySqlParameter[] arParams = new MySqlParameter[9];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?FeatureGuid", MySqlDbType.Guid);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            arParams[2] = new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = definitionGuid;

            arParams[3] = new MySqlParameter("?Guid", MySqlDbType.Guid);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = guid;

            arParams[4] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = title;

            arParams[5] = new MySqlParameter("?Keywords", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = keywords;

            arParams[6] = new MySqlParameter("?Description", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new MySqlParameter("?Link", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = link;

            arParams[8] = new MySqlParameter("?LinkQueryAddendum", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = linkQueryAddendum;

            int rowsAffected = Convert.ToInt32(MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return rowsAffected > 0;
        }

        public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_searchdefs WHERE FieldDefinitionGuid = ?FieldDefinitionGuid;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fieldDefGuid;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_searchdefs WHERE SiteGuid = ?SiteGuid;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM i7_sflexi_searchdefs WHERE Guid = ?Guid;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_searchdefs WHERE FieldDefinitionGuid = ?FieldDefinitionGuid;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FieldDefinitionGuid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = fieldDefinitionGuid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * FROM i7_sflexi_searchdefs WHERE Guid = ?Guid;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }
    }
}
