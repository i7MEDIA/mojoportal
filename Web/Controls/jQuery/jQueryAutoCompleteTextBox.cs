//  Author:                     
//  Created:                    2013-10-23
//	Last Modified:              2013-10-25
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using mojoPortal.Web.Framework;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a wrapper control for the jqueryui autocomplete
    /// http://api.jqueryui.com/autocomplete/
    /// 
    /// example usage can be found in Admin/ModuleSettings.aspx which uses the web service url
    /// SiteRoot + "/Services/UserLookup.asmx/AutoComplete"
    /// examine the code for that service to learn how to implement your own simialr service for use with this control
    /// </summary>
    public class jQueryAutoCompleteTextBox : TextBox
    {
        private const string defaultPosition = "{ my: \"left top\", at: \"left bottom\", collision: \"none\" }";

        private string dataUrl = string.Empty;
        /// <summary>
        /// url that returns json in the label/value format used by jqueryui autocomplete plugin
        /// [{"label":"Client example","value":1},{"label":"Lorem Ipsum","value":2},{"label":"Microsoft","value":3}]
        /// 
        /// the label is shown and the value can be stored in another control such as a textbox or hidden field given the client id of the target control
        /// this property is required if not set then we don't wire up the javascript
        /// </summary>
        [Themeable(false)]
        public string DataUrl
        {
            get { return dataUrl; }
            set { dataUrl = value; }
        }

        private string targetValueElementClientId = string.Empty;
        /// <summary>
        /// set to the .ClientID of an <asp:HiddenField to store the selected value or <asp:TextBox
        /// </summary>
        public string TargetValueElementClientId
        {
            get { return targetValueElementClientId; }
            set { targetValueElementClientId = value; }
        }

        private bool blockTargetFocus = false;
        /// <summary>
        /// if true then the focus event of the target element will trigger blur to prevent focus
        /// </summary>
        public bool BlockTargetFocus
        {
            get { return blockTargetFocus; }
            set { blockTargetFocus = value; }
        }

        private bool clearTargetOnEmptyInput = true;
        /// <summary>
        /// true by default, if you are storing the value from the label/value in another textbox or hidden field
        /// then typically clearing the autocomplete input should also clear the target value
        /// </summary>
        public bool ClearTargetOnEmptyInput
        {
            get { return clearTargetOnEmptyInput; }
            set { clearTargetOnEmptyInput = value; }
        }

        private string clearValue = string.Empty;
        /// <summary>
        /// when we clear the target the default is to clear it to an empty string but you can assing something here
        /// to use instead, an empty guid string for example
        /// </summary>
        public string ClearValue
        {
            get { return clearValue; }
            set { clearValue = value; }
        }

        private string appendTo = string.Empty;
        /// <summary>
        /// Which element the menu should be appended to. 
        /// When the value is null, the parents of the input field will be checked for a class of ui-front. 
        /// If an element with the ui-front class is found, the menu will be appended to that element. 
        /// Regardless of the value, if no element is found, the menu will be appended to the body.
        /// </summary>
        public string AppendTo
        {
            get { return appendTo; }
            set { appendTo = value; }
        }

        private bool autoFocus = false;
        /// <summary>
        /// If set to true the first item will automatically be focused when the menu is shown.
        /// </summary>
        public bool AutoFocus
        {
            get { return autoFocus; }
            set { autoFocus = value; }
        }

        private int delay = 300;
        /// <summary>
        /// The delay in milliseconds between when a keystroke occurs and when a search is performed. 
        /// A zero-delay makes sense for local data (more responsive), but can produce a lot of load 
        /// for remote data, while being less responsive.
        /// </summary>
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        //private bool disabled = false;
        ///// <summary>
        ///// Disables the autocomplete if set to true.
        ///// </summary>
        //public bool Disabled
        //{
        //    get { return disabled; }
        //    set { disabled = value; }
        //}

        private int minLength = 1;
        /// <summary>
        /// The minimum number of characters a user must type before a search is performed. 
        /// Zero is useful for local data with just a few items, but a higher value should be used 
        /// when a single character search could match a few thousand items.
        /// </summary>
        public int MinLength
        {
            get { return minLength; }
            set { minLength = value; }
        }

        private string position = "{ my: \"left top\", at: \"left bottom\", collision: \"none\" }";
        /// <summary>
        /// Identifies the position of the suggestions menu in relation to the associated input element. 
        /// The of option defaults to the input element, but you can specify another element to position against. 
        /// You can refer to the jQuery UI Position utility for more details about the various options.
        /// </summary>
        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private bool useJQueryUICssClass = true;
        /// <summary>
        /// default is true adds ui-autocomplete-input as a css class on the input
        /// </summary>
        public bool UseJQueryUICssClass
        {
            get { return useJQueryUICssClass; }
            set { useJQueryUICssClass = value; }
        }

        private string ajaxVerb = "POST";

        public string AjaxVerb
        {
            get { return ajaxVerb; }
            set { ajaxVerb = value; }
        }

        private bool alertOnAjaxError = false;

        public bool AlertOnAjaxError
        {
            get { return alertOnAjaxError; }
            set { alertOnAjaxError = value; }
        }

        private string ajaxCallbackGuts = "$.map(data.d, function (item) { return { label: item.Label, value: item.Value } })";
        /// <summary>
        /// in case you need to override $.map(data.d, function (item) { return { label: item.Label, value: item.Value } })
        /// </summary>
        public string AjaxCallbackGuts
        {
            get { return ajaxCallbackGuts; }
            set { ajaxCallbackGuts = value; }
        }

        private string ajaxDataJsFragment = "data: \"{ 'term': '\" + request.term + \"' }\",";
        /// <summary>
        /// in case you need to modify "data: \"{ 'term': '\" + request.term + \"' }\",";
        /// </summary>
        public string AjaxDataJsFragment
        {
            get { return ajaxDataJsFragment; }
            set { ajaxDataJsFragment = value; }
        }

        private string ajaxDataUrlAddendumJsFragment = string.Empty;

        public string AjaxDataUrlAddendumJsFragment
        {
            get { return ajaxDataUrlAddendumJsFragment; }
            set { ajaxDataUrlAddendumJsFragment = value; }
        }

        private string ajaxContentType = "application/json; charset=utf-8";

        public string AjaxContentType
        {
            get { return ajaxContentType; }
            set { ajaxContentType = value; }
        }

        private string overrideInputClientId = string.Empty;
        /// <summary>
        /// if you want to use the script with an existing input then specify
        /// this property with the id of the html input that exists on the page
        /// and set ScriptOnly to true to prevent rendering this textbox input
        /// </summary>
        public string OverrideInputClientId
        {
            get { return overrideInputClientId; }
            set { overrideInputClientId = value; }
        }

        private bool scriptOnly = false;
        /// <summary>
        /// If true no textbox input is rendered. Only reason to set to true is if you are also providing a OverrideInputClientId
        /// to attach the autocomplete to instead of the normal textbox
        /// </summary>
        public bool ScriptOnly
        {
            get { return scriptOnly; }
            set { scriptOnly = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupInstanceScript();
            if(useJQueryUICssClass)
            {
                if(CssClass.Length > 0)
                {
                    CssClass += " ui-autocomplete-input";
                }
                else
                {
                    CssClass = "ui-autocomplete-input";
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (scriptOnly) { return; }
            
            base.Render(writer);
        }

        private void SetupInstanceScript()
        {
            if (dataUrl.Length == 0) { return; }

            string inputId = ClientID;
            if (overrideInputClientId.Length > 0)
            {
                inputId = overrideInputClientId;
            }

            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">\n");

            script.Append("$(function() {");

            if (blockTargetFocus)
            {

                script.Append("$('#" + targetValueElementClientId + "').focus(function() { this.blur(); });");

            }

            if ((clearTargetOnEmptyInput)&&(targetValueElementClientId.Length > 0))
            {
                script.Append("$('#" + inputId + "').blur(function() {");
                //script.Append(" alert($(this).val()); ");
                script.Append("if ($(this).val() == '') {");
                script.Append("$('#" + targetValueElementClientId + "').val('" + clearValue + "'); ");
                script.Append("} ");
                
                script.Append("}); "); //end blur
            }

            
           script.Append("$('#" + inputId + "').autocomplete({");
            
            

            // function to call the data url
            script.Append("source: function(request, response) { ");

            //script.Append(" alert(request.term); ");

            script.Append("$.ajax({");
            script.Append("type: \"" + ajaxVerb + "\",");

            // application/json; charset=utf-8
            script.Append("contentType: \"" + ajaxContentType + "\",");

            //script.Append("url:'" + dataUrl + "?f=json&query=' + request.term,");
            script.Append("url:'" + dataUrl + ajaxDataUrlAddendumJsFragment + "',");

            // "data: \"{ 'term': '\" + request.term + \"' }\",";
            script.Append(ajaxDataJsFragment);
            
            
            script.Append("dataType:'json',");
            script.Append("success: function( data ) {");
            // call the callback with the result
            // $.map(data.d, function (item) { return { label: item.Label, value: item.Value } })
            script.Append("response(" + ajaxCallbackGuts + ")");
            
            script.Append(" }"); // end success function

            script.Append(",failure: function(data) {");
            if(alertOnAjaxError)
            {
                script.Append(" alert('oops something went wrong with the data request'); ");
            }
            

            script.Append(" }"); // end failure function

            script.Append("}); "); //end ajax

            script.Append("} "); // end source function

            // select function fies when user selects from the list
            script.Append(",select: function( event, ui ) {");
            // the default would put the value in the input, we want the label in the input
            script.Append("event.preventDefault(); ");
            script.Append("$('#" + ClientID + "').val(ui.item.label); ");

            //put the value in the target input or hidden field
            if(targetValueElementClientId.Length > 0)
            {
                script.Append("$('#" + targetValueElementClientId + "').val(ui.item.value); ");
            }

            script.Append("} "); // end select function

            

            if (appendTo.Length > 0)
            {
                script.Append(",appendTo:'" + appendTo + "'");
            }

            if (autoFocus)
            {
                script.Append(",autoFocus:true");
            }

            if (position != defaultPosition)
            {
                script.Append(",position: " + position);

            }

            if (delay != 300)
            {
                script.Append(",delay:" + delay.ToInvariantString());
            }

            if (minLength != 1)
            {
                script.Append(",minLength:" + minLength.ToInvariantString());
            }


            script.Append("}); ");

            script.Append("});");

            script.Append("\n</script>");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(jQueryAutoCompleteTextBox),
                UniqueID,
                script.ToString(),
                false);
        }

    }
}