// Author:		        
// Created:            2007-05-29
// Last Modified:      2014-01-07
//
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

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
                    Editor.DropFileUploadUrl = Editor.ResolveUrl(siteRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
                    + "&t=" + Global.FileSystemToken.ToString());
                    //Editor.EnableFileBrowser = true;          
                    Editor.StyleFormats = SiteUtils.BuildStylesListForTinyMce4();
                    
                    break;

                case ToolBar.FullWithTemplates:

                    editorSettings = config.GetEditorSettings("FullWithTemplates");

                    string sRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = sRoot + WebConfigSettings.FileDialogRelativeUrl;
                    Editor.DropFileUploadUrl = Editor.ResolveUrl(sRoot + "/Services/FileService.ashx?cmd=uploadfromeditor&rz=true&ko=" + WebConfigSettings.KeepFullSizeImagesDroppedInEditor.ToString().ToLower()
                    + "&t=" + Global.FileSystemToken.ToString());

                    //Editor.EnableFileBrowser = true;
                    Editor.StyleFormats = SiteUtils.BuildStylesListForTinyMce4();
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

        public Control GetEditorControl()
        {
            return Editor;
        }



        #endregion

    }

    /// <summary>
    /// this one is for the older version 3.x of TinyMCE
    /// </summary>
    public class TinyMCEAdapter : IWebEditor
    {
        #region Constructors

        public TinyMCEAdapter()
        {
            InitializeEditor();
        }

        #endregion

        #region Private Properties

        private TinyMCE Editor = new TinyMCE();
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

            Editor.AdvancedBlockFormats = config.AdvancedFormatBlocks;

            Editor.AdvancedStyles = SiteUtils.BuildStylesListForTinyMce();
            Editor.TemplatesUrl = SiteUtils.GetNavigationSiteRoot() + "/Services/TinyMceTemplates.ashx?cb=" + Guid.NewGuid().ToString(); //cache busting guid
            Editor.EmotionsBaseUrl = Editor.ResolveUrl("~/Data/SiteImages/emoticons/tinymce/");
        
            Editor.DialogType = config.DialogType;
            
            Editor.Height = editorHeight;
            Editor.Width = editorWidth;

            Editor.AdvancedSourceEditorWidth = config.AdvancedSourceEditorWidth;
            Editor.AdvancedSourceEditorHeight = config.AdvancedSourceEditorHeight;
            Editor.AdvancedToolbarLocation = config.AdvancedToolbarLocation;
            Editor.AdvancedToolbarAlign = config.AdvancedToolbarAlign;
            Editor.AdvancedStatusBarLocation = config.AdvancedStatusBarLocation;

            
            if (setFocusOnStart)
            {
                Editor.AutoFocus = true;
            }

            
            Editor.BasePath = WebConfigSettings.TinyMceBasePath;
            Editor.Skin = WebConfigSettings.TinyMceSkin;

            
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

                    
                    Editor.Plugins = config.FullToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.FullToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.FullToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.FullToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.FullToolbarExtendedValidElements;

                    string siteRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = siteRoot + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;
                    Editor.ForcePasteAsPlainText = config.FullToolbarForcePasteAsPlainText;
                    
                    break;

                case ToolBar.FullWithTemplates:

                   
                    Editor.Plugins = config.FullWithTemplatesToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.FullWithTemplatesToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.FullWithTemplatesToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.FullWithTemplatesToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.FullWithTemplatesToolbarExtendedValidElements;

                    string sRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = sRoot + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;
                    Editor.ForcePasteAsPlainText = config.FullWithTemplatesToolbarForcePasteAsPlainText;
                   
                    
                    break;

                case ToolBar.Newsletter:

                    Editor.Plugins = config.NewsletterToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.NewsletterToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.NewsletterToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.NewsletterToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.NewsletterToolbarExtendedValidElements;

                    string snRoot = SiteUtils.GetNavigationSiteRoot();
                    Editor.FileManagerUrl = snRoot + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;
                    Editor.ForcePasteAsPlainText = config.NewsletterToolbarForcePasteAsPlainText;
                    
                    break;

                case ToolBar.ForumWithImages:

                    Editor.Plugins = config.ForumWithImagesToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.ForumWithImagesToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.ForumWithImagesToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.ForumWithImagesToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.ForumWithImagesToolbarExtendedValidElements;
                    
                    Editor.FileManagerUrl = SiteUtils.GetNavigationSiteRoot() + "/Dialog/FileDialog.aspx";
                    Editor.EnableFileBrowser = true;
                    Editor.ForcePasteAsPlainText = config.ForumWithImagesToolbarForcePasteAsPlainText;

                    break;

                case ToolBar.Forum:

                    Editor.Plugins = config.ForumToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.ForumToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.ForumToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.ForumToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.ForumToolbarExtendedValidElements;

                    Editor.ForcePasteAsPlainText = config.ForumToolbarForcePasteAsPlainText;
                    
                    
                    break;



                case ToolBar.AnonymousUser:

                    Editor.Plugins = config.AnonymousToolbarPlugins;
                    Editor.AdvancedRow1Buttons = config.AnonymousToolbarRow1Buttons;
                    Editor.AdvancedRow2Buttons = config.AnonymousToolbarRow2Buttons;
                    Editor.AdvancedRow3Buttons = config.AnonymousToolbarRow3Buttons;
                    Editor.ExtendedValidElements = config.AnonymousToolbarExtendedValidElements;

                    Editor.ForcePasteAsPlainText = config.AnonymousToolbarForcePasteAsPlainText;


                    break;

                case ToolBar.SimpleWithSource:

                    Editor.Plugins = "paste,print,searchreplace,fullscreen,emotions,directionality,table,advimage,inlinepopups";

                    Editor.AdvancedRow1Buttons = "code,cut,copy,pastetext,separator,blockquote,bold,italic,separator,bullist,numlist,separator,link,unlink,emotions";

                    Editor.AdvancedRow2Buttons = "";

                    Editor.AdvancedRow3Buttons = "";

                    break;
            }

        }

        #endregion

        #region Public Methods

        public Control GetEditorControl()
        {
            return Editor;
        }

        

        #endregion

    }
}
