using mojoPortal.Web.Controls.DatePicker;
using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// the goal of this control is to be a .net wrapper around the javascript for this
    /// jquery datepicker plugin http://keith-wood.name/calendarsPicker.html
    /// benefits vs the standard jqueryui datepicker are that it supports many world calendars
    /// whereas the jqueryui widget supports only Gregorian calendars.
    /// the challenge will be to integrate the time picker 
    /// http://keith-wood.name/datetimeEntry.html
    /// 
    /// I could not get the time picker to work with Persian Calendar
    /// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=10974~1#post46054
    /// 
    /// add this to the style.config of your skin to get the css for datepicker
    /// <file cssvpath="/ClientScript/jquery.calendars.package-1.1.4/jquery.calendars.picker.css" imagebasevpath="/ClientScript/jquery.calendars.package-1.1.4/">none</file>
    /// add this for time picker
    /// <file cssvpath="/ClientScript/jquery.datetimeentry.package-1.0.1/jquery.datetimeentry.css" imagebasevpath="/ClientScript/jquery.datetimeentry.package-1.0.1/">none</file>
    ///
    /// </summary>
    public class CalendarsDatePicker : TextBox
    {
		//TO DO: implement

		private string scriptFormatRef { get; set; } = "\n<script src=\"{0}\" data-loader=\"calendarsdatepicker\"></script>";
		private string scriptFormatInline { get; set; } = "\n<script data-loader=\"calendarsdatepicker\">\n{0}\n</script>";

        public string ScriptBasePath { get; set; } = "~/ClientScript/jquery.calendars.package-1.1.4/";

        public string ScriptFileName { get; set; } = "jquery.calendars.all.min.js";

        public string DateTimeScriptBasePath { get; set; } = "~/ClientScript/jquery.datetimeentry.package-1.0.1/";

        public string DateTimeScriptFileName { get; set; } = "jquery.datetimeentry.min.js";

        public bool ShowTime { get; set; } = false;

        public bool AutoLocalize { get; set; } = true;

        public string LangCode { get; set; } = "en";

        public string View { get; set; }
		public string MinView { get; set; }
		public string RelatedPickerControl { get; set; }
		public RelatedPickerRelation RelatedPickerRelation { get; set; }

		public bool ShowTimeOnly { get; set; } = false;
		public string MinDate { get; set; }
		public string MaxDate { get; set; }
		public string OnSelectJS { get; set; }
		public string ExtraSettingsJS { get; set; }


		protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (WebConfigSettings.DisablejQueryUI) { return; }

            //ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
            //    "jqcalendars", "\n<script src=\""
            //    + Page.ResolveUrl(scriptBasePath + scriptFileName) + "\" ></script>", 
            //    false);

            

            if (CssClass.Length == 0) { CssClass = "normaltextbox forminput datepicker"; }


            //if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            if (AutoLocalize)
            {
                LangCode = GetSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            }

            
            switch (LangCode)
            {
                case "fa":
                    ShowTime = false; //timepicker doesn't handle Persian date formats
                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendars", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.js")),
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarsplus", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.plus.js")),
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqdpersian", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.persian.js")),
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarspicker", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.picker.js")),
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarspickerext", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.picker.ext.js")),
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarspickerfa", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.picker-fa.js")),
                        false);

                    break;

                default:

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendars", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + ScriptFileName)),
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqdpi18n", string.Format(scriptFormatRef, Page.ResolveUrl(ScriptBasePath + "jquery.calendars.picker-" + LangCode + ".js")),
                        false);

                    break;

            }

            if (ShowTime)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                "jqdatetime", string.Format(scriptFormatRef, Page.ResolveUrl(DateTimeScriptBasePath + DateTimeScriptFileName)),
                false);
            }

            SetupScript();           
        }

        private void SetupScript()
        {
            //if (showTime)
            //{
            //    SetupTimePickerScript();
            //}

            StringBuilder script = new StringBuilder();
            //script.Append("<script type=\"text/javascript\"> ");
            script.Append("$(function() { ");

            switch (LangCode)
            {
                case "fa":
                    //script.Append("$('#" + ClientID + "').calendarsPicker('persian', 'fa'); ");

                    script.Append("$('#" + ClientID + "').calendarsPicker({calendar: $.calendars.instance('persian')}); ");

                    //$('#l10nPicker').calendars.picker($.extend( 
                    //    {calendar: $.calendars.instance('gregorian', 'fr')}, 
                     //   $.calendars.picker.regional['fr']));

                    break;

                case "en":
                default:

                    script.Append("$('#" + ClientID + "').calendarsPicker(); ");
                    break;
            }


            script.Append(" ");

            if (ShowTime)
            {
                string timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
                    + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

                script.Append("$('#" + ClientID + "').datetimeEntry({ ");

                script.Append("datetimeFormat: '" + timeFormat + "'");
                script.Append(",spinnerImage: '" + Page.ResolveUrl(DateTimeScriptBasePath + "spinnerDefault.png") + "' ");

                script.Append("}); ");
            }

            script.Append(" });");
            ScriptManager.RegisterStartupScript(this, GetType(), "setupdp" + this.ClientID, string.Format(scriptFormatInline, script.ToString()), false);

        }

        public static string GetSupportedLangCode(string cultureName, string cultureCode)
        {
            switch (cultureName)
            {
                case "ar-DZ":
                case "de-CH":
                case "en-AU":
                case "en-GB":
                case "en-NZ":
                case "es-AR":
                case "es-PE":
                case "fr-CH":
                case "me-ME":
                case "nl-BE":
                case "pt-BR":
                case "sr-SR":
                case "zh-CN":
                case "zh-HK":
                case "zh-TW":
                    return cultureName;
            }
            return cultureCode;
        }
    }
}