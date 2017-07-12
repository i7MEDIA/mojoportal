///							DBContactFormMessage.cs
/// Author:					
/// Created:				2008-03-29
/// Last Modified:			2012-08-12
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
using Npgsql;


namespace mojoPortal.Data
{
    public static class DBContactFormMessage
    {
        
        /// <summary>
        /// Inserts a row in the mp_ContactFormMessage table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="email"> email </param>
        /// <param name="url"> url </param>
        /// <param name="subject"> subject </param>
        /// <param name="message"> message </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdFromIpAddress"> createdFromIpAddress </param>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            Guid moduleGuid,
            string email,
            string url,
            string subject,
            string message,
            DateTime createdUtc,
            string createdFromIpAddress,
            Guid userGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[10];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = email;

            arParams[4] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new NpgsqlParameter("subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = subject;

            arParams[6] = new NpgsqlParameter("message", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = message;

            arParams[7] = new NpgsqlParameter("createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new NpgsqlParameter("createdfromipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIpAddress;

            arParams[9] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_contactformmessage (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("email, ");
            sqlCommand.Append("url, ");
            sqlCommand.Append("subject, ");
            sqlCommand.Append("message, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("createdfromipaddress, ");
            sqlCommand.Append("userguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":email, ");
            sqlCommand.Append(":url, ");
            sqlCommand.Append(":subject, ");
            sqlCommand.Append(":message, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":createdfromipaddress, ");
            sqlCommand.Append(":userguid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_ContactFormMessage table. Returns true if row updated.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="email"> email </param>
        /// <param name="url"> url </param>
        /// <param name="subject"> subject </param>
        /// <param name="message"> message </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="createdFromIpAddress"> createdFromIpAddress </param>
        /// <param name="userGuid"> userGuid </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid rowGuid,
            Guid siteGuid,
            Guid moduleGuid,
            string email,
            string url,
            string subject,
            string message,
            DateTime createdUtc,
            string createdFromIpAddress,
            Guid userGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[10];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter("siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new NpgsqlParameter("email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = email;

            arParams[4] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = url;

            arParams[5] = new NpgsqlParameter("subject", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = subject;

            arParams[6] = new NpgsqlParameter("message", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = message;

            arParams[7] = new NpgsqlParameter("createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdUtc;

            arParams[8] = new NpgsqlParameter("createdfromipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdFromIpAddress;

            arParams[9] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_contactformmessage ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("siteguid = :siteguid, ");
            sqlCommand.Append("moduleguid = :moduleguid, ");
            sqlCommand.Append("email = :email, ");
            sqlCommand.Append("url = :url, ");
            sqlCommand.Append("subject = :subject, ");
            sqlCommand.Append("message = :message, ");
            sqlCommand.Append("createdutc = :createdutc, ");
            sqlCommand.Append("createdfromipaddress = :createdfromipaddress, ");
            sqlCommand.Append("userguid = :userguid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Deletes a row from the mp_ContactFormMessage table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contactformmessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contactformmessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_contactformmessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid IN (SELECT siteguid FROM mp_sites WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_ContactFormMessage table.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        public static IDataReader GetOne(
            Guid rowGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_contactformmessage ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("rowguid = :rowguid ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //return Common.DBContactFormMessage.GetOne(
            //    rowGuid);
        }


        /// <summary>
        /// Gets a count of rows in the mp_ContactFormMessage table.
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_contactformmessage_count(:moduleguid)",
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the mp_ContactFormMessage table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount(moduleGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
           
            arParams[0] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_contactformmessage_selectpage(:moduleguid,:pagenumber,:pagesize)",
                arParams);

        }
    }
}
