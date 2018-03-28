using mojoPortal.Web.Framework;
using Resources;
using System;

namespace mojoPortal.Web.UI.Pages
{


	public partial class AccessDeniedPage : NonCmsBasePage
	{
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

		private void Page_Load(object sender, System.EventArgs e)
		{

            //having this here causes a redirect to the login page.
            //Response.StatusCode = 401;

            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AccessDenied);
            lnkHome.Text = Resource.ReturnHomeLabel;
            lnkHome.ToolTip = Resource.ReturnHomeLabel;
            lnkHome.NavigateUrl = SiteRoot + "/Default.aspx";

			if (!Request.IsAuthenticated)
			{
				lnkLogin.Visible = true;
				lnkLogin.Text = Resource.LoginLink;
				lnkLogin.NavigateUrl = SiteUtils.GetLoginRelativeUrl();
			}

            SiteUtils.AddNoIndexMeta(this);
            
			lblAccessDenied.Text = ResourceHelper.GetMessageTemplate("AccessDeniedMessage.config");

            AddClassToBody("accessdenied");
		}

        
    }
}
