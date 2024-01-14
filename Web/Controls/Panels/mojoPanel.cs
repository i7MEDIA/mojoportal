using System;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

/// <summary>
/// The purpose of this control is to try and support easy use of Artisteer html designs
/// by adding settings that allow additional unsemantic divs to be added to support Artisteer.
/// by default no extra markup will be applied. The idea will be to use theme.skin to enable a RenderArtisteer property to trigger the extra markup
/// by default this control will only render its contents 
/// </summary>
[Obsolete()]
public class mojoPanel : Panel
{
    private string columnId = UIHelper.CenterColumnId;

	public bool RenderArtisteer { get; set; } = false;

	public bool RenderArtisteerBlockContentDivs { get; set; } = false;

	public bool UseLowerCaseArtisteerClasses { get; set; } = false;

	public string ArtisteerCssClass { get; set; } = UIHelper.ArtisteerPost;

	/// <summary>
	/// Specify a tag to render as, expected values are the new html 5 elements header footer nav article section
	/// </summary>
	public string OverrideTag { get; set; } = string.Empty;

	/// <summary>
	/// If you need to use different css for the overridden tag than for the non overriden div specify it here.
	/// </summary>
	public string OverrideCss { get; set; } = string.Empty;

	public bool UseJQueryUI { get; set; } = false;

	protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);        
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (RenderArtisteer) 
        { 
            columnId = this.GetColumnId(); 
        
            switch (columnId)
            {
                case UIHelper.LeftColumnId:
                case UIHelper.RightColumnId:

                    if (UseLowerCaseArtisteerClasses)
                    {
                        if ((ArtisteerCssClass == UIHelper.ArtisteerPostLower)|(ArtisteerCssClass == UIHelper.ArtisteerPost))
                        {
                            ArtisteerCssClass = UIHelper.ArtisteerBlockLower;
                            RenderArtisteerBlockContentDivs = true;
                        }

                        if ((ArtisteerCssClass == UIHelper.ArtisteerPostContentLower)||(ArtisteerCssClass == UIHelper.ArtisteerPostContent))
                        {
                            ArtisteerCssClass = UIHelper.ArtisteerBlockContentLower;
                            RenderArtisteerBlockContentDivs = true;
                        }
                    }
                    else
                    {
                        if (ArtisteerCssClass == UIHelper.ArtisteerPost)
                        {
                            ArtisteerCssClass = UIHelper.ArtisteerBlock;
                            RenderArtisteerBlockContentDivs = true;
                        }

                        if (ArtisteerCssClass == UIHelper.ArtisteerPostContent)
                        {
                            ArtisteerCssClass = UIHelper.ArtisteerBlockContent;
                            RenderArtisteerBlockContentDivs = true;
                        }
                    }

                    break;

                case UIHelper.CenterColumnId:
                default:
                    if (UseLowerCaseArtisteerClasses) 
                    {
                        if (ArtisteerCssClass == UIHelper.ArtisteerPost)
                        {
                            ArtisteerCssClass = UIHelper.ArtisteerPostLower;
                        }

                        // added for Artisteer 3.0
                        if (ArtisteerCssClass == UIHelper.ArtisteerPostContent)
                        {
                            ArtisteerCssClass = UIHelper.ArtisteerPostContentLower;

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


        if (RenderArtisteer)
        {
            writer.Write("<div class='" + ArtisteerCssClass + "'>\n");

            if (RenderArtisteerBlockContentDivs)
            {
                writer.Write($@"
<div class=""{ArtisteerCssClass}-tl""></div>
<div class=""{ArtisteerCssClass}-tr""></div>
<div class=""{ArtisteerCssClass}-bl""></div>
<div class=""{ArtisteerCssClass}-br""></div>
<div class=""{ArtisteerCssClass}-tc""></div>
<div class=""{ArtisteerCssClass}-bc""></div>
<div class=""{ArtisteerCssClass}-cl""></div>
<div class=""{ArtisteerCssClass}-cr""></div>
<div class=""{ArtisteerCssClass}-cc""></div>
<div class=""{ArtisteerCssClass}-body"">");
            }

            base.RenderContents(writer);

            if (RenderArtisteerBlockContentDivs)
            {
                writer.Write(@"
</div>
<div class=""cleared""></div>");
            }
            writer.Write("\n</div>");
            return;

        }

        if (UseJQueryUI)
        {
            if (ArtisteerCssClass == UIHelper.ArtisteerPostContent)
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


        if (OverrideCss.Length > 0) { CssClass = OverrideCss; }

        if ((OverrideTag.Length > 0))
        {
            writer.Write($"<{OverrideTag} class=\"{CssClass}\">\n");

            base.RenderContents(writer);

            writer.Write($"\n</{OverrideTag}>");
            return;
        }

        base.RenderContents(writer);
        return;
    }
}