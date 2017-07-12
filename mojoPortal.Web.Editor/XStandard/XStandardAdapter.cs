using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// Author:					
    /// Created:				2007-08-26
    /// Last Modified:		    2008-01-14
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// 
    /// </summary>
    public class XStandardAdapter : IWebEditor
    {
        #region Constructors

        public XStandardAdapter()
        {
            InitializeEditor();
        }

        #endregion

        #region Private Properties

        private XStandardControl Editor = new XStandardControl();
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
                Editor.BasePath = scriptBaseUrl + "/XStandard/";
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
            //this is true because we are using xhtml
            //Editor.UseStrictLoadingMode = true;

            // these are defaults but could use a service?
            //Editor.AdvancedBlockFormats = "p,address,pre,h1,h2,h3,h4,h5,h6";
            //Editor.AdvancedStyles = "floatpanel=floatpanel";
            
            
            Editor.Height = editorHeight;
            Editor.Width = editorWidth;

            //Editor.AdvancedSourceEditorWidth = "500";
            //Editor.AdvancedSourceEditorHeight = editorHeight.ToString().Replace("px", string.Empty);
            //Editor.AdvancedToolbarLocation = "top";
            //Editor.AdvancedToolbarAlign = "left";
            //Editor.AdvancedStatusBarLocation = "none";
            //Editor.Plugins = "template,paste,print,searchreplace,flash,fullscreen,emotions,directionality,table";
            
            if (setFocusOnStart)
            {
                Editor.AutoFocus = true;
            }


            SetToolBar();
        }

        private void SetToolBar()
        {
            

           
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
