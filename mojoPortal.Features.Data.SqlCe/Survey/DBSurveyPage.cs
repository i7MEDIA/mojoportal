// Author:				Joe Audette
// Created:			    2010-07-02
// Last Modified:		2010-07-08
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
    public static class DBSurveyPage
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static int Add(
            Guid pageGuid,
            Guid surveyGuid,
            string pageTitle,
            bool pageEnabled)
        {
            StringBuilder sqlCommand = new StringBuilder();
           
            sqlCommand.Append("INSERT INTO mp_SurveyPages ");
            sqlCommand.Append("(PageGuid, ");
            sqlCommand.Append("SurveyGuid, ");
            sqlCommand.Append("PageTitle, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("PageEnabled) ");
            sqlCommand.Append("SELECT @PageGuid, @SurveyGuid, @PageTitle, ");
            sqlCommand.Append("Count(*), @PageEnabled FROM mp_SurveyPages; ");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            arParams[1] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid;

            arParams[2] = new SqlCeParameter("@PageTitle", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageTitle;

            arParams[3] = new SqlCeParameter("@PageEnabled", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageEnabled;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid surveyPageGuid,
            Guid surveyGuid,
            string pageTitle,
            int pageOrder,
            bool pageEnabled)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyPages ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid, ");
            sqlCommand.Append("PageTitle = @PageTitle, ");
            sqlCommand.Append("PageOrder = @PageOrder, ");
            sqlCommand.Append("PageEnabled = @PageEnabled ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("PageGuid = @PageGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyPageGuid;

            arParams[1] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid;

            arParams[2] = new SqlCeParameter("@PageTitle", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageTitle;

            arParams[3] = new SqlCeParameter("@PageOrder", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new SqlCeParameter("@PageEnabled", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = pageEnabled;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(
            Guid surveyPageGuid)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid IN (");
            sqlCommand.Append("SELECT QuestionGuid ");
            sqlCommand.Append("FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid); ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyPageGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //now delete survey questions
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid; ");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyPageGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //now delete pages
            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = @PageGuid ;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyPageGuid;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetOne(
            Guid surveyPageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  sp.*, ");
            sqlCommand.Append("COALESCE(s2.QuestionCount, 0) AS QuestionCount ");

            //sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyQuestions sq WHERE sp.PageGuid = sq.PageGuid) AS QuestionCount ");
            
            sqlCommand.Append("FROM	mp_SurveyPages sp ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT PageGuid, Count(*) As QuestionCount ");
            sqlCommand.Append("FROM mp_SurveyQuestions ");
            sqlCommand.Append("GROUP BY PageGuid ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.PageGuid = sp.PageGuid ");
            
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sp.PageGuid = @PageGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyPageGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAll(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT sp.*, ");
            sqlCommand.Append("COALESCE(s2.QuestionCount, 0) AS QuestionCount ");
            
            //sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyQuestions sq WHERE sp.PageGuid = sq.PageGuid) AS QuestionCount ");
            
            sqlCommand.Append("FROM	mp_SurveyPages sp ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT PageGuid, Count(*) As QuestionCount ");
            sqlCommand.Append("FROM mp_SurveyQuestions ");
            sqlCommand.Append("GROUP BY PageGuid ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.PageGuid = sp.PageGuid ");
            
            sqlCommand.Append("WHERE sp.SurveyGuid = @SurveyGuid ");
            
            sqlCommand.Append("ORDER BY sp.PageOrder; ");

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

        public static int GetQuestionsCount(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SurveyQuestions ");
            sqlCommand.Append("WHERE PageGuid = @PageGuid; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

    }
}
