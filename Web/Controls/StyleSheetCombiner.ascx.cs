using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI;

/// <summary>
/// This control combines the css files in the order they are listed in
/// the style.config file located in the skin folder
/// </summary>
public partial class StyleSheetCombiner : UserControl
{
	private string skinBaseUrl = string.Empty;
	private string protocol = "https";

	public bool AddBodyClassForLiveWriter { get; set; } = true;

	/// <summary>
	/// this property is not used directly by this control but the base page and cms page detect ths setting and use it
	/// so it allows configuring this per skin
	/// </summary>
	public bool UseIconsForAdminLinks { get; set; } = true;

	/// <summary>
	/// set this to true if you are including the css from /Data/style/formvalidation in your style.config
	/// this flag is chacked by code in the register.aspx page and if true then it wires up the script for jquery validation
	/// </summary>
	public bool UsingjQueryHintsOnRegisterPage { get; set; } = false;

	/// <summary>
	/// this property is not used directly by this control but the base page and cms page detect ths setting and use it
	/// so it allows configuring this per skin
	/// </summary>
	public bool UseTextLinksForFeatureSettings { get; set; } = true;

	/// <summary>
	/// this property is not used directly by this control but the base page and cms page detect ths setting and use it
	/// so it allows configuring this per skin
	/// </summary>
	public bool SiteMapPopulateOnDemand { get; set; } = false;

	/// <summary>
	/// this property is not used directly by this control but the base page and cms page detect ths setting and use it
	/// so it allows configuring this per skin
	/// the default -1 means fully expanded, when using SiteMapPopulateOnDemand=true you would typically set this to 0
	/// </summary>
	public int SiteMapExpandDepth { get; set; } = -1;

	/// <summary>
	/// valid options are: base, black-tie, blitzer, cupertino, dot-luv, excite-bike, hot-sneaks, humanity, mint-choc,
	/// redmond, smoothness, south-street, start, swanky-purse, trontastic, ui-darkness, ui-lightness, vader
	/// </summary>
	public string JQueryUIThemeName { get; set; } = "smoothness";

	public bool IncludejQueryUI { get; set; } = true;

	public bool IncludejCrop { get; set; } = false;

	public bool AllowPageOverride { get; set; } = false;

	public bool LoadSkinCss { get; set; } = true;

	public string OverrideSkinName { get; set; } = string.Empty;

	public bool IncludeColorPickerCss { get; set; } = false;

	public bool AlwaysShowLeftColumn { get; set; } = false;

	public bool AlwaysShowRightColumn { get; set; } = false;

	public bool IncludeGoogleCustomSearchCss { get; set; } = false;

	public bool DisableCssHandler { get; set; } = false;

	#region Property Bag settings stored here but not used in this control

	/// <summary>
	/// it is possible for dynamic menus like jQuery superfish to make unclickable menu links
	/// the menu controls will check this setting to determine whether to enable it
	/// the property is stored here just to make it controlled by the skin since 
	/// other menus don't makesense with unclickable links
	/// doesn't work for TreeView
	/// 2014-01-08 changed the default from false to to true because all newer skins use FlexMenu
	/// which does support it, TreeView is now legacy
	/// 2023-12-22 changed default to false because this is not good for SEO or UX unless you're building an
	/// application, most mojo sites are websites, not applications
	/// </summary>
	public bool EnableNonClickablePageLinks { get; set; } = false;

	/// <summary>
	/// there are no good ways to expand MenuItem with additional properties so we are using a property for something other than its intended purposes
	/// the MenuAdapterArtisteer is used to override the rendering and there we can use the tooltip property as a way to add a custom css class to soecific menu items.
	/// Admittedly an ugly solution but no other solutions seem feasible
	/// </summary>
	public bool UseMenuTooltipForCustomCss { get; set; } = true;

	public bool UseArtisteer3 { get; set; } = false;

	public bool HideEmptyAlt1 { get; set; } = true;

	public bool HideEmptyAlt2 { get; set; } = true;

	public string Media { get; set; } = string.Empty;

	#endregion

	private string GetMediaProperty()
	{
		if (Media.Length > 0)
		{
			return $"media=\"{Media}\"";
		}
		return string.Empty;
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if (!WebHelper.IsSecureRequest())
		{
			protocol = "http";
		}

		if (IncludejQueryUI)
		{
			SetupjQueryUICss();
		}

		if (IncludejCrop)
		{
			SetupjCropCss();
		}

		if (IncludeGoogleCustomSearchCss)
		{
			Controls.Add(new Literal
			{
				ID = "googlesearchcsss",
				Text = $"\n<link rel=\"stylesheet\" data-loader=\"StyleSheetCombiner\" href=\"{protocol}://www.google.com/cse/static/style/look/v4/default.css\" />"
			});
		}

		if (!LoadSkinCss)
		{
			return;
		}

		skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(AllowPageOverride, Page);

		if (OverrideSkinName.Length > 0)
		{
			skinBaseUrl = SiteUtils.DetermineSkinBaseUrl(OverrideSkinName);
		}

		if (AppConfig.CombineCSS && !DisableCssHandler)
		{
			SetupCombinedCssUrl();
		}
		else
		{
			AddCssLinks();
		}
	}

	private void SetupCombinedCssUrl()
	{
		var cssLink = new Literal();

		var siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (SiteUtils.UseMobileSkin())
		{
			if (!string.IsNullOrWhiteSpace(siteSettings.MobileSkin))
			{
				OverrideSkinName = siteSettings.MobileSkin;
			}
			if (!string.IsNullOrWhiteSpace(WebConfigSettings.MobilePhoneSkin))
			{
				//web.config setting trumps site setting
				OverrideSkinName = WebConfigSettings.MobilePhoneSkin;
			}
		}

		cssLink.Text = $"""
				<link rel="stylesheet" data-loader="StyleSheetCombiner" {GetMediaProperty()} href="{SiteUtils.GetCssHandlerUrl(true, OverrideSkinName)}"/>
				""";

		Controls.Add(cssLink);
	}


	private void SetupjQueryUICss()
	{
		if (WebConfigSettings.DisablejQueryUI) { return; }

		string jQueryUIBasePath;
		string jQueryUIVersion = WebConfigSettings.GoogleCDNjQueryUIVersion;
		string jQueryCssAllName = "jquery-ui.css"; //the jquery.ui.all.css uses @imports so it loads 15 separate style sheets, whereas jquery-ui.css is all in one file

		if (WebConfigSettings.UseGoogleCDN)
		{
			jQueryUIBasePath = $"{protocol}:{WebConfigSettings.GoogleCDNJQueryUIBaseUrl + jQueryUIVersion}/";
		}
		else
		{
			jQueryUIBasePath = Page.ResolveUrl(WebConfigSettings.jQueryUIBasePath);
		}

		Controls.Add(new Literal
		{
			ID = "jqueryui-css",
			Text = $"""
				   <link rel="stylesheet" data-loader="StyleSheetCombiner" href="{jQueryUIBasePath}themes/{JQueryUIThemeName}/{jQueryCssAllName}" />
				   """
		});
	}

	private void SetupjCropCss()
	{
		Controls.Add(new Literal
		{
			ID = "jcrop-css",
			Text = $"""
				   <link rel="stylesheet" data-loader="StyleSheetCombiner" href="{Page.ResolveUrl("~/ClientScript/jcrop0912/jquery.Jcrop.css")}" />
				   """
		});
	}

	private void AddCssLinks()
	{
		string configFilePath;
		if (OverrideSkinName.Length > 0)
		{
			configFilePath = Server.MapPath(SiteUtils.DetermineSkinBaseUrl(SiteUtils.SanitizeSkinParam(OverrideSkinName)) + "style.config");
		}
		else
		{
			configFilePath = Server.MapPath(SiteUtils.DetermineSkinBaseUrl(AllowPageOverride, Page) + "style.config");
		}

		if (File.Exists(configFilePath)) //if no file, no style is added
		{
			using XmlReader reader = new XmlTextReader(new StreamReader(configFilePath));
			reader.MoveToContent();
			while (reader.Read())
			{
				if (("file" == reader.Name) && (reader.NodeType != XmlNodeType.EndElement))
				{
					string cssVPath = reader.GetAttribute("cssvpath");
					string cssUrl = string.Empty;
					if (!string.IsNullOrWhiteSpace(cssVPath))
					{
						cssUrl = Page.ResolveUrl($"~{cssVPath}");
					}
					else
					{
						cssUrl = $"{skinBaseUrl}{reader.ReadElementContentAsString()}";
					}

					// only add CSS files
					if (!cssUrl.EndsWith(".css"))
					{
						return;
					}

					Controls.Add(new Literal
					{
						Text = $"""
							  <link rel="stylesheet" data-loader="StyleSheetCombiner" href="{cssUrl}" />
							  """
					});
				}
			}
		}
	}
}