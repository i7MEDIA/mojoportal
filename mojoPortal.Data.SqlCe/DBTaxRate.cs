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
    public static class DBTaxRate
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid modifiedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaxRate ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("GeoZoneGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("Priority, ");
            sqlCommand.Append("Rate, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("ModifiedBy ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@GeoZoneGuid, ");
            sqlCommand.Append("@TaxClassGuid, ");
            sqlCommand.Append("@Priority, ");
            sqlCommand.Append("@Rate, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@Created, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModified, ");
            sqlCommand.Append("@ModifiedBy ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = geoZoneGuid;

            arParams[3] = new SqlCeParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = taxClassGuid;

            arParams[4] = new SqlCeParameter("@Priority", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = priority;

            arParams[5] = new SqlCeParameter("@Rate", SqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = rate;

            arParams[6] = new SqlCeParameter("@Description", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = description;

            arParams[7] = new SqlCeParameter("@Created", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = created;

            arParams[8] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = createdBy;

            arParams[9] = new SqlCeParameter("@LastModified", SqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = lastModified;

            arParams[10] = new SqlCeParameter("@ModifiedBy", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = modifiedBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime lastModified,
            Guid modifiedBy)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_TaxRate ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("GeoZoneGuid = @GeoZoneGuid, ");
            sqlCommand.Append("TaxClassGuid = @TaxClassGuid, ");
            sqlCommand.Append("Priority = @Priority, ");
            sqlCommand.Append("Rate = @Rate, ");
            sqlCommand.Append("Description = @Description, ");
            
            sqlCommand.Append("LastModified = @LastModified, ");
            sqlCommand.Append("ModifiedBy = @ModifiedBy ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[8];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = geoZoneGuid;

            arParams[2] = new SqlCeParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = taxClassGuid;

            arParams[3] = new SqlCeParameter("@Priority", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = priority;

            arParams[4] = new SqlCeParameter("@Rate", SqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = rate;

            arParams[5] = new SqlCeParameter("@Description", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = description;

            arParams[6] = new SqlCeParameter("@LastModified", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastModified;

            arParams[7] = new SqlCeParameter("@ModifiedBy", SqlDbType.UniqueIdentifier);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = modifiedBy;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_TaxRate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaxRate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetTaxRates(
            Guid siteGuid,
            Guid geoZoneGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_TaxRate ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND [GeoZoneGuid] = @GeoZoneGuid ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("[Priority] ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = geoZoneGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Inserts a row in the mp_TaxRateHistory table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="taxRateGuid"> taxRateGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="taxClassGuid"> taxClassGuid </param>
        /// <param name="priority"> priority </param>
        /// <param name="rate"> rate </param>
        /// <param name="description"> description </param>
        /// <param name="created"> created </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="lastModified"> lastModified </param>
        /// <param name="modifiedBy"> modifiedBy </param>
        /// <param name="logTime"> logTime </param>
        /// <returns>int</returns>
        public static int AddHistory(
            Guid guid,
            Guid taxRateGuid,
            Guid siteGuid,
            Guid geoZoneGuid,
            Guid taxClassGuid,
            int priority,
            decimal rate,
            string description,
            DateTime created,
            Guid createdBy,
            DateTime lastModified,
            Guid modifiedBy,
            DateTime logTime)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_TaxRateHistory ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("TaxRateGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("GeoZoneGuid, ");
            sqlCommand.Append("TaxClassGuid, ");
            sqlCommand.Append("Priority, ");
            sqlCommand.Append("Rate, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("LastModified, ");
            sqlCommand.Append("ModifiedBy, ");
            sqlCommand.Append("LogTime ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@TaxRateGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@GeoZoneGuid, ");
            sqlCommand.Append("@TaxClassGuid, ");
            sqlCommand.Append("@Priority, ");
            sqlCommand.Append("@Rate, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@Created, ");
            sqlCommand.Append("@CreatedBy, ");
            sqlCommand.Append("@LastModified, ");
            sqlCommand.Append("@ModifiedBy, ");
            sqlCommand.Append("@LogTime ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[13];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@TaxRateGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = taxRateGuid;

            arParams[2] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid;

            arParams[3] = new SqlCeParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = geoZoneGuid;

            arParams[4] = new SqlCeParameter("@TaxClassGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = taxClassGuid;

            arParams[5] = new SqlCeParameter("@Priority", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = priority;

            arParams[6] = new SqlCeParameter("@Rate", SqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = rate;

            arParams[7] = new SqlCeParameter("@Description", SqlDbType.NVarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = description;

            arParams[8] = new SqlCeParameter("@Created", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = created;

            arParams[9] = new SqlCeParameter("@CreatedBy", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = createdBy;

            arParams[10] = new SqlCeParameter("@LastModified", SqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = lastModified;

            arParams[11] = new SqlCeParameter("@ModifiedBy", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = modifiedBy;

            arParams[12] = new SqlCeParameter("@LogTime", SqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = logTime;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

    }
}
