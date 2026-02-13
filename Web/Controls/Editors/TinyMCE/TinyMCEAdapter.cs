using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Serializers.Newtonsoft;
using mojoPortal.Web.Controls.Editors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor;

public class TinyMceEditorAdapter : IWebEditor
{
	#region Constructors

	public TinyMceEditorAdapter() => InitializeEditor();

	#endregion


	#region Private Properties

	private readonly TinyMceEditor _editor = new();
	private TinyMceConfiguration _config = null;
	private TinyMceSettings _editorSettings = null;

	#endregion


	#region Public Properties

	public string ControlID
	{
		get => _editor.ID;
		set => _editor.ID = value;
	}

	public string ClientID => _editor.ClientID;

	public string Text
	{
		get => _editor.Text;
		set => _editor.Text = value;
	}

	public string ScriptBaseUrl
	{
		get => field;
		set
		{
			field = value;
			_editor.BasePath = ConfigHelper.GetStringProperty("TinyMCE:BasePath", $"{field}/tiny_mce/");
		}
	} = string.Empty;

	public string SiteRoot { get; set; } = string.Empty;
	public string SkinName { get; set; } = string.Empty;

	public string EditorCSSUrl
	{
		get => field;
		set
		{
			field = value;

			if (field.Length > 0)
			{
				_editor.EditorAreaCSS = field;
			}
		}
	} = string.Empty;

	public Unit Width
	{
		get;
		set
		{
			field = value;
			_editor.Width = field;
		}
	} = Unit.Percentage(98);

	public Unit Height
	{
		get;
		set
		{
			field = value;
			_editor.Height = field;

		}
	} = Unit.Pixel(350);

	public Direction TextDirection
	{
		get;
		set
		{
			field = value;

			if (value == Direction.RightToLeft)
			{
				_editor.TextDirection = "rtl";
			}

		}
	} = Direction.LeftToRight;

	public ToolBar ToolBar
	{
		get;
		set
		{
			field = value;
			SetToolBar();
		}
	} = ToolBar.AnonymousUser;

	public bool SetFocusOnStart
	{
		get;
		set
		{
			field = value;
			_editor.AutoFocus = field;
		}
	} = false;

	public bool FullPageMode { get; set; } = false;
	public bool UseFullyQualifiedUrlsForResources { get; set; } = false;

	#endregion


	#region Private Methods

	private void InitializeEditor()
	{
		_config = TinyMceConfiguration.GetConfig();
		_editor.EmotionsBaseUrl = _editor.ResolveUrl("~/Data/SiteImages/emoticons/tinymce/");
		_editor.Height = Height;
		_editor.Width = Width;

		if (SetFocusOnStart)
		{
			_editor.AutoFocus = true;
		}

		_editor.BasePath = ConfigHelper.GetStringProperty("TinyMCE:BasePath", _editor.BasePath);

		SetToolBar();
	}


	private void SetToolBar()
	{
		var fileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();
		var imageUploadUrl = "~/Services/FileService.ashx"
			.ToLinkBuilder()
			.AddParam("cmd", "uploadfromeditor")
			.AddParam("rz", "true")
			.AddParam("ko", WebConfigSettings.KeepFullSizeImagesDroppedInEditor)
			.AddParam("t", Global.FileSystemToken)
			.ToString();

		switch (ToolBar)
		{
			case ToolBar.Full:
				_editorSettings = _config.GetEditorSettings("Full");
				_editor.FileManagerUrl = fileManagerUrl;
				_editor.ImagesUploadUrl = imageUploadUrl;
				_editor.StyleFormats = BuildTinyMceStyleJson();

				break;

			case ToolBar.FullWithTemplates:

				_editorSettings = _config.GetEditorSettings("FullWithTemplates");
				_editor.FileManagerUrl = fileManagerUrl;
				_editor.ImagesUploadUrl = imageUploadUrl;
				_editor.StyleFormats = BuildTinyMceStyleJson();
				_editor.TemplatesUrl = "~/Services/TinyMceTemplates.ashx"
					.ToLinkBuilder()
					.AddParam("cb", Guid.NewGuid().ToString("N")) //cache busting guid
					.ToString();

				break;

			case ToolBar.Newsletter:
				_editorSettings = _config.GetEditorSettings("Newsletter");
				_editor.FileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();

				break;

			case ToolBar.ForumWithImages:
				_editorSettings = _config.GetEditorSettings("ForumWithImages");
				_editor.FileManagerUrl = WebConfigSettings.FileDialogRelativeUrl.ToLinkBuilder().ToString();

				break;

			case ToolBar.Forum:
				_editorSettings = _config.GetEditorSettings("Forum");

				break;

			case ToolBar.AnonymousUser:
				_editorSettings = _config.GetEditorSettings("Anonymous");

				break;

			case ToolBar.SimpleWithSource:
				_editor.Plugins = "paste,searchreplace,fullscreen,emoticons,directionality,table,image";
				_editor.Toolbar1Buttons = "code,cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emoticons";
				_editor.Toolbar2Buttons = "";
				_editor.Toolbar3Buttons = "";
				_editor.DisableMenuBar = true;

				break;
		}

		_editor.Plugins = _editorSettings.Plugins;
		_editor.Toolbar1Buttons = _editorSettings.Toolbar1Buttons;
		_editor.Toolbar2Buttons = _editorSettings.Toolbar2Buttons;
		_editor.Toolbar3Buttons = _editorSettings.Toolbar3Buttons;
		_editor.ExtendedValidElements = _editorSettings.ExtendedValidElements;
		_editor.ForcePasteAsPlainText = _editorSettings.ForcePasteAsPlainText;
		_editor.DisableMenuBar = _editorSettings.DisableMenuBar;
		_editor.Menubar = _editorSettings.Menubar;
		_editor.UnDoLevels = _editorSettings.UnDoLevels;
		_editor.EnableObjectResizing = _editorSettings.EnableObjectResizing;
		_editor.Theme = _editorSettings.Theme;
		_editor.Skin = _editorSettings.Skin;
		_editor.AutoLocalize = _editorSettings.AutoLocalize;
		_editor.Language = _editorSettings.Language;
		_editor.TextDirection = _editorSettings.TextDirection;
		_editor.EnableBrowserSpellCheck = _editorSettings.EnableBrowserSpellCheck;
		_editor.EditorBodyCssClass = _editorSettings.EditorBodyCssClass;
		_editor.NoWrap = _editorSettings.NoWrap;
		_editor.RemovedMenuItems = _editorSettings.RemovedMenuItems;
		_editor.FileDialogWidth = _editorSettings.FileDialogWidth;
		_editor.FileDialogHeight = _editorSettings.FileDialogHeight;
		_editor.EnableImageAdvancedTab = _editorSettings.EnableImageAdvancedTab;
		_editor.ShowStatusbar = _editorSettings.ShowStatusbar;
	}

	#endregion


	#region Public Methods

	public static string BuildTinyMceStyleJson()
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings == null)
		{
			return string.Empty;
		}

		var skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
		var currentSkin = siteSettings.Skin;
		var currentPage = CacheHelper.GetCurrentPage();

		if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
		{
			currentSkin = currentPage.Skin;
		}

		var skinStylesFile = new FileInfo($"""{skinRootFolder + currentSkin}\config\editorstyles.json""");
		var systemStylesFile = new FileInfo(HttpContext.Current.Server.MapPath("~/data/style/editorstyles.json")); ;

		var styles = new List<EditorStyle>();

		if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates && systemStylesFile.Exists)
		{
			styles.AddRange(EditorStyle.GetEditorStyles(systemStylesFile));
		}

		using (var reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
		{
			while (reader.Read())
			{
				styles.Add(new()
				{
					Name = reader["Name"].ToString(),
					Element = [reader["Element"].ToString()],
					Attributes = new Dictionary<string, string>()
					{
						{ "class", reader["CssClass"].ToString() }
					}
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

		var collection = new List<ITinyMceStyleBase>();

		if (styles.Any(x => x.Group?.Count > 0))
		{
			var groupNames = styles
				.Where(x => x.Group?.Count > 0)
				.SelectMany(x => x.Group)
				.Distinct()
				.ToList();

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
				var groupStyles = styles
					.Where(x => x.Group?.Count > 0)
					.Where(x => x.Group.Contains(group))
					.ToList();

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

		static void processStyles(List<EditorStyle> editorStyles, TinyMceStyleCollection col)
		{
			foreach (var editorStyle in editorStyles)
			{
				var style = new TinyMceStyle
				{
					Title = editorStyle.Name,
				};

				if (editorStyle.ShouldSerializeElement())
				{
					if (editorStyle.ShouldSerializeAttributes())
					{
						var classList = editorStyle.Attributes
							.Where(a => a.Key == "class")
							.Select(a => a.Value)
							.ToList();

						style.Classes = string.Join(" ", classList);
					}

					if (editorStyle.Element.Count == 1)
					{
						style.Selector = editorStyle.Element[0];
						col.Items.Add(style);
					}
					else
					{
						editorStyle.Element.ForEach(element => col.Items.Add(new()
						{
							Title = editorStyle.Name,
							Selector = element,
							Classes = style.Classes
						}));
					}
				}
			}
		}

		return JsonConvert.SerializeObject(collection, Formatting.None);
	}


	public Control GetEditorControl() => _editor;

	#endregion
}


public interface ITinyMceStyleBase
{ }


public class TinyMceStyleCollection : ITinyMceStyleBase
{
	[JsonProperty(PropertyName = "title")]
	public string Title { get; set; }

	[JsonProperty(PropertyName = "items")]
	public List<TinyMceStyle> Items { get; set; }


	public TinyMceStyleCollection() => Items = [];
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

	public bool ShouldSerializeType() => Type != null;
	public bool ShouldSerializeWidget() => Widget != null;
	public bool ShouldSerializeGroup() => Group != null;
}
