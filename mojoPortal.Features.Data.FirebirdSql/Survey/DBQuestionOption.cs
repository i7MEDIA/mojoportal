using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
//using log4net;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    /// <summary>
    /// Author:				Joe Audette
    /// Created:			2008-08-29
    /// Last Modified:		2008-08-29
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public static class DBQuestionOption
    {
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        /// <summary>
        /// Inserts a row in the mp_SurveyQuestionOptions table. Returns rows affected count.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="order"> order </param>
        /// <returns>int</returns>
        public static int Add(
            Guid questionOptionGuid,
            Guid questionGuid,
            string answer,
            int order)
        {
            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@QuestionOptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            arParams[1] = new FbParameter("@QuestionGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new FbParameter("@Answer", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new FbParameter("@Order", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = order;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyQuestionOptions (");
            sqlCommand.Append("QuestionOptionGuid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("\"Order\" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@QuestionOptionGuid, ");
            sqlCommand.Append("@QuestionGuid, ");
            sqlCommand.Append("@Answer, ");
            sqlCommand.Append("@Order )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_SurveyQuestionOptions table. Returns true if row updated.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="order"> order </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid questionOptionGuid,
            Guid questionGuid,
            string answer,
            int order)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyQuestionOptions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid, ");
            sqlCommand.Append("Answer = @Answer, ");
            sqlCommand.Append("\"Order\" = @Order ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("QuestionOptionGuid = @QuestionOptionGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@QuestionOptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            arParams[1] = new FbParameter("@QuestionGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new FbParameter("@Answer", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new FbParameter("@Order", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = order;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SurveyQuestionOptions table. Returns true if row deleted.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid questionOptionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionOptionGuid = @QuestionOptionGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@QuestionOptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyQuestionOptions table.
        /// </summary>
        public static IDataReader GetAll(Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = @QuestionGuid ");
            sqlCommand.Append("ORDER BY \"Order\" ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@QuestionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyQuestionOptions table.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        public static IDataReader GetOne(
            Guid questionOptionGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionOptionGuid = @QuestionOptionGuid ");
            sqlCommand.Append("ORDER BY \"Order\" ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@QuestionOptionGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }


    }
}
