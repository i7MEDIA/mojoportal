//  Author:                 Joe Audette
//	Created:			    2010-05-25
//	Last Modified:		    2013-12-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// this is a control used to attach a jquery datepicker to a TextBox and configure it
    /// http://docs.jquery.com/UI/API/1.8/Datepicker
    /// 
    /// http://trentrichardson.com/examples/timepicker/
    /// 
    /// </summary>
    public class jDatePicker : TextBox
    {
        
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (WebConfigSettings.DisablejQueryUI) { return; }


            //if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            if (CssClass.Length == 0) 
            { 
                if(showTime)
                {
                    CssClass = "mediumtextbox forminput datepicker"; 
                }
                else
                {
                    CssClass = "normaltextbox forminput datepicker"; 
                }
                
            }

            if (autoLocalize)
            {
                langCode = GetSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                if (CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator.Length == 0)
                {
                    clockHours = "24";
                }
                else
                {
                    amDesignator = CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator;
                    pmDesignator = CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator;
                }
                if (langCode.StartsWith("zh")) { clockHours = "24"; }
            }

            if (langCode != "en")
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "jqdatei18n", "\n<script src=\""
                + GetJQueryUIBasePath(Page) + "i18n/jquery.ui.datepicker-" + langCode + ".js" + "\" type=\"text/javascript\" ></script>");

            }

            if (useOldTimePicker)
            {
                if (showTime) { SetupTimeScript(); }

                SetupOldScript();
            }
            else
            {
                SetupScript();
            }

        }

        private void SetupScript()
        {
            if (showTime)
            {
                SetupTimePickerScript();
            }

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");
            script.Append("$(document).ready(function() { ");

            if (showTimeOnly)
            {
                script.Append("$('#" + ClientID + "').timepicker(");
            }
            else if (showTime)
            {
                script.Append("$('#" + ClientID + "').datetimepicker(");
            }
            else
            {
                script.Append("$('#" + ClientID + "').datepicker(");
            }


            script.Append("{");

            script.Append("showOn: '" + showOn + "'");
            if (buttonImage.Length > 0)
            {
                //script.Append(",buttonImage: '" + Page.ResolveUrl("/Data/SiteImages/calendar.png") + "' ");
                script.Append(",buttonImage: '" + Page.ResolveUrl(buttonImage) + "' ");
            }

            if (buttonImageOnly) { script.Append(",buttonImageOnly: true"); }
            script.Append(",buttonText:'...'");

            script.Append(",duration: ''");

            if (showTime)
            {
                // in .NET M means month 1 -12 with no leading zero
                // in javascript it means month name like Dec
                // we need m for month withno leading zero, so we need to lower it
                // also in C# yyyy means four digit year but in js yy means 4 digit year so
                // we must replace yyyy with yy
                string dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("M", "m").Replace("yyyy","yy");
                //http://trentrichardson.com/examples/timepicker/
                //in .NET tt means use AM or PM but in this js it means am or pm
                // wee need TT to get AM or PM
                string timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt","TT");
                
                script.Append(",dateFormat:'" + dateFormat + "'");
                script.Append(",timeFormat:'" + timeFormat + "'");

                if (clockHours != "24")
                {
                   // script.Append(",'ampm': true ");
                     // localize the labels
                    script.Append(",timeOnlyTitle:'" + Resource.TimePickerTimeOnlyTitle + "'");
                    script.Append(",timeText:'" + Resource.TimePickerTimeText + "'");
                    script.Append(",hourText:'" + Resource.TimePickerHourText + "'");
                    script.Append(",minuteText:'" + Resource.TimePickerMinuteText + "'");
                    script.Append(",secondText:'" + Resource.TimePickerSecondText + "'");
                    script.Append(",currentText:'" + Resource.TimePickerCurrentText + "'");
                    script.Append(",closeText:'" + Resource.TimePickerCloseText + "'");
                }

            }

            if (changeMonth) { script.Append(",changeMonth:true"); }
            if (changeYear) { script.Append(",changeYear:true,yearRange:'" + yearRange + "'"); }
            
            if (showWeek) { script.Append(",showWeek:true"); }
            if (firstDay > -1) { script.Append(",firstDay:" + firstDay.ToInvariantString()); }
            if (calculateWeek.Length > 0) { script.Append(",calculateWeek:" + calculateWeek); }

            script.Append("}");
            script.Append("); ");

            script.Append(" });");

            script.Append("\n ");
            script.Append(" </script>");

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "setup" + this.ClientID,
                script.ToString(),
                false);

            if (showTime)
            {
                switch(CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                {
                    case "en":
                    
                        //do nothing en is the edefault
                        break;

                    default:
                        string timeCulture = GetTimePickerSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                        string localizationFile = WebConfigSettings.TimePickerScriptLocaleBaseUrl + "jquery-ui-timepicker-" + timeCulture + ".js";

                        if(File.Exists(HostingEnvironment.MapPath(localizationFile)))
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page),
                                "jqtimelocal", "\n<script src=\""
                                + Page.ResolveUrl(localizationFile)
                                + "\" type=\"text/javascript\"></script>", false);
                        }

                        

                        break;
                }

            }

        }

        private void SetupTimePickerScript()
        {
            //http://trentrichardson.com/examples/timepicker/

            ScriptManager.RegisterClientScriptBlock(
                this,
                typeof(Page),
                        "jqtimecore", "\n<script src=\""
                        + Page.ResolveUrl(WebConfigSettings.TimePickerScriptUrl) + "\" type=\"text/javascript\"></script>", false);

        }

        private void SetupOldScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");
            script.Append("$(document).ready(function() { ");

            script.Append("$('#" + ClientID + "').datepicker(");
            script.Append("{");

            script.Append("showOn: '" + showOn + "'");
            if (buttonImage.Length > 0)
            {
                //script.Append(",buttonImage: '" + Page.ResolveUrl("/Data/SiteImages/calendar.png") + "' ");
                script.Append(",buttonImage: '" + Page.ResolveUrl(buttonImage) + "' ");
            }

            if (buttonImageOnly) { script.Append(",buttonImageOnly: true"); }
            script.Append(",buttonText:'...'");

            script.Append(",duration: ''");

            if (showTime)
            {
                script.Append(",showTime: true");
                if (!constrainInput) { script.Append(",constrainInput: false"); }
                if (clockHours == "24")
                {
                    script.Append(",'time24h': true ");
                   

                }

            }

            if (changeMonth) { script.Append(",changeMonth:true"); }
            if (changeYear) { script.Append(",changeYear:true,yearRange:'" + yearRange + "'"); }

            
            script.Append("}");
            script.Append("); ");

            script.Append(" });");

            script.Append("\n ");
            script.Append(" </script>");

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "setup" + this.ClientID,
                script.ToString());

        }

        private void SetupTimeScript()
        {
            //http://puna.net.nz/timepicker.htm
            string is24HourClock = "false";

            if (clockHours == "24")
            {
                is24HourClock = "true";
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jtimepickerresources", "\n<script type=\"text/javascript\"> var jqtDoneLabel = '" + doneLabel + "'; var jqtHourLabel = '" + hourLabel + "'; var jqtMinuteLabel = '" + minuteLabel + "'; var jqt24Hour = " + is24HourClock + "; var jqtAM = '" + amDesignator + "'; var jqtPM = '" + pmDesignator + "'; </script>");

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jtimepicker", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/timepicker.mod.js") + "\" type=\"text/javascript\" ></script>");

        }

        public static string GetJQueryUIBasePath(Page page)
        {

            if (WebConfigSettings.UseGoogleCDN)
            {
                return  "//ajax.googleapis.com/ajax/libs/jqueryui/" + WebConfigSettings.GoogleCDNjQueryUIVersion + "/";
            }

            if (ConfigurationManager.AppSettings["jQueryUIBasePath"] != null)
            {
                string jqueryBasePath = ConfigurationManager.AppSettings["jQueryUIBasePath"];
                return page.ResolveUrl(jqueryBasePath);

            }

            return string.Empty;
        }

        public static string GetSupportedLangCode(string cultureName, string cultureCode)
        {
            // a list of supported languages can be found here:
            // http://jquery-ui.googlecode.com/svn/trunk/ui/i18n/

            switch (cultureName)
            {
                //case "de-CH":
                case "en-GB":
                
                case "fr-CH":
                case "nl-BE":
                case "pt-BR":
                case "sr-SR":
                case "zh-CN":
                case "zh-HK":
                case "zh-TW":
                
                    return cultureName;

                case "en-NZ":
                case "en-AU":

                    switch (WebConfigSettings.GoogleCDNjQueryUIVersion)
                    {
                        case "1.8.6":
                        case "1.8.7":
                        case "1.8.8":
                            //not suported in older versions
                            break;

                        default:
                            return cultureName;

                    }

                    break;

                

            }

            switch (cultureCode)
            {
                case "af":
                case "ar":
                case "az":
                case "bg":
                case "bs":
                case "ca":
                case "cs":
                case "da":
                case "de":
                case "el":
                case "eo":
                case "es":
                case "et":
                case "eu":
                case "fa":
                case "fi":
                case "fo":
                case "fr":
                case "he":
                case "hr":
                case "hu":
                case "hy":
                case "id":
                case "is":
                case "it":
                case "ja":
                case "ko":
                case "lt":
                case "lv":
                case "ms":
                case "nl":
                case "no":
                case "pl":
                case "pt":
                case "ro":
                case "ru":
                case "sk":
                case "sl":
                case "sq":
                case "sr":
                case "sv":
                case "ta":
                case "tr":
                case "uk":
                case "vi":

                    return cultureCode;


                default:
                    return "en";

            }

            
        }

        public static string GetTimePickerSupportedLangCode(string cultureName, string cultureCode)
        {
            // a list of supported languages can be found here:
            // http://jquery-ui.googlecode.com/svn/trunk/ui/i18n/

            switch (cultureName)
            {
                
                case "pt-BR":
                case "sr-RS":
                case "sr-YU":
                case "zh-CN":
                case "zh-TW":

                    return cultureName;

            }

            return cultureCode;

        }



        //private string protocol = "http";

        private bool showTime = false;

        /// <summary>
        /// Disables (true) or enables (false) the timepicker. Can be set when initialising (first creating) the datepicker.
        /// </summary>
        public bool ShowTime
        {
            get { return showTime; }
            set { showTime = value; }
        }

        private bool showTimeOnly = false;
        /// <summary>
        /// if true the control will be rendered only as a time picker with not datepicker
        /// </summary>
        public bool ShowTimeOnly
        {
            get { return showTimeOnly; }
            set { showTimeOnly = value; }
        }

        private string clockHours = "12";
        /// <summary>
        /// 12 or 24
        /// </summary>
        public string ClockHours
        {
            get { return clockHours; }
            set { clockHours = value; }
        }

        private bool autoLocalize = true;

        public bool AutoLocalize
        {
            get { return autoLocalize; }
            set { autoLocalize = value; }
        }

        private bool useOldTimePicker = false;

        /// <summary>
        /// the old time picker only works with jQueryUI 1.8.6, if a newer version is used it does not work
        /// therefore we now use the newer one that does work. If you want to use the old one you could add this in theme.skin:
        /// <portal:jDatePicker runat="server"  UseOldTimePicker="true" />
        /// </summary>
        public bool UseOldTimePicker
        {
            get { return useOldTimePicker; }
            set { useOldTimePicker = value; }
        }

        private string langCode = "en";

        public string LangCode
        {
            get { return langCode; }
            set { langCode = value; }
        }

        private string doneLabel = "Done";

        /// <summary>
        /// this allows localizing the Done button in the time picker
        /// </summary>
        public string DoneLabel
        {
            get { return doneLabel; }
            set { doneLabel = value; }
        }

        private string hourLabel = "Hour";

        /// <summary>
        /// this allows localizing the Hour label in the time picker
        /// </summary>
        public string HourLabel
        {
            get { return hourLabel; }
            set { hourLabel = value; }
        }

        private string minuteLabel = "Minute";

        /// <summary>
        /// this allows localizing the Minute label in the time picker
        /// </summary>
        public string MinuteLabel
        {
            get { return minuteLabel; }
            set { minuteLabel = value; }
        }

        private string amDesignator = "AM";

        /// <summary>
        /// this allows localizing the AM label in the time picker
        /// </summary>
        public string AmDesignator
        {
            get { return amDesignator; }
            set { amDesignator = value; }
        }

        private string pmDesignator = "PM";

        /// <summary>
        /// this allows localizing the PM label in the time picker
        /// </summary>
        public string PmDesignator
        {
            get { return pmDesignator; }
            set { pmDesignator = value; }
        }

        //TODO: could implement use of more of these properties


        ////configuration options for jQuery DatePicker
        ////http://docs.jquery.com/UI/API/1.8/Datepicker

        //private string altControlId = string.Empty;

        ///// <summary>
        ///// the Client side id of the control (TextBox) to recieve the date from the date picker
        ///// </summary>
        //public string AltControlID
        //{
        //    get { return altControlId; }
        //    set { altControlId = value; }
        //}

        //private string altTimeField = string.Empty;
        ///// <summary>
        ///// Selector for an alternate field to store time into
        ///// </summary>
        //public string AltTimeField
        //{
        //    get { return altTimeField; }
        //    set { altTimeField = value; }
        //}

        //private bool disabled = false;

        ///// <summary>
        ///// Disables (true) or enables (false) the datepicker. Can be set when initialising (first creating) the datepicker.
        ///// </summary>
        //public bool Disabled
        //{
        //    get { return disabled; }
        //    set { disabled = value; }
        //}

        //private string altFormat = string.Empty;

        ///// <summary>
        ///// The dateFormat to be used for the altField option. This allows one date format to be shown to the user for selection purposes,
        ///// while a different format is actually sent behind the scenes. For a full list of the possible formats see the formatDate function
        ///// http://docs.jquery.com/UI/Datepicker#option-dateFormat
        ///// http://docs.jquery.com/UI/Datepicker/formatDate
        ///// </summary>
        //public string AltFormat
        //{
        //    get { return altFormat; }
        //    set { altFormat = value; }
        //}

        //private string appendText = string.Empty;
        ///// <summary>
        ///// The text to display after each date field, e.g. to show the required format.
        ///// </summary>
        //public string AppendText
        //{
        //    get { return appendText; }
        //    set { appendText = value; }
        //}

        //private bool autoSize = false;

        ///// <summary>
        ///// Set to true to automatically resize the input field to accomodate dates in the current dateFormat.
        ///// </summary>
        //public bool AutoSize
        //{
        //    get { return autoSize; }
        //    set { autoSize = value; }
        //}

        private string buttonImage = string.Empty;
        /// <summary>
        /// The URL for the popup button image. If set, buttonText becomes the alt value and is not directly displayed.
        /// </summary>
        public string ButtonImage
        {
            get { return buttonImage; }
            set { buttonImage = value; }
        }

        private bool buttonImageOnly = false;

        /// <summary>
        /// Set to true to place an image after the field to use as the trigger without it appearing on a button.
        /// </summary>
        public bool ButtonImageOnly
        {
            get { return buttonImageOnly; }
            set { buttonImageOnly = value; }
        }

        //private string buttonText = "...";
        ///// <summary>
        ///// The text to display on the trigger button. Use in conjunction with showOn equal to 'button' or 'both'.
        ///// </summary>
        //public string ButtonText
        //{
        //    get { return buttonText; }
        //    set { buttonText = value; }
        //}

        //private string calculateWeek = "$.datepicker.iso8601Week";
        private string calculateWeek = string.Empty;
        /// <summary>
        /// A function to calculate the week of the year for a given date. 
        /// The default implementation uses the ISO 8601 definition: weeks start on a Monday; 
        /// the first week of the year contains the first Thursday of the year.
        /// </summary>
        public string CalculateWeek
        {
            get { return calculateWeek; }
            set { calculateWeek = value; }
        }


        // *** enabled by ghalib ghniem Aug-14-2011 ChangeMonth: bool ,ChangeYear: bool, YearRange: string c-10:c+10
        private bool changeMonth = false;

        /// <summary>
        /// Allows you to change the month by selecting from a drop-down list. You can enable this feature by setting the attribute to true.
        /// </summary>
        public bool ChangeMonth
        {
            get { return changeMonth; }
            set { changeMonth = value; }
        }

        private bool changeYear = false;

        /// <summary>
        /// Allows you to change the year by selecting from a drop-down list. 
        /// You can enable this feature by setting the attribute to true. 
        /// Use the yearRange option to control which years are made available for selection.
        /// </summary>
        public bool ChangeYear
        {
            get { return changeYear; }
            set { changeYear = value; }
        }

        private string yearRange = "c-10:c+10";
        /// <summary>
        /// Control the range of years displayed in the year drop-down: either relative to today's year (-nn:+nn), relative to the currently selected year (c-nn:c+nn), 
        /// absolute (nnnn:nnnn), or combinations of these formats (nnnn:-nn). 
        /// Note that this option only affects what appears in the drop-down, 
        /// to restrict which dates may be selected use the minDate and/or maxDate options.
        /// </summary>
        public string YearRange
        {
            get { return yearRange; }
            set { yearRange = value; }
        }

        //private string closeText = "Done";
        ///// <summary>
        ///// The text to display for the close link. This attribute is one of the regionalisation attributes. 
        ///// Use the showButtonPanel to display this button.
        ///// </summary>
        //public string CloseText
        //{
        //    get { return closeText; }
        //    set { closeText = value; }
        //}

        private bool constrainInput = false;

        /// <summary>
        /// When true entry in the input field is constrained to those characters allowed by the current dateFormat.
        /// </summary>
        public bool ConstrainInput
        {
            get { return constrainInput; }
            set { constrainInput = value; }
        }

        //private string currentText = "Today";
        ///// <summary>
        ///// The text to display for the current day link. This attribute is one of the regionalisation attributes. 
        ///// Use the showButtonPanel to display this button.
        ///// </summary>
        //public string CurrentText
        //{
        //    get { return currentText; }
        //    set { currentText = value; }
        //}

        //private string dateFormat = "mm/dd/yy";
        ///// <summary>
        ///// The format for parsed and displayed dates. This attribute is one of the regionalisation attributes. 
        ///// For a full list of the possible formats see the formatDate function.
        ///// http://docs.jquery.com/UI/Datepicker/formatDate
        ///// </summary>
        //public string DateFormat
        //{
        //    get { return dateFormat; }
        //    set { dateFormat = value; }
        //}

        //private string dayNames = "['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday']";
        ///// <summary>
        ///// The list of long day names, starting from Sunday, for use as requested via the dateFormat setting. 
        ///// They also appear as popup hints when hovering over the corresponding column headings. 
        ///// This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public string DayNames
        //{
        //    get { return dayNames; }
        //    set { dayNames = value; }
        //}

        //private string dayNamesMin = "['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa']";
        ///// <summary>
        ///// The list of minimised day names, starting from Sunday, for use as column headers within the datepicker. 
        ///// This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public string DayNamesMin
        //{
        //    get { return dayNamesMin; }
        //    set { dayNamesMin = value; }
        //}

        //private string dayNamesShort = "['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']";
        ///// <summary>
        ///// The list of abbreviated day names, starting from Sunday, for use as requested via the dateFormat setting. 
        ///// This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public string DayNamesShort
        //{
        //    get { return dayNamesShort; }
        //    set { dayNamesShort = value; }
        //}

        //private string defaultDate = string.Empty;
        ///// <summary>
        ///// Set the date to highlight on first opening if the field is blank. Specify either an actual date via a Date object or as a string in the current dateFormat, 
        ///// or a number of days from today (e.g. +7) or a string of values and periods ('y' for years, 'm' for months, 'w' for weeks, 'd' for days, e.g. '+1m +7d'), 
        ///// or null for today.
        ///// </summary>
        //public string DefaultDate
        //{
        //    get { return defaultDate; }
        //    set { defaultDate = value; }
        //}

        //private string duration = "normal";
        ///// <summary>
        ///// Control the speed at which the datepicker appears, it may be a time in milliseconds 
        ///// or a string representing one of the three predefined speeds ("slow", "normal", "fast").
        ///// </summary>
        //public string Duration
        //{
        //    get { return duration; }
        //    set { duration = value; }
        //}

        //private int firstDay = 0;
        private int firstDay = -1; //-1 mean use default don't set it by script
        /// <summary>
        /// Set the first day of the week: Sunday is 0, Monday is 1, ... 
        /// This attribute is one of the regionalisation attributes.
        /// </summary>
        public int FirstDay
        {
            get { return firstDay; }
            set { firstDay = value; }
        }

        //private bool gotoCurrent = false;
        ///// <summary>
        ///// When true the current day link moves to the currently selected date instead of today.
        ///// </summary>
        //public bool GotoCurrent
        //{
        //    get { return gotoCurrent; }
        //    set { gotoCurrent = value; }
        //}

        //private bool hideIfNoPrevNext = false;
        ///// <summary>
        ///// Normally the previous and next links are disabled when not applicable (see minDate/maxDate). 
        ///// You can hide them altogether by setting this attribute to true.
        ///// </summary>
        //public bool HideIfNoPrevNext
        //{
        //    get { return hideIfNoPrevNext; }
        //    set { hideIfNoPrevNext = value; }
        //}

        //private bool isRTL = false;
        ///// <summary>
        ///// Normally the previous and next links are disabled when not applicable (see minDate/maxDate). 
        ///// You can hide them altogether by setting this attribute to true.
        ///// </summary>
        //public bool IsRTL
        //{
        //    get { return isRTL; }
        //    set { isRTL = value; }
        //}

        //private string maxDate = string.Empty;
        ///// <summary>
        ///// Set a maximum selectable date via a Date object or as a string in the current dateFormat, 
        ///// or a number of days from today (e.g. +7) 
        ///// or a string of values and periods ('y' for years, 'm' for months, 'w' for weeks, 'd' for days, e.g. '+1m +1w'), 
        ///// or null for no limit.
        ///// </summary>
        //public string MaxDate
        //{
        //    get { return maxDate; }
        //    set { maxDate = value; }
        //}

        //private string minDate = string.Empty;
        ///// <summary>
        ///// Set a minimum selectable date via a Date object or as a string in the current dateFormat, 
        ///// or a number of days from today (e.g. +7) 
        ///// or a string of values and periods ('y' for years, 'm' for months, 'w' for weeks, 'd' for days, e.g. '-1y -1m'), 
        ///// or null for no limit.
        ///// </summary>
        //public string MinDate
        //{
        //    get { return minDate; }
        //    set { minDate = value; }
        //}

        //private string monthNames = "['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']";
        ///// <summary>
        ///// The list of full month names, for use as requested via the dateFormat setting. 
        ///// This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public string MonthNames
        //{
        //    get { return monthNames; }
        //    set { monthNames = value; }
        //}

        //private string monthNamesShort = "['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']";
        ///// <summary>
        ///// The list of short month names, for use as requested via the dateFormat setting. 
        ///// This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public string MonthNamesShort
        //{
        //    get { return monthNamesShort; }
        //    set { monthNamesShort = value; }
        //}

        //private bool navigationAsDateFormat = false;
        ///// <summary>
        ///// When true the formatDate function is applied to the prevText, nextText, and currentText values before display, allowing them to display the target month names for example.
        ///// </summary>
        //public bool NavigationAsDateFormat
        //{
        //    get { return navigationAsDateFormat; }
        //    set { navigationAsDateFormat = value; }
        //}

        //private string nextText = "Next";
        ///// <summary>
        ///// The text to display for the next month link. This attribute is one of the regionalisation attributes. With the standard ThemeRoller styling, this value is replaced by an icon.
        ///// </summary>
        //public string NextText
        //{
        //    get { return nextText; }
        //    set { nextText = value; }
        //}

        //private string prevText = "Prev";
        ///// <summary>
        ///// TThe text to display for the previous month link. This attribute is one of the regionalisation attributes. With the standard ThemeRoller styling, this value is replaced by an icon.
        ///// </summary>
        //public string PrevText
        //{
        //    get { return prevText; }
        //    set { prevText = value; }
        //}

        //private int numberOfMonths = 1;
        ///// <summary>
        ///// Set how many months to show at once. The value can be a straight integer, or can be a two-element array to define the number of rows and columns to display.
        ///// </summary>
        //public int NumberOfMonths
        //{
        //    get { return numberOfMonths; }
        //    set { numberOfMonths = value; }
        //}

        //private bool selectOtherMonths = false;
        ///// <summary>
        ///// When true days in other months shown before or after the current month are selectable. This only applies if showOtherMonths is also true.
        ///// </summary>
        //public bool SelectOtherMonths
        //{
        //    get { return selectOtherMonths; }
        //    set { selectOtherMonths = value; }
        //}

        //private string shortYearCutoff = "+10";
        ///// <summary>
        ///// Set the cutoff year for determining the century for a date (used in conjunction with dateFormat 'y'). 
        ///// If a numeric value (0-99) is provided then this value is used directly. 
        ///// If a string value is provided then it is converted to a number and added to the current year. Once the cutoff year is calculated, 
        ///// any dates entered with a year value less than or equal to it are considered to be in the current century, 
        ///// while those greater than it are deemed to be in the previous century.
        ///// </summary>
        //public string ShortYearCutoff
        //{
        //    get { return shortYearCutoff; }
        //    set { shortYearCutoff = value; }
        //}

        //private string showAnim = "show";
        ///// <summary>
        ///// Set the name of the animation used to show/hide the datepicker. Use 'show' (the default), 'slideDown', 'fadeIn', any of the show/hide jQuery UI effects, or '' for no animation.
        ///// </summary>
        //public string ShowAnim
        //{
        //    get { return showAnim; }
        //    set { showAnim = value; }
        //}


        //private bool showButtonPanel = false;
        ///// <summary>
        ///// Whether to show the button panel.
        ///// </summary>
        //public bool ShowButtonPanel
        //{
        //    get { return showButtonPanel; }
        //    set { showButtonPanel = value; }
        //}

        //private int showCurrentAtPos = 0;
        ///// <summary>
        ///// Specify where in a multi-month display the current month shows, starting from 0 at the top/left.
        ///// </summary>
        //public int ShowCurrentAtPos
        //{
        //    get { return showCurrentAtPos; }
        //    set { showCurrentAtPos = value; }
        //}

        //private bool showMonthAfterYear = false;
        ///// <summary>
        ///// Whether to show the month after the year in the header. This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public bool ShowMonthAfterYear
        //{
        //    get { return showMonthAfterYear; }
        //    set { showMonthAfterYear = value; }
        //}

        private string showOn = "button";
        /// <summary>
        /// Have the datepicker appear automatically when the field receives focus ('focus'), appear only when a button is clicked ('button'), or appear when either event takes place ('both').
        /// </summary>
        public string ShowOn
        {
            get { return showOn; }
            set { showOn = value; }
        }

        //private string showOptions = "{}";
        ///// <summary>
        ///// If using one of the jQuery UI effects for showAnim, you can provide additional settings for that animation via this option.
        ///// </summary>
        //public string ShowOptions
        //{
        //    get { return showOptions; }
        //    set { showOptions = value; }
        //}

        //private bool showOtherMonths = false;
        ///// <summary>
        ///// Display dates in other months (non-selectable) at the start or end of the current month. To make these days selectable use selectOtherMonths.
        ///// </summary>
        //public bool ShowOtherMonths
        //{
        //    get { return showOtherMonths; }
        //    set { showOtherMonths = value; }
        //}

        private bool showWeek = false;
        /// <summary>
        /// When true a column is added to show the week of the year. The calculateWeek option determines how the week of the year is calculated. You may also want to change the firstDay option.
        /// </summary>
        public bool ShowWeek
        {
            get { return showWeek; }
            set { showWeek = value; }
        }

        //private int stepMonths = 1;
        ///// <summary>
        ///// Set how many months to move when clicking the Previous/Next links.
        ///// </summary>
        //public int StepMonths
        //{
        //    get { return stepMonths; }
        //    set { stepMonths = value; }
        //}

        //private string weekHeader = "Wk";
        ///// <summary>
        ///// The text to display for the week of the year column heading. This attribute is one of the regionalisation attributes. Use showWeek to display this column.
        ///// </summary>
        //public string WeekHeader
        //{
        //    get { return weekHeader; }
        //    set { weekHeader = value; }
        //}

        //private string yearSuffix = string.Empty;
        ///// <summary>
        ///// Additional text to display after the year in the month headers. This attribute is one of the regionalisation attributes.
        ///// </summary>
        //public string YearSuffix
        //{
        //    get { return yearSuffix; }
        //    set { yearSuffix = value; }
        //}
        

    }
}