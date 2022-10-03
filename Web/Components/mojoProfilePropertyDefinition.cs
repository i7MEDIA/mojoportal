// Author:             
// Created:            2006-10-29
// Last Modified:      2014-03-14

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Web.UI;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Controls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;


namespace mojoPortal.Web.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class mojoProfilePropertyDefinition
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(mojoProfilePropertyDefinition));
        public const string TimeOffsetHoursKey = "TimeOffsetHours";
        public const string TimeZoneIdKey = "TimeZoneId";

        #region Constructors

        public mojoProfilePropertyDefinition()
        {

        }

        #endregion

        #region Private Properties

        private String name = String.Empty;
        private String type = "System.String";
        private string iSettingControlSrc = string.Empty;
        private bool includeTimeForDate = false; //applies only when type = System.DateTime
        private bool allowMarkup = false;
        private String resourceFile = "ProfileResource";
        private String labelResourceKey = String.Empty;
        private bool lazyLoad = false;
        private bool requiredForRegistration = false;
        private bool showOnRegistration = false;
        private bool allowAnonymous = true;
        private bool visibleToAnonymous = false;
        private bool visibleToAuthenticated = true;
        private bool visibleToUser = true;
        private bool editableByUser = true;
        private String onlyAvailableForRoles = String.Empty;
        private String onlyVisibleForRoles = String.Empty;
        private bool includeHelpLink = false;
        private int maxLength = 0;
        private int rows = 0;
        private int columns = 0;
        private String regexValidationExpression = String.Empty;
        private String regexValidationErrorResourceKey = String.Empty;
        private string stateValue = string.Empty;

        private SettingsSerializeAs serializeAs = SettingsSerializeAs.String;
        private Collection<mojoProfilePropertyOption> optionList = new Collection<mojoProfilePropertyOption>();
        private String defaultValue = String.Empty;
        private string cssClass = string.Empty;

        #endregion

        #region Public Properties

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public String ISettingControlSrc
        {
            get { return iSettingControlSrc; }
            set { iSettingControlSrc = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public String StateValue
        {
            get { return stateValue; }
            set { stateValue = value; }
        }

        public bool IncludeTimeForDate
        {
            get { return includeTimeForDate; }
            set { includeTimeForDate = value; }
        }

        private bool datePickerShowMonthList = true;

        public bool DatePickerShowMonthList
        {
            get { return datePickerShowMonthList; }
            set { datePickerShowMonthList = value; }
        }

        private bool datePickerShowYearList = true;

        public bool DatePickerShowYearList
        {
            get { return datePickerShowYearList; }
            set { datePickerShowYearList = value; }
        }

        private string datePickerYearRange = string.Empty;
        public string DatePickerYearRange
        {
            get { return datePickerYearRange; }
            set { datePickerYearRange = value; }
        }




        public bool AllowMarkup
        {
            get { return allowMarkup; }
            set { allowMarkup = value; }
        }

        public String ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

        public String LabelResourceKey
        {
            get { return labelResourceKey; }
            set { labelResourceKey = value; }
        }

        public bool LazyLoad
        {
            get { return lazyLoad; }
            set { lazyLoad = value; }
        }

        public bool RequiredForRegistration
        {
            get { return requiredForRegistration; }
            set { requiredForRegistration = value; }
        }

        public bool ShowOnRegistration
        {
            get { return showOnRegistration; }
            set { showOnRegistration = value; }
        }

        public bool AllowAnonymous
        {
            get { return allowAnonymous; }
            set { allowAnonymous = value; }
        }

        public String OnlyAvailableForRoles
        {
            get { return onlyAvailableForRoles; }
            set { onlyAvailableForRoles = value; }
        }

        public String OnlyVisibleForRoles
        {
            get { return onlyVisibleForRoles; }
            set { onlyVisibleForRoles = value; }
        }

        public bool IncludeHelpLink
        {
            get { return includeHelpLink; }
            set { includeHelpLink = value; }
        }

        public bool VisibleToAnonymous
        {
            get { return visibleToAnonymous; }
            set { visibleToAnonymous = value; }
        }

        public bool VisibleToAuthenticated
        {
            get { return visibleToAuthenticated; }
            set { visibleToAuthenticated = value; }
        }

        public bool VisibleToUser
        {
            get { return visibleToUser; }
            set { visibleToUser = value; }
        }

        public bool EditableByUser
        {
            get { return editableByUser; }
            set { editableByUser = value; }
        }

        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        public int Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public String RegexValidationExpression
        {
            get { return regexValidationExpression; }
            set { regexValidationExpression = value; }
        }

        public String RegexValidationErrorResourceKey
        {
            get { return regexValidationErrorResourceKey; }
            set { regexValidationErrorResourceKey = value; }
        }

        public SettingsSerializeAs SerializeAs
        {
            get { return serializeAs; }
            set { serializeAs = value; }
        }

        public String DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public Collection<mojoProfilePropertyOption> OptionList
        {
            get
            {
                return optionList;
            }
        }

        #endregion

        #region UI Helper Methods

        public static void SetupPropertyControl(
            Page currentPage,
            Panel parentControl,
            mojoProfilePropertyDefinition propertyDefinition,
            Double legacyTimeZoneOffset,
            TimeZoneInfo timeZone,
            string siteRoot)
        {
            if (propertyDefinition.StateValue.Length > 0)
            {
                SetupPropertyControl(
                    currentPage,
                    parentControl,
                    propertyDefinition,
                    propertyDefinition.StateValue,
                    legacyTimeZoneOffset,
                    timeZone,
                    siteRoot);
            }
            else
            {
                SetupPropertyControl(
                    currentPage,
                    parentControl,
                    propertyDefinition,
                    propertyDefinition.DefaultValue,
                    legacyTimeZoneOffset,
                    timeZone,
                    siteRoot);

            }

        }

        
        public static void SetupPropertyControl(
            Page currentPage,
            Panel parentControl, 
            mojoProfilePropertyDefinition propertyDefinition,
            String propertyValue,
            Double legacyTimeZoneOffset,
            TimeZoneInfo timeZone,
            string siteRoot)
        {
            if (propertyValue == null)
            {
                propertyValue = String.Empty;
            }

            string validatorSkinID = "Profile";

            if (currentPage is mojoPortal.Web.UI.Pages.Register)
            {
                validatorSkinID = "Registration";
            }

            Literal rowOpenTag = new Literal();
            rowOpenTag.Text = "<div class='settingrow " + propertyDefinition.CssClass + "'>";
            parentControl.Controls.Add(rowOpenTag);

            SiteLabel label = new SiteLabel();
            label.ResourceFile = propertyDefinition.ResourceFile;
            // if key isn't in resource file use assume the resource hasn't been
            //localized and just use the key as the resource
            label.ShowWarningOnMissingKey = false;
            label.ConfigKey = propertyDefinition.LabelResourceKey;
            label.CssClass = "settinglabel";

            if (propertyDefinition.ISettingControlSrc.Length > 0)
            {
                Control c = null;
                if (propertyDefinition.ISettingControlSrc.EndsWith(".ascx"))
                {
                    c = currentPage.LoadControl(propertyDefinition.ISettingControlSrc);
                }
                else
                {
                    try
                    {
                        c = Activator.CreateInstance(System.Type.GetType(propertyDefinition.ISettingControlSrc)) as Control;
                       
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }

                if ((c != null) && (c is ISettingControl))
                {
                    c.ID = "isc" + propertyDefinition.Name;
                    parentControl.Controls.Add(label);

                    ISettingControl settingControl = (ISettingControl)c;

                    settingControl.SetValue(propertyValue);
                    parentControl.Controls.Add(c);

                    if (propertyDefinition.IncludeHelpLink)
                    {
                        AddHelpLink(parentControl, propertyDefinition);
                    }
                }
                
            }
            else if (propertyDefinition.OptionList.Count > 0)
            {
                if (propertyDefinition.Type == "CheckboxList")
                {
                    CheckBoxList cbl = CreateCheckBoxListQuestion(propertyDefinition, propertyValue);
                    cbl.ID = "cbl" + propertyDefinition.Name;
                    cbl.EnableTheming = false;
                    cbl.CssClass = "forminput";

                    cbl.TabIndex = 0;
                    label.ForControl = cbl.ID;
                    parentControl.Controls.Add(label);

                    parentControl.Controls.Add(cbl);

                    if (propertyDefinition.IncludeHelpLink)
                    {
                        AddHelpLink(parentControl, propertyDefinition);
                    }

                    if (propertyDefinition.RequiredForRegistration)
                    {
                        CheckBoxListValidator rfv = new CheckBoxListValidator();
                        rfv.SkinID = validatorSkinID;
                        rfv.ControlToValidate = cbl.ID;

                        rfv.ErrorMessage = string.Format(CultureInfo.InvariantCulture, Resources.ProfileResource.ProfileRequiredItemFormat,
                            ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey));

                        //rfv.Display = ValidatorDisplay.None;
                        rfv.ValidationGroup = "profile";
                        parentControl.Controls.Add(rfv);
                    }

                }
                else
                {
                    // add a dropdownlist with the options

                    DropDownList dd = CreateDropDownQuestion(propertyDefinition, propertyValue);
                    dd.ID = "dd" + propertyDefinition.Name;
                    dd.EnableTheming = false;
                    dd.CssClass = "forminput " + propertyDefinition.CssClass;

                    dd.TabIndex = 0;
                    label.ForControl = dd.ID;
                    parentControl.Controls.Add(label);

                    parentControl.Controls.Add(dd);

                    if (propertyDefinition.IncludeHelpLink)
                    {
                        AddHelpLink(parentControl, propertyDefinition);
                    }

                    if (propertyDefinition.RequiredForRegistration)
                    {
                        RequiredFieldValidator rfvDd = new RequiredFieldValidator();
                        rfvDd.SkinID = validatorSkinID;
                        rfvDd.ControlToValidate = dd.ID;
                        //if(dd.Items.Count > 0)
                        //{
                        //    rfvDd.InitialValue = dd.Items[0].Value;
                        //}
                        

                        rfvDd.ErrorMessage = string.Format(CultureInfo.InvariantCulture, Resources.ProfileResource.ProfileRequiredItemFormat,
                            ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey));

                 
                        rfvDd.ValidationGroup = "profile";
                        parentControl.Controls.Add(rfvDd);

                    }

                    if (propertyDefinition.RegexValidationExpression.Length > 0)
                    {
                        RegularExpressionValidator regexValidator = new RegularExpressionValidator();
                        regexValidator.SkinID = validatorSkinID;
                        regexValidator.ControlToValidate = dd.ID;
                        regexValidator.ValidationExpression = propertyDefinition.RegexValidationExpression;
                        regexValidator.ValidationGroup = "profile";
                        if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
                        {
                            regexValidator.ErrorMessage = ResourceHelper.GetResourceString(
                                propertyDefinition.ResourceFile,
                                propertyDefinition.RegexValidationErrorResourceKey);



                        }

                        //regexValidator.Display = ValidatorDisplay.None;
                        parentControl.Controls.Add(regexValidator);
                    }
                }

               
            }
            else
            {

                switch (propertyDefinition.Type)
                {
                    case "System.Boolean":
                        CheckBox checkBox = new CheckBox();
                        checkBox.TabIndex = 0;
                        checkBox.ID = "chk" + propertyDefinition.Name;
                        checkBox.CssClass = "forminput " + propertyDefinition.CssClass;
                        label.ForControl = checkBox.ID;
                        parentControl.Controls.Add(label);
                        parentControl.Controls.Add(checkBox);
                        if (propertyDefinition.IncludeHelpLink)
                        {
                            AddHelpLink(parentControl, propertyDefinition);
                        }

                        if (propertyValue.ToLower() == "true")
                        {
                            checkBox.Checked = true;
                        }
                        break;

                    case "System.DateTime":
                        // TODO: to really make this culture aware we should store the users
                        // culture as well and use the user's culture to 
                        // parse the date
                        DatePickerControl datePicker = CreateDatePicker(propertyDefinition, propertyValue, legacyTimeZoneOffset, timeZone, siteRoot);
                        
                        datePicker.TabIndex = 0;
                        datePicker.ID = "dp" + propertyDefinition.Name;
                        datePicker.CssClass = "forminput " + propertyDefinition.CssClass;
                        parentControl.Controls.Add(label);
                       
                        parentControl.Controls.Add(datePicker);
                        

                        if (propertyDefinition.IncludeHelpLink)
                        {
                            AddHelpLink(parentControl, propertyDefinition);
                        }

                        if (propertyDefinition.RequiredForRegistration)
                        {
                            RequiredFieldValidator rfvDate = new RequiredFieldValidator();
                            rfvDate.SkinID = validatorSkinID;
                            rfvDate.ControlToValidate = datePicker.ID;

                            rfvDate.ErrorMessage = string.Format(CultureInfo.InvariantCulture, Resources.ProfileResource.ProfileRequiredItemFormat,
                                ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey));

                            //rfvDate.Display = ValidatorDisplay.None;
                            rfvDate.ValidationGroup = "profile";
                            parentControl.Controls.Add(rfvDate);
                        }

                        if (propertyDefinition.RegexValidationExpression.Length > 0)
                        {
                            RegularExpressionValidator regexValidatorDate = new RegularExpressionValidator();
                            regexValidatorDate.SkinID = validatorSkinID;
                            regexValidatorDate.ControlToValidate = datePicker.ID;
                            regexValidatorDate.ValidationExpression = propertyDefinition.RegexValidationExpression;
                            regexValidatorDate.ValidationGroup = "profile";
                            if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
                            {
                                regexValidatorDate.ErrorMessage = ResourceHelper.GetResourceString(
                                    propertyDefinition.ResourceFile,
                                    propertyDefinition.RegexValidationErrorResourceKey);

                            }

                            //regexValidatorDate.Display = ValidatorDisplay.None;
                            parentControl.Controls.Add(regexValidatorDate);
                        }

                        break;

                    case "System.String":
                    default:

                        TextBox textBox = new TextBox();
                        textBox.TabIndex = 0;
                        textBox.ID = "txt" + propertyDefinition.Name;
                        textBox.CssClass = "forminput " + propertyDefinition.CssClass;
                        label.ForControl = textBox.ID;
                        parentControl.Controls.Add(label);
                  
                        if (propertyDefinition.MaxLength > 0)
                        {
                            textBox.MaxLength = propertyDefinition.MaxLength;
                        }

                        if (propertyDefinition.Columns > 0)
                        {
                            textBox.Columns = propertyDefinition.Columns;
                        }

                        if (propertyDefinition.Rows > 1)
                        {
                            textBox.TextMode = TextBoxMode.MultiLine;
                            textBox.Rows = propertyDefinition.Rows;
                        }

                        parentControl.Controls.Add(textBox);
                        if (propertyDefinition.IncludeHelpLink)
                        {
                            AddHelpLink(parentControl, propertyDefinition);
                        }

                        if (propertyValue.Length > 0)
                        {
                            textBox.Text = propertyValue;
                        }
                        if (propertyDefinition.RequiredForRegistration)
                        {
                            RequiredFieldValidator rfv = new RequiredFieldValidator();
                            rfv.SkinID = validatorSkinID;
                            rfv.ControlToValidate = textBox.ID;

                            rfv.ErrorMessage = string.Format(CultureInfo.InvariantCulture, Resources.ProfileResource.ProfileRequiredItemFormat,
                                ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey));

                            //rfv.Display = ValidatorDisplay.None;
                            rfv.ValidationGroup = "profile";
                            parentControl.Controls.Add(rfv);
                        }

                        if (propertyDefinition.RegexValidationExpression.Length > 0)
                        {
                            RegularExpressionValidator regexValidator = new RegularExpressionValidator();
                            regexValidator.SkinID = validatorSkinID;
                            regexValidator.ControlToValidate = textBox.ID;
                            regexValidator.ValidationExpression = propertyDefinition.RegexValidationExpression;
                            regexValidator.ValidationGroup = "profile";
                            if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
                            {
                                regexValidator.ErrorMessage = ResourceHelper.GetResourceString(
                                    propertyDefinition.ResourceFile,
                                    propertyDefinition.RegexValidationErrorResourceKey);

                              

                            }

                            //regexValidator.Display = ValidatorDisplay.None;
                            parentControl.Controls.Add(regexValidator);
                        }

                        break;

                }

            }

            
            Literal rowCloseTag = new Literal();
            rowCloseTag.Text = "</div>";
            parentControl.Controls.Add(rowCloseTag);

        }

        private static DropDownList CreateDropDownQuestion(
            mojoProfilePropertyDefinition propertyDefinition,
            String propertyValue)
        {
            DropDownList dd = new DropDownList();
           
            if (dd.Items.Count == 0)
            {
                foreach (mojoProfilePropertyOption option in propertyDefinition.OptionList)
                {
                    ListItem listItem = new ListItem();
                    listItem.Value = option.Value;
                    listItem.Text = option.TextResourceKey;
                    if (option.TextResourceKey.Length > 0)
                    {
                        if (HttpContext.Current != null)
                        {
                            Object obj = HttpContext.GetGlobalResourceObject(
                                propertyDefinition.ResourceFile, option.TextResourceKey);

                            if (obj != null)
                            {
                                listItem.Text = obj.ToString();
                            }
                        }
                    }

                    dd.Items.Add(listItem);
                }
            }

            ListItem defaultItem = dd.Items.FindByValue(propertyValue);
            if (defaultItem != null)
            {
                dd.ClearSelection();
                defaultItem.Selected = true;
            }

            return dd;


        }

        private static CheckBoxList CreateCheckBoxListQuestion(
            mojoProfilePropertyDefinition propertyDefinition,
            String propertyValue)
        {
            CheckBoxList cbl = new CheckBoxList();

            if (cbl.Items.Count == 0)
            {
                foreach (mojoProfilePropertyOption option in propertyDefinition.OptionList)
                {
                    ListItem listItem = new ListItem();
                    listItem.Value = option.Value;
                    listItem.Text = option.TextResourceKey;
                    if (option.TextResourceKey.Length > 0)
                    {
                        if (HttpContext.Current != null)
                        {
                            Object obj = HttpContext.GetGlobalResourceObject(
                                propertyDefinition.ResourceFile, option.TextResourceKey);

                            if (obj != null)
                            {
                                listItem.Text = obj.ToString();
                            }
                        }
                    }

                    cbl.Items.Add(listItem);
                }
            }

            ListItem item;
            if (propertyValue.Contains(","))
            {

                List<string> items = propertyValue.SplitOnChar(',');
                foreach (string s in items)
                {
                    item = cbl.Items.FindByValue(s);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
            else
            {

                ListItem defaultItem = cbl.Items.FindByValue(propertyValue);
                if (defaultItem != null)
                {
                    //cbl.ClearSelection();
                    defaultItem.Selected = true;
                }
            }

            return cbl;


        }

        private static DatePickerControl CreateDatePicker(
            mojoProfilePropertyDefinition propertyDefinition,
            String propertyValue,
            Double legacyTimeZoneOffset,
            TimeZoneInfo timeZone,
            string siteRoot)
        {
            DatePickerControl datePicker = new DatePickerControl();
            try
            {
                datePicker.SkinID = propertyDefinition.Name.Replace(" ", string.Empty);
            }
            catch (ArgumentException) { }
            datePicker.ID = "dp" + propertyDefinition.Name;
            datePicker.ShowMonthList = propertyDefinition.DatePickerShowMonthList;
            datePicker.ShowYearList = propertyDefinition.DatePickerShowYearList;
            if (propertyDefinition.DatePickerYearRange.Length > 0)
            {
                datePicker.YearRange = propertyDefinition.DatePickerYearRange;
            }
            

  
            if (propertyValue.Length > 0)
            {
                DateTime dt;
                if (DateTime.TryParse(
                    propertyValue,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.AdjustToUniversal, out dt))
                {
                    
                    if (propertyDefinition.IncludeTimeForDate)
                    {
                        if (timeZone != null)
                        {
                            dt = dt.ToLocalTime(timeZone);
                        }
                        else
                        {
                            dt = dt.AddHours(legacyTimeZoneOffset);
                        }
                        datePicker.Text = dt.ToString("g");

                    }
                    else
                    {
                        datePicker.Text = dt.Date.ToShortDateString();
                    }
                }
                else
                {
                    datePicker.Text = propertyValue;
                }
            }
            else
            {
                if (propertyDefinition.DefaultValue.Length > 0)
                {
                    datePicker.Text = propertyDefinition.DefaultValue;
                }
            }


            datePicker.ShowTime = propertyDefinition.IncludeTimeForDate;

            return datePicker;

        }

        public static void SetupReadOnlyPropertyControl(
            Panel parentControl,
            mojoProfilePropertyDefinition propertyDefinition,
            String propertyValue,
            Double legacyTimeZoneOffset,
            TimeZoneInfo timeZone)
        {
            if (propertyValue == null)
            {
                propertyValue = String.Empty;
            }

            Literal rowOpenTag = new Literal();
            rowOpenTag.Text = "<div class='settingrow " + propertyDefinition.CssClass + "'>";
            parentControl.Controls.Add(rowOpenTag);

            SiteLabel label = new SiteLabel();
            label.ResourceFile = propertyDefinition.ResourceFile;
            // if key isn't in resource file use assume the resource hasn't been
            //localized and just use the key as the resource
            label.ShowWarningOnMissingKey = false;
            label.ConfigKey = propertyDefinition.LabelResourceKey;
            label.CssClass = "settinglabel";
            label.UseLabelTag = false;
            parentControl.Controls.Add(label);

            Label propertyLabel = new Label();
            parentControl.Controls.Add(propertyLabel);

            bool didLoadControl = false;

            if (propertyDefinition.ISettingControlSrc.Length > 0)
            {
                Control c = parentControl.Page.LoadControl(propertyDefinition.ISettingControlSrc);

                if ((c != null) && (c is IReadOnlySettingControl))
                {
                    c.ID = "isc" + propertyDefinition.Name;
                    parentControl.Controls.Add(label);

                    IReadOnlySettingControl settingControl = (IReadOnlySettingControl)c;

                    settingControl.SetReadOnlyValue(propertyValue);
                    parentControl.Controls.Add(c);
                    didLoadControl = true;
                   
                }
            }

            if (!didLoadControl)
            {
                if ((propertyDefinition.OptionList.Count > 0) && (propertyDefinition.Type != "CheckboxList"))
                {
                    DropDownList dd = new DropDownList();
                    dd.ID = "dd" + propertyDefinition.Name;

                    foreach (mojoProfilePropertyOption option in propertyDefinition.OptionList)
                    {
                        ListItem listItem = new ListItem();
                        listItem.Value = option.Value;
                        listItem.Text = option.TextResourceKey;
                        if (option.TextResourceKey.Length > 0)
                        {
                            if (HttpContext.Current != null)
                            {
                                Object obj = HttpContext.GetGlobalResourceObject(
                                    propertyDefinition.ResourceFile, option.TextResourceKey);

                                if (obj != null)
                                {
                                    listItem.Text = obj.ToString();
                                }
                            }
                        }

                        dd.Items.Add(listItem);
                    }

                    ListItem defaultItem = dd.Items.FindByValue(propertyValue);
                    if (defaultItem != null)
                    {
                        propertyLabel.Text = HttpUtility.HtmlEncode(defaultItem.Text);
                    }
                    //dd.Enabled = false;

                }
                else
                {
                    switch (propertyDefinition.Type)
                    {

                        case "System.Boolean":
                            Literal litBool = new Literal();

                            string imgVal = propertyValue.ToLower();
                            if (imgVal.Length == 0) { imgVal = propertyDefinition.DefaultValue.ToLower(); }
                            if (imgVal.Length == 0) { imgVal = "false"; }

                            litBool.Text = "<img src='/Data/SiteImages/"
                                + imgVal + ".png' alt='" + propertyDefinition.Name + "' />";

                            parentControl.Controls.Add(litBool);

                            break;

                        case "System.DateTime":
                            Literal litDateTime = new Literal();
                            DateTime dt;
                            if (DateTime.TryParse(
                                propertyValue,
                                CultureInfo.CurrentCulture,
                                DateTimeStyles.AdjustToUniversal, out dt))
                            {

                                if (propertyDefinition.IncludeTimeForDate)
                                {
                                    dt = dt.AddHours(legacyTimeZoneOffset);
                                    litDateTime.Text = dt.ToString();
                                }
                                else
                                {
                                    litDateTime.Text = dt.Date.ToShortDateString();
                                }
                            }
                            else
                            {
                                litDateTime.Text = SecurityHelper.PreventCrossSiteScripting(propertyValue);
                            }

                            parentControl.Controls.Add(litDateTime);
                            break;

                        case "System.String":
                        default:

                            if (propertyValue.Length > 0)
                            {
                                if (propertyDefinition.AllowMarkup)
                                {
                                    propertyLabel.Text = SecurityHelper.PreventCrossSiteScripting(propertyValue);
                                }
                                else
                                {
                                    if (propertyDefinition.Name.ToLower().IndexOf("url") > -1)
                                    {
                                        Literal litLink = new Literal();
                                        litLink.Text = "<a href='" + HttpUtility.HtmlEncode(propertyValue)
                                            + "'>" + HttpUtility.HtmlEncode(propertyValue)
                                            + "</a>";

                                        parentControl.Controls.Add(litLink);

                                    }
                                    else
                                    {
                                        propertyLabel.Text = HttpUtility.HtmlEncode(propertyValue);
                                    }
                                }
                            }
                            else
                            {
                                propertyLabel.Text = "&nbsp;";
                            }

                            break;

                    }

                }

            }

            if (propertyLabel.Text.Length > 0)
            {
                parentControl.Controls.Add(propertyLabel);
            }


            Literal rowCloseTag = new Literal();
            rowCloseTag.Text = "</div>";
            parentControl.Controls.Add(rowCloseTag);

        }

        private static void AddHelpLink(
            Panel parentControl,
            mojoProfilePropertyDefinition propertyDefinition)
        {
            Literal litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

            mojoHelpLink helpLinkButton = new mojoHelpLink();
            helpLinkButton.HelpKey = "profile-" + propertyDefinition.Name.ToLower() + "-help";
            parentControl.Controls.Add(helpLinkButton);

            litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

        }

        public static void SaveProperty(
            SiteUser siteUser, 
            Panel parentControl, 
            mojoProfilePropertyDefinition propertyDefinition,
            Double legacyTimeZoneOffset,
            TimeZoneInfo timeZone)
        {
            String controlID;
            Control control;

            if (propertyDefinition.ISettingControlSrc.Length > 0)
            {
                controlID = "isc" + propertyDefinition.Name;
                control = parentControl.FindControl(controlID);
                if (control != null)
                {
                    siteUser.SetProperty(
                        propertyDefinition.Name,
                        ((ISettingControl)control).GetValue(),
                        propertyDefinition.SerializeAs,
                        propertyDefinition.LazyLoad);
                }

            }
            else
            {

                switch (propertyDefinition.Type)
                {
                    case "System.Boolean":

                        controlID = "chk" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            siteUser.SetProperty(
                                propertyDefinition.Name,
                                ((CheckBox)control).Checked,
                                propertyDefinition.SerializeAs,
                                propertyDefinition.LazyLoad);

                        }

                        break;

                    case "System.DateTime":

                        controlID = "dp" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            DatePickerControl dp = (DatePickerControl)control;
                            if (dp.Text.Length > 0)
                            {
                                DateTime dt;
                                if (DateTime.TryParse(
                                    dp.Text,
                                    CultureInfo.CurrentCulture,
                                    DateTimeStyles.AdjustToUniversal, out dt))
                                {
                                    
                                    if (propertyDefinition.IncludeTimeForDate)
                                    {
                                        if (timeZone != null)
                                        {
                                            dt = dt.ToUtc(timeZone);
                                        }
                                        else
                                        {
                                            dt = dt.AddHours(-legacyTimeZoneOffset);
                                        }

                                        if (propertyDefinition.Name == "DateOfBirth")
                                        {
                                            siteUser.DateOfBirth = dt.Date;
                                            siteUser.Save();
                                        }
                                        else
                                        {
                                            siteUser.SetProperty(
                                                propertyDefinition.Name,
                                                dt.ToString(),
                                                propertyDefinition.SerializeAs,
                                                propertyDefinition.LazyLoad);
                                        }
                                    }
                                    else
                                    {

                                        if(propertyDefinition.Name == "DateOfBirth")
                                        {
                                            siteUser.DateOfBirth = dt.Date;
                                            siteUser.Save();
                                        }
                                        else
                                        {
                                            siteUser.SetProperty(
                                            propertyDefinition.Name,
                                            dt.Date.ToShortDateString(),
                                            propertyDefinition.SerializeAs,
                                            propertyDefinition.LazyLoad);
                                        }
                                        
                                    }

                                }
                                else
                                {
                                    
                                        siteUser.SetProperty(
                                        propertyDefinition.Name,
                                        dp.Text,
                                        propertyDefinition.SerializeAs,
                                        propertyDefinition.LazyLoad);
                                    
                                }

                            }
                            else // blank
                            {
                                if (propertyDefinition.Name == "DateOfBirth")
                                {
                                    siteUser.DateOfBirth = DateTime.MinValue;
                                    siteUser.Save();
                                }
                                else
                                {
                                    siteUser.SetProperty(
                                        propertyDefinition.Name,
                                        String.Empty,
                                        propertyDefinition.SerializeAs,
                                        propertyDefinition.LazyLoad);
                                }
                            }
                        }

                        break;

                    case "System.String":
                    default:

                        if (propertyDefinition.OptionList.Count > 0)
                        {
                            if (propertyDefinition.Type == "CheckboxList")
                            {
                                controlID = "cbl" + propertyDefinition.Name;
                                control = parentControl.FindControl(controlID);
                                if (control != null)
                                {
                                    if (control is CheckBoxList)
                                    {
                                        CheckBoxList cbl = (CheckBoxList)control;
                                        
                                        siteUser.SetProperty(
                                            propertyDefinition.Name,
                                            cbl.Items.SelectedItemsToCommaSeparatedString(),
                                            propertyDefinition.SerializeAs,
                                            propertyDefinition.LazyLoad);
                                        
                                    }
                                }

                            }
                            else
                            {

                                controlID = "dd" + propertyDefinition.Name;
                                control = parentControl.FindControl(controlID);
                                if (control != null)
                                {
                                    if (control is DropDownList)
                                    {
                                        DropDownList dd = (DropDownList)control;
                                        if (dd.SelectedIndex > -1)
                                        {
                                            siteUser.SetProperty(
                                                propertyDefinition.Name,
                                                dd.SelectedValue,
                                                propertyDefinition.SerializeAs,
                                                propertyDefinition.LazyLoad);
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            controlID = "txt" + propertyDefinition.Name;
                            control = parentControl.FindControl(controlID);
                            if (control != null)
                            {
                                siteUser.SetProperty(
                                    propertyDefinition.Name,
                                    ((TextBox)control).Text,
                                    propertyDefinition.SerializeAs,
                                    propertyDefinition.LazyLoad);
                            }

                        }

                        break;

                }
            }

        }

        public static void SavePropertyDefault(
            SiteUser siteUser,
            mojoProfilePropertyDefinition propertyDefinition)
        {

            siteUser.SetProperty(
                            propertyDefinition.Name,
                            propertyDefinition.DefaultValue,
                            propertyDefinition.SerializeAs,
                            propertyDefinition.LazyLoad);


        }


        


        public static void LoadState(
            Panel parentControl,
            mojoProfilePropertyDefinition propertyDefinition)
        {
            String controlID;
            Control control;

            switch (propertyDefinition.Type)
            {
                case "System.Boolean":

                    controlID = "chk" + propertyDefinition.Name;
                    control = parentControl.FindControl(controlID);
                    if (control != null)
                    {
                        propertyDefinition.StateValue = ((CheckBox)control).Checked.ToString();
                        
                    }

                    break;

                case "System.DateTime":

                    controlID = "dp" + propertyDefinition.Name;
                    control = parentControl.FindControl(controlID);
                    if (control != null)
                    {
                        DatePickerControl dp = (DatePickerControl)control;
                        if (dp.Text.Length > 0)
                        {
                            propertyDefinition.StateValue = dp.Text;

                        }
                    }

                    break;

                case "System.String":
                default:

                    if (propertyDefinition.OptionList.Count > 0)
                    {
                        if (propertyDefinition.Type == "CheckboxList")
                        {
                            controlID = "cbl" + propertyDefinition.Name;
                            control = parentControl.FindControl(controlID);
                            if (control != null)
                            {
                                if (control is CheckBoxList)
                                {
                                    CheckBoxList cbl = (CheckBoxList)control;
                                    propertyDefinition.StateValue = cbl.Items.SelectedItemsToCommaSeparatedString();
 
                                }
                            }

                        }
                        else
                        {
                            controlID = "dd" + propertyDefinition.Name;
                            control = parentControl.FindControl(controlID);
                            if (control != null)
                            {
                                if (control is DropDownList)
                                {
                                    DropDownList dd = (DropDownList)control;
                                    if (dd.SelectedIndex > -1)
                                    {
                                        propertyDefinition.StateValue = dd.SelectedValue;

                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        controlID = "txt" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            propertyDefinition.StateValue = ((TextBox)control).Text;
                            
                        }

                    }

                    break;

            }

        }

        #endregion



    }
}
