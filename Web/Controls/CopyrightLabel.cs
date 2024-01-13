using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI;

public class CopyrightLabel : WebControl
{
	private int beginYear = -1;
	private string copyrightBy = string.Empty;
	private bool showYear = true;

	public string CopyrightBy
	{
		get { return copyrightBy; }
		set { copyrightBy = value; }
	}

	public int BeginYear
	{
		get { return beginYear; }
		set { beginYear = value; }
	}

	public bool ShowYear
	{
		get { return showYear; }
		set { showYear = value; }
	}

	protected override void Render(HtmlTextWriter writer)
	{

		if (HttpContext.Current == null)
		{
			
			writer.Write("[" + this.ID + "]");
		}

		DoRender(writer);
	}

	private void DoRender(HtmlTextWriter writer)
	{
		if (copyrightBy.Length == 0)
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null)
			{
				this.Visible = false;
				return;
			}

			copyrightBy = siteSettings.CompanyName;

		}

		writer.Write("&copy; ");
		if (showYear)
		{
			if (beginYear > -1)
			{
				if (beginYear != DateTime.UtcNow.Year)
				{
					writer.Write(beginYear.ToString(CultureInfo.InvariantCulture));
					writer.Write(" - ");
				}
				writer.Write(DateTime.UtcNow.Year.ToString(CultureInfo.InvariantCulture));
				writer.Write(" ");
			}

		}

		writer.WriteEncodedText(copyrightBy);
	}
}