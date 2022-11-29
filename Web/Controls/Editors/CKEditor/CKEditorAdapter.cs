using mojoPortal.Business.WebHelpers;
using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
	public class CKEditorAdapter : IWebEditor
	{
		public CKEditorAdapter()
		{
			InitializeEditor();
		}

		#region Private Properties

		private CKEditorControl Editor = new CKEditorControl();
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
				Editor.EditorCSSUrl = value;
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
				Editor.TextDirection = value;
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
				Editor.ToolBar = value;
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
				Editor.FullPageMode = value;
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
			Editor.Skin = WebConfigSettings.CKEditorSkin;
		}

		private void SetToolBar()
		{
			string siteRoot = SiteUtils.GetNavigationSiteRoot();

			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			string skinRootFolder = SiteUtils.GetSiteSkinFolderPath();

			string currentSkin = siteSettings.Skin;
			var currentPage = CacheHelper.GetCurrentPage();

			if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
			{
				currentSkin = currentPage.Skin;
			}

			FileInfo skinTemplates = new FileInfo($"{skinRootFolder + currentSkin}\\config\\ckeditortemplates.js");

			var skinUrl = SiteUtils.DetermineSkinBaseUrl(currentSkin);
			
			Editor.MojoSkinPath = skinUrl;

			switch (toolBar)
			{
				case ToolBar.Full:
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					Editor.StylesJsonUrl = siteRoot + "/Services/CKeditorStyles.ashx?cb=" + Guid.NewGuid().ToString().Replace("-", string.Empty);
					Editor.DropFileUploadUrl = $"{siteRoot}/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko={WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()}&t={Global.FileSystemToken}";
					break;

				case ToolBar.FullWithTemplates:
					//string sRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					//string navRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.TemplatesJsonUrl = siteRoot + "/Services/CKeditorTemplates.ashx?cb=" + Guid.NewGuid().ToString(); //prevent caching with a guid param
					
					if (skinTemplates.Exists)
					{
						
						Editor.SkinTemplatesUrl = $"{skinUrl}config/ckeditortemplates.js?cb={Guid.NewGuid()}";
					}
 
					Editor.StylesJsonUrl = siteRoot + "/Services/CKeditorStyles.ashx?cb=" + Guid.NewGuid().ToString().Replace("-",string.Empty);
					Editor.DropFileUploadUrl = $"{siteRoot}/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko={WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()}&t={Global.FileSystemToken}";
					break;

				case ToolBar.Newsletter:
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					Editor.FullPageMode = true;
					break;

				case ToolBar.ForumWithImages:
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					Editor.ForcePasteAsPlainText = true;
					break;

				case ToolBar.Forum:
					Editor.ForcePasteAsPlainText = true;
					break;

				case ToolBar.AnonymousUser:
					break;

				case ToolBar.SimpleWithSource:
					break;
			}
		}

		#endregion

		#region Public Methods

		public Control GetEditorControl()
		{
			InitializeEditor();
			return Editor;
		}

		#endregion
	}
}
