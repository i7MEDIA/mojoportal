// Author:					Joe Audette
// Created:					2009-12-24
// Last Modified:			2009-12-24
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
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBSavedQuery
    {
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["FirebirdWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["FirebirdWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            FbParameter[] arParams = new FbParameter[7];


            arParams[0] = new FbParameter("@Id", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id.ToString();

            arParams[1] = new FbParameter("@Name", FbDbType.VarChar, 50);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new FbParameter("@Statement", FbDbType.VarChar);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = statement;

            arParams[3] = new FbParameter("@CreatedUtc", FbDbType.TimeStamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdUtc;

            arParams[4] = new FbParameter("@CreatedBy", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdBy.ToString();

            arParams[5] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdUtc;

            arParams[6] = new FbParameter("@LastModBy", FbDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy.ToString();


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SavedQuery (");
            sqlCommand.Append("Id, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Statement, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy )");


            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Id, ");
            sqlCommand.Append("@Name, ");
            sqlCommand.Append("@Statement, ");
            sqlCommand.Append("@CreatedUtc, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModUtc, ");
            sqlCommand.Append("@LastModBy )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[4];

            arParams[0] = new FbParameter("@Id", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id.ToString();

            arParams[1] = new FbParameter("@Statement", FbDbType.VarChar);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = statement;

            arParams[2] = new FbParameter("@LastModUtc", FbDbType.TimeStamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new FbParameter("@LastModBy", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = lastModBy.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Id", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Id", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SavedQuery table.
        /// </summary>
        /// <param name="id"> id </param>
        public static IDataReader GetOne(string name)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SavedQuery ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Name = @Name ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Name", FbDbType.VarChar, 50);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = name;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
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
            sqlCommand.Append("Name ");
            sqlCommand.Append(";");

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

        }


        
    }
}
