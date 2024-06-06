using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Core.Extensions;
using Resources;


namespace mojoPortal.Web.Editor;

public class TinyMceEditor : TextBox, ITinyMceSettings
{

	#region Configuration Properties


	public string BasePath { get; set; } = "/ClientScript/tinymce/";

	/// <summary>
	/// This option enables you to auto focus an editor instance. The 
	/// value of this option should be an editor instance id. Editor 
	/// instance ids are specified as "mce_editor_<index>", where 
	/// index is a value starting from 0. So if there are 3 editor 
	/// instances on a page, they would have the following 
	/// ids - mce_editor_0, mce_editor_1 and mce_editor_2.
	/// </summary>
	public bool AutoFocus { get; set; } = false;

	public bool EnableBrowserSpellCheck { get; set; } = true;

	/// <summary>
	/// true by default sets the language according the culture of the executing server thread
	/// set to false if you are setting the language property
	/// </summary>
	public bool AutoLocalize { get; set; } = true;

	/// <summary>
	/// This option should contain a language code of the language pack to 
	/// use with TinyMCE. These codes are in ISO-639-1 format to see if 
	/// your language is available check the contents of 
	/// "tinymce/jscripts/tiny_mce/langs". The default value of this 
	/// option is 'en'
	/// </summary>
	public string Language { get; set; } = "en";

	/// <summary>
	/// This option specifies the default writing direction, some languages 
	/// (Like Hebrew, Arabic, Urdu...) write from right to left instead 
	/// of left to right. The default value of this option is "ltr" but 
	/// if you want to use from right to left mode specify "rtl" instead.
	/// </summary>
	public string TextDirection { get; set; } = "ltr";

	/// <summary>
	/// This true/false option gives you the ability to turn on/off the 
	/// inline resizing controls of tables and images in Firefox/Mozilla. 
	/// These are enabled by default.
	/// </summary>
	public bool EnableObjectResizing { get; set; } = true;

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
	public string Plugins { get; set; } = string.Empty;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:theme
	/// </summary>
	public string Theme { get; set; } = "modern";

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:skin
	/// </summary>
	public string Skin { get; set; } = "lightgray";

	/// <summary>
	/// This option should contain the number of undo levels to keep in 
	/// memory. This is set to -1 by default and such a value tells 
	/// TinyMCE to use a unlimited number of undo levels. But this 
	/// steals lots of memory so for low end systems a value of 10 may 
	/// be better.
	/// http://www.tinymce.com/wiki.php/Configuration:custom_undo_redo_levels
	/// </summary>
	public int UnDoLevels { get; set; } = -1;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:style_formats
	/// 
	/// </summary>
	public string StyleFormats { get; set; } = string.Empty;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:menubar
	/// </summary>
	public bool DisableMenuBar { get; set; } = false;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:menubar
	/// </summary>
	public string Menubar { get; set; } = "file edit insert view format table tools";

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
	public string Toolbar1Buttons { get; set; } = "bold,italic,underline,strikethrough,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,formatselect,styleselect";

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
	public string Toolbar2Buttons { get; set; } = "bullist,numlist,separator,outdent,indent,separator,undo,redo,separator,link,unlink,anchor,image,cleanup,help,code";

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
	public string Toolbar3Buttons { get; set; } = string.Empty;

	/// <summary>
	/// This option enables you to specify a custom CSS file that extends 
	/// the theme content CSS. This CSS file is the one used within the 
	/// editor (the editable area). The default location of this CSS file 
	/// is within the current theme. This option can also be a comma 
	/// separated list of URLs.
	/// </summary>
	public string EditorAreaCSS { get; set; } = string.Empty;

	public string EditorBodyCssClass { get; set; } = "wysiwygeditor modulecontent";

	/// <summary>
	/// JavaScript file containing an array of template files.
	/// </summary>
	public string TemplatesUrl { get; set; } = string.Empty;

	/// <summary>
	/// JavaScript file containing an array of template files.
	/// </summary>
	public string EmotionsBaseUrl { get; set; } = string.Empty;

	public string FileManagerUrl { get; set; } = string.Empty;

	public string ImagesUploadUrl { get; set; } = string.Empty;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:extended_valid_elements
	/// </summary>
	public string ExtendedValidElements { get; set; } = string.Empty;

	public bool ForcePasteAsPlainText { get; set; } = false;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:convert_urls
	/// </summary>
	public bool ConvertUrls { get; set; } = false;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:inline
	/// </summary>
	public bool Inline { get; set; } = false;

	private bool noWrap = false;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Configuration:nowrap
	/// </summary>
	public bool NoWrap
	{
		get { return noWrap; }
		set { Inline = noWrap; }
	}

	/// <summary>
	/// http://www.tinymce.com/forum/viewtopic.php?id=30954
	/// </summary>
	public string RemovedMenuItems { get; set; } = "newdocument,print";

	public int FileDialogHeight { get; set; } = 700;

	public int FileDialogWidth { get; set; } = 860;

	/// <summary>
	/// this tab encourages creation of hard coded inline styles
	/// </summary>
	public bool EnableImageAdvancedTab { get; set; } = false;

	public bool ShowStatusbar { get; set; } = true;

	/// <summary>
	/// maps to fixed_toolbar_container
	/// http://www.tinymce.com/wiki.php/Configuration:fixed_toolbar_container
	/// </summary>
	public string CustomToolbarElementClientId { get; set; } = string.Empty;

	public string GlobarVarToAssignEditor { get; set; } = string.Empty;

	/// <summary>
	/// http://www.tinymce.com/wiki.php/Plugin:save
	/// </summary>
	public string OnSaveCallback { get; set; } = string.Empty;

	public bool SaveEnableWhenDirty { get; set; } = true;

	public bool PromptOnNavigationWithUnsavedChanges { get; set; } = true;

	#endregion

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		//Page.EnableViewState = true;
		//this.EnableViewState = true;
		TextMode = TextBoxMode.MultiLine;
		BasePath = ConfigHelper.GetStringProperty("TinyMCE:BasePath", "/ClientScript/tinymce641/");
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
			$"<script data-loader=\"TinyMCE\" src=\"{ResolveUrl($"{BasePath}tinymce.min.js")}\"></script>"
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
		if (script == null || config == null)
		{
			return;
		}

		script.Append("tinymce.init({");

		if (config.Inline)
		{
			script.Append("inline:true,");
		}

		script.Append($"selector:\"#{targetClientId}\"");

		//http://www.tinymce.com/wiki.php/Configuration

		if (width != Unit.Empty)
		{
			script.Append($",width:\"{width}\"");
		}

		if (height != Unit.Empty)
		{
			script.Append($",height: \"{height}\"");
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
			script.Append($",menubar:'{config.Menubar}'");
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
			script.Append($",removed_menuitems: '{config.RemovedMenuItems}'");
		}

		script.Append(",schema:'html5'");

		if (config.CustomToolbarElementClientId.Length > 0)
		{
			script.Append($",fixed_toolbar_container:'{config.CustomToolbarElementClientId}'");
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
				script.Append($"hookupGoodbyePrompt(\"{Resource.UnSavedChangesPrompt.HtmlEscapeQuotes().RemoveLineBreaks()}\"); ");
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

		return cultureName switch
		{
			"bg-BG" or "de-AT" or "en-CA" or "en-GB" or "fr-FR" or "he-IL" or "hu-HU" or "ka-GE" or "ko-KR" or "nb-NO" or "pt-BR" or "pt-PT" or "sl-SL" or "sv-SE" or "ta-IN" or "th-TH" or "tr-TR" or "uk-UA" or "vi-VN" or "zh-CN" or "zh-TW" => cultureName.Replace("-", "_"),
			_ => cultureCode switch
			{
				"fr" => "fr_FR",
				"he" => "he_IL",
				"hu" => "hu_HU",
				"ka" => "ka_GE",
				"ko" => "ko_KR",
				"pt" => "pt_PT",
				"tr" => "tr_TR",
				//case "af":
				"ar" or "bg" or "ca" or "cs" or "cy" or "da" or "de" or "el" or "es" or "fa" or "fi" or "fo" or "hr" or "hy" or "id" or "it" or "ja" or "nl" or "pl" or "ro" or "ru" or "sk" or "sl" or "sr" or "sv" or "ta" or "uk" or "vi" => cultureCode,
				_ => cultureCode,//return "en";
			},
		};
	}
}
