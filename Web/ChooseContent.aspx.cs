/// Author:					Joe Audette
/// Created:				2006-06-14
/// Last Modified:			2009-04-10
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI.Pages
{
    
    public partial class ChooseContent : mojoBasePage
    {
#if !MONO
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.dlWebParts.ItemCommand += new DataListCommandEventHandler(dlWebParts_ItemCommand);
            if (WebConfigSettings.HideAllMenusOnMyPage)
            {
                SuppressAllMenus();
            }
            else
            {
                SuppressMenuSelection();
                SuppressPageMenu();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            
            
            SecurityHelper.DisableBrowserCache();
            PopulateLabels();

            if (!IsPostBack)
            {
                BindList();
            }
        }


        protected void BindList()
        {
            using (IDataReader reader = WebPartContent.GetWebPartsForMyPage(siteSettings.SiteId))
            {
                this.dlWebParts.DataSource = reader;
                this.dlWebParts.DataBind();
            }
        }


        void dlWebParts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            String returnUrl = SiteRoot + "/MyPage.aspx";
            switch (e.CommandName)
            {
                case "addpart":
                    returnUrl += "?addpart=" + e.CommandArgument.ToString();
                    break;

                case "addmodule":
                    returnUrl += "?addmpart=" + e.CommandArgument.ToString();

                    break;

            }

            WebUtils.SetupRedirect(this, returnUrl);


        }

        protected String GetCommandName(int moduleId, String webPartId)
        {
            if (moduleId > -1)
            {
                return "addmodule";
            }

            return "addpart";

        }

        protected String GetCommandArgument(int moduleId, String webPartId)
        {
            if (moduleId > -1)
            {
                return moduleId.ToString();
            }

            return webPartId;

        }

        protected String GetIcon(String moduleIcon, String featureIcon)
        {
            if (moduleIcon.Length > 0)
            {
                return moduleIcon;
            }

            return featureIcon;

        }

        private void PopulateLabels()
        {

            AddClassToBody("choosecontent");
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            Panel p = Page.Master.FindControl("divLeft") as Panel;
            if (p != null) { p.CssClass = "left-mypage"; }

            p = Page.Master.FindControl("divRight") as Panel;
            if (p != null) { p.CssClass = "right-mypage"; }

            p = Page.Master.FindControl("divCenter") as Panel;
            if (p != null) { p.CssClass = "center-mypage"; }
        }

#endif

    }
}

