using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// a wrapper control for html audio and video elements
	/// </summary>
	public class MediaElement : Panel
    {
        [Themeable(false)]
        public string FileUrl { get; set; } = string.Empty;
        [Themeable(false)]
        public string FileType { get; set; } = string.Empty;
        [Themeable(false)]
        public string PosterUrl { get; set; } = string.Empty;
        [Themeable(false)]
        public string Title { get; set; } = string.Empty;
        [Themeable(false)]
        public string OverrideClientId { get; set; } = string.Empty;
        public bool RenderId { get; set; } = false;
        public bool UseControls { get; set; } = true;
        public bool AutoPlay { get; set; } = false;
        public bool Muted { get; set; } = true;
        public bool Loop { get; set; } = false;
        public string Preload { get; set; } = "metadata";
        public bool AutoDetectFileType { get; set; } = true;
        public bool AllowDownload { get; set; } = true;
        public List<string> ControlsList { get; set; } = new();
        private string type = string.Empty;
        private List<string> attributesList { get; set; } = new();
        //public string DownloadLinkText { get; set; } = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if ((!FileType.StartsWith("audio")) && (!FileType.StartsWith("video"))) 
			{ 
				return; 
			}
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (FileUrl.Length == 0) { return; }
            if (FileType.Length == 0)
            {
                if (!AutoDetectFileType) { return; }
                FileType = FileUrl.ToMimeType();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            if (FileUrl.Length == 0) { return; }
            if (FileType.Length == 0) { return; }

            type = FileType.StartsWith("audio") ? "audio" : "video";

			if (!AllowDownload)
			{
				ControlsList.Add("nodownload");
			}

			if (RenderId)
			{
				if (OverrideClientId.Length > 0)
				{
					attributesList.Add($"id=\"{OverrideClientId}\"");
				}
				else
				{
					attributesList.Add($"id=\"{ClientID}\"");
				}
			}

			if (type == "video")
			{
				if (Height != Unit.Empty)
				{
					attributesList.Add($"height=\"{Height}\"");
				}

				if (Width != Unit.Empty)
				{
					attributesList.Add($"width=\"{Width}\"");
				}
				if (PosterUrl.Length > 0)
				{
					attributesList.Add($"poster=\"{Page.ResolveUrl(PosterUrl)}\"");
				}
			}

			if (UseControls)
			{
				attributesList.Add("controls");

				if (ControlsList.Count > 0)
				{
					attributesList.Add($"controlsList=\"{string.Join(" ", ControlsList)}\"");
				}
			}

			if (AutoPlay)
			{
				attributesList.Add($"autoplay");
				if (Muted)
				{
					attributesList.Add("muted");
				}
			}

			if (Loop)
			{
				attributesList.Add("loop");
			}

			if (Preload.Length > 0)
			{
				attributesList.Add($"preload=\"{Preload}\"");
			}

			if (CssClass.Length > 0)
			{
				attributesList.Add($"class=\"{CssClass}\"");
			}

			if (!AllowDownload)
			{
				attributesList.Add("oncontextmenu=\"return false;\"");
			}

			writer.Write($"<{type} {string.Join(" ", attributesList)}><source type=\"{FileType}\" src=\"{Page.ResolveUrl(FileUrl)}\" /></{type}>");
		}
    }
}