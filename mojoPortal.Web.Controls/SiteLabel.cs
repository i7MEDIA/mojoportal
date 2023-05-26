///	Author:				    
///	Created:			    2006-12-13
///	Last Modified:		    2019-01-14
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
/// 2007-11-26  - change to render directly to output without unneccesary string creation
/// 2008-08-15  removed use of viewstate

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
	[ParseChildren(false)]
	[PersistChildren(true)]
    public class SiteLabel : WebControl, INamingContainer
    {

		#region Public Properties

		public string ResourceFile { get; set; } = "Resource";
		public string ConfigKey { get; set; } = string.Empty;
		public bool UseLabelTag { get; set; } = true;
		public string ForControl { get; set; } = string.Empty;
		public bool ShowWarningOnMissingKey { get; set; } = true;
		//public string Format { get; set; } = "{0}";


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
			if (string.IsNullOrWhiteSpace(ConfigKey)) return;
			//DoRender(writer);
			writer.Write(GetControlMarkup());
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
				if (!(HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) is string text))
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
				if (!(HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) is string text))
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
				if (!(HttpContext.GetGlobalResourceObject(ResourceFile, ConfigKey) is string text))
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

            List<string> attribs = new List<string>();

			foreach (string key in Attributes.Keys)
			{
				attribs.Add($"{key}=\"{Attributes[key]}\"");
			}

            var attribsString = string.Join(" ", attribs);
            if (!string.IsNullOrWhiteSpace(attribsString))
            {
                attribsString = $" {attribsString}";
            }

            var cssString = string.Empty;
            if (!string.IsNullOrWhiteSpace(CssClass))
            {
                cssString = $" class=\"{CssClass}\"";
            }
            return UseLabelTag
                ? $"<label {forString}{cssString}{attribsString}>{text}</label>"
                : string.IsNullOrWhiteSpace(CssClass) ? text : $"<span{cssString}{attribsString}>{text}</span>";
        }
    }
}
