using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI
{
    public class CommerceTestModeWarning : Label
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();

            if (!commerceConfig.IsConfigured)
            {
                this.Visible = false;
                return;
            }

            // if not in test mode hide this warning
            if (!commerceConfig.PaymentGatewayUseTestMode)
            {
                this.Visible = false;
                return;
            }

            this.CssClass = "txterror warning commercewarning";
            this.Text = Resource.CommerceTestModeWarning;
            //this.Style.Add(HtmlTextWriterStyle.FontSize, "large");
            this.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            this.Style.Add(HtmlTextWriterStyle.Color, "red");

        }
    }
}
