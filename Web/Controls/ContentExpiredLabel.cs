using System;
using mojoPortal.Web.Controls;

namespace mojoPortal.Web.UI;

public class ContentExpiredLabel : SiteLabel
{
	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		UseLabelTag = false;
		ConfigKey = "ContentExpiredWarning";
		CssClass = "txterror expired";
	}
}