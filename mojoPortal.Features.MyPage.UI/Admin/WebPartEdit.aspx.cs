/// Author:					    
/// Created:				    2006-06-10
/// Last Modified:			    2011-02-26
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
    public partial class WebPartEdit : NonCmsBasePage
    {
        #region OnInit
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        #endregion

        private Guid webPartID = Guid.Empty;
        private string iconPath = WebUtils.GetSiteRoot() + "/Data/SiteImages/FeatureIcons/";
        private bool isSiteEditor = false;

        /// <summary>
        /// Handles the Load event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsAdminOrContentAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }


            SecurityHelper.DisableBrowserCache();

            if ((Request.Params.Get("part") != null)&&(Request.Params.Get("part").Length == 36))
            {
                webPartID = new Guid(Request.Params.Get("part"));

            }

            PopulateLabels();

            if (!Page.IsPostBack)
            {
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();

                }
                PopulateControls();
            }
        }


        /// <summary>
        /// Populates the controls.
        /// </summary>
        private void PopulateControls()
        {
            this.ddIcons.DataSource = SiteUtils.GetFeatureIconList();
            this.ddIcons.DataBind();
            ddIcons.Items.Insert(0, new ListItem(MyPageResources.ModuleSettingsNoIconLabel, "blank.gif"));
            ddIcons.Attributes.Add("onChange", "javascript:showIcon(this);");
            ddIcons.Attributes.Add("size", "6");
            SetupIconScript(this.imgIcon);

            if (webPartID != Guid.Empty)
            {
                WebPartContent webPartContent = new WebPartContent(webPartID);
                if (webPartContent.SiteId == siteSettings.SiteId)
                {
                    ListItem item = this.ddIcons.Items.FindByValue(webPartContent.ImageUrl);
                    if (item != null)
                    {
                        ddIcons.ClearSelection();
                        ddIcons.Items.FindByValue(webPartContent.ImageUrl).Selected = true;
                    }
                    imgIcon.Src = this.iconPath + webPartContent.ImageUrl;
                    lblClassName.Text = webPartContent.ClassName;
                    lblAssemblyName.Text = webPartContent.AssemblyName;
                    lblTitle.Text = webPartContent.Title;
                    lblDescription.Text = webPartContent.Description;
                    chkAvailableForMyPage.Checked = webPartContent.AvailableForMyPage;
                    chkAllowMultipleInstancesOnMyPage.Checked = webPartContent.AllowMultipleInstancesOnMyPage;
                    chkAvailableForContentSystem.Checked = webPartContent.AvailableForContentSystem;
                }

            }
        }


        /// <summary>
        /// Populates the labels.
        /// </summary>
        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, MyPageResources.AdminMenuWebPartAdminLink);

            lnkAdminMenu.Text = MyPageResources.AdminMenuLink;
            lnkAdminMenu.ToolTip = MyPageResources.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";
            lnkWebPartAdmin.Text = MyPageResources.AdminMenuWebPartAdminLink;
            lnkWebPartAdmin.ToolTip = MyPageResources.AdminMenuWebPartAdminLink;
            lnkWebPartAdmin.NavigateUrl = SiteRoot + "/Admin/WebPartAdmin.aspx";
            btnUpdate.Text = MyPageResources.WebPartEditUpdateButton;
            btnCancel.Text = MyPageResources.WebPartEditCancelButton;
            btnDelete.Text = MyPageResources.WebPartEditDeleteButton;
            UIHelper.AddConfirmationDialog(btnDelete, MyPageResources.WebPartEditDeleteWarning);

            AddClassToBody("administration");
            AddClassToBody("webpartedit");

        }


        /// <summary>
        /// Handles the Click event of the btnUpdate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.webPartID != Guid.Empty)
            {
                WebPartContent webPartContent = new WebPartContent(webPartID);
                if (webPartContent.SiteId == siteSettings.SiteId)
                {
                    webPartContent.AvailableForMyPage = chkAvailableForMyPage.Checked;
                    webPartContent.AllowMultipleInstancesOnMyPage = chkAllowMultipleInstancesOnMyPage.Checked;
                    webPartContent.AvailableForContentSystem = chkAvailableForContentSystem.Checked;
                    webPartContent.ImageUrl = ddIcons.SelectedValue;
                    webPartContent.Save();
                }

            }

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

            
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.webPartID != Guid.Empty)
            {
                WebPartContent webPartContent = new WebPartContent(webPartID);
                if (webPartContent.SiteId == siteSettings.SiteId)
                {
                    WebPartContent.DeleteWebPart(webPartID);
                }
            }

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        /// <summary>
        /// Setups the icon script.
        /// </summary>
        /// <param name="imgIcon">The HtmlImage with runat=server.</param>
        private void SetupIconScript(HtmlImage imgIcon)
        {
            string logoScript = "<script type=\"text/javascript\">"
                + "function showIcon(listBox) { if(!document.images) return; "
                + "var iconPath = '" + iconPath + "'; "
                + "document.images." + imgIcon.ClientID + ".src = iconPath + listBox.value;"
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showIcon", logoScript);

        }


    }
}
#endif
