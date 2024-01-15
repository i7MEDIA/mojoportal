using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Core.Extensions;

namespace mojoPortal.Web.Controls;

public class AddThisButton : HyperLink
{
	#region Private Properties

	private string protocol = "http";

	#endregion


	#region Public Properties


	/// <summary>
	/// Your addthis.com username.
	/// If this is not set the control will not render.
	/// </summary>
	public string AccountId { get; set; } = string.Empty;

	/// <summary>
	/// if true will show widget in the page
	/// </summary>
	public bool UseMouseOverWidget { get; set; } = true;

	/// <summary>
	/// The logo to display on the popup window (about 200x50 pixels). 
	/// The popup window is show when the user selects the 'More' choice
	/// </summary>
	public string CustomLogoUrl { get; set; } = string.Empty;

	/// <summary>
	/// The color to use as a background around the logo in the popup 
	/// </summary>
	public string CustomLogoBackgroundColor { get; set; } = string.Empty;


	/// <summary>
	/// The color to use for the text next to the logo in the popup 
	/// </summary>
	public string CustomLogoColor { get; set; } = string.Empty;


	/// <summary>
	/// The brand name to display in the drop-down (top right)
	/// </summary>
	public string CustomBrand { get; set; } = string.Empty;


	/// <summary>
	/// A comma-separated ordered list of options to include in the drop-down
	/// Example: addthis_options = 'favorites, email, digg, delicious, more'; 
	/// Currently supported options:
	/// delicious, digg, email, favorites, facebook, fark, furl, google, live, myweb, myspace, 
	/// newsvine, reddit, slashdot, stumbleupon, technorati, twitter, more 
	/// (the default is currently 'favorites, digg, delicious, google, myspace, facebook, 
	/// reddit, newsvine, 
	/// live, more', in that order).
	/// </summary>
	public string CustomOptions { get; set; } = string.Empty;

	/// <summary>
	/// Vertical offset for the drop-down window widget (in pixels) 
	/// thiscontrol defaults to -999 which means unsepcified
	/// this will result in the defaults from addthis.com
	/// not sure what the defsault is
	/// </summary>
	public int CustomOffsetTop { get; set; } = -999;

	/// <summary>
	/// Horizontal offset for the drop-down window widget (in pixels) 
	/// thiscontrol defaults to -999 which means unsepcified
	/// this will result in the defaults from addthis.com
	/// not sure what the defsault is
	/// </summary>
	public int CustomOffsetLeft { get; set; } = -999;

	public string ButtonImageUrl { get; set; } = "~/Data/SiteImages/addthissharebutton.gif";

	public string UrlToShare { get; set; } = string.Empty;

	public string TitleOfUrlToShare { get; set; } = string.Empty;


	#endregion

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		if (HttpContext.Current == null) { return; }

		if (AccountId.Length == 0)
		{
			this.Visible = false;
			return;
		}

		if (Page.Request.IsSecureConnection)
			protocol = "https";

		SetupScripts();

		this.ImageUrl = Page.ResolveUrl(ButtonImageUrl);
		this.NavigateUrl = "http://www.addthis.com/bookmark.php";

		if (UseMouseOverWidget)
			SetupWidget();
		else
			SetupNormalLink();

	}

	private void SetupNormalLink()
	{
		StringBuilder onClickAttribute = new StringBuilder();

		if (UrlToShare.Length > 0)
		{
			onClickAttribute.Append("addthis_url = '" + UrlToShare + "'; ");
		}
		else
		{
			onClickAttribute.Append("addthis_url = location.href; ");
		}

		if (TitleOfUrlToShare.Length > 0)
		{
			onClickAttribute.Append("addthis_title ='" + TitleOfUrlToShare.HtmlEscapeQuotes() + "'; ");
		}
		else
		{
			onClickAttribute.Append("addthis_title = document.title; ");

		}

		onClickAttribute.Append("return addthis_click(this); ");

		this.Attributes.Add("onclick", onClickAttribute.ToString());

		//this.Attributes.Add("onclick", "return addthis_click(this); ");

	}

	private void SetupWidget()
	{
		StringBuilder mouseOverAttribute = new StringBuilder();

		mouseOverAttribute.Append("try {return addthis_open(this, '',");

		if (UrlToShare.Length > 0)
		{
			mouseOverAttribute.Append("'" + UrlToShare + "', ");
		}
		else
		{
			mouseOverAttribute.Append("'[URL]', ");
		}

		if (TitleOfUrlToShare.Length > 0)
		{
			mouseOverAttribute.Append("'" + TitleOfUrlToShare.HtmlEscapeQuotes() + "' ");
		}
		else
		{
			mouseOverAttribute.Append("'[TITLE]' ");

		}

		mouseOverAttribute.Append(");}catch(ex){} ");


		this.Attributes.Add("onmouseover", mouseOverAttribute.ToString());

		this.Attributes.Add("onmouseout", "try { addthis_close(); }catch(ex){}");

	}

	private void SetupScripts()
	{
		StringBuilder script = new StringBuilder();
		script.Append("<script data-loader=\"addThisButton.cs\"> ");
		script.Append("\n<!-- \n");

		script.Append("var addthis_pub = '" + AccountId + "';");

		if (CustomLogoUrl.Length > 0)
			script.Append("var addthis_logo = '" + CustomLogoUrl + "';");

		if (CustomLogoBackgroundColor.Length > 0)
			script.Append("var addthis_logo_background = '" + CustomLogoBackgroundColor + "';");

		if (CustomLogoColor.Length > 0)
			script.Append("var addthis_logo_color = '" + CustomLogoColor + "';");

		if (CustomBrand.Length > 0)
			script.Append("var addthis_brand = '" + CustomBrand + "';");

		if (CustomOptions.Length > 0)
			script.Append("var addthis_options = '" + CustomOptions + "';");

		if (CustomOffsetTop != -999)
			script.Append("var addthis_offset_top = " + CustomOffsetTop.ToString(CultureInfo.InvariantCulture) + ";");

		if (CustomOffsetLeft != -999)
			script.Append("var addthis_offset_left = " + CustomOffsetLeft.ToString(CultureInfo.InvariantCulture) + ";");


		script.Append("\n//--> ");
		script.Append(" </script>");


		Page.ClientScript.RegisterClientScriptBlock(
			typeof(AddThisButton),
			"addthisbutton",
			script.ToString());

		if (UseMouseOverWidget)
			Page.ClientScript.RegisterStartupScript(
				typeof(AddThisButton),
				"addthisbuttonsetup", $"\n<script data-loader=\"addThisButton.cs\" src=\"{protocol}://s7.addthis.com/js/152/addthis_widget.js\" ></script>");
		else
			Page.ClientScript.RegisterStartupScript(
				typeof(AddThisButton),
				"addthisbuttonsetup", $"\n<script data-loader=\"addThisButton.cs\" src=\"{protocol}://s9.addthis.com/js/widget.php?v=10\" ></script>");
	}


	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write("[" + this.ID + "]");
			return;
		}

		base.Render(writer);
	}
}