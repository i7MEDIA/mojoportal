// Author:					
// Created:					2009-04-02
// Last Modified:			2014-01-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

//TODO: need to figure out how to implement image browsing and link browsing

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
	public class CKEditorAdapter : IWebEditor
	{
		#region Constructors

		public CKEditorAdapter()
		{
			InitializeEditor();
		}

		#endregion

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

			switch (toolBar)
			{
				case ToolBar.Full:
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					Editor.StylesJsonUrl = siteRoot + "/Services/CKeditorStyles.ashx?cb=" + Guid.NewGuid().ToString().Replace("-", string.Empty);
					Editor.DropFileUploadUrl = siteRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
					+ "&t=" + Global.FileSystemToken.ToString();
					break;

				case ToolBar.FullWithTemplates:
					//string sRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					//string navRoot = SiteUtils.GetNavigationSiteRoot();
					Editor.TemplatesJsonUrl = siteRoot + "/Services/CKeditorTemplates.ashx?cb=" + Guid.NewGuid().ToString(); //prevent caching with a guid param
					//Editor.TemplatesXmlUrl = navRoot + "/Services/HtmlTemplates.ashx?cb=" + Guid.NewGuid().ToString(); 
					Editor.StylesJsonUrl = siteRoot + "/Services/CKeditorStyles.ashx?cb=" + Guid.NewGuid().ToString().Replace("-",string.Empty);
					//Editor.StylesJsonUrl =  "/ckstyles.js";
					Editor.DropFileUploadUrl = siteRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
					+ "&t=" + Global.FileSystemToken.ToString();
					break;

				case ToolBar.Newsletter:
					Editor.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
					Editor.EnableFileBrowser = true;
					Editor.FullPageMode = true;
					//Editor.CustomConfigPath = "~/ClientScript/ckeditor-mojo-newsletterconfig.js";
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
