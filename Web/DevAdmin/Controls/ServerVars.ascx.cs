using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.DevAdmin
{
	public partial class ServerVarsControl : System.Web.UI.UserControl
	{
		private NameValueCollection serverVars = new();
		private SiteSettings siteSettings = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteUtils.ForceSsl();

			LoadSettings();
			if (
				(!WebUser.IsAdmin)
				|| (siteSettings == null)
				|| (!siteSettings.IsServerAdminSite)
				|| (!WebConfigSettings.EnableDeveloperMenuInAdminMenu)
				)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			BindList();
		}

		private void BindList()
		{
			if (serverVars == null) return;

			rptrServerVars.DataSource = serverVars;
			rptrServerVars.DataBind();

		}

		void rptrServerVars_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item
				|| e.Item.ItemType == ListItemType.AlternatingItem)
			{
				string key = e.Item.DataItem as string;
				Literal Value = e.Item.FindControl("VarValue") as Literal;
				Value.Text = serverVars.Get(key);
			}

		}

		private void LoadSettings()
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			//serverVars = Request.ServerVariables;

			foreach (string i in Request.ServerVariables)
			{
				if (Request.ServerVariables[i] != null)
				{
					serverVars.Add(i, Request.ServerVariables[i]);
				}
			}

			foreach (string i in Session.Contents)
			{
				if (Session[i] != null)
				{
					serverVars.Add(i, Session[i].ToString());
				}
			}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);

			rptrServerVars.ItemDataBound += new RepeaterItemEventHandler(rptrServerVars_ItemDataBound);
		}
	}
}