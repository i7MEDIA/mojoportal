/// Author:				Rob Henry
/// Created:			2007-08-31
/// Last Modified:		2010-07-02
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
    /// <summary>
    
    ///  
    /// </summary>
    public static class DBQuestionAnswer
    {
        
        /// <summary>
        /// Inserts a row in the mp_SurveyQuestionAnswers table. Returns rows affected count.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="answeredDate"> answeredDate </param>
        /// <returns>int</returns>
        public static int Add(
            Guid answerGuid,
            Guid questionGuid,
            Guid responseGuid,
            string answer)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestionAnswers_Insert", 4);
            sph.DefineSqlParameter("@AnswerGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, answerGuid);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, -1, ParameterDirection.Input, answer);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }


        /// <summary>
        /// Updates a row in the mp_SurveyQuestionAnswers table. Returns true if row updated.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="answeredDate"> answeredDate </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid answerGuid,
            Guid questionGuid,
            Guid responseGuid,
            string answer)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyQuestionAnswers_Update", 4);
            sph.DefineSqlParameter("@AnswerGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, answerGuid);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            sph.DefineSqlParameter("@Answer", SqlDbType.NVarChar, -1, ParameterDirection.Input, answer);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyQuestionAnswers table.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        public static IDataReader GetOne(Guid responseGuid, Guid questionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyQuestionAnswers_SelectOne", 2);
            sph.DefineSqlParameter("@ResponseGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, responseGuid);
            sph.DefineSqlParameter("@QuestionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, questionGuid);
            return sph.ExecuteReader();
        }



    }
}
