/// Author:						
/// Created:					2005-09-11
/// Last Modified:				2017-03-15
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    public partial class BlogCategoryEdit : NonCmsBasePage
	{
        
		private int pageId = -1;
		private int moduleId = -1;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.dlCategories.ItemCommand += new DataListCommandEventHandler(dlCategories_ItemCommand);
            this.dlCategories.ItemDataBound += new DataListItemEventHandler(dlCategories_ItemDataBound);
            this.btnAddCategory.Click += new EventHandler(btnAddCategory_Click);
            base.OnInit(e);
        }

        
        #endregion

        private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
            {
                SiteUtils.ForceSsl();
            }
            else
            {
                SiteUtils.ClearSsl();
            }
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            SecurityHelper.DisableBrowserCache();

            LoadParams();

            if (!UserCanEditModule(moduleId, Blog.FeatureGuid))
			{
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

			PopulateLabels();

			if (!Page.IsPostBack)
			{
				PopulateControls();
			}

		}


		private void PopulateLabels()
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, BlogResources.BlogEditCategoriesLabel);

            heading.Text = BlogResources.BlogCategoriesLabel;

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Blog/Manage.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='selectedcrumb'>" + BlogResources.Administration
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            //EditLabel = Resource.EditImageAltText;
            //DeleteLabel = Resource.BlogEditDeleteButton;
            //ApplyLabel = Resource.BlogEditUpdateButton;
            btnAddCategory.Text = BlogResources.BlogAddCategoriesLabel;
            btnAddCategory.ToolTip = BlogResources.BlogAddCategoriesLabel;
		}


		private void PopulateControls()
		{
            using (IDataReader reader = Blog.GetCategoriesList(this.moduleId))
            {
                this.dlCategories.DataSource = reader;
                this.dlCategories.DataBind();
            }
		}


		private void btnAddCategory_Click(Object sender, EventArgs e) 
		{
			if(this.txtNewCategoryName.Text.Length > 0)
			{
				Blog.AddBlogCategory(this.moduleId, this.txtNewCategoryName.Text);

				WebUtils.SetupRedirect(
						this, 
						SiteRoot + "/Blog/EditCategory.aspx?pageid=" + this.pageId.ToInvariantString() 
						+ "&mid=" + this.moduleId.ToInvariantString());

			}
		}


		private void dlCategories_ItemCommand(object sender, DataListCommandEventArgs e) 
		{
			int categoryId = Convert.ToInt32(this.dlCategories.DataKeys[e.Item.ItemIndex]);
			string categoryName;

			switch(e.CommandName)
			{
				case "edit":
					this.dlCategories.EditItemIndex = e.Item.ItemIndex;
					PopulateControls();
					break;

				case "apply":

					categoryName = ((TextBox) e.Item.FindControl("categoryName")).Text;
					if(categoryName.Length > 0)
					{
						Blog.UpdateBlogCategory(categoryId, categoryName);
					}


					this.dlCategories.EditItemIndex = -1;
					WebUtils.SetupRedirect(
						this, 
						SiteRoot + "/Blog/EditCategory.aspx?pageid=" 
                        + this.pageId.ToInvariantString() 
						+ "&mid=" + this.moduleId.ToInvariantString());

					break;

				case "delete":

					Blog.DeleteCategory(categoryId);
					this.dlCategories.EditItemIndex = -1;
					WebUtils.SetupRedirect(
						this, 
						SiteRoot + "/Blog/EditCategory.aspx?pageid=" 
                        + this.pageId.ToInvariantString()
						+ "&mid=" + this.moduleId.ToInvariantString());

					break;

			}
		}


        void dlCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnDelete = e.Item.FindControl("btnDelete") as ImageButton;
            UIHelper.AddConfirmationDialog(btnDelete, BlogResources.BlogDeleteCategoryWarning);
        }


        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            AddClassToBody("blogeditcategory");
        }
    

	}
}
