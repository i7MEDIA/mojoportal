/// Author:				Joe Audette
/// Created:			2008-08-29
/// Last Modified:		2009-06-23
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
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    
    public static class DBSurvey
    {
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }


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
            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@SurveyName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new FbParameter("@CreationDate", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new FbParameter("@StartPageText", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new FbParameter("@EndPageText", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Surveys (");
            sqlCommand.Append("SurveyGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SurveyName, ");
            sqlCommand.Append("CreationDate, ");
            sqlCommand.Append("StartPageText, ");
            sqlCommand.Append("EndPageText )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@SurveyGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SurveyName, ");
            sqlCommand.Append("@CreationDate, ");
            sqlCommand.Append("@StartPageText, ");
            sqlCommand.Append("@EndPageText )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("UPDATE mp_Surveys ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("SurveyName = @SurveyName, ");
            sqlCommand.Append("CreationDate = @CreationDate, ");
            sqlCommand.Append("StartPageText = @StartPageText, ");
            sqlCommand.Append("EndPageText = @EndPageText ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@SurveyName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new FbParameter("@CreationDate", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new FbParameter("@StartPageText", FbDbType.VarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new FbParameter("@EndPageText", FbDbType.VarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
         
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid IN (");
            sqlCommand.Append("SELECT QuestionGuid ");
            sqlCommand.Append("FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (");
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid)); ");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //now delete survey questions
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (");
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid);");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //now delete survey pages
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid; ");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //now delete survey
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid;");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid IN (");
            sqlCommand.Append("SELECT QuestionGuid ");
            sqlCommand.Append("FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (");
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID) ");
            sqlCommand.Append("))); ");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //now delete survey questions
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (");
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID) ");
            sqlCommand.Append("))); ");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //now delete survey pages
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID) ");
            sqlCommand.Append("))); ");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            //now delete survey
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID);");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyPages sp WHERE sp.SurveyGuid = s.SurveyGuid) AS PageCount, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyResponses sr WHERE sr.SurveyGuid = s.SurveyGuid) AS ResponseCount ");
            sqlCommand.Append("FROM	mp_Surveys s ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.SurveyGuid = @SurveyGuid; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_Surveys ");
            sqlCommand.Append(";");

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid ");
            sqlCommand.Append("And Complete = 1; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("s.SurveyGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("s.SurveyName, ");
            sqlCommand.Append("s.CreationDate, ");
            sqlCommand.Append("s.StartPageText, ");
            sqlCommand.Append("s.EndPageText, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyPages sp WHERE sp.SurveyGuid = s.SurveyGuid) AS PageCount, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyResponses sr WHERE sr.SurveyGuid = s.SurveyGuid) AS ResponseCount ");
            sqlCommand.Append(" ");
            sqlCommand.Append(" ");
            sqlCommand.Append(" ");

            sqlCommand.Append("FROM	mp_Surveys s ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("s.SurveyName ");
            //sqlCommand.Append(" ");
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
        /// Gets the count of pages in a survey
        /// </summary>
        /// <param name="surveyGuid"></param>
        /// <returns></returns>
        public static int PagesCount(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE SurveyGuid = @SurveyGuid; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = @ModuleId; ");

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyModules (SurveyGuid, ModuleId) ");
            sqlCommand.Append("VALUES(@SurveyGuid, @ModuleId); ");

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static void RemoveFromModule(Guid surveyGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = @ModuleId ");
            sqlCommand.Append("AND SurveyGuid = @SurveyGuid; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static void RemoveFromModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = @ModuleId ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetModulesCurrentSurvey(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleId = @ModuleId; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            object id = FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());


        }

        public static Guid GetFirstPageGuid(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST 1 PageGuid ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid ");
            sqlCommand.Append("Order By PageOrder ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            object id = FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT FIRST 1 PageGuid ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE PageOrder > (");
            sqlCommand.Append("SELECT PageOrder ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid) ");
            sqlCommand.Append("Order By PageOrder ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            object id = FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT FIRST 1 PageGuid ");

            sqlCommand.Append("FROM	mp_SurveyPages ");

            sqlCommand.Append("WHERE PageOrder < (");
            sqlCommand.Append("SELECT PageOrder ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid) ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid) ");
            sqlCommand.Append("Order By PageOrder DESC ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@PageGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            object id = FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("qa.AnswerGuid,  ");
            sqlCommand.Append("qa.QuestionGuid,  ");
            sqlCommand.Append("qa.ResponseGuid,  ");
            sqlCommand.Append("qa.Answer,  ");
            sqlCommand.Append("qa.AnsweredDate,  ");
            sqlCommand.Append("u.Name,  ");
            sqlCommand.Append("u.Email  ");
            //sqlCommand.Append("  ");
            //sqlCommand.Append("  ");
            //sqlCommand.Append("  ");

            sqlCommand.Append("FROM mp_SurveyQuestionAnswers qa ");

            sqlCommand.Append("JOIN mp_SurveyResponses sr ");
            sqlCommand.Append("ON qa.ResponseGuid = sr.ResponseGuid ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON u.UserGuid = sr.UserGuid ");


            sqlCommand.Append("WHERE sr.SurveyGuid = @SurveyGuid ");
            //sqlCommand.Append("AND sr.Complete = 1 ");
            //sqlCommand.Append("AND sp.PageEnabled = 1 ");

            //sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("FROM mp_Surveys s ");

            sqlCommand.Append("JOIN mp_SurveyResponses sr ");
            sqlCommand.Append("ON s.SurveyGuid = sr.SurveyGuid ");

            sqlCommand.Append("JOIN mp_SurveyPages sp ");
            sqlCommand.Append("ON sr.SurveyGuid = sp.SurveyGuid ");

            sqlCommand.Append("JOIN mp_SurveyQuestions sq ");
            sqlCommand.Append("ON sp.PageGuid = sq.PageGuid ");

            sqlCommand.Append("LEFT JOIN mp_SurveyQuestionAnswers qa ");
            sqlCommand.Append("ON sq.QuestionGuid = qa.QuestionGuid ");
            sqlCommand.Append("AND sr.REsponseGuid = qa.ResponseGuid ");

            sqlCommand.Append("WHERE sr.ResponseGuid = @ResponseGuid ");
            sqlCommand.Append("AND sr.Complete = 1 ");
            sqlCommand.Append("AND sp.PageEnabled = 1 ");

            sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
