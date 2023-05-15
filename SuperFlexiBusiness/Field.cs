using SuperFlexiData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SuperFlexiBusiness
{

	public class Field : Hashtable, IComparable<Field>
	{

		#region Constructors
		 
		public Field() { }

		public Field(Guid fieldGuid)
		{
			GetField(fieldGuid);
		}

		#endregion

		#region Private Properties

		private Guid siteGuid = Guid.Empty;
		private Guid featureGuid = Guid.Empty;
		private Guid definitionGuid = Guid.Empty;
		private Guid fieldGuid = Guid.Empty;
		private string definitionName = string.Empty;
		private string name = string.Empty;
		private string label = string.Empty;
		private string defaultValue = string.Empty;
		private string controlType = string.Empty;
		private string controlSrc = string.Empty;
		private int sortOrder = -1;
		private string helpKey = string.Empty;
		private bool required = false;
		private string requiredMessageFormat = string.Empty;
		private string regex = string.Empty;
		private string regexMessageFormat = string.Empty;
		private string token = "$_NONE_$";
		private string preTokenString = string.Empty;
		private string postTokenString = string.Empty;
		private string preTokenStringWhenTrue = string.Empty;
		private string postTokenStringWhenTrue = string.Empty;
		private string preTokenStringWhenFalse = string.Empty;
		private string postTokenStringWhenFalse = string.Empty;
		private bool searchable = true;
		private string editPageControlWrapperCssClass = "settingrow";
		private string editPageLabelCssClass = "settinglabel";
		private string editPageControlCssClass = "forminput"; //only used on controlType=text or checkbox
		private bool datePickerIncludeTimeForDate = false; //applies only when type = System.DateTime
		private bool datePickerShowMonthList = false;
		private bool datePickerShowYearList = false;
		private string datePickerYearRange = string.Empty;
		private string imageBrowserEmptyUrl = "~/Data/SiteImages/1x1.gif";
		//private string iSettingControlSettings = string.Empty;
		private string options = string.Empty;
		private bool checkBoxReturnBool = true;
		private string checkBoxReturnValueWhenTrue = string.Empty;
		private string checkBoxReturnValueWhenFalse = string.Empty;
		private string dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
		private string textBoxMode = "SingleLine";
		private string attributes = string.Empty;
        private bool isGlobal = false;
		private string viewRoles = "All Users;";
		private string editRoles = string.Empty;
		/// <summary>
		/// Used to flag a field as deleted when SuperFlexi:DeleteOrphanedFieldValues is false.
		/// </summary>
		private bool isDeleted = false;
		/// <summary>
		/// Not used yet!
		/// </summary>
		private string linkedField = string.Empty;
		#endregion

		#region Public Properties

		public Guid SiteGuid
		{
			get { return siteGuid; }
			set { siteGuid = value; }
		}
		public Guid FeatureGuid
		{
			get { return featureGuid; }
			set { featureGuid = value; }
		}
		public Guid DefinitionGuid
		{
			get { return definitionGuid; }
			set { definitionGuid = value; }
		}
		public Guid FieldGuid
		{
			get { return fieldGuid; }
			set { fieldGuid = value; }
		}
		public string DefinitionName
		{
			get { return definitionName; }
			set { definitionName = value; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		public string Label
		{
			get { return label; }
			set { label = value; }
		}
		public string DefaultValue
		{
			get { return defaultValue; }
			set { defaultValue = value; }
		}
		public string ControlType
		{
			get { return controlType; }
			set { controlType = value; }
		}
		public string ControlSrc
		{
			get { return controlSrc; }
			set { controlSrc = value; }
		}
		public string DataType { get; set; } = "string";
		public bool IsList { get; set; } = false;
		public int SortOrder
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}
		public string HelpKey
		{
			get { return helpKey; }
			set { helpKey = value; }
		}
		public bool Required
		{
			get { return required; }
			set { required = value; }
		}
		public string RequiredMessageFormat
		{
			get { return requiredMessageFormat; }
			set { requiredMessageFormat = value; }
		}
		public string Regex
		{
			get { return regex; }
			set { regex = value; }
		}
		public string RegexMessageFormat
		{
			get { return regexMessageFormat; }
			set { regexMessageFormat = value; }
		}
		public string Token
		{
			get { return token; }
			set { token = value; }
		}

		public string PreTokenString
		{
			get { return preTokenString; }
			set { preTokenString = value; }
		}

		public string PostTokenString
		{
			get { return postTokenString; }
			set { postTokenString = value; }
		}
		public string PreTokenStringWhenTrue
		{
			get { return preTokenStringWhenTrue; }
			set { preTokenStringWhenTrue = value; }
		}

		public string PostTokenStringWhenTrue
		{
			get { return postTokenStringWhenTrue; }
			set { postTokenStringWhenTrue = value; }
		}
		public string PreTokenStringWhenFalse
		{
			get { return preTokenStringWhenFalse; }
			set { preTokenStringWhenFalse = value; }
		}

		public string PostTokenStringWhenFalse
		{
			get { return postTokenStringWhenFalse; }
			set { postTokenStringWhenFalse = value; }
		}
		public bool Searchable
		{
			get { return searchable; }
			set { searchable = value; }
		}
		public string EditPageControlWrapperCssClass
		{
			get { return editPageControlWrapperCssClass; }
			set { editPageControlWrapperCssClass = value; }
		}
		public string EditPageLabelCssClass
		{
			get { return editPageLabelCssClass; }
			set { editPageLabelCssClass = value; }
		}
		public string EditPageControlCssClass
		{
			get { return editPageControlCssClass; }
			set { editPageControlCssClass = value; }
		}
		public bool DatePickerIncludeTimeForDate
		{
			get { return datePickerIncludeTimeForDate; }
			set { datePickerIncludeTimeForDate = value; }
		}
		public bool DatePickerShowMonthList
		{
			get { return datePickerShowMonthList; }
			set { datePickerShowMonthList = value; }
		}
		public bool DatePickerShowYearList
		{
			get { return datePickerShowYearList; }
			set { datePickerShowYearList = value; }
		}
		public string DatePickerYearRange
		{
			get { return datePickerYearRange; }
			set { datePickerYearRange = value; }
		}
		public string ImageBrowserEmptyUrl
		{
			get { return imageBrowserEmptyUrl; }
			set { imageBrowserEmptyUrl = value; }
		}
		//public string ISettingControlSettings
		//{
		//    get { return iSettingControlSettings; }
		//    set { iSettingControlSettings = value; }
		//}
		public string Options
		{
			get { return options; }
			set { options = value; }
		}
		public bool CheckBoxReturnBool 
		{
			get { return checkBoxReturnBool; }
			set { checkBoxReturnBool = value; }
		}
		public string CheckBoxReturnValueWhenTrue 
		{
			get { return checkBoxReturnValueWhenTrue; }
			set { checkBoxReturnValueWhenTrue = value; }
		}
		public string CheckBoxReturnValueWhenFalse
		{
			get { return checkBoxReturnValueWhenFalse; }
			set { checkBoxReturnValueWhenFalse = value; }
		}
		public string DateFormat
		{
			get { return dateFormat; }
			set { dateFormat = value; }
		}

		public string TextBoxMode
		{
			get { return textBoxMode; }
			set { textBoxMode = value; }
		}

		public string Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}
		/// <summary>
		/// Used to flag a field as deleted when SuperFlexi:DeleteOrphanedFieldValues is false.
		/// </summary>
		public bool IsDeleted
		{
			get { return isDeleted; }
			set { isDeleted = value; }
		}

        public bool IsGlobal
        {
            get { return isGlobal; }
            set { isGlobal = value; }
        }

		/// <summary>
		/// Not used!
		/// </summary>
		public string LinkedField
		{
			get { return linkedField; }
			set { linkedField = value; }
		}

		public string ViewRoles { get => viewRoles; set => viewRoles = value; }
		public string EditRoles { get => editRoles; set => editRoles = value; }
		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of field.
		/// </summary>
		/// <param name="fieldGuid"> fieldGuid </param>
		private void GetField(Guid fieldGuid)
		{
			using (IDataReader reader = DBFields.GetOne(fieldGuid))
			{
				PopulateFromReader(reader);
			}

		}


		private void PopulateFromReader(IDataReader reader)
		{
			if (reader.Read())
			{
				this.siteGuid = new Guid(reader["SiteGuid"].ToString());
				this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
				this.definitionGuid = new Guid(reader["DefinitionGuid"].ToString());
				this.fieldGuid = new Guid(reader["FieldGuid"].ToString());
				this.definitionName = reader["DefinitionName"].ToString();
				this.name = reader["Name"].ToString();
				this.label = reader["Label"].ToString();
				this.defaultValue = reader["DefaultValue"].ToString();
				this.controlType = reader["ControlType"].ToString();
				this.controlSrc = reader["ControlSrc"].ToString();
				this.DataType = reader["DataType"].ToString();
				this.IsList = Convert.ToBoolean(reader["IsList"]);
				this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
				this.helpKey = reader["HelpKey"].ToString();
				this.required = Convert.ToBoolean(reader["Required"]);
				this.requiredMessageFormat = reader["RequiredMessageFormat"].ToString();
				this.regex = reader["Regex"].ToString();
				this.regexMessageFormat = reader["RegexMessageFormat"].ToString();
				this.token = reader["Token"].ToString();
				this.preTokenString = reader["PreTokenString"].ToString();
				this.postTokenString = reader["PostTokenString"].ToString();
				this.preTokenStringWhenTrue = reader["PreTokenStringWhenTrue"].ToString();
				this.postTokenStringWhenTrue = reader["PostTokenStringWhenTrue"].ToString();
				this.preTokenStringWhenFalse = reader["PreTokenStringWhenFalse"].ToString();
				this.postTokenStringWhenFalse = reader["PostTokenStringWhenFalse"].ToString();
				this.searchable = Convert.ToBoolean(reader["Searchable"]);
				this.editPageControlWrapperCssClass = reader["EditPageControlWrapperCssClass"].ToString();
				this.editPageLabelCssClass = reader["EditPageLabelCssClass"].ToString();
				this.editPageControlCssClass = reader["EditPageControlCssClass"].ToString();
				this.datePickerIncludeTimeForDate = Convert.ToBoolean(reader["DatePickerIncludeTimeForDate"]);
				this.datePickerShowMonthList = Convert.ToBoolean(reader["DatePickerShowMonthList"]);
				this.datePickerShowYearList = Convert.ToBoolean(reader["DatePickerShowYearList"]);
				this.datePickerYearRange = reader["DatePickerYearRange"].ToString();
				this.imageBrowserEmptyUrl = reader["ImageBrowserEmptyUrl"].ToString();
				//this.iSettingControlSettings = reader["ISettingControlSettings"].ToString();
				this.options = reader["Options"].ToString();
				this.checkBoxReturnBool = Convert.ToBoolean(reader["CheckBoxReturnBool"]);
				this.checkBoxReturnValueWhenTrue = reader["CheckBoxReturnValueWhenTrue"].ToString();
				this.checkBoxReturnValueWhenFalse = reader["CheckBoxReturnValueWhenFalse"].ToString();
				this.textBoxMode = reader["TextBoxMode"].ToString();
				this.attributes = reader["Attributes"].ToString();
                this.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                this.IsGlobal = Convert.ToBoolean(reader["IsGlobal"]);
				this.ViewRoles = reader["ViewRoles"].ToString();
				this.EditRoles = reader["EditRoles"].ToString();
				if (String.IsNullOrWhiteSpace(this.ViewRoles ))
				{
					//mysql doesn't allow default values for TEXT columns so we do this because the field should never be empty
					this.ViewRoles = "AllUsers;";
				}
                string format = reader["DateFormat"].ToString().Trim();
				if (format.Length > 0)
				{
					try
					{
						string d = DateTime.Now.ToString(format, CultureInfo.CurrentCulture);
						this.dateFormat = format;
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
			this.fieldGuid = Guid.NewGuid();

			int rowsAffected = DBFields.Create(
				this.siteGuid,
				this.featureGuid,
				this.definitionGuid,
				this.fieldGuid,
				this.definitionName,
				this.name,
				this.label,
				this.defaultValue,
				this.controlType,
				this.controlSrc,
				this.DataType,
				this.IsList,
				this.sortOrder,
				this.helpKey,
				this.required,
				this.requiredMessageFormat,
				this.regex,
				this.regexMessageFormat,
				this.token,
				this.preTokenString,
				this.postTokenString,
				this.preTokenStringWhenTrue,
				this.postTokenStringWhenTrue,
				this.preTokenStringWhenFalse,
				this.postTokenStringWhenFalse,
				this.searchable,
				this.editPageControlWrapperCssClass,
				this.editPageLabelCssClass,
				this.editPageControlCssClass,
				this.datePickerIncludeTimeForDate,
				this.datePickerShowMonthList,
				this.datePickerShowYearList,
				this.datePickerYearRange,
				this.imageBrowserEmptyUrl,
				//this.iSettingControlSettings,
				this.options,
				this.checkBoxReturnBool,
				this.checkBoxReturnValueWhenTrue,
				this.checkBoxReturnValueWhenFalse,
				this.dateFormat,
				this.textBoxMode,
				this.attributes,
                this.IsGlobal,
				this.ViewRoles,
				this.EditRoles);

			return (rowsAffected > 0);

		}


		/// <summary>
		/// Updates this instance of field. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBFields.Update(
				this.siteGuid,
				this.featureGuid,
				this.definitionGuid,
				this.fieldGuid,
				this.definitionName,
				this.name,
				this.label,
				this.defaultValue,
				this.controlType,
				this.controlSrc,
				this.DataType,
				this.IsList,
				this.sortOrder,
				this.helpKey,
				this.required,
				this.requiredMessageFormat,
				this.regex,
				this.regexMessageFormat,
				this.token,
				this.preTokenString,
				this.postTokenString,
				this.preTokenStringWhenTrue,
				this.postTokenStringWhenTrue,
				this.preTokenStringWhenFalse,
				this.postTokenStringWhenFalse,
				this.searchable,
				this.editPageControlWrapperCssClass,
				this.editPageLabelCssClass,
				this.editPageControlCssClass,
				this.datePickerIncludeTimeForDate,
				this.datePickerShowMonthList,
				this.datePickerShowYearList,
				this.datePickerYearRange,
				this.imageBrowserEmptyUrl,
				//this.iSettingControlSettings,
				this.options,
				this.checkBoxReturnBool,
				this.checkBoxReturnValueWhenTrue,
				this.checkBoxReturnValueWhenFalse,
				this.dateFormat,
				this.textBoxMode,
				this.attributes,
				this.isDeleted,
                this.isGlobal,
				this.ViewRoles,
				this.EditRoles);

		}





		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of field. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.fieldGuid != Guid.Empty)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}


		public int CompareTo(Field field)
		{
			return this.SortOrder.CompareTo(field.SortOrder);
		}


		public bool IsDynamicListField()
		{
			if (ControlType == "DynamicRadioButtonList" || ControlType == "DynamicCheckBoxList")
			{
				return true;
			}
			return false;
		}

		public bool IsCheckBoxListField()
		{
			if (ControlType == "CheckBoxList" || ControlType == "DynamicCheckBoxList")
			{
				return true;
			}
			return false;
		}

		public bool IsRadioButtonListField()
		{
			if (ControlType == "RadioButtonList" || ControlType == "DynamicRadioButtonList")
			{
				return true;
			}
			return false;
		}

		public bool IsDateField()
		{
			if (ControlType == "DateTime" || ControlType == "Date" || (ControlType == "TextBox" && (TextBoxMode == "Date" || TextBoxMode == "DateTime" || TextBoxMode == "DateTimeLocal")))
			{
				return true;
			}
			return false;
		}


		#endregion

		#region Static Methods

		/// <summary>
		/// Deletes an instance of field. Returns true on success.
		/// </summary>
		/// <param name="fieldGuid"> fieldGuid </param>
		/// <returns>bool</returns>
		public static bool Delete(
			Guid fieldGuid)
		{
			return DBFields.Delete(
				fieldGuid);
		}

		/// <summary>
		/// Deletes Fields by Site. Returns true on success.
		/// </summary>
		public static bool DeleteBySite(Guid siteGuid)
		{
			return DBFields.DeleteBySite(siteGuid);
		}

		/// <summary>
		/// Deletes Fields by Field Definition. Returns true on success.
		/// </summary>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <returns>bool</returns>
		public static bool DeleteByDefinition(Guid definitionGuid)
		{
			return DBFields.DeleteByDefinition(definitionGuid);
		}

		/// <summary>
		/// Gets a count of Field. 
		/// </summary>
		public static int GetCount()
		{
			return DBFields.GetCount();
		}

		private static List<Field> LoadListFromReader(IDataReader reader)
		{
			List<Field> fieldList = new List<Field>();
			try
			{
				while (reader.Read())
				{
					Field field = new Field();
					field.siteGuid = new Guid(reader["SiteGuid"].ToString());
					field.featureGuid = new Guid(reader["FeatureGuid"].ToString());
					field.definitionGuid = new Guid(reader["DefinitionGuid"].ToString());
					field.fieldGuid = new Guid(reader["FieldGuid"].ToString());
					field.definitionName = reader["DefinitionName"].ToString();
					field.name = reader["Name"].ToString();
					field.label = reader["Label"].ToString();
					field.defaultValue = reader["DefaultValue"].ToString();
					field.controlType = reader["ControlType"].ToString();
					field.controlSrc = reader["ControlSrc"].ToString();
					field.DataType = reader["DataType"].ToString();
					field.IsList = Convert.ToBoolean(reader["IsList"]);
					field.sortOrder = Convert.ToInt32(reader["SortOrder"]);
					field.helpKey = reader["HelpKey"].ToString();
					field.required = Convert.ToBoolean(reader["Required"]);
					field.requiredMessageFormat = reader["RequiredMessageFormat"].ToString();
					field.regex = reader["Regex"].ToString();
					field.regexMessageFormat = reader["RegexMessageFormat"].ToString();
					field.token = reader["Token"].ToString();
					field.preTokenString = reader["PreTokenString"].ToString();
					field.postTokenString = reader["PostTokenString"].ToString();
					field.preTokenStringWhenTrue = reader["PreTokenStringWhenTrue"].ToString();
					field.postTokenStringWhenTrue = reader["PostTokenStringWhenTrue"].ToString();
					field.preTokenStringWhenFalse = reader["PreTokenStringWhenFalse"].ToString();
					field.postTokenStringWhenFalse = reader["PostTokenStringWhenFalse"].ToString();
					field.searchable = Convert.ToBoolean(reader["Searchable"]);
					field.editPageControlWrapperCssClass = reader["EditPageControlWrapperCssClass"].ToString();
					field.editPageLabelCssClass = reader["EditPageLabelCssClass"].ToString();
					field.editPageControlCssClass = reader["EditPageControlCssClass"].ToString();
					field.datePickerIncludeTimeForDate = Convert.ToBoolean(reader["DatePickerIncludeTimeForDate"]);
					field.datePickerShowMonthList = Convert.ToBoolean(reader["DatePickerShowMonthList"]);
					field.datePickerShowYearList = Convert.ToBoolean(reader["DatePickerShowYearList"]);
					field.datePickerYearRange = reader["DatePickerYearRange"].ToString();
					field.imageBrowserEmptyUrl = reader["ImageBrowserEmptyUrl"].ToString();
					//field.iSettingControlSettings = reader["ISettingControlSettings"].ToString();
					field.options = reader["Options"].ToString();
					field.checkBoxReturnBool = Convert.ToBoolean(reader["CheckBoxReturnBool"]);
					field.checkBoxReturnValueWhenTrue = reader["CheckBoxReturnValueWhenTrue"].ToString();
					field.checkBoxReturnValueWhenFalse = reader["CheckBoxReturnValueWhenFalse"].ToString();
					field.dateFormat = reader["dateFormat"].ToString();
					field.textBoxMode = reader["textBoxMode"].ToString();
					field.attributes = reader["Attributes"].ToString();
					field.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
					field.isGlobal = Convert.ToBoolean(reader["IsGlobal"]);
					field.viewRoles = reader["ViewRoles"].ToString();
					field.editRoles = reader["EditRoles"].ToString();

					if (String.IsNullOrWhiteSpace(field.viewRoles))
					{
						//mysql doesn't allow default values for TEXT columns so we do this because the field should never be empty
						field.viewRoles = "All Users;";
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
			totalPages = 1;
			IDataReader reader = DBFields.GetPage(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}


		public static bool MarkAsDeleted(Guid fieldGuid)
		{
			return DBFields.MarkAsDeleted(fieldGuid);
		}

        //public static Field GetByGuid(Guid fieldGuid)
        //{
        //    throw new NotImplementedException();
        //}
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
				//&& x.ISettingControlSettings == y.ISettingControlSettings
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
			//int hashISettingControlSettings = field.ISettingControlSettings.GetHashCode();
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
				 //^ hashISettingControlSettings
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
}





