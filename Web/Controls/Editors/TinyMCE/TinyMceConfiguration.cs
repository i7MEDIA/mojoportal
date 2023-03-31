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
		string ImagesUploadUrl { get; set; }
	}

	public class TinyMceSettings : ITinyMceSettings
	{
		public TinyMceSettings() { }

		public TinyMceSettings Clone()
		{
			return this.MemberwiseClone() as TinyMceSettings;
		}

		#region NonConfig Settings
		// these are not populated from the config file but can be set programatically

		public bool Inline { get; set; } = false;
		public string OnSaveCallback { get; set; } = string.Empty;
		public bool SaveEnableWhenDirty { get; set; } = true;
		public string CustomToolbarElementClientId { get; set; } = string.Empty;
		public string EditorAreaCSS { get; set; } = string.Empty;
		public string TemplatesUrl { get; set; } = string.Empty;
		public string StyleFormats { get; set; } = string.Empty;
		public string EmotionsBaseUrl { get; set; } = string.Empty;
		public string FileManagerUrl { get; set; } = string.Empty;
		public string ImagesUploadUrl { get; set; } = string.Empty;
		public string GlobarVarToAssignEditor { get; set; } = string.Empty;

		#endregion


		public string Name { get; set; } = "NotFound";
		public bool AutoFocus { get; set; } = false;
		public string Menubar { get; set; } = string.Empty;
		public bool DisableMenuBar { get; set; } = true;
		public string Plugins { get; set; } = "emotions,directionality,inlinepopups";
		public string Toolbar1Buttons { get; set; } = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";
		public string Toolbar2Buttons { get; set; } = string.Empty;
		public string Toolbar3Buttons { get; set; } = string.Empty;
		public string ExtendedValidElements { get; set; } = string.Empty;
		public bool ForcePasteAsPlainText { get; set; } = true;
		public bool ConvertUrls { get; set; } = false;
		public bool EnableObjectResizing { get; set; } = false;
		public int UnDoLevels { get; set; } = 10;
		public string Theme { get; set; } = "modern";
		public string Skin { get; set; } = "lightgray";
		public bool AutoLocalize { get; set; } = true;
		public string Language { get; set; } = "en";
		public string TextDirection { get; set; } = "ltr";
		public bool EnableBrowserSpellCheck { get; set; } = true;
		public string EditorBodyCssClass { get; set; } = "wysiwygeditor modulecontent art-postcontent";
		private bool noWrap = false;
		public bool NoWrap
		{
			get { return noWrap; }
			set { Inline = noWrap; }
		}
		public string RemovedMenuItems { get; set; } = "newdocument,print";
		public int FileDialogHeight { get; set; } = 700;
		public int FileDialogWidth { get; set; } = 860;
		public bool EnableImageAdvancedTab { get; set; } = false;
		public bool ShowStatusbar { get; set; } = true;
		public bool PromptOnNavigationWithUnsavedChanges { get; set; } = false;
	}

	public class TinyMceConfiguration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(TinyMceConfiguration));

		private TinyMceConfiguration() { }

		private List<TinyMceSettings> editorDefinitions = new();

		public TinyMceSettings GetEditorSettings(string name)
		{
			foreach (TinyMceSettings t in editorDefinitions)
			{
				if (t.Name == name) { return t; }
			}

			log.Error("could not load requested editor settings for editor definition " + name);
			return new TinyMceSettings();
		}

		public bool FullAutoFocus { get; private set; } = true;
		public string FullMenubar { get; set; } = "edit insert view format table tools";
		public bool FullToolbarDisableMenuBar { get; set; } = false;
		public string FullToolbarPlugins { get; set; } = "media,template,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,wordcount,safari";
		public string FullToolbarRow1Buttons { get; private set; } = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
		public string FullToolbarRow2Buttons { get; private set; } = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";
		public string FullToolbarRow3Buttons { get; private set; } = "formatselect,styleselect,separator,cleanup,fullscreen";
		public bool FullToolbarForcePasteAsPlainText { get; private set; } = false;
		public bool FullEnableObjectResizing { get; private set; } = true;
		public int FullUnDoLevels { get; private set; } = 10;
		public string FullToolbarExtendedValidElements { get; private set; } = "iframe[src|width|height|style|name|title|align|scrolling|frameborder|allowtransparency]";
		public bool FullWithTemplatesAutoFocus { get; private set; } = true;
		public string FullWithTemplatesMenubar { get; set; } = "edit insert view format table tools";
		public bool FullWithTemplatesDisableMenuBar { get; set; } = false;
		public string FullWithTemplatesToolbarPlugins { get; private set; } = "media,template,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups,wordcount,safari";
		public string FullWithTemplatesToolbarRow1Buttons { get; private set; } = "code,separator,selectall,removeformat,cleanup,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
		public string FullWithTemplatesToolbarRow2Buttons { get; private set; } = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,emotions,charmap";
		public string FullWithTemplatesToolbarRow3Buttons { get; private set; } = "template,formatselect,styleselect,separator,cleanup,fullscreen";
		public string FullWithTemplatesToolbarExtendedValidElements { get; private set; } = "iframe[src|width|height|style|name|title|align|scrolling|frameborder|allowtransparency]";
		public bool FullWithTemplatesToolbarForcePasteAsPlainText { get; private set; } = false;
		public bool FullWithTemplatesEnableObjectResizing { get; private set; } = true;
		public int FullWithTemplatesUnDoLevels { get; private set; } = 10;
		public bool ForumWithImagesAutoFocus { get; private set; } = true;
		public string ForumWithImagesMenubar { get; set; } = "";
		public bool ForumWithImagesDisableMenuBar { get; set; } = true;
		public string ForumWithImagesToolbarPlugins { get; private set; } = "searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";
		public string ForumWithImagesToolbarRow1Buttons { get; private set; } = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";
		public string ForumWithImagesToolbarRow2Buttons { get; private set; } = string.Empty;
		public string ForumWithImagesToolbarRow3Buttons { get; private set; } = string.Empty;
		public string ForumWithImagesToolbarExtendedValidElements { get; private set; } = "";
		public bool ForumWithImagesToolbarForcePasteAsPlainText { get; private set; } = true;
		public bool ForumWithImagesEnableObjectResizing { get; private set; } = true;
		public int ForumWithImagesUnDoLevels { get; private set; } = 10;
		public bool ForumAutoFocus { get; private set; } = true;
		public string ForumMenubar { get; set; } = "";
		public bool ForumDisableMenuBar { get; set; } = true;
		public string ForumToolbarPlugins { get; private set; } = "searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";
		public string ForumToolbarRow1Buttons { get; private set; } = "cut,copy,pastetext,separator,blockquote,bold,italic,underline,separator,bullist,numlist,separator,link,unlink,separator,charmap,emotions";
		public string ForumToolbarRow2Buttons { get; private set; } = string.Empty;
		public string ForumToolbarRow3Buttons { get; private set; } = string.Empty;
		public string ForumToolbarExtendedValidElements { get; private set; } = "";
		public bool ForumToolbarForcePasteAsPlainText { get; private set; } = true;
		public bool ForumEnableObjectResizing { get; private set; } = true;
		public int ForumUnDoLevels { get; private set; } = 10;
		public bool AnonymousAutoFocus { get; private set; } = false;
		public string AnonymousMenubar { get; set; } = "";
		public bool AnonymousDisableMenuBar { get; set; } = true;
		public string AnonymousToolbarPlugins { get; private set; } = "emotions,directionality,inlinepopups";
		public string AnonymousToolbarRow1Buttons { get; private set; } = "cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";
		public string AnonymousToolbarRow2Buttons { get; private set; } = string.Empty;
		public string AnonymousToolbarRow3Buttons { get; private set; } = string.Empty;
		public string AnonymousToolbarExtendedValidElements { get; private set; } = "";
		public bool AnonymousToolbarForcePasteAsPlainText { get; private set; } = true;
		public bool AnonymousEnableObjectResizing { get; private set; } = false;
		public int AnonymousUnDoLevels { get; private set; } = 10;
		public string AdvancedFormatBlocks { get; private set; } = "p,address,pre,h1,h2,h3,h4,h5,h6";
		public string AdvancedSourceEditorWidth { get; private set; } = "780";
		public string AdvancedSourceEditorHeight { get; private set; } = "700";
		public string AdvancedToolbarLocation { get; private set; } = "top";
		public string AdvancedToolbarAlign { get; private set; } = "left";
		public string AdvancedStatusBarLocation { get; private set; } = "bottom";
		public bool AccessibilityWarnings { get; private set; } = true;
		public string DialogType { get; private set; } = "modal";
		public bool EnableObjectResizing { get; private set; } = true;
		public bool EnableUndoRedo { get; private set; } = true;
		public int UnDoLevels { get; private set; } = 10;
		public bool Cleanup { get; private set; } = true;
		public bool CleanupOnStart { get; private set; } = false;
		public bool AutoFocus { get; private set; } = true;
		public bool NewsletterAutoFocus { get; private set; } = false;
		public string NewsletterMenubar { get; set; } = "edit insert view format table tools";
		public bool NewsletterDisableMenuBar { get; set; } = false;
		public string NewsletterToolbarPlugins { get; private set; } = "fullpage,searchreplace,fullscreen,emotions,directionality,table,contextmenu,advimage,inlinepopups,wordcount,safari";
		public string NewsletterToolbarRow1Buttons { get; private set; } = "code,separator,selectall,removeformat,cut,copy,separator,paste,pastetext,pasteword,separator,print,separator,undo,redo,separator,search,replace";
		public string NewsletterToolbarRow2Buttons { get; private set; } = "blockquote,bold,italic,underline,strikethrough,separator,sub,sup,separator,bullist,numlist,separator,outdent,indent,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,link,unlink,anchor,image,media,table,hr,charmap";
		public string NewsletterToolbarRow3Buttons { get; private set; } = "formatselect,fontselect,fontsizeselect,forecolorpicker,backcolorpicker,separator,cleanup,fullscreen,fullpage";
		public string NewsletterToolbarExtendedValidElements { get; private set; } = "";
		public bool NewsletterToolbarForcePasteAsPlainText { get; private set; } = false;
		public bool NewsletterEnableObjectResizing { get; private set; } = false;
		public int NewsletterUnDoLevels { get; private set; } = 10;

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

				var configXml = Core.Helpers.XmlHelper.GetXmlDocument(pathToConfigFile);

				config.LoadConfigXml(configXml.DocumentElement);


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

		public void LoadConfigXml(XmlNode documentNode)
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

		//public void LoadValuesFromConfigurationXml(XmlNode node)
		//{
		//	if (node == null) { return; }

		//	XmlAttributeCollection attributeCollection = node.Attributes;

		//	if (attributeCollection["FullAutoFocus"] != null)
		//	{
		//		try
		//		{
		//			FullAutoFocus = Convert.ToBoolean(attributeCollection["FullAutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullToolbarDisableMenuBar"] != null)
		//	{
		//		try
		//		{
		//			FullToolbarDisableMenuBar = Convert.ToBoolean(attributeCollection["FullToolbarDisableMenuBar"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullUnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			FullUnDoLevels = Convert.ToInt32(attributeCollection["FullUnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullMenubar"] != null)
		//	{
		//		FullMenubar = attributeCollection["FullMenubar"].Value;
		//	}

		//	if (attributeCollection["FullEnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			FullEnableObjectResizing = Convert.ToBoolean(attributeCollection["FullEnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullToolbarPlugins"] != null)
		//	{
		//		FullToolbarPlugins = attributeCollection["FullToolbarPlugins"].Value;
		//	}

		//	if (attributeCollection["FullToolbarRow1Buttons"] != null)
		//	{
		//		FullToolbarRow1Buttons = attributeCollection["FullToolbarRow1Buttons"].Value;
		//	}

		//	if (attributeCollection["FullToolbarRow2Buttons"] != null)
		//	{
		//		FullToolbarRow2Buttons = attributeCollection["FullToolbarRow2Buttons"].Value;
		//	}

		//	if (attributeCollection["FullToolbarRow3Buttons"] != null)
		//	{
		//		FullToolbarRow3Buttons = attributeCollection["FullToolbarRow3Buttons"].Value;
		//	}

		//	if (attributeCollection["FullToolbarExtendedValidElements"] != null)
		//	{
		//		FullToolbarExtendedValidElements = attributeCollection["FullToolbarExtendedValidElements"].Value;
		//	}

		//	if (attributeCollection["FullToolbarForcePasteAsPlainText"] != null)
		//	{
		//		try
		//		{
		//			FullToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["FullToolbarForcePasteAsPlainText"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	// full with templates

		//	#region FullWithTemplates

		//	if (attributeCollection["FullWithTemplatesAutoFocus"] != null)
		//	{
		//		try
		//		{
		//			FullWithTemplatesAutoFocus = Convert.ToBoolean(attributeCollection["FullWithTemplatesAutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullWithTemplatesEnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			FullWithTemplatesEnableObjectResizing = Convert.ToBoolean(attributeCollection["FullWithTemplatesEnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullWithTemplatesUnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			FullWithTemplatesUnDoLevels = Convert.ToInt32(attributeCollection["FullWithTemplatesUnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullWithTemplatesMenubar"] != null)
		//	{
		//		FullWithTemplatesMenubar = attributeCollection["FullWithTemplatesMenubar"].Value;
		//	}

		//	if (attributeCollection["FullWithTemplatesDisableMenuBar"] != null)
		//	{
		//		try
		//		{
		//			FullWithTemplatesDisableMenuBar = Convert.ToBoolean(attributeCollection["FullWithTemplatesDisableMenuBar"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["FullWithTemplatesToolbarPlugins"] != null)
		//	{
		//		FullWithTemplatesToolbarPlugins = attributeCollection["FullWithTemplatesToolbarPlugins"].Value;
		//	}

		//	if (attributeCollection["FullWithTemplatesToolbarRow1Buttons"] != null)
		//	{
		//		FullWithTemplatesToolbarRow1Buttons = attributeCollection["FullWithTemplatesToolbarRow1Buttons"].Value;
		//	}

		//	if (attributeCollection["FullWithTemplatesToolbarRow2Buttons"] != null)
		//	{
		//		FullWithTemplatesToolbarRow2Buttons = attributeCollection["FullWithTemplatesToolbarRow2Buttons"].Value;
		//	}

		//	if (attributeCollection["FullWithTemplatesToolbarRow3Buttons"] != null)
		//	{
		//		FullWithTemplatesToolbarRow3Buttons = attributeCollection["FullWithTemplatesToolbarRow3Buttons"].Value;
		//	}

		//	if (attributeCollection["FullWithTemplatesToolbarExtendedValidElements"] != null)
		//	{
		//		FullWithTemplatesToolbarExtendedValidElements = attributeCollection["FullWithTemplatesToolbarExtendedValidElements"].Value;
		//	}

		//	if (attributeCollection["FullWithTemplatesToolbarForcePasteAsPlainText"] != null)
		//	{
		//		try
		//		{
		//			FullWithTemplatesToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["FullWithTemplatesToolbarForcePasteAsPlainText"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	#endregion

		//	// forum with images

		//	#region ForumWithImages

		//	if (attributeCollection["ForumWithImagesAutoFocus"] != null)
		//	{
		//		try
		//		{
		//			ForumWithImagesAutoFocus = Convert.ToBoolean(attributeCollection["ForumWithImagesAutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumWithImagesEnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			ForumWithImagesEnableObjectResizing = Convert.ToBoolean(attributeCollection["ForumWithImagesEnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumWithImagesUnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			ForumWithImagesUnDoLevels = Convert.ToInt32(attributeCollection["ForumWithImagesUnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumWithImagesMenubar"] != null)
		//	{
		//		ForumWithImagesMenubar = attributeCollection["ForumWithImagesMenubar"].Value;
		//	}

		//	if (attributeCollection["ForumWithImagesDisableMenuBar"] != null)
		//	{
		//		try
		//		{
		//			ForumWithImagesDisableMenuBar = Convert.ToBoolean(attributeCollection["ForumWithImagesDisableMenuBar"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumWithImagesToolbarPlugins"] != null)
		//	{
		//		ForumWithImagesToolbarPlugins = attributeCollection["ForumWithImagesToolbarPlugins"].Value;
		//	}

		//	if (attributeCollection["ForumWithImagesToolbarRow1Buttons"] != null)
		//	{
		//		ForumWithImagesToolbarRow1Buttons = attributeCollection["ForumWithImagesToolbarRow1Buttons"].Value;
		//	}

		//	if (attributeCollection["ForumWithImagesToolbarRow2Buttons"] != null)
		//	{
		//		ForumWithImagesToolbarRow2Buttons = attributeCollection["ForumWithImagesToolbarRow2Buttons"].Value;
		//	}

		//	if (attributeCollection["ForumWithImagesToolbarRow3Buttons"] != null)
		//	{
		//		ForumWithImagesToolbarRow3Buttons = attributeCollection["ForumWithImagesToolbarRow3Buttons"].Value;
		//	}

		//	if (attributeCollection["ForumWithImagesToolbarExtendedValidElements"] != null)
		//	{
		//		ForumWithImagesToolbarExtendedValidElements = attributeCollection["ForumWithImagesToolbarExtendedValidElements"].Value;
		//	}

		//	if (attributeCollection["ForumWithImagesToolbarForcePasteAsPlainText"] != null)
		//	{
		//		try
		//		{
		//			ForumWithImagesToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["ForumWithImagesToolbarForcePasteAsPlainText"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	#endregion

		//	// forum

		//	#region Forum

		//	if (attributeCollection["ForumAutoFocus"] != null)
		//	{
		//		try
		//		{
		//			ForumAutoFocus = Convert.ToBoolean(attributeCollection["ForumAutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumEnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			ForumEnableObjectResizing = Convert.ToBoolean(attributeCollection["ForumEnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumUnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			ForumUnDoLevels = Convert.ToInt32(attributeCollection["ForumUnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumMenubar"] != null)
		//	{
		//		ForumMenubar = attributeCollection["ForumMenubar"].Value;
		//	}


		//	if (attributeCollection["ForumDisableMenuBar"] != null)
		//	{
		//		try
		//		{
		//			ForumDisableMenuBar = Convert.ToBoolean(attributeCollection["ForumDisableMenuBar"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["ForumToolbarPlugins"] != null)
		//	{
		//		ForumToolbarPlugins = attributeCollection["ForumToolbarPlugins"].Value;
		//	}

		//	if (attributeCollection["ForumToolbarRow1Buttons"] != null)
		//	{
		//		ForumToolbarRow1Buttons = attributeCollection["ForumToolbarRow1Buttons"].Value;
		//	}

		//	if (attributeCollection["ForumToolbarRow2Buttons"] != null)
		//	{
		//		ForumToolbarRow2Buttons = attributeCollection["ForumToolbarRow2Buttons"].Value;
		//	}

		//	if (attributeCollection["ForumToolbarRow3Buttons"] != null)
		//	{
		//		ForumToolbarRow3Buttons = attributeCollection["ForumToolbarRow3Buttons"].Value;
		//	}

		//	if (attributeCollection["ForumToolbarExtendedValidElements"] != null)
		//	{
		//		ForumToolbarExtendedValidElements = attributeCollection["ForumToolbarExtendedValidElements"].Value;
		//	}

		//	if (attributeCollection["ForumToolbarForcePasteAsPlainText"] != null)
		//	{
		//		try
		//		{
		//			ForumToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["ForumToolbarForcePasteAsPlainText"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	#endregion

		//	// anonymous

		//	#region Anonymous

		//	if (attributeCollection["AnonymousMenubar"] != null)
		//	{
		//		AnonymousMenubar = attributeCollection["AnonymousMenubar"].Value;
		//	}

		//	if (attributeCollection["AnonymousAutoFocus"] != null)
		//	{
		//		try
		//		{
		//			AnonymousAutoFocus = Convert.ToBoolean(attributeCollection["AnonymousAutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["AnonymousEnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			AnonymousEnableObjectResizing = Convert.ToBoolean(attributeCollection["AnonymousEnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["AnonymousUnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			AnonymousUnDoLevels = Convert.ToInt32(attributeCollection["AnonymousUnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["AnonymousDisableMenuBar"] != null)
		//	{
		//		try
		//		{
		//			AnonymousDisableMenuBar = Convert.ToBoolean(attributeCollection["AnonymousDisableMenuBar"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["AnonymousToolbarPlugins"] != null)
		//	{
		//		AnonymousToolbarPlugins = attributeCollection["AnonymousToolbarPlugins"].Value;
		//	}

		//	if (attributeCollection["AnonymousToolbarRow1Buttons"] != null)
		//	{
		//		AnonymousToolbarRow1Buttons = attributeCollection["AnonymousToolbarRow1Buttons"].Value;
		//	}

		//	if (attributeCollection["AnonymousToolbarRow2Buttons"] != null)
		//	{
		//		AnonymousToolbarRow2Buttons = attributeCollection["AnonymousToolbarRow2Buttons"].Value;
		//	}

		//	if (attributeCollection["AnonymousToolbarRow3Buttons"] != null)
		//	{
		//		AnonymousToolbarRow3Buttons = attributeCollection["AnonymousToolbarRow3Buttons"].Value;
		//	}

		//	if (attributeCollection["AnonymousToolbarExtendedValidElements"] != null)
		//	{
		//		AnonymousToolbarExtendedValidElements = attributeCollection["AnonymousToolbarExtendedValidElements"].Value;
		//	}

		//	if (attributeCollection["AnonymousToolbarForcePasteAsPlainText"] != null)
		//	{
		//		try
		//		{
		//			AnonymousToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["AnonymousToolbarForcePasteAsPlainText"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	#endregion

		//	#region Newsletter

		//	if (attributeCollection["NewsletterAutoFocus"] != null)
		//	{
		//		try
		//		{
		//			NewsletterAutoFocus = Convert.ToBoolean(attributeCollection["NewsletterAutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["NewsletterEnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			NewsletterEnableObjectResizing = Convert.ToBoolean(attributeCollection["NewsletterEnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["NewsletterUnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			NewsletterUnDoLevels = Convert.ToInt32(attributeCollection["NewsletterUnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["NewsletterMenubar"] != null)
		//	{
		//		NewsletterMenubar = attributeCollection["NewsletterMenubar"].Value;
		//	}

		//	if (attributeCollection["NewsletterDisableMenuBar"] != null)
		//	{
		//		try
		//		{
		//			NewsletterDisableMenuBar = Convert.ToBoolean(attributeCollection["NewsletterDisableMenuBar"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["NewsletterToolbarForcePasteAsPlainText"] != null)
		//	{
		//		try
		//		{
		//			NewsletterToolbarForcePasteAsPlainText = Convert.ToBoolean(attributeCollection["NewsletterToolbarForcePasteAsPlainText"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["NewsletterToolbarPlugins"] != null)
		//	{
		//		NewsletterToolbarPlugins = attributeCollection["NewsletterToolbarPlugins"].Value;
		//	}

		//	if (attributeCollection["NewsletterToolbarRow1Buttons"] != null)
		//	{
		//		NewsletterToolbarRow1Buttons = attributeCollection["NewsletterToolbarRow1Buttons"].Value;
		//	}

		//	if (attributeCollection["NewsletterToolbarRow2Buttons"] != null)
		//	{
		//		NewsletterToolbarRow2Buttons = attributeCollection["NewsletterToolbarRow2Buttons"].Value;
		//	}

		//	if (attributeCollection["NewsletterToolbarRow3Buttons"] != null)
		//	{
		//		NewsletterToolbarRow3Buttons = attributeCollection["NewsletterToolbarRow3Buttons"].Value;
		//	}

		//	if (attributeCollection["NewsletterToolbarExtendedValidElements"] != null)
		//	{
		//		NewsletterToolbarExtendedValidElements = attributeCollection["NewsletterToolbarExtendedValidElements"].Value;
		//	}

		//	#endregion

		//	// legacy

		//	if (attributeCollection["EnableObjectResizing"] != null)
		//	{
		//		try
		//		{
		//			EnableObjectResizing = Convert.ToBoolean(attributeCollection["EnableObjectResizing"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["EnableUndoRedo"] != null)
		//	{
		//		try
		//		{
		//			EnableUndoRedo = Convert.ToBoolean(attributeCollection["EnableUndoRedo"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["UnDoLevels"] != null)
		//	{
		//		try
		//		{
		//			UnDoLevels = Convert.ToInt32(attributeCollection["UnDoLevels"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["Cleanup"] != null)
		//	{
		//		try
		//		{
		//			Cleanup = Convert.ToBoolean(attributeCollection["Cleanup"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["CleanupOnStart"] != null)
		//	{
		//		try
		//		{
		//			CleanupOnStart = Convert.ToBoolean(attributeCollection["CleanupOnStart"].Value);
		//		}
		//		catch (FormatException) { }
		//	}

		//	if (attributeCollection["AutoFocus"] != null)
		//	{
		//		try
		//		{
		//			AutoFocus = Convert.ToBoolean(attributeCollection["AutoFocus"].Value);
		//		}
		//		catch (FormatException) { }
		//	}


		//	if (attributeCollection["AdvancedFormatBlocks"] != null)
		//	{
		//		AdvancedFormatBlocks = attributeCollection["AdvancedFormatBlocks"].Value;
		//	}

		//	if (attributeCollection["AdvancedSourceEditorWidth"] != null)
		//	{
		//		AdvancedSourceEditorWidth = attributeCollection["AdvancedSourceEditorWidth"].Value;
		//	}

		//	if (attributeCollection["AdvancedSourceEditorHeight"] != null)
		//	{
		//		AdvancedSourceEditorHeight = attributeCollection["AdvancedSourceEditorHeight"].Value;
		//	}

		//	if (attributeCollection["AdvancedToolbarLocation"] != null)
		//	{
		//		AdvancedToolbarLocation = attributeCollection["AdvancedToolbarLocation"].Value;
		//	}

		//	if (attributeCollection["AdvancedToolbarAlign"] != null)
		//	{
		//		AdvancedToolbarAlign = attributeCollection["AdvancedToolbarAlign"].Value;
		//	}

		//	if (attributeCollection["AdvancedStatusBarLocation"] != null)
		//	{
		//		AdvancedStatusBarLocation = attributeCollection["AdvancedStatusBarLocation"].Value;
		//	}

		//	if (attributeCollection["AccessibilityWarnings"] != null)
		//	{
		//		AccessibilityWarnings = Convert.ToBoolean(attributeCollection["AccessibilityWarnings"].Value);
		//	}

		//	if (attributeCollection["DialogType"] != null)
		//	{
		//		DialogType = attributeCollection["DialogType"].Value;
		//	}
		//}

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