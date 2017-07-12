// Author:					
// Created:				    2008-07-03
// Last Modified:			2009-12-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;


namespace mojoPortal.Web.DevAdmin
{
    public partial class ServerVarsControl : System.Web.UI.UserControl
    {
        private NameValueCollection serverVars = null;
        private SiteSettings siteSettings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();

            LoadSettings();
            if (
                (!WebUser.IsAdmin)
                ||(siteSettings == null)
                || (!siteSettings.IsServerAdminSite)
                || (!WebConfigSettings.EnableDeveloperMenuInAdminMenu)
                )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

           
            PopulateControls();

        }

        private void PopulateControls()
        {
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
            serverVars = Request.ServerVariables;

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);

            rptrServerVars.ItemDataBound += new RepeaterItemEventHandler(rptrServerVars_ItemDataBound);
        }
    }
}