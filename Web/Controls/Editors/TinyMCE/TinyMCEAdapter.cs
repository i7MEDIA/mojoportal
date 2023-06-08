using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using System.IO;
using mojoPortal.Business.WebHelpers;
using System.Web;
using mojoPortal.Web.Controls.Editors;
using Newtonsoft.Json;
using mojoPortal.Core.Serializers.Newtonsoft;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace mojoPortal.Web.Editor
{

	public class TinyMceEditorAdapter : IWebEditor
	{
		#region Constructors

		public TinyMceEditorAdapter()
		{
			InitializeEditor();
		}

		#endregion

		#region Private Properties

		private TinyMceEditor Editor = new TinyMceEditor();
		private string scriptBaseUrl = string.Empty;
		private string siteRoot = string.Empty;
		private string skinName = string.Empty;
		private Unit editorWidth = Unit.Percentage(98);
		private Unit editorHeight = Unit.Pixel(350);
		private string editorCSSUrl = string.Empty;
		private Direction textDirection = Direction.LeftToRight;
		private ToolBar toolBar = ToolBar.AnonymousUser;
		private bool setFocusOnStart = false;
		private bool fullPageMode = false;
		private bool useFullyQualifiedUrlsForResources = false;
		private TinyMceConfiguration config = null;
		private TinyMceSettings editorSettings = null;

		#endregion

		#region Public Properties

		public string ControlID
		{
			get
			{
				return Editor.ID;
			}
			set
			{
				Editor.ID = value;
			}
		}

		public string ClientID
		{
			get
			{
				return Editor.ClientID;
			}

		}

		public string Text
		{
			get { return Editor.Text; }
			set { Editor.Text = value; }
		}

		public string ScriptBaseUrl
		{
			get
			{
				return scriptBaseUrl;
			}
			set
			{
				scriptBaseUrl = value;
				Editor.BasePath = scriptBaseUrl + "/tiny_mce/";
				if (ConfigurationManager.AppSettings["TinyMCE:BasePath"] != null)
				{
					Editor.BasePath = ConfigurationManager.AppSettings["TinyMCE:BasePath"];
				}

				//Editor.SkinPath = virtualRoot + "/FCKeditor/editor/skins/normal/";
			}
		}

		public string SiteRoot
		{
			get
			{
				return siteRoot;
			}
			set
			{
				siteRoot = value;
				//Editor.ImageBrowserURL = siteRoot + "/FCKeditor/editor/filemanager/browser/default/browser.html?Type=Image&Connector=connectors/aspx/connector.aspx";
				//Editor.LinkBrowserURL = siteRoot + "/FCKeditor/editor/filemanager/browser/default/browser.html?Connector=connectors/aspx/connector.aspx";

			}
		}

		public string SkinName
		{
			get { return skinName; }
			set { skinName = value; }
		}

		public string EditorCSSUrl
		{
			get
			{
				return editorCSSUrl;
			}
			set
			{
				editorCSSUrl = value;
				if (editorCSSUrl.Length > 0)
				{
					Editor.EditorAreaCSS = editorCSSUrl;
				}
			}
		}

		public Unit Width
		{
			get
			{
				return editorWidth;
			}
			set
			{
				editorWidth = value;
				Editor.Width = editorWidth;
			}
		}

		public Unit Height
		{
			get
			{
				return editorHeight;
			}
			set
			{
				editorHeight = value;
				Editor.Height = editorHeight;

			}
		}

		public Direction TextDirection
		{
			get
			{
				return textDirection;
			}
			set
			{
				textDirection = value;
				if (value == Direction.RightToLeft)
				{
					Editor.TextDirection = "rtl";
				}

			}
		}

		public ToolBar ToolBar
		{
			get
			{
				return toolBar;
			}
			set
			{
				toolBar = value;
				SetToolBar();
			}
		}

		public bool SetFocusOnStart
		{
			get { return setFocusOnStart; }
			set
			{
				setFocusOnStart = value;
				Editor.AutoFocus = setFocusOnStart;
			}
		}

		public bool FullPageMode
		{
			get { return fullPageMode; }
			set
			{
				fullPageMode = value;

			}
		}

		public bool UseFullyQualifiedUrlsForResources
		{
			get { return useFullyQualifiedUrlsForResources; }
			set
			{
				useFullyQualifiedUrlsForResources = value;

			}
		}

		#endregion

		#region Private Methods



		private void InitializeEditor()
		{


			config = TinyMceConfiguration.GetConfig();
			Editor.EmotionsBaseUrl = Editor.ResolveUrl("~/Data/SiteImages/emoticons/tinymce/");


			Editor.Height = editorHeight;
			Editor.Width = editorWidth;

			if (setFocusOnStart)
			{
				Editor.AutoFocus = true;
			}

			Editor.BasePath = WebConfigSettings.TinyMceBasePath;
			//Editor.Skin = WebConfigSettings.TinyMceSkin;

			SetToolBar();
		}

		private void SetToolBar()
		{
			/*
             http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
             */

			switch (toolBar)
			{
				case ToolBar.Full:

					editorSettings = config.GetEditorSettings("Full");

					string siteRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.ImagesUploadUrl = Editor.ResolveUrl(siteRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
					+ "&t=" + Global.FileSystemToken.ToString());
					//Editor.EnableFileBrowser = true;          
					Editor.StyleFormats = BuildTinyMceStyleJson();

					break;

				case ToolBar.FullWithTemplates:

					editorSettings = config.GetEditorSettings("FullWithTemplates");

					string sRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.FileManagerUrl = sRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.ImagesUploadUrl = Editor.ResolveUrl(sRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
					+ "&t=" + Global.FileSystemToken.ToString());

					//Editor.EnableFileBrowser = true;
					Editor.StyleFormats = BuildTinyMceStyleJson();
					Editor.TemplatesUrl = SiteUtils.GetNavigationSiteRoot() + "/Services/TinyMceTemplates.ashx?cb=" + Guid.NewGuid().ToString(); //cache busting guid

					break;

				case ToolBar.Newsletter:

					editorSettings = config.GetEditorSettings("Newsletter");

					string snRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.FileManagerUrl = snRoot + WebConfigSettings.FileDialogRelativeUrl;
					//Editor.EnableFileBrowser = true;

					break;

				case ToolBar.ForumWithImages:

					editorSettings = config.GetEditorSettings("ForumWithImages");

					Editor.FileManagerUrl = SiteUtils.GetNavigationSiteRoot() + WebConfigSettings.FileDialogRelativeUrl;
					//Editor.EnableFileBrowser = true;

					break;

				case ToolBar.Forum:

					editorSettings = config.GetEditorSettings("Forum");

					break;

				case ToolBar.AnonymousUser:

					editorSettings = config.GetEditorSettings("Anonymous");

					break;

				case ToolBar.SimpleWithSource:

					Editor.Plugins = "paste,searchreplace,fullscreen,emoticons,directionality,table,image";

					Editor.Toolbar1Buttons = "code,cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emoticons";

					Editor.Toolbar2Buttons = "";

					Editor.Toolbar3Buttons = "";

					Editor.DisableMenuBar = true;

					break;
			}

			Editor.Plugins = editorSettings.Plugins;
			Editor.Toolbar1Buttons = editorSettings.Toolbar1Buttons;
			Editor.Toolbar2Buttons = editorSettings.Toolbar2Buttons;
			Editor.Toolbar3Buttons = editorSettings.Toolbar3Buttons;
			Editor.ExtendedValidElements = editorSettings.ExtendedValidElements;
			Editor.ForcePasteAsPlainText = editorSettings.ForcePasteAsPlainText;
			Editor.DisableMenuBar = editorSettings.DisableMenuBar;
			Editor.Menubar = editorSettings.Menubar;
			Editor.UnDoLevels = editorSettings.UnDoLevels;
			Editor.EnableObjectResizing = editorSettings.EnableObjectResizing;
			Editor.Theme = editorSettings.Theme;
			Editor.Skin = editorSettings.Skin;
			Editor.AutoLocalize = editorSettings.AutoLocalize;
			Editor.Language = editorSettings.Language;
			Editor.TextDirection = editorSettings.TextDirection;
			Editor.EnableBrowserSpellCheck = editorSettings.EnableBrowserSpellCheck;
			Editor.EditorBodyCssClass = editorSettings.EditorBodyCssClass;
			Editor.NoWrap = editorSettings.NoWrap;
			Editor.RemovedMenuItems = editorSettings.RemovedMenuItems;
			Editor.FileDialogWidth = editorSettings.FileDialogWidth;
			Editor.FileDialogHeight = editorSettings.FileDialogHeight;
			Editor.EnableImageAdvancedTab = editorSettings.EnableImageAdvancedTab;
			Editor.ShowStatusbar = editorSettings.ShowStatusbar;

		}


		#endregion

		#region Public Methods
		public static string BuildTinyMceStyleJson()
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				return string.Empty;
			}


			string skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
			string currentSkin = siteSettings.Skin;

			var currentPage = CacheHelper.GetCurrentPage();

			if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
			{
				currentSkin = currentPage.Skin;
			}

			FileInfo skinStylesFile = new($"{skinRootFolder + currentSkin}\\config\\editorstyles.json"); ;
			FileInfo systemStylesFile = new(HttpContext.Current.Server.MapPath("~/data/style/editorstyles.json")); ;

			List<EditorStyle> styles = new();

			if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates && systemStylesFile.Exists)
			{
				styles.AddRange(EditorStyle.GetEditorStyles(systemStylesFile));
			}

			using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
			{
				while (reader.Read())
				{
					styles.Add(new EditorStyle
					{
						Name = reader["Name"].ToString(),
						Element = new List<string> { reader["Element"].ToString() },
						Attributes = new Dictionary<string, string>() { { "class", reader["CssClass"].ToString() } }
					});
				}
			}

			if (skinStylesFile.Exists)
			{
				styles.AddRange(EditorStyle.GetEditorStyles(skinStylesFile));
			}

			if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates && systemStylesFile.Exists)
			{
				styles.AddRange(EditorStyle.GetEditorStyles(systemStylesFile));
			}

			List<ITinyMceStyleBase> collection = new();




			if (styles.Any(x => x.Group?.Count > 0))
			{
				//var groups = styles
				//	.Where(x => x.Group?.Count > 0)
				//	.GroupBy(g => g.Group);

				List<string> groupNames = styles.Where(x => x.Group?.Count > 0).SelectMany(x => x.Group).Distinct().ToList();

				foreach (var group in groupNames)
				{
					if (string.IsNullOrWhiteSpace(group))
					{
						continue;
					}

					var col = new TinyMceStyleCollection
					{
						Title = group
					};
					var groupStyles = styles.Where(x => x.Group?.Count > 0).Where(x => x.Group.Contains(group)).ToList();

					processStyles(groupStyles, col);

					collection.Add(col);
				}
			}

			if (styles.Any(x => x.Group == null))
			{
				var iStyles = styles.Where(x => x.Group == null).ToList();
				var tCol = new TinyMceStyleCollection { Title = "empty" };
				processStyles(iStyles, tCol);

				collection.AddRange(tCol.Items);
			}

			void processStyles(List<EditorStyle> styles, TinyMceStyleCollection col)
			{
				foreach (var style in styles)
				{
					var tStyle = new TinyMceStyle
					{
						Title = style.Name,
					};
					if (style.ShouldSerializeElement())
					{
						if (style.ShouldSerializeAttributes())
						{
							tStyle.Classes = string.Join(" ", style.Attributes.Where(a => a.Key == "class").Select(a => a.Value).ToList());
						}
						if (style.Element.Count == 1)
						{
							tStyle.Selector = style.Element[0];
							col.Items.Add(tStyle);
						}
						else
						{
							foreach (var elem in style.Element)
							{
								var nestedStyle = new TinyMceStyle
								{
									Title = style.Name,
									Selector = elem,
									Classes = tStyle.Classes
								};
								col.Items.Add(nestedStyle);
							}
						}
					}
				}
			}


			//foreach (var group in groups)
			//{
			//	var col = new TinyMceStyleCollection
			//	{
			//		Title = group.title[0]
			//	};

			//	foreach (var item in group.items)
			//	{
			//		var style = new TinyMceStyle
			//		{
			//			Title = item.Name
			//		};

			//		if (item.ShouldSerializeElement())
			//		{
			//			style.Classes = string.Join(" ", item.Attributes.Where(a => a.Key == "class").Select(a=>a.Value).ToList());
			//			if (item.Element.Count == 1)
			//			{
			//				style.Selector = item.Element[0];
			//				col.Items.Add(style);
			//			}
			//			else
			//			{
			//				foreach (var elem in item.Element)
			//				{
			//					var nestedStyle = new TinyMceStyle
			//					{
			//						Title = style.Title,
			//						Selector = elem,
			//						Classes = style.Classes
			//					};
			//					col.Items.Add(nestedStyle);
			//				}
			//			}
			//		}
			//	}
			//collection.Add(col);
			//}



			var json = JsonConvert.SerializeObject(collection, Formatting.None);

			return json;

		}

		public Control GetEditorControl()
		{
			return Editor;
		}



		#endregion

	}

	public interface ITinyMceStyleBase
	{

	}

	public class TinyMceStyleCollection : ITinyMceStyleBase
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }
		[JsonProperty(PropertyName = "items")]
		public List<TinyMceStyle> Items { get; set; }

		public TinyMceStyleCollection()
		{
			Items = new List<TinyMceStyle>();
		}
	}

	public class TinyMceStyle : ITinyMceStyleBase
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "selector")]
		public string Selector { get; set; }

		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		[JsonProperty(PropertyName = "widget")]
		public string Widget { get; set; }

		[JsonProperty(PropertyName = "group")]
		[JsonConverter(typeof(SingleOrArrayConverter<string>))]
		public List<string> Group { get; set; }


		[JsonProperty(PropertyName = "classes")]
		public string Classes { get; set; }

		public bool ShouldSerializeType()
		{
			return Type != null;
		}

		public bool ShouldSerializeWidget()
		{
			return Widget != null;
		}

		public bool ShouldSerializeGroup()
		{
			return Group != null;
		}
	}
}
