using mojoPortal.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
			string editRoles
		)
		{
			const string sqlCommand = @"
				INSERT INTO `i7_sflexi_fields` (
					`SiteGuid`,
					`FeatureGuid`,
					`DefinitionGuid`,
					`FieldGuid`,
					`DefinitionName`,
					`Name`,
					`Label`,
					`DefaultValue`,
					`ControlType`,
					`ControlSrc`,
					`DataType`,
					`IsList`,
					`SortOrder`,
					`HelpKey`,
					`Required`,
					`RequiredMessageFormat`,
					`Regex`,
					`RegexMessageFormat`,
					`Token`,
					`Searchable`,
					`EditPageControlWrapperCssClass`,
					`EditPageLabelCssClass`,
					`EditPageControlCssClass`,
					`DatePickerIncludeTimeForDate`,
					`DatePickerShowMonthList`,
					`DatePickerShowYearList`,
					`DatePickerYearRange`,
					`ImageBrowserEmptyUrl`,
					`Options`,
					`CheckBoxReturnBool`,
					`CheckBoxReturnValueWhenTrue`,
					`CheckBoxReturnValueWhenFalse`,
					`DateFormat`,
					`TextBoxMode`,
					`Attributes`,
					`PreTokenString`,
					`PostTokenString`,
					`PreTokenStringWhenTrue`,
					`PostTokenStringWhenTrue`,
					`PreTokenStringWhenFalse`,
					`PostTokenStringWhenFalse`,
					`IsGlobal`,
					`ViewRoles`,
					`EditRoles`
				) VALUES (
					?SiteGuid,
					?FeatureGuid,
					?DefinitionGuid,
					?FieldGuid,
					?DefinitionName,
					?Name,
					?Label,
					?DefaultValue,
					?ControlType,
					?ControlSrc,
					?DataType,
					?IsList,
					?SortOrder,
					?HelpKey,
					?Required,
					?RequiredMessageFormat,
					?Regex,
					?RegexMessageFormat,
					?Token,
					?Searchable,
					?EditPageControlWrapperCssClass,
					?EditPageLabelCssClass,
					?EditPageControlCssClass,
					?DatePickerIncludeTimeForDate,
					?DatePickerShowMonthList,
					?DatePickerShowYearList,
					?DatePickerYearRange,
					?ImageBrowserEmptyUrl,
					?Options,
					?CheckBoxReturnBool,
					?CheckBoxReturnValueWhenTrue,
					?CheckBoxReturnValueWhenFalse,
					?DateFormat,
					?TextBoxMode,
					?Attributes,
					?PreTokenString,
					?PostTokenString,
					?PreTokenStringWhenTrue,
					?PostTokenStringWhenTrue,
					?PreTokenStringWhenFalse,
					?PostTokenStringWhenFalse,
					?IsGlobal,
					?ViewRoles,
					?EditRoles
				);";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid },
				new MySqlParameter("?DefinitionName", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = definitionName },
				new MySqlParameter("?Name", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = name },
				new MySqlParameter("?Label", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = label },
				new MySqlParameter("?DefaultValue", MySqlDbType.LongText) { Direction = ParameterDirection.Input, Value = defaultValue },
				new MySqlParameter("?ControlType", MySqlDbType.VarChar, 16) { Direction = ParameterDirection.Input, Value = controlType },
				new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
				new MySqlParameter("?DataType", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Input, Value = dataType },				
				new MySqlParameter("?IsList", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isList },
				new MySqlParameter("?SortOrder", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
				new MySqlParameter("?HelpKey", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
				new MySqlParameter("?Required", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = required },
				new MySqlParameter("?RequiredMessageFormat", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
				new MySqlParameter("?Regex", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = regex },
				new MySqlParameter("?RegexMessageFormat", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
				new MySqlParameter("?Token", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = token },
				new MySqlParameter("?PreTokenString", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenString },
				new MySqlParameter("?PostTokenString", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenString },
				new MySqlParameter("?PreTokenStringWhenTrue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
				new MySqlParameter("?PostTokenStringWhenTrue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
				new MySqlParameter("?PreTokenStringWhenFalse", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
				new MySqlParameter("?PostTokenStringWhenFalse", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
				new MySqlParameter("?Searchable", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = searchable },
				new MySqlParameter("?EditPageControlWrapperCssClass", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
				new MySqlParameter("?EditPageLabelCssClass", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
				new MySqlParameter("?EditPageControlCssClass", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
				new MySqlParameter("?DatePickerIncludeTimeForDate", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
				new MySqlParameter("?DatePickerShowMonthList", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
				new MySqlParameter("?DatePickerShowYearList", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
				new MySqlParameter("?DatePickerYearRange", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
				new MySqlParameter("?ImageBrowserEmptyUrl", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
				new MySqlParameter("?Options", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = options },
				new MySqlParameter("?CheckBoxReturnBool", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
				new MySqlParameter("?CheckBoxReturnValueWhenTrue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
				new MySqlParameter("?CheckBoxReturnValueWhenFalse", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
				new MySqlParameter("?DateFormat", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
				new MySqlParameter("?TextBoxMode", MySqlDbType.VarChar, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
				new MySqlParameter("?Attributes", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = attributes },
				new MySqlParameter("?IsGlobal", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isGlobal },
				new MySqlParameter("?ViewRoles", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
				new MySqlParameter("?EditRoles", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = editRoles }
			};

			int rowsAffected = Convert.ToInt32(
				MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand,
					sqlParams.ToArray()
				)
			);

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
			string editRoles
		)
		{
			const string sqlCommand = @"
				UPDATE
					`i7_sflexi_fields`
				SET
					`SiteGuid` = ?SiteGuid,
					`FeatureGuid` = ?FeatureGuid,
					`DefinitionGuid` = ?DefinitionGuid,
					`DefinitionName` = ?DefinitionName,
					`Name` = ?Name,
					`Label` = ?Label,
					`DefaultValue` = ?DefaultValue,
					`ControlType` = ?ControlType,
					`ControlSrc` = ?ControlSrc,
					`DataType` = ?DataType,
					`IsList` = ?IsList,					
					`SortOrder` = ?SortOrder,
					`HelpKey` = ?HelpKey,
					`Required` = ?Required,
					`RequiredMessageFormat` = ?RequiredMessageFormat,
					`Regex` = ?Regex,
					`RegexMessageFormat` = ?RegexMessageFormat,
					`Token` = ?Token,
					`PreTokenString` = ?PreTokenString,
					`PostTokenString` = ?PostTokenString,
					`PreTokenStringWhenTrue` = ?PreTokenStringWhenTrue,
					`PostTokenStringWhenTrue` = ?PostTokenStringWhenTrue,
					`PreTokenStringWhenFalse` = ?PreTokenStringWhenFalse,
					`PostTokenStringWhenFalse` = ?PostTokenStringWhenFalse,
					`Searchable` = ?Searchable,
					`EditPageControlWrapperCssClass` = ?EditPageControlWrapperCssClass,
					`EditPageLabelCssClass` = ?EditPageLabelCssClass,
					`EditPageControlCssClass` = ?EditPageControlCssClass,
					`DatePickerIncludeTimeForDate` = ?DatePickerIncludeTimeForDate,
					`DatePickerShowMonthList` = ?DatePickerShowMonthList,
					`DatePickerShowYearList` = ?DatePickerShowYearList,
					`DatePickerYearRange` = ?DatePickerYearRange,
					`ImageBrowserEmptyUrl` = ?ImageBrowserEmptyUrl,
					`Options` = ?Options,
					`CheckBoxReturnBool` = ?CheckBoxReturnBool,
					`CheckBoxReturnValueWhenTrue` = ?CheckBoxReturnValueWhenTrue,
					`CheckBoxReturnValueWhenFalse` = ?CheckBoxReturnValueWhenFalse,
					`DateFormat` = ?DateFormat,
					`TextBoxMode` = ?TextBoxMode,
					`Attributes` = ?Attributes,
					`IsDeleted` = ?IsDeleted,
					`IsGlobal` = ?IsGlobal,
					`ViewRoles` = ?ViewRoles,
					`EditRoles` = ?EditRoles
				WHERE
					`FieldGuid` = ?FieldGuid;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?SiteGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new MySqlParameter("?FeatureGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?FieldGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = fieldGuid },
				new MySqlParameter("?DefinitionName", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = definitionName },
				new MySqlParameter("?Name", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = name },
				new MySqlParameter("?Label", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = label },
				new MySqlParameter("?DefaultValue", MySqlDbType.LongText) { Direction = ParameterDirection.Input, Value = defaultValue },
				new MySqlParameter("?ControlType", MySqlDbType.VarChar, 16) { Direction = ParameterDirection.Input, Value = controlType },
				new MySqlParameter("?ControlSrc", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
				new MySqlParameter("?DataType", MySqlDbType.VarChar, 500) { Direction = ParameterDirection.Input, Value = dataType },
				new MySqlParameter("?IsList", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isList },
				new MySqlParameter("?SortOrder", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
				new MySqlParameter("?HelpKey", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
				new MySqlParameter("?Required", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = required },
				new MySqlParameter("?RequiredMessageFormat", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
				new MySqlParameter("?Regex", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = regex },
				new MySqlParameter("?RegexMessageFormat", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
				new MySqlParameter("?Token", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = token },
				new MySqlParameter("?PreTokenString", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenString },
				new MySqlParameter("?PostTokenString", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenString },
				new MySqlParameter("?PreTokenStringWhenTrue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
				new MySqlParameter("?PostTokenStringWhenTrue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
				new MySqlParameter("?PreTokenStringWhenFalse", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
				new MySqlParameter("?PostTokenStringWhenFalse", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
				new MySqlParameter("?Searchable", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = searchable },
				new MySqlParameter("?EditPageControlWrapperCssClass", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
				new MySqlParameter("?EditPageLabelCssClass", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
				new MySqlParameter("?EditPageControlCssClass", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
				new MySqlParameter("?DatePickerIncludeTimeForDate", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
				new MySqlParameter("?DatePickerShowMonthList", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
				new MySqlParameter("?DatePickerShowYearList", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
				new MySqlParameter("?DatePickerYearRange", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
				new MySqlParameter("?ImageBrowserEmptyUrl", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
				new MySqlParameter("?Options", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = options },
				new MySqlParameter("?CheckBoxReturnBool", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
				new MySqlParameter("?CheckBoxReturnValueWhenTrue", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
				new MySqlParameter("?CheckBoxReturnValueWhenFalse", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
				new MySqlParameter("?DateFormat", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
				new MySqlParameter("?TextBoxMode", MySqlDbType.VarChar, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
				new MySqlParameter("?Attributes", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = attributes },
				new MySqlParameter("?IsDeleted", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isDeleted },
				new MySqlParameter("?IsGlobal", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isGlobal },
				new MySqlParameter("?ViewRoles", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
				new MySqlParameter("?EditRoles", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = editRoles }
			};

			int rowsAffected = Convert.ToInt32(
				MySqlHelper.ExecuteNonQuery(
					ConnectionString.GetWriteConnectionString(),
					sqlCommand,
					sqlParams.ToArray()
				).ToString()
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes a row from the i7_sflexi_fields table. Returns true if row deleted.
		/// </summary>
		/// <param name="fieldGuid"> fieldGuid </param>
		/// <returns>bool</returns>
		public static bool Delete(
			Guid fieldGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_fields` WHERE `FieldGuid` = ?FieldGuid;";

			var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = fieldGuid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_fields table. Returns true if rows deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_fields` WHERE `SiteGuid` = ?SiteGuid;";

			var sqlParam = new MySqlParameter("?SiteGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Deletes rows from the i7_sflexi_fields table. Returns true if rows deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByDefinition(Guid definitionGuid)
		{
			const string sqlCommand = "DELETE FROM `i7_sflexi_fields` WHERE `DefinitionGuid` = ?DefinitionGuid;";

			var sqlParam = new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = definitionGuid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		/// <summary>
		/// Gets an IDataReader with one row from the i7_sflexi_fields table.
		/// </summary>
		/// <param name="fieldGuid"> fieldGuid </param>
		public static IDataReader GetOne(Guid fieldGuid)
		{
			const string sqlCommand = "SELECT * FROM `i7_sflexi_fields` WHERE `FieldGuid` = ?FieldGuid;";

			var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = fieldGuid
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);
		}


		/// <summary>
		/// Gets a count of rows in the i7_sflexi_fields table.
		/// </summary>
		public static int GetCount()
		{
			const string sqlCommand = "SELECT Count(*) FROM `i7_sflexi_fields`;";

			return Convert.ToInt32(
				MySqlHelper.ExecuteScalar(
					ConnectionString.GetReadConnectionString(),
					sqlCommand.ToString()
				)
			);
		}


		/// <summary>
		/// Gets an IDataReader with all rows in the i7_sflexi_fields table.
		/// </summary>
		public static IDataReader GetAll()
		{
			const string sqlCommand = "SELECT * FROM `i7_sflexi_fields` WHERE `IsDeleted` = 0;";

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand
			);
		}


		public static IDataReader GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
		{
			const string sqlCommand = @"
				SELECT *
				FROM `i7_sflexi_fields`
				WHERE `DefinitionGuid` = ?DefinitionGuid
				AND ((?IncludeDeleted = 1) OR (`IsDeleted` = 0))
				ORDER BY `SortOrder`, `Name`;";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?DefinitionGuid", MySqlDbType.Guid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new MySqlParameter("?IncludeDeleted", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = includeDeleted }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		/// <summary>
		/// Gets a page of data from the i7_sflexi_fields table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			var pageLowerBound = (pageSize * pageNumber) - pageSize;

			totalPages = 1;

			var totalRows = GetCount();

			if (pageSize > 0)
			{
				totalPages = totalRows / pageSize;
			}

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				Math.DivRem(totalRows, pageSize, out var remainder);

				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			var sqlCommand = $"SELECT * FROM `i7_sflexi_fields` LIMIT ?PageSize {ifTrue(pageNumber > 1, "OFFSET ?OffsetRows")};";

			var sqlParams = new List<MySqlParameter>
			{
				new MySqlParameter("?PageSize", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
				new MySqlParameter("?OffsetRows", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
			};

			return MySqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand,
				sqlParams.ToArray()
			);
		}


		/// <summary>
		/// Marks a field as deleted.
		/// </summary>
		/// <param name="fieldGuid"></param>
		/// <returns></returns>
		public static bool MarkAsDeleted(Guid fieldGuid)
		{
			const string sqlCommand = "UPDATE `i7_sflexi_fields` SET `IsDeleted` = 1 WHERE `FieldGuid` = ?FieldGuid;";

			var sqlParam = new MySqlParameter("?FieldGuid", MySqlDbType.Guid)
			{
				Direction = ParameterDirection.Input,
				Value = fieldGuid
			};

			int rowsAffected = MySqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand,
				sqlParam
			);

			return rowsAffected > 0;
		}


		private static string ifTrue(bool clause, string str)
		{
			return clause ? str : string.Empty;
		}
	}
}
