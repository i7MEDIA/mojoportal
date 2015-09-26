// Author:				Joe Audette
// Created:			    2010-07-02
// Last Modified:		2010-07-09
// 
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
using System.Data.SqlServerCe;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    public static class DBSurvey
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_Surveys ");
            sqlCommand.Append("(");
            sqlCommand.Append("SurveyGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SurveyName, ");
            sqlCommand.Append("CreationDate, ");
            sqlCommand.Append("StartPageText, ");
            sqlCommand.Append("EndPageText ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SurveyGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SurveyName, ");
            sqlCommand.Append("@CreationDate, ");
            sqlCommand.Append("@StartPageText, ");
            sqlCommand.Append("@EndPageText ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@SurveyName", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new SqlCeParameter("@CreationDate", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new SqlCeParameter("@StartPageText", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new SqlCeParameter("@EndPageText", SqlDbType.NText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@SurveyName", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = surveyName;

            arParams[3] = new SqlCeParameter("@CreationDate", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = creationDate;

            arParams[4] = new SqlCeParameter("@StartPageText", SqlDbType.NVarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = startPageText;

            arParams[5] = new SqlCeParameter("@EndPageText", SqlDbType.NVarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = endPageText;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //now delete survey pages
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid; ");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //now delete survey
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            

            

        }

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
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID) ");
            sqlCommand.Append("))); ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //now delete survey pages
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid IN (SELECT SurveyGuid FROM mp_Surveys ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID) ");
            sqlCommand.Append("))); ");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //now delete survey
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_Surveys ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = @SiteID);");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("COALESCE(s2.PageCount, 0) AS PageCount, ");
            sqlCommand.Append("COALESCE(s3.ResponseCount, 0) AS ResponseCount ");
            
            sqlCommand.Append("FROM	mp_Surveys s ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT SurveyGuid, Count(*) As PageCount ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("GROUP BY SurveyGuid ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.SurveyGuid = s.SurveyGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT SurveyGuid, Count(*) As ResponseCount ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("GROUP BY SurveyGuid ");
            sqlCommand.Append(") s3  ");
            sqlCommand.Append("ON s3.SurveyGuid = s.SurveyGuid ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.SurveyGuid = @SurveyGuid; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            
            sqlCommand.Append("COALESCE(s2.PageCount, 0) AS PageCount, ");
            sqlCommand.Append("COALESCE(s3.ResponseCount, 0) AS ResponseCount ");

            sqlCommand.Append("FROM	mp_Surveys s ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT SurveyGuid, Count(*) As PageCount ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("GROUP BY SurveyGuid ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.SurveyGuid = s.SurveyGuid ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT SurveyGuid, Count(*) As ResponseCount ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("GROUP BY SurveyGuid ");
            sqlCommand.Append(") s3  ");
            sqlCommand.Append("ON s3.SurveyGuid = s.SurveyGuid ");



            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.SiteGuid = @SiteGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("s.SurveyName ");
            //sqlCommand.Append(" ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static void AddToModule(Guid surveyGuid, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ModuleId = @ModuleId; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyModules (SurveyGuid, ModuleId) ");
            sqlCommand.Append("VALUES(@SurveyGuid, @ModuleId); ");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetModulesCurrentSurvey(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyModules ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleId = @ModuleId; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleId", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            object id = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());

        }

        public static Guid GetFirstPageGuid(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) PageGuid ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid ");
            sqlCommand.Append("Order By PageOrder ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            object id = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());

        }

        public static Guid GetNextPageGuid(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT PageOrder ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            int pageOrder = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) sp.PageGuid ");

            sqlCommand.Append("FROM	mp_SurveyPages sp ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sp.PageGuid <> @PageGuid ");
            sqlCommand.Append("AND sp.PageGuid IN (");

            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("SurveyGuid IN (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ) ");

            sqlCommand.Append(") ");

            sqlCommand.Append("AND sp.PageGuid IN (");

            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageOrder > @PageOrder ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY sp.[PageOrder]  ");
            sqlCommand.Append("; ");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            arParams[1] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            object id = SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            if (id == null) return Guid.Empty;

            return new Guid(id.ToString());

        }

        public static Guid GetPreviousPageGuid(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT PageOrder ");
            sqlCommand.Append("FROM	mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            int pageOrder = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) sp.PageGuid ");

            sqlCommand.Append("FROM	mp_SurveyPages sp ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sp.PageGuid <> @PageGuid ");
            sqlCommand.Append("AND sp.PageGuid IN (");

            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("SurveyGuid IN (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ) ");

            sqlCommand.Append(") ");

            sqlCommand.Append("AND sp.PageGuid IN (");

            sqlCommand.Append("SELECT PageGuid ");
            sqlCommand.Append("FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageOrder < @PageOrder ");
            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY sp.[PageOrder] DESC  ");
            sqlCommand.Append("; ");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            arParams[1] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageOrder;

            object id = SqlHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("qa.AnswerGuid,  ");
            sqlCommand.Append("qa.QuestionGuid,  ");
            sqlCommand.Append("qa.ResponseGuid,  ");
            sqlCommand.Append("qa.Answer,  ");
            sqlCommand.Append("qa.AnsweredDate,  ");
            sqlCommand.Append("u.Name,  ");
            sqlCommand.Append("u.Email  ");
            
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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

            sqlCommand.Append("WHERE sr.ResponseGuid = @ResponseGuid ");
            sqlCommand.Append("AND sr.Complete = 1 ");
            sqlCommand.Append("AND sp.PageEnabled = 1 ");

            sqlCommand.Append("ORDER BY sp.PageOrder, sq.QuestionOrder; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
