using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("insert into i7_sflexi_fields ({0}) values ({1});"
                ,@"siteguid
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
				  ,editroles"
                , @":siteguid
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
				  ,:editroles");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
                new NpgsqlParameter(":definitionname", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = definitionName },
                new NpgsqlParameter(":name", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = name },
                new NpgsqlParameter(":label", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = label },
                new NpgsqlParameter(":defaultvalue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = defaultValue },
                new NpgsqlParameter(":controltype", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = controlType },
                new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
				new NpgsqlParameter(":datatype", NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Input, Value = dataType },
                new NpgsqlParameter(":islist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
				new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = sortOrder },
                new NpgsqlParameter(":helpkey", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
                new NpgsqlParameter(":required", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
                new NpgsqlParameter(":requiredmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
                new NpgsqlParameter(":regex", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regex },
                new NpgsqlParameter(":regexmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
                new NpgsqlParameter(":token", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = token },
                new NpgsqlParameter(":pretokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenString },
                new NpgsqlParameter(":posttokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenString },
				new NpgsqlParameter(":pretokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
				new NpgsqlParameter(":posttokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
				new NpgsqlParameter(":pretokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
				new NpgsqlParameter(":posttokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
				new NpgsqlParameter(":searchable", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
                new NpgsqlParameter(":editpagecontrolwrappercssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
                new NpgsqlParameter(":editpagelabelcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
                new NpgsqlParameter(":editpagecontrolcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
                new NpgsqlParameter(":datepickerincludetimefordate", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
                new NpgsqlParameter(":datepickershowmonthlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
                new NpgsqlParameter(":datepickershowyearlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
                new NpgsqlParameter(":datepickeryearrange", NpgsqlDbType.Varchar, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
                new NpgsqlParameter(":imagebrowseremptyurl", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
                new NpgsqlParameter(":options", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = options },
                new NpgsqlParameter(":checkboxreturnbool", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
                new NpgsqlParameter(":checkboxreturnvaluewhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
                new NpgsqlParameter(":checkboxreturnvaluewhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
                new NpgsqlParameter(":dateformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
                new NpgsqlParameter(":textboxmode", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
                new NpgsqlParameter(":attributes", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = attributes },
				new NpgsqlParameter(":isglobal", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
                new NpgsqlParameter(":viewroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
                new NpgsqlParameter(":editroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = editRoles }
            };
            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("update i7_sflexi_fields set {0} where fieldguid = :fieldguid;"
                ,@"siteguid = :siteguid
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
				 ,editroles = :editroles");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
                new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
                new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid },
                new NpgsqlParameter(":definitionname", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = definitionName },
                new NpgsqlParameter(":name", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = name },
                new NpgsqlParameter(":label", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = label },
                new NpgsqlParameter(":defaultvalue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = defaultValue },
                new NpgsqlParameter(":controltype", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = controlType },
                new NpgsqlParameter(":controlsrc", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = controlSrc },
				new NpgsqlParameter(":datatype", NpgsqlDbType.Varchar, 100) { Direction = ParameterDirection.Input, Value = dataType },
				new NpgsqlParameter(":islist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isList },
				new NpgsqlParameter(":sortorder", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = sortOrder },
                new NpgsqlParameter(":helpkey", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = helpKey },
                new NpgsqlParameter(":required", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = required },
                new NpgsqlParameter(":requiredmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = requiredMessageFormat },
                new NpgsqlParameter(":regex", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regex },
                new NpgsqlParameter(":regexmessageformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = regexMessageFormat },
                new NpgsqlParameter(":token", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = token },
                new NpgsqlParameter(":pretokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenString },
                new NpgsqlParameter(":posttokenstring", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenString },
				new NpgsqlParameter(":pretokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenTrue },
				new NpgsqlParameter(":posttokenstringwhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenTrue },
				new NpgsqlParameter(":pretokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = preTokenStringWhenFalse },
				new NpgsqlParameter(":posttokenstringwhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = postTokenStringWhenFalse },
				new NpgsqlParameter(":searchable", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = searchable },
                new NpgsqlParameter(":editpagecontrolwrappercssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlWrapperCssClass },
                new NpgsqlParameter(":editpagelabelcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageLabelCssClass },
                new NpgsqlParameter(":editpagecontrolcssclass", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = editPageControlCssClass },
                new NpgsqlParameter(":datepickerincludetimefordate", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerIncludeTimeForDate },
                new NpgsqlParameter(":datepickershowmonthlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowMonthList },
                new NpgsqlParameter(":datepickershowyearlist", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = datePickerShowYearList },
                new NpgsqlParameter(":datepickeryearrange", NpgsqlDbType.Varchar, 10) { Direction = ParameterDirection.Input, Value = datePickerYearRange },
                new NpgsqlParameter(":imagebrowseremptyurl", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = imageBrowserEmptyUrl },
                new NpgsqlParameter(":options", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = options },
                new NpgsqlParameter(":checkboxreturnbool", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = checkBoxReturnBool },
                new NpgsqlParameter(":checkboxreturnvaluewhentrue", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenTrue },
                new NpgsqlParameter(":checkboxreturnvaluewhenfalse", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = checkBoxReturnValueWhenFalse },
                new NpgsqlParameter(":dateformat", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = dateFormat },
                new NpgsqlParameter(":textboxmode", NpgsqlDbType.Varchar, 25) { Direction = ParameterDirection.Input, Value = textBoxMode },
                new NpgsqlParameter(":attributes", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = attributes },
                new NpgsqlParameter(":isdeleted", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isDeleted },
                new NpgsqlParameter(":isglobal", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = isGlobal },
				new NpgsqlParameter(":viewroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = viewRoles },
				new NpgsqlParameter(":editroles", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = editRoles }
			};
			var returnValue = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParams.ToArray());
			int rowsAffected = Convert.ToInt32(returnValue.ToString());

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
            var sqlCommand = "delete from i7_sflexi_fields where fieldguid = :fieldguid;";

            var sqlParam = new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldGuid };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            var sqlCommand = "delete from i7_sflexi_fields where siteguid = :siteguid;";

			var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            var sqlCommand = "delete from i7_sflexi_fields where definitionguid = :definitionguid;";

			var sqlParam = new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
        public static int GetCount()
        {
            var sqlCommand = "select count(*) from i7_sflexi_fields;";

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                 ConnectionString.GetReadConnectionString(),
				 CommandType.Text,
				 sqlCommand));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the i7_sflexi_fields table.
        /// </summary>
        public static IDataReader GetAll()
        {
            var sqlCommand = "select * from i7_sflexi_fields where isdeleted = false;";

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand);
        }

        public static IDataReader GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
        {
            var sqlCommand = @"select * from i7_sflexi_fields 
                where definitionguid = :definitionguid 
                and ((:includedeleted = true) or (isdeleted = false)) 
                order by sortorder, name;";

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":definitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
                new NpgsqlParameter(":includedeleted", NpgsqlDbType.Boolean) { Direction = ParameterDirection.Input, Value = includeDeleted }
            };
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            var sqlCommand = "select * from i7_sflexi_fields limit :pagesize" + (pageNumber > 1 ? "offset :offsetrows;" : ";");

            var sqlParams = new List<NpgsqlParameter>
            {
                new NpgsqlParameter(":pagesize", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageSize },
                new NpgsqlParameter(":offsetrows", NpgsqlDbType.Integer) { Direction = ParameterDirection.Input, Value = pageLowerBound }
            };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
            var sqlCommand = "update i7_sflexi_fields set isdeleted = true where fieldguid = :fieldguid;";

            var sqlParam = new NpgsqlParameter(":fieldguid", NpgsqlDbType.Uuid)
            {
                Direction = ParameterDirection.Input,
                Value = fieldGuid
            };

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand,
                sqlParam);

            return (rowsAffected > 0);
        }
    }

}


