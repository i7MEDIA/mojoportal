// Created:					2015-03-06
// Last Modified:			2022-12-16

using System;
using System.Data;
using mojoPortal.Data;

namespace SuperFlexiData
{
    public static class DBFields
    {
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
            string dataType,
            bool isList,
			int sortOrder,
            string helpKey,
            bool required,
            string requiredMessageFormat,
            string regex,
            string regexMessageFormat,
            string token,
            string preTokenString,
            string postTokenString,
			string preTokenStringWhenTrue,
			string postTokenStringWhenTrue,
			string preTokenStringWhenFalse,
			string postTokenStringWhenFalse,
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
            bool isGlobal,
			string viewRoles,
			string editRoles)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_Insert", 44);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@DefinitionName", SqlDbType.NVarChar, 50, ParameterDirection.Input, definitionName);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Label", SqlDbType.NVarChar, 255, ParameterDirection.Input, label);
            sph.DefineSqlParameter("@DefaultValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, defaultValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 25, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, -1, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@DataType", SqlDbType.NVarChar, 100, ParameterDirection.Input, dataType);
			sph.DefineSqlParameter("@IsList", SqlDbType.Bit, ParameterDirection.Input, isList);
			sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@Required", SqlDbType.Bit, ParameterDirection.Input, required);
            sph.DefineSqlParameter("@RequiredMessageFormat", SqlDbType.NVarChar, -1, ParameterDirection.Input, requiredMessageFormat);
            sph.DefineSqlParameter("@Regex", SqlDbType.NVarChar, -1, ParameterDirection.Input, regex);
            sph.DefineSqlParameter("@RegexMessageFormat", SqlDbType.NVarChar, -1, ParameterDirection.Input, regexMessageFormat);
            sph.DefineSqlParameter("@Token", SqlDbType.NVarChar, -1, ParameterDirection.Input, token);
            sph.DefineSqlParameter("@PreTokenString", SqlDbType.NVarChar, -1, ParameterDirection.Input, preTokenString);
            sph.DefineSqlParameter("@PostTokenString", SqlDbType.NVarChar, -1, ParameterDirection.Input, postTokenString);
			sph.DefineSqlParameter("@PreTokenStringWhenTrue", SqlDbType.NVarChar, -1, ParameterDirection.Input, preTokenStringWhenTrue);
			sph.DefineSqlParameter("@PostTokenStringWhenTrue", SqlDbType.NVarChar, -1, ParameterDirection.Input, postTokenStringWhenTrue);
			sph.DefineSqlParameter("@PreTokenStringWhenFalse", SqlDbType.NVarChar, -1, ParameterDirection.Input, preTokenStringWhenFalse);
			sph.DefineSqlParameter("@PostTokenStringWhenFalse", SqlDbType.NVarChar, -1, ParameterDirection.Input, postTokenStringWhenFalse);
			sph.DefineSqlParameter("@Searchable", SqlDbType.Bit, ParameterDirection.Input, searchable);
            sph.DefineSqlParameter("@EditPageControlWrapperCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, editPageControlWrapperCssClass);
            sph.DefineSqlParameter("@EditPageLabelCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, editPageLabelCssClass);
            sph.DefineSqlParameter("@EditPageControlCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, editPageControlCssClass);
            sph.DefineSqlParameter("@DatePickerIncludeTimeForDate", SqlDbType.Bit, ParameterDirection.Input, datePickerIncludeTimeForDate);
            sph.DefineSqlParameter("@DatePickerShowMonthList", SqlDbType.Bit, ParameterDirection.Input, datePickerShowMonthList);
            sph.DefineSqlParameter("@DatePickerShowYearList", SqlDbType.Bit, ParameterDirection.Input, datePickerShowYearList);
            sph.DefineSqlParameter("@DatePickerYearRange", SqlDbType.NVarChar, 10, ParameterDirection.Input, datePickerYearRange);
            sph.DefineSqlParameter("@ImageBrowserEmptyUrl", SqlDbType.NVarChar, -1, ParameterDirection.Input, imageBrowserEmptyUrl);
            sph.DefineSqlParameter("@Options", SqlDbType.NVarChar, -1, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@CheckBoxReturnBool", SqlDbType.Bit, ParameterDirection.Input, checkBoxReturnBool);
            sph.DefineSqlParameter("@CheckBoxReturnValueWhenTrue", SqlDbType.NVarChar, -1, ParameterDirection.Input, checkBoxReturnValueWhenTrue);
            sph.DefineSqlParameter("@CheckBoxReturnValueWhenFalse", SqlDbType.NVarChar, -1, ParameterDirection.Input, checkBoxReturnValueWhenFalse);
            sph.DefineSqlParameter("@DateFormat", SqlDbType.NVarChar, -1, ParameterDirection.Input, dateFormat);
            sph.DefineSqlParameter("@TextBoxMode", SqlDbType.NVarChar, 25, ParameterDirection.Input, textBoxMode);
            sph.DefineSqlParameter("@Attributes", SqlDbType.NVarChar, 100, ParameterDirection.Input, attributes);
			sph.DefineSqlParameter("@IsGlobal", SqlDbType.Bit, ParameterDirection.Input, isGlobal);
            sph.DefineSqlParameter("@ViewRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, viewRoles);
            sph.DefineSqlParameter("@EditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, editRoles);
			int rowsAffected = sph.ExecuteNonQuery();
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
            string dataType,
            bool isList,
			int sortOrder,
            string helpKey,
            bool required,
            string requiredMessageFormat,
            string regex,
            string regexMessageFormat,
            string token,
            string preTokenString,
            string postTokenString,
			string preTokenStringWhenTrue,
			string postTokenStringWhenTrue,
			string preTokenStringWhenFalse,
			string postTokenStringWhenFalse,
			bool searchable,
            string editPageControlWrapperCssClass,
            string editPageLabelCssClass,
            string editPageControlCssClass,
            bool datePickerIncludeTimeForDate,
            bool datePickerShowMonthList,
            bool datePickerShowYearList,
            string datePickerYearRange,
            string imageBrowserEmptyUrl,
            //string iSettingControlSettings,
            string options,
            bool checkBoxReturnBool,
            string checkBoxReturnValueWhenTrue,
            string checkBoxReturnValueWhenFalse,
            string dateFormat,
            string textBoxMode,
            string attributes,
            bool isDeleted,
            bool isGlobal,
			string viewRoles,
			string editRoles)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "i7_sflexi_fields_Update", 45);
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
            sph.DefineSqlParameter("@DefinitionName", SqlDbType.NVarChar, 50, ParameterDirection.Input, definitionName);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Label", SqlDbType.NVarChar, 255, ParameterDirection.Input, label);
            sph.DefineSqlParameter("@DefaultValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, defaultValue);
            sph.DefineSqlParameter("@ControlType", SqlDbType.NVarChar, 25, ParameterDirection.Input, controlType);
            sph.DefineSqlParameter("@ControlSrc", SqlDbType.NVarChar, -1, ParameterDirection.Input, controlSrc);
            sph.DefineSqlParameter("@DataType", SqlDbType.NVarChar, 500, ParameterDirection.Input, dataType);
			sph.DefineSqlParameter("@IsList", SqlDbType.Bit, ParameterDirection.Input, isList);
			sph.DefineSqlParameter("@SortOrder", SqlDbType.Int, ParameterDirection.Input, sortOrder);
            sph.DefineSqlParameter("@HelpKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, helpKey);
            sph.DefineSqlParameter("@Required", SqlDbType.Bit, ParameterDirection.Input, required);
            sph.DefineSqlParameter("@RequiredMessageFormat", SqlDbType.NVarChar, -1, ParameterDirection.Input, requiredMessageFormat);
            sph.DefineSqlParameter("@Regex", SqlDbType.NVarChar, -1, ParameterDirection.Input, regex);
            sph.DefineSqlParameter("@RegexMessageFormat", SqlDbType.NVarChar, -1, ParameterDirection.Input, regexMessageFormat);
            sph.DefineSqlParameter("@Token", SqlDbType.NVarChar, 50, ParameterDirection.Input, token);
            sph.DefineSqlParameter("@PreTokenString", SqlDbType.NVarChar, -1, ParameterDirection.Input, preTokenString);
            sph.DefineSqlParameter("@PostTokenString", SqlDbType.NVarChar, -1, ParameterDirection.Input, postTokenString);
			sph.DefineSqlParameter("@PreTokenStringWhenTrue", SqlDbType.NVarChar, -1, ParameterDirection.Input, preTokenStringWhenTrue);
			sph.DefineSqlParameter("@PostTokenStringWhenTrue", SqlDbType.NVarChar, -1, ParameterDirection.Input, postTokenStringWhenTrue);
			sph.DefineSqlParameter("@PreTokenStringWhenFalse", SqlDbType.NVarChar, -1, ParameterDirection.Input, preTokenStringWhenFalse);
			sph.DefineSqlParameter("@PostTokenStringWhenFalse", SqlDbType.NVarChar, -1, ParameterDirection.Input, postTokenStringWhenFalse);
			sph.DefineSqlParameter("@Searchable", SqlDbType.Bit, ParameterDirection.Input, searchable);
            sph.DefineSqlParameter("@EditPageControlWrapperCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, editPageControlWrapperCssClass);
            sph.DefineSqlParameter("@EditPageLabelCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, editPageLabelCssClass);
            sph.DefineSqlParameter("@EditPageControlCssClass", SqlDbType.NVarChar, 50, ParameterDirection.Input, editPageControlCssClass);
            sph.DefineSqlParameter("@DatePickerIncludeTimeForDate", SqlDbType.Bit, ParameterDirection.Input, datePickerIncludeTimeForDate);
            sph.DefineSqlParameter("@DatePickerShowMonthList", SqlDbType.Bit, ParameterDirection.Input, datePickerShowMonthList);
            sph.DefineSqlParameter("@DatePickerShowYearList", SqlDbType.Bit, ParameterDirection.Input, datePickerShowYearList);
            sph.DefineSqlParameter("@DatePickerYearRange", SqlDbType.NVarChar, 10, ParameterDirection.Input, datePickerYearRange);
            sph.DefineSqlParameter("@ImageBrowserEmptyUrl", SqlDbType.NVarChar, -1, ParameterDirection.Input, imageBrowserEmptyUrl);
            //sph.DefineSqlParameter("@ISettingControlSettings", SqlDbType.NVarChar, -1, ParameterDirection.Input, iSettingControlSettings);
            sph.DefineSqlParameter("@Options", SqlDbType.NVarChar, -1, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@CheckBoxReturnBool", SqlDbType.Bit, ParameterDirection.Input, checkBoxReturnBool);
            sph.DefineSqlParameter("@CheckBoxReturnValueWhenTrue", SqlDbType.NVarChar, -1, ParameterDirection.Input, checkBoxReturnValueWhenTrue);
            sph.DefineSqlParameter("@CheckBoxReturnValueWhenFalse", SqlDbType.NVarChar, -1, ParameterDirection.Input, checkBoxReturnValueWhenFalse);
            sph.DefineSqlParameter("@DateFormat", SqlDbType.NVarChar, -1, ParameterDirection.Input, dateFormat);
            sph.DefineSqlParameter("@TextBoxMode", SqlDbType.NVarChar, 25, ParameterDirection.Input, textBoxMode);
            sph.DefineSqlParameter("@Attributes", SqlDbType.NVarChar, 100, ParameterDirection.Input, attributes);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@IsGlobal", SqlDbType.Bit, ParameterDirection.Input, isGlobal);
			sph.DefineSqlParameter("@ViewRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, viewRoles);
			sph.DefineSqlParameter("@EditRoles", SqlDbType.NVarChar, -1, ParameterDirection.Input, editRoles);
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
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
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
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
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
            sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
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
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
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
            sph.DefineSqlParameter("@DefinitionGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, definitionGuid);
            sph.DefineSqlParameter("@IncludeDeleted", SqlDbType.Bit, ParameterDirection.Input, includeDeleted);
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
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
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
            sph.DefineSqlParameter("@FieldGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, fieldGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
    }

}


