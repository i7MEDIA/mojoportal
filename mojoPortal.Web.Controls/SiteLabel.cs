using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    ///	Author:				    Joe Audette
    ///	Created:			    2006-12-13
    ///	Last Modified:		    2008-08-15
    /// 
    /// 02/12/2007  Alexander Yushchenko: refactoring to simplify control logic.
    ///	2007/04/16 JA fixed bugs introduced in refactoring, 
    /// was not resolving booleans correctly with tryparse
    /// and once that was fixed was showing config key instead of text 
    /// when UseLabelTag = false
    /// 
    /// This control is really just designed for use in mojoPortal
    /// and external projects that plug into mojoPortal.
    /// 
    /// 2007-11-26 Joe Audette - change to render directly to output without unneccesary string creation
    /// 2008-08-15 Joe Audette removed use of viewstate
    /// </summary>
    public class SiteLabel : WebControl, INamingContainer
    {
        #region Public Properties

        private string resourceFile = "Resource";
        private string configKey = string.Empty;
        private bool useLabelTag = true;
        private string forControl = string.Empty;
        private bool showWarningOnMissingKey = true;

        public string ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

        public string ConfigKey
        {
            get { return configKey; }
            set { configKey = value; }
        }

        public bool UseLabelTag
        {
            get { return useLabelTag; }
            set { useLabelTag = value; }
        }

        public string ForControl
        {
            get { return forControl; }
            set { forControl = value; }
        }

        public bool ShowWarningOnMissingKey
        {
            get { return showWarningOnMissingKey; }
            set { showWarningOnMissingKey = value; }
        }

       

        

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnableViewState = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            DoRender(writer);
        }

        private void DoRender(HtmlTextWriter writer)
        {
            if (String.IsNullOrEmpty(ConfigKey)) return;

            if (UseLabelTag)
            {
                RenderLabelElement(writer);
                return;
            }

            if (CssClass.Length > 0)
            {
                RenderSpanElement(writer);
                return;
            }

            RenderRaw(writer);
        }

        private void RenderLabelElement(HtmlTextWriter writer)
        {
            
            writer.WriteBeginTag("label");
            
            if (ForControl.Length > 0)
            {
                Control c = NamingContainer.FindControl(ForControl);
                if (c != null)
                {
                    writer.WriteAttribute("for", c.ClientID);
                }

            }

            if (CssClass.Length > 0)
            {

                writer.WriteAttribute("class", CssClass);
            }
            writer.Write(HtmlTextWriter.TagRightChar);

            if ((ConfigKey != "EmptyLabel") && (ConfigKey != "spacer"))
            {
                string text = HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) as string;
                if (text == null)
                {
                    text = ShowWarningOnMissingKey
                               ? string.Format("{0} not found in {1}.resx file", ConfigKey, ResourceFile)
                               : ConfigKey;
                }
                // should we be html encoding here?
                writer.WriteEncodedText(text);

            }

            writer.WriteEndTag("label");
            

        }

        private void RenderSpanElement(HtmlTextWriter writer)
        {
            writer.WriteBeginTag("span");

            writer.WriteAttribute("class", CssClass);
            
            writer.Write(HtmlTextWriter.TagRightChar);

            if ((ConfigKey != "EmptyLabel") && (ConfigKey != "spacer"))
            {
                string text = HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) as string;
                if (text == null)
                {
                    text = ShowWarningOnMissingKey
                               ? string.Format("{0} not found in {1}.resx file", ConfigKey, ResourceFile)
                               : ConfigKey;
                }

                // should we be html encoding here?
                writer.WriteEncodedText(text);

            }

            writer.WriteEndTag("span");

        }

        private void RenderRaw(HtmlTextWriter writer)
        {
            if ((ConfigKey != "EmptyLabel") && (ConfigKey != "spacer"))
            {
                string text = HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) as string;
                if (text == null)
                {
                    text = ShowWarningOnMissingKey
                               ? string.Format("{0} not found in {1}.resx file", ConfigKey, ResourceFile)
                               : ConfigKey;
                }
                // should we be html encoding here?
                writer.WriteEncodedText(text);
            }
        }

        private string GetControlMarkup()
        {
            if (String.IsNullOrEmpty(ConfigKey)) return String.Empty;

            string text;
            if ((ConfigKey == "EmptyLabel") || (ConfigKey == "spacer"))
            {
                text = String.Empty;
            }
            else
            {
                text = HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) as string;
                if (text == null)
                {
                    text = ShowWarningOnMissingKey
                               ? string.Format("{0} not found in {1}.resx file", ConfigKey, ResourceFile)
                               : ConfigKey;
                }
            }

            string forString = string.Empty;
            if (ForControl.Length > 0)
            {
                Control c = NamingContainer.FindControl(ForControl);
                if (c != null)
                {
                    forString = " for='" + c.ClientID + "' ";
                }

            }


            if (CssClass.Length > 0)
            {
                return UseLabelTag
                           ? string.Format("<label " + forString + " class='{0}'>{1}</label>", CssClass, text)
                           : string.Format("<span class='{0}'>{1}</span>", CssClass, text);
            }
            else
            {
                return UseLabelTag
                           ? string.Format("<label " + forString + ">{0}</label>", text)
                           : text;
            }
        }
    }
}
