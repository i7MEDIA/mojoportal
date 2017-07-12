/// Author:				
/// Created:			2008-08-29
/// Last Modified:		2012-08-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.


using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Npgsql;

namespace SurveyFeature.Data
{
    
    public static class DBSurveyPage
    {
        /// <summary>
        /// Inserts a row in the mp_SurveyPages table. Returns rows affected count.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="pageTitle"> pageTitle </param>
        /// <param name="pageOrder"> pageOrder </param>
        /// <param name="pageEnabled"> pageEnabled </param>
        /// <returns>int</returns>
        public static int Add(
            Guid pageGuid,
            Guid surveyGuid,
            string pageTitle,
            bool pageEnabled)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_surveypages (");
            sqlCommand.Append("pageguid, ");
            sqlCommand.Append("surveyguid, ");
            sqlCommand.Append("pagetitle, ");
            sqlCommand.Append("pageorder, ");
            sqlCommand.Append("pageenabled )");

            sqlCommand.Append("SELECT :pageguid, :surveyguid, :pagetitle, ");
            sqlCommand.Append("Count(*), :pageenabled FROM mp_surveypages; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            arParams[1] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            arParams[2] = new NpgsqlParameter("pagetitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageTitle;

            arParams[3] = new NpgsqlParameter("pageenabled", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageEnabled;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the mp_SurveyPages table. Returns true if row updated.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="pageTitle"> pageTitle </param>
        /// <param name="pageOrder"> pageOrder </param>
        /// <param name="pageEnabled"> pageEnabled </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid pageGuid,
            Guid surveyGuid,
            string pageTitle,
            int pageOrder,
            bool pageEnabled)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_surveypages ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("surveyguid = :surveyguid, ");
            sqlCommand.Append("pagetitle = :pagetitle, ");
            sqlCommand.Append("pageorder = :pageorder, ");
            sqlCommand.Append("pageenabled = :pageenabled ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("pageguid = :pageguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            arParams[1] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            arParams[2] = new NpgsqlParameter("pagetitle", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageTitle;

            arParams[3] = new NpgsqlParameter("pageorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new NpgsqlParameter("pageenabled", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageEnabled;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SurveyPages table. Returns true if row deleted.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid pageGuid)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_surveyquestionoptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid IN (");
            sqlCommand.Append("SELECT questionguid ");
            sqlCommand.Append("FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid); ");

            //now delete survey questions
            sqlCommand.Append("DELETE FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid; ");

            //now delete pages
            sqlCommand.Append("DELETE FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyPages table.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        public static IDataReader GetOne(
            Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  sp.*, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_surveyquestions sq WHERE sp.pageguid = sq.pageguid) AS questioncount ");
            sqlCommand.Append("FROM	mp_surveypages sp ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sp.pageguid = :pageguid ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyPages table.
        /// </summary>
        public static IDataReader GetAll(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT sp.*, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_surveyquestions sq WHERE sp.pageguid = sq.pageguid) AS questioncount ");
            sqlCommand.Append("FROM	mp_surveypages sp ");
            sqlCommand.Append("WHERE sp.surveyguid = :surveyguid ");
            sqlCommand.Append("ORDER BY sp.pageorder; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_SurveyPages table.
        /// </summary>
        public static int GetQuestionsCount(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_surveyquestions ");
            sqlCommand.Append("WHERE pageguid = :pageguid; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }



    }
}
