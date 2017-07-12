// Author:					
// Created:					2011-03-14
// Last Modified:			2011-12-07
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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// designed for flexible rendering of headings similar to ModuleTitleControl but for other palces where there is no module involved but the same rendering is desired
    /// configurable from theme.skin
    /// </summary>
    public class HeadingControl : WebControl
    {
        //private string artHeader = UIHelper.ArtisteerPostMetaHeader;
        //private string artHeadingCss = UIHelper.ArtPostHeader;

        private string text = string.Empty;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private string headingTag = "h2";

        public string HeadingTag
        {
            get { return headingTag; }
            set { headingTag = value; }
        }

        private string literalExtraMarkup = string.Empty;

        public string LiteralExtraMarkup
        {
            get { return literalExtraMarkup; }
            set { literalExtraMarkup = value; }
        }

        #region deprecated properties
        // not used only kept here to avoid errors because some theme.skin fioles may refer to these old properties

        private bool renderArtisteer = false;

        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        private bool useLowerCaseArtisteerClasses = false;

        public bool UseLowerCaseArtisteerClasses
        {
            get { return useLowerCaseArtisteerClasses; }
            set { useLowerCaseArtisteerClasses = value; }
        }

        private bool useArtisteer3 = false;

        public bool UseArtisteer3
        {
            get { return useArtisteer3; }
            set { useArtisteer3 = value; }
        }

        private bool useJQueryUI = false;

        public bool UseJQueryUI
        {
            get { return useJQueryUI; }
            set { useJQueryUI = value; }

        }

        #endregion


        private string literalExtraTopContent = string.Empty;

        public string LiteralExtraTopContent
        {
            get { return literalExtraTopContent; }
            set { literalExtraTopContent = value; }
        }

        private string literalExtraBottomContent = string.Empty;

        public string LiteralExtraBottomContent
        {
            get { return literalExtraBottomContent; }
            set { literalExtraBottomContent = value; }
        }


        private string extraCssClasses = string.Empty;

        public string ExtraCssClasses
        {
            get { return extraCssClasses; }
            set { extraCssClasses = value; }
        }

        private string literalHeadingTopWrap = string.Empty;

        public string LiteralHeadingTopWrap
        {
            get { return literalHeadingTopWrap; }
            set { literalHeadingTopWrap = value; }
        }

        private string literalHeadingBottomWrap = string.Empty;

        public string LiteralHeadingBottomWrap
        {
            get { return literalHeadingBottomWrap; }
            set { literalHeadingBottomWrap = value; }
        }

        

        protected override void Render(HtmlTextWriter writer)
        {

            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }
            
            //if (renderArtisteer)
            //{
            //    if (useLowerCaseArtisteerClasses)
            //    {
            //        artHeader = UIHelper.ArtisteerPostMetaHeaderLower;
            //        artHeadingCss = UIHelper.ArtPostHeaderLower;

            //    }

            //    writer.Write("<div class=\"" + artHeader + "\">\n");

            //    if ((artHeader == UIHelper.ArtisteerBlockHeader) || (artHeader == UIHelper.ArtisteerBlockHeaderLower))
            //    {
            //        writer.Write("<div class=\"l\"></div>");
            //        writer.Write("<div class=\"r\"></div>");
            //        writer.Write("<div class=\"art-header-tag-icon\">");
            //        if (!useArtisteer3) { writer.Write("<div class=\"t\">"); }
            //    }

            //}
            //else if (useJQueryUI) 
            //{
            //    writer.Write("<div class=\"ui-widget-header ui-corner-top\">");
            //}

            if (literalExtraTopContent.Length > 0)
            {
                writer.Write(literalExtraTopContent);
            }

            if (headingTag.Length > 0)
            {
                writer.WriteBeginTag(headingTag);
                //if (useArtisteer3)
                //{
                //    writer.WriteAttribute("class", artHeadingCss + " t moduletitle " + CssClass);
                //}
                //else
                //{
                //    writer.WriteAttribute("class", artHeadingCss + " moduletitle " + CssClass);
                //}

                writer.WriteAttribute("class", extraCssClasses + " moduletitle " + CssClass);

                writer.Write(HtmlTextWriter.TagRightChar);

                if (literalHeadingTopWrap.Length > 0)
                {
                    writer.Write(literalHeadingTopWrap);
                }

                writer.Write(text);

                if (literalHeadingBottomWrap.Length > 0)
                {
                    writer.Write(literalHeadingBottomWrap);
                }

                if (literalExtraMarkup.Length > 0)
                {
                    writer.Write(literalExtraMarkup);
                }

                writer.WriteEndTag(headingTag);

            }

                
            //if (renderArtisteer)
            //{
            //    writer.Write("</div>");
            //    if ((artHeader == UIHelper.ArtisteerBlockHeader) || (artHeader == UIHelper.ArtisteerBlockHeaderLower))
            //    {
            //        writer.Write("</div>");
            //        if (!useArtisteer3) { writer.Write("</div>"); }
            //    }
            //}
            //else if (useJQueryUI)
            //{
            //    writer.Write("</div>");
            //}

            if (literalExtraBottomContent.Length > 0)
            {
                writer.Write(literalExtraBottomContent);
            }

            
        }
    }
}