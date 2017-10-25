

// Author:					Jamie Eubanks / i7MEDIA
// Created:					2015-8-14
// Last Modified:			2015-8-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
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

   public static class DBCalendarEventContact
   {

      /// <summary>
      /// Inserts a row in the sts_calendareventcontact table. Returns new integer id.
      /// </summary>
      /// <param name="itemID"> itemID </param>
      /// <param name="name"> name </param>
      /// <param name="phone"> phone </param>
      /// <param name="email"> email </param>
      /// <returns>int</returns>
      public static int Create(
         int itemID,
         string name,
         string phone,
         string email)
      {
         #region Bit Conversion

         #endregion

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("INSERT INTO sts_calendareventcontact (");
         sqlCommand.Append("ItemID, ");
         sqlCommand.Append("Name, ");
         sqlCommand.Append("Phone, ");
         sqlCommand.Append("Email )");

         sqlCommand.Append(" VALUES (");
         sqlCommand.Append("?ItemID, ");
         sqlCommand.Append("?Name, ");
         sqlCommand.Append("?Phone, ");
         sqlCommand.Append("?Email )");
         sqlCommand.Append(";");

         sqlCommand.Append("SELECT LAST_INSERT_ID();");

         MySqlParameter[] arParams = new MySqlParameter[4];

         arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = itemID;

         arParams[1] = new MySqlParameter("?Name", MySqlDbType.VarChar, 100);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = name;

         arParams[2] = new MySqlParameter("?Phone", MySqlDbType.VarChar, 15);
         arParams[2].Direction = ParameterDirection.Input;
         arParams[2].Value = phone;

         arParams[3] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
         arParams[3].Direction = ParameterDirection.Input;
         arParams[3].Value = email;

         int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams).ToString());
         return newID;
      }


      /// <summary>
      /// Updates a row in the sts_calendareventcontact table. Returns true if row updated.
      /// </summary>
      /// <param name="categoryContactID"> categoryContactID </param>
      /// <param name="itemID"> itemID </param>
      /// <param name="name"> name </param>
      /// <param name="phone"> phone </param>
      /// <param name="email"> email </param>
      /// <returns>bool</returns>
      public static bool Update(
         int categoryContactID,
         int itemID,
         string name,
         string phone,
         string email)
      {
         #region Bit Conversion


         #endregion

         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("UPDATE sts_calendareventcontact ");
         sqlCommand.Append("SET  ");
         sqlCommand.Append("ItemID = ?ItemID, ");
         sqlCommand.Append("Name = ?Name, ");
         sqlCommand.Append("Phone = ?Phone, ");
         sqlCommand.Append("Email = ?Email ");

         sqlCommand.Append("WHERE  ");
         sqlCommand.Append("CategoryContactID = ?CategoryContactID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[5];

         arParams[0] = new MySqlParameter("?CategoryContactID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = categoryContactID;

         arParams[1] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = itemID;

         arParams[2] = new MySqlParameter("?Name", MySqlDbType.VarChar, 100);
         arParams[2].Direction = ParameterDirection.Input;
         arParams[2].Value = name;

         arParams[3] = new MySqlParameter("?Phone", MySqlDbType.VarChar, 15);
         arParams[3].Direction = ParameterDirection.Input;
         arParams[3].Value = phone;

         arParams[4] = new MySqlParameter("?Email", MySqlDbType.VarChar, 100);
         arParams[4].Direction = ParameterDirection.Input;
         arParams[4].Value = email;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);

         return (rowsAffected > -1);

      }

      /// <summary>
      /// Deletes a row from the sts_calendareventcontact table. Returns true if row deleted.
      /// </summary>
      /// <param name="categoryContactID"> categoryContactID </param>
      /// <returns>bool</returns>
      public static bool Delete(
         int categoryContactID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("DELETE FROM sts_calendareventcontact ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("CategoryContactID = ?CategoryContactID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?CategoryContactID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = categoryContactID;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);
         return (rowsAffected > 0);
      }

      /// <summary>
      /// Deletes all rows from the sts_calendareventcontact table for the parent key. Returns true if row deleted.
      /// </summary>
      /// <param name="itemID">Shared parent table key.</param>
      public static int DeleteForParent(int itemID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("DELETE FROM sts_calendareventcontact ");
         sqlCommand.Append("WHERE ItemID = ?ItemID");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = itemID;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);
         return (rowsAffected);
      }


      /// <summary>
      /// Gets an IDataReader with one row from the sts_calendareventcontact table.
      /// </summary>
      /// <param name="categoryContactID"> categoryContactID </param>
      public static IDataReader GetOne(
         int categoryContactID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendareventcontact ");
         sqlCommand.Append("WHERE ");
         sqlCommand.Append("CategoryContactID = ?CategoryContactID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?CategoryContactID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = categoryContactID;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);

      }

      /// <summary>
      /// Gets an IDataReader with all rows in the sts_calendareventcontact table.
      /// </summary>
      public static IDataReader GetAll()
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendareventcontact ");
         sqlCommand.Append(";");

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            null);
      }

      /// <summary>
      /// Gets all rows in the sts_calendareventcontact table for the parent key..
      /// </summary>
      /// <param name="itemID">Shared parent table key.</param>
      public static IDataReader GetAll(int itemID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  * ");
         sqlCommand.Append("FROM	sts_calendareventcontact ");
         sqlCommand.Append("WHERE ItemID = ?ItemID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = itemID;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);
      }

      /// <summary>
      /// Gets a count of rows in the sts_calendareventcontact table.
      /// </summary>
      public static int GetCount()
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  Count(*) ");
         sqlCommand.Append("FROM	sts_calendareventcontact ");
         sqlCommand.Append(";");

         return Convert.ToInt32(MySqlHelper.ExecuteScalar(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            null));
      }

      /// <summary>
      /// Gets a count of rows in the sts_calendareventcontact table for the parent key..
      /// </summary>
      /// <param name="itemID">Shared parent table key.</param>
      public static int GetCount(int itemID)
      {
         StringBuilder sqlCommand = new StringBuilder();
         sqlCommand.Append("SELECT  Count(*) ");
         sqlCommand.Append("FROM	sts_calendareventcontact ");
         sqlCommand.Append("WHERE ItemID = ?ItemID ");
         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[1];

         arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = itemID;

         int rowsAffected = MySqlHelper.ExecuteNonQuery(
            ConnectionString.GetWriteConnectionString(),
            sqlCommand.ToString(),
            arParams);
         return (rowsAffected);
      }

      /// <summary>
      /// Gets a page of data from the sts_calendareventcontact table.
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
         sqlCommand.Append("FROM	sts_calendareventcontact  ");
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

      /// <summary>
      /// Gets a page of data from the sts_calendareventcontact table.
      /// </summary>
      /// <param name="itemID">Shared parent table key.</param>
      /// <param name="pageNumber">The page number.</param>
      /// <param name="pageSize">Size of the page.</param>
      /// <param name="totalPages">total pages</param>
      public static IDataReader GetPage(
         int itemID,
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
         sqlCommand.Append("FROM	sts_calendareventcontact  ");
         sqlCommand.Append("WHERE ItemID = ?ItemID ");
         //sqlCommand.Append("ORDER BY  ");
         //sqlCommand.Append("  ");
         sqlCommand.Append("LIMIT ?PageSize ");

         if (pageNumber > 1)
         {
            sqlCommand.Append("OFFSET ?OffsetRows ");
         }

         sqlCommand.Append(";");

         MySqlParameter[] arParams = new MySqlParameter[3];

         arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
         arParams[0].Direction = ParameterDirection.Input;
         arParams[0].Value = itemID;

         arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
         arParams[1].Direction = ParameterDirection.Input;
         arParams[1].Value = pageSize;

         arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
         arParams[2].Direction = ParameterDirection.Input;
         arParams[2].Value = pageLowerBound;

         return MySqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            sqlCommand.ToString(),
            arParams);
      }


   }
}


