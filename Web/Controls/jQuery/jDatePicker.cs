using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.DatePicker;
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

            if(ShowTime)
            {
                CssClass += $" {TimeCssClass}"; 
            }

            if (AutoLocalize)
            {
                LangCode = GetSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
                if (CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator.Length == 0)
                {
                    ClockHours = "24";
                }
                else
                {
                    AmDesignator = CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator;
                    PmDesignator = CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator;
                }
                if (LangCode.StartsWith("zh")) { ClockHours = "24"; }
            }

            if (LangCode != "en")
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                "jqdatei18n", "\n<script src=\""
                + GetJQueryUIBasePath(Page) + "i18n/jquery.ui.datepicker-" + LangCode + ".js" + "\" type=\"text/javascript\" ></script>");

            }

            if (UseOldTimePicker)
            {
                if (ShowTime) { SetupTimeScript(); }

                SetupOldScript();
            }
            else
            {
                SetupScript();
            }

        }

        private void SetupScript()
        {
            if (ShowTime)
            {
                SetupTimePickerScript();
            }

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");
            script.Append("$(document).ready(function() { ");

            if (ShowTimeOnly)
            {
                script.Append("$('#" + ClientID + "').timepicker(");
            }
            else if (ShowTime)
            {
                script.Append("$('#" + ClientID + "').datetimepicker(");
            }
            else
            {
                script.Append("$('#" + ClientID + "').datepicker(");
            }


            script.Append("{");

            script.Append("showOn: '" + ShowOn + "'");
            if (ButtonImage.Length > 0)
            {
                //script.Append(",buttonImage: '" + Page.ResolveUrl("/Data/SiteImages/calendar.png") + "' ");
                script.Append(",buttonImage: '" + Page.ResolveUrl(ButtonImage) + "' ");
            }

            if (ButtonImageOnly) { script.Append(",buttonImageOnly: true"); }
            script.Append(",buttonText:'...'");

            script.Append(",duration: ''");

            if (ShowTime)
            {
                // in .NET M means month 1 -12 with no leading zero
                // in javascript it means month name like Dec
                // we need m for month with no leading zero, so we need to lower it
                // also in C# yyyy means four digit year but in js yy means 4 digit year so
                // we must replace yyyy with yy
                string dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("M", "m").Replace("yyyy","yy");
                //http://trentrichardson.com/examples/timepicker/
                //in .NET tt means use AM or PM but in this js it means am or pm
                // wee need TT to get AM or PM
                string timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Replace("tt","TT");
                
                script.Append(",dateFormat:'" + dateFormat + "'");
                script.Append(",timeFormat:'" + timeFormat + "'");

                if (ClockHours != "24")
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

            if (ChangeMonth) { script.Append(",changeMonth:true"); }
            if (ChangeYear) { script.Append(",changeYear:true,yearRange:'" + YearRange + "'"); }
            
            if (ShowWeek) { script.Append(",showWeek:true"); }
            if (FirstDay > -1) { script.Append(",firstDay:" + FirstDay.ToInvariantString()); }
            if (CalculateWeek.Length > 0) { script.Append(",calculateWeek:" + CalculateWeek); }
			
            if (!string.IsNullOrWhiteSpace(ExtraSettingsJS))
			{
				script.Append($",{ExtraSettingsJS}");
			}

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

            if (ShowTime)
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

            script.Append("showOn: '" + ShowOn + "'");
            if (ButtonImage.Length > 0)
            {
                //script.Append(",buttonImage: '" + Page.ResolveUrl("/Data/SiteImages/calendar.png") + "' ");
                script.Append(",buttonImage: '" + Page.ResolveUrl(ButtonImage) + "' ");
            }

            if (ButtonImageOnly) { script.Append(",buttonImageOnly: true"); }
            script.Append(",buttonText:'...'");

            script.Append(",duration: ''");

            if (ShowTime)
            {
                script.Append(",showTime: true");
                if (!ConstrainInput) { script.Append(",constrainInput: false"); }
                if (ClockHours == "24")
                {
                    script.Append(",'time24h': true ");
                   

                }

            }

            if (ChangeMonth) { script.Append(",changeMonth:true"); }
            if (ChangeYear) { script.Append(",changeYear:true,yearRange:'" + YearRange + "'"); }

            
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

            if (ClockHours == "24")
            {
                is24HourClock = "true";
            }

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jtimepickerresources", "\n<script type=\"text/javascript\"> var jqtDoneLabel = '" + DoneLabel + "'; var jqtHourLabel = '" + HourLabel + "'; var jqtMinuteLabel = '" + MinuteLabel + "'; var jqt24Hour = " + is24HourClock + "; var jqtAM = '" + AmDesignator + "'; var jqtPM = '" + PmDesignator + "'; </script>");

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

		//public bool UseBrowserPicker { get; set; } = true;

		/// <summary>
		/// Disables (true) or enables (false) the timepicker. Can be set when initialising (first creating) the datepicker.
		/// </summary>
		public bool ShowTime { get; set; } = false;
		/// <summary>
		/// if true the control will be rendered only as a time picker with not datepicker
		/// </summary>
		public bool ShowTimeOnly { get; set; } = false;
		/// <summary>
		/// 12 or 24
		/// </summary>
		public string ClockHours { get; set; } = "12";

		public bool AutoLocalize { get; set; } = true;

		/// <summary>
		/// the old time picker only works with jQueryUI 1.8.6, if a newer version is used it does not work
		/// therefore we now use the newer one that does work. If you want to use the old one you could add this in theme.skin:
		/// <portal:jDatePicker runat="server"  UseOldTimePicker="true" />
		/// </summary>
		public bool UseOldTimePicker { get; set; } = false;

		public string LangCode { get; set; } = "en";

		/// <summary>
		/// this allows localizing the Done button in the time picker
		/// </summary>
		public string DoneLabel { get; set; } = "Done";

		/// <summary>
		/// this allows localizing the Hour label in the time picker
		/// </summary>
		public string HourLabel { get; set; } = "Hour";

		/// <summary>
		/// this allows localizing the Minute label in the time picker
		/// </summary>
		public string MinuteLabel { get; set; } = "Minute";

		/// <summary>
		/// this allows localizing the AM label in the time picker
		/// </summary>
		public string AmDesignator { get; set; } = "AM";

		/// <summary>
		/// this allows localizing the PM label in the time picker
		/// </summary>
		public string PmDesignator { get; set; } = "PM";
		/// <summary>
		/// The URL for the popup button image. If set, buttonText becomes the alt value and is not directly displayed.
		/// </summary>
		public string ButtonImage { get; set; } = string.Empty;

		/// <summary>
		/// Set to true to place an image after the field to use as the trigger without it appearing on a button.
		/// </summary>
		public bool ButtonImageOnly { get; set; } = false;
		/// <summary>
		/// A function to calculate the week of the year for a given date. 
		/// The default implementation uses the ISO 8601 definition: weeks start on a Monday; 
		/// the first week of the year contains the first Thursday of the year.
		/// </summary>
		public string CalculateWeek { get; set; } = string.Empty;

		// *** enabled by ghalib ghniem Aug-14-2011 ChangeMonth: bool ,ChangeYear: bool, YearRange: string c-10:c+10
		/// <summary>
		/// Allows you to change the month by selecting from a drop-down list. You can enable this feature by setting the attribute to true.
		/// </summary>
		public bool ChangeMonth { get; set; } = false;

		/// <summary>
		/// Allows you to change the year by selecting from a drop-down list. 
		/// You can enable this feature by setting the attribute to true. 
		/// Use the yearRange option to control which years are made available for selection.
		/// </summary>
		public bool ChangeYear { get; set; } = false;
		/// <summary>
		/// Control the range of years displayed in the year drop-down: either relative to today's year (-nn:+nn), relative to the currently selected year (c-nn:c+nn), 
		/// absolute (nnnn:nnnn), or combinations of these formats (nnnn:-nn). 
		/// Note that this option only affects what appears in the drop-down, 
		/// to restrict which dates may be selected use the minDate and/or maxDate options.
		/// </summary>
		public string YearRange { get; set; } = "c-10:c+10";

		/// <summary>
		/// When true entry in the input field is constrained to those characters allowed by the current dateFormat.
		/// </summary>
		public bool ConstrainInput { get; set; } = false;
		/// <summary>
		/// Set the first day of the week: Sunday is 0, Monday is 1, ... 
		/// This attribute is one of the regionalisation attributes.
		/// -1 use default don't set it by script
		/// </summary>
		public int FirstDay { get; set; } = -1;
		/// <summary>
		/// Have the datepicker appear automatically when the field receives focus ('focus'), appear only when a button is clicked ('button'), or appear when either event takes place ('both').
		/// </summary>
		public string ShowOn { get; set; } = "button";
		/// <summary>
		/// When true a column is added to show the week of the year. The calculateWeek option determines how the week of the year is calculated. You may also want to change the firstDay option.
		/// </summary>
		public bool ShowWeek { get; set; } = false;

		public string TimeCssClass { get; set; } = "timepicker";

		public string View { get; set; }
		public string MinView { get; set; }
		public string RelatedPickerControl { get; set; }
		public RelatedPickerRelation RelatedPickerRelation { get; set; }

		public string MinDate { get; set; }
		public string MaxDate { get; set; }
        public string OnSelectJS { get; set; }
		public string ExtraSettingsJS { get; set; }

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