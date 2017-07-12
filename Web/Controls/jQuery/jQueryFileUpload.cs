//  Author:                     
//  Created:                    2013-03-30
//	Last Modified:              2014-01-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI
{
    //https://github.com/blueimp/jQuery-File-Upload/issues/638#issuecomment-2215669
    //https://github.com/blueimp/jQuery-File-Upload/wiki/Basic-plugin
    //https://github.com/blueimp/jQuery-File-Upload/wiki/Options


    public class jQueryFileUpload : FileUpload
    {
        private string serviceUrl = string.Empty;

        [Themeable(false)]
        public string ServiceUrl
        {
            get { return serviceUrl; }
            set { serviceUrl = value; }
        }

        private int maxFilesAllowed = 1;

        [Themeable(false)]
        public int MaxFilesAllowed
        {
            get { return maxFilesAllowed; }
            set { maxFilesAllowed = value; }
        }

        private int maxConcurrentUploadsAllowed = 1;

        [Themeable(false)]
        public int MaxConcurrentUploadsAllowed
        {
            get { return maxConcurrentUploadsAllowed; }
            set { maxConcurrentUploadsAllowed = value; }
        }

        private string iframeTransportScriptPath = "~/ClientScript/jqfileupload/jquery.iframe-transport.js";
        [Themeable(false)]
        public string IframeTransportScriptPath
        {
            get { return iframeTransportScriptPath; }
            set { iframeTransportScriptPath = value; }
        }

        private string uploadScriptPath = "~/ClientScript/jqfileupload/jquery.fileupload.js";
        [Themeable(false)]
        public string UploadScriptPath
        {
            get { return uploadScriptPath; }
            set { uploadScriptPath = value; }
        }

        private bool singleFileUploads = false; // js default is true
        [Themeable(false)]
        public bool SingleFileUploads
        {
            get { return singleFileUploads; }
            set { singleFileUploads = value; }
        }

        private bool sequentialUploads = false;
        [Themeable(false)]
        public bool SequentialUploads
        {
            get { return sequentialUploads; }
            set { sequentialUploads = value; }
        }

        private bool replaceFileInput = true;
        [Themeable(false)]
        public bool ReplaceFileInput
        {
            get { return replaceFileInput; }
            set { replaceFileInput = value; }
        }

        private bool forceIframeTransport = false;
        [Themeable(false)]
        public bool ForceIframeTransport
        {
            get { return forceIframeTransport; }
            set { forceIframeTransport = value; }
        }

        private string requestType = "POST";
        [Themeable(false)]
        public string RequestType
        {
            get { return requestType; }
            set
            {
                switch (value)
                {
                    case "POST":
                    case "PUT":
                    case "PATCH":
                        requestType = value;
                        break;
                    default:
                        // invalid
                        break;

                }

            }
        }

        private string formAcceptCharset = string.Empty;
        [Themeable(false)]
        public string FormAcceptCharset
        {
            get { return formAcceptCharset; }
            set { formAcceptCharset = value; }
        }

        private string maxChunkSize = "undefined";
        [Themeable(false)]
        public string MaxChunkSize
        {
            get { return maxChunkSize; }
            set { maxChunkSize = value; }
        }

        private int bitrateInterval = 500;
        [Themeable(false)]
        public int BitrateInterval
        {
            get { return bitrateInterval; }
            set { bitrateInterval = value; }
        }

        private string uploadButtonClientId = string.Empty;
        /// <summary>
        /// if provided the javascript will hide this button since
        /// the ajax ui will replace the normal postback functionality
        /// </summary>
        [Themeable(false)]
        public string UploadButtonClientId
        {
            get { return uploadButtonClientId; }
            set { uploadButtonClientId = value; }
        }

        private string addFilesText = "Select files...";
        /// <summary>
        /// override to localize
        /// </summary>
        [Themeable(false)]
        public string AddFilesText
        {
            get { return addFilesText; }
            set { addFilesText = value; }
        }

        private string addFileText = "Select file...";
        /// <summary>
        /// override to localize, used when only 1 file is allowed
        /// </summary>
        [Themeable(false)]
        public string AddFileText
        {
            get { return addFileText; }
            set { addFileText = value; }
        }

        private string uploadButtonText = "Upload";
        [Themeable(false)]
        public string UploadButtonText
        {
            get { return uploadButtonText; }
            set { uploadButtonText = value; }
        }

        private string uploadingText = "Uploading...";
        [Themeable(false)]
        public string UploadingText
        {
            get { return uploadingText; }
            set { uploadingText = value; }
        }

        private string uploadCompleteText = "Upload finished.";
        [Themeable(false)]
        public string UploadCompleteText
        {
            get { return uploadCompleteText; }
            set { uploadCompleteText = value; }
        }

        private string uploadCompleteCallback = string.Empty;
        /// <summary>
        /// a custom javacript function name to call after upload completes
        /// </summary>
        public string UploadCompleteCallback
        {
            get { return uploadCompleteCallback; }
            set { uploadCompleteCallback = value; }
        }

        private string addFilesButtonCssClass = "ui-button ui-widget ui-state-default ui-corner-all";

        public string AddFilesButtonCssClass
        {
            get { return addFilesButtonCssClass; }
            set { addFilesButtonCssClass = value; }
        }

        private string browseButtonCssClass = string.Empty;

        public string BrowseButtonCssClass
        {
            get { return browseButtonCssClass; }
            set { browseButtonCssClass = value; }
        }

        private string browseButtonExtraTopMarkup = string.Empty;

        public string BrowseButtonExtraTopMarkup
        {
            get { return browseButtonExtraTopMarkup; }
            set { browseButtonExtraTopMarkup = value; }
        }

        private string browseButtonExtraBottomMarkup = string.Empty;

        public string BrowseButtonExtraBottomMarkup
        {
            get { return browseButtonExtraBottomMarkup; }
            set { browseButtonExtraBottomMarkup = value; }
        }

        private string deleteSpanCssClass = "ui-icon ui-icon-trash";

        public string DeleteSpanCssClass
        {
            get { return deleteSpanCssClass; }
            set { deleteSpanCssClass = value; }
        }

        private string errorDeleteSpanCssClass = "ui-icon ui-icon-circle-close";

        public string eErrorDeleteSpanCssClass
        {
            get { return errorDeleteSpanCssClass; }
            set { errorDeleteSpanCssClass = value; }
        }

        private bool useDropZone = true;

        public bool UseDropZone
        {
            get { return useDropZone; }
            set { useDropZone = value; }
        }

        private string dropZoneCssClass = "fileupload-dropzone";

        public string DropZoneCssClass
        {
            get { return dropZoneCssClass; }
            set { dropZoneCssClass = value; }
        }

        private string dropFileText = "Drag and drop a file here";

        public string DropFileText
        {
            get { return dropFileText; }
            set { dropFileText = value; }
        }

        private string dropFilesText = "Drag and drop files here";

        public string DropFilesText
        {
            get { return dropFilesText; }
            set { dropFilesText = value; }
        }

        private string errorOcurredMessage = Resource.UploadErrorMessage;

        public string ErrorOcurredMessage
        {
            get { return errorOcurredMessage; }
            set { errorOcurredMessage = value; }
        }

        private bool alertOnError = false;
        /// <summary>
        /// for debugging purposes
        /// </summary>
        [Themeable(false)]
        public bool AlertOnError
        {
            get { return alertOnError; }
            set { alertOnError = value; }
        }

        private string formFieldClientId = string.Empty;
        /// <summary>
        /// if provided then the value of this field will be passed as frmData instead of all form variables
        /// </summary>
        [Themeable(false)]
        public string FormFieldClientId
        {
            get { return formFieldClientId; }
            set { formFieldClientId = value; }
        }

        private string returnValueFormFieldClientId = string.Empty;
        /// <summary>
        /// if provided then the returnvalue from the service will be populated in this form element
        /// typical use would be a hidden field
        /// </summary>
        [Themeable(false)]
        public string ReturnValueFormFieldClientId
        {
            get { return returnValueFormFieldClientId; }
            set { returnValueFormFieldClientId = value; }
        }

        private string acceptFileTypes = string.Empty;
        [Themeable(false)]
        public string AcceptFileTypes
        {
            get { return acceptFileTypes; }
            set { acceptFileTypes = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                if (useDropZone)
                {
                    writer.Write(" <div id='dropZone" + ClientID + "'></div>");
                }


                writer.Write("<div id='pnlFiles" + ClientID + "' class='uploadcontainer'>");

                writer.Write("<div id='browseButton" + ClientID + "'>");
                //writer.Write("<span class='jqbutton ui-button ui-widget ui-state-default ui-corner-all'>Add files...</span>");
            }

            base.Render(writer);

            if (!string.IsNullOrEmpty(serviceUrl))
            {
                writer.Write("</div>");
                writer.Write("</div>");

                writer.Write(" <div id='progress" + ClientID + "'></div>");
                writer.Write(" <div id='filelist" + ClientID + "' class='uploadfilelist'></div>");
                writer.Write(" <div id='ui" + ClientID + "' class='uploadfilesui'></div>");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Visible) { return; }

            if (WebConfigSettings.ForceLegacyFileUpload)
            {
                serviceUrl = string.Empty;
                useDropZone = false;
                return;
            }

            if (string.IsNullOrEmpty(serviceUrl)) { return; }

            if (maxFilesAllowed > 1) { Attributes.Add("multiple", "multiple"); }

            SetupCommonScript();
            SetupInstanceScript();
        }

        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">\n");

            // begin util functions

            script.Append(" function addFileToList" + ClientID + " (data, fileList, index, file) {");

            //script.Append("\nalert('Added file' + index + ': ' + file.name); ");

            //show files in UI list
            script.Append("var d = $(\"<span class='" + deleteSpanCssClass + "' title='Remove'></span>\")");
            script.Append(".click(function(){ ");
            // see https://github.com/joeaudette/mojoportal/issues/4       
            script.Append("var indexAtRuntime = $('#filelist" + ClientID + " ul:not(.file-errors) li').index($(this).parent());");
            script.Append(" data.files.splice(indexAtRuntime,1); ");
            // sync the copy of file array
            script.Append(" fileList = data.files; ");
            // see https://github.com/joeaudette/mojoportal/issues/4
            script.Append("$('#filelist" + ClientID + " li').eq(indexAtRuntime).remove(); ");
            script.Append("if(fileList.length === 0) {");
            script.Append("$('#filelist" + ClientID + "').html('');");
            script.Append("} ");
            script.Append("}); "); //end delete function

            script.Append("var item = $(\"<li>\", {text:file.name}).append(d);");
            script.Append("\n$('#filelist" + ClientID + " ul').append(item); ");

            script.Append("}\n "); // end addFileToList

            script.Append(" function addErrorToList" + ClientID + " (index,file) {");

           
            //show error messages in UI list 
            script.Append("var item = $(\"<li>\", {text:file.ErrorMessage});");
            script.Append("\n$('#filelist" + ClientID + " ul').append(item); ");

            script.Append("}\n "); // end addErrorToList

            // end util functions

            // begin self executing function

            script.Append("$(function () { ");

            // detect drag and drop
            //http://stackoverflow.com/questions/5290861/determine-if-browser-is-drag-and-drop-capable

            // hide the fallback upload button used with standard fileupload control
            // if javascript is disabled the fallback functionality will still work
            if (uploadButtonClientId.Length > 0)
            {
                script.Append("$('#" + uploadButtonClientId + "').hide(); ");
            }

            if (browseButtonCssClass.Length > 0)
            {
                script.Append("$('#browseButton" + ClientID + "').addClass('" + browseButtonCssClass + "'); ");
            }

            // this css supporting this class makes the widget overlay and hide the actual file input
            // we are replacing and standardizing the ui as per https://github.com/blueimp/jQuery-File-Upload/wiki/Style-Guide
            script.Append("$('#browseButton" + ClientID + "').addClass('fileinput-button'); ");

            if (browseButtonExtraBottomMarkup.Length > 0)
            {
                script.Append("var btnb = $(\"" + browseButtonExtraBottomMarkup + "\"); ");
                // prepending so we're working backwards this should be below the button since the button is prepended later
                script.Append("btnb.prependTo($('#browseButton" + ClientID + "')); ");
            }

            script.Append("var btnText = $(\"<button class='" + addFilesButtonCssClass + "'>");
            if (MaxFilesAllowed > 1)
            {
                script.Append(addFilesText);
            }
            else
            {
                script.Append(addFileText);
            }
            script.Append("</button>\"); ");

            script.Append("btnText.prependTo($('#browseButton" + ClientID + "')); ");

            if (browseButtonExtraTopMarkup.Length > 0)
            {
                script.Append("var btnt = $(\"" + browseButtonExtraTopMarkup + "\"); ");
                // prepending so we're working backwards this should be above the button since the button is prepended first
                script.Append("btnt.prependTo($('#browseButton" + ClientID + "')); ");
            }

            if (useDropZone)
            {
                script.Append("$('#dropZone" + ClientID + "').addClass('" + dropZoneCssClass + "'); ");
                script.Append("var dzText = $(\"<span>");
                if (MaxFilesAllowed > 1)
                {
                    script.Append(dropFilesText);
                }
                else
                {
                    script.Append(dropFileText);
                }
                script.Append("</span>\"); ");
                script.Append("dzText.prependTo($('#dropZone" + ClientID + "')); ");

                script.Append("$(document).bind('drop dragover', function (e) {");
                script.Append("e.preventDefault(); ");
                script.Append("}); ");

            }

            // initialize progress bar and hide it
            script.Append("$('#progress" + ClientID + "').progressbar({ value: 0 });");
            script.Append("$('#progress" + ClientID + "').hide(); ");

            script.Append(" var fileList" + ClientID + " = []; ");

            // the start of the file uploader
            script.Append("$('#pnlFiles" + ClientID + "').fileupload({");

            script.Append("fileInput: $('#" + ClientID + "') ");
            if (!replaceFileInput) { script.Append(",replaceFileInput:false "); }
            script.Append(",url: '" + Page.ResolveUrl(serviceUrl) + "'");
            script.Append(",limitMultiFileUploads:" + maxFilesAllowed.ToString());
            script.Append(",limitConcurrentUploads:" + maxConcurrentUploadsAllowed.ToString());

            if (maxChunkSize != "undefined") { script.Append(",maxChunkSize:" + maxChunkSize); }
            if (bitrateInterval != 500) { script.Append(",bitrateInterval:" + bitrateInterval.ToString()); }

            if (!singleFileUploads) { script.Append(",singleFileUploads:false"); }
            if (sequentialUploads) { script.Append(",sequentialUploads: true"); }

            if (forceIframeTransport) { script.Append(",forceIframeTransport: true"); }
            if (formAcceptCharset.Length > 0) { script.Append(",formAcceptCharset:'" + formAcceptCharset + "'"); }

            if (useDropZone)
            {
                script.Append(",dropZone: $('#dropZone" + ClientID + "') ");
                script.Append(",pasteZone: $('#dropZone" + ClientID + "') ");
            }


            if (requestType != "POST") { script.Append(",type:'" + requestType + "'"); }
            script.Append(",dataType: 'json'");

            script.Append(",add: function (e, data) {");

            // clear out the previous selection if any
            script.Append("$('#filelist" + ClientID + "').html('');");
            script.Append("$('#filelist" + ClientID + "').append($(\"<ul class='filelist'></ul>\"));");
            



            if (acceptFileTypes.Length > 0)
            {
                // filter out any files with invalid extensions
                // http://stackoverflow.com/questions/3661071/js-remove-element-from-an-array-based-on-regexp  
                script.Append("var regx = " + acceptFileTypes + "; ");
                script.Append("var j = 0; ");
                script.Append("var k = data.files.length; ");
                script.Append("while (j < k) { ");
                script.Append("if ((regx.test(data.files[j].name)) === false) { ");
                //script.Append("if(!(isValidFileName" + ClientID + "(data.files[j].name))) {");
                script.Append("data.files.splice(j, 1); ");
                script.Append("k = data.files.length; ");
                script.Append("j = -1; "); // will be set to 0 by ++ below
                script.Append("} ");// end if
                script.Append("j ++; ");
                script.Append("} "); // end while
            }

            // add to any previously selected files
            script.Append("fileList" + ClientID + " = fileList" + ClientID + ".concat(data.files); ");

            // limit the number of files
            script.Append("var maxAllowed = " + maxFilesAllowed.ToString() + "; ");
            script.Append("while(fileList" + ClientID + ".length > maxAllowed) {");
            script.Append("fileList" + ClientID + ".pop(); ");
            script.Append("} ");

            script.Append("data.files = fileList" + ClientID + "; ");

            // add a new upload button if valid files have been selected
            script.Append("if(data.files.length > 0) {");
            //script.Append("var btnSend = $(\"<button>\", {id:'btnSend" + ClientID + "',text:'"
            //    + uploadButtonText
            //    + "', class:'" + addFilesButtonCssClass + "'});");

            script.Append("var btnSend = $(\"<button id='btnSend" + ClientID + "' class='" + addFilesButtonCssClass + "'>"
               + uploadButtonText
               + "</button>\"); ");

            script.Append("btnSend.appendTo($('#filelist" + ClientID + "')); ");
            script.Append("} ");

            // add the files to the list with delete icons
            script.Append("$.each(data.files, function (index, file) {");
            script.Append("addFileToList" + ClientID + " (data, fileList" + ClientID + ", index, file); ");
            script.Append("}); ");

            script.Append("$('#btnSend" + ClientID + "').click(function () {");
            script.Append("data.context = $('<p/>').text('" + HttpUtility.HtmlAttributeEncode(uploadingText) + "').replaceAll($(this));");
            script.Append("data.submit(); ");
            script.Append("}); ");

            script.Append("} ");

            // this is the default behavior so not needed
            // https://github.com/blueimp/jQuery-File-Upload/wiki/How-to-submit-additional-form-data
            //script.Append(",formData:function (form) { ");
            //script.Append("return form.serializeArray(); ");
            //script.Append("}");
            // see formFieldClientId below

            script.Append(",done: function (e, data) {");

            // this isn't being shown?
            script.Append("data.context.text('" + HttpUtility.HtmlAttributeEncode(uploadCompleteText) + "');");

            // hide the progress bar and clear the file list
            script.Append("$('#progress" + ClientID + "').hide();");
            script.Append("$('#filelist" + ClientID + "').html('');");
            script.Append("fileList" + ClientID + " = []; "); //reset the list

            // set the return value from the upload as the value on the provided input id
            if (returnValueFormFieldClientId.Length > 0)
            {
                script.Append("var input = $('#" + returnValueFormFieldClientId + "'); ");
                script.Append("if(input){");

                script.Append("if (data.result && $.isArray(data.result.files)) {");
                script.Append("input.val(data.result.files[0].ReturnValue); ");
                script.Append("} "); //isarray
                script.Append("} ");
            }

            // clear out the previous selection if any
            
            script.Append("$('#filelist" + ClientID + "').append($(\"<ul class='filelist file-errors'></ul>\"));");
            // show any file errors in the UI
            script.Append("var j = 0; ");
            script.Append("var errorsOccurred = false; ");
            script.Append("while (j < data.result.files.length) { ");
            script.Append("if(data.result.files[j].ErrorMessage) {");

            script.Append("errorsOccurred = true; ");
            //script.Append("alert(data.result.files[j].ErrorMessage); ");
            script.Append("addErrorToList" + ClientID + " (j, data.result.files[j]); ");


            script.Append("} "); //end if

            script.Append("j++; ");

            script.Append("} ");// end while

            // fire custom uploadComplete callback if supplied
            if (uploadCompleteCallback.Length > 0)
            {
                script.Append(uploadCompleteCallback + "(data, errorsOccurred); ");
            }

            script.Append("}");

            if (alertOnError)
            {
                script.Append(",fail: function (e, data) {");
                script.Append("alert(data.errorThrown);");
                script.Append("}");
            }
            else
            {
                script.Append(",fail: function (e, data) {");
                script.Append("$('#filelist" + ClientID + "').html('');");
                script.Append("$('#filelist" + ClientID + "').append($(\"<ul class='filelist file-errors'></ul>\"));");
                //show error messages in UI list 
                script.Append("var item = $(\"<li>\", {text:\"" + HttpUtility.HtmlAttributeEncode(errorOcurredMessage) + "\"});");
                script.Append("\n$('#filelist" + ClientID + " ul').append(item); ");
                script.Append("}");
            }

            //http://stackoverflow.com/questions/1476542/display-value-in-jqueryui-progressbar
            script.Append(",progressall: function (e, data) {");
            script.Append("var progress = parseInt(data.loaded / data.total * 100, 10); ");
            script.Append("$('#progress" + ClientID + "').show(); ");
            script.Append("$('#progress" + ClientID + "').progressbar('option', 'value', progress);");
            script.Append("}");

            script.Append("});");


            if (formFieldClientId.Length > 0)
            {
                script.Append("$('#pnlFiles" + ClientID + "').bind('fileuploadsubmit', function (e, data) {");
                script.Append("var input = $('#" + formFieldClientId + "'); ");
                script.Append("data.formData = {frmData: input.val()}; ");
                script.Append("}); ");
            }


            script.Append("}); "); // end function

            script.Append("\n</script>");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(jQueryFileUpload),
                UniqueID,
                script.ToString(),
                false);

        }

        private void SetupCommonScript()
        {
            // register main required scripts
            ScriptManager.RegisterClientScriptInclude(
                this,
                typeof(Page),
                "jiframetransport",
                Page.ResolveUrl(iframeTransportScriptPath));

            ScriptManager.RegisterClientScriptInclude(
                this,
                typeof(Page),
                "jqupload",
                Page.ResolveUrl(uploadScriptPath));

        }



    }
}