// Author:					
// Created:				    2009-09-14
// Last Modified:			2013-11-05
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
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.Web.Editor
{
	public interface ITinyMceSettings
	{
		bool Inline { get; set; }
		bool AutoFocus { get; set; }
		string Menubar { get; set; }
		bool DisableMenuBar { get; set; }
		string Plugins { get; set; }
		string Toolbar1Buttons { get; set; }
		string Toolbar2Buttons { get; set; }
		string Toolbar3Buttons { get; set; }
		string ExtendedValidElements { get; set; }
		bool ForcePasteAsPlainText { get; set; }
		bool ConvertUrls { get; set; }
		bool EnableObjectResizing { get; set; }
		int UnDoLevels { get; set; }
		string Theme { get; set; }
		string Skin { get; set; }
		bool AutoLocalize { get; set; }
		string Language { get; set; }
		string TextDirection { get; set; }
		bool EnableBrowserSpellCheck { get; set; }
		string EditorBodyCssClass { get; set; }
		bool NoWrap { get; set; }
		string RemovedMenuItems { get; set; }
		int FileDialogHeight { get; set; }
		int FileDialogWidth { get; set; }
		bool EnableImageAdvancedTab { get; set; }
		bool ShowStatusbar { get; set; }

		string CustomToolbarElementClientId { get; set; }
		string EditorAreaCSS { get; set; }
		string TemplatesUrl { get; set; }
		string StyleFormats { get; set; }
		string EmotionsBaseUrl { get; set; }
		string FileManagerUrl { get; set; }
		string GlobarVarToAssignEditor { get; set; }
		string OnSaveCallback { get; set; }
		bool SaveEnableWhenDirty { get; set; }
		bool PromptOnNavigationWithUnsavedChanges { get; set; }
		string DropFileUploadUrl { get; set; }
	}

	public class TinyMceSettings : ITinyMceSettings
	{
		public TinyMceSettings()
		{

		}

		public TinyMceSettings Clone()
		{
			return this.MemberwiseClone() as TinyMceSettings;
		}

		#region NonConfig Settings
		// these are not populated from the config file but can be set programatically

		private bool inline = false;
		public bool Inline
		{
			get { return inline; }
			set { inline = value; }
		}

		private string onSaveCallback = string.Empty;

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

		private string customToolbarElementClientId = string.Empty;

		public string CustomToolbarElementClientId
		{
			get { return customToolbarElementClientId; }
			set { customToolbarElementClientId = value; }
		}

		private string editorAreaCSS = string.Empty;

		public string EditorAreaCSS
		{
			get { return editorAreaCSS; }
			set { editorAreaCSS = value; }
		}

		private string templatesUrl = string.Empty;

		public string TemplatesUrl
		{
			get { return templatesUrl; }
			set { templatesUrl = value; }
		}

		private string styleFormats = string.Empty;

		public string StyleFormats
		{
			get { return styleFormats; }
			set { styleFormats = value; }
		}

		private string emotionsBaseUrl = string.Empty;

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

		private string dropFileUploadUrl = string.Empty;
		public string DropFileUploadUrl
		{
			get { return dropFileUploadUrl; }
			set { dropFileUploadUrl = value; }
		}

		private string globarVarToAssignEditor = string.Empty;

		public string GlobarVarToAssignEditor
		{
			get { return globarVarToAssignEditor; }
			set { globarVarToAssignEditor = value; }
		}

		#endregion

		private string name = "NotFound";

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private bool autoFocus = false;
		public bool AutoFocus
		{
			get { return autoFocus; }
			set { autoFocus = value; }
		}

		private string menubar = string.Empty;

		public string Menubar
		{
			get { return menubar; }
			set { menubar = value; }
		}

		private bool disableMenuBar = true;

		public bool DisableMenuBar
		{
			get { return disableMenuBar; }
			set { disableMenuBar = value; }
		}

		private string plugins = "paste,emotions,directionality,inlinepopups";
		public string Plugins
		{
			get { return plugins; }
			set { plugins = value; }
		}

		private string toolbar1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";
		public string Toolbar1Buttons
		{
			get { return toolbar1Buttons; }
			set { toolbar1Buttons = value; }
		}

		private string toolbar2Buttons = string.Empty;
		public string Toolbar2Buttons
		{
			get { return toolbar2Buttons; }
			set { toolbar2Buttons = value; }
		}

		private string toolbar3Buttons = string.Empty;
		public string Toolbar3Buttons
		{
			get { return toolbar3Buttons; }
			set { toolbar3Buttons = value; }
		}

		private string extendedValidElements = string.Empty;
		public string ExtendedValidElements
		{
			get { return extendedValidElements; }
			set { extendedValidElements = value; }
		}

		private bool forcePasteAsPlainText = true;
		public bool ForcePasteAsPlainText
		{
			get { return forcePasteAsPlainText; }
			set { forcePasteAsPlainText = value; }
		}

		private bool convertUrls = false;
		public bool ConvertUrls
		{
			get { return convertUrls; }
			set { convertUrls = value; }
		}

		private bool enableObjectResizing = false;
		public bool EnableObjectResizing
		{
			get { return enableObjectResizing; }
			set { enableObjectResizing = value; }
		}

		private int unDoLevels = 10;
		public int UnDoLevels
		{
			get { return unDoLevels; }
			set { unDoLevels = value; }
		}

		private string theme = "modern";

		public string Theme
		{
			get { return theme; }
			set { theme = value; }
		}

		private string skin = "lightgray";

		public string Skin
		{
			get { return skin; }
			set { skin = value; }
		}

		private bool autoLocalize = true;

		public bool AutoLocalize
		{
			get { return autoLocalize; }
			set { autoLocalize = value; }
		}

		private string language = "en";

		public string Language
		{
			get { return language; }
			set { language = value; }
		}

		private string textDirection = "ltr";

		public string TextDirection
		{
			get { return textDirection; }
			set { textDirection = value; }
		}

		private bool enableBrowserSpellCheck = true;

		public bool EnableBrowserSpellCheck
		{
			get { return enableBrowserSpellCheck; }
			set { enableBrowserSpellCheck = value; }
		}

		private string editorBodyCssClass = "wysiwygeditor modulecontent art-postcontent";

		public string EditorBodyCssClass
		{
			get { return editorBodyCssClass; }
			set { editorBodyCssClass = value; }
		}

		private bool noWrap = false;

		public bool NoWrap
		{
			get { return noWrap; }
			set { inline = noWrap; }
		}

		private string removedMenuItems = "newdocument,print";

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

		private bool promptOnNavigationWithUnsavedChanges = false;

		public bool PromptOnNavigationWithUnsavedChanges
		{
			get { return promptOnNavigationWithUnsavedChanges; }
			set { promptOnNavigationWithUnsavedChanges = value; }
		}

	}

	public class TinyMceConfiguration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(TinyMceConfiguration));

		private TinyMceConfiguration()
		{ }

		private List<TinyMceSettings> editorDefinitions = new List<TinyMceSettings>();

		public TinyMceSettings GetEditorSettings(string name)
		{
			foreach (TinyMceSettings t in editorDefinitions)
			{
				if (t.Name == name) { return t; }
			}

			log.Error("could not load requested editor settings for editor definition " + name);
			return new TinyMceSettings();
		}

		private bool fullAutoFocus = true;
		public bool FullAutoFocus
		{
			get { return fullAutoFocus; }
		}

		private string fullMenubar = "edit insert view format table tools";

		public string FullMenubar
		{
			get { return fullMenubar; }
			set { fullMenubar = value; }
		}

		private bool fullToolbarDisableMenuBar = false;

		public bool FullToolbarDisableMenuBar
		{
			get { return fullToolbarDisableMenuBar; }
			set { fullToolbarDisableMenuBar = value; }
		}

		private string fullToolbarPlugins = "media,template,paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,wordcount,safari";
		public string FullToolbarPlugins
		{
			get { return fullToolbarPlugins; }
			set { fullToolbarPlugins = value; }
		}

		private string fullToolbarRow1Buttons = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
		public string FullToolbarRow1Buttons
		{
			get { return fullToolbarRow1Buttons; }
		}

		private string fullToolbarRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";
		public string FullToolbarRow2Buttons
		{
			get { return fullToolbarRow2Buttons; }
		}

		private string fullToolbarRow3Buttons = "formatselect,styleselect,separator,cleanup,fullscreen";
		public string FullToolbarRow3Buttons
		{
			get { return fullToolbarRow3Buttons; }
		}

		private bool fullToolbarForcePasteAsPlainText = false;
		public bool FullToolbarForcePasteAsPlainText
		{
			get { return fullToolbarForcePasteAsPlainText; }
		}

		private bool fullEnableObjectResizing = true;
		public bool FullEnableObjectResizing
		{
			get { return fullEnableObjectResizing; }
		}

		private int fullUnDoLevels = 10;
		public int FullUnDoLevels
		{
			get { return fullUnDoLevels; }
		}

		private string fullToolbarExtendedValidElements = "iframe[src|width|height|style|name|title|align|scrolling|frameborder|allowtransparency]";
		public string FullToolbarExtendedValidElements
		{
			get { return fullToolbarExtendedValidElements; }
		}

		private bool fullWithTemplatesAutoFocus = true;
		public bool FullWithTemplatesAutoFocus
		{
			get { return fullWithTemplatesAutoFocus; }
		}

		private string fullWithTemplatesMenubar = "edit insert view format table tools";

		public string FullWithTemplatesMenubar
		{
			get { return fullWithTemplatesMenubar; }
			set { fullWithTemplatesMenubar = value; }
		}

		private bool fullWithTemplatesDisableMenuBar = false;

		public bool FullWithTemplatesDisableMenuBar
		{
			get { return fullWithTemplatesDisableMenuBar; }
			set { fullWithTemplatesDisableMenuBar = value; }
		}

		private string fullWithTemplatesToolbarPlugins = "media,template,paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,wordcount,safari";
		public string FullWithTemplatesToolbarPlugins
		{
			get { return fullWithTemplatesToolbarPlugins; }
		}

		private string fullWithTemplatesToolbarRow1Buttons = "code,separator,selectall,removeformat,cleanup,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
		public string FullWithTemplatesToolbarRow1Buttons
		{
			get { return fullWithTemplatesToolbarRow1Buttons; }
		}

		private string fullWithTemplatesToolbarRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";
		public string FullWithTemplatesToolbarRow2Buttons
		{
			get { return fullWithTemplatesToolbarRow2Buttons; }
		}

		private string fullWithTemplatesToolbarRow3Buttons = "template,formatselect,styleselect,separator,cleanup,fullscreen";
		public string FullWithTemplatesToolbarRow3Buttons
		{
			get { return fullWithTemplatesToolbarRow3Buttons; }
		}

		private string fullWithTemplatesToolbarExtendedValidElements = "iframe[src|width|height|style|name|title|align|scrolling|frameborder|allowtransparency]";
		public string FullWithTemplatesToolbarExtendedValidElements
		{
			get { return fullWithTemplatesToolbarExtendedValidElements; }
		}

		private bool fullWithTemplatesToolbarForcePasteAsPlainText = false;
		public bool FullWithTemplatesToolbarForcePasteAsPlainText
		{
			get { return fullWithTemplatesToolbarForcePasteAsPlainText; }
		}

		private bool fullWithTemplatesEnableObjectResizing = true;
		public bool FullWithTemplatesEnableObjectResizing
		{
			get { return fullWithTemplatesEnableObjectResizing; }
		}

		private int fullWithTemplatesUnDoLevels = 10;
		public int FullWithTemplatesUnDoLevels
		{
			get { return fullWithTemplatesUnDoLevels; }
		}

		private bool forumWithImagesAutoFocus = true;
		public bool ForumWithImagesAutoFocus
		{
			get { return forumWithImagesAutoFocus; }
		}

		private string forumWithImagesMenubar = "";

		public string ForumWithImagesMenubar
		{
			get { return forumWithImagesMenubar; }
			set { forumWithImagesMenubar = value; }
		}

		private bool forumWithImagesDisableMenuBar = true;

		public bool ForumWithImagesDisableMenuBar
		{
			get { return forumWithImagesDisableMenuBar; }
			set { forumWithImagesDisableMenuBar = value; }
		}

		private string forumWithImagesToolbarPlugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";
		public string ForumWithImagesToolbarPlugins
		{
			get { return forumWithImagesToolbarPlugins; }
		}

		private string forumWithImagesToolbarRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";
		public string ForumWithImagesToolbarRow1Buttons
		{
			get { return forumWithImagesToolbarRow1Buttons; }
		}

		private string forumWithImagesToolbarRow2Buttons = string.Empty;
		public string ForumWithImagesToolbarRow2Buttons
		{
			get { return forumWithImagesToolbarRow2Buttons; }
		}

		private string forumWithImagesToolbarRow3Buttons = string.Empty;
		public string ForumWithImagesToolbarRow3Buttons
		{
			get { return forumWithImagesToolbarRow3Buttons; }
		}

		private string forumWithImagesToolbarExtendedValidElements = "";
		public string ForumWithImagesToolbarExtendedValidElements
		{
			get { return forumWithImagesToolbarExtendedValidElements; }
		}

		private bool forumWithImagesToolbarForcePasteAsPlainText = true;
		public bool ForumWithImagesToolbarForcePasteAsPlainText
		{
			get { return forumWithImagesToolbarForcePasteAsPlainText; }
		}

		private bool forumWithImagesEnableObjectResizing = true;
		public bool ForumWithImagesEnableObjectResizing
		{
			get { return forumWithImagesEnableObjectResizing; }
		}

		private int forumWithImagesUnDoLevels = 10;
		public int ForumWithImagesUnDoLevels
		{
			get { return forumWithImagesUnDoLevels; }
		}

		private bool forumAutoFocus = true;
		public bool ForumAutoFocus
		{
			get { return forumAutoFocus; }
		}

		private string forumMenubar = "";

		public string ForumMenubar
		{
			get { return forumMenubar; }
			set { forumMenubar = value; }
		}

		private bool forumDisableMenuBar = true;

		public bool ForumDisableMenuBar
		{
			get { return forumDisableMenuBar; }
			set { forumDisableMenuBar = value; }
		}

		private string forumToolbarPlugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";
		public string ForumToolbarPlugins
		{
			get { return forumToolbarPlugins; }
		}

		private string forumToolbarRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";
		public string ForumToolbarRow1Buttons
		{
			get { return forumToolbarRow1Buttons; }
		}

		private string forumToolbarRow2Buttons = string.Empty;
		public string ForumToolbarRow2Buttons
		{
			get { return forumToolbarRow2Buttons; }
		}

		private string forumToolbarRow3Buttons = string.Empty;
		public string ForumToolbarRow3Buttons
		{
			get { return forumToolbarRow3Buttons; }
		}

		private string forumToolbarExtendedValidElements = "";
		public string ForumToolbarExtendedValidElements
		{
			get { return forumToolbarExtendedValidElements; }
		}

		private bool forumToolbarForcePasteAsPlainText = true;
		public bool ForumToolbarForcePasteAsPlainText
		{
			get { return forumToolbarForcePasteAsPlainText; }
		}

		private bool forumEnableObjectResizing = true;
		public bool ForumEnableObjectResizing
		{
			get { return forumEnableObjectResizing; }
		}

		private int forumUnDoLevels = 10;
		public int ForumUnDoLevels
		{
			get { return forumUnDoLevels; }
		}

		private bool anonymousAutoFocus = false;
		public bool AnonymousAutoFocus
		{
			get { return anonymousAutoFocus; }
		}

		private string anonymousMenubar = "";

		public string AnonymousMenubar
		{
			get { return anonymousMenubar; }
			set { anonymousMenubar = value; }
		}

		private bool anonymousDisableMenuBar = true;

		public bool AnonymousDisableMenuBar
		{
			get { return anonymousDisableMenuBar; }
			set { anonymousDisableMenuBar = value; }
		}

		private string anonymousToolbarPlugins = "paste,emotions,directionality,inlinepopups";
		public string AnonymousToolbarPlugins
		{
			get { return anonymousToolbarPlugins; }
		}

		private string anonymousToolbarRow1Buttons = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";
		public string AnonymousToolbarRow1Buttons
		{
			get { return anonymousToolbarRow1Buttons; }
		}

		private string anonymousToolbarRow2Buttons = string.Empty;
		public string AnonymousToolbarRow2Buttons
		{
			get { return anonymousToolbarRow2Buttons; }
		}

		private string anonymousToolbarRow3Buttons = string.Empty;
		public string AnonymousToolbarRow3Buttons
		{
			get { return anonymousToolbarRow3Buttons; }
		}

		private string anonymousToolbarExtendedValidElements = "";
		public string AnonymousToolbarExtendedValidElements
		{
			get { return anonymousToolbarExtendedValidElements; }
		}

		private bool anonymousToolbarForcePasteAsPlainText = true;
		public bool AnonymousToolbarForcePasteAsPlainText
		{
			get { return anonymousToolbarForcePasteAsPlainText; }
		}

		private bool anonymousEnableObjectResizing = false;
		public bool AnonymousEnableObjectResizing
		{
			get { return anonymousEnableObjectResizing; }
		}

		private int anonymousUnDoLevels = 10;
		public int AnonymousUnDoLevels
		{
			get { return anonymousUnDoLevels; }
		}





		private string advancedFormatBlocks = "p,address,pre,h1,h2,h3,h4,h5,h6";
		public string AdvancedFormatBlocks
		{
			get { return advancedFormatBlocks; }
		}

		private string advancedSourceEditorWidth = "780";
		public string AdvancedSourceEditorWidth
		{
			get { return advancedSourceEditorWidth; }
		}

		private string advancedSourceEditorHeight = "700";
		public string AdvancedSourceEditorHeight
		{
			get { return advancedSourceEditorHeight; }
		}

		private string advancedToolbarLocation = "top";
		public string AdvancedToolbarLocation
		{
			get { return advancedToolbarLocation; }
		}

		private string advancedToolbarAlign = "left";
		public string AdvancedToolbarAlign
		{
			get { return advancedToolbarAlign; }
		}

		private string advancedStatusBarLocation = "bottom";
		public string AdvancedStatusBarLocation
		{
			get { return advancedStatusBarLocation; }
		}

		private bool accessibilityWarnings = true;
		public bool AccessibilityWarnings
		{
			get { return accessibilityWarnings; }
		}

		private string dialogType = "modal";
		public string DialogType
		{
			get { return dialogType; }
		}

		private bool enableObjectResizing = true;
		public bool EnableObjectResizing
		{
			get { return enableObjectResizing; }
		}

		private bool enableUndoRedo = true;
		public bool EnableUndoRedo
		{
			get { return enableUndoRedo; }
		}

		private int unDoLevels = 10;
		public int UnDoLevels
		{
			get { return unDoLevels; }
		}

		private bool cleanup = true;
		public bool Cleanup
		{
			get { return cleanup; }
		}

		private bool cleanupOnStart = false;
		public bool CleanupOnStart
		{
			get { return cleanupOnStart; }
		}

		private bool autoFocus = true;
		public bool AutoFocus
		{
			get { return autoFocus; }
		}


		private bool newsletterAutoFocus = false;
		public bool NewsletterAutoFocus
		{
			get { return newsletterAutoFocus; }
		}

		private string newsletterMenubar = "edit insert view format table tools";

		public string NewsletterMenubar
		{
			get { return newsletterMenubar; }
			set { newsletterMenubar = value; }
		}

		private bool newsletterDisableMenuBar = false;

		public bool NewsletterDisableMenuBar
		{
			get { return newsletterDisableMenuBar; }
			set { newsletterDisableMenuBar = value; }
		}

		private string newsletterToolbarPlugins = "fullpage,paste,print,searchreplace,fullscreen,emotions,directionality,table,contextmenu,advimage,inlinepopups,wordcount,safari";
		public string NewsletterToolbarPlugins
		{
			get { return newsletterToolbarPlugins; }
		}

		private string newsletterToolbarRow1Buttons = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
		public string NewsletterToolbarRow1Buttons
		{
			get { return newsletterToolbarRow1Buttons; }
		}

		private string newsletterToolbarRow2Buttons = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,charmap";
		public string NewsletterToolbarRow2Buttons
		{
			get { return newsletterToolbarRow2Buttons; }
		}

		private string newsletterToolbarRow3Buttons = "formatselect,fontselect,fontsizeselect,forecolorpicker,backcolorpicker,separator,cleanup,fullscreen,fullpage";
		public string NewsletterToolbarRow3Buttons
		{
			get { return newsletterToolbarRow3Buttons; }
		}

		private string newsletterToolbarExtendedValidElements = "";
		public string NewsletterToolbarExtendedValidElements
		{
			get { return newsletterToolbarExtendedValidElements; }
		}

		private bool newsletterToolbarForcePasteAsPlainText = false;
		public bool NewsletterToolbarForcePasteAsPlainText
		{
			get { return newsletterToolbarForcePasteAsPlainText; }
		}

		private bool newsletterEnableObjectResizing = false;
		public bool NewsletterEnableObjectResizing
		{
			get { return newsletterEnableObjectResizing; }
		}

		private int newsletterUnDoLevels = 10;
		public int NewsletterUnDoLevels
		{
			get { return newsletterUnDoLevels; }
		}

		public static TinyMceConfiguration GetConfig()
		{
			TinyMceConfiguration config = new TinyMceConfiguration();

			try
			{
				if (
					(HttpRuntime.Cache["mojoTinyConfiguration"] != null)
					&& (HttpRuntime.Cache["mojoTinyConfiguration"] is TinyMceConfiguration)
				)
				{
					return (TinyMceConfiguration)HttpRuntime.Cache["mojoTinyConfiguration"];
				}

				string pathToConfigFile = HostingEnvironment.MapPath("~/" + GetConfigFileName());

				XmlDocument configXml = new XmlDocument();
				configXml.Load(pathToConfigFile);

				if (WebConfigSettings.TinyMceUseV4)
				{
					config.LoadV4FromConfigurationXml(configXml.DocumentElement);
				}
				else
				{
					config.LoadValuesFromConfigurationXml(configXml.DocumentElement);
				}


				AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();

				string pathToWebConfig = HostingEnvironment.MapPath("~/Web.config");

				aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

				HttpRuntime.Cache.Insert(
					"mojoTinyConfiguration",
					config,
					aggregateCacheDependency,
					DateTime.Now.AddYears(1),
					TimeSpan.Zero,
					CacheItemPriority.Default,
					null
				);

				return (TinyMceConfiguration)HttpRuntime.Cache["mojoTinyConfiguration"];

			}

			catch (HttpException ex)
			{
				log.Error(ex);
			}
			catch (XmlException ex)
			{
				log.Error(ex);
			}
			catch (ArgumentException ex)
			{
				log.Error(ex);
			}
			catch (NullReferenceException ex)
			{
				log.Error(ex);
			}

			return config;
		}

		public void LoadV4FromConfigurationXml(XmlNode documentNode)
		{
			if (documentNode == null) { return; }

			foreach (XmlNode node in documentNode.ChildNodes)
			{
				if (node.Name == "Editor")
				{
					XmlAttributeCollection attributeCollection = node.Attributes;
					TinyMceSettings editor = new TinyMceSettings();

					if (attributeCollection["Name"] != null)
					{
						editor.Name = attributeCollection["Name"].Value;
					}


					if (attributeCollection["AutoFocus"] != null)
					{
						try
						{
							editor.AutoFocus = Convert.ToBoolean(attributeCollection["AutoFocus"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["PromptOnNavigationWithUnsavedChanges"] != null)
					{
						try
						{
							editor.PromptOnNavigationWithUnsavedChanges = Convert.ToBoolean(attributeCollection["PromptOnNavigationWithUnsavedChanges"].Value);
						}
						catch (FormatException) { }
					}




					if (attributeCollection["AutoLocalize"] != null)
					{
						try
						{
							editor.AutoLocalize = Convert.ToBoolean(attributeCollection["AutoLocalize"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["DisableMenuBar"] != null)
					{
						try
						{
							editor.DisableMenuBar = Convert.ToBoolean(attributeCollection["DisableMenuBar"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["UnDoLevels"] != null)
					{
						try
						{
							editor.UnDoLevels = Convert.ToInt32(attributeCollection["UnDoLevels"].Value);
						}
						catch (FormatException) { }
					}


					if (attributeCollection["Menubar"] != null)
					{
						editor.Menubar = attributeCollection["Menubar"].Value;
					}

					if (attributeCollection["Theme"] != null)
					{
						editor.Theme = attributeCollection["Theme"].Value;
					}

					if (attributeCollection["Skin"] != null)
					{
						editor.Skin = attributeCollection["Skin"].Value;
					}

					if (attributeCollection["EnableObjectResizing"] != null)
					{
						try
						{
							editor.EnableObjectResizing = Convert.ToBoolean(attributeCollection["EnableObjectResizing"].Value);
						}
						catch (FormatException) { }
					}



					if (attributeCollection["Plugins"] != null)
					{
						editor.Plugins = attributeCollection["Plugins"].Value;
					}

					if (attributeCollection["Toolbar1Buttons"] != null)
					{
						editor.Toolbar1Buttons = attributeCollection["Toolbar1Buttons"].Value;
					}

					if (attributeCollection["Toolbar2Buttons"] != null)
					{
						editor.Toolbar2Buttons = attributeCollection["Toolbar2Buttons"].Value;
					}

					if (attributeCollection["Toolbar3Buttons"] != null)
					{
						editor.Toolbar3Buttons = attributeCollection["Toolbar3Buttons"].Value;
					}

					if (attributeCollection["ExtendedValidElements"] != null)
					{
						editor.ExtendedValidElements = attributeCollection["ExtendedValidElements"].Value;
					}

					if (attributeCollection["ForcePasteAsPlainText"] != null)
					{
						try
						{
							editor.ForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["ForcePasteAsPlainText"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["ConvertUrls"] != null)
					{
						try
						{
							editor.ConvertUrls = Convert.ToBoolean(attributeCollection["ConvertUrls"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["Language"] != null)
					{
						editor.Language = attributeCollection["Language"].Value;
					}

					if (attributeCollection["TextDirection"] != null)
					{
						editor.TextDirection = attributeCollection["TextDirection"].Value;
					}

					if (attributeCollection["EnableBrowserSpellCheck"] != null)
					{
						try
						{
							editor.EnableBrowserSpellCheck = Convert.ToBoolean(attributeCollection["EnableBrowserSpellCheck"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["EditorBodyCssClass"] != null)
					{
						editor.EditorBodyCssClass = attributeCollection["EditorBodyCssClass"].Value;
					}

					if (attributeCollection["NoWrap"] != null)
					{
						try
						{
							editor.NoWrap = Convert.ToBoolean(attributeCollection["NoWrap"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["RemovedMenuItems"] != null)
					{
						editor.RemovedMenuItems = attributeCollection["RemovedMenuItems"].Value;
					}

					if (attributeCollection["FileDialogHeight"] != null)
					{
						try
						{
							editor.FileDialogHeight = Convert.ToInt32(attributeCollection["FileDialogHeight"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["FileDialogWidth"] != null)
					{
						try
						{
							editor.FileDialogWidth = Convert.ToInt32(attributeCollection["FileDialogWidth"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["EnableImageAdvancedTab"] != null)
					{
						try
						{
							editor.EnableImageAdvancedTab = Convert.ToBoolean(attributeCollection["EnableImageAdvancedTab"].Value);
						}
						catch (FormatException) { }
					}

					if (attributeCollection["ShowStatusbar"] != null)
					{
						try
						{
							editor.ShowStatusbar = Convert.ToBoolean(attributeCollection["ShowStatusbar"].Value);
						}
						catch (FormatException) { }
					}

					editorDefinitions.Add(editor);
				}
			}

		}

		public void LoadValuesFromConfigurationXml(XmlNode node)
		{
			if (node == null) { return; }

			XmlAttributeCollection attributeCollection = node.Attributes;

			if (attributeCollection["FullAutoFocus"] != null)
			{
				try
				{
					fullAutoFocus = Convert.ToBoolean(attributeCollection["FullAutoFocus"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullToolbarDisableMenuBar"] != null)
			{
				try
				{
					fullToolbarDisableMenuBar = Convert.ToBoolean(attributeCollection["FullToolbarDisableMenuBar"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullUnDoLevels"] != null)
			{
				try
				{
					fullUnDoLevels = Convert.ToInt32(attributeCollection["FullUnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullMenubar"] != null)
			{
				fullMenubar = attributeCollection["FullMenubar"].Value;
			}

			if (attributeCollection["FullEnableObjectResizing"] != null)
			{
				try
				{
					fullEnableObjectResizing = Convert.ToBoolean(attributeCollection["FullEnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullToolbarPlugins"] != null)
			{
				fullToolbarPlugins = attributeCollection["FullToolbarPlugins"].Value;
			}

			if (attributeCollection["FullToolbarRow1Buttons"] != null)
			{
				fullToolbarRow1Buttons = attributeCollection["FullToolbarRow1Buttons"].Value;
			}

			if (attributeCollection["FullToolbarRow2Buttons"] != null)
			{
				fullToolbarRow2Buttons = attributeCollection["FullToolbarRow2Buttons"].Value;
			}

			if (attributeCollection["FullToolbarRow3Buttons"] != null)
			{
				fullToolbarRow3Buttons = attributeCollection["FullToolbarRow3Buttons"].Value;
			}

			if (attributeCollection["FullToolbarExtendedValidElements"] != null)
			{
				fullToolbarExtendedValidElements = attributeCollection["FullToolbarExtendedValidElements"].Value;
			}

			if (attributeCollection["FullToolbarForcePasteAsPlainText"] != null)
			{
				try
				{
					fullToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["FullToolbarForcePasteAsPlainText"].Value);
				}
				catch (FormatException) { }
			}

			// full with templates

			#region FullWithTemplates

			if (attributeCollection["FullWithTemplatesAutoFocus"] != null)
			{
				try
				{
					fullWithTemplatesAutoFocus = Convert.ToBoolean(attributeCollection["FullWithTemplatesAutoFocus"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullWithTemplatesEnableObjectResizing"] != null)
			{
				try
				{
					fullWithTemplatesEnableObjectResizing = Convert.ToBoolean(attributeCollection["FullWithTemplatesEnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullWithTemplatesUnDoLevels"] != null)
			{
				try
				{
					fullWithTemplatesUnDoLevels = Convert.ToInt32(attributeCollection["FullWithTemplatesUnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullWithTemplatesMenubar"] != null)
			{
				fullWithTemplatesMenubar = attributeCollection["FullWithTemplatesMenubar"].Value;
			}

			if (attributeCollection["FullWithTemplatesDisableMenuBar"] != null)
			{
				try
				{
					fullWithTemplatesDisableMenuBar = Convert.ToBoolean(attributeCollection["FullWithTemplatesDisableMenuBar"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["FullWithTemplatesToolbarPlugins"] != null)
			{
				fullWithTemplatesToolbarPlugins = attributeCollection["FullWithTemplatesToolbarPlugins"].Value;
			}

			if (attributeCollection["FullWithTemplatesToolbarRow1Buttons"] != null)
			{
				fullWithTemplatesToolbarRow1Buttons = attributeCollection["FullWithTemplatesToolbarRow1Buttons"].Value;
			}

			if (attributeCollection["FullWithTemplatesToolbarRow2Buttons"] != null)
			{
				fullWithTemplatesToolbarRow2Buttons = attributeCollection["FullWithTemplatesToolbarRow2Buttons"].Value;
			}

			if (attributeCollection["FullWithTemplatesToolbarRow3Buttons"] != null)
			{
				fullWithTemplatesToolbarRow3Buttons = attributeCollection["FullWithTemplatesToolbarRow3Buttons"].Value;
			}

			if (attributeCollection["FullWithTemplatesToolbarExtendedValidElements"] != null)
			{
				fullWithTemplatesToolbarExtendedValidElements = attributeCollection["FullWithTemplatesToolbarExtendedValidElements"].Value;
			}

			if (attributeCollection["FullWithTemplatesToolbarForcePasteAsPlainText"] != null)
			{
				try
				{
					fullWithTemplatesToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["FullWithTemplatesToolbarForcePasteAsPlainText"].Value);
				}
				catch (FormatException) { }
			}

			#endregion

			// forum with images

			#region ForumWithImages

			if (attributeCollection["ForumWithImagesAutoFocus"] != null)
			{
				try
				{
					forumWithImagesAutoFocus = Convert.ToBoolean(attributeCollection["ForumWithImagesAutoFocus"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumWithImagesEnableObjectResizing"] != null)
			{
				try
				{
					forumWithImagesEnableObjectResizing = Convert.ToBoolean(attributeCollection["ForumWithImagesEnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumWithImagesUnDoLevels"] != null)
			{
				try
				{
					forumWithImagesUnDoLevels = Convert.ToInt32(attributeCollection["ForumWithImagesUnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumWithImagesMenubar"] != null)
			{
				forumWithImagesMenubar = attributeCollection["ForumWithImagesMenubar"].Value;
			}

			if (attributeCollection["ForumWithImagesDisableMenuBar"] != null)
			{
				try
				{
					forumWithImagesDisableMenuBar = Convert.ToBoolean(attributeCollection["ForumWithImagesDisableMenuBar"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumWithImagesToolbarPlugins"] != null)
			{
				forumWithImagesToolbarPlugins = attributeCollection["ForumWithImagesToolbarPlugins"].Value;
			}

			if (attributeCollection["ForumWithImagesToolbarRow1Buttons"] != null)
			{
				forumWithImagesToolbarRow1Buttons = attributeCollection["ForumWithImagesToolbarRow1Buttons"].Value;
			}

			if (attributeCollection["ForumWithImagesToolbarRow2Buttons"] != null)
			{
				forumWithImagesToolbarRow2Buttons = attributeCollection["ForumWithImagesToolbarRow2Buttons"].Value;
			}

			if (attributeCollection["ForumWithImagesToolbarRow3Buttons"] != null)
			{
				forumWithImagesToolbarRow3Buttons = attributeCollection["ForumWithImagesToolbarRow3Buttons"].Value;
			}

			if (attributeCollection["ForumWithImagesToolbarExtendedValidElements"] != null)
			{
				forumWithImagesToolbarExtendedValidElements = attributeCollection["ForumWithImagesToolbarExtendedValidElements"].Value;
			}

			if (attributeCollection["ForumWithImagesToolbarForcePasteAsPlainText"] != null)
			{
				try
				{
					forumWithImagesToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["ForumWithImagesToolbarForcePasteAsPlainText"].Value);
				}
				catch (FormatException) { }
			}

			#endregion

			// forum

			#region Forum

			if (attributeCollection["ForumAutoFocus"] != null)
			{
				try
				{
					forumAutoFocus = Convert.ToBoolean(attributeCollection["ForumAutoFocus"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumEnableObjectResizing"] != null)
			{
				try
				{
					forumEnableObjectResizing = Convert.ToBoolean(attributeCollection["ForumEnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumUnDoLevels"] != null)
			{
				try
				{
					forumUnDoLevels = Convert.ToInt32(attributeCollection["ForumUnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumMenubar"] != null)
			{
				forumMenubar = attributeCollection["ForumMenubar"].Value;
			}


			if (attributeCollection["ForumDisableMenuBar"] != null)
			{
				try
				{
					forumDisableMenuBar = Convert.ToBoolean(attributeCollection["ForumDisableMenuBar"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["ForumToolbarPlugins"] != null)
			{
				forumToolbarPlugins = attributeCollection["ForumToolbarPlugins"].Value;
			}

			if (attributeCollection["ForumToolbarRow1Buttons"] != null)
			{
				forumToolbarRow1Buttons = attributeCollection["ForumToolbarRow1Buttons"].Value;
			}

			if (attributeCollection["ForumToolbarRow2Buttons"] != null)
			{
				forumToolbarRow2Buttons = attributeCollection["ForumToolbarRow2Buttons"].Value;
			}

			if (attributeCollection["ForumToolbarRow3Buttons"] != null)
			{
				forumToolbarRow3Buttons = attributeCollection["ForumToolbarRow3Buttons"].Value;
			}

			if (attributeCollection["ForumToolbarExtendedValidElements"] != null)
			{
				forumToolbarExtendedValidElements = attributeCollection["ForumToolbarExtendedValidElements"].Value;
			}

			if (attributeCollection["ForumToolbarForcePasteAsPlainText"] != null)
			{
				try
				{
					forumToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["ForumToolbarForcePasteAsPlainText"].Value);
				}
				catch (FormatException) { }
			}

			#endregion

			// anonymous

			#region Anonymous

			if (attributeCollection["AnonymousMenubar"] != null)
			{
				anonymousMenubar = attributeCollection["AnonymousMenubar"].Value;
			}

			if (attributeCollection["AnonymousAutoFocus"] != null)
			{
				try
				{
					anonymousAutoFocus = Convert.ToBoolean(attributeCollection["AnonymousAutoFocus"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["AnonymousEnableObjectResizing"] != null)
			{
				try
				{
					anonymousEnableObjectResizing = Convert.ToBoolean(attributeCollection["AnonymousEnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["AnonymousUnDoLevels"] != null)
			{
				try
				{
					anonymousUnDoLevels = Convert.ToInt32(attributeCollection["AnonymousUnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["AnonymousDisableMenuBar"] != null)
			{
				try
				{
					anonymousDisableMenuBar = Convert.ToBoolean(attributeCollection["AnonymousDisableMenuBar"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["AnonymousToolbarPlugins"] != null)
			{
				anonymousToolbarPlugins = attributeCollection["AnonymousToolbarPlugins"].Value;
			}

			if (attributeCollection["AnonymousToolbarRow1Buttons"] != null)
			{
				anonymousToolbarRow1Buttons = attributeCollection["AnonymousToolbarRow1Buttons"].Value;
			}

			if (attributeCollection["AnonymousToolbarRow2Buttons"] != null)
			{
				anonymousToolbarRow2Buttons = attributeCollection["AnonymousToolbarRow2Buttons"].Value;
			}

			if (attributeCollection["AnonymousToolbarRow3Buttons"] != null)
			{
				anonymousToolbarRow3Buttons = attributeCollection["AnonymousToolbarRow3Buttons"].Value;
			}

			if (attributeCollection["AnonymousToolbarExtendedValidElements"] != null)
			{
				anonymousToolbarExtendedValidElements = attributeCollection["AnonymousToolbarExtendedValidElements"].Value;
			}

			if (attributeCollection["AnonymousToolbarForcePasteAsPlainText"] != null)
			{
				try
				{
					anonymousToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["AnonymousToolbarForcePasteAsPlainText"].Value);
				}
				catch (FormatException) { }
			}

			#endregion

			#region Newsletter

			if (attributeCollection["NewsletterAutoFocus"] != null)
			{
				try
				{
					newsletterAutoFocus = Convert.ToBoolean(attributeCollection["NewsletterAutoFocus"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["NewsletterEnableObjectResizing"] != null)
			{
				try
				{
					newsletterEnableObjectResizing = Convert.ToBoolean(attributeCollection["NewsletterEnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["NewsletterUnDoLevels"] != null)
			{
				try
				{
					newsletterUnDoLevels = Convert.ToInt32(attributeCollection["NewsletterUnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["NewsletterMenubar"] != null)
			{
				newsletterMenubar = attributeCollection["NewsletterMenubar"].Value;
			}

			if (attributeCollection["NewsletterDisableMenuBar"] != null)
			{
				try
				{
					newsletterDisableMenuBar = Convert.ToBoolean(attributeCollection["NewsletterDisableMenuBar"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["NewsletterToolbarForcePasteAsPlainText"] != null)
			{
				try
				{
					newsletterToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["NewsletterToolbarForcePasteAsPlainText"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["NewsletterToolbarPlugins"] != null)
			{
				newsletterToolbarPlugins = attributeCollection["NewsletterToolbarPlugins"].Value;
			}

			if (attributeCollection["NewsletterToolbarRow1Buttons"] != null)
			{
				newsletterToolbarRow1Buttons = attributeCollection["NewsletterToolbarRow1Buttons"].Value;
			}

			if (attributeCollection["NewsletterToolbarRow2Buttons"] != null)
			{
				newsletterToolbarRow2Buttons = attributeCollection["NewsletterToolbarRow2Buttons"].Value;
			}

			if (attributeCollection["NewsletterToolbarRow3Buttons"] != null)
			{
				newsletterToolbarRow3Buttons = attributeCollection["NewsletterToolbarRow3Buttons"].Value;
			}

			if (attributeCollection["NewsletterToolbarExtendedValidElements"] != null)
			{
				newsletterToolbarExtendedValidElements = attributeCollection["NewsletterToolbarExtendedValidElements"].Value;
			}

			#endregion

			// legacy

			if (attributeCollection["EnableObjectResizing"] != null)
			{
				try
				{
					enableObjectResizing = Convert.ToBoolean(attributeCollection["EnableObjectResizing"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["EnableUndoRedo"] != null)
			{
				try
				{
					enableUndoRedo = Convert.ToBoolean(attributeCollection["EnableUndoRedo"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["UnDoLevels"] != null)
			{
				try
				{
					unDoLevels = Convert.ToInt32(attributeCollection["UnDoLevels"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["Cleanup"] != null)
			{
				try
				{
					cleanup = Convert.ToBoolean(attributeCollection["Cleanup"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["CleanupOnStart"] != null)
			{
				try
				{
					cleanupOnStart = Convert.ToBoolean(attributeCollection["CleanupOnStart"].Value);
				}
				catch (FormatException) { }
			}

			if (attributeCollection["AutoFocus"] != null)
			{
				try
				{
					autoFocus = Convert.ToBoolean(attributeCollection["AutoFocus"].Value);
				}
				catch (FormatException) { }
			}


			if (attributeCollection["AdvancedFormatBlocks"] != null)
			{
				advancedFormatBlocks = attributeCollection["AdvancedFormatBlocks"].Value;
			}

			if (attributeCollection["AdvancedSourceEditorWidth"] != null)
			{
				advancedSourceEditorWidth = attributeCollection["AdvancedSourceEditorWidth"].Value;
			}

			if (attributeCollection["AdvancedSourceEditorHeight"] != null)
			{
				advancedSourceEditorHeight = attributeCollection["AdvancedSourceEditorHeight"].Value;
			}

			if (attributeCollection["AdvancedToolbarLocation"] != null)
			{
				advancedToolbarLocation = attributeCollection["AdvancedToolbarLocation"].Value;
			}

			if (attributeCollection["AdvancedToolbarAlign"] != null)
			{
				advancedToolbarAlign = attributeCollection["AdvancedToolbarAlign"].Value;
			}

			if (attributeCollection["AdvancedStatusBarLocation"] != null)
			{
				advancedStatusBarLocation = attributeCollection["AdvancedStatusBarLocation"].Value;
			}

			if (attributeCollection["AccessibilityWarnings"] != null)
			{
				accessibilityWarnings = Convert.ToBoolean(attributeCollection["AccessibilityWarnings"].Value);
			}

			if (attributeCollection["DialogType"] != null)
			{
				dialogType = attributeCollection["DialogType"].Value;
			}
		}

		private static string GetConfigFileName()
		{
			string configFileName = "mojoTinyMCE.config";

			if (ConfigurationManager.AppSettings["TinyMCE:ConfigFile"] != null)
			{
				configFileName = ConfigurationManager.AppSettings["TinyMCE:ConfigFile"];
			}

			return configFileName;
		}
	}
}