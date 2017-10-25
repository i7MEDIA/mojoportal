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

   public static class DBCalendarCategoryColor
   {

      /// <summary>
      /// Inserts a row in the sts_calendarcategorycolor table. Returns new integer id.
      /// </summary>
      /// <param name="description"> description </param>
      /// <param name="cssclass"> cssclass </param>
      /// <returns>int</returns>
      public static int Create(
         string description,
         string cssclass)
      {
         #region Bit Conversion

         #endregion

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("INSERT INTO sts_calendarcategorycolor (");
         sqlCommand.Append("Description, ");
         sqlCommand.Append("Cssclass )");

         sqlCommand.Append(" VALUES (");
         sqlCommand.Append("?Description, ");
         sqlCommand.Append("?Cssclass )");
         sqlCommand.Append(";");

         sqlCommand.Append("SELECT LAST_INSERT_ID();");

         MySqlParameter[] arParams = new MySqlParameter[2];

         arParams[0] = new MySqlParameter("?Description", MySqlDbType.VarChar, 256);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = description;

         arParams[1] = new MySqlParameter("?Cssclass", MySqlDbType.VarChar, 256);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = cssclass;

         int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams).ToString());
         return newID;

      }


      /// <summary>
      /// Updates a row in the sts_calendarcategorycolor table. Returns true if row updated.
      /// </summary>
      /// <param name="colorID"> colorID </param>
      /// <param name="description"> description </param>
      /// <param name="cssclass"> cssclass </param>
      /// <returns>bool</returns>
      public static bool Update(
         int colorID,
         string description,
         string cssclass)
      {
         #region Bit Conversion


         #endregion

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("UPDATE sts_calendarcategorycolor ");
         sqlCommand.Append("SET  ");
         sqlCommand.Append("Description = ?Description, ");
         sqlCommand.Append("Cssclass = ?Cssclass ");

         sqlCommand.Append("WHERE  ");
         sqlCommand.Append("ColorID = ?ColorID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[3];

         arParams[0] = new MySqlParameter("?ColorID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = colorID;

         arParams[1] = new MySqlParameter("?Description", MySqlDbType.VarChar, 256);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = description;

         arParams[2] = new MySqlParameter("?Cssclass", MySqlDbType.VarChar, 256);
         arParams[2].Direction = ParameterDirection.Input;
         arParams[2].Value = cssclass;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);

         return (rowsAffected > -1);

      }

      /// <summary>
      /// Deletes a row from the sts_calendarcategorycolor table. Returns true if row deleted.
      /// </summary>
      /// <param name="colorID"> colorID </param>
      /// <returns>bool</returns>
      public static bool Delete(
         int colorID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("DELETE FROM sts_calendarcategorycolor ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("ColorID = ?ColorID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?ColorID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = colorID;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);
         return (rowsAffected > 0);

      }

      /// <summary>
      /// Gets an IDataReader with one row from the sts_calendarcategorycolor table.
      /// </summary>
      /// <param name="colorID"> colorID </param>
      public static IDataReader GetOne(
         int colorID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendarcategorycolor ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("ColorID = ?ColorID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?ColorID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = colorID;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);

      }

      /// <summary>
      /// Gets an IDataReader with all rows in the sts_calendarcategorycolor table.
      /// </summary>
      public static IDataReader GetAll()
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendarcategorycolor ");
         sqlCommand.Append(";");

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            null);
      }

      /// <summary>
      /// Gets a count of rows in the sts_calendarcategorycolor table.
      /// </summary>
      public static int GetCount()
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  Count(*) ");
         sqlCommand.Append("FROM	sts_calendarcategorycolor ");
         sqlCommand.Append(";");

         return Convert.ToInt32(MySqlHelper.ExecuteScalar(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            null));
      }

      /// <summary>
      /// Gets a page of data from the sts_calendarcategorycolor table.
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
         sqlCommand.Append("FROM	sts_calendarcategorycolor  ");
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


