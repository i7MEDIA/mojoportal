// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2010-04-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBSavedQuery
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        /// <summary>
        /// Inserts a row in the mp_SavedQuery table. Returns rows affected count.
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="name"> name </param>
        /// <param name="statement"> statement </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdBy"> createdBy </param>
        /// <returns>int</returns>
        public static int Create(
            Guid id,
            string name,
            string statement,
            DateTime createdUtc,
            Guid createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SavedQuery ");
            sqlCommand.Append("(");
            sqlCommand.Append("Id, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Statement, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Id, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@Statement, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@LastModBy ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[7];

            arParams[0] = new SqlCeParameter("@Id", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new SqlCeParameter("@Statement", SqlDbType.NText);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = statement;

            arParams[3] = new SqlCeParameter("@CreatedUtc", SqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdUtc;

            arParams[4] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdBy;

            arParams[5] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdUtc;

            arParams[6] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the mp_SavedQuery table. Returns true if row updated.
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="statement"> statement </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="lastModBy"> lastModBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid id,
            string statement,
            DateTime lastModUtc,
            Guid lastModBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SavedQuery ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("Statement = @Statement, ");
            sqlCommand.Append("LastModUtc = @LastModUtc, ");
            sqlCommand.Append("LastModBy = @LastModBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[4];

            arParams[0] = new SqlCeParameter("@Id", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            arParams[1] = new SqlCeParameter("@Statement", SqlDbType.NText);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = statement;

            arParams[2] = new SqlCeParameter("@LastModUtc", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new SqlCeParameter("@LastModBy", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastModBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SavedQuery table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SavedQuery ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Id", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SavedQuery table.
        /// </summary>
        /// <param name="id"> id </param>
        public static IDataReader GetOne(Guid id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SavedQuery ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Id = @Id ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Id", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SavedQuery table.
        /// </summary>
        /// <param name="name"> name </param>
        public static IDataReader GetOne(string name)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SavedQuery ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("[Name] = @Name ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Name", SqlDbType.NVarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = name;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SavedQuery table.
        /// </summary>
        public static IDataReader GetAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SavedQuery ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Name] ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = applicationId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);
        }
    }
}
