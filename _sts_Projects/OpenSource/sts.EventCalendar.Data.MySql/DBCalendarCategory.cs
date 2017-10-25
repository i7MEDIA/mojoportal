// Author:					Jamie Eubanks / i7MEDIA
// Created:					2015-8-14
// Last Modified:			2015-8-14
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace sts.Events.Data
{

   public static class DBCalendarCategory
   {

      /// <summary>
      /// Inserts a row in the sts_calendarcategory table. Returns new integer id.
      /// </summary>
      /// <param name="siteID"> siteID </param>
      /// <param name="description"> description </param>
      /// <param name="colorID"> colorID </param>
      /// <param name="authorizedRoles"> authorizedRoles </param>
      /// <param name="notificationRoles"> notificationRoles </param>
      /// <param name="notificationUsers"> notificationUsers </param>
      /// <returns>int</returns>
      public static int Create(
         int siteID,
         string description,
         int colorID,
         string authorizedRoles,
         string notificationRoles,
         string notificationUsers)
      {
         #region Bit Conversion

         #endregion

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("INSERT INTO sts_calendarcategory (");
         sqlCommand.Append("SiteID, ");
         sqlCommand.Append("Description, ");
         sqlCommand.Append("ColorID, ");
         sqlCommand.Append("AuthorizedRoles, ");
         sqlCommand.Append("NotificationRoles, ");
         sqlCommand.Append("NotificationUsers )");

         sqlCommand.Append(" VALUES (");
         sqlCommand.Append("?SiteID, ");
         sqlCommand.Append("?Description, ");
         sqlCommand.Append("?ColorID, ");
         sqlCommand.Append("?AuthorizedRoles, ");
         sqlCommand.Append("?NotificationRoles, ");
         sqlCommand.Append("?NotificationUsers )");
         sqlCommand.Append(";");

         sqlCommand.Append("SELECT LAST_INSERT_ID();");

         MySqlParameter[] arParams = new MySqlParameter[6];

         arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = siteID;

         arParams[1] = new MySqlParameter("?Description", MySqlDbType.VarChar, 60);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = description;

         arParams[2] = new MySqlParameter("?ColorID", MySqlDbType.Int32);
         arParams[2].Direction = ParameterDirection.Input;
         arParams[2].Value = colorID;

         arParams[3] = new MySqlParameter("?AuthorizedRoles", MySqlDbType.Text);
         arParams[3].Direction = ParameterDirection.Input;
         arParams[3].Value = authorizedRoles;

         arParams[4] = new MySqlParameter("?NotificationRoles", MySqlDbType.Text);
         arParams[4].Direction = ParameterDirection.Input;
         arParams[4].Value = notificationRoles;

         arParams[5] = new MySqlParameter("?NotificationUsers", MySqlDbType.Text);
         arParams[5].Direction = ParameterDirection.Input;
         arParams[5].Value = notificationUsers;

         int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams).ToString());
         return newID;

      }


      /// <summary>
      /// Updates a row in the sts_calendarcategory table. Returns true if row updated.
      /// </summary>
      /// <param name="categoryID"> categoryID </param>
      /// <param name="siteID"> siteID </param>
      /// <param name="description"> description </param>
      /// <param name="colorID"> colorID </param>
      /// <param name="authorizedRoles"> authorizedRoles </param>
      /// <param name="notificationRoles"> notificationRoles </param>
      /// <param name="notificationUsers"> notificationUsers </param>
      /// <returns>bool</returns>
      public static bool Update(
         int categoryID,
         int siteID,
         string description,
         int colorID,
         string authorizedRoles,
         string notificationRoles,
         string notificationUsers)
      {
         #region Bit Conversion


         #endregion

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("UPDATE sts_calendarcategory ");
         sqlCommand.Append("SET  ");
         sqlCommand.Append("SiteID = ?SiteID, ");
         sqlCommand.Append("Description = ?Description, ");
         sqlCommand.Append("ColorID = ?ColorID, ");
         sqlCommand.Append("AuthorizedRoles = ?AuthorizedRoles, ");
         sqlCommand.Append("NotificationRoles = ?NotificationRoles, ");
         sqlCommand.Append("NotificationUsers = ?NotificationUsers ");

         sqlCommand.Append("WHERE  ");
         sqlCommand.Append("CategoryID = ?CategoryID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[7];

         arParams[0] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = categoryID;

         arParams[1] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = siteID;

         arParams[2] = new MySqlParameter("?Description", MySqlDbType.VarChar, 60);
         arParams[2].Direction = ParameterDirection.Input;
         arParams[2].Value = description;

         arParams[3] = new MySqlParameter("?ColorID", MySqlDbType.Int32);
         arParams[3].Direction = ParameterDirection.Input;
         arParams[3].Value = colorID;

         arParams[4] = new MySqlParameter("?AuthorizedRoles", MySqlDbType.Text);
         arParams[4].Direction = ParameterDirection.Input;
         arParams[4].Value = authorizedRoles;

         arParams[5] = new MySqlParameter("?NotificationRoles", MySqlDbType.Text);
         arParams[5].Direction = ParameterDirection.Input;
         arParams[5].Value = notificationRoles;

         arParams[6] = new MySqlParameter("?NotificationUsers", MySqlDbType.Text);
         arParams[6].Direction = ParameterDirection.Input;
         arParams[6].Value = notificationUsers;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);

         return (rowsAffected > -1);

      }

      /// <summary>
      /// Deletes a row from the sts_calendarcategory table. Returns true if row deleted.
      /// </summary>
      /// <param name="categoryID"> categoryID </param>
      /// <returns>bool</returns>
      public static bool Delete(
         int categoryID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("DELETE FROM sts_calendarcategory ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("CategoryID = ?CategoryID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = categoryID;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);
         return (rowsAffected > 0);

      }

      /// <summary>
      /// Gets an IDataReader with one row from the sts_calendarcategory table.
      /// </summary>
      /// <param name="categoryID"> categoryID </param>
      public static IDataReader GetOne(
         int categoryID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendarcategory ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("CategoryID = ?CategoryID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?CategoryID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = categoryID;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);

      }

      /// <summary>
      /// Gets an IDataReader with all rows in the sts_calendarcategory table.
      /// </summary>
      public static IDataReader GetAll()
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendarcategory ");
         sqlCommand.Append(";");

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            null);
      }

      /// <summary>
      /// Gets an IDataReader with all rows in the sts_calendarcategory table.
      /// </summary>
      public static IDataReader GetAll(int siteID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendarcategory ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("SiteID = ?SiteID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = siteID;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);
      }

      /// <summary>
      /// Gets a count of rows in the sts_calendarcategory table.
      /// </summary>
      public static int GetCount()
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  Count(*) ");
         sqlCommand.Append("FROM	sts_calendarcategory ");
         sqlCommand.Append(";");

         return Convert.ToInt32(MySqlHelper.ExecuteScalar(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            null));
      }

      /// <summary>
      /// Gets a page of data from the sts_calendarcategory table.
      /// </summary>
      /// <param name="pageNumber">The page number.</param>
      /// <param name="pageSize">Size of the page.</param>
      /// <param name="totalPages">total pages</param>
      public static IDataReader GetPage(
         int pageNumber,
         int pageSize,
         out int totalPages)
      {
         int pageLowerBound = (pageSize * pageNumber) - pageSize;
         totalPages = 1;
         int totalRows = GetCount();

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

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT	* ");
         sqlCommand.Append("FROM	sts_calendarcategory  ");
         //sqlCommand.Append("WHERE  ");
         //sqlCommand.Append("ORDER BY  ");
         //sqlCommand.Append("  ");
         sqlCommand.Append("LIMIT ?PageSize ");

         if (pageNumber > 1)
         {
            sqlCommand.Append("OFFSET ?OffsetRows ");
         }

         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[2];

         arParams[0] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = pageSize;

         arParams[1] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = pageLowerBound;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);
      }
   }
}


