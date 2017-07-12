// Author:					
// Created:				    2009-07-12
// Last Modified:			2009-07-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// A Code editor using edit_area javascript editor
    /// http://www.cdolivet.com/index.php?page=editArea
    /// http://www.cdolivet.com/editarea/editarea/docs/
    /// http://www.cdolivet.com/editarea/editarea/docs/configuration.html
    /// </summary>
    public class CodeEditor : TextBox
    {

        #region Private Properties

        private string scriptBaseDirectory = WebConfigSettings.EditAreaBasePath;
        private string language = "en";
        private string syntax = "css";
        private bool startHighlighted = false;
        private bool multiFileMode = false;
        private int minWidth = 400;
        private int minHeight = 200;
        private string allowResize = "both"; //no, both, x, y are valid values
        private bool allowToggle = true;
        private string plugins = string.Empty;
        private string browsers = "known"; //all or known
        private string display = "onload"; // onload or later
        private string toolbar = "search, go_to_line, fullscreen, |, undo, redo, |, select_font,|, change_smooth_selection, highlight, reset_highlight, word_wrap, |, help";
        private string beginToolbar = string.Empty;
        private string endToolbar = string.Empty;
        private int fontSize = 10;
        private string fontFamily = "monospace";
        private string cursorPosition = "begin"; //begin or auto
        private bool useGeckoSpellCheck = false;
        private int maxUndo = 20;
        private bool startFullScreen = false;
        private bool isEditable = true;
        private bool wordWrap = false;
        private bool replaceTabsWithSpaces = false;
        private bool debug = false;



        #endregion

        #region Public Properties

        /// <summary>
        /// The path to the folder where the edit_area js is located
        /// </summary>
        public string ScriptBaseDirectory
        {
            get { return scriptBaseDirectory; }
            set { scriptBaseDirectory = value; }
        }

        /// <summary>
        /// Default = "en", should contain a code of the language pack to be used for translation. 
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        /// <summary>
        /// should contain a code of the syntax definition file that must be used for the highlight mode. 
        /// Valid options: c, css, html, js, tsql, vb, xml
        /// actually there are others too but these are the ones of interest in mojoPortal
        /// </summary>
        public string Syntax
        {
            get { return syntax; }
            set { syntax = value; }
        }

        /// <summary>
        /// set if the editor should start with highlighted syntax displayed. Default is false.
        /// </summary>
        public bool StartHighlighted
        {
            get { return startHighlighted; }
            set { startHighlighted = value; }
        }

        /// <summary>
        /// determine if the editor load the content of the textarea (false) or if it wait for an openFile() call for allowing file editing. 
        /// probably we will not use this in mojoportal, we will use mainly the text area content
        /// </summary>
        public bool MultiFileMode
        {
            get { return multiFileMode; }
            set { multiFileMode = value; }
        }

        /// <summary>
        /// define the minimum width of the editor default is 400
        /// </summary>
        public int MinWidth
        {
            get { return minWidth; }
            set { minWidth = value; }
        }

        /// <summary>
        /// define the minimum height of the editor default is 200
        /// </summary>
        public int MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; }
        }

        /// <summary>
        /// no, both, x, y are valid values, default is both
        /// </summary>
        public string AllowResize
        {
            get { return allowResize; }
            set { allowResize = value; }
        }

        /// <summary>
        /// define if a toggle button must be added under the editor in order to allow to toggle between the editor and the orginal textarea.
        /// </summary>
        public bool AllowToggle
        {
            get { return allowToggle; }
            set { allowToggle = value; }
        }

        /// <summary>
        /// a comma separated list of plugins to load. 
        /// </summary>
        public string Plugins
        {
            get { return plugins; }
            set { plugins = value; }
        }

        /// <summary>
        /// define if the editor must be loaded only when the user navigotr is known to be a working one, or if it will be loaded for all navigators.
        /// all or known are valid choices, default is known
        /// </summary>
        public string Browsers
        {
            get { return browsers; }
            set { browsers = value; }
        }

        //public string Display
        //{
        //    get { return display; }
        //    set { display = value; }
        //}

        /// <summary>
        /// define the toolbar that will be displayed, each element being separated by a ",". 
        /// default:
        /// search, go_to_line, fullscreen, |, undo, redo, |, select_font,|, change_smooth_selection, highlight, reset_highlight, word_wrap, |, help
        /// </summary>
        public string Toolbar
        {
            get { return toolbar; }
            set { toolbar = value; }
        }

        /// <summary>
        /// toolbar button list to add before the toolbar defined by the "toolbar" option. 
        /// </summary>
        public string BeginToolbar
        {
            get { return beginToolbar; }
            set { beginToolbar = value; }
        }

        /// <summary>
        /// toolbar button list to add after the toolbar defined by the "toolbar" option. 
        /// </summary>
        public string EndToolbar
        {
            get { return endToolbar; }
            set { endToolbar = value; }
        }

        /// <summary>
        /// define the font-size used to display the text in the editor. default is 10
        /// </summary>
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        /// <summary>
        /// define the font-familly used to display the text in the editor. default is monospace
        /// </summary>
        public string FontFamily
        {
            get { return fontFamily; }
            set { fontFamily = value; }
        }

        /// <summary>
        /// define if the cursor should be placed where it was in the textarea before replacement (auto) or at the beginning of the file (begin).
        /// default is begin
        /// </summary>
        public string CursorPosition
        {
            get { return cursorPosition; }
            set { cursorPosition = value; }
        }

        /// <summary>
        /// allow to disable/enable the Firefox 2 spellchecker default is false
        /// </summary>
        public bool UseGeckoSpellCheck
        {
            get { return useGeckoSpellCheck; }
            set { useGeckoSpellCheck = value; }
        }

        /// <summary>
        /// number of undo action allowed default is 20
        /// </summary>
        public int MaxUndo
        {
            get { return maxUndo; }
            set { maxUndo = value; }
        }

        /// <summary>
        /// determine if EditArea start in fullscreen mode or not default is false
        /// </summary>
        public bool StartFullScreen
        {
            get { return startFullScreen; }
            set { startFullScreen = value; }
        }

        /// <summary>
        /// determine if EditArea display only
        /// </summary>
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        /// <summary>
        /// determine if the text will be automatically wrapped to the next line when it reach the end of a line. This is linked ot the word_wrap icon available in the toolbar.
        /// default is false
        /// </summary>
        public bool WordWrap
        {
            get { return wordWrap; }
            set { wordWrap = value; }
        }

        /// <summary>
        /// define the number of spaces that will replace tabulations (\t) in text. If tabulation should stay tabulation, set this option to false.
        /// default is false
        /// </summary>
        public bool ReplaceTabsWithSpaces
        {
            get { return replaceTabsWithSpaces; }
            set { replaceTabsWithSpaces = value; }
        }

        /// <summary>
        /// used to display some debug information into a newly created textarea. Can be usefull to display trace info in it if you want to modify the code.
        /// default is false
        /// </summary>
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        private bool disable = false;

        public bool Disable
        {
            get { return disable; }
            set { disable = value; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            TextMode = TextBoxMode.MultiLine;
            Rows = 20;
            Columns = 170;
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (WebConfigSettings.DisableEditArea) { return; }

            if (disable) { return; }

            SetupScript();

        }

        private void SetupScript()
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "editarea", "<script src=\""
                + ResolveUrl(scriptBaseDirectory + "/edit_area_full.js") + "\" type=\"text/javascript\"></script>");

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n<!-- \n");

            script.Append("editAreaLoader.init({");
            script.Append("id:\"" + ClientID + "\" ");
            script.Append(",syntax:\"" + syntax + "\"");
            script.Append(",language:\"" + language + "\"");

            if (startHighlighted)
            {
                script.Append(",start_highlight:true");
            }

            if (multiFileMode)
            {
                script.Append(",is_multi_files:true");
            }

            script.Append(",min_width:" + minWidth.ToInvariantString());
            script.Append(",min_height:" + minHeight.ToInvariantString());

            script.Append(",allow_resize:\"" + allowResize + "\"");

            if (!allowToggle)
            {
                script.Append(",allow_toggle:false");
            }

            if (plugins.Length > 0)
            {
                script.Append(",plugins:\"" + plugins + "\"");
            }

            script.Append(",browsers:\"" + browsers + "\"");
            script.Append(",display:\"" + display + "\"");
            script.Append(",toolbar:\"" + toolbar + "\"");

            if (beginToolbar.Length > 0)
            {
                script.Append(",begin_toolbar:\"" + beginToolbar + "\"");
            }

            if (endToolbar.Length > 0)
            {
                script.Append(",end_toolbar:\"" + endToolbar + "\"");
            }

            script.Append(",font_size:" + fontSize.ToInvariantString());
            script.Append(",font_family:\"" + fontFamily + "\"");
            script.Append(",cursor_position:\"" + cursorPosition + "\"");

            if (useGeckoSpellCheck)
            {
                script.Append(",gecko_spellcheck:true");
            }

            script.Append(",max_undo:" + maxUndo.ToString(CultureInfo.InvariantCulture));

            if (startFullScreen)
            {
                script.Append(",fullscreen:true");
            }

            if (!isEditable)
            {
                script.Append(",is_editable:false");
            }

            if (wordWrap)
            {
                script.Append(",word_wrap:true");
            }

            if (replaceTabsWithSpaces)
            {
                script.Append(",replace_tab_by_spaces:true");
            }

            if (debug)
            {
                script.Append(",debug:true");
            }

            script.Append("}); ");

            script.Append("\n//--> ");
            script.Append(" </script>");

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "setup" + this.ClientID,
                script.ToString());

        }

    }
}
