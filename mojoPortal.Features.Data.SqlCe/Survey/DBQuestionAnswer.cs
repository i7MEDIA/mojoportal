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
    public static class DBQuestionAnswer
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static int Add(
            Guid answerGuid,
            Guid questionGuid,
            Guid responseGuid,
            string answer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyQuestionAnswers ");
            sqlCommand.Append("(");
            sqlCommand.Append("AnswerGuid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("ResponseGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("AnsweredDate ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@AnswerGuid, ");
            sqlCommand.Append("@QuestionGuid, ");
            sqlCommand.Append("@ResponseGuid, ");
            sqlCommand.Append("@Answer, ");
            sqlCommand.Append("@AnsweredDate ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@AnswerGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid;

            arParams[1] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid;

            arParams[2] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid;

            arParams[3] = new SqlCeParameter("@Answer", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            arParams[4] = new SqlCeParameter("@AnsweredDate", SqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = DateTime.UtcNow;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid answerGuid,
            Guid questionGuid,
            Guid responseGuid,
            string answer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyQuestionAnswers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid, ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid, ");
            sqlCommand.Append("Answer = @Answer ");


            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("AnswerGuid = @AnswerGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@AnswerGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid;

            arParams[1] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid;

            arParams[2] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid;

            arParams[3] = new SqlCeParameter("@Answer", SqlDbType.NVarChar);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetOne(Guid responseGuid, Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestionAnswers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid ");
            sqlCommand.Append("AND ResponseGuid = @ResponseGuid; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@QuestionGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid;

            arParams[1] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = responseGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
