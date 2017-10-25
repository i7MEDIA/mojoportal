using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.UI.Pages 
{

	
    public partial class EditAccessDenied : NonCmsBasePage
	{
        

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

		private void Page_Load(object sender, System.EventArgs e)
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AccessDenied);
            lnkHome.Text = Resource.ReturnHomeLabel;
            lnkHome.ToolTip = Resource.ReturnHomeLabel;
            lnkHome.NavigateUrl = SiteRoot + "/Default.aspx";
            SiteUtils.AddNoIndexMeta(this);
			//lblEditAccessDeniedMessage.Text = ResourceHelper.GetMessageTemplate("EditAccessDeniedMessage.config");

            AddClassToBody("accessdenied");

		}

    }
}
