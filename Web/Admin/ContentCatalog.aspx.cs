/// Author:					
/// Created:				2006-05-16
/// Last Modified:			2018-03-28
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
using System.Globalization;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class ContentCatalogPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContentCatalogPage));

        private string sortParam;
        private String sort = "ModuleTitle";
        private int pageNumber = 1;
        private int pageSize = WebConfigSettings.ContentCatalogPageSize;
        private int totalPages = 0;
        
        private bool sortByFeature = false;
        private bool sortByAuthor = false;
        private string skinBaseUrl;
        private bool isContentAdmin = false;
        private bool isAdmin = false;
        private bool isSiteEditor = false;
        private int moduleDefId = -1;
        private string title = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadSettings();
            if ((!isContentAdmin)&& (!isSiteEditor))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            BindFeatureList();
            BindGrid();

            if (moduleDefId > -1)
            {
                chkFilterByFeature.Checked = true;
                ListItem item = ddModuleType.Items.FindByValue(moduleDefId.ToInvariantString());
                if (item != null)
                {
                    ddModuleType.ClearSelection();
                    item.Selected = true;
                }
            }
            txtTitleFilter.Text = title;
            
        }

        private void BindGrid()
        {

            DataTable dt = Module.SelectPage(
                siteSettings.SiteId,
                moduleDefId,
                title,
                pageNumber,
                pageSize,
                sortByFeature,
                sortByAuthor,
                out totalPages);

            string pageUrl = SiteRoot + "/Admin/ContentCatalog.aspx"
                + "?md=" + moduleDefId.ToInvariantString()
                + "&amp;title=" + Server.UrlEncode(title)
                + "&amp;sort=" + this.sort
                + "&amp;pagenumber={0}";

            pgrContent.PageURLFormat = pageUrl;
            pgrContent.ShowFirstLast = true;
            pgrContent.CurrentIndex = pageNumber;
            pgrContent.PageSize = pageSize;
            pgrContent.PageCount = totalPages;
            pgrContent.Visible = (totalPages > 1);

            grdContent.DataSource = dt;
            grdContent.DataBind();

        }

        private void BindFeatureList()
        {
            using (IDataReader reader = ModuleDefinition.GetUserModules(siteSettings.SiteId))
            {
                ListItem listItem;
                while (reader.Read())
                {
                    string allowedRoles = reader["AuthorizedRoles"].ToString();
                    if (WebUser.IsInRoles(allowedRoles))
                    {
                        listItem = new ListItem(
                            ResourceHelper.GetResourceString(
                            reader["ResourceFile"].ToString(),
                            reader["FeatureName"].ToString()),
                            reader["ModuleDefID"].ToString());

                        ddModuleType.Items.Add(listItem);
                    }

                }

            }

            btnCreateNewContent.Enabled = (ddModuleType.Items.Count > 0);

        }

        protected void btnCreateNewContent_Click(object sender, EventArgs e)
        {
            Page.Validate("contentcatalog");
            if (!Page.IsValid) { return; }

            int moduleDefID = int.Parse(ddModuleType.SelectedItem.Value, CultureInfo.InvariantCulture);
            ModuleDefinition moduleDefinition = new ModuleDefinition(moduleDefID);

            Module module = new Module();
            module.ModuleTitle = this.txtModuleTitle.Text;
            module.ModuleDefId = moduleDefID;
            module.FeatureGuid = moduleDefinition.FeatureGuid;
            module.Icon = moduleDefinition.Icon;
            module.SiteId = siteSettings.SiteId;
            module.SiteGuid = siteSettings.SiteGuid;
            module.CreatedByUserId = SiteUtils.GetCurrentSiteUser().UserId;
            module.CacheTime = moduleDefinition.DefaultCacheTime;
            module.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
            module.HeadElement = WebConfigSettings.ModuleTitleTag;
            module.Save();
            WebUtils.SetupRedirect(this, SiteRoot
                + "/Admin/ContentManagerPreview.aspx?mid="
                + module.ModuleId.ToInvariantString()
                );


        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuContentManagerLink);
            heading.Text = Resource.AdminMenuContentManagerLink;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkContentManager.Text = Resource.AdminMenuContentManagerLink;
            lnkContentManager.ToolTip = Resource.AdminMenuContentManagerLink;
            lnkContentManager.NavigateUrl = SiteRoot + "/Admin/ContentCatalog.aspx";
            
            this.grdContent.Columns[0].HeaderText = Resource.ContentManagerContentTitleColumnHeader;
            this.grdContent.Columns[1].HeaderText = Resource.ContentManagerFeatureTypeColumnHeader;
            this.grdContent.Columns[2].HeaderText = Resource.ContentManagerAuthorColumnHeader;

            this.btnCreateNewContent.Text = Resource.ContentManagerCreateNewContentButton;
            SiteUtils.SetButtonAccessKey
                (btnCreateNewContent, AccessKeys.ContentManagerCreateNewContentButtonAccessKey);

            //viewEditText = Resource.ContentManagerViewEditLabel;
            //publishingText = Resource.ContentManagerPublishDeleteLabel;
            if (!Page.IsPostBack)
            {
                if (WebConfigSettings.PrePopulateDefaultContentTitle)
                {
                    txtModuleTitle.Text = Resource.PageLayoutDefaultNewModuleName;
                }
            }

            chkFilterByFeature.Text = Resource.ContentManagerFilterByFeature;
            btnFind.Text = Resource.ContentManagerFindButton;

            reqModuleTitle.ErrorMessage = Resource.TitleRequiredWarning;
            reqModuleTitle.Enabled = WebConfigSettings.RequireContentTitle;

            cvModuleTitle.ValueToCompare = Resource.PageLayoutDefaultNewModuleName;
            cvModuleTitle.ErrorMessage = Resource.DefaultContentTitleWarning;
            cvModuleTitle.Enabled = WebConfigSettings.RequireChangeDefaultContentTitle;
            
        }

        void btnFind_Click(object sender, EventArgs e)
        {
            if (chkFilterByFeature.Checked)
            {
                int.TryParse(ddModuleType.SelectedValue, out moduleDefId);
            }
            else
            {
                moduleDefId = -1;
            }

            title = txtTitleFilter.Text;

            String redirectUrl = SiteRoot
                + "/Admin/ContentCatalog.aspx"
                + "?md=" + moduleDefId.ToInvariantString()
                + "&title=" + Server.UrlEncode(title)
                + "&sort=" + sort
                + "&pagenumber=" + pageNumber.ToInvariantString();

            WebUtils.SetupRedirect(this, redirectUrl);

        }

       

        void grdContent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (isAdmin) { return; }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = e.Row.DataItem as DataRowView;
                if (row != null)
                {
                    if (row["AuthorizedEditRoles"].ToString() == "Admins;")
                    {
                        e.Row.Visible = false;
                        
                    }

                }
            }
        }

        protected void grdContent_Sorting(object sender, GridViewSortEventArgs e)
        {
            String redirectUrl = SiteRoot
                + "/Admin/ContentCatalog.aspx"
                + "?md=" + moduleDefId.ToInvariantString()
                + "&title=" + Server.UrlEncode(title)
                + "&sort=" + e.SortExpression
                + "&pagenumber=" + pageNumber.ToInvariantString();

            WebUtils.SetupRedirect(this, redirectUrl);

        }

        private void LoadSettings()
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            isContentAdmin = WebUser.IsAdminOrContentAdmin;
            isAdmin = WebUser.IsAdmin;
            skinBaseUrl = SiteUtils.GetSkinBaseUrl(this);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            moduleDefId = WebUtils.ParseInt32FromQueryString("md", moduleDefId);

            AddClassToBody("administration");
            AddClassToBody("cmadmin");

            if (Request.QueryString["title"] != null)
            {
                title = Request.QueryString["title"];
            }

            sortParam = "sort";

            if (Page.Request.Params[sortParam] != null)
            {
                sort = Page.Request.Params[sortParam];
                switch (sort)
                {
                    

                    case "FeatureName":
                        this.sortByFeature = true;
                        this.sortByAuthor = false;
                        break;

                    case "CreatedBy":
                        this.sortByFeature = false;
                        this.sortByAuthor = true;

                        break;

                    case "ModuleTitle":
                    default:
                        this.sortByFeature = false;
                        this.sortByAuthor = false;
                        sort = "ModuleTitle";

                        break;

                }
            }

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnCreateNewContent.Click += new EventHandler(btnCreateNewContent_Click);
            this.grdContent.Sorting += new GridViewSortEventHandler(grdContent_Sorting);
            grdContent.RowDataBound += new GridViewRowEventHandler(grdContent_RowDataBound);
           
            btnFind.Click += new EventHandler(btnFind_Click);

            SuppressMenuSelection();
            SuppressPageMenu();

            ScriptConfig.IncludeJQTable = true;
            
        }

        

        

        

        #endregion
    }
}
