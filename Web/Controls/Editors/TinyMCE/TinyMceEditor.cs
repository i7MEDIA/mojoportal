// Author:					
// Created:					2009-08-13
// Last Modified:			2013-11-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Resources;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.Editor
{
	/// <summary>
	/// re-write as of 2013-07-24 to implement the new version 4.x of TinyMce
	/// instead of breaking the old one revived this one that was not being used and
	/// re-implemented it against TinyMCE 4.0.2
	/// </summary>
	public class TinyMceEditor : TextBox, ITinyMceSettings
	{

		#region Configuration Properties

		private string basePath = "/ClientScript/tinymce/";

		public string BasePath
		{
			get { return basePath; }
			set { basePath = value; }
		}

		private bool autoFocus = false;
		/// <summary>
		/// This option enables you to auto focus an editor instance. The 
		/// value of this option should be an editor instance id. Editor 
		/// instance ids are specified as "mce_editor_<index>", where 
		/// index is a value starting from 0. So if there are 3 editor 
		/// instances on a page, they would have the following 
		/// ids - mce_editor_0, mce_editor_1 and mce_editor_2.
		/// </summary>
		public bool AutoFocus
		{
			get { return autoFocus; }
			set { autoFocus = value; }
		}

		private bool enableBrowserSpellCheck = true;

		public bool EnableBrowserSpellCheck
		{
			get { return enableBrowserSpellCheck; }
			set { enableBrowserSpellCheck = value; }
		}

		private bool autoLocalize = true;
		/// <summary>
		/// true by default sets the language according the culture of the executing server thread
		/// set to false if you are setting the language property
		/// </summary>
		public bool AutoLocalize
		{
			get { return autoLocalize; }
			set { autoLocalize = value; }
		}

		private string language = "en";
		/// <summary>
		/// This option should contain a language code of the language pack to 
		/// use with TinyMCE. These codes are in ISO-639-1 format to see if 
		/// your language is available check the contents of 
		/// "tinymce/jscripts/tiny_mce/langs". The default value of this 
		/// option is 'en'
		/// </summary>
		public string Language
		{
			get { return language; }
			set { language = value; }
		}

		private string textDirection = "ltr";
		/// <summary>
		/// This option specifies the default writing direction, some languages 
		/// (Like Hebrew, Arabic, Urdu...) write from right to left instead 
		/// of left to right. The default value of this option is "ltr" but 
		/// if you want to use from right to left mode specify "rtl" instead.
		/// </summary>
		public string TextDirection
		{
			get { return textDirection; }
			set { textDirection = value; }
		}

		private bool enableObjectResizing = true;
		/// <summary>
		/// This true/false option gives you the ability to turn on/off the 
		/// inline resizing controls of tables and images in Firefox/Mozilla. 
		/// These are enabled by default.
		/// </summary>
		public bool EnableObjectResizing
		{
			get { return enableObjectResizing; }
			set { enableObjectResizing = value; }
		}

		private string plugins = string.Empty;
		/// <summary>
		/// This option should contain a comma separated list of plugins. Plugins 
		/// are loaded from the "tinymce/jscripts/tiny_mce/plugins" directory, 
		/// and the plugin name matches the name of the directory. TinyMCE is 
		/// shipped with some core plugins; these are described in greater 
		/// detail in the Plugins reference.
		/// http://www.tinymce.com/wiki.php/Plugins
		/// 
		/// TinyMCE also supports the ability to have plugins added from a external 
		/// resource. These plugins need to be self registering and loaded 
		/// after the tinyMCE.init call. You should also prefix these plugins 
		/// with a "-" character, so that TinyMCE doesn't try to load it from 
		/// the TinyMCE plugins directory.
		/// 
		/// There are many third party plugins for TinyMCE; some of these may be 
		/// found under "Plugins" at SourceForge, and, if you have developed 
		/// one of your own, please contribute it to this project by uploading 
		/// it to SourceForge.
		/// </summary>
		public string Plugins
		{
			get { return plugins; }
			set { plugins = value; }
		}

		private string theme = "modern";
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:theme
		/// </summary>
		public string Theme
		{
			get { return theme; }
			set { theme = value; }
		}

		private string skin = "lightgray";
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:skin
		/// </summary>
		public string Skin
		{
			get { return skin; }
			set { skin = value; }
		}

		private int unDoLevels = -1;
		/// <summary>
		/// This option should contain the number of undo levels to keep in 
		/// memory. This is set to -1 by default and such a value tells 
		/// TinyMCE to use a unlimited number of undo levels. But this 
		/// steals lots of memory so for low end systems a value of 10 may 
		/// be better.
		/// http://www.tinymce.com/wiki.php/Configuration:custom_undo_redo_levels
		/// </summary>
		public int UnDoLevels
		{
			get { return unDoLevels; }
			set { unDoLevels = value; }
		}

		private string styleFormats = string.Empty;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:style_formats
		/// 
		/// </summary>
		public string StyleFormats
		{
			get { return styleFormats; }
			set { styleFormats = value; }
		}

		private bool disableMenuBar = false;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:menubar
		/// </summary>
		public bool DisableMenuBar
		{
			get { return disableMenuBar; }
			set { disableMenuBar = value; }
		}

		private string menubar = "file edit insert view format table tools";
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:menubar
		/// </summary>
		public string Menubar
		{
			get { return menubar; }
			set { menubar = value; }
		}

		private string toolbar1Buttons = "bold,italic,underline,strikethrough,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,formatselect,styleselect";
		/// <summary>
		/// This option should contain a comma separated list of button/control 
		/// names to insert into the toolbar.  
		/// This property populates the first row of 3 allowed rows.
		/// Below is a list of built in 
		/// controls, plugins may include other controls names that can be 
		/// inserted but these are documented in the individual plugins. 
		/// This option can only be used when theme is set to advanced and when 
		/// the theme_advanced_layout_manager option is set to the default 
		/// value of "SimpleLayout".
		/// A list of possible options can be found here:
		/// http://www.tinymce.com/wiki.php/Controls
		/// </summary>
		public string Toolbar1Buttons
		{
			get { return toolbar1Buttons; }
			set { toolbar1Buttons = value; }
		}

		private string toolbar2Buttons = "bullist,numlist,separator,outdent,indent,separator,undo,redo,separator,link,unlink,anchor,image,cleanup,help,code";
		/// <summary>
		/// This option should contain a comma separated list of button/control 
		/// names to insert into the toolbar.  
		/// This property populates row 2 of 3 allowed rows.
		/// Below is a list of built in 
		/// controls, plugins may include other controls names that can be 
		/// inserted but these are documented in the individual plugins. 
		/// This option can only be used when theme is set to advanced and when 
		/// the theme_advanced_layout_manager option is set to the default 
		/// value of "SimpleLayout".
		/// A list of possible options can be found here:
		/// http://www.tinymce.com/wiki.php/Controls
		/// </summary>
		public string Toolbar2Buttons
		{
			get { return toolbar2Buttons; }
			set { toolbar2Buttons = value; }
		}

		private string toolbar3Buttons = string.Empty;
		/// <summary>
		/// This option should contain a comma separated list of button/control 
		/// names to insert into the toolbar.  
		/// This property populates row 2 of 3 allowed rows.
		/// Below is a list of built in 
		/// controls, plugins may include other controls names that can be 
		/// inserted but these are documented in the individual plugins. 
		/// This option can only be used when theme is set to advanced and when 
		/// the theme_advanced_layout_manager option is set to the default 
		/// value of "SimpleLayout".
		/// A list of possible options can be found here:
		/// http://www.tinymce.com/wiki.php/Controls
		/// </summary>
		public string Toolbar3Buttons
		{
			get { return toolbar3Buttons; }
			set { toolbar3Buttons = value; }
		}

		private string editorAreaCSS = string.Empty;
		/// <summary>
		/// This option enables you to specify a custom CSS file that extends 
		/// the theme content CSS. This CSS file is the one used within the 
		/// editor (the editable area). The default location of this CSS file 
		/// is within the current theme. This option can also be a comma 
		/// separated list of URLs.
		/// </summary>
		public string EditorAreaCSS
		{
			get { return editorAreaCSS; }
			set { editorAreaCSS = value; }
		}

		private string editorBodyCssClass = "wysiwygeditor modulecontent art-postcontent";

		public string EditorBodyCssClass
		{
			get { return editorBodyCssClass; }
			set { editorBodyCssClass = value; }
		}


		private string templatesUrl = string.Empty;

		/// <summary>
		/// JavaScript file containing an array of template files.
		/// </summary>
		public string TemplatesUrl
		{
			get { return templatesUrl; }
			set { templatesUrl = value; }
		}

		private string emotionsBaseUrl = string.Empty;

		/// <summary>
		/// JavaScript file containing an array of template files.
		/// </summary>
		public string EmotionsBaseUrl
		{
			get { return emotionsBaseUrl; }
			set { emotionsBaseUrl = value; }
		}

		private string fileManagerUrl = string.Empty;
		public string FileManagerUrl
		{
			get { return fileManagerUrl; }
			set { fileManagerUrl = value; }
		}

		public string ImagesUploadUrl { get; set; } = string.Empty;

		private string extendedValidElements = string.Empty;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:extended_valid_elements
		/// </summary>
		public string ExtendedValidElements
		{
			get { return extendedValidElements; }
			set { extendedValidElements = value; }
		}

		private bool forcePasteAsPlainText = false;

		public bool ForcePasteAsPlainText
		{
			get { return forcePasteAsPlainText; }
			set { forcePasteAsPlainText = value; }
		}

		private bool convertUrls = false;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:convert_urls
		/// </summary>
		public bool ConvertUrls
		{
			get { return convertUrls; }
			set { convertUrls = value; }
		}

		private bool inline = false;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:inline
		/// </summary>
		public bool Inline
		{
			get { return inline; }
			set { inline = value; }
		}

		private bool noWrap = false;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Configuration:nowrap
		/// </summary>
		public bool NoWrap
		{
			get { return noWrap; }
			set { inline = noWrap; }
		}

		private string removedMenuItems = "newdocument,print";
		/// <summary>
		/// http://www.tinymce.com/forum/viewtopic.php?id=30954
		/// </summary>
		public string RemovedMenuItems
		{
			get { return removedMenuItems; }
			set { removedMenuItems = value; }
		}

		private int fileDialogHeight = 700;

		public int FileDialogHeight
		{
			get { return fileDialogHeight; }
			set { fileDialogHeight = value; }
		}

		private int fileDialogWidth = 860;

		public int FileDialogWidth
		{
			get { return fileDialogWidth; }
			set { fileDialogWidth = value; }
		}

		private bool enableImageAdvancedTab = false;
		/// <summary>
		/// this tab encourages creation of hard coded inline styles
		/// </summary>
		public bool EnableImageAdvancedTab
		{
			get { return enableImageAdvancedTab; }
			set { enableImageAdvancedTab = value; }
		}

		private bool showStatusbar = true;

		public bool ShowStatusbar
		{
			get { return showStatusbar; }
			set { showStatusbar = value; }
		}

		private string customToolbarElementClientId = string.Empty;
		/// <summary>
		/// maps to fixed_toolbar_container
		/// http://www.tinymce.com/wiki.php/Configuration:fixed_toolbar_container
		/// </summary>
		public string CustomToolbarElementClientId
		{
			get { return customToolbarElementClientId; }
			set { customToolbarElementClientId = value; }
		}

		private string globarVarToAssignEditor = string.Empty;

		public string GlobarVarToAssignEditor
		{
			get { return globarVarToAssignEditor; }
			set { globarVarToAssignEditor = value; }
		}

		private string onSaveCallback = string.Empty;
		/// <summary>
		/// http://www.tinymce.com/wiki.php/Plugin:save
		/// </summary>
		public string OnSaveCallback
		{
			get { return onSaveCallback; }
			set { onSaveCallback = value; }
		}

		private bool saveEnableWhenDirty = true;

		public bool SaveEnableWhenDirty
		{
			get { return saveEnableWhenDirty; }
			set { saveEnableWhenDirty = value; }
		}

		private bool promptOnNavigationWithUnsavedChanges = true;

		public bool PromptOnNavigationWithUnsavedChanges
		{
			get { return promptOnNavigationWithUnsavedChanges; }
			set { promptOnNavigationWithUnsavedChanges = value; }
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			//Page.EnableViewState = true;
			//this.EnableViewState = true;
			TextMode = TextBoxMode.MultiLine;
			basePath = WebConfigSettings.TinyMceBasePath;
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			SetupScripts();
		}

		private void SetupScripts()
		{
			Page.ClientScript.RegisterClientScriptBlock(
				GetType(),
				"tinymcemain",
				"<script data-loader=\"TinyMCE\" src=\"" + ResolveUrl(BasePath + "tinymce.min.js") + "\"></script>"
			);

			StringBuilder script = new StringBuilder();

			//script.Append("function mojoTinyMCEOnChangeHandler(inst) {");
			//script.Append("hookupGoodbyePrompt(\"" + Resource.UnSavedChangesPrompt.HtmlEscapeQuotes().RemoveLineBreaks() + "\"); ");
			//script.Append("} ");

			script.Append("\n<script data-loader=\"TinyMCE\">");

			BuildScript(script, this, ClientID, Width, Height);

			script.Append("</script>");

			ScriptManager.RegisterStartupScript(
				this,
				GetType(),
				UniqueID,
				script.ToString(),
				false
			);
		}

		public static void BuildScript(StringBuilder script, ITinyMceSettings config, string targetClientId, Unit width, Unit height)
		{
			if (script == null) { return; }
			if (config == null) { return; }

			script.Append("tinymce.init({");

			if (config.Inline)
			{
				script.Append("inline:true,");
			}

			script.Append("selector:\"#" + targetClientId + "\"");

			//http://www.tinymce.com/wiki.php/Configuration

			if (width != Unit.Empty)
			{
				script.Append(",width:\"" + width.ToString() + "\"");
			}

			if (height != Unit.Empty)
			{
				script.Append(",height: \"" + height.ToString() + "\"");
			}

			if (!config.ConvertUrls)
			{
				script.Append(",convert_urls:false");
			}

			if (config.NoWrap)
			{
				script.Append(",nowrap:true");
			}

			if (config.DisableMenuBar)
			{
				script.Append(",menubar:false");
			}
			else
			{
				script.Append(",menubar:'" + config.Menubar + "'");
			}

			if ((!config.ShowStatusbar) || (config.Inline))
			{
				script.Append(",statusbar:false");
			}

			if (config.ForcePasteAsPlainText)
			{
				script.Append(",paste_as_text:true");
			}

			if (config.RemovedMenuItems.Length > 0)
			{
				script.Append(",removed_menuitems: '" + config.RemovedMenuItems + "'");
			}

			script.Append(",schema:'html5'");

			if (config.CustomToolbarElementClientId.Length > 0)
			{
				script.Append(",fixed_toolbar_container:'" + config.CustomToolbarElementClientId + "'");
			}

			if (config.EnableBrowserSpellCheck)
			{
				script.Append(",browser_spellcheck:true ");
			}

			if (config.AutoLocalize)
			{
				CultureInfo culture;
				if (WebConfigSettings.UseCultureOverride)
				{
					culture = SiteUtils.GetDefaultUICulture();
				}
				else
				{
					culture = CultureInfo.CurrentUICulture;
				}

				config.Language = GetSupportedLangCode(culture.Name, culture.TwoLetterISOLanguageName);

				if (culture.TextInfo.IsRightToLeft)
				{
					config.TextDirection = "rtl";
				}
			}

			if (config.Language.Length > 0)
			{
				script.Append(",language:\"" + config.Language + "\"");
			}

			if (config.ExtendedValidElements.Length > 0)
			{
				script.Append(",extended_valid_elements:\"" + config.ExtendedValidElements + "\"");
			}

			if ((config.TextDirection != "ltr") && (config.TextDirection.Length > 0))
			{
				script.Append(",directionality:'" + config.TextDirection + "'");
			}

			if (!config.EnableObjectResizing)
			{
				script.Append(",object_resizing:false");
			}

			// http://www.tinymce.com/wiki.php/Plugins
			if (config.Plugins.Length > 0)
			{
				script.Append(",plugins:\"" + config.Plugins + "\"");
			}

			if (config.Theme != "modern")
			{
				script.Append(",theme:\"" + config.Theme + "\"");
			}

			if (config.Skin != "lightgray")
			{
				script.Append(",skin:\"" + config.Skin + "\"");
			}

			if (config.UnDoLevels != -1)
			{
				script.Append(",custom_undo_redo_levels:" + config.UnDoLevels.ToInvariantString());
			}

			if (config.Toolbar1Buttons.Length > 0)
			{
				script.Append(",toolbar1:\"" + config.Toolbar1Buttons + "\"");
			}

			if (config.Toolbar2Buttons.Length > 0)
			{
				script.Append(",toolbar2:\"" + config.Toolbar2Buttons + "\"");
			}

			if (config.Toolbar3Buttons.Length > 0)
			{
				script.Append(",toolbar3:\"" + config.Toolbar3Buttons + "\"");
			}

			if (config.EnableImageAdvancedTab)
			{
				script.Append(",image_advtab:true");
			}

			if (config.EditorAreaCSS.Length > 0)
			{
				script.Append(",content_css:\"" + config.EditorAreaCSS + "\"");
			}

			if (config.EditorBodyCssClass.Length > 0)
			{
				script.Append(",body_class:\"" + config.EditorBodyCssClass + "\"");
			}

			if (config.AutoFocus)
			{
				script.Append(",auto_focus:\"" + targetClientId + "\" ");
			}

			if (config.TemplatesUrl.Length > 0)
			{
				script.Append(",templates: \"" + config.TemplatesUrl + "\"");
			}

			if (config.StyleFormats.Length > 0)
			{
				script.Append(",style_formats_merge:true,style_formats:" + config.StyleFormats);
			}

			if (config.EmotionsBaseUrl.Length > 0)
			{
				script.Append(",emotions_images_url:'" + config.EmotionsBaseUrl + "'");
			}

			if (config.OnSaveCallback.Length > 0)
			{
				script.Append(",save_onsavecallback:" + config.OnSaveCallback);
				if (config.SaveEnableWhenDirty)
				{
					script.Append(",save_enablewhendirty:true");
				}
			}

			if (config.ImagesUploadUrl.Length > 0)
			{
				script.Append(",images_upload_url:'" + config.ImagesUploadUrl + "'");
			}

			if (config.FileManagerUrl.Length > 0)
			{
				script.Append(@$"
,file_picker_types: 'file image media'
,file_picker_callback: function(callback, value, meta) {{
	window.tinymceCallBackURL = '';
	window.tinymceWindowManager = tinymce.activeEditor.windowManager;
	tinymceWindowManager.openUrl({{title: '{Resource.FileBrowser.HtmlEscapeQuotes()}',
		url: '{config.FileManagerUrl}?editor=tinymce&type=' + meta.filetype,
		width: ~~((80 / 100) * window.innerWidth),
		height: ~~((80 / 100) * window.innerWidth),
		onClose: function() {{
			callback(tinymceCallBackURL);
		}}
	}});
}}
");
			}

			script.Append(",promotion: false,setup:function(editor) {");

			if (config.GlobarVarToAssignEditor.Length > 0)
			{
				script.Append(config.GlobarVarToAssignEditor + " = editor; ");
			}

			if (config.PromptOnNavigationWithUnsavedChanges)
			{
				// autosave plugin also prompts so don't need this if it is used
				if (!config.Plugins.Contains("autosave,"))
				{
					script.Append("editor.on('change', function(e) {");
					script.Append("hookupGoodbyePrompt(\"" + Resource.UnSavedChangesPrompt.HtmlEscapeQuotes().RemoveLineBreaks() + "\"); ");
					script.Append("});");
				}
			}

			script.Append("}"); //end setup
			script.Append("});");
		}

		public static string GetSupportedLangCode(string cultureName, string cultureCode)
		{
			// a list of supported languages can be found here:
			// http://jquery-ui.googlecode.com/svn/trunk/ui/i18n/

			switch (cultureName)
			{

				case "bg-BG":
				case "de-AT":
				case "en-CA":
				case "en-GB":
				case "fr-FR":
				case "he-IL":
				case "hu-HU":
				case "ka-GE":
				case "ko-KR":
				case "nb-NO":
				case "pt-BR":
				case "pt-PT":
				case "sl-SL":
				case "sv-SE":
				case "ta-IN":
				case "th-TH":
				case "tr-TR":
				case "uk-UA":
				case "vi-VN":
				case "zh-CN":
				case "zh-TW":
					return cultureName.Replace("-", "_");
			}

			switch (cultureCode)
			{
				case "fr":
					return "fr_FR";

				case "he":
					return "he_IL";

				case "hu":
					return "hu_HU";

				case "ka":
					return "ka_GE";

				case "ko":
					return "ko_KR";

				case "pt":
					return "pt_PT";

				case "tr":
					return "tr_TR";

				//case "af":
				case "ar":
				//case "az":
				case "bg":
				//case "bs":
				case "ca":
				case "cs":
				case "cy":
				case "da":
				case "de":
				case "el":
				// case "eo":
				case "es":
				//case "et":
				//case "eu":
				case "fa":
				case "fi":
				case "fo":

				case "hr":

				case "hy":
				case "id":
				//case "is":
				case "it":
				case "ja":

				//case "lt":
				//case "lv":
				//case "ms":
				case "nl":
				//case "no":
				case "pl":

				case "ro":
				case "ru":
				case "sk":
				case "sl":
				//case "sq":
				case "sr":
				case "sv":
				case "ta":

				case "uk":
				case "vi":
					return cultureCode;

				default:
					//return "en";
					return cultureCode;
			}
		}

		//    private void SetupFileBrowserScript()
		//    {
		//        //http://www.tinymce.com/forum/viewtopic.php?pid=108477#p108477
		//        //http://www.tinymce.com/wiki.php/Tutorials:Creating_custom_dialogs

		//        StringBuilder script = new StringBuilder();
		//        script.Append("\n<script type=\"text/javascript\">");

		//        //string fileManagerUrl = SiteUtils.GetNavigationSiteRoot() + "/Dialog/MCEFileDialog.aspx";

		//        script.Append("var serviceUrl = '" + fileManagerUrl + "'; ");
		//        script.Append("function myFileBrowser (field_name, url, type, win) {");
		//        //script.Append("alert(type);");
		//        script.Append("tinyMCE.activeEditor.windowManager.open({");
		//        //script.Append("file : serviceUrl + '?ed=tmc&type=' + type, ");
		//        script.Append("file : serviceUrl + '?ed=tmc&type=' + type, ");
		//        script.Append("title : '" + Resource.FileBrowser.HtmlEscapeQuotes() + "', ");
		//        script.Append("width : 860,");
		//        script.Append("height : 700,");
		//        script.Append("resizable : 'yes',");
		//        script.Append("inline : 'yes',");
		//        script.Append(" close_previous : 'no'");
		//        script.Append("}, {");
		//        script.Append("window : win,");
		//        script.Append("input : field_name");
		//        script.Append(",oninsert: function(url) {win.document.getElementById(field_name).value = url;}");
		//        script.Append("}); ");
		//        script.Append("return false;");
		//        script.Append("}");
		//        script.Append("\n</script>");

		//        ScriptManager.RegisterClientScriptBlock(
		//            this,
		//            typeof(Page),
		//            "tmcfb",
		//            script.ToString(),
		//            false
		//		);
		//    }
	}
}
