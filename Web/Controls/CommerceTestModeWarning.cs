using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI;

public class CommerceTestModeWarning : Label
{
	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);

		CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();

		if (!commerceConfig.IsConfigured)
		{
			Visible = false;
			return;
		}

		// if not in test mode hide this warning
		if (!commerceConfig.PaymentGatewayUseTestMode)
		{
			Visible = false;
			return;
		}

		CssClass = "txterror warning commercewarning";
		Text = Resource.CommerceTestModeWarning;
		Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
		Style.Add(HtmlTextWriterStyle.Color, "red");
	}
}