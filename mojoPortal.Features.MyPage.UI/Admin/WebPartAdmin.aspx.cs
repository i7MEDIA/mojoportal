/// Author:					
/// Created:				2006-06-04
/// Last Modified:			2011-02-26
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

#if !MONO

using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Features.UI;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class WebPartAdminPage : NonCmsBasePage
    {
        private string pageNumberParam;
        private string sortParam;
        private String sort = "Title";
        //protected string ViewPage = "Default.aspx";
        private int pageNumber = 1;
        private int pageSize = 15;
        private int totalPages = 0;
        private string iconPath;
        private bool sortByClassName = false;
        private bool sortByAssemblyName = false;
        //private Type webPartType;
        private bool isSiteEditor = false;
        private string editContentImage = ConfigurationManager.AppSettings["EditContentImage"]; 

        protected string EditContentImage
        {
            get{return editContentImage;}
        }

        #region Private ViewState Properties

        private String CurrentAssembly
        {
            get
            {
                object obj = ViewState["CurrentAssembly"];
                return (obj != null) ? (String)obj : String.Empty;
            }
            set
            {
                ViewState["CurrentAssembly"] = value;
            }
        }

        private String CurrentClass
        {
            get
            {
                object obj = ViewState["CurrentClass"];
                return (obj != null) ? (String)obj : String.Empty;
            }
            set
            {
                ViewState["CurrentClass"] = value;
            }
        }

        private String CurrentTitle
        {
            get
            {
                object obj = ViewState["CurrentTitle"];
                return (obj != null) ? (String)obj : String.Empty;
            }
            set
            {
                ViewState["CurrentTitle"] = value;
            }
        }

        private String CurrentDescription
        {
            get
            {
                object obj = ViewState["CurrentDescription"];
                return (obj != null) ? (String)obj : String.Empty;
            }
            set
            {
                ViewState["CurrentDescription"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsAdminOrContentAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            if (!Page.IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            try
            {
                BindNotInstalledWebParts();
                BindInstalledWebParts();
            }
            catch (ReflectionTypeLoadException)
            {
                lblNoAvailableWebParts.Text = MyPageResources.WebPartAdminMediumTrustMessage;
            }

        }

        private void BindNotInstalledWebParts()
        {
            grdAvailableParts.DataSource = WebPartHelper.GetUninstalledWebPartsFromAssemblies(siteSettings.SiteId, WebConfigSettings.AssembliesNotSearchedForWebParts);
            grdAvailableParts.DataBind();
            if (grdAvailableParts.Rows.Count == 0)
            {
                this.lblNoAvailableWebParts.Text = MyPageResources.WebPartAllInstalledMessage;
            }
        }


        private void BindInstalledWebParts()
        {
            DataTable dt = WebPartContent.SelectPage(
                siteSettings.SiteId,
                this.pageNumber,
                this.pageSize,
                this.sortByClassName,
                this.sortByAssemblyName);

            if (dt.Rows.Count > 0)
            {
                totalPages = Convert.ToInt32(dt.Rows[0]["TotalPages"], CultureInfo.InvariantCulture);
            }

            this.grdWebParts.DataSource = dt;

            this.grdWebParts.PageIndex = this.pageNumber;
            this.grdWebParts.PageSize = this.pageSize;
            this.grdWebParts.DataBind();

            if (this.totalPages > 1)
            {
                Literal pageLinks = new Literal();
                string pageUrl = SiteRoot 
                    + "/Admin/WebPartAdmin.aspx"
                    + "?sort=" + this.sort
                    + "&amp;pagenumber=";

                pageLinks.Text = UIHelper.GetPagerLinksWithPrevNext(
                    pageUrl, 
                    1, 
                    this.totalPages, 
                    this.pageNumber, "modulepager", "selectedpage");

                this.spnPager.Controls.Add(pageLinks);
            }
        }


        protected void grdAvailableParts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            GridView grid = (GridView)sender;
            grid.EditIndex = e.NewEditIndex;
            BindNotInstalledWebParts();
        }


        protected void grdAvailableParts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            switch (e.CommandName)
            {
                case "edit":
                    this.CurrentAssembly = e.CommandArgument.ToString();
                    if (this.CurrentAssembly.IndexOf(",") > -1)
                    {
                        this.CurrentAssembly = this.CurrentAssembly.Substring(0, this.CurrentAssembly.IndexOf(","));
                    }
                    break;

            }
        }


        protected void grdAvailableParts_DataBound(object sender, EventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            GridView grid = (GridView)sender;

            if (grid.EditIndex > -1)
            {
                String FullName = grid.DataKeys[grid.EditIndex].Value.ToString();
                this.CurrentClass = FullName;

                Label lblClassName = (Label)grid.Rows[grid.EditIndex].Cells[1].FindControl("lblClassName");
                lblClassName.Text = FullName;

                Label lblAssemblyName = (Label)grid.Rows[grid.EditIndex].Cells[1].FindControl("lblAssemblyName");
                lblAssemblyName.Text = this.CurrentAssembly;

                Assembly assembly;
                object obj = null;
                String path = HttpContext.Current.Server.MapPath("~/bin")
                    + Path.DirectorySeparatorChar + this.CurrentAssembly + ".dll";
                assembly = Assembly.LoadFrom(path);
                Type type = assembly.GetType(FullName, true, true);
                obj = Activator.CreateInstance(type);
                if (obj != null)
                {
                    WebPart webPart = (WebPart)obj;

                    Label lblTitle = (Label)grid.Rows[grid.EditIndex].Cells[1].FindControl("lblTitle");
                    lblTitle.Text = webPart.Title;
                    this.CurrentTitle = webPart.Title;

                    Label lblDescription = (Label)grid.Rows[grid.EditIndex].Cells[1].FindControl("lblDescription");
                    lblDescription.Text = webPart.Description + "&nbsp;";
                    this.CurrentDescription = webPart.Description;

                }

                DropDownList ddIcons = (DropDownList)grid.Rows[grid.EditIndex].Cells[1].FindControl("ddIcons");
                ddIcons.DataSource = SiteUtils.GetFeatureIconList();
                ddIcons.DataBind();
                ddIcons.Items.Insert(0, new ListItem(MyPageResources.ModuleSettingsNoIconLabel, "blank.gif"));
                ddIcons.Attributes.Add("onChange", "javascript:showIcon(this);");
                ddIcons.Attributes.Add("size", "6");
                HtmlImage imgIcon = (HtmlImage)grid.Rows[grid.EditIndex].Cells[1].FindControl("imgIcon");
                SetupIconScript(imgIcon);

            }

        }


        protected void grdAvailableParts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;

            GridView grid = (GridView)sender;

            CheckBox chkAvailableForMyPage = (CheckBox)grid.Rows[e.RowIndex].Cells[1].FindControl("chkAvailableForMyPage");
            CheckBox chkAllowMultipleInstancesOnMyPage = (CheckBox)grid.Rows[e.RowIndex].Cells[1].FindControl("chkAllowMultipleInstancesOnMyPage");
            CheckBox chkAvailableForContentSystem = (CheckBox)grid.Rows[e.RowIndex].Cells[1].FindControl("chkAvailableForContentSystem");
            DropDownList ddIcons = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddIcons");

            WebPartContent webPartContent = new WebPartContent();
            webPartContent.AllowMultipleInstancesOnMyPage = chkAllowMultipleInstancesOnMyPage.Checked;
            webPartContent.AssemblyName = this.CurrentAssembly;
            webPartContent.AvailableForContentSystem = chkAvailableForContentSystem.Checked;
            webPartContent.AvailableForMyPage = chkAvailableForMyPage.Checked;
            webPartContent.ClassName = this.CurrentClass;
            webPartContent.Description = this.CurrentDescription;
            webPartContent.ImageUrl = ddIcons.SelectedValue;
            webPartContent.SiteId = siteSettings.SiteId;
            webPartContent.SiteGuid = siteSettings.SiteGuid;
            webPartContent.Title = this.CurrentTitle;
            webPartContent.Save();


            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        protected void grdAvailableParts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        protected void grdWebParts_Sorting(object sender, GridViewSortEventArgs e)
        {
            // TODO: sorting

            //String redirectUrl = WebUtils.GetSiteRoot()
            //    + "/Default.aspx?pageid=" + siteSettings.ActivePage.PageID.ToString()
            //    + "&pagenumber"
            //    + this.ModuleID.ToString()
            //    + "=" + this.pageNumber.ToString()
            //    + "&sort"
            //    + this.ModuleID.ToString()
            //    + "=" + e.SortExpression;

            //WebUtils.SetupRedirect(this, redirectUrl);

        }



        private void SetupIconScript(HtmlImage imgIcon)
        {
            string logoScript = "<script type=\"text/javascript\">"
                + "function showIcon(listBox) { if(!document.images) return; "
                + "var iconPath = '" + iconPath + "'; "
                + "document.images." + imgIcon.ClientID + ".src = iconPath + listBox.value;"
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showIcon", logoScript);

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, MyPageResources.AdminMenuWebPartAdminLink);

            lnkAdminMenu.Text = MyPageResources.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkAdvancedTools.Text = MyPageResources.AdvancedToolsLink;
            lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

            lnkWebPartAdmin.Text = MyPageResources.AdminMenuWebPartAdminLink;
            lnkWebPartAdmin.NavigateUrl = SiteRoot + "/Admin/WebPartAdmin.aspx";
            
            
            lblNoAvailableWebParts.Text = String.Empty;
            this.grdAvailableParts.Columns[0].HeaderText = MyPageResources.WebPartAvailableNameLabel;

            this.grdWebParts.Columns[1].HeaderText = MyPageResources.WebPartTitleLabel;
            this.grdWebParts.Columns[2].HeaderText = MyPageResources.WebPartDescriptionLabel;
            this.grdWebParts.Columns[3].HeaderText = MyPageResources.WebPartClassNameLabel;
            this.grdWebParts.Columns[4].HeaderText = MyPageResources.WebPartAvailableForMyPageLabel;
            this.grdWebParts.Columns[5].HeaderText = MyPageResources.WebPartAllowMultipleInstancesOnMyPageLabel;
            this.grdWebParts.Columns[6].HeaderText = MyPageResources.WebPartAvailableForContentSystemLabel;

            
        }

        private void LoadSettings()
        {
            pageNumberParam = "pagenumber";
            pageNumber = WebUtils.ParseInt32FromQueryString(pageNumberParam, 1);
            sortParam = "sort";

            if (Page.Request.Params[sortParam] != null)
            {
                sort = Page.Request.Params[sortParam];
                switch (sort)
                {
                    case "Title":
                        this.sortByClassName = false;
                        this.sortByAssemblyName = false;
                        break;

                    case "ClassName":
                        this.sortByClassName = true;
                        this.sortByAssemblyName = false;
                        break;

                    case "AssemblyName":
                        this.sortByClassName = false;
                        this.sortByAssemblyName = true;

                        break;

                }
            }

            iconPath = Page.ResolveUrl("~/Data/SiteImages/FeatureIcons/");

            AddClassToBody("administration");
            AddClassToBody("webpartadmin");
            
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grdAvailableParts.RowEditing += new GridViewEditEventHandler(grdAvailableParts_RowEditing);
            this.grdAvailableParts.RowUpdating += new GridViewUpdateEventHandler(grdAvailableParts_RowUpdating);
            this.grdAvailableParts.RowCancelingEdit += new GridViewCancelEditEventHandler(grdAvailableParts_RowCancelingEdit);
            this.grdAvailableParts.RowCommand += new GridViewCommandEventHandler(grdAvailableParts_RowCommand);
            this.grdAvailableParts.DataBound += new EventHandler(grdAvailableParts_DataBound);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        #endregion
    }
}
#endif
