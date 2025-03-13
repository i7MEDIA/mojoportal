using System;
using System.Data;
using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;

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
		string editRoles
		)
	{

		var sqlCommand = """
			insert into i7_sflexi_fields (
				siteguid
				,featureguid
				,definitionguid
				,fieldguid
				,definitionname
				,name
				,label
				,defaultvalue
				,controltype
				,controlsrc
				,datatype
				,islist
				,sortorder
				,helpkey
				,required
				,requiredmessageformat
				,regex
				,regexmessageformat
				,token
				,searchable
				,editpagecontrolwrappercssclass
				,editpagelabelcssclass
				,editpagecontrolcssclass
				,datepickerincludetimefordate
				,datepickershowmonthlist
				,datepickershowyearlist
				,datepickeryearrange
				,imagebrowseremptyurl
				,options
				,checkboxreturnbool
				,checkboxreturnvaluewhentrue
				,checkboxreturnvaluewhenfalse
				,dateformat
				,textboxmode
				,attributes
				,pretokenstring
				,posttokenstring
				,pretokenstringwhentrue
				,posttokenstringwhentrue
				,pretokenstringwhenfalse
				,posttokenstringwhenfalse
				,isglobal
				,viewroles
				,editroles
			) values (
				:siteguid
				,:featureguid
				,:definitionguid
				,:fieldguid
				,:definitionname
				,:name
				,:label
				,:defaultvalue
				,:controltype
				,:controlsrc
				,:datatype
				,:islist
				,:sortorder
				,:helpkey
				,:required
				,:requiredmessageformat
				,:regex
				,:regexmessageformat
				,:token
				,:searchable
				,:editpagecontrolwrappercssclass
				,:editpagelabelcssclass
				,:editpagecontrolcssclass
				,:datepickerincludetimefordate
				,:datepickershowmonthlist
				,:datepickershowyearlist
				,:datepickeryearrange
				,:imagebrowseremptyurl
				,:options
				,:checkboxreturnbool
				,:checkboxreturnvaluewhentrue
				,:checkboxreturnvaluewhenfalse
				,:dateformat
				,:textboxmode
				,:attributes
				,:pretokenstring
				,:posttokenstring
				,:pretokenstringwhentrue
				,:posttokenstringwhentrue
				,:pretokenstringwhenfalse
				,:posttokenstringwhenfalse
				,:isglobal
				,:viewroles
				,:editroles
			);
			""";

		var sqlParams = new NpgsqlParameter[]
		{
			new(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
			new(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
			new(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
			new(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
			new(":definitionname", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = definitionName },
			new(":name", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = name },
			new(":label", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = label },
			new(":defaultvalue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = defaultValue },
			new(":controltype", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = controlType },
			new(":controlsrc", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
			new(":datatype", NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Input, Value = dataType },
			new(":islist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
			new(":sortorder", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = sortOrder },
			new(":helpkey", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
			new(":required", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
			new(":requiredmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
			new(":regex", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regex },
			new(":regexmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
			new(":token", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = token },
			new(":pretokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenString },
			new(":posttokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenString },
			new(":pretokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
			new(":posttokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
			new(":pretokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
			new(":posttokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
			new(":searchable", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
			new(":editpagecontrolwrappercssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
			new(":editpagelabelcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
			new(":editpagecontrolcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
			new(":datepickerincludetimefordate", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
			new(":datepickershowmonthlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
			new(":datepickershowyearlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
			new(":datepickeryearrange", NpgsqlDbType.Varchar, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
			new(":imagebrowseremptyurl", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
			new(":options", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = options },
			new(":checkboxreturnbool", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
			new(":checkboxreturnvaluewhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
			new(":checkboxreturnvaluewhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
			new(":dateformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
			new(":textboxmode", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
			new(":attributes", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = attributes },
			new(":isglobal", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
			new(":viewroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
			new(":editroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = editRoles }
		};

		int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
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
		var sqlCommand = """
			update i7_sflexi_fields 
			set 
				siteguid = :siteguid
				,featureguid = :featureguid
				,definitionguid = :definitionguid
				,definitionname = :definitionname
				,name = :name
				,label = :label
				,defaultvalue = :defaultvalue
				,controltype = :controltype
				,controlsrc = :controlsrc
				,datatype = :datatype
				,islist = :islist
				,sortorder = :sortorder
				,helpkey = :helpkey
				,required = :required
				,requiredmessageformat = :requiredmessageformat
				,regex = :regex
				,regexmessageformat = :regexmessageformat
				,token = :token
				,pretokenstring = :pretokenstring
				,posttokenstring = :posttokenstring
				,pretokenstringwhentrue = :pretokenstringwhentrue
				,posttokenstringwhentrue = :posttokenstringwhentrue
				,pretokenstringwhenfalse = :pretokenstringwhenfalse
				,posttokenstringwhenfalse = :posttokenstringwhenfalse
				,searchable = :searchable
				,editpagecontrolwrappercssclass = :editpagecontrolwrappercssclass
				,editpagelabelcssclass = :editpagelabelcssclass
				,editpagecontrolcssclass = :editpagecontrolcssclass
				,datepickerincludetimefordate = :datepickerincludetimefordate
				,datepickershowmonthlist = :datepickershowmonthlist
				,datepickershowyearlist = :datepickershowyearlist
				,datepickeryearrange = :datepickeryearrange
				,imagebrowseremptyurl = :imagebrowseremptyurl
				,options = :options
				,checkboxreturnbool = :checkboxreturnbool
				,checkboxreturnvaluewhentrue = :checkboxreturnvaluewhentrue
				,checkboxreturnvaluewhenfalse = :checkboxreturnvaluewhenfalse
				,dateformat = :dateformat
				,textboxmode = :textboxmode
				,attributes = :attributes
				,isdeleted = :isdeleted
				,isglobal = :isglobal
				,viewroles = :viewroles
				,editroles = :editroles
			where 
				fieldguid = :fieldguid;
			""";

		var sqlParams = new NpgsqlParameter[]
		{
			new(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
			new(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
			new(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
			new(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
			new(":definitionname", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = definitionName },
			new(":name", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = name },
			new(":label", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = label },
			new(":defaultvalue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = defaultValue },
			new(":controltype", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = controlType },
			new(":controlsrc", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
			new(":datatype", NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Input, Value = dataType },
			new(":islist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
			new(":sortorder", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = sortOrder },
			new(":helpkey", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
			new(":required", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
			new(":requiredmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
			new(":regex", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regex },
			new(":regexmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
			new(":token", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = token },
			new(":pretokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenString },
			new(":posttokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenString },
			new(":pretokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
			new(":posttokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
			new(":pretokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
			new(":posttokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
			new(":searchable", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
			new(":editpagecontrolwrappercssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
			new(":editpagelabelcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
			new(":editpagecontrolcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
			new(":datepickerincludetimefordate", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
			new(":datepickershowmonthlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
			new(":datepickershowyearlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
			new(":datepickeryearrange", NpgsqlDbType.Varchar, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
			new(":imagebrowseremptyurl", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
			new(":options", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = options },
			new(":checkboxreturnbool", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
			new(":checkboxreturnvaluewhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
			new(":checkboxreturnvaluewhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
			new(":dateformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
			new(":textboxmode", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
			new(":attributes", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = attributes },
			new(":isdeleted", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isDeleted },
			new(":isglobal", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
			new(":viewroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
			new(":editroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = editRoles }
			};

		var returnValue = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			sqlParams);

		int rowsAffected = Convert.ToInt32(returnValue.ToString());

		return rowsAffected > 0;
	}

	/// <summary>
	/// Deletes a row from the i7_sflexi_fields table. Returns true if row deleted.
	/// </summary>
	/// <param name="fieldGuid"> fieldGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid fieldGuid)
	{
		var sqlCommand = "delete from i7_sflexi_fields where fieldguid = :fieldguid;";
		var sqlParam = new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
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
		var sqlCommand = "delete from i7_sflexi_fields where siteguid = :siteguid;";
		var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
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
		var sqlCommand = "delete from i7_sflexi_fields where definitionguid = :definitionguid;";
		var sqlParam = new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
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
		var sqlCommand = "select * from i7_sflexi_fields where fieldguid = :fieldguid;";
		var sqlParam = new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid };

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParam);
	}

	/// <summary>
	/// Gets a count of rows in the i7_sflexi_fields table.
	/// </summary>
	public static int GetCount() => Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
		ConnectionString.GetReadConnectionString(),
		CommandType.Text,
		"select count(*) from i7_sflexi_fields;"));

	/// <summary>
	/// Gets an IDataReader with all rows in the i7_sflexi_fields table.
	/// </summary>
	public static IDataReader GetAll() => NpgsqlHelper.ExecuteReader(
		ConnectionString.GetWriteConnectionString(),
		CommandType.Text,
		"select * from i7_sflexi_fields where isdeleted = false;");

	public static IDataReader GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
	{
		var sqlCommand = """
			select * 
			from i7_sflexi_fields 
			where definitionguid = :definitionguid 
				and ((:includedeleted = true) or (isdeleted = false)) 
			order by sortorder, name;
			""";

		var sqlParams = new NpgsqlParameter[]
		{
			new(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
			new(":includedeleted", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeDeleted }
		};

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
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
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}
		var sqlCommand = $"select * from i7_sflexi_fields limit :pagesize {(pageNumber > 1 ? "offset :offsetrows;" : ";")}";

		var sqlParams = new NpgsqlParameter[]
		{
			new(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
			new(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageLowerBound }
		};

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParams);
	}

	/// <summary>
	/// Marks a field as deleted.
	/// </summary>
	/// <param name="fieldGuid"></param>
	/// <returns></returns>
	public static bool MarkAsDeleted(Guid fieldGuid)
	{
		var sqlCommand = "update i7_sflexi_fields set isdeleted = true where fieldguid = :fieldguid;";
		var sqlParam = new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParam);

		return rowsAffected > 0;
	}
}
