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
    
    public static class DBQuestion
    {
        
        /// <summary>
        /// Inserts a row in the mp_SurveyQuestions table. Returns rows affected count.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="questionText"> questionText </param>
        /// <param name="questionTypeId"> questionTypeId </param>
        /// <param name="answerIsRequired"> answerIsRequired </param>
        /// <param name="questionOrder"> questionOrder </param>
        /// <param name="validationMessage"> validationMessage </param>
        /// <returns>int</returns>
        public static int Add(
            Guid questionGuid,
            Guid pageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            string validationMessage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_surveyquestions (");
            sqlCommand.Append("questionguid, ");
            sqlCommand.Append("pageguid, ");
            sqlCommand.Append("questiontext, ");
            sqlCommand.Append("questiontypeid, ");
            sqlCommand.Append("answerisrequired, ");
            sqlCommand.Append("questionorder, ");
            sqlCommand.Append("validationmessage )");

            sqlCommand.Append("SELECT :questionguid, :pageguid, :questiontext, ");
            sqlCommand.Append(":questiontypeid, :answerisrequired, Count(*), :validationmessage ");
            sqlCommand.Append("FROM mp_surveypages; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[6];

            arParams[0] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            arParams[1] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageGuid.ToString();

            arParams[2] = new NpgsqlParameter("questiontext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new NpgsqlParameter("questiontypeid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionTypeId;

            arParams[4] = new NpgsqlParameter("answerisrequired", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = answerIsRequired;

            arParams[5] = new NpgsqlParameter("validationmessage", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = validationMessage;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_SurveyQuestions table. Returns true if row updated.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="questionText"> questionText </param>
        /// <param name="questionTypeId"> questionTypeId </param>
        /// <param name="answerIsRequired"> answerIsRequired </param>
        /// <param name="questionOrder"> questionOrder </param>
        /// <param name="validationMessage"> validationMessage </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid questionGuid,
            Guid pageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            int questionOrder,
            string validationMessage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_surveyquestions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("pageguid = :pageguid, ");
            sqlCommand.Append("questiontext = :questiontext, ");
            sqlCommand.Append("questiontypeid = :questiontypeid, ");
            sqlCommand.Append("answerisrequired = :answerisrequired, ");
            sqlCommand.Append("questionorder = :questionorder, ");
            sqlCommand.Append("validationmessage = :validationmessage ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("questionguid = :questionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[7];

            arParams[0] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            arParams[1] = new NpgsqlParameter("pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageGuid.ToString();

            arParams[2] = new NpgsqlParameter("questiontext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new NpgsqlParameter("questiontypeid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionTypeId;

            arParams[4] = new NpgsqlParameter("answerisrequired", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = answerIsRequired;

            arParams[5] = new NpgsqlParameter("questionorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = questionOrder;

            arParams[6] = new NpgsqlParameter("validationmessage", NpgsqlTypes.NpgsqlDbType.Varchar, 256);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = validationMessage;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SurveyQuestions table. Returns true if row deleted.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid = :questionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyQuestions table.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        public static IDataReader GetOne(
            Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyquestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid = :questionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyQuestions table.
        /// </summary>
        public static IDataReader GetAllByPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyquestions ");
            sqlCommand.Append("WHERE pageguid = :pageguid ");
            sqlCommand.Append("ORDER BY questionorder; ");

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


    }
}
