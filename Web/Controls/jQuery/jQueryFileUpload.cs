using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI;

//https://github.com/blueimp/jQuery-File-Upload/issues/638#issuecomment-2215669
//https://github.com/blueimp/jQuery-File-Upload/wiki/Basic-plugin
//https://github.com/blueimp/jQuery-File-Upload/wiki/Options

public class jQueryFileUpload : FileUpload
{
	[Themeable(false)]
	public string ServiceUrl { get; set; } = string.Empty;

	[Themeable(false)]
	public int MaxFilesAllowed { get; set; } = 1;

	[Themeable(false)]
	public int MaxConcurrentUploadsAllowed { get; set; } = 1;

	[Themeable(false)]
	public string IframeTransportScriptPath { get; set; } = "~/ClientScript/jqfileupload/jquery.iframe-transport.js";
	[Themeable(false)]
	public string UploadScriptPath { get; set; } = "~/ClientScript/jqfileupload/jquery.fileupload.js";

	[Themeable(false)]
	public bool SingleFileUploads { get; set; } = false;
	[Themeable(false)]
	public bool SequentialUploads { get; set; } = false;

	[Themeable(false)]
	public bool ReplaceFileInput { get; set; } = true;
	[Themeable(false)]
	public bool ForceIframeTransport { get; set; } = false;

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

	[Themeable(false)]
	public string FormAcceptCharset { get; set; } = string.Empty;
	[Themeable(false)]
	public string MaxChunkSize { get; set; } = "undefined";

	[Themeable(false)]
	public int BitrateInterval { get; set; } = 500;

	/// <summary>
	/// if provided the javascript will hide this button since
	/// the ajax ui will replace the normal postback functionality
	/// </summary>
	[Themeable(false)]
	public string UploadButtonClientId { get; set; } = string.Empty;

	/// <summary>
	/// override to localize
	/// </summary>
	[Themeable(false)]
	public string AddFilesText { get; set; } = "Select files...";

	/// <summary>
	/// override to localize, used when only 1 file is allowed
	/// </summary>
	[Themeable(false)]
	public string AddFileText { get; set; } = "Select file...";

	[Themeable(false)]
	public string UploadButtonText { get; set; } = "Upload";
	[Themeable(false)]
	public string UploadingText { get; set; } = "Uploading...";

	[Themeable(false)]
	public string UploadCompleteText { get; set; } = "Upload finished.";

	/// <summary>
	/// a custom javacript function name to call after upload completes
	/// </summary>
	public string UploadCompleteCallback { get; set; } = string.Empty;

	public string AddFilesButtonCssClass { get; set; } = "btn btn-default ui-button ui-widget ui-state-default ui-corner-all";

	public string BrowseButtonCssClass { get; set; } = string.Empty;

	public string BrowseButtonExtraTopMarkup { get; set; } = string.Empty;

	public string BrowseButtonExtraBottomMarkup { get; set; } = string.Empty;

	public string DeleteSpanCssClass { get; set; } = "ui-icon ui-icon-trash";

	public string eErrorDeleteSpanCssClass { get; set; } = "ui-icon ui-icon-circle-close";

	public bool UseDropZone { get; set; } = true;

	public string DropZoneCssClass { get; set; } = "fileupload-dropzone";

	public string DropFileText { get; set; } = "Drag and drop a file here";

	public string DropFilesText { get; set; } = "Drag and drop files here";

	public string ErrorOcurredMessage { get; set; } = Resource.UploadErrorMessage;

	/// <summary>
	/// for debugging purposes
	/// </summary>
	[Themeable(false)]
	public bool AlertOnError { get; set; } = false;

	/// <summary>
	/// if provided then the value of this field will be passed as frmData instead of all form variables
	/// </summary>
	[Themeable(false)]
	public string FormFieldClientId { get; set; } = string.Empty;

	/// <summary>
	/// if provided then the returnvalue from the service will be populated in this form element
	/// typical use would be a hidden field
	/// </summary>
	[Themeable(false)]
	public string ReturnValueFormFieldClientId { get; set; } = string.Empty;

	[Themeable(false)]
	public string AcceptFileTypes { get; set; } = string.Empty;

	protected override void Render(HtmlTextWriter writer)
	{
		if (!string.IsNullOrEmpty(ServiceUrl))
		{
			if (UseDropZone)
			{
				writer.Write(" <div id='dropZone" + ClientID + "'></div>");
			}


			writer.Write("<div id='pnlFiles" + ClientID + "' class='uploadcontainer'>");

			writer.Write("<div id='browseButton" + ClientID + "'>");
			//writer.Write("<span class='jqbutton ui-button ui-widget ui-state-default ui-corner-all'>Add files...</span>");
		}

		base.Render(writer);

		if (!string.IsNullOrEmpty(ServiceUrl))
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
			ServiceUrl = string.Empty;
			UseDropZone = false;
			return;
		}

		if (string.IsNullOrEmpty(ServiceUrl)) { return; }

		if (MaxFilesAllowed > 1) { Attributes.Add("multiple", "multiple"); }

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
		script.Append("var d = $(\"<span class='" + DeleteSpanCssClass + "' title='Remove'></span>\")");
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
		if (UploadButtonClientId.Length > 0)
		{
			script.Append("$('#" + UploadButtonClientId + "').hide(); ");
		}

		if (BrowseButtonCssClass.Length > 0)
		{
			script.Append("$('#browseButton" + ClientID + "').addClass('" + BrowseButtonCssClass + "'); ");
		}

		// this css supporting this class makes the widget overlay and hide the actual file input
		// we are replacing and standardizing the ui as per https://github.com/blueimp/jQuery-File-Upload/wiki/Style-Guide
		script.Append("$('#browseButton" + ClientID + "').addClass('fileinput-button'); ");

		if (BrowseButtonExtraBottomMarkup.Length > 0)
		{
			script.Append("var btnb = $(\"" + BrowseButtonExtraBottomMarkup + "\"); ");
			// prepending so we're working backwards this should be below the button since the button is prepended later
			script.Append("btnb.prependTo($('#browseButton" + ClientID + "')); ");
		}

		script.Append("var btnText = $(\"<button class='" + AddFilesButtonCssClass + "'>");
		if (MaxFilesAllowed > 1)
		{
			script.Append(AddFilesText);
		}
		else
		{
			script.Append(AddFileText);
		}
		script.Append("</button>\"); ");

		script.Append("btnText.prependTo($('#browseButton" + ClientID + "')); ");

		if (BrowseButtonExtraTopMarkup.Length > 0)
		{
			script.Append("var btnt = $(\"" + BrowseButtonExtraTopMarkup + "\"); ");
			// prepending so we're working backwards this should be above the button since the button is prepended first
			script.Append("btnt.prependTo($('#browseButton" + ClientID + "')); ");
		}

		if (UseDropZone)
		{
			script.Append("$('#dropZone" + ClientID + "').addClass('" + DropZoneCssClass + "'); ");
			script.Append("var dzText = $(\"<span>");
			if (MaxFilesAllowed > 1)
			{
				script.Append(DropFilesText);
			}
			else
			{
				script.Append(DropFileText);
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
		if (!ReplaceFileInput) { script.Append(",replaceFileInput:false "); }
		script.Append(",url: '" + Page.ResolveUrl(ServiceUrl) + "'");
		script.Append(",limitMultiFileUploads:" + MaxFilesAllowed.ToString());
		script.Append(",limitConcurrentUploads:" + MaxConcurrentUploadsAllowed.ToString());

		if (MaxChunkSize != "undefined") { script.Append(",maxChunkSize:" + MaxChunkSize); }
		if (BitrateInterval != 500) { script.Append(",bitrateInterval:" + BitrateInterval.ToString()); }

		if (!SingleFileUploads) { script.Append(",singleFileUploads:false"); }
		if (SequentialUploads) { script.Append(",sequentialUploads: true"); }

		if (ForceIframeTransport) { script.Append(",forceIframeTransport: true"); }
		if (FormAcceptCharset.Length > 0) { script.Append(",formAcceptCharset:'" + FormAcceptCharset + "'"); }

		if (UseDropZone)
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




		if (AcceptFileTypes.Length > 0)
		{
			// filter out any files with invalid extensions
			// http://stackoverflow.com/questions/3661071/js-remove-element-from-an-array-based-on-regexp  
			script.Append("var regx = " + AcceptFileTypes + "; ");
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
		script.Append("var maxAllowed = " + MaxFilesAllowed.ToString() + "; ");
		script.Append("while(fileList" + ClientID + ".length > maxAllowed) {");
		script.Append("fileList" + ClientID + ".pop(); ");
		script.Append("} ");

		script.Append("data.files = fileList" + ClientID + "; ");

		// add a new upload button if valid files have been selected
		script.Append("if(data.files.length > 0) {");
		//script.Append("var btnSend = $(\"<button>\", {id:'btnSend" + ClientID + "',text:'"
		//    + uploadButtonText
		//    + "', class:'" + addFilesButtonCssClass + "'});");

		script.Append("var btnSend = $(\"<button id='btnSend" + ClientID + "' class='" + AddFilesButtonCssClass + "'>"
		   + UploadButtonText
		   + "</button>\"); ");

		script.Append("btnSend.appendTo($('#filelist" + ClientID + "')); ");
		script.Append("} ");

		// add the files to the list with delete icons
		script.Append("$.each(data.files, function (index, file) {");
		script.Append("addFileToList" + ClientID + " (data, fileList" + ClientID + ", index, file); ");
		script.Append("}); ");

		script.Append("$('#btnSend" + ClientID + "').click(function () {");
		script.Append("data.context = $('<p/>').text('" + HttpUtility.HtmlAttributeEncode(UploadingText) + "').replaceAll($(this));");
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
		script.Append("data.context.text('" + HttpUtility.HtmlAttributeEncode(UploadCompleteText) + "');");

		// hide the progress bar and clear the file list
		script.Append("$('#progress" + ClientID + "').hide();");
		script.Append("$('#filelist" + ClientID + "').html('');");
		script.Append("fileList" + ClientID + " = []; "); //reset the list

		// set the return value from the upload as the value on the provided input id
		if (ReturnValueFormFieldClientId.Length > 0)
		{
			script.Append("var input = $('#" + ReturnValueFormFieldClientId + "'); ");
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
		if (UploadCompleteCallback.Length > 0)
		{
			script.Append(UploadCompleteCallback + "(data, errorsOccurred); ");
		}

		script.Append("}");

		if (AlertOnError)
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
			script.Append("var item = $(\"<li>\", {text:\"" + HttpUtility.HtmlAttributeEncode(ErrorOcurredMessage) + "\"});");
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


		if (FormFieldClientId.Length > 0)
		{
			script.Append("$('#pnlFiles" + ClientID + "').bind('fileuploadsubmit', function (e, data) {");
			script.Append("var input = $('#" + FormFieldClientId + "'); ");
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
			Page.ResolveUrl(IframeTransportScriptPath));

		ScriptManager.RegisterClientScriptInclude(
			this,
			typeof(Page),
			"jqupload",
			Page.ResolveUrl(UploadScriptPath));

	}



}