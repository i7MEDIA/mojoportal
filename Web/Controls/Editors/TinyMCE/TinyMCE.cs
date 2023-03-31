/// Author:		        
/// Created:            2007-05-27
/// Last Modified:      2013-03-26
/// 
/// Licensed under the terms of the GNU Lesser General Public License:
///	http://www.opensource.org/licenses/lgpl-license.php
///
/// You must not remove this notice, or any other, from this software.
/// 
/// This is a .NET wrapper control for the TinyMCE javascript editor
/// http://wiki.moxiecode.com/index.php/TinyMCE:Index

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Permissions;
using Resources;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// A .NET wrapper control around the TinyMCE editor
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Index
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/template
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/theme_advanced_blockformats
    /// </summary>
    [DefaultProperty("Text")]
    [ValidationProperty("Text")]
    [ToolboxData("<{0}:TinyMCE runat=server></{0}:TinyMCE>")]
    [Designer("mojoPortal.Web.Editor.TinyMCEDesigner")]
    [ParseChildren(false)]
    public class TinyMCE : Control, IPostBackDataHandler
    {
       
        public TinyMCE(){}

        #region Properties

        [DefaultValue("")]
        public string Text { get; set; } = string.Empty;


        #region Appearence Properties

        /// <summary>
        /// advanced theme has 2 skins default and o2k7
        /// o2k7 also supports skin_variant with default, silver and black as options
        /// so basically we have
        /// default
        /// o2k7default
        /// o2k7silver
        /// o2k7black
        /// </summary>
        public string Skin { get; set; } = "default";

        [Category("Appearence")]
        [DefaultValue("100%")]
        public Unit Width { get; set; } = Unit.Percentage(100);

        [Category("Appearence")]
        [DefaultValue("200px")]
        public Unit Height
        {
            get { object o = ViewState["Height"]; return (o == null ? Unit.Pixel(200) : (Unit)o); }
            set { ViewState["Height"] = value; }
        }

        #endregion

        #region Configuration Properties


        public string BasePath { get; set; } = "/ClientScript/tiny_mce/";
        public string ForcedRootBlock { get; set; } = "p";

        public bool EnableFileBrowser { get; set; } = false;

        public bool ConvertUrls { get; set; } = false;

        public bool ForcePasteAsPlainText { get; set; } = false;

        public string FileManagerUrl { get; set; } = string.Empty;

        public string ExtendedValidElements { get; set; } = string.Empty;

        /// <summary>
        /// This option enables you to auto focus an editor instance. The 
        /// value of this option should be an editor instance id. Editor 
        /// instance ids are specified as "mce_editor_<index>", where 
        /// index is a value starting from 0. So if there are 3 editor 
        /// instances on a page, they would have the following 
        /// ids - mce_editor_0, mce_editor_1 and mce_editor_2.
        /// </summary>
        public bool AutoFocus { get; set; } = false;

        /// <summary>
        /// If true, some accessibility focus will be available to all buttons: 
        /// you will be able to tab through them all. If false, focus will be 
        /// placed inside the text area when you tab through the interface. 
        /// The default is true.
        /// </summary>
        public bool AccessibilityFocus { get; set; } = true;

        /// <summary>
        /// If this option is set to true some accessibility warnings will be 
        /// presented to the user if they miss specifying that information. 
        /// This option is set to true default, since we should all try to 
        /// make this world a better place for disabled people. But if you 
        /// are annoyed with the warnings, set this option to false.
        /// </summary>
        public bool AccessibilityWarnings { get; set; } = true;

        /// <summary>
        /// This option should contain a comma separated list of supported 
        /// browsers. This enables you, for example, to disable the editor 
        /// while running on Safari. The default value of this option 
        /// is: msie,gecko,safari,opera. Since the support for Safari is 
        /// very limited, a warning message will appear until a better 
        /// version is released. The possible values of this option 
        /// are msie, gecko, safari and opera.
        /// </summary>
        public string Browsers { get; set; } = "msie,gecko,safari,opera";

        /// <summary>
        /// This option enables you to disable/enable the custom keyboard 
        /// shortcuts, which plugins and themes may register. The value 
        /// of this option is set to true by default.
        /// </summary>
        public bool CustomShortcuts { get; set; } = true;

        /// <summary>
        /// This option enables you to specify how dialogs/popups should be 
        /// opened, possible values are "window" and "modal", where the 
        /// window option opens a normal window and the dialog option opens 
        /// a modal dialog. This option is set to "window" by default.
        /// </summary>
        public string DialogType { get; set; } = "window";

        /// <summary>
        /// This option specifies the default writing direction, some languages 
        /// (Like Hebrew, Arabic, Urdu...) write from right to left instead 
        /// of left to right. The default value of this option is "ltr" but 
        /// if you want to use from right to left mode specify "rtl" instead.
        /// </summary>
        public string TextDirection { get; set; } = "ltr";

        /// <summary>
        /// This option enables you to specify a CSS class name that will 
        /// deselect textareas from being converted into editor instances. 
        /// If this option isn't set to a value, this option will not have 
        /// any effect, and the mode option will choose textareas instead. 
        /// The default value of this option is "mceNoEditor", so if 
        /// mceNoEditor is added to the class attribute of a textarea it will 
        /// be excluded for conversion. This option also enables you to use 
        /// regular expressions like myEditor|myOtherEditor or .*editor.
        /// </summary>
        public string DeSelectorCSSClass { get; set; } = "mceNoEditor";

        public bool EnableGeckoSpellCheck { get; set; } = true;

        /// <summary>
        /// This option should contain a language code of the language pack to 
        /// use with TinyMCE. These codes are in ISO-639-1 format to see if 
        /// your language is available check the contents of 
        /// "tinymce/jscripts/tiny_mce/langs". The default value of this 
        /// option is "en" for English.
        /// </summary>
        public string Language { get; set; } = "en";

        /// <summary>
        /// This true/false option gives you the ability to turn on/off the 
        /// inline resizing controls of tables and images in Firefox/Mozilla. 
        /// These are enabled by default.
        /// </summary>
        public bool EnableObjectResizing { get; set; } = true;

        /// <summary>
        /// This option should contain a comma separated list of plugins. Plugins 
        /// are loaded from the "tinymce/jscripts/tiny_mce/plugins" directory, 
        /// and the plugin name matches the name of the directory. TinyMCE is 
        /// shipped with some core plugins; these are described in greater 
        /// detail in the Plugins reference.
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins
        /// 
        /// TinyMCE also supports the ability to have plugins added from a external 
        /// resource. These plugins need to be self registering and loaded 
        /// after the tinyMCE.init call. You should also prefix these plugins 
        /// with a "-" character, so that TinyMCE doesn't try to load it from 
        /// the TinyMCE plugins directory.
        /// 
        /// There are many third party plugins for TinyMCE; some of these may be 
        /// found under "Plugins" at SourceForge, and, if you have developed 
        /// one of your own, please contribute it to this project by uploading 
        /// it to SourceForge.
        /// </summary>
        public string Plugins { get; set; } = string.Empty;

        /// <summary>
        /// JavaScript file containing an array of template files.
        /// </summary>
        public string TemplatesUrl { get; set; } = string.Empty;

        /// <summary>
        /// JavaScript file containing an array of template files.
        /// </summary>
        public string EmotionsBaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// This option will force TinyMCE to load script using a DOM insert 
        /// method, instead of document.write, on Gecko browsers. Since this 
        /// results in asynchronous script loading, a build in synchronized 
        /// will ensure that themes, plugins and language packs files are 
        /// loaded in the correct order. This will, on the other hand, make 
        /// the initialization procedure of TinyMCE a bit slower, and that's 
        /// why this isn't the default behavior. This option is set to true 
        /// by default, if the document content type is application/xhtml+xml.
        /// 
        /// </summary>
        public bool UseStrictLoadingMode { get; set; } = false;

        /// <summary>
        /// This option enables you to specify what theme to use when rendering 
        /// the TinyMCE WYSIWYG editor instances. This name matches the 
        /// directories located in tinymce/jscripts/tiny_mce/themes. The 
        /// default value of this option is "advanced". TinyMCE has two 
        /// built-in themes described below.
        /// 
        /// advanced
        /// This theme enables users to add/remove buttons and panels and is a 
        /// lot more flexible than the simple theme. For more information about 
        /// this theme's specific options check the advanced theme 
        /// configuration section. This is the default theme.
        /// 
        /// simple
        /// This is the most simple theme for TinyMCE. It contains only 
        /// the basic functions.
        /// 
        /// Example Usage
        /// 
        ///     tinyMCE.init({
	    ///         ...
	    ///     theme : "advanced",
	    ///     theme_advanced_buttons3_add_before : "tablecontrols,separator"
        ///     });
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("advanced")]
        public string Theme
        {
            get { return (ViewState["Theme"] != null ? (string)ViewState["Theme"] : "advanced"); }
            set { ViewState["Theme"] = value; }
        }

        /// <summary>
        /// This option is a true/false option that enables you to 
        /// disable/enable the custom undo/redo logic within TinyMCE. 
        /// This option is enabled by default, if you disable it some 
        /// operations may not be undo able.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue(true)]
        public bool EnableUndoRedo
        {
            get { return (ViewState["EnableUndoRedo"] != null ? (bool)ViewState["EnableUndoRedo"] : true); }
            set { ViewState["EnableUndoRedo"] = value; }
        }

        /// <summary>
        /// This option should contain the number of undo levels to keep in 
        /// memory. This is set to -1 by default and such a value tells 
        /// TinyMCE to use a unlimited number of undo levels. But this 
        /// steals lots of memory so for low end systems a value of 10 may 
        /// be better.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue(10)]
        public int UnDoLevels
        {
            get { return (ViewState["UnDoLevels"] != null ? (int)ViewState["UnDoLevels"] : 10); }
            set { ViewState["UnDoLevels"] = value; }
        }

        /// <summary>
        /// This option enables you to switch button and panel layout 
        /// functionality.
        /// 
        /// There are three different layout manager options:
        /// 
        /// SimpleLayout is the default layout manager,
        /// RowLayout is a more advanced layout manager, and
        /// CustomLayout executes a custom layout manager function.
        /// 
        /// Each of these layout managers have different options and can be 
        /// configured in different ways. This option is only available if the "advanced" theme is used.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("SimpleLayout")]
        public string AdvancedLayoutManager
        {
            get { return (ViewState["AdvancedLayoutManager"] != null ? (string)ViewState["AdvancedLayoutManager"] : "SimpleLayout"); }
            set { ViewState["AdvancedLayoutManager"] = value; }
        }

        /// <summary>
        /// This option should contain a comma separated list of formats that 
        /// will be available in the format drop down list. The default value 
        /// of this option is 
        /// "p,address,pre,h1,h2,h3,h4,h5,h6". 
        /// This option is only available if the advanced theme is used.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("p,address,pre,h1,h2,h3,h4,h5,h6")]
        public string AdvancedBlockFormats
        {
            get { return (ViewState["AdvancedBlockFormats"] != null ? (string)ViewState["AdvancedBlockFormats"] : "p,address,pre,h1,h2,h3,h4,h5,h6"); }
            set { ViewState["AdvancedBlockFormats"] = value; }
        }

        /// <summary>
        /// This option should contain a semicolon separated list of class 
        /// titles and class names separated by =. The titles will be 
        /// presented to the user in the styles dropdown list and the 
        /// class names will be inserted. If this option is not defined, 
        /// TinyMCE imports the classes from the content_css.
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/content_css
        /// 
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("")]
        public string AdvancedStyles
        {
            get { return (ViewState["AdvancedStyles"] != null ? (string)ViewState["AdvancedStyles"] : ""); }
            set { ViewState["AdvancedStyles"] = value; }
        }

        /// <summary>
        /// This option is used to define the width of the source editor 
        /// dialog. This option is set to 500 by default.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("500")]
        public string AdvancedSourceEditorWidth
        {
            get { return (ViewState["AdvancedSourceEditorWidth"] != null ? (string)ViewState["AdvancedSourceEditorWidth"] : "500"); }
            set { ViewState["AdvancedSourceEditorWidth"] = value; }
        }

        /// <summary>
        /// This option is used to define the height of the source editor 
        /// dialog. This option is set to 400 by default.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("400")]
        public string AdvancedSourceEditorHeight
        {
            get { return (ViewState["AdvancedSourceEditorHeight"] != null ? (string)ViewState["AdvancedSourceEditorHeight"] : "400"); }
            set { ViewState["AdvancedSourceEditorHeight"] = value; }
        }

        /// <summary>
        /// This option enables you to force word wrap for the source editor, 
        /// this option is set to true by default.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue(true)]
        public bool AdvancedSourceEditorWrap
        {
            get { return (ViewState["AdvancedSourceEditorWrap"] != null ? (bool)ViewState["AdvancedSourceEditorWrap"] : true); }
            set { ViewState["AdvancedSourceEditorWrap"] = value; }
        }

        /// <summary>
        /// This option enables you to specify where the toolbar should be located. 
        /// This option can be set to "top" or "bottom" (the default) 
        /// or "external". This option can only be used when theme is 
        /// set to advanced and when the theme_advanced_layout_manager 
        /// option is set to the default value of "SimpleLayout".
        /// 
        /// Choosing the "external" location adds the toolbar to a DIV 
        /// element and sets the class of this DIV to "mceToolbarExternal". 
        /// This enables you to freely specify the location of the toolbar.
        /// 
        /// In mojoPortal I made the default "top", which seems a better default
        /// to me.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("top")]
        public string AdvancedToolbarLocation
        {
            get { return (ViewState["AdvancedToolbarLocation"] != null ? (string)ViewState["AdvancedToolbarLocation"] : "top"); }
            set { ViewState["AdvancedToolbarLocation"] = value; }
        }

        /// <summary>
        /// This option enables you to specify the alignment of the toolbar, 
        /// this value can be "left", "right" or "center" (the default). 
        /// This option can only be used when theme is set to advanced and 
        /// when the theme_advanced_layout_manager option is set to the 
        /// default value of "SimpleLayout".
        /// 
        /// In mojoPortal I changed the default to left.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("left")]
        public string AdvancedToolbarAlign
        {
            get { return (ViewState["AdvancedToolbarAlign"] != null ? (string)ViewState["AdvancedToolbarAlign"] : "left"); }
            set { ViewState["AdvancedToolbarAlign"] = value; }
        }

        /// <summary>
        /// This option enables you to specify where the element statusbar 
        /// with the path and resize tool should be located. This option can 
        /// be set to "top", "bottom" or "none". The default value is set to 
        /// "none". This option can only be used when the theme is set to 
        /// "advanced" and when the theme_advanced_layout_manager option is 
        /// set to the default value of "SimpleLayout".
        /// 
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("bottom")]
        public string AdvancedStatusBarLocation
        {
            get { return (ViewState["AdvancedStatusBarLocation"] != null ? (string)ViewState["AdvancedStatusBarLocation"] : "bottom"); }
            set { ViewState["AdvancedStatusBarLocation"] = value; }
        }

        /// <summary>
        /// This option should contain a comma separated list of button/control 
        /// names to insert into the toolbar.  
        /// This property populates the first row of 3 allowed rows.
        /// Below is a list of built in 
        /// controls, plugins may include other controls names that can be 
        /// inserted but these are documented in the individual plugins. 
        /// This option can only be used when theme is set to advanced and when 
        /// the theme_advanced_layout_manager option is set to the default 
        /// value of "SimpleLayout".
        /// A list of possible options can be found here:
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("bold,italic,underline,strikethrough,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,formatselect,styleselect")]
        public string AdvancedRow1Buttons
        {
            get { return (ViewState["AdvancedRow1Buttons"] != null ? (string)ViewState["AdvancedRow1Buttons"] : "bold,italic,underline,strikethrough,separator,justifyleft,justifycenter,justifyright,justifyfull,separator,formatselect,styleselect"); }
            set { ViewState["AdvancedRow1Buttons"] = value; }
        }

        /// <summary>
        /// This option should contain a comma separated list of button/control 
        /// names to insert into the toolbar.  
        /// This property populates row 2 of 3 allowed rows.
        /// Below is a list of built in 
        /// controls, plugins may include other controls names that can be 
        /// inserted but these are documented in the individual plugins. 
        /// This option can only be used when theme is set to advanced and when 
        /// the theme_advanced_layout_manager option is set to the default 
        /// value of "SimpleLayout".
        /// A list of possible options can be found here:
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("bullist,numlist,separator,outdent,indent,separator,undo,redo,separator,link,unlink,anchor,image,cleanup,help,code")]
        public string AdvancedRow2Buttons
        {
            get { return (ViewState["AdvancedRow2Buttons"] != null ? (string)ViewState["AdvancedRow2Buttons"] : "bullist,numlist,separator,outdent,indent,separator,undo,redo,separator,link,unlink,anchor,image,cleanup,help,code"); }
            set { ViewState["AdvancedRow2Buttons"] = value; }
        }

        /// <summary>
        /// This option should contain a comma separated list of button/control 
        /// names to insert into the toolbar.  
        /// This property populates row 2 of 3 allowed rows.
        /// Below is a list of built in 
        /// controls, plugins may include other controls names that can be 
        /// inserted but these are documented in the individual plugins. 
        /// This option can only be used when theme is set to advanced and when 
        /// the theme_advanced_layout_manager option is set to the default 
        /// value of "SimpleLayout".
        /// A list of possible options can be found here:
        /// http://wiki.moxiecode.com/index.php/TinyMCE:Control_reference
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("hr,removeformat,visualaid,separator,sub,sup,separator,charmap")]
        public string AdvancedRow3Buttons
        {
            get { return (ViewState["AdvancedRow3Buttons"] != null ? (string)ViewState["AdvancedRow3Buttons"] : "hr,removeformat,visualaid,separator,sub,sup,separator,charmap"); }
            set { ViewState["AdvancedRow3Buttons"] = value; }
        }

        /// <summary>
        /// This option enables or disables the built-in clean up functionality. 
        /// TinyMCE is equipped with powerful clean up functionality that 
        /// enables you to specify what elements and attributes are allowed 
        /// and how HTML contents should be generated. This option is set to 
        /// true by default, but if you want to disable it you may set it to 
        /// false.
        /// Notice: It's not recommended to disable this feature.
        /// 
        /// It might be worth mentioning that the browser usually messes with the 
        /// HTML. The clean up not only fixes several problems with the 
        /// browsers' parsed HTML document, like paths etc., it also makes 
        /// sure it is a correct XHTML document, with all tags closed, the " 
        /// at the right places, and things like that.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue(true)]
        public bool Cleanup
        {
            get { return (ViewState["Cleanup"] != null ? (bool)ViewState["Cleanup"] : true); }
            set { ViewState["Cleanup"] = value; }
        }

        /// <summary>
        /// If you set this option to true, TinyMCE will perform a HTML cleanup 
        /// call when the editor loads. This option is set to false by default.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue(false)]
        public bool CleanupOnStart
        {
            get { return (ViewState["CleanupOnStart"] != null ? (bool)ViewState["CleanupOnStart"] : false); }
            set { ViewState["CleanupOnStart"] = value; }
        }

        public bool InlineStyles
        {
            get { return (ViewState["InlineStyles"] != null ? (bool)ViewState["InlineStyles"] : true); }
            set { ViewState["InlineStyles"] = value; }
        }

        /// <summary>
        /// This option enables you to specify a custom CSS file that extends 
        /// the theme content CSS. This CSS file is the one used within the 
        /// editor (the editable area). The default location of this CSS file 
        /// is within the current theme. This option can also be a comma 
        /// separated list of URLs.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("")]
        public string EditorAreaCSS
        {
            get { return (ViewState["EditorAreaCSS"] != null ? (string)ViewState["EditorAreaCSS"] : ""); }
            set { ViewState["EditorAreaCSS"] = value; }
        }

        public string EditorBodyCssClass { get; set; } = "wysiwygeditor modulecontent art-postcontent";

        #endregion

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            
            writer.Write(
                "<textarea id=\"{0}\" name=\"{1}\"  rows=\"4\" cols=\"40\" style=\"width: {2}; height: {3}\" >{4}</textarea>",
                    this.ClientID,
                    this.UniqueID,
                    this.Width,
                    this.Height,
                    System.Web.HttpUtility.HtmlEncode(this.Text));

                
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupScripts();
        }

        private void SetupScripts()
        {
            if (BasePath.Contains("tiny_mce"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                    "tinymcemain",
                    "<script data-loader=\"TinyMCE\" src=\""
                    + ResolveUrl(this.BasePath + "tiny_mce.js") + "\"></script>");
            }
            else
            {
                this.Page.ClientScript.RegisterClientScriptBlock(
                    this.GetType(),
                    "tinymcemain",
                    "<script data-loader=\"TinyMCE\" src=\""
                    + ResolveUrl(this.BasePath + "tinymce.min.js") + "\"></script>");

            }

            StringBuilder setupScript = new StringBuilder();

            setupScript.Append("function mojoTinyMCEOnChangeHandler(inst) {");
            //setupScript.Append("hookupGoodbyePrompt(\"" + Page.Server.HtmlEncode(Resource.UnSavedChangesPrompt) + "\"); ");
            setupScript.Append("hookupGoodbyePrompt(\"" + Resource.UnSavedChangesPrompt.HtmlEscapeQuotes().RemoveLineBreaks() + "\"); ");
            setupScript.Append("} ");

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                   "tinymceExitPrompt", "\n<script data-loader=\"TinyMCE\">\n"
				   + setupScript.ToString() + "\n</script>");

            setupScript = new StringBuilder();
            setupScript.Append("\n<script data-loader=\"TinyMCE\">");

            //this older approach did not work inside tabs
            // so we switched to use the editor constructor
            //setupScript.Append("tinyMCE.init({");
            //setupScript.Append("mode :\"specific_textareas\" ");
            //setupScript.Append(", editor_selector : \"mceEditor\" ");

            //if (BasePath.Contains("tiny_mce"))
            //{

               setupScript.Append(" var ed" + this.ClientID + " = new tinymce.Editor('" + this.ClientID + "', { ");
            //}
            //else
            //{
            //    setupScript.Append("tinyMCE.init({");
            //    setupScript.Append("selector: 'textarea#" + this.ClientID + "',");
            //    setupScript.Append("theme: 'modern',");
            //}
            
           
            setupScript.Append("accessibility_focus : true ");

            //waiting for the 3.5 branch of TinyMCE to comeout of beta
            // it has more support for html 5
            
            setupScript.Append(", schema :'" + WebConfigSettings.TinyMceSchema + "' ");
            //http://www.tinymce.com/tryit/html5_formats.php
           // setupScript.Append(",style_formats :[{title : 'section', block : 'section', wrapper: true, merge_siblings: false},");
           // setupScript.Append("{title : 'article', block : 'article', wrapper: true, merge_siblings: false},");
           // setupScript.Append("{title : 'aside', block : 'aside', wrapper: true},");
           // setupScript.Append("{title : 'figure', block : 'figure', wrapper: true}]");

            

            //if (!AccessibilityFocus) // true is default
            //{
            //    setupScript.Append(", accessibility_focus : false ");
            //}
            if (!AccessibilityWarnings)
            {
                setupScript.Append(", accessibility_warnings : false ");
            }

            setupScript.Append(", browsers : \"" + this.Browsers + "\"");

            setupScript.Append(",forced_root_block:'" + ForcedRootBlock + "'");

            if (!CustomShortcuts)
            {
                setupScript.Append(", custom_shortcuts : false ");
            }

            if (!ConvertUrls)
            {
                setupScript.Append(", convert_urls : false ");
            }

            // added 2009-12-15 previously TinyMCE was encoding German chars and this made it not find matches in search index
            setupScript.Append(", entity_encoding : 'raw' ");

            //setupScript.Append(",encoding :'xml'");

            setupScript.Append(", dialog_type : \"" + this.DialogType + "\"");

            CultureInfo culture;
            if (WebConfigSettings.UseCultureOverride)
            {
                culture = SiteUtils.GetDefaultUICulture();
            }
            else
            {
                culture = CultureInfo.CurrentUICulture;
            }

            

            setupScript.Append(",language:'" + culture.TwoLetterISOLanguageName + "'");

            if (culture.TextInfo.IsRightToLeft) { TextDirection = "rtl"; }

            if (ExtendedValidElements.Length > 0)
            {
                setupScript.Append(",extended_valid_elements:\"" + ExtendedValidElements + "\"");
            }

            //extended_valid_elements

            setupScript.Append(",directionality:\"" + this.TextDirection + "\"");

            setupScript.Append(",editor_deselector:\"" + this.DeSelectorCSSClass + "\"");

            
                
			if (EnableGeckoSpellCheck)
			{
				setupScript.Append(",gecko_spellcheck : true ");
			}
            

            //setupScript.Append(", language : \"" + this.Language + "\"");

            setupScript.Append(",body_class:'" + EditorBodyCssClass + "'");

            if (!EnableObjectResizing)
            {
                setupScript.Append(", object_resizing : false ");
            }

            if (Plugins.Length > 0)
            {
                setupScript.Append(", plugins : \"" + this.Plugins + "\"");
                if (Plugins.Contains("preview"))
                {
                    setupScript.Append(",plugin_preview_width:'850'");
                    setupScript.Append(",plugin_preview_height:'900'");

                }

                //TODO: populate from style templates for a element
                //http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/advlink
                //This option should contain a semicolon separated list of class titles and class names separated by =
                //advlink_styles : “Code=code;Excel=excel;Flash=flash;Sound=sound;Office=office;PDF=pdf;Image=image;PowerPoint=powerpoint;Word=word;Video=video”
                //advlink_styles

                //setupScript.Append(",media_strict:false");
            }

            //setupScript.Append(",apply_source_formatting:true");

            //if (UseStrictLoadingMode)
            //{
            //    setupScript.Append(", strict_loading_mode : true ");
            //}

            setupScript.Append(", theme : \"" + this.Theme + "\"");

            if (!EnableUndoRedo)
            {
                setupScript.Append(", custom_undo_redo : false ");
            }

            if (EnableUndoRedo)
            {
                setupScript.Append(", custom_undo_redo_levels : " + this.UnDoLevels.ToString());
            }

            if (Theme == "advanced")
            {
                setupScript.Append(", layout_manager : \"" + this.AdvancedLayoutManager + "\"");
                setupScript.Append(", theme_advanced_blockformats : \"" + this.AdvancedBlockFormats + "\"");
                if (AdvancedStyles.Length > 0)
                {
                    setupScript.Append(", theme_advanced_styles : \"" + this.AdvancedStyles + "\"");
                }

                setupScript.Append(", theme_advanced_source_editor_width : \"" + this.AdvancedSourceEditorWidth + "\"");
                setupScript.Append(", theme_advanced_source_editor_height : \"" + this.AdvancedSourceEditorHeight + "\"");
                if (!AdvancedSourceEditorWrap)
                {
                    setupScript.Append(", theme_advanced_source_editor_wrap : false ");
                }

                if (AdvancedLayoutManager == "SimpleLayout")
                {
                    setupScript.Append(", theme_advanced_toolbar_location : \"" + this.AdvancedToolbarLocation + "\"");
                    setupScript.Append(", theme_advanced_toolbar_align : \"" + this.AdvancedToolbarAlign + "\"");
                    setupScript.Append(", theme_advanced_statusbar_location : \"" + this.AdvancedStatusBarLocation + "\"");

                    setupScript.Append(", theme_advanced_buttons1 : \"" + this.AdvancedRow1Buttons + "\"");
                    setupScript.Append(", theme_advanced_buttons2 : \"" + this.AdvancedRow2Buttons + "\"");
                    setupScript.Append(", theme_advanced_buttons3 : \"" + this.AdvancedRow3Buttons + "\"");

                    //setupScript.Append(", theme_advanced_buttons1_add : \"pastetext,pasteword,selectall\"");
                   
                    setupScript.Append(",theme_advanced_resizing : true ");
                
                }

                //advanced theme has 2 skins default and o2k7
                // o2k7 also supports skin_variant with default, silver and black as options
                // so basically we have
                // default
                // o2k7default
                // o2k7silver
                // o2k7black
                switch (Skin)
                {
                    case "o2k7default":
                        setupScript.Append(",skin :'o2k7'");
                        break;

                    case "o2k7silver":
                        setupScript.Append(",skin :'o2k7'");
                        setupScript.Append(",skin_variant :'silver'");
                        break;

                    case "o2k7black":
                        setupScript.Append(",skin :'o2k7'");
                        setupScript.Append(",skin_variant :'black'");
                        break;


                    case "default":
                    default:
                        //do nothing
                        break;

                }

                if (TemplatesUrl.Length > 0)
                {
                    setupScript.Append(",template_external_list_url: \"" + this.TemplatesUrl + "\"");
                }

                if (!Cleanup)
                {
                    setupScript.Append(", cleanup : false ");
                }

                if (CleanupOnStart)
                {
                    setupScript.Append(", cleanup_on_startup : true ");
                }

                //setupScript.Append(",convert_newlines_to_brs : true ");

                if (!InlineStyles)
                {
                    setupScript.Append(", inline_styles : false ");
                }

                if (EditorAreaCSS.Length > 0)
                {
                    setupScript.Append(", content_css : \"" + this.EditorAreaCSS + "\"");
                }

                if (EmotionsBaseUrl.Length > 0)
                {
                    setupScript.Append(",emotions_images_url : '" + EmotionsBaseUrl + "'");
                }

                if (AutoFocus)
                {
                    setupScript.Append(", auto_focus : \"" + this.ClientID + "\" ");
                }

                if ((EnableFileBrowser)&&(FileManagerUrl.Length > 0))
                {
                    setupScript.Append(",file_browser_callback : 'myFileBrowser' ");
                }

                
                setupScript.Append(",onchange_callback : 'mojoTinyMCEOnChangeHandler' ");
                

                if (ForcePasteAsPlainText)
                {
                    //setupScript.Append(",oninit:'setPlainText'");
                    setupScript.Append(",paste_text_sticky:true");
                    setupScript.Append(",setup : function(ed) {");
                    setupScript.Append("ed.onInit.add(function(ed) {");
                    setupScript.Append("ed.pasteAsPlainText = true;");
                    setupScript.Append("});}");

                }

            }

            setupScript.Append("}); ");

            if (Plugins.Contains("fullpage"))
            {
                StringBuilder supScript = new StringBuilder();
                supScript.Append("\n<script data-loader=\"TinyMCE\">");
                supScript.Append("var editorContentFilter = {");
                supScript.Append("setContent: function(originalContent) {");
                supScript.Append(" var headMatch = originalContent.match(/<head>(.|\\n|\\r)*?<\\/head>/i);");
                supScript.Append("if (headMatch) {");
                supScript.Append("var headText = headMatch[0];");
                supScript.Append("var stylesMatch = headText.match(/<style(.|\\n|\\r)*?>(.|\\n|\\r)*?<\\/style>/ig);");
                supScript.Append("if (stylesMatch) {");
                supScript.Append("var styleText = \"\";");
                supScript.Append("for (var i = 0; i < stylesMatch.length; i++) {");
                supScript.Append("styleText += stylesMatch[i];");
                supScript.Append("}");
                supScript.Append("originalContent = originalContent.replace(/<body((.|\\n|\\r)*?)>/gi, \"<body$1><!--style begin-->\" + styleText + \"<!--style end-->\");");
                supScript.Append("}");
                supScript.Append("}");
                supScript.Append("return originalContent;");
                supScript.Append("},");
                supScript.Append("getContent: function(originalContent) {");
                supScript.Append(" return originalContent.replace(/<!--style begin-->(.|\\n|\\r)*?<!--style end-->/g, '');");
                supScript.Append("}");
                supScript.Append("};");

                supScript.Append("</script>");

                this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "tmcfpfix",
                supScript.ToString());

                setupScript.Append("ed" + this.ClientID + ".onBeforeSetContent.add(function(ed, o) {");
                setupScript.Append("o.content = editorContentFilter.setContent(o.content);");
                setupScript.Append(" });");

                setupScript.Append("ed" + this.ClientID + ".onGetContent.add(function(ed, o) {");
                setupScript.Append("o.content = editorContentFilter.getContent(o.content);");
                setupScript.Append(" });");

            }

            setupScript.Append("ed" + this.ClientID + ".render(); ");

            //if (forcePasteAsPlainText)
            //{
            //    setupScript.Append("ed" + this.ClientID + ".onPaste.add( function(ed, e, o){ ");
            //    setupScript.Append("ed.execCommand('mcePasteText', true); ");
            //    setupScript.Append("ed.execCommand('mceAddUndoLevel');");
            //    setupScript.Append("return tinymce.dom.Event.cancel(e); ");
            //    setupScript.Append("});");

            //}

            

            setupScript.Append("</script>");
            
            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(), 
                this.UniqueID, 
                setupScript.ToString());


            if ((EnableFileBrowser)&&(FileManagerUrl.Length > 0)) { SetupFileBrowserScript(); }

            //string submitScript = " tinyMCE.triggerSave(false,true); ";
            ////submitScript = " alert(tinyMCE.getContent('" + this.ClientID + "')); return false; ";

            //this.Page.ClientScript.RegisterOnSubmitStatement(
            //    this.GetType(), "tmsubmit", submitScript);


        }

        private void SetupFileBrowserScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            //string fileManagerUrl = SiteUtils.GetNavigationSiteRoot() + "/Dialog/MCEFileDialog.aspx";
            
            script.Append("var serviceUrl = '" + FileManagerUrl + "'; ");
            script.Append("function myFileBrowser (field_name, url, type, win) {");

            //script.Append("alert(type);");

            script.Append("tinyMCE.activeEditor.windowManager.open({");
            script.Append("file : serviceUrl + '?ed=tmc&type=' + type, ");
            script.Append("title : '" + Resource.FileBrowser.HtmlEscapeQuotes() + "', ");
            script.Append("width : 860,");
            script.Append("height : 700,");
            script.Append("resizable : 'yes',");
            script.Append("inline : 'yes',");
            script.Append(" close_previous : 'no'");
            script.Append("}, {");
            script.Append("window : win,");
            script.Append("input : field_name");
            script.Append("}); ");
            script.Append("return false;");


            script.Append("}");
            script.Append("\n</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(
                typeof(Page),
                "tmcfb",
                script.ToString());

        }


        #region Postback Handling

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            if (postCollection[postDataKey] == null) { return false; }

            string newContent = postCollection[postDataKey];
            if (newContent != this.Text)
            {
                this.Text = newContent;
                return true;
            }
            
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            // Do nothing
        }

        #endregion

    }

}
