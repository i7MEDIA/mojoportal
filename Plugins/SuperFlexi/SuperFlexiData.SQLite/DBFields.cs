using System;
using System.Data;
using mojoPortal.Data;
using Mono.Data.Sqlite;

namespace SuperFlexiData;

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

		var sqlCommand = """
			insert into 
					i7_sflexi_fields (
							SiteGuid
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
							,EditRoles
				) values (
					:SiteGuid
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
						,:EditRoles
					);
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() },
			new(":DefinitionName", DbType.String, 50) { Direction = ParameterDirection.Input, Value = definitionName },
			new(":Name", DbType.String, 50) { Direction = ParameterDirection.Input, Value = name },
			new(":Label", DbType.String, 255) { Direction = ParameterDirection.Input, Value = label },
			new(":DefaultValue", DbType.Object) { Direction = ParameterDirection.Input, Value = defaultValue },
			new(":ControlType", DbType.String, 16) { Direction = ParameterDirection.Input, Value = controlType },
			new(":ControlSrc", DbType.String, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
			new(":DataType", DbType.String, 100) { Direction = ParameterDirection.Input, Value = dataType },
			new(":IsList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
			new(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
			new(":HelpKey", DbType.String, 255) { Direction = ParameterDirection.Input, Value = helpKey },
			new(":Required", DbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
			new(":RequiredMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
			new(":Regex", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regex },
			new(":RegexMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
			new(":Token", DbType.String, 50) { Direction = ParameterDirection.Input, Value = token },
			new(":PreTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenString },
			new(":PostTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenString },
			new(":PreTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
			new(":PostTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
			new(":PreTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
			new(":PostTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
			new(":Searchable", DbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
			new(":EditPageControlWrapperCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
			new(":EditPageLabelCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
			new(":EditPageControlCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
			new(":DatePickerIncludeTimeForDate", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
			new(":DatePickerShowMonthList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
			new(":DatePickerShowYearList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
			new(":DatePickerYearRange", DbType.String, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
			new(":ImageBrowserEmptyUrl", DbType.String, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
			new(":Options", DbType.Object) { Direction = ParameterDirection.Input, Value = options },
			new(":CheckBoxReturnBool", DbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
			new(":CheckBoxReturnValueWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
			new(":CheckBoxReturnValueWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
			new(":DateFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
			new(":TextBoxMode", DbType.String, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
			new(":Attributes", DbType.String, 255) { Direction = ParameterDirection.Input, Value = attributes },
			new(":IsGlobal", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
			new(":ViewRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
			new(":EditRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = editRoles }
		};
		int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams).ToString());

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
		string sqlCommand = """
			update i7_sflexi_fields 
			set 
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
			where FieldGuid = :FieldGuid;
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() },
			new(":DefinitionName", DbType.String, 50) { Direction = ParameterDirection.Input, Value = definitionName },
			new(":Name", DbType.String, 50) { Direction = ParameterDirection.Input, Value = name },
			new(":Label", DbType.String, 255) { Direction = ParameterDirection.Input, Value = label },
			new(":DefaultValue", DbType.Object) { Direction = ParameterDirection.Input, Value = defaultValue },
			new(":ControlType", DbType.String, 16) { Direction = ParameterDirection.Input, Value = controlType },
			new(":ControlSrc", DbType.String, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
			new(":DataType", DbType.String, 100) { Direction = ParameterDirection.Input, Value = dataType },
			new(":IsList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
			new(":SortOrder", DbType.Int32) { Direction = ParameterDirection.Input, Value = sortOrder },
			new(":HelpKey", DbType.String, 255) { Direction = ParameterDirection.Input, Value = helpKey },
			new(":Required", DbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
			new(":RequiredMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
			new(":Regex", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regex },
			new(":RegexMessageFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
			new(":Token", DbType.String, 50) { Direction = ParameterDirection.Input, Value = token },
			new(":PreTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenString },
			new(":PostTokenString", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenString },
			new(":PreTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
			new(":PostTokenStringWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
			new(":PreTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
			new(":PostTokenStringWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
			new(":Searchable", DbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
			new(":EditPageControlWrapperCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
			new(":EditPageLabelCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
			new(":EditPageControlCssClass", DbType.String, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
			new(":DatePickerIncludeTimeForDate", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
			new(":DatePickerShowMonthList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
			new(":DatePickerShowYearList", DbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
			new(":DatePickerYearRange", DbType.String, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
			new(":ImageBrowserEmptyUrl", DbType.String, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
			new(":Options", DbType.Object) { Direction = ParameterDirection.Input, Value = options },
			new(":CheckBoxReturnBool", DbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
			new(":CheckBoxReturnValueWhenTrue", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
			new(":CheckBoxReturnValueWhenFalse", DbType.Object) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
			new(":DateFormat", DbType.String, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
			new(":TextBoxMode", DbType.String, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
			new(":Attributes", DbType.String, 255) { Direction = ParameterDirection.Input, Value = attributes },
			new(":IsDeleted", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isDeleted },
			new(":IsGlobal", DbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
			new(":ViewRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
			new(":EditRoles", DbType.String, 255) { Direction = ParameterDirection.Input, Value = editRoles }
			};
		int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams).ToString());

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes a row from the i7_sflexi_fields table. Returns true if row deleted.
	/// </summary>
	/// <param name="fieldGuid"> fieldGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid fieldGuid)
	{
		var sqlCommand = "delete from i7_sflexi_fields where FieldGuid = :FieldGuid;";
		var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid.ToString() };

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParam);

		return rowsAffected > 0;
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

		return rowsAffected > 0;
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

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the i7_sflexi_fields table.
	/// </summary>
	/// <param name="fieldGuid"> fieldGuid </param>
	public static IDataReader GetOne(Guid fieldGuid)
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
	public static int GetCount() => Convert.ToInt32(
		SqliteHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			"select count(*) from i7_sflexi_fields;"));

	/// <summary>
	/// Gets an IDataReader with all rows in the i7_sflexi_fields table.
	/// </summary>
	public static IDataReader GetAll() => SqliteHelper.ExecuteReader(
		ConnectionString.GetWriteConnectionString(),
		"select * from i7_sflexi_fields where IsDeleted = 0;");

	public static IDataReader GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
	{
		var sqlCommand = """
			select * from i7_sflexi_fields 
			where DefinitionGuid = :DefinitionGuid 
			and ((':IncludeDeleted' = 'true') or (IsDeleted = 0)) 
			order by SortOrder, name;
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":DefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":IncludeDeleted", DbType.Boolean) { Direction = ParameterDirection.Input, Value = includeDeleted }
		};

		return SqliteHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams);
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

		var sqlCommand = "select * from i7_sflexi_fields limit :PageSize" + (pageNumber > 1 ? " offset :OffsetRows;" : ";");

		var sqlParams = new SqliteParameter[]
		{
			new(":PageSize", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":OffsetRows", DbType.Int32) { Direction = ParameterDirection.Input, Value = pageLowerBound }
		};

		return SqliteHelper.ExecuteReader(ConnectionString.GetReadConnectionString(), sqlCommand, sqlParams);
	}

	/// <summary>
	/// Marks a field as deleted.
	/// </summary>
	/// <param name="fieldGuid"></param>
	/// <returns></returns>
	public static bool MarkAsDeleted(Guid fieldGuid)
	{
		var sqlCommand = "update i7_sflexi_fields set IsDeleted = 0 where FieldGuid = :FieldGuid;";
		var sqlParam = new SqliteParameter(":FieldGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldGuid };

		int rowsAffected = SqliteHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand, sqlParam);

		return rowsAffected > 0;
	}
}
