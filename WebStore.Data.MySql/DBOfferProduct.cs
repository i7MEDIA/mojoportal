/// Author:					Joe Audette
/// Created:				2008-02-25
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
    
    public static class DBOfferProduct
    {
        
        public static int Add(
            Guid guid,
            Guid offerGuid,
            Guid productGuid,
            byte fullfillType,
            Guid fullFillTermsGuid,
            int quantity,
            int sortOrder,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP)
        {
            
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OfferProduct (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("ProductGuid, ");
            sqlCommand.Append("FullfillType, ");
            sqlCommand.Append("FullFillTermsGuid, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("SortOrder, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifiedBy, ");
            sqlCommand.Append("LastModifiedFromIP )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?ProductGuid, ");
            sqlCommand.Append("?FullfillType, ");
            sqlCommand.Append("?FullFillTermsGuid, ");
            sqlCommand.Append("?Quantity, ");
            sqlCommand.Append("?SortOrder, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifiedBy, ");
            sqlCommand.Append("?LastModifiedFromIP )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = offerGuid.ToString();

            arParams[2] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = productGuid.ToString();

            arParams[3] = new MySqlParameter("?FullfillType", MySqlDbType.Int16);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = fullfillType;

            arParams[4] = new MySqlParameter("?FullFillTermsGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = fullFillTermsGuid.ToString();

            arParams[5] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = quantity;

            arParams[6] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sortOrder;

            arParams[7] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = created;

            arParams[8] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdBy.ToString();

            arParams[9] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdFromIP;

            arParams[10] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModified;

            arParams[11] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = lastModifiedBy.ToString();

            arParams[12] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lastModifiedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

          

        }


        public static bool Update(
            Guid guid,
            byte fullfillType,
            Guid fullFillTermsGuid,
            int quantity,
            int sortOrder,
            DateTime lastModified,
            Guid lastModifiedBy,
            string lastModifiedFromIP)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OfferProduct ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("FullfillType = ?FullfillType, ");
            sqlCommand.Append("FullFillTermsGuid = ?FullFillTermsGuid, ");
            sqlCommand.Append("Quantity = ?Quantity, ");
            sqlCommand.Append("SortOrder = ?SortOrder, ");
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastModifiedBy = ?LastModifiedBy, ");
            sqlCommand.Append("LastModifiedFromIP = ?LastModifiedFromIP ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[8];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?FullfillType", MySqlDbType.Int16);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = fullfillType;

            arParams[2] = new MySqlParameter("?FullFillTermsGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = fullFillTermsGuid.ToString();

            arParams[3] = new MySqlParameter("?Quantity", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = quantity;

            arParams[4] = new MySqlParameter("?SortOrder", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = sortOrder;

            arParams[5] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = lastModified;

            arParams[6] = new MySqlParameter("?LastModifiedBy", MySqlDbType.VarChar, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModifiedBy.ToString();

            arParams[7] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModifiedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            string deletedFromIP,
            DateTime deletedTime)
        {
           
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OfferProduct ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("IsDeleted = 1, ");
            sqlCommand.Append("DeletedBy = ?DeletedBy, ");
            sqlCommand.Append("DeletedFromIP = ?DeletedFromIP, ");
            sqlCommand.Append("DeletedTime = ?DeletedTime ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?DeletedBy", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = deletedBy.ToString();

            arParams[2] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = deletedFromIP;

            arParams[3] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = deletedTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
           
        }


        public static bool DeleteByProduct(
            Guid productGuid,
            Guid deletedBy,
            string deletedFromIP,
            DateTime deletedTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OfferProduct ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("IsDeleted = ?IsDeleted, ");
            sqlCommand.Append("DeletedBy = ?DeletedBy, ");
            sqlCommand.Append("DeletedFromIP = ?DeletedFromIP, ");
            sqlCommand.Append("DeletedTime = ?DeletedTime ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ProductGuid = ?ProductGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?ProductGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = productGuid.ToString();

            arParams[1] = new MySqlParameter("?IsDeleted", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = 1;

            arParams[2] = new MySqlParameter("?DeletedBy", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = deletedBy.ToString();

            arParams[3] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = deletedFromIP;

            arParams[4] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = deletedTime;

            
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static IDataReader Get(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_OfferProduct ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
				
        }

        /// <summary>
        /// Gets a count of rows in the ws_OfferProduct table.
        /// </summary>
        public static int GetCountByOffer(Guid offerGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	ws_OfferProduct ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OfferGuid = ?OfferGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = offerGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  op.*, ");
            sqlCommand.Append("p.Name, ");
            sqlCommand.Append("p.Description, ");
            sqlCommand.Append("p.Abstract, ");
            sqlCommand.Append("p.TeaserFile, ");
            sqlCommand.Append("p.TeaserFileLink ");

            sqlCommand.Append("FROM	ws_OfferProduct op ");
            sqlCommand.Append("JOIN ");
            sqlCommand.Append("ws_Product p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("op.ProductGuid = p.Guid ");
            
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("op.OfferGuid = ?OfferGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("op.IsDeleted = 0 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("p.IsDeleted = 0 ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("op.SortOrder ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = offerGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }




    }
}
