// Author:					i7MEDIA
// Created:					2015-3-6
// Last Modified:			2015-5-1
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;
using MySql.Data.MySqlClient;

namespace SuperFlexiData
{

    public static class DBFields
    {

        private static string insertFormat = "INSERT INTO {0} ({1}) VALUES ({2});";

        /// <summary>
        /// Inserts a row in the i7_sflexi_fields table. Returns rows affected count.
        /// </summary>
        /// <returns>int</returns>
        public static int Create(
            Guid siteGuid,
            Guid featureGuid,
            Guid definitionGuid,
            Guid fieldGuid,
            string definitionName,
            string name,
            string label,
            string defaultValue,
            string controlType,
            string controlSrc,
            int sortOrder,
            string helpKey,
            bool required,
            string requiredMessageFormat,
            string regex,
            string regexMessageFormat,
            string token,
            bool searchable,
            string editPageControlWrapperCssClass,
            string editPageLabelCssClass,
            string editPageControlCssClass,
            bool datePickerIncludeTimeForDate,
            bool datePickerShowMonthList,
            bool datePickerShowYearList,
            string datePickerYearRange,
            string imageBrowserEmptyUrl,
            string options,
            bool checkBoxReturnBool,
            string checkBoxReturnValueWhenTrue,
            string checkBoxReturnValueWhenFalse,
            string dateFormat,
            string textBoxMode,
            string attributes)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat(insertFormat,
                "i7_sflexi_fields",
                "SiteGuid,"
                + "FeatureGuid,"
                + "DefinitionGuid,"
                + "FieldGuid,"
                + "DefinitionName,"
                + "Name,"
                + "Label,"
                + "DefaultValue,"
                + "ControlType,"
                + "ControlSrc,"
                + "SortOrder,"
                + "HelpKey,"
                + "Required,"
                + "RequiredMessageFormat,"
                + "Regex,"
                + "RegexMessageFormat,"
                + "Token,"
                + "Searchable,"
                + "EditPageControlWrapperCssClass,"
                + "EditPageLabelCssClass,"
                + "EditPageControlCssClass,"
                + "DatePickerIncludeTimeForDate,"
                + "DatePickerShowMonthList,"
                + "DatePickerShowYearList,"
                + "DatePickerYearRange,"
                + "ImageBrowserEmptyUrl,"
                + "Options,"
                + "CheckBoxReturnBool,"
                + "CheckBoxReturnValueWhenTrue,"
                + "CheckBoxReturnValueWhenFalse,"
                + "DateFormat,"
                + "TextBoxMode,"
                + "Attributes",
                "?SiteGuid,"
                + "?FeatureGuid,"
                + "?DefinitionGuid,"
                + "?FieldGuid,"
                + "?DefinitionName,"
                + "?Name,"
                + "?Label,"
                + "?DefaultValue,"
                + "?ControlType,"
                + "?ControlSrc,"
                + "?SortOrder,"
                + "?HelpKey,"
                + "?Required,"
                + "?RequiredMessageFormat,"
                + "?Regex,"
                + "?RegexMessageFormat,"
                + "?Token,"
                + "?Searchable,"
                + "?EditPageControlWrapperCssClass,"
                + "?EditPageLabelCssClass,"
                + "?EditPageControlCssClass,"
                + "?DatePickerIncludeTimeForDate,"
                + "?DatePickerShowMonthList,"
                + "?DatePickerShowYearList,"
                + "?DatePickerYearRange,"
                + "?ImageBrowserEmptyUrl,"
                + "?Options,"
                + "?CheckBoxReturnBool,"
                + "?CheckBoxReturnValueWhenTrue,"
                + "?CheckBoxReturnValueWhenFalse,"
                + "?DateFormat,"
                + "?TextBoxMode,"
                + "?Attributes");
            MySqlParameter[] arParams = new MySqlParameter[33];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.Guid);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new MySqlParameter("?FeatureGuid", MySqlDbType.Guid);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = featureGuid;

            arParams[2] = new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = definitionGuid;

            arParams[3] = new MySqlParameter("?FieldGuid", MySqlDbType.Guid);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = fieldGuid;
            
            arParams[4] = new MySqlParameter("?DefinitionName", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = definitionName;

            arParams[5] = new MySqlParameter("?Name", MySqlDbType.VarChar, 50);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = name;

            arParams[6] = new MySqlParameter("?Label", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = label;
            
            arParams[7] = new MySqlParameter("?DefaultValue", MySqlDbType.Text); 
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = defaultValue;

            arParams[8] = new MySqlParameter("?ControlType", MySqlDbType.VarChar, 16); 
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = controlType;

            arParams[9] = new MySqlParameter("?ControlSrc", MySqlDbType.Text); 
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = controlSrc;

            arParams[10] = new MySqlParameter("?SortOrder", MySqlDbType.Int32); 
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = sortOrder;

            arParams[11] = new MySqlParameter("?HelpKey", MySqlDbType.VarChar, 255); 
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = helpKey;

            arParams[12] = new MySqlParameter("?Required", MySqlDbType.Bit); 
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = required;

            arParams[13] = new MySqlParameter("?RequiredMessageFormat", MySqlDbType.Text); 
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = requiredMessageFormat;

            arParams[14] = new MySqlParameter("?Regex", MySqlDbType.Text); 
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = regex;

            arParams[15] = new MySqlParameter("?RegexMessageFormat", MySqlDbType.Text); 
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = regexMessageFormat;

            arParams[16] = new MySqlParameter("?Token", MySqlDbType.VarChar, 50); 
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = token;

            arParams[17] = new MySqlParameter("?Searchable", MySqlDbType.Bit); 
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = searchable;

            arParams[18] = new MySqlParameter("?EditPageControlWrapperCssClass", MySqlDbType.VarChar, 50); 
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = editPageControlWrapperCssClass;

            arParams[19] = new MySqlParameter("?EditPageLabelCssClass", MySqlDbType.VarChar, 50); 
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = editPageLabelCssClass;

            arParams[20] = new MySqlParameter("?EditPageControlCssClass", MySqlDbType.VarChar, 50); 
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = editPageControlCssClass;

            arParams[21] = new MySqlParameter("?DatePickerIncludeTimeForDate", MySqlDbType.Bit); 
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = datePickerIncludeTimeForDate;

            arParams[22] = new MySqlParameter("?DatePickerShowMonthList", MySqlDbType.Bit); 
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = datePickerShowMonthList;

            arParams[23] = new MySqlParameter("?DatePickerShowYearList", MySqlDbType.Bit); 
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = datePickerShowYearList;

            arParams[24] = new MySqlParameter("?DatePickerYearRange", MySqlDbType.VarChar, 10); 
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = datePickerYearRange;

            arParams[25] = new MySqlParameter("?ImageBrowserEmptyUrl", MySqlDbType.Text); 
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = imageBrowserEmptyUrl;

            arParams[26] = new MySqlParameter("?Options", MySqlDbType.Text); 
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = options;

            arParams[27] = new MySqlParameter("?CheckBoxReturnBool", MySqlDbType.Bit); 
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = checkBoxReturnBool;


            arParams[28] = new MySqlParameter("?CheckBoxReturnValueWhenTrue", MySqlDbType.Text); 
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = checkBoxReturnValueWhenTrue;

            arParams[29] = new MySqlParameter("?CheckBoxReturnValueWhenFalse", MySqlDbType.Text); 
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = checkBoxReturnValueWhenFalse;

            arParams[30] = new MySqlParameter("?DateFormat", MySqlDbType.Text); 
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = dateFormat;

            arParams[31] = new MySqlParameter("?TextBoxMode", MySqlDbType.VarChar, 25); 
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = textBoxMode;

            arParams[32] = new MySqlParameter("?Attributes", MySqlDbType.VarChar, 100); 
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = attributes;


            int rowsAffected = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the i7_sflexi_fields table. Returns true if row updated.
        /// </summary>
        public static bool Update(
            Guid siteGuid,
            Guid featureGuid,
            Guid definitionGuid,
            Guid fieldGuid,
            string definitionName,
            string name,
            string label,
            string defaultValue,
            string controlType,
            string controlSrc,
            int sortOrder,
            string helpKey,
            bool required,
            string requiredMessageFormat,
            string regex,
            string regexMessageFormat,
            string token,
            bool searchable,
            string editPageControlWrapperCssClass,
            string editPageLabelCssClass,
            string editPageControlCssClass,
            bool datePickerIncludeTimeForDate,
            bool datePickerShowMonthList,
            bool datePickerShowYearList,
            string datePickerYearRange,
            string imageBrowserEmptyUrl,
            string options,
            bool checkBoxReturnBool,
            string checkBoxReturnValueWhenTrue,
            string checkBoxReturnValueWhenFalse,
            string dateFormat,
            string textBoxMode,
            string attributes,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_Update", 34);
            sph.DefineSqlParameter("@FieldGuid", MySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@SiteGuid", MySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", MySqlDbType.Guid, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@DefinitionGuid", MySqlDbType.Guid, ParameterDirection.Input, definitionGuid);
            sph.DefineSqlParameter("@DefinitionName", MySqlDbType.VarChar, 50, ParameterDirection.Input, definitionName);
            sph.DefineSqlParameter("@Name", MySqlDbType.VarChar, 50, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Label", MySqlDbType.VarChar, 255, ParameterDirection.Input, label);
            sph.DefineSqlParameter("@DefaultValue", MySqlDbType.Text, ParameterDirection.Input, defaultValue);
            sph.DefineSqlParameter("@ControlType", MySqlDbType.VarChar, 16, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@ControlSrc", MySqlDbType.Text, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@SortOrder", MySqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@HelpKey", MySqlDbType.VarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@Required", MySqlDbType.Bit, ParameterDirection.Input, required);
            sph.DefineSqlParameter("@RequiredMessageFormat", MySqlDbType.Text, ParameterDirection.Input, requiredMessageFormat);
            sph.DefineSqlParameter("@Regex", MySqlDbType.Text, ParameterDirection.Input, regex);
            sph.DefineSqlParameter("@RegexMessageFormat", MySqlDbType.Text, ParameterDirection.Input, regexMessageFormat);
            sph.DefineSqlParameter("@Token", MySqlDbType.VarChar, 50, ParameterDirection.Input, token);
            sph.DefineSqlParameter("@Searchable", MySqlDbType.Bit, ParameterDirection.Input, searchable);
            sph.DefineSqlParameter("@EditPageControlWrapperCssClass", MySqlDbType.VarChar, 50, ParameterDirection.Input, editPageControlWrapperCssClass);
            sph.DefineSqlParameter("@EditPageLabelCssClass", MySqlDbType.VarChar, 50, ParameterDirection.Input, editPageLabelCssClass);
            sph.DefineSqlParameter("@EditPageControlCssClass", MySqlDbType.VarChar, 50, ParameterDirection.Input, editPageControlCssClass);
            sph.DefineSqlParameter("@DatePickerIncludeTimeForDate", MySqlDbType.Bit, ParameterDirection.Input, datePickerIncludeTimeForDate);
            sph.DefineSqlParameter("@DatePickerShowMonthList", MySqlDbType.Bit, ParameterDirection.Input, datePickerShowMonthList);
            sph.DefineSqlParameter("@DatePickerShowYearList", MySqlDbType.Bit, ParameterDirection.Input, datePickerShowYearList);
            sph.DefineSqlParameter("@DatePickerYearRange", MySqlDbType.VarChar, 10, ParameterDirection.Input, datePickerYearRange);
            sph.DefineSqlParameter("@ImageBrowserEmptyUrl", MySqlDbType.Text, ParameterDirection.Input, imageBrowserEmptyUrl);
            sph.DefineSqlParameter("@Options", MySqlDbType.Text, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@CheckBoxReturnBool", MySqlDbType.Bit, ParameterDirection.Input, checkBoxReturnBool);
            sph.DefineSqlParameter("@CheckBoxReturnValueWhenTrue", MySqlDbType.Text, ParameterDirection.Input, checkBoxReturnValueWhenTrue);
            sph.DefineSqlParameter("@CheckBoxReturnValueWhenFalse", MySqlDbType.Text, ParameterDirection.Input, checkBoxReturnValueWhenFalse);
            sph.DefineSqlParameter("@DateFormat", MySqlDbType.Text, ParameterDirection.Input, dateFormat);
            sph.DefineSqlParameter("@TextBoxMode", MySqlDbType.VarChar, 25, ParameterDirection.Input, textBoxMode);
            sph.DefineSqlParameter("@Attributes", MySqlDbType.VarChar, 100, ParameterDirection.Input, attributes);
            sph.DefineSqlParameter("@IsDeleted", MySqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the i7_sflexi_fields table. Returns true if row deleted.
        /// </summary>
        /// <param name="fieldGuid"> fieldGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_Delete", 1);
            sph.DefineSqlParameter("@FieldGuid", MySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_fields table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteGuid", MySqlDbType.Guid, ParameterDirection.Input, siteGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_fields table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByDefinition(Guid definitionGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_DeleteByDefinition", 1);
            sph.DefineSqlParameter("@DefinitionGuid", MySqlDbType.Guid, ParameterDirection.Input, definitionGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_fields table.
        /// </summary>
        /// <param name="fieldGuid"> fieldGuid </param>
        public static IDataReader GetOne(
            Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_fields_SelectOne", 1);
            sph.DefineSqlParameter("@FieldGuid", MySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the i7_sflexi_fields table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "i7_sflexi_fields_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_fields table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "i7_sflexi_fields_SelectAll",
                null);

        }

        public static IDataReader GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_fields_SelectAllForDefinition", 2);
            sph.DefineSqlParameter("@DefinitionGuid", MySqlDbType.Guid, ParameterDirection.Input, definitionGuid);
            sph.DefineSqlParameter("@IncludeDeleted", MySqlDbType.Guid, ParameterDirection.Input, includeDeleted);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a page of data from the i7_sflexi_fields table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount();

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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "i7_sflexi_fields_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", MySqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", MySqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Marks a field as deleted.
        /// </summary>
        /// <param name="fieldGuid"></param>
        /// <returns></returns>
        public static bool MarkAsDeleted(Guid fieldGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_MarkAsDeleted", 1);
            sph.DefineSqlParameter("@FieldGuid", MySqlDbType.Guid, ParameterDirection.Input, fieldGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
    }

}


