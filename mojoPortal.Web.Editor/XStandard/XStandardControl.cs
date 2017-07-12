// Author:					
// Created:				2007-08-26
// Last Modified:		    2009-05-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Permissions;
using log4net;

namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// XStandard Editor wrapper control
    /// </summary>
    [DefaultProperty("Value")]
    [ValidationProperty("Value")]
    [ToolboxData("<{0}:XStandard runat=server></{0}:XStandard>")]
    [Designer("mojoPortal.Web.Editor.XStandardDesigner")]
    [ParseChildren(false)]
    public class XStandardControl : Control, IPostBackDataHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(XStandardControl));

        public XStandardControl() { }

        #region Properties

        [DefaultValue("")]
        public string Text
        {
            get 
            { 
                object o = ViewState["Text"]; 
                if(o != null)return (string)o;
                string result = string.Empty;

                if(HttpContext.Current != null)
                {
                    result = HttpContext.Current.Request.Form[this.ClientID];
                    if (string.IsNullOrEmpty(result))
                    {
                        result = HttpContext.Current.Request.Form[this.ClientID + "xhtml"];
                    }
                }

                return result;
            }
            
            set { ViewState["Text"] = value; }
        }

        #region Appearence Properties

        [Category("Appearence")]
        [DefaultValue("100%")]
        public Unit Width
        {
            get { object o = ViewState["Width"]; return (o == null ? Unit.Percentage(100) : (Unit)o); }
            set { ViewState["Width"] = value; }
        }

        [Category("Appearence")]
        [DefaultValue("200px")]
        public Unit Height
        {
            get { object o = ViewState["Height"]; return (o == null ? Unit.Pixel(200) : (Unit)o); }
            set { ViewState["Height"] = value; }
        }

        #endregion

        #region Configuration Properties

        [DefaultValue("/ClientScript/XStandard/")]
        public string BasePath
        {
            get
            {
                object o = ViewState["BasePath"];

                if (o == null)
                    o = ConfigurationManager.AppSettings["XStandard:BasePath"];

                return (o == null ? "/ClientScript/XStandard/" : (string)o);
            }
            set { ViewState["BasePath"] = value; }
        }

        /// <summary>
        /// This option enables you to auto focus an editor instance. The 
        /// value of this option should be an editor instance id. Editor 
        /// instance ids are specified as "mce_editor_index", where 
        /// index is a value starting from 0. So if there are 3 editor 
        /// instances on a page, they would have the following 
        /// ids - mce_editor_0, mce_editor_1 and mce_editor_2.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue(false)]
        public bool AutoFocus
        {
            get { return (ViewState["AutoFocus"] != null ? (bool)ViewState["AutoFocus"] : false); }
            set { ViewState["AutoFocus"] = value; }
        }

        /// <summary>
        /// This option specifies the default writing direction, some languages 
        /// (Like Hebrew, Arabic, Urdu...) write from right to left instead 
        /// of left to right. The default value of this option is "ltr" but 
        /// if you want to use from right to left mode specify "rtl" instead.
        /// </summary>
        [Bindable(false), Category("Behavior"), DefaultValue("ltr")]
        public string TextDirection
        {
            get { return (ViewState["TextDirection"] != null ? (string)ViewState["TextDirection"] : "ltr"); }
            set { ViewState["TextDirection"] = value; }
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

        #endregion

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page.IsPostBack)
            {
                string result = HttpContext.Current.Request.Form[this.ClientID];
                if (string.IsNullOrEmpty(result))
                {
                    result = HttpContext.Current.Request.Form[this.ClientID + "xhtml"];
                }

                if (string.IsNullOrEmpty(result))
                {
                    return;
                }

                Text = result;
                


            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (
                (HttpContext.Current == null)
                || (this.Site != null && this.Site.DesignMode)
                )
            {
                // render to the designer
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                writer.Write("<object ");
                // http://xstandard.com/en/support/ie-7-auto-install-workaround/
                // http://support.microsoft.com/?id=934014
                if (HttpContext.Current.Request.Browser.Browser.ToLower().Contains("msie 7"))
                {
                    writer.Write("classid=\"clsid:0EED7206-1661-11D7-84A3-00606744831D\" ");
                }
                else
                {
                    writer.Write("type=\"application/x-xstandard\" ");
                    
                }

                writer.Write("id=\"" + this.ClientID + "\" ");
                writer.Write("name=\"" + this.ClientID + "\" ");
                writer.Write("width=\"100%\" ");
                writer.Write("height=\"" + this.Height.ToString() + "\" ");
                writer.Write("codebase=\"" + ResolveUrl(this.BasePath) 
                    + "XStandard.cab#Version=2,0,0,0\"");
                writer.Write("> ");

                string currentValue = HttpContext.Current.Server.HtmlEncode(this.Text);

                writer.Write("\n<param name=\"Value\" ");
                writer.Write("value=\""
                    + currentValue
                    + "\" ></param>");

                // in October 2007 when new version of XStandard is released
                // can use mojoCMSCode
                //string mojoCMSCode = "F9C8F756-4E92-4204-837D-ECDAA8868E3B";
                string devCMSCode = "993F60E0-753D-4DA1-8257-D37CF5ED7DDB";

                writer.Write("\n<param name=\"CMSCode\" ");
                writer.Write("value=\""
                    + devCMSCode
                    + "\" ></param>");

                // TODO: additional features
                // http://xstandard.com/en/documentation/xstandard-dev-guide/api/

                // must be absolute url
                //<param name="CMSImageLibraryURL" value="http://website.com/images/cmsimagelibrary.php" />

                //<param name="CMSAttachmentLibraryURL" value="http://website.com/docs/cmsattachmentlibrary.php" />

                // markup snippet library
                // http://xstandard.com/en/programs/xstandard-lite-for-partner-cms/directory/
                //<param name="CMSDirectoryURL" value="http://website.com/cmsdirectory.php" />

                writer.Write("\n<param name=\"EnablePasteMarkup\" value=\"yes\"></param>");

                // When pasting content into the editor containing images 
                // that point to the local hard drive from applications such 
                // as Word, the editor tries to upload these images to the 
                // server. Since the file upload feature is only available 
                // in the Pro version of XStandard and to avoid displaying 
                // a message box to the user to this effect
                writer.Write("\n<param name=\"Options\" value=\"32768\" ></param>");

                writer.Write("\n<param name=\"EnableTimestamp\" value=\"no\" ></param>");
                writer.Write("\n<param name=\"Dir\" value=\"" + this.TextDirection + "\" ></param>");

                //writer.Write("<param name=\"EditorCSS \" value=\"" + this.EditorAreaCSS + "\" />");
                writer.Write("\n<param name=\"CSS \" value=\"" + this.EditorAreaCSS + "\" ></param>");

                writer.Write("\n<textarea name=\""
                    + this.ClientID + "alternate\" ");
                writer.Write(" id=\"" + this.ClientID + "alternate\" ");
                writer.Write(" cols=\"60\" rows=\"15\" > ");

                writer.Write(currentValue);
                //writer.Write(this.Text);

                writer.Write("\n</textarea> ");

                writer.Write("\n<span class='txterror'>If you do not see the editor here, there are three possible reasons. (1) You have not yet installed XStandard to your computer. Before proceeding, please download and install XStandard. (2) You need to quit and restart your Web browser. (3) You have not enabled 'add-ons' (plug-ins) to run in your browser. Any questions? Please contact us at support@xstandard.com</span>");

                writer.Write("\n</object> ");
                writer.Write("\n<input type=\"hidden\" ");
                writer.Write("name=\"" + this.ClientID + "xhtml\" ");
                writer.Write("id=\"" + this.ClientID + "xhtml\" ");
                writer.Write(" />");
                writer.Write(" ");

            }


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

       
        #region Postback Handling

        public bool LoadPostData(
            string postDataKey, 
            NameValueCollection postCollection)
        {
            // for some reason we don't hit this event
            // so we get the posted data during  load

            if (
                (postCollection[postDataKey] != null)
                && (postCollection[postDataKey] != this.Text)
                )
            {
                this.Text = postCollection[postDataKey];
                return true;
            }

            if (this.ID != null)
            {
                String noScriptKey = postDataKey.Replace(this.ID, "xhtml");
                if ((postCollection[noScriptKey] != null)
                    && (postCollection[noScriptKey] != this.Text)
                    )
                {
                    this.Text = postCollection[noScriptKey];
                    return true;
                }
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
