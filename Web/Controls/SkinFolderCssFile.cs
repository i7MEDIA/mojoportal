using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

/// <summary>
/// this control can be used in layout.master to load an extra css file
/// you can specify roles for whom the extra css link will be rendered
/// if left blank then it will always render provided that either CssFileName or CssFullUrl are specified
/// requested here:
/// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12248~1#post50892
/// 
/// Example usage:
/// <portal:SkinFolderCssFile id="css1" runat="server" CssFile="admin.css" VisibleRoles="Admins;Content Administrators" />
/// where admin.css is an extra file in the skin folder that is not combined in the main css
/// </summary>
public class SkinFolderCssFile : WebControl
{
	public string CssFileName { get; set; } = string.Empty;

	/// <summary>
	/// if specified this is used instead of CssFileName. for using css file that is not in the skin folder
	/// use a protocol relative url //www.somedomain.com/style.css
	/// </summary>
	public string CssFullUrl { get; set; } = string.Empty;

	/// <summary>
	/// a semi colon separated list of role names
	/// Admins;Content Administrators
	/// </summary>
	public string VisibleRoles { get; set; } = string.Empty;

	/// <summary>
	/// a comma separated list of relative urls where the extra css file shold be used
	/// if specified then the link will only be rendered if the current Request.RawUrl contains on of the specified values
	/// /Admin,/HtmlEdit.aspx would add the css only on pages in the Admin folder and on the HtmlEdit.aspx page in the root
	/// </summary>
	public string VisibleUrls { get; set; } = string.Empty;

	public string LinkFormat { get; set; } = "<link rel=\"stylesheet\" href=\"{0}\" data-loader=\"skinfoldercss\">";


	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		if (CssFileName.Length == 0)
		{
			Visible = false;
		}

		if (VisibleRoles.Length > 0)
		{
			if (!WebUser.IsInRoles(VisibleRoles))
			{
				Visible = false;
			}
		}

		if (VisibleUrls.Length > 0)
		{
			bool match = false;
			List<string> allowedUrls = VisibleUrls.SplitOnChar(',');

			foreach (string u in allowedUrls)
			{
				//Page.AppRelativeVirtualPath will match for things like blog posts where the friendly url is something like /my-cool-post which
				//is then mapped to the /Blog/ViewPost.aspx page. So, one could use /Blog/ViewPost.aspx in the AllowedUrls property to render
				//a css file on blog post pages.
				if (base.Page.AppRelativeVirtualPath.Contains(u, StringComparison.OrdinalIgnoreCase))
				{
					match = true;
				}

				//Page.Request.RawUrl is the url used for the request, as in the example above '/my-cool-post'
				if (base.Page.Request.RawUrl.Contains(u, StringComparison.OrdinalIgnoreCase))
				{
					match = true;
				}
			}

			Visible = match;
		}
	}


	protected override void Render(HtmlTextWriter writer)
	{
		if (!Visible)
		{
			return;
		}

		if (CssFullUrl.Length > 0)
		{
			writer.Write(string.Format(CultureInfo.InvariantCulture, $"\n{LinkFormat}", CssFullUrl));

			return;
		}

		writer.Write(string.Format(
			CultureInfo.InvariantCulture,
			$"\n{LinkFormat}",
			LinkBuilder.SkinUrl(CssFileName, Page).ToString()
		));
	}
}
