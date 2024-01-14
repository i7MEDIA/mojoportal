using System;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web.UI;

/// <summary>
/// this is useful when you need a div container but you want it to render only on mobile browsers
/// </summary>
public class MobileViewPanel : BasePanel
{
	private bool isMobileDevice = false;
	private bool useMobileSkin = false;

	/// <summary>
	/// If set to true, this control will only render when a mobile skin is set in Site Settings. Default is false.
	/// </summary>
	public bool RequireMobileSkin { get; set; } = false;

	protected override void OnPreRender(EventArgs e)
	{
		if (HttpContext.Current == null)
		{
			base.OnPreRender(e);
			return;
		}

		isMobileDevice = SiteUtils.IsMobileDevice();
		useMobileSkin = SiteUtils.UseMobileSkin();

		if ((!isMobileDevice) //if it is not a mobile device, hide the panel
			|| (RequireMobileSkin && !useMobileSkin)) //if we are requiring a mobile skin but one is not set, hide the panel
		{
			Visible = false;
		}

		base.OnPreRender(e);
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write("[" + this.ID + "]");
			return;
		}

		if (Visible)
		{
			base.Render(writer);
		}
	}
}
