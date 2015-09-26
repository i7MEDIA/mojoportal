/// Author:				Rob Henry
/// Created:			2007-11-26
/// Last Modified:		2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

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
            sqlCommand.Append("INSERT INTO mp_Surveys (");
            sqlCommand.Append("SurveyGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SurveyName, ");
            sqlCommand.Append("CreationDate, ");
            sqlCommand.Append("StartPageText, ");
            sqlCommand.Append("EndPageText )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?SurveyGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?SurveyName, ");
            sqlCommand.Append("?CreationDate, ");
            sqlCommand.Append("?StartPageText, ");
            sqlCommand.Append("?EndPageText );");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?SurveyName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new MySqlParameter("?CreationDate", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new MySqlParameter("?StartPageText", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new MySqlParameter("?EndPageText", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_Surveys ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("SiteGuid = ?SiteGuid, ");
            sqlCommand.Append("SurveyName = ?SurveyName, ");
            sqlCommand.Append("CreationDate = ?CreationDate, ");
            sqlCommand.Append("StartPageText = ?StartPageText, ");
            sqlCommand.Append("EndPageText = ?EndPageText ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SurveyGuid = ?SurveyGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?SurveyName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new MySqlParameter("?CreationDate", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new MySqlParameter("?StartPageText", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new MySqlParameter("?EndPageText", MySqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("SurveyGuid = ?SurveyGuid)); ");

            //now delete survey questions
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (");
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = ?SurveyGuid);");

            //now delete survey pages
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = ?SurveyGuid; ");

            //now delete survey
            sqlCommand.Append("DELETE FROM mp_Surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = ?SurveyGuid;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("))); ");

            //now delete survey questions
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid IN (");
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("))); ");

            //now delete survey pages
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("))); ");

            //now delete survey
            sqlCommand.Append("DELETE FROM mp_Surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID);");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("s.SurveyGuid = ?SurveyGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_Surveys; ");

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SurveyGuid = ?SurveyGuid ");
            sqlCommand.Append("And Complete = 1; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("s.SiteGuid = ?SiteGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("s.SurveyName ");
            //sqlCommand.Append(" ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("WHERE SurveyGuid = ?SurveyGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = ?ModuleId; ");

            sqlCommand.Append("INSERT INTO mp_SurveyModules (SurveyGuid, ModuleId) ");
            sqlCommand.Append("VALUES(?SurveyGuid, ?ModuleId); ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static void RemoveFromModule(Guid surveyGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = ?ModuleId ");
            sqlCommand.Append("AND SurveyGuid = ?SurveyGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static void RemoveFromModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = ?ModuleId ");
            sqlCommand.Append("; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static Guid GetModulesCurrentSurvey(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleId = ?ModuleId; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleId", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId.ToString();

            object id = MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());
        }

        public static Guid GetFirstPageGuid(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = ?SurveyGuid ");
            sqlCommand.Append("Order By PageOrder ");
            sqlCommand.Append("Limit 1; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            object id = MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE PageOrder > (");
            sqlCommand.Append("SELECT PageOrder ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid) ");
            sqlCommand.Append("Order By PageOrder ");
            sqlCommand.Append("Limit 1; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            object id = MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("SELECT PageGuid ");

            sqlCommand.Append("FROM	mp_SurveyPages ");

            sqlCommand.Append("WHERE PageOrder < (");
            sqlCommand.Append("SELECT PageOrder ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid) ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid) ");
            sqlCommand.Append("Order By PageOrder DESC ");
            sqlCommand.Append("Limit 1; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            object id = MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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


            sqlCommand.Append("WHERE sr.SurveyGuid = ?SurveyGuid ");
            //sqlCommand.Append("AND sr.Complete = 1 ");
            //sqlCommand.Append("AND sp.PageEnabled = 1 ");

            //sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

           


        }


        ///// <summary>
        ///// Gets an IDataReader from the mp_SurveyQuestionAnswers table.
        ///// </summary>
        ///// <param name="answerGuid"> answerGuid </param>
        //public static DataTable GetResults(Guid surveyGuid)
        //{
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("SELECT  ");
        //    sqlCommand.Append("qa.AnswerGuid,  ");
        //    sqlCommand.Append("qa.QuestionGuid,  ");
        //    sqlCommand.Append("qa.ResponseGuid,  ");
        //    sqlCommand.Append("qa.Answer,  ");
        //    sqlCommand.Append("qa.AnsweredDate,  ");
        //    sqlCommand.Append("u.Name,  ");
        //    sqlCommand.Append("u.Email  ");
        //    //sqlCommand.Append("  ");
        //    //sqlCommand.Append("  ");
        //    //sqlCommand.Append("  ");

        //    sqlCommand.Append("FROM mp_SurveyQuestionAnswers qa ");

        //    sqlCommand.Append("JOIN mp_SurveyResponses sr ");
        //    sqlCommand.Append("ON qa.ResponseGuid = sr.ResponseGuid ");

        //    sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
        //    sqlCommand.Append("ON u.UserGuid = sr.UserGuid ");


        //    sqlCommand.Append("WHERE sr.SurveyGuid = ?SurveyGuid ");
        //    //sqlCommand.Append("AND sr.Complete = 1 ");
        //    //sqlCommand.Append("AND sp.PageEnabled = 1 ");

        //    //sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder ");

        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[1];

        //    arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = surveyGuid.ToString();

        //    DataTable dataTable = new DataTable();
        //    dataTable.Columns.Add("AnswerGuid", typeof(String));
        //    dataTable.Columns.Add("QuestionGuid", typeof(String));
        //    dataTable.Columns.Add("ResponseGuid", typeof(String));
        //    dataTable.Columns.Add("Answer", typeof(String));
        //    dataTable.Columns.Add("AnsweredDate", typeof(DateTime));
        //    dataTable.Columns.Add("Name", typeof(String));
        //    dataTable.Columns.Add("Email", typeof(String));

        //    IDataReader reader = MySqlHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    while (reader.Read())
        //    {
        //        DataRow row = dataTable.NewRow();
        //        row["AnswerGuid"] = reader["AnswerGuid"].ToString();
        //        row["QuestionGuid"] = reader["QuestionGuid"].ToString();
        //        row["ResponseGuid"] = reader["ResponseGuid"].ToString();
        //        row["Answer"] = reader["Answer"].ToString();
        //        row["AnsweredDate"] = Convert.ToDateTime(reader["AnsweredDate"]);
        //        row["Name"] = reader["Name"].ToString();
        //        row["Email"] = reader["Email"].ToString();

        //        dataTable.Rows.Add(row);

        //    }

        //    reader.Close();

        //    return dataTable;


        //}


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
            sqlCommand.Append("AND sr.ResponseGuid = qa.ResponseGuid ");

            sqlCommand.Append("WHERE sr.ResponseGuid = ?ResponseGuid ");
            sqlCommand.Append("AND sr.Complete = 1 ");
            sqlCommand.Append("AND sp.PageEnabled = 1 ");

            sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
