/// Author:				Rob Henry
/// Created:			2007-08-31
/// Last Modified:		2008-01-21
/// 
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
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
   
    public static class DBQuestionOption
    {
        

        /// <summary>
        /// Inserts a row in the mp_QuestionOptions table. Returns rows affected count.
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestionOptions_Insert", 4);
            sph.DefineSqlParameter("@QuestionOptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionOptionGuid);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, 255, ParameterDirection.Input, answer);
            sph.DefineSqlParameter("@Order", SqlDbType.Int, ParameterDirection.Input, order);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }


        /// <summary>
        /// Updates a row in the mp_QuestionOptions table. Returns true if row updated.
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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestionOptions_Update", 4);
            sph.DefineSqlParameter("@QuestionOptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionOptionGuid);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, 255, ParameterDirection.Input, answer);
            sph.DefineSqlParameter("@Order", SqlDbType.Int, ParameterDirection.Input, order);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_QuestionOptions table. Returns true if row deleted.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid questionOptionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestionOptions_Delete", 1);
            sph.DefineSqlParameter("@QuestionOptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionOptionGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_QuestionOptions table.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        public static IDataReader GetOne(
            Guid questionOptionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyQuestionOptions_SelectOne", 1);
            sph.DefineSqlParameter("@QuestionOptionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionOptionGuid);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the mp_QuestionOptions table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_QuestionOptions_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_QuestionOptions for a question.
        /// </summary>
        public static IDataReader GetAll(Guid questionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyQuestionOptions_Select", 1);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            return sph.ExecuteReader();
        }



    }
}
