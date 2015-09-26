/// Author:					Joe Audette
/// Created:				2008-02-26
/// Last Modified:		    2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace WebStore.Data
{
    public static class DBProductFile
    {
        


        public static int Add(
            Guid productGuid,
            string fileName,
            byte[] fileImage,
            string serverFileName,
            int byteLength,
            DateTime created,
            Guid createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_ProductFile (");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("FileName, ");
            sqlCommand.Append("FileImage, ");
            sqlCommand.Append("ServerFileName, ");
            sqlCommand.Append("ByteLength, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?FileName, ");
            sqlCommand.Append("?FileImage, ");
            sqlCommand.Append("?ServerFileName, ");
            sqlCommand.Append("?ByteLength, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            arParams[1] = new MySqlParameter("?FileName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = fileName;

            arParams[2] = new MySqlParameter("?FileImage", MySqlDbType.LongBlob);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = fileImage;

            arParams[3] = new MySqlParameter("?ServerFileName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = serverFileName;

            arParams[4] = new MySqlParameter("?ByteLength", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = byteLength;

            arParams[5] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = created;

            arParams[6] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool Update(
            Guid productGuid,
            string fileName,
            byte[] fileImage,
            string serverFileName,
            int byteLength,
            DateTime created,
            Guid createdBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_ProductFile ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FileName = ?FileName, ");
            sqlCommand.Append("FileImage = ?FileImage, ");
            sqlCommand.Append("ServerFileName = ?ServerFileName, ");
            sqlCommand.Append("ByteLength = ?ByteLength, ");
            sqlCommand.Append("Created = ?Created, ");
            sqlCommand.Append("CreatedBy = ?CreatedBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ProductGuid = ?ProductGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[7];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            arParams[1] = new MySqlParameter("?FileName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = fileName;

            arParams[2] = new MySqlParameter("?FileImage", MySqlDbType.LongBlob);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = fileImage;

            arParams[3] = new MySqlParameter("?ServerFileName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = serverFileName;

            arParams[4] = new MySqlParameter("?ByteLength", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = byteLength;

            arParams[5] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = created;

            arParams[6] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid productGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_ProductFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ProductGuid = ?ProductGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);
        }

        public static IDataReader Get(Guid productGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_ProductFile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ProductGuid = ?ProductGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
