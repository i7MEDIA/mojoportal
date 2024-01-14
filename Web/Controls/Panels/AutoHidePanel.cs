using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

/// <summary>
/// this is useful when you need a div container but you don't want it to render if none of the WebControls inside are visible
/// </summary>
public class AutoHidePanel : Panel
{
	private int countOfVisibleWebControls = 0;

	protected override void OnPreRender(EventArgs e)
	{
		if (HttpContext.Current == null)
		{
			base.OnPreRender(e);
			return;
		}

		countOfVisibleWebControls = GetCountVisibleChildWebControls();
		Visible = (countOfVisibleWebControls > 0);

		base.OnPreRender(e);
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write($"[{this.ID}]");
			return;
		}

		countOfVisibleWebControls = GetCountVisibleChildWebControls();
		if (countOfVisibleWebControls > 0)
		{
			base.Render(writer);
		}
	}

	private int GetCountVisibleChildWebControls()
	{
		foreach (Control c in Controls)
		{
			if (c is WebControl && c.Visible) { return 1; }
			if (c is ContentPlaceHolder)
			{
				foreach (Control child in c.Controls)
				{
					if (child is WebControl && child.Visible) { return 1; }
				}
			}
		}

		return 0;
	}
}