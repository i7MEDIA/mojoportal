using SuperFlexiData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SuperFlexiBusiness;
public class Field : Hashtable, IComparable<Field>
{

	#region Constructors

	public Field() { }

	public Field(Guid fieldGuid)
	{
		GetField(fieldGuid);
	}

	#endregion

	#region Public Properties

	public Guid SiteGuid { get; set; } = Guid.Empty;
	public Guid FeatureGuid { get; set; } = Guid.Empty;
	public Guid DefinitionGuid { get; set; } = Guid.Empty;
	public Guid FieldGuid{ get; set; } = Guid.Empty;
	public string DefinitionName { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Label { get; set; } = string.Empty;
	public string DefaultValue { get; set; } = string.Empty;
	public string ControlType { get; set; } = string.Empty;
	public string ControlSrc { get; set; } = string.Empty;
	public string DataType { get; set; } = "string";
	public bool IsList { get; set; } = false;
	public int SortOrder { get; set; } = -1;
	public string HelpKey { get; set; } = string.Empty;
	public bool Required { get; set; } = false;
	public string RequiredMessageFormat { get; set; } = string.Empty;
	public string Regex { get; set; } = string.Empty;
	public string RegexMessageFormat { get; set; } = string.Empty;
	public string Token { get; set; } = "$_NONE_$";
	public string PreTokenString { get; set; } = string.Empty;
	public string PostTokenString { get; set; } = string.Empty;
	public string PreTokenStringWhenTrue { get; set; } = string.Empty;
	public string PostTokenStringWhenTrue { get; set; } = string.Empty;
	public string PreTokenStringWhenFalse { get; set; } = string.Empty;
	public string PostTokenStringWhenFalse { get; set; } = string.Empty;
	public bool Searchable { get; set; } = true;
	public string EditPageControlWrapperCssClass { get; set; } = "settingrow";
	public string EditPageLabelCssClass { get; set; } = "settinglabel";
	public string EditPageControlCssClass { get; set; } = "forminput";
	public bool DatePickerIncludeTimeForDate { get; set; } = false;
	public bool DatePickerShowMonthList { get; set; } = false;
	public bool DatePickerShowYearList { get; set; } = false;
	public string DatePickerYearRange { get; set; } = string.Empty;
	public string ImageBrowserEmptyUrl { get; set; } = "~/Data/SiteImages/1x1.gif";
	public string Options { get; set; } = string.Empty;
	public bool CheckBoxReturnBool { get; set; } = true;
	public string CheckBoxReturnValueWhenTrue { get; set; } = string.Empty;
	public string CheckBoxReturnValueWhenFalse { get; set; } = string.Empty;
	public string DateFormat { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
	public string TextBoxMode { get; set; } = "SingleLine";
	public string Attributes { get; set; } = string.Empty;
	public string ViewRoles { get; set; } = "All Users;";
	public string EditRoles { get; set; } = string.Empty;
	/// <summary>
	/// Used to flag a field as deleted when SuperFlexi:DeleteOrphanedFieldValues is false.
	/// </summary>
	public bool IsDeleted { get; set; } = false;
	public bool IsGlobal { get; set; } = false;
	/// <summary>
	/// Not used!
	/// </summary>
	public string LinkedField { get; set; } = string.Empty;


	public bool IsDynamicListField
	{
		get
		{
			if (ControlType == "DynamicRadioButtonList" || ControlType == "DynamicCheckBoxList")
			{
				return true;
			}
			return false;
		}
	}

	public bool IsCheckBoxListField
	{
		get
		{
			if (ControlType == "CheckBoxList" || ControlType == "DynamicCheckBoxList")
			{
				return true;
			}
			return false;
		}
	}

	public bool IsRadioButtonListField
	{
		get
		{
			if (ControlType == "RadioButtonList" || ControlType == "DynamicRadioButtonList")
			{
				return true;
			}
			return false;
		}
	}

	public bool IsDateField
	{
		get
		{
			if (ControlType == "DateTime" || ControlType == "Date" || (ControlType == "TextBox" && (TextBoxMode == "Date" || TextBoxMode == "DateTime" || TextBoxMode == "DateTimeLocal")))
			{
				return true;
			}
			return false;
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Gets an instance of field.
	/// </summary>
	/// <param name="fieldGuid"> fieldGuid </param>
	private void GetField(Guid fieldGuid)
	{
		using IDataReader reader = DBFields.GetOne(fieldGuid);
		PopulateFromReader(reader);
	}

	private void PopulateFromReader(IDataReader reader)
	{
		if (reader.Read())
		{
			SiteGuid = new Guid(reader["SiteGuid"].ToString());
			FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
			DefinitionGuid = new Guid(reader["DefinitionGuid"].ToString());
			FieldGuid = new Guid(reader["FieldGuid"].ToString());
			DefinitionName = reader["DefinitionName"].ToString();
			Name = reader["Name"].ToString();
			Label = reader["Label"].ToString();
			DefaultValue = reader["DefaultValue"].ToString();
			ControlType = reader["ControlType"].ToString();
			ControlSrc = reader["ControlSrc"].ToString();
			DataType = reader["DataType"].ToString();
			IsList = Convert.ToBoolean(reader["IsList"]);
			SortOrder = Convert.ToInt32(reader["SortOrder"]);
			HelpKey = reader["HelpKey"].ToString();
			Required = Convert.ToBoolean(reader["Required"]);
			RequiredMessageFormat = reader["RequiredMessageFormat"].ToString();
			Regex = reader["Regex"].ToString();
			RegexMessageFormat = reader["RegexMessageFormat"].ToString();
			Token = reader["Token"].ToString();
			PreTokenString = reader["PreTokenString"].ToString();
			PostTokenString = reader["PostTokenString"].ToString();
			PreTokenStringWhenTrue = reader["PreTokenStringWhenTrue"].ToString();
			PostTokenStringWhenTrue = reader["PostTokenStringWhenTrue"].ToString();
			PreTokenStringWhenFalse = reader["PreTokenStringWhenFalse"].ToString();
			PostTokenStringWhenFalse = reader["PostTokenStringWhenFalse"].ToString();
			Searchable = Convert.ToBoolean(reader["Searchable"]);
			EditPageControlWrapperCssClass = reader["EditPageControlWrapperCssClass"].ToString();
			EditPageLabelCssClass = reader["EditPageLabelCssClass"].ToString();
			EditPageControlCssClass = reader["EditPageControlCssClass"].ToString();
			DatePickerIncludeTimeForDate = Convert.ToBoolean(reader["DatePickerIncludeTimeForDate"]);
			DatePickerShowMonthList = Convert.ToBoolean(reader["DatePickerShowMonthList"]);
			DatePickerShowYearList = Convert.ToBoolean(reader["DatePickerShowYearList"]);
			DatePickerYearRange = reader["DatePickerYearRange"].ToString();
			ImageBrowserEmptyUrl = reader["ImageBrowserEmptyUrl"].ToString();
			//this.iSettingControlSettings = reader["ISettingControlSettings"].ToString();
			Options = reader["Options"].ToString();
			CheckBoxReturnBool = Convert.ToBoolean(reader["CheckBoxReturnBool"]);
			CheckBoxReturnValueWhenTrue = reader["CheckBoxReturnValueWhenTrue"].ToString();
			CheckBoxReturnValueWhenFalse = reader["CheckBoxReturnValueWhenFalse"].ToString();
			TextBoxMode = reader["TextBoxMode"].ToString();
			Attributes = reader["Attributes"].ToString();
			IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
			IsGlobal = Convert.ToBoolean(reader["IsGlobal"]);
			ViewRoles = reader["ViewRoles"].ToString();
			EditRoles = reader["EditRoles"].ToString();
			if (string.IsNullOrWhiteSpace(ViewRoles))
			{
				//mysql doesn't allow default values for TEXT columns so we do this because the field should never be empty
				ViewRoles = "AllUsers;";
			}
			
			string format = reader["DateFormat"].ToString().Trim();

			if (format.Length > 0)
			{
				try
				{
					string d = DateTime.Now.ToString(format, CultureInfo.CurrentCulture);
					DateFormat = format;
				}
				catch (FormatException) { }
			}
		}
	}

	/// <summary>
	/// Persists a new instance of field. Returns true on success.
	/// </summary>
	/// <returns></returns>
	private bool Create()
	{
		FieldGuid = Guid.NewGuid();

		int rowsAffected = DBFields.Create(
			SiteGuid,
			FeatureGuid,
			DefinitionGuid,
			FieldGuid,
			DefinitionName,
			Name,
			Label,
			DefaultValue,
			ControlType,
			ControlSrc,
			DataType,
			IsList,
			SortOrder,
			HelpKey,
			Required,
			RequiredMessageFormat,
			Regex,
			RegexMessageFormat,
			Token,
			PreTokenString,
			PostTokenString,
			PreTokenStringWhenTrue,
			PostTokenStringWhenTrue,
			PreTokenStringWhenFalse,
			PostTokenStringWhenFalse,
			Searchable,
			EditPageControlWrapperCssClass,
			EditPageLabelCssClass,
			EditPageControlCssClass,
			DatePickerIncludeTimeForDate,
			DatePickerShowMonthList,
			DatePickerShowYearList,
			DatePickerYearRange,
			ImageBrowserEmptyUrl,
			Options,
			CheckBoxReturnBool,
			CheckBoxReturnValueWhenTrue,
			CheckBoxReturnValueWhenFalse,
			DateFormat,
			TextBoxMode,
			Attributes,
			IsGlobal,
			ViewRoles,
			EditRoles);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Updates this instance of field. Returns true on success.
	/// </summary>
	/// <returns>bool</returns>
	private bool Update()
	{
		return DBFields.Update(
			SiteGuid,
			FeatureGuid,
			DefinitionGuid,
			FieldGuid,
			DefinitionName,
			Name,
			Label,
			DefaultValue,
			ControlType,
			ControlSrc,
			DataType,
			IsList,
			SortOrder,
			HelpKey,
			Required,
			RequiredMessageFormat,
			Regex,
			RegexMessageFormat,
			Token,
			PreTokenString,
			PostTokenString,
			PreTokenStringWhenTrue,
			PostTokenStringWhenTrue,
			PreTokenStringWhenFalse,
			PostTokenStringWhenFalse,
			Searchable,
			EditPageControlWrapperCssClass,
			EditPageLabelCssClass,
			EditPageControlCssClass,
			DatePickerIncludeTimeForDate,
			DatePickerShowMonthList,
			DatePickerShowYearList,
			DatePickerYearRange,
			ImageBrowserEmptyUrl,
			Options,
			CheckBoxReturnBool,
			CheckBoxReturnValueWhenTrue,
			CheckBoxReturnValueWhenFalse,
			DateFormat,
			TextBoxMode,
			Attributes,
			IsDeleted,
			IsGlobal,
			ViewRoles,
			EditRoles);
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Saves this instance of field. Returns true on success.
	/// </summary>
	/// <returns>bool</returns>
	public bool Save() => FieldGuid != Guid.Empty ? Update() : Create();

	public int CompareTo(Field field) => SortOrder.CompareTo(field.SortOrder);

	#endregion

	#region Static Methods

	/// <summary>
	/// Deletes an instance of field. Returns true on success.
	/// </summary>
	/// <param name="fieldGuid"> fieldGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid fieldGuid) => DBFields.Delete(fieldGuid);

	/// <summary>
	/// Deletes Fields by Site. Returns true on success.
	/// </summary>
	public static bool DeleteBySite(Guid siteGuid) => DBFields.DeleteBySite(siteGuid);

	/// <summary>
	/// Deletes Fields by Field Definition. Returns true on success.
	/// </summary>
	/// <param name="moduleGuid"> moduleGuid </param>
	/// <returns>bool</returns>
	public static bool DeleteByDefinition(Guid definitionGuid) => DBFields.DeleteByDefinition(definitionGuid);

	/// <summary>
	/// Gets a count of Field. 
	/// </summary>
	public static int GetCount() => DBFields.GetCount();

	private static List<Field> LoadListFromReader(IDataReader reader)
	{
		List<Field> fieldList = new();
		try
		{
			while (reader.Read())
			{
				Field field = new()
				{
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					FeatureGuid = new Guid(reader["FeatureGuid"].ToString()),
					DefinitionGuid = new Guid(reader["DefinitionGuid"].ToString()),
					FieldGuid = new Guid(reader["FieldGuid"].ToString()),
					DefinitionName = reader["DefinitionName"].ToString(),
					Name = reader["Name"].ToString(),
					Label = reader["Label"].ToString(),
					DefaultValue = reader["DefaultValue"].ToString(),
					ControlType = reader["ControlType"].ToString(),
					ControlSrc = reader["ControlSrc"].ToString(),
					DataType = reader["DataType"].ToString(),
					IsList = Convert.ToBoolean(reader["IsList"]),
					SortOrder = Convert.ToInt32(reader["SortOrder"]),
					HelpKey = reader["HelpKey"].ToString(),
					Required = Convert.ToBoolean(reader["Required"]),
					RequiredMessageFormat = reader["RequiredMessageFormat"].ToString(),
					Regex = reader["Regex"].ToString(),
					RegexMessageFormat = reader["RegexMessageFormat"].ToString(),
					Token = reader["Token"].ToString(),
					PreTokenString = reader["PreTokenString"].ToString(),
					PostTokenString = reader["PostTokenString"].ToString(),
					PreTokenStringWhenTrue = reader["PreTokenStringWhenTrue"].ToString(),
					PostTokenStringWhenTrue = reader["PostTokenStringWhenTrue"].ToString(),
					PreTokenStringWhenFalse = reader["PreTokenStringWhenFalse"].ToString(),
					PostTokenStringWhenFalse = reader["PostTokenStringWhenFalse"].ToString(),
					Searchable = Convert.ToBoolean(reader["Searchable"]),
					EditPageControlWrapperCssClass = reader["EditPageControlWrapperCssClass"].ToString(),
					EditPageLabelCssClass = reader["EditPageLabelCssClass"].ToString(),
					EditPageControlCssClass = reader["EditPageControlCssClass"].ToString(),
					DatePickerIncludeTimeForDate = Convert.ToBoolean(reader["DatePickerIncludeTimeForDate"]),
					DatePickerShowMonthList = Convert.ToBoolean(reader["DatePickerShowMonthList"]),
					DatePickerShowYearList = Convert.ToBoolean(reader["DatePickerShowYearList"]),
					DatePickerYearRange = reader["DatePickerYearRange"].ToString(),
					ImageBrowserEmptyUrl = reader["ImageBrowserEmptyUrl"].ToString(),
					//field.iSettingControlSettings = reader["ISettingControlSettings"].ToString();
					Options = reader["Options"].ToString(),
					CheckBoxReturnBool = Convert.ToBoolean(reader["CheckBoxReturnBool"]),
					CheckBoxReturnValueWhenTrue = reader["CheckBoxReturnValueWhenTrue"].ToString(),
					CheckBoxReturnValueWhenFalse = reader["CheckBoxReturnValueWhenFalse"].ToString(),
					DateFormat = reader["dateFormat"].ToString(),
					TextBoxMode = reader["textBoxMode"].ToString(),
					Attributes = reader["Attributes"].ToString(),
					IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
					IsGlobal = Convert.ToBoolean(reader["IsGlobal"]),
					ViewRoles = reader["ViewRoles"].ToString(),
					EditRoles = reader["EditRoles"].ToString()
				};

				if (string.IsNullOrWhiteSpace(field.ViewRoles))
				{
					//mysql doesn't allow default values for TEXT columns so we do this because the field should never be empty
					field.ViewRoles = "All Users;";
				}

				fieldList.Add(field);
			}
		}
		finally
		{
			reader.Close();
		}

		return fieldList;
	}

	/// <summary>
	/// Gets an IList with all instances of field.
	/// </summary>
	public static List<Field> GetAll()
	{
		IDataReader reader = DBFields.GetAll();
		return LoadListFromReader(reader);
	}

	/// <summary>
	/// Gets an IList with all fields for a definition
	/// </summary>
	/// <param name="definitionGuid"></param>
	/// <returns>IList</returns>
	public static List<Field> GetAllForDefinition(Guid definitionGuid, bool includeDeleted = false)
	{
		IDataReader reader = DBFields.GetAllForDefinition(definitionGuid, includeDeleted);
		return LoadListFromReader(reader);
	}

	/// <summary>
	/// Gets an IList with page of instances of field.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static List<Field> GetPage(int pageNumber, int pageSize, out int totalPages)
	{
		IDataReader reader = DBFields.GetPage(pageNumber, pageSize, out totalPages);
		return LoadListFromReader(reader);
	}

	public static bool MarkAsDeleted(Guid fieldGuid) => DBFields.MarkAsDeleted(fieldGuid);

	#endregion
}

/// <summary>
/// Simple Name and DefinitionGuid comparison of two fields
/// </summary>
public class SimpleFieldComparer : IEqualityComparer<Field>
{
	//Fields are equal if their names, definition guids are equal.
	public bool Equals(Field x, Field y)
	{
		//check whether the compared objects reference the same data.
		if (Object.ReferenceEquals(x, y)) return true;

		//check for nulls
		if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
		{
			return false;
		}

		//check if properties are equal
		return x.Name == y.Name && x.DefinitionGuid == y.DefinitionGuid;
	}

	// If Equals() returns true for a pair of objects 
	// then GetHashCode() must return the same value for these objects.
	public int GetHashCode(Field field)
	{
		//Check whether the object is null
		if (Object.ReferenceEquals(field, null)) return 0;

		int hashName = field.Name.GetHashCode();
		int hashDefinitionGuid = field.DefinitionGuid.GetHashCode();

		return hashName ^ hashDefinitionGuid;
	}
}

/// <summary>
/// Full comparison of two fields
/// </summary>
public class FieldComparer : IEqualityComparer<Field>
{
	public bool Equals(Field x, Field y)
	{
		//check whether the compared objects reference the same data.
		if (Object.ReferenceEquals(x, y)) return true;

		//check for nulls
		if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
		{
			return false;
		}

		//check if properties are equal
		return x.Name == y.Name
			&& x.DefinitionGuid == y.DefinitionGuid
			&& x.DefinitionName == y.DefinitionName
			&& x.ControlType == y.ControlType
			&& x.ControlSrc == y.ControlSrc
			&& x.DataType == y.DataType
			&& x.IsList == y.IsList
			&& x.SortOrder == y.SortOrder
			&& x.DefaultValue == y.DefaultValue
			&& x.Token == y.Token
			&& x.PreTokenString == y.PreTokenString
			&& x.PostTokenString == y.PostTokenString
			&& x.PreTokenStringWhenTrue == y.PreTokenStringWhenTrue
			&& x.PostTokenStringWhenTrue == y.PostTokenStringWhenTrue
			&& x.PreTokenStringWhenFalse == y.PreTokenStringWhenFalse
			&& x.PostTokenStringWhenFalse == y.PostTokenStringWhenFalse
			&& x.Label == y.Label
			&& x.HelpKey == y.HelpKey
			&& x.Required == y.Required
			&& x.RequiredMessageFormat == y.RequiredMessageFormat
			&& x.Regex == y.Regex
			&& x.RegexMessageFormat == y.RegexMessageFormat
			&& x.Searchable == y.Searchable
			&& x.EditPageControlWrapperCssClass == y.EditPageControlWrapperCssClass
			&& x.EditPageLabelCssClass == y.EditPageLabelCssClass
			&& x.EditPageControlCssClass == y.EditPageControlCssClass
			&& x.DatePickerIncludeTimeForDate == y.DatePickerIncludeTimeForDate
			&& x.DatePickerShowMonthList == y.DatePickerShowMonthList
			&& x.DatePickerShowYearList == y.DatePickerShowYearList
			&& x.DatePickerYearRange == y.DatePickerYearRange
			&& x.ImageBrowserEmptyUrl == y.ImageBrowserEmptyUrl
			&& x.Options == y.Options
			&& x.CheckBoxReturnBool == y.CheckBoxReturnBool
			&& x.CheckBoxReturnValueWhenTrue == y.CheckBoxReturnValueWhenTrue
			&& x.CheckBoxReturnValueWhenFalse == y.CheckBoxReturnValueWhenFalse
			&& x.DateFormat == y.DateFormat
			&& x.TextBoxMode == y.TextBoxMode
			&& x.Attributes == y.Attributes
			&& x.IsDeleted == y.IsDeleted
			&& x.IsGlobal == y.IsGlobal
			&& x.ViewRoles == y.ViewRoles
			&& x.EditRoles == y.EditRoles;
	}

	// If Equals() returns true for a pair of objects 
	// then GetHashCode() must return the same value for these objects.
	public int GetHashCode(Field field)
	{
		//Check whether the object is null
		if (Object.ReferenceEquals(field, null)) return 0;

		int hashName = field.Name.GetHashCode();
		int hashDefinitionGuid = field.DefinitionGuid.GetHashCode();
		int hashDefinitionName = field.DefinitionName.GetHashCode();
		int hashControlType = field.ControlType.GetHashCode();
		int hashControlSrc = field.ControlSrc.GetHashCode();
		int hashDataType = field.DataType.GetHashCode();
		int hashIsList = field.IsList.GetHashCode();
		int hashSortOrder = field.SortOrder.GetHashCode();
		int hashDefaultValue = field.DefaultValue.GetHashCode();
		int hashToken = field.Token.GetHashCode();
		int hashPreTokenString = field.PreTokenString.GetHashCode();
		int hashPostTokenString = field.PostTokenString.GetHashCode();
		int hashPreTokenStringWhenTrue = field.PreTokenStringWhenTrue.GetHashCode();
		int hashPostTokenStringWhenTrue = field.PostTokenStringWhenTrue.GetHashCode();
		int hashPreTokenStringWhenFalse = field.PreTokenStringWhenFalse.GetHashCode();
		int hashPostTokenStringWhenFalse = field.PostTokenStringWhenFalse.GetHashCode();
		int hashLabel = field.Label.GetHashCode();
		int hashHelpKey = field.HelpKey.GetHashCode();
		int hashRequired = field.Required.GetHashCode();
		int hashRequiredMessageFormat = field.RequiredMessageFormat.GetHashCode();
		int hashRegex = field.Regex.GetHashCode();
		int hashRegexMessageFormat = field.RegexMessageFormat.GetHashCode();
		int hashSearchable = field.Searchable.GetHashCode();
		int hashEditPageControlWrapperCssClass = field.EditPageControlWrapperCssClass.GetHashCode();
		int hashEditPageLabelCssClass = field.EditPageLabelCssClass.GetHashCode();
		int hashEditPageControlCssClass = field.EditPageControlCssClass.GetHashCode();
		int hashDatePickerIncludeTimeForDate = field.DatePickerIncludeTimeForDate.GetHashCode();
		int hashDatePickerShowMonthList = field.DatePickerShowMonthList.GetHashCode();
		int hashDatePickerShowYearList = field.DatePickerShowYearList.GetHashCode();
		int hashDatePickerYearRange = field.DatePickerYearRange.GetHashCode();
		int hashImageBrowserEmptyUrl = field.ImageBrowserEmptyUrl.GetHashCode();
		int hashOptions = field.Options.GetHashCode();
		int hashCheckBoxReturnBool = field.CheckBoxReturnBool.GetHashCode();
		int hashCheckBoxReturnValueWhenTrue = field.CheckBoxReturnValueWhenTrue.GetHashCode();
		int hashCheckBoxReturnValueWhenFalse = field.CheckBoxReturnValueWhenFalse.GetHashCode();
		int hashDateFormat = field.DateFormat.GetHashCode();
		int hashTextBoxMode = field.TextBoxMode.GetHashCode();
		int hashAttributes = field.Attributes.GetHashCode();
		int hashIsDeleted = field.IsDeleted.GetHashCode();
		int hashIsGlobal = field.IsGlobal.GetHashCode();
		int hashViewRoles = field.ViewRoles.GetHashCode();
		int hashEditRoles = field.EditRoles.GetHashCode();
		//Calculate the hash code for the field.
		return hashName
			 ^ hashDefinitionGuid
			 ^ hashDefinitionName
			 ^ hashControlType
			 ^ hashControlSrc
			 ^ hashDataType
			 ^ hashIsList
			 ^ hashLabel
			 ^ hashDefaultValue
			 ^ hashSortOrder
			 ^ hashHelpKey
			 ^ hashRequired
			 ^ hashRequiredMessageFormat
			 ^ hashRegex
			 ^ hashRegexMessageFormat
			 ^ hashToken
			 ^ hashPreTokenString
			 ^ hashPostTokenString
			 ^ hashPreTokenStringWhenTrue
			 ^ hashPostTokenStringWhenTrue
			 ^ hashPreTokenStringWhenFalse
			 ^ hashPostTokenStringWhenFalse
			 ^ hashSearchable
			 ^ hashEditPageControlWrapperCssClass
			 ^ hashEditPageLabelCssClass
			 ^ hashEditPageControlCssClass
			 ^ hashDatePickerIncludeTimeForDate
			 ^ hashDatePickerShowMonthList
			 ^ hashDatePickerShowYearList
			 ^ hashDatePickerYearRange
			 ^ hashImageBrowserEmptyUrl
			 ^ hashOptions
			 ^ hashCheckBoxReturnBool
			 ^ hashCheckBoxReturnValueWhenTrue
			 ^ hashCheckBoxReturnValueWhenFalse
			 ^ hashDateFormat
			 ^ hashTextBoxMode
			 ^ hashAttributes
			 ^ hashIsDeleted
			 ^ hashIsGlobal
			 ^ hashViewRoles
			 ^ hashEditRoles;
	}
}