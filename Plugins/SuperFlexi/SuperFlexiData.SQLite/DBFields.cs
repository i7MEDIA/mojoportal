// Created:					2018-01-02
// Last Modified:   2018-01-03

using mojoPortal.Data;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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

            string sqlCommand = string.Format("insert into i7_sflexi_fields ({0}) values ({1});"
                ,@"SiteGuid
                  ,FeatureGuid
                  ,DefinitionGuid
                  ,FieldGuid
                  ,DefinitionName
                  ,name
                  ,label
                  ,DefaultValue
                  ,ControlType
                  ,ControlSrc
                  ,DataType
                  ,IsList
                  ,SortOrder
                  ,HelpKey
                  ,Required
                  ,RequiredMessageFormat
                  ,Regex
                  ,RegexMessageFormat
                  ,Token
                  ,Searchable
                  ,EditPageControlWrapperCssClass
                  ,EditPageLabelCssClass
                  ,EditPageControlCssClass
                  ,DatePickerIncludeTimeForDate
                  ,DatePickerShowMonthList
                  ,DatePickerShowYearList
                  ,DatePickerYearRange
                  ,ImageBrowserEmptyUrl
                  ,Options
                  ,CheckBoxReturnBool
                  ,CheckBoxReturnValueWhenTrue
                  ,CheckBoxReturnValueWhenFalse
                  ,DateFormat
                  ,TextBoxMode
                  ,Attributes
				  ,PreTokenString
				  ,PostTokenString
				  ,PreTokenStringWhenTrue
				  ,PostTokenStringWhenTrue
				  ,PreTokenStringWhenFalse
				  ,PostTokenStringWhenFalse
				  ,IsGlobal
				  ,ViewRoles
				  ,EditRoles"
                , @":SiteGuid
                  ,:FeatureGuid
                  ,:DefinitionGuid
                  ,:FieldGuid
                  ,:DefinitionName
                  ,:Name
                  ,:Label
                  ,:DefaultValue
                  ,:ControlType
                  ,:ControlSrc
                  ,:DataType
                  ,:IsList
                  ,:SortOrder
                  ,:HelpKey
                  ,:Required
                  ,:RequiredMessageFormat
                  ,:Regex
                  ,:RegexMessageFormat
                  ,:Token
                  ,:Searchable
                  ,:EditPageControlWrapperCssClass
                  ,:EditPageLabelCssClass
                  ,:EditPageControlCssClass
                  ,:DatePickerIncludeTimeForDate
                  ,:DatePickerShowMonthList
                  ,:DatePickerShowYearList
                  ,:DatePickerYearRange
                  ,:ImageBrowserEmptyUrl
                  ,:Options
                  ,:CheckBoxReturnBool
                  ,:CheckBoxReturnValueWhenTrue
                  ,:CheckBoxReturnValueWhenFalse
                  ,:DateFormat
                  ,:TextBoxMode
                  ,:Attributes
                  ,:PreTokenString
                  ,:PostTokenString
				  ,:PreTokenStringWhenTrue
				  ,:PostTokenStringWhenTrue
				  ,:PreTokenStringWhenFalse
				  ,:PostTokenStringWhenFalse
				  ,:IsGlobal
				  ,:ViewRoles
				  ,:EditRoles");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
                new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() },
                new SqliteParameter(":DefinitionName", DbType.String, 50) { Direction = ParameterDirection.Input, Value = definitionName },
                new SqliteParameter(":Name", DbType.String, 50) { Direction = ParameterDirection.Input, Value = name },
                new SqliteParameter(":Label", DbType.String, 255) { Direction = ParameterDirection.Input, Value = label },
                new SqliteParameter(":DefaultValue", DbType.Object) { Direction = ParameterDirection.Input, Value = defaultValue },
                new SqliteParameter(":ControlType", DbType.String, 16) { Direction = ParameterDirection.Input, Value = controlType },
                new SqliteParameter(":ControlSrc", DbType.String, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
				new SqliteParameter(":DataType", DbType.String, 100) { Direction = ParameterDirection.Input, Value = dataType },
				new SqliteParameter(":IsList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
				new SqliteParameter(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
                new SqliteParameter(":HelpKey", DbType.String, 255) { Direction = ParameterDirection.Input, Value = helpKey },
                new SqliteParameter(":Required", DbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
                new SqliteParameter(":RequiredMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
                new SqliteParameter(":Regex", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regex },
                new SqliteParameter(":RegexMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
                new SqliteParameter(":Token", DbType.String, 50) { Direction = ParameterDirection.Input, Value = token },
                new SqliteParameter(":PreTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenString },
                new SqliteParameter(":PostTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenString },
				new SqliteParameter(":PreTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
				new SqliteParameter(":PostTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
				new SqliteParameter(":PreTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
				new SqliteParameter(":PostTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
				new SqliteParameter(":Searchable", DbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
                new SqliteParameter(":EditPageControlWrapperCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
                new SqliteParameter(":EditPageLabelCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
                new SqliteParameter(":EditPageControlCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
                new SqliteParameter(":DatePickerIncludeTimeForDate", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
                new SqliteParameter(":DatePickerShowMonthList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
                new SqliteParameter(":DatePickerShowYearList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
                new SqliteParameter(":DatePickerYearRange", DbType.String, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
                new SqliteParameter(":ImageBrowserEmptyUrl", DbType.String, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
                new SqliteParameter(":Options", DbType.Object) { Direction = ParameterDirection.Input, Value = options },
                new SqliteParameter(":CheckBoxReturnBool", DbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
                new SqliteParameter(":CheckBoxReturnValueWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
                new SqliteParameter(":CheckBoxReturnValueWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
                new SqliteParameter(":DateFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
                new SqliteParameter(":TextBoxMode", DbType.String, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
                new SqliteParameter(":Attributes", DbType.String, 255) { Direction = ParameterDirection.Input, Value = attributes },
				new SqliteParameter(":IsGlobal", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
                new SqliteParameter(":ViewRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
                new SqliteParameter(":EditRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = editRoles }
            };
            int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParams.ToArray()).ToString());

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
            string sqlCommand = @"update i7_sflexi_fields set 
                  SiteGuid = :SiteGuid
                 ,FeatureGuid = :FeatureGuid
                 ,DefinitionGuid = :DefinitionGuid
                 ,DefinitionName = :DefinitionName
                 ,Name = :Name
                 ,Label = :Label
                 ,DefaultValue = :DefaultValue
                 ,ControlType = :ControlType
                 ,ControlSrc = :ControlSrc
                 ,DataType = :DataType
                 ,IsList = :IsList
                 ,SortOrder = :SortOrder
                 ,HelpKey = :HelpKey
                 ,Required = :Required
                 ,RequiredMessageFormat = :RequiredMessageFormat
                 ,Regex = :Regex
                 ,RegexMessageFormat = :RegexMessageFormat
                 ,Token = :Token
                 ,PreTokenString = :PreTokenString
                 ,PostTokenString = :PostTokenString
				 ,PreTokenStringWhenTrue = :PreTokenStringWhenTrue
				 ,PostTokenStringWhenTrue = :PostTokenStringWhenTrue
				 ,PreTokenStringWhenFalse = :PreTokenStringWhenFalse
				 ,PostTokenStringWhenFalse = :PostTokenStringWhenFalse
				 ,Searchable = :Searchable
                 ,EditPageControlWrapperCssClass = :EditPageControlWrapperCssClass
                 ,EditPageLabelCssClass = :EditPageLabelCssClass
                 ,EditPageControlCssClass = :EditPageControlCssClass
                 ,DatePickerIncludeTimeForDate = :DatePickerIncludeTimeForDate
                 ,DatePickerShowMonthList = :DatePickerShowMonthList
                 ,DatePickerShowYearList = :DatePickerShowYearList
                 ,DatePickerYearRange = :DatePickerYearRange
                 ,ImageBrowserEmptyUrl = :ImageBrowserEmptyUrl
                 ,Options = :Options
                 ,CheckBoxReturnBool = :CheckBoxReturnBool
                 ,CheckBoxReturnValueWhenTrue = :CheckBoxReturnValueWhenTrue
                 ,CheckBoxReturnValueWhenFalse = :CheckBoxReturnValueWhenFalse
                 ,DateFormat = :DateFormat
                 ,TextBoxMode = :TextBoxMode
                 ,Attributes = :Attributes
                 ,IsDeleted = :IsDeleted
                 ,IsGlobal = :IsGlobal
                 ,ViewRoles = :ViewRoles
				 ,EditRoles = :EditRoles
            where FieldGuid = :FieldGuid;";

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
                new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
                new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() },
                new SqliteParameter(":DefinitionName", DbType.String, 50) { Direction = ParameterDirection.Input, Value = definitionName },
                new SqliteParameter(":Name", DbType.String, 50) { Direction = ParameterDirection.Input, Value = name },
                new SqliteParameter(":Label", DbType.String, 255) { Direction = ParameterDirection.Input, Value = label },
                new SqliteParameter(":DefaultValue", DbType.Object) { Direction = ParameterDirection.Input, Value = defaultValue },
                new SqliteParameter(":ControlType", DbType.String, 16) { Direction = ParameterDirection.Input, Value = controlType },
                new SqliteParameter(":ControlSrc", DbType.String, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
				new SqliteParameter(":DataType", DbType.String, 100) { Direction = ParameterDirection.Input, Value = dataType },
				new SqliteParameter(":IsList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
                new SqliteParameter(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
                new SqliteParameter(":HelpKey", DbType.String, 255) { Direction = ParameterDirection.Input, Value = helpKey },
                new SqliteParameter(":Required", DbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
                new SqliteParameter(":RequiredMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
                new SqliteParameter(":Regex", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regex },
                new SqliteParameter(":RegexMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
                new SqliteParameter(":Token", DbType.String, 50) { Direction = ParameterDirection.Input, Value = token },
                new SqliteParameter(":PreTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenString },
                new SqliteParameter(":PostTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenString },
				new SqliteParameter(":PreTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
				new SqliteParameter(":PostTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
				new SqliteParameter(":PreTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
				new SqliteParameter(":PostTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
				new SqliteParameter(":Searchable", DbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
                new SqliteParameter(":EditPageControlWrapperCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
                new SqliteParameter(":EditPageLabelCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
                new SqliteParameter(":EditPageControlCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
                new SqliteParameter(":DatePickerIncludeTimeForDate", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
                new SqliteParameter(":DatePickerShowMonthList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
                new SqliteParameter(":DatePickerShowYearList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
                new SqliteParameter(":DatePickerYearRange", DbType.String, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
                new SqliteParameter(":ImageBrowserEmptyUrl", DbType.String, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
                new SqliteParameter(":Options", DbType.Object) { Direction = ParameterDirection.Input, Value = options },
                new SqliteParameter(":CheckBoxReturnBool", DbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
                new SqliteParameter(":CheckBoxReturnValueWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
                new SqliteParameter(":CheckBoxReturnValueWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
                new SqliteParameter(":DateFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
                new SqliteParameter(":TextBoxMode", DbType.String, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
                new SqliteParameter(":Attributes", DbType.String, 255) { Direction = ParameterDirection.Input, Value = attributes },
                new SqliteParameter(":IsDeleted", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isDeleted },
                new SqliteParameter(":IsGlobal", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
				new SqliteParameter(":ViewRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
				new SqliteParameter(":EditRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = editRoles }
			};
            int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),				
                sqlCommand,
                sqlParams.ToArray()).ToString());

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
            var sqlCommand = "delete from i7_sflexi_fields where FieldGuid = :FieldGuid;";

            var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36) { 
                Direction = ParameterDirection.Input, 
                Value = fieldGuid.ToString() 
            };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_fields table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            var sqlCommand = "delete from i7_sflexi_fields where SiteGuid = :SiteGuid;";

			var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes rows from the i7_sflexi_fields table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByDefinition(Guid definitionGuid)
        {
            var sqlCommand = "delete from i7_sflexi_fields where DefinitionGuid = :DefinitionGuid;";

			var sqlParam = new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the i7_sflexi_fields table.
        /// </summary>
        /// <param name="fieldGuid"> fieldGuid </param>
        public static IDataReader GetOne(
            Guid fieldGuid)
        {
            var sqlCommand = "select * from i7_sflexi_fields where FieldGuid = :FieldGuid;";

			var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam);
        }

        /// <summary>
        /// Gets a count of rows in the i7_sflexi_fields table.
        /// </summary>
        public static int GetCount()
        {
            var sqlCommand = "select count(*) from i7_sflexi_fields;";

            return Convert.ToInt32(SqliteHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
				 sqlCommand));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_fields table.
        /// </summary>
        public static IDataReader GetAll()
        {
            var sqlCommand = "select * from i7_sflexi_fields where IsDeleted = 0;";

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand);
        }

        public static IDataReader GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
        {
            var sqlCommand = @"select * from i7_sflexi_fields 
                where DefinitionGuid = :DefinitionGuid 
                and ((':IncludeDeleted' = 'true') or (IsDeleted = 0)) 
                order by SortOrder, name;";

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
                new SqliteParameter(":IncludeDeleted", DbType.Boolean) { Direction = ParameterDirection.Input, Value = includeDeleted }
            };
            
            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParams.ToArray());
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
                Math.DivRem(totalRows, pageSize, out int remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }
            var sqlCommand = "select * from i7_sflexi_fields limit :PageSize" + (pageNumber > 1 ? "offset :OffsetRows;" : ";");

            var sqlParams = new List<SqliteParameter>
            {
                new SqliteParameter(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
                new SqliteParameter(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
            };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
				sqlCommand,
                sqlParams.ToArray());
        }

        /// <summary>
        /// Marks a field as deleted.
        /// </summary>
        /// <param name="fieldGuid"></param>
        /// <returns></returns>
        public static bool MarkAsDeleted(Guid fieldGuid)
        {
            var sqlCommand = "update i7_sflexi_fields set IsDeleted = 0 where FieldGuid = :FieldGuid;";

            var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36)
            {
                Direction = ParameterDirection.Input,
                Value = fieldGuid
            };

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand,
                sqlParam);

            return (rowsAffected > 0);
        }
    }

}


