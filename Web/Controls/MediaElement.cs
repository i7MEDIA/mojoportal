// Author:					
// Created:				    2012-09-14
// Last Modified:			2012-09-19
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using System.Web.Hosting;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a wrapper control that renders the markup for http://mediaelementjs.com/
    /// using either the html 4 audio or video element based on the file type
    /// if a non audio or video file is used it just renders as a link
    /// </summary>
    public class MediaElement : Panel
    {
        private string fileUrl = string.Empty;
        [Themeable(false)]
        public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }

        private string fileType = string.Empty;
        [Themeable(false)]
        public string FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }

        private string posterUrl = string.Empty;
        [Themeable(false)]
        public string PosterUrl
        {
            get { return posterUrl; }
            set { posterUrl = value; }
        }

        private string title = string.Empty;
        [Themeable(false)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private bool includeNonHtml5Fallback = true;

        public bool IncludeNonHtml5Fallback
        {
            get { return includeNonHtml5Fallback; }
            set { includeNonHtml5Fallback = value; }
        }

        private string overrideClientId = string.Empty;

        [Themeable(false)]
        public string OverrideClientId
        {
            get { return overrideClientId; }
            set { overrideClientId = value; }
        }

        private bool renderId = false;
        public bool RenderId
        {
            get { return renderId; }
            set { renderId = value; }
        }

        private bool useControls = true;
        public bool UseControls
        {
            get { return useControls; }
            set { useControls = value; }
        }

        private bool autoPlay = false;
        public bool AutoPlay
        {
            get { return autoPlay; }
            set { autoPlay = value; }
        }

        private bool loop = false;
        public bool Loop
        {
            get { return loop; }
            set { loop = value; }
        }

        private string preload = "metadata";
        public string Preload
        {
            get { return preload; }
            set { preload = value; }
        }

        private string mediaElementBasePath = "~/ClientScript/mediaelement2-13-1/";

        public string MediaElementBasePath
        {
            get { return mediaElementBasePath; }
            set { mediaElementBasePath = value; }
        }

        private bool addInitScript = false;

        public bool AddInitScript
        {
            get { return addInitScript; }
            set { addInitScript = value; }
        }

        private bool autoDetectFileType = true;
        public bool AutoDetectFileType
        {
            get { return autoDetectFileType; }
            set { autoDetectFileType = value; }
        }

        private bool includeDownloadLinkForMedia = true;
        public bool IncludeDownloadLinkForMedia
        {
            get { return includeDownloadLinkForMedia; }
            set { includeDownloadLinkForMedia = value; }
        }

        private string downloadLinkText = string.Empty;

        public string DownloadLinkText
        {
            get { return downloadLinkText; }
            set { downloadLinkText = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if ((!fileType.StartsWith("audio")) && (!fileType.StartsWith("video"))) { return; }

            if (Page is mojoBasePage)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                basePage.ScriptConfig.IncludeMediaElement = true;
                basePage.StyleCombiner.IncludeMediaElement = true;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (fileUrl.Length == 0) { return; }
            if (fileType.Length == 0) 
            {
                if (!autoDetectFileType) { return; }
                fileType = fileUrl.ToMimeType();
            }

            if (downloadLinkText.Length == 0)
            {
                downloadLinkText = Resource.Download;
            }

            if (!addInitScript) { return; }

            if ((!fileType.StartsWith("audio")) && (!fileType.StartsWith("video"))) { return; }

            //if (Page is mojoBasePage)
            //{
            //    mojoBasePage basePage = Page as mojoBasePage;
            //    basePage.ScriptConfig.IncludeMediaElement = true;
            //    basePage.StyleCombiner.IncludeMediaElement = true;
            //}

            SetupInitScript();
        }

        private void SetupInitScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(document).ready(function(){");

            script.Append("$('audio,video').mediaelementplayer( {");
            script.Append("audioWidth: '100%',");
            script.Append("audioHeight: 30,");
            script.Append("loop: false,");
            script.Append("plugins: ['silverlight','flash'],");
            script.Append("pluginPath: '" + Page.ResolveUrl(mediaElementBasePath) + "',");
            script.Append("flashName: 'flashmediaelement.swf',");
            script.Append("silverlightName: 'silverlightmediaelement.xap',");
            script.Append("enableAutosize: false");
            script.Append("});");

            script.Append("}); ");

            script.Append("</script>");

            ScriptManager.RegisterStartupScript(
                this,
                typeof(Page),
                "mediaelementinit",
                script.ToString(),
                false);

        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (fileUrl.Length == 0) { return; }
            if (fileType.Length == 0) { return; }

            if (fileType.StartsWith("audio"))
            {
                RenderAudio(writer);
            }
            else if (fileType.StartsWith("video"))
            {
                RenderVideo(writer);
            }
            else
            {
                RenderLink(writer);
            }


        }

        private void RenderAudio(HtmlTextWriter writer)
        {
            writer.Write("<audio ");
            if (renderId)
            {
                if (overrideClientId.Length > 0)
                {
                    writer.Write("id='" + overrideClientId + "' ");
                }
                else
                {
                    writer.Write("id='" + ClientID + "' ");
                }
            }

            if (Height != Unit.Empty)
            {
                writer.Write("height='" + Height.ToString() + "' ");
            }

            if (useControls)
            {
                writer.Write("controls='controls' ");
            }

            if (autoPlay)
            {
                writer.Write("autoplay='autoplay' ");
            }

            if (loop)
            {
                writer.Write("loop='loop' ");
            }

            if (preload.Length > 0)
            {
                writer.Write("preload='" + preload + "' ");
            }

            if (CssClass.Length > 0)
            {
                writer.Write("class='" + CssClass + "' ");
            }
            

            writer.Write(">");

            writer.Write("<source type='");
            writer.Write(fileType);
            writer.Write("' src='");
            writer.Write(Page.ResolveUrl(fileUrl));
            writer.Write("' />");

            if (includeNonHtml5Fallback)
            {
                writer.Write("<object ");
                if (Height != Unit.Empty)
                {
                    writer.Write("height='" + Height.ToString() + "' ");
                }

                writer.Write("type='application/x-shockwave-flash' ");
                writer.Write("data='" + Page.ResolveUrl(mediaElementBasePath + "flashmediaelement.swf") + "'>");
                writer.Write("<param name='movie' value='" + Page.ResolveUrl(mediaElementBasePath + "flashmediaelement.swf") + "' />");

                writer.Write("<param name='flashvars' ");
                writer.Write("value='controls=true&file=");
                writer.Write(Page.ResolveUrl(fileUrl));
                writer.Write("' />");
                writer.Write("</object>");

            }


            writer.Write("</audio>");

            if (includeDownloadLinkForMedia)
            {
                writer.Write("<a ");
                writer.Write("class='download' ");
                writer.Write("href='");
                writer.Write(Page.ResolveUrl(fileUrl));
                writer.Write("'");
                writer.Write(">");
                writer.Write(downloadLinkText);
                writer.Write("</a>");

            }
        }

        private void RenderVideo(HtmlTextWriter writer)
        {
            writer.Write("<video ");
            if (renderId)
            {
                if (overrideClientId.Length > 0)
                {
                    writer.Write("id='" + overrideClientId + "' ");
                }
                else
                {
                    writer.Write("id='" + ClientID + "' ");
                }
            }

            if (Height != Unit.Empty)
            {
                writer.Write("height='" + Height.ToString() + "' ");
            }

            if (Width != Unit.Empty)
            {
                writer.Write("width='" + Width.ToString() + "' ");
            }

            if (useControls)
            {
                writer.Write("controls='controls' ");
            }

            if (autoPlay)
            {
                writer.Write("autoplay='autoplay' ");
            }

            if (loop)
            {
                writer.Write("loop='loop' ");
            }

            if (preload.Length > 0)
            {
                writer.Write("preload='" + preload + "' ");
            }

            if (posterUrl.Length > 0)
            {
                writer.Write("poster='");
                writer.Write(Page.ResolveUrl(posterUrl));
                writer.Write("' ");
            }

            if (CssClass.Length > 0)
            {
                writer.Write("class='" + CssClass + "' ");
            }

            writer.Write(">");

            writer.Write("<source type='");
            writer.Write(fileType);
            writer.Write("' src='");
            writer.Write(Page.ResolveUrl(fileUrl));
            writer.Write("' />");

            if (includeNonHtml5Fallback)
            {
                writer.Write("<object ");

                if (Width != Unit.Empty)
                {
                    writer.Write("width='" + Width.ToString() + "' ");
                }

                if (Height != Unit.Empty)
                {
                    writer.Write("height='" + Height.ToString() + "' ");
                }

                writer.Write("type='application/x-shockwave-flash' ");
                writer.Write("data='" + Page.ResolveUrl(mediaElementBasePath + "flashmediaelement.swf") + "'>");
                writer.Write("<param name='movie' value='" + Page.ResolveUrl(mediaElementBasePath + "flashmediaelement.swf") + "' />");

                writer.Write("<param name='flashvars' ");
                writer.Write("value='controls=true&file=");
                writer.Write(Page.ResolveUrl(fileUrl));
                writer.Write("' />");

                if (posterUrl.Length > 0)
                {
                    writer.Write("<img src='");
                    writer.Write(Page.ResolveUrl(posterUrl));
                    writer.Write("' ");

                    if (Width != Unit.Empty)
                    {
                        writer.Write("width='" + Width.ToString() + "' ");
                    }

                    if (Height != Unit.Empty)
                    {
                        writer.Write("height='" + Height.ToString() + "' ");
                    }

                    writer.Write("alt='");
                    if (title.Length > 0)
                    {
                        writer.Write(title);
                    }
                    else
                    {
                        writer.Write(" ");
                    }
                    writer.Write("' />");

                }

                writer.Write("</object>");

            }

            writer.Write("</video>");

            if (includeDownloadLinkForMedia)
            {
                writer.Write("<a ");
                writer.Write("class='download' ");
                writer.Write("href='");
                writer.Write(Page.ResolveUrl(fileUrl));
                writer.Write("'");
                writer.Write(">");
                writer.Write(downloadLinkText);
                writer.Write("</a>");

            }


        }

        private void RenderLink(HtmlTextWriter writer)
        {
            writer.Write("<a ");
            if (renderId)
            {
                if (overrideClientId.Length > 0)
                {
                    writer.Write("id='" + overrideClientId + "' ");
                }
                else
                {
                    writer.Write("id='" + ClientID + "' ");
                }
            }

            if (CssClass.Length > 0)
            {
                writer.Write("class='" + CssClass + "' ");
            }

            writer.Write("href='");
            writer.Write(Page.ResolveUrl(fileUrl));
            writer.Write("'");

            writer.Write(">");

            if (title.Length > 0)
            {
                writer.Write(title);
            }
            else
            {
                title = System.IO.Path.GetFileName(HostingEnvironment.MapPath(fileUrl));
                writer.Write(title);
            }

            writer.Write("</a>");





        }

        

    }
}