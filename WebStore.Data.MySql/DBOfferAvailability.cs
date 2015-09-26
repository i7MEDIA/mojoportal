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
    public static class DBOfferAvailability
    {
        
        public static int Add(
            Guid guid,
            Guid offerGuid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP)
        {
            #region Bit Conversion

            int intRequiresOfferCode;
            if (requiresOfferCode)
            {
                intRequiresOfferCode = 1;
            }
            else
            {
                intRequiresOfferCode = 0;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OfferAvailability (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("BeginUTC, ");
            sqlCommand.Append("EndUTC, ");
            sqlCommand.Append("RequiresOfferCode, ");
            sqlCommand.Append("OfferCode, ");
            sqlCommand.Append("MaxAllowedPerCustomer, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifedBy, ");
            sqlCommand.Append("LastModifedFromIP )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?BeginUTC, ");
            sqlCommand.Append("?EndUTC, ");
            sqlCommand.Append("?RequiresOfferCode, ");
            sqlCommand.Append("?OfferCode, ");
            sqlCommand.Append("?MaxAllowedPerCustomer, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifedBy, ");
            sqlCommand.Append("?LastModifedFromIP )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = offerGuid.ToString();

            arParams[2] = new MySqlParameter("?BeginUTC", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginUtc;

            arParams[3] = new MySqlParameter("?EndUTC", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = endUtc;

            arParams[4] = new MySqlParameter("?RequiresOfferCode", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intRequiresOfferCode;

            arParams[5] = new MySqlParameter("?OfferCode", MySqlDbType.VarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = offerCode;

            arParams[6] = new MySqlParameter("?MaxAllowedPerCustomer", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = maxAllowedPerCustomer;

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

            arParams[11] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = lastModifedBy.ToString();

            arParams[12] = new MySqlParameter("?LastModifedFromIP", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lastModifedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP)
        {
            #region Bit Conversion

            int intRequiresOfferCode;
            if (requiresOfferCode)
            {
                intRequiresOfferCode = 1;
            }
            else
            {
                intRequiresOfferCode = 0;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OfferAvailability ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("BeginUTC = ?BeginUTC, ");
            sqlCommand.Append("EndUTC = ?EndUTC, ");
            sqlCommand.Append("RequiresOfferCode = ?RequiresOfferCode, ");
            sqlCommand.Append("OfferCode = ?OfferCode, ");
            sqlCommand.Append("MaxAllowedPerCustomer = ?MaxAllowedPerCustomer, ");
            sqlCommand.Append("LastModified = ?LastModified, ");
            sqlCommand.Append("LastModifedBy = ?LastModifedBy, ");
            sqlCommand.Append("LastModifedFromIP = ?LastModifedFromIP ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[10];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?BeginUTC", MySqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = beginUtc;

            arParams[2] = new MySqlParameter("?EndUTC", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = endUtc;

            arParams[3] = new MySqlParameter("?RequiresOfferCode", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intRequiresOfferCode;

            arParams[4] = new MySqlParameter("?OfferCode", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = offerCode;

            arParams[5] = new MySqlParameter("?MaxAllowedPerCustomer", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = maxAllowedPerCustomer;

            arParams[7] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = lastModified;

            arParams[8] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModifedBy.ToString();

            arParams[9] = new MySqlParameter("?LastModifedFromIP", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModifedFromIP;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
            
        }


        public static bool Delete(
            Guid guid,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_OfferAvailability ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("IsDeleted = 1, ");
            sqlCommand.Append("DeletedBy = ?DeletedBy, ");
            sqlCommand.Append("DeletedTime = ?DeletedTime, ");
            sqlCommand.Append("DeletedFromIP = ?DeletedFromIP ");
            
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

            arParams[2] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = deletedTime;

            arParams[3] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = deletedFromIP;

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
            sqlCommand.Append("FROM	ws_OfferAvailability ");
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

        public static IDataReader GetByOffer(Guid offerGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	ws_OfferAvailability ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("OfferGuid = ?OfferGuid ");
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


        public static int AddHistory(
            Guid guid,
            Guid availabilityGuid,
            Guid offerGuid,
            DateTime beginUtc,
            DateTime endUtc,
            bool requiresOfferCode,
            string offerCode,
            int maxAllowedPerCustomer,
            DateTime created,
            Guid createdBy,
            string createdFromIP,
            bool isDeleted,
            Guid deletedBy,
            DateTime deletedTime,
            string deletedFromIP,
            DateTime lastModified,
            Guid lastModifedBy,
            string lastModifedFromIP,
            DateTime logTime)
        {
            #region Bit Conversion

            int intRequiresOfferCode;
            if (requiresOfferCode)
            {
                intRequiresOfferCode = 1;
            }
            else
            {
                intRequiresOfferCode = 0;
            }

            int intIsDeleted;
            if (isDeleted)
            {
                intIsDeleted = 1;
            }
            else
            {
                intIsDeleted = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_OfferAvailabilityHistory (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("AvailabilityGuid, ");
            sqlCommand.Append("OfferGuid, ");
            sqlCommand.Append("BeginUTC, ");
            sqlCommand.Append("EndUTC, ");
            sqlCommand.Append("RequiresOfferCode, ");
            sqlCommand.Append("OfferCode, ");
            sqlCommand.Append("MaxAllowedPerCustomer, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("IsDeleted, ");
            sqlCommand.Append("DeletedBy, ");
            sqlCommand.Append("DeletedTime, ");
            sqlCommand.Append("DeletedFromIP, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("LastModifedBy, ");
            sqlCommand.Append("LastModifedFromIP, ");
            sqlCommand.Append("LogTime )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?AvailabilityGuid, ");
            sqlCommand.Append("?OfferGuid, ");
            sqlCommand.Append("?BeginUTC, ");
            sqlCommand.Append("?EndUTC, ");
            sqlCommand.Append("?RequiresOfferCode, ");
            sqlCommand.Append("?OfferCode, ");
            sqlCommand.Append("?MaxAllowedPerCustomer, ");
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?IsDeleted, ");
            sqlCommand.Append("?DeletedBy, ");
            sqlCommand.Append("?DeletedTime, ");
            sqlCommand.Append("?DeletedFromIP, ");
            sqlCommand.Append("?LastModified, ");
            sqlCommand.Append("?LastModifedBy, ");
            sqlCommand.Append("?LastModifedFromIP, ");
            sqlCommand.Append("?LogTime )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[19];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?AvailabilityGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = availabilityGuid.ToString();

            arParams[2] = new MySqlParameter("?OfferGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = offerGuid.ToString();

            arParams[3] = new MySqlParameter("?BeginUTC", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = beginUtc;

            arParams[4] = new MySqlParameter("?EndUTC", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = endUtc;

            arParams[5] = new MySqlParameter("?RequiresOfferCode", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intRequiresOfferCode;

            arParams[6] = new MySqlParameter("?OfferCode", MySqlDbType.VarChar, 50);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = offerCode;

            arParams[7] = new MySqlParameter("?MaxAllowedPerCustomer", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = maxAllowedPerCustomer;

            arParams[8] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = created;

            arParams[9] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy.ToString();

            arParams[10] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = createdFromIP;

            arParams[11] = new MySqlParameter("?IsDeleted", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intIsDeleted;

            arParams[12] = new MySqlParameter("?DeletedBy", MySqlDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = deletedBy.ToString();

            arParams[13] = new MySqlParameter("?DeletedTime", MySqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = deletedTime;

            arParams[14] = new MySqlParameter("?DeletedFromIP", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = deletedFromIP;

            arParams[15] = new MySqlParameter("?LastModified", MySqlDbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = lastModified;

            arParams[16] = new MySqlParameter("?LastModifedBy", MySqlDbType.VarChar, 36);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = lastModifedBy.ToString();

            arParams[17] = new MySqlParameter("?LastModifedFromIP", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = lastModifedFromIP;

            arParams[18] = new MySqlParameter("?LogTime", MySqlDbType.DateTime);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = logTime;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
            
        }


    }
}
