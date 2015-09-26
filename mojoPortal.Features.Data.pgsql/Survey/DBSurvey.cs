/// Author:				Joe Audette
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
    
    public static class DBSurvey
    {
        /// <summary>
        /// Inserts a row in the mp_Surveys table. Returns rows affected count.
        /// </summary>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="surveyName"> surveyName </param>
        /// <param name="creationDate"> creationDate </param>
        /// <param name="startPageText"> startPageText </param>
        /// <param name="endPageText"> endPageText </param>
        /// <returns>int</returns>
        public static int Add(
            Guid surveyGuid,
            Guid siteGuid,
            string surveyName,
            DateTime creationDate,
            string startPageText,
            string endPageText)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_surveys (");
            sqlCommand.Append("surveyguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("surveyname, ");
            sqlCommand.Append("creationdate, ");
            sqlCommand.Append("startpagetext, ");
            sqlCommand.Append("endpagetext )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":surveyguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":surveyname, ");
            sqlCommand.Append(":creationdate, ");
            sqlCommand.Append(":startpagetext, ");
            sqlCommand.Append(":endpagetext ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("surveyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new NpgsqlParameter("creationdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new NpgsqlParameter("startpagetext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new NpgsqlParameter("endpagetext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the mp_Surveys table. Returns true if row updated.
        /// </summary>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="surveyName"> surveyName </param>
        /// <param name="creationDate"> creationDate </param>
        /// <param name="startPageText"> startPageText </param>
        /// <param name="endPageText"> endPageText </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid surveyGuid,
            Guid siteGuid,
            string surveyName,
            DateTime creationDate,
            string startPageText,
            string endPageText)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_surveys ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("surveyname = :surveyname, ");
            sqlCommand.Append("creationdate = :creationdate, ");
            sqlCommand.Append("startpagetext = :startpagetext, ");
            sqlCommand.Append("endpagetext = :endpagetext ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("surveyguid = :surveyguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("surveyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new NpgsqlParameter("creationdate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new NpgsqlParameter("startpagetext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new NpgsqlParameter("endpagetext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Surveys table. Returns true if row deleted.
        /// </summary>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <returns>bool</returns>
        public static void Delete(Guid surveyGuid)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_surveyquestionoptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid IN (");
            sqlCommand.Append("SELECT questionguid ");
            sqlCommand.Append("FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid IN (");
            sqlCommand.Append("SELECT pageguid ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid)); ");

            //now delete survey questions
            sqlCommand.Append("DELETE FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid IN (");
            sqlCommand.Append("SELECT pageguid ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid);");

            //now delete survey pages
            sqlCommand.Append("DELETE FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid; ");

            //now delete survey
            sqlCommand.Append("DELETE FROM mp_surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();


            NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// deletes all survey content for the passed in siteid
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static bool DeleteBySite(int siteId)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_surveyquestionoptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid IN (");
            sqlCommand.Append("SELECT questionguid ");
            sqlCommand.Append("FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid IN (");
            sqlCommand.Append("SELECT pageguid ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid IN (SELECT surveyguid FROM mp_surveys ");
            sqlCommand.Append("WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid) ");
            sqlCommand.Append("))); ");

            //now delete survey questions
            sqlCommand.Append("DELETE FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid IN (");
            sqlCommand.Append("SELECT pageguid ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid IN (SELECT surveyguid FROM mp_surveys ");
            sqlCommand.Append("WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid) ");
            sqlCommand.Append("))); ");

            //now delete survey pages
            sqlCommand.Append("DELETE FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid IN (SELECT surveyguid FROM mp_surveys ");
            sqlCommand.Append("WHERE siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid) ");
            sqlCommand.Append("))); ");

            //now delete survey
            sqlCommand.Append("DELETE FROM mp_surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid);");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Surveys table.
        /// </summary>
        /// <param name="surveyGuid"> surveyGuid </param>
        public static IDataReader GetOne(
            Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  s.*, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_surveypages sp WHERE sp.surveyguid = s.surveyguid) AS pagecount, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_surveyresponses sr WHERE sr.surveyguid = s.surveyguid) AS responsecount ");
            sqlCommand.Append("FROM	mp_surveys s ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.surveyguid = :surveyguid; ");

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
        /// Gets a count of rows in the mp_Surveys table.
        /// </summary>
        public static int GetCount()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_surveys ");
            sqlCommand.Append(";");

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets a count of responses in a survey.
        /// </summary>
        public static int GetResponseCount(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid ");
            sqlCommand.Append("And complete = true; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_Surveys table.
        /// </summary>
        public static IDataReader GetAll(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.surveyguid, ");
            sqlCommand.Append("s.siteguid, ");
            sqlCommand.Append("s.surveyname, ");
            sqlCommand.Append("s.creationdate, ");
            sqlCommand.Append("s.startpagetext, ");
            sqlCommand.Append("s.endpagetext, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_surveypages sp WHERE sp.surveyguid = s.surveyguid) AS pagecount, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_surveyresponses sr WHERE sr.surveyguid = s.surveyguid) AS responsecount ");
            sqlCommand.Append(" ");
            sqlCommand.Append(" ");
            sqlCommand.Append(" ");

            sqlCommand.Append("FROM	mp_surveys s ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.siteguid = :siteguid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("s.surveyname ");
            //sqlCommand.Append(" ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets the count of pages in a survey
        /// </summary>
        /// <param name="surveyGuid"></param>
        /// <returns></returns>
        public static int PagesCount(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_surveypages ");
            sqlCommand.Append("WHERE surveyguid = :surveyguid; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Set the current survey for a module
        /// </summary>
        /// <param name="surveyGuid"></param>
        /// <param name="moduleId"></param>
        public static void AddToModule(Guid surveyGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_surveymodules ");
            sqlCommand.Append("WHERE moduleid = :moduleid; ");

            sqlCommand.Append("INSERT INTO mp_surveymodules (surveyguid, moduleid) ");
            sqlCommand.Append("VALUES(:surveyguid, :moduleid); ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static void RemoveFromModule(Guid surveyGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_surveymodules ");
            sqlCommand.Append("WHERE moduleid = :moduleid ");
            sqlCommand.Append("AND surveyguid = :surveyguid; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static void RemoveFromModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_surveymodules ");
            sqlCommand.Append("WHERE moduleid = :moduleid ");
            sqlCommand.Append("; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetModulesCurrentSurvey(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT surveyguid ");
            sqlCommand.Append("FROM mp_surveymodules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            object id = NpgsqlHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());

        }

        public static Guid GetFirstPageGuid(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT pageguid ");
            sqlCommand.Append("FROM	mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid ");
            sqlCommand.Append("Order By pageorder ");
            sqlCommand.Append("Limit 1; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            object id = NpgsqlHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());


        }

        /// <summary>
        /// Gets the guid of the next page in the survey
        /// </summary>
        /// <param name="pageGuid">The guid of the current page</param>
        /// <returns></returns>
        public static Guid GetNextPageGuid(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT pageguid ");
            sqlCommand.Append("FROM	mp_surveypages ");
            sqlCommand.Append("WHERE pageorder > (");
            sqlCommand.Append("SELECT pageorder ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("surveyguid = (");
            sqlCommand.Append("SELECT surveyguid ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid) ");
            sqlCommand.Append("Order By pageorder ");
            sqlCommand.Append("Limit 1; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            object id = NpgsqlHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());

        }

        /// <summary>
        /// Gets the previous page Guid in the survey
        /// </summary>
        /// <param name="pageGuid">Current page Guid</param>
        /// <returns></returns>
        public static Guid GetPreviousPageGuid(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT pageguid ");

            sqlCommand.Append("FROM	mp_surveypages ");

            sqlCommand.Append("WHERE pageorder < (");
            sqlCommand.Append("SELECT pageorder ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid) ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("surveyguid = (");
            sqlCommand.Append("SELECT surveyguid ");
            sqlCommand.Append("FROM mp_surveypages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pageguid = :pageguid) ");
            sqlCommand.Append("Order By pageorder DESC ");
            sqlCommand.Append("Limit 1; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            object id = NpgsqlHelper.ExecuteScalar(ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());

        }

        /// <summary>
        /// Gets an IDataReader from the mp_SurveyQuestionAnswers table.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        public static IDataReader GetResults(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("qa.answerguid,  ");
            sqlCommand.Append("qa.questionguid,  ");
            sqlCommand.Append("qa.responseguid,  ");
            sqlCommand.Append("qa.answer,  ");
            sqlCommand.Append("qa.answereddate,  ");
            sqlCommand.Append("u.name,  ");
            sqlCommand.Append("u.email  ");
            //sqlCommand.Append("  ");
            //sqlCommand.Append("  ");
            //sqlCommand.Append("  ");

            sqlCommand.Append("FROM mp_surveyquestionanswers qa ");

            sqlCommand.Append("JOIN mp_surveyresponses sr ");
            sqlCommand.Append("ON qa.responseguid = sr.responseguid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON u.userguid = sr.userguid ");


            sqlCommand.Append("WHERE sr.surveyguid = :surveyguid ");
            //sqlCommand.Append("AND sr.Complete = 1 ");
            //sqlCommand.Append("AND sp.PageEnabled = 1 ");

            //sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder ");

            sqlCommand.Append(";");

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
        /// Gets an IDataReader with all rows in the mp_SurveyQuestionAnswers table.
        /// </summary>
        public static IDataReader GetOneResult(Guid responseGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM mp_surveys s ");

            sqlCommand.Append("JOIN mp_surveyresponses sr ");
            sqlCommand.Append("ON s.surveyguid = sr.SurveyGuid ");

            sqlCommand.Append("JOIN mp_surveypages sp ");
            sqlCommand.Append("ON sr.surveyguid = sp.SurveyGuid ");

            sqlCommand.Append("JOIN mp_surveyquestions sq ");
            sqlCommand.Append("ON sp.pageguid = sq.pageguid ");

            sqlCommand.Append("LEFT JOIN mp_SurveyQuestionAnswers qa ");
            sqlCommand.Append("ON sq.questionguid = qa.questionguid ");
            sqlCommand.Append("AND sr.responseguid = qa.responseguid ");

            sqlCommand.Append("WHERE sr.responseguid = :responseguid ");
            sqlCommand.Append("AND sr.complete = true ");
            sqlCommand.Append("AND sp.pageenabled = true ");

            sqlCommand.Append("ORDER BY sp.pageorder, sq.questionorder; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
