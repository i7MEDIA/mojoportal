using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.AdminUI;

public partial class MonitorPage : NonCmsBasePage
{
	// this page is experimental at this point, it only works in .NET 4 full trust
	// and only if you have these set to true in user.config
	// <add key="AppDomainMonitoringEnabled" value="true" />
	// <add key="FirstChanceExceptionMonitoringEnabled" value="true" />
	// it can show current memory usage and it can show all exceptions both handled and unhandled
	// however you should not keep this enabled for very long because it keeps all the excpetions in memory
	// so using this tool can drive up memory usage while you are using it.
	//


	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		if ((!siteSettings.IsServerAdminSite) || (!WebUser.IsAdmin))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (!WebConfigSettings.AppDomainMonitoringEnabled && !WebConfigSettings.FirstChanceExceptionMonitoringEnabled)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}


		PopulateControls();

	}

	private void PopulateControls()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, "Monitor");

		litTotalAllocatedMemorySize.Text = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize.ToString(CultureInfo.InvariantCulture);
		litSurvivedMemorySize.Text = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize.ToString(CultureInfo.InvariantCulture);
		litTotalProcessorTime.Text = AppDomain.CurrentDomain.MonitoringTotalProcessorTime.Milliseconds.ToInvariantString();

		var listRequestException = Context.Application["AllExc"] as List<RequestException>;
		if (listRequestException == null)
		{
			listRequestException = new List<RequestException>();
		}


		rpt.DataSource = listRequestException;
		rpt.DataBind();


	}

	void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		Repeater rptExceptions = (Repeater)e.Item.FindControl("rptExceptions");

		if (rptExceptions == null) { return; }

		RequestException r = e.Item.DataItem as RequestException;



		if (r != null)
		{

			rptExceptions.DataSource = r.Exceptions;
			rptExceptions.DataBind();
		}
	}







	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		rpt.ItemDataBound += new RepeaterItemEventHandler(rpt_ItemDataBound);


	}



	#endregion
}
