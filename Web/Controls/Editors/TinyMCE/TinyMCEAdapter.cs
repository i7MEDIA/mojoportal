using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Serializers.Newtonsoft;
using mojoPortal.Web.Controls.Editors;
using Newtonsoft.Json;

namespace mojoPortal.Web.Editor;

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
	private Unit editorWidth = Unit.Percentage(98);
	private Unit editorHeight = Unit.Pixel(350);
	private string editorCSSUrl = string.Empty;
	private Direction textDirection = Direction.LeftToRight;
	private ToolBar toolBar = ToolBar.AnonymousUser;
	private bool setFocusOnStart = false;
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
		get => scriptBaseUrl;
		set
		{
			scriptBaseUrl = value;
			Editor.BasePath = ConfigHelper.GetStringProperty("TinyMCE:BasePath", $"{scriptBaseUrl}/tiny_mce/");
		}
	}

	public string SiteRoot { get; set; } = string.Empty;

	public string SkinName { get; set; } = string.Empty;

	public string EditorCSSUrl
	{
		get => editorCSSUrl;
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
		get => editorWidth;
		set
		{
			editorWidth = value;
			Editor.Width = editorWidth;
		}
	}

	public Unit Height
	{
		get => editorHeight;
		set
		{
			editorHeight = value;
			Editor.Height = editorHeight;

		}
	}

	public Direction TextDirection
	{
		get => textDirection;
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
		get => toolBar;
		set
		{
			toolBar = value;
			SetToolBar();
		}
	}

	public bool SetFocusOnStart
	{
		get => setFocusOnStart;
		set
		{
			setFocusOnStart = value;
			Editor.AutoFocus = setFocusOnStart;
		}
	}

	public bool FullPageMode { get; set; } = false;

	public bool UseFullyQualifiedUrlsForResources { get; set; } = false;

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

		Editor.BasePath = ConfigHelper.GetStringProperty("TinyMCE:BasePath", Editor.BasePath);

		SetToolBar();
	}

	private void SetToolBar()
	{
		var fileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();

		var imageUploadUrl = "Services/FileService.ashx".ToLinkBuilder().AddParams(
			new Dictionary<string, object> {
				{ "cmd", "uploadfromeditor" },
				{ "rz", "true" },
				{ "ko", WebConfigSettings.KeepFullSizeImagesDroppedInEditor },
				{ "t", Global.FileSystemToken }
			}).ToString();

		switch (toolBar)
		{
			case ToolBar.Full:

				editorSettings = config.GetEditorSettings("Full");
				Editor.FileManagerUrl = fileManagerUrl;
				Editor.ImagesUploadUrl = imageUploadUrl;
				Editor.StyleFormats = BuildTinyMceStyleJson();

				break;

			case ToolBar.FullWithTemplates:

				editorSettings = config.GetEditorSettings("FullWithTemplates");
				Editor.FileManagerUrl = fileManagerUrl;
				Editor.ImagesUploadUrl = imageUploadUrl;
				Editor.StyleFormats = BuildTinyMceStyleJson();
				Editor.TemplatesUrl = "Services/TinyMceTemplates.ashx".ToLinkBuilder().AddParam("cb", Guid.NewGuid()).ToString(); //cache busting guid

				break;

			case ToolBar.Newsletter:

				editorSettings = config.GetEditorSettings("Newsletter");

				string snRoot = SiteUtils.GetNavigationSiteRoot();
				Editor.FileManagerUrl = snRoot + WebConfigSettings.FileDialogRelativeUrl;


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
					Element = [reader["Element"].ToString()],
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

		List<ITinyMceStyleBase> collection = [];

		if (styles.Any(x => x.Group?.Count > 0))
		{
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
