// Author:				Joe Audette
// Created:			    2010-07-02
// Last Modified:		2010-07-07
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
    public static class DBQuestion
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static int Add(
            Guid questionGuid,
            Guid surveyPageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            string validationMessage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyQuestions (");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("QuestionText, ");
            sqlCommand.Append("QuestionTypeId, ");
            sqlCommand.Append("AnswerIsRequired, ");
            sqlCommand.Append("QuestionOrder, ");
            sqlCommand.Append("ValidationMessage) ");
            sqlCommand.Append("SELECT @QuestionGuid, @PageGuid, @QuestionText, ");
            sqlCommand.Append("@QuestionTypeId, @AnswerIsRequired, Count(*), @ValidationMessage ");
            sqlCommand.Append("FROM mp_SurveyPages; ");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid;

            arParams[1] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyPageGuid;

            arParams[2] = new SqlCeParameter("@QuestionText", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new SqlCeParameter("@QuestionTypeId", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionTypeId;

            arParams[4] = new SqlCeParameter("@AnswerIsRequired", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = answerIsRequired;

            arParams[5] = new SqlCeParameter("@ValidationMessage", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = validationMessage;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid questionGuid,
            Guid surveyPageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            int questionOrder,
            string validationMessage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyQuestions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PageGuid = @PageGuid, ");
            sqlCommand.Append("QuestionText = @QuestionText, ");
            sqlCommand.Append("QuestionTypeId = @QuestionTypeId, ");
            sqlCommand.Append("AnswerIsRequired = @AnswerIsRequired, ");
            sqlCommand.Append("QuestionOrder = @QuestionOrder, ");
            sqlCommand.Append("ValidationMessage = @ValidationMessage ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid;

            arParams[1] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyPageGuid;

            arParams[2] = new SqlCeParameter("@QuestionText", SqlDbType.NVarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new SqlCeParameter("@QuestionTypeId", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionTypeId;

            arParams[4] = new SqlCeParameter("@AnswerIsRequired", SqlDbType.Bit);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = answerIsRequired;

            arParams[5] = new SqlCeParameter("@QuestionOrder", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = questionOrder;

            arParams[6] = new SqlCeParameter("@ValidationMessage", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = validationMessage;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(
            Guid questionGuid)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid; ");

            //now delete the question
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static IDataReader GetOne(
            Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAllByPage(Guid pageQuestionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestions ");
            sqlCommand.Append("WHERE PageGuid = @PageGuid ");
            sqlCommand.Append("ORDER BY QuestionOrder; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@PageGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageQuestionGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }




    }
}
