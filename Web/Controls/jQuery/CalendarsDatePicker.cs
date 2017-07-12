// Author:					
// Created:				    2012-10-30
// Last Modified:			2012-11-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//


using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

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

        private string scriptBasePath = "~/ClientScript/jquery.calendars.package-1.1.4/";

        public string ScriptBasePath
        {
            get { return scriptBasePath; }
            set { scriptBasePath = value; }
        }

        private string scriptFileName = "jquery.calendars.all.min.js";

        public string ScriptFileName
        {
            get { return scriptFileName; }
            set { scriptFileName = value; }
        }

        private string dateTimeScriptBasePath = "~/ClientScript/jquery.datetimeentry.package-1.0.1/";

        public string DateTimeScriptBasePath
        {
            get { return dateTimeScriptBasePath; }
            set { dateTimeScriptBasePath = value; }
        }

        private string dateTimeScriptFileName = "jquery.datetimeentry.min.js";

        public string DateTimeScriptFileName
        {
            get { return dateTimeScriptFileName; }
            set { dateTimeScriptFileName = value; }
        }

        private bool showTime = false;

        public bool ShowTime
        {
            get { return showTime; }
            set { showTime = value; }
        }

        private bool autoLocalize = true;

        public bool AutoLocalize
        {
            get { return autoLocalize; }
            set { autoLocalize = value; }
        }

        private string langCode = "en";

        public string LangCode
        {
            get { return langCode; }
            set { langCode = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (WebConfigSettings.DisablejQueryUI) { return; }

            //ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
            //    "jqcalendars", "\n<script src=\""
            //    + Page.ResolveUrl(scriptBasePath + scriptFileName) + "\" type=\"text/javascript\" ></script>", 
            //    false);

            

            if (CssClass.Length == 0) { CssClass = "normaltextbox forminput datepicker"; }


            //if (SiteUtils.IsSecureRequest()) { protocol = "https"; }

            if (autoLocalize)
            {
                langCode = GetSupportedLangCode(CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            }

            
            switch (langCode)
            {
                case "fa":
                    showTime = false; //timepicker doesn't handle Persian date formats
                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendars", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.js") + "\" type=\"text/javascript\" ></script>",
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarsplus", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.plus.js") + "\" type=\"text/javascript\" ></script>",
                        false);

                    

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqdpersian", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.persian.js") 
                        + "\" type=\"text/javascript\" ></script>",
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarspicker", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.picker.js") + "\" type=\"text/javascript\" ></script>",
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarspickerext", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.picker.ext.js") + "\" type=\"text/javascript\" ></script>",
                        false);

                    //jquery.calendars.picker.ext.js

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendarspickerfa", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.picker-fa.js") + "\" type=\"text/javascript\" ></script>",
                        false);

                    break;

                default:

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqcalendars", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + scriptFileName) + "\" type=\"text/javascript\" ></script>",
                        false);

                    ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                        "jqdpi18n", "\n<script src=\""
                        + Page.ResolveUrl(scriptBasePath + "jquery.calendars.picker-" + langCode + ".js")
                        + "\" type=\"text/javascript\" ></script>",
                        false);

                    break;

            }

            

            if (showTime)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(CalendarsDatePicker),
                "jqdatetime", "\n<script src=\""
                + Page.ResolveUrl(dateTimeScriptBasePath + dateTimeScriptFileName) + "\" type=\"text/javascript\" ></script>",
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
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n");
            script.Append("$(function() { ");

            switch (langCode)
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

            if (showTime)
            {
                string timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
                    + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

                script.Append("$('#" + ClientID + "').datetimeEntry({ ");

                script.Append("datetimeFormat: '" + timeFormat + "'");
                script.Append(",spinnerImage: '" + Page.ResolveUrl(dateTimeScriptBasePath + "spinnerDefault.png") + "' ");

                script.Append("}); ");
            }


            script.Append(" });");

            script.Append("\n ");
            script.Append(" </script>");

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "setupdp" + this.ClientID,
                script.ToString(),
                false);

        }

        private void SetupTimePickerScript()
        {
            
            //ScriptManager.RegisterClientScriptBlock(
            //    this,
            //    typeof(Page),
            //            "jqtimecore", "\n<script src=\""
            //            + Page.ResolveUrl(WebConfigSettings.TimePickerScriptUrl) + "\" type=\"text/javascript\"></script>", false);

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

            //switch (cultureCode)
            //{
            //    case "af":
            //    case "ar":
            //    case "az":
            //    case "bg":
            //    case "bs":
            //    case "ca":
            //    case "cs":
            //    case "da":
            //    case "de":
            //    case "el":
            //    case "eo":
            //    case "es":
            //    case "et":
            //    case "eu":
            //    case "fa":
            //    case "fi":
            //    case "fo":
            //    case "fr":
            //    case "he":
            //    case "hr":
            //    case "hu":
            //    case "hy":
            //    case "id":
            //    case "is":
            //    case "it":
            //    case "ja":
            //    case "ko":
            //    case "lt":
            //    case "lv":
            //    case "ms":
            //    case "nl":
            //    case "no":
            //    case "pl":
            //    case "pt":
            //    case "ro":
            //    case "ru":
            //    case "sk":
            //    case "sl":
            //    case "sq":
            //    case "sr":
            //    case "sv":
            //    case "ta":
            //    case "tr":
            //    case "uk":
            //    case "vi":

            //        return cultureCode;


            //    default:
            //        return "en";

            //}


        }


    }
}