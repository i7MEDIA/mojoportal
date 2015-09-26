//	Created:			    2009-08-07
//	Last Modified:		    2010-12-26
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
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// The purpose of this control is to try and support easy use of Artisteer html designs
    /// by adding settings that allow additional unsemantic divs to be added to support Artisteer.
    /// by default no extra markup will be applied. The idea will be to use theme.skin to enable a RenderArtisteer property to trigger the extra markup
    /// by default this control will only render its contents
    /// 
    /// </summary>
    public class mojoPanel : Panel
    {
        private string columnId = UIHelper.CenterColumnId;
        private string overrideTag = string.Empty;
        private string overrideCss = string.Empty;

        private bool renderArtisteer = false;
        private bool useLowerCaseArtisteerClasses = false;

        public bool RenderArtisteer
        {
            get { return renderArtisteer; }
            set { renderArtisteer = value; }
        }

        private bool renderArtisteerBlockContentDivs = false;

        public bool RenderArtisteerBlockContentDivs
        {
            get { return renderArtisteerBlockContentDivs; }
            set { renderArtisteerBlockContentDivs = value; }
        }

        public bool UseLowerCaseArtisteerClasses
        {
            get { return useLowerCaseArtisteerClasses; }
            set { useLowerCaseArtisteerClasses = value; }
        }

        private string artisteerCssClass = UIHelper.ArtisteerPost;

        public string ArtisteerCssClass
        {
            get { return artisteerCssClass; }
            set { artisteerCssClass = value; }
        }

        /// <summary>
        /// Specify a tag to render as, expected values are the new html 5 elements header footer nav article section
        /// </summary>
        public string OverrideTag
        {
            get { return overrideTag; }
            set { overrideTag = value; }
        }

        /// <summary>
        /// If you need to use different css for the overridden tag than for the non overriden div specify it here.
        /// </summary>
        public string OverrideCss
        {
            get { return overrideCss; }
            set { overrideCss = value; }
        }

        private bool useJQueryUI = false;

        public bool UseJQueryUI
        {
            get { return useJQueryUI; }
            set { useJQueryUI = value; }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (renderArtisteer) 
            { 
                columnId = this.GetColumnId(); 
            
                switch (columnId)
                {
                    case UIHelper.LeftColumnId:
                    case UIHelper.RightColumnId:

                        if (useLowerCaseArtisteerClasses)
                        {
                            if ((artisteerCssClass == UIHelper.ArtisteerPostLower)|(artisteerCssClass == UIHelper.ArtisteerPost))
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlockLower;
                                renderArtisteerBlockContentDivs = true;
                            }

                            if ((artisteerCssClass == UIHelper.ArtisteerPostContentLower)||(artisteerCssClass == UIHelper.ArtisteerPostContent))
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlockContentLower;
                                renderArtisteerBlockContentDivs = true;
                            }
                        }
                        else
                        {
                            if (artisteerCssClass == UIHelper.ArtisteerPost)
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlock;
                                renderArtisteerBlockContentDivs = true;
                            }

                            if (artisteerCssClass == UIHelper.ArtisteerPostContent)
                            {
                                artisteerCssClass = UIHelper.ArtisteerBlockContent;
                                renderArtisteerBlockContentDivs = true;
                            }
                        }

                        break;

                    case UIHelper.CenterColumnId:
                    default:
                        if (useLowerCaseArtisteerClasses) 
                        {
                            if (artisteerCssClass == UIHelper.ArtisteerPost)
                            {
                                artisteerCssClass = UIHelper.ArtisteerPostLower;
                            }

                            // added for Artisteer 3.0
                            if (artisteerCssClass == UIHelper.ArtisteerPostContent)
                            {
                                artisteerCssClass = UIHelper.ArtisteerPostContentLower;

                            }
                        }

                        break;

                }
            }

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }


            if (renderArtisteer)
            {
                writer.Write("<div class='" + artisteerCssClass + "'>\n");
                
                if (renderArtisteerBlockContentDivs)
                {
                    writer.Write("<div class=\"" + artisteerCssClass + "-tl\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-tr\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-bl\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-br\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-tc\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-bc\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-cl\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-cr\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-cc\"></div>\n");
                    writer.Write("<div class=\"" + artisteerCssClass + "-body\">");
                }

                base.RenderContents(writer);

                if (renderArtisteerBlockContentDivs)
                {
                    writer.Write("\n</div>");
                    writer.Write("<div class=\"cleared\"></div>");
                }
                writer.Write("\n</div>");
                return;

            }

            if (useJQueryUI) 
            {
                if (artisteerCssClass == UIHelper.ArtisteerPostContent)
                {
                    CssClass = "ui-widget-content ui-corner-bottom";
                }
                else
                {
                    CssClass = "ui-widget";
                }
                base.Render(writer);
                return;
            }


            if (overrideCss.Length > 0) { CssClass = overrideCss; }

            if ((overrideTag.Length > 0))
            {
                writer.Write("<" + overrideTag + " class='" + CssClass + "'>\n");

                base.RenderContents(writer);

                writer.Write("\n</" + overrideTag + ">");
                return;
            }

            base.RenderContents(writer);
            return;


        }


    }
}
