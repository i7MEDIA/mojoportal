// Author:					
// Created:					2012-01-14
// Last Modified:			2018-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.SearchIndex;
using Resources;
using log4net;



namespace mojoPortal.Web.AdminUI
{

    public partial class PagePermissionPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PagePermissionPage));

        private const string viewPermission = "v";
        private const string editPermission = "e";
        private const string draftEditPermission = "d";
        private const string draftApprovalPermission = "a"; //joe davis
        private const string childEditPermission = "ce";

        private string currentPermission = string.Empty;

        private int pageId = -1;
        private string headingFormat = string.Empty;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (pageId == -1)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (!isAdmin && !isContentAdmin && !isSiteEditor)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if ((!isAdmin) && (CurrentPage.EditRoles == "Admins;"))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (currentPermission.Length == 0)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            if (!IsPostBack)
            {
                PopulateControls();
            }

        }

        private void PopulateControls()
        {
            chkAllowedRoles.Items.Clear();

            ListItem allItem = new ListItem();
            // localize display
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";

            switch (currentPermission)
            {
                case viewPermission:
                    if (CurrentPage.AuthorizedRoles.LastIndexOf("All Users") > -1)
                    {
                        allItem.Selected = true;
                    }
                    chkAllowedRoles.Items.Add(allItem);

                    break;

                case draftEditPermission:
                case draftApprovalPermission:

                    divRadioButtons.Visible = false;

                    break;
				

                default:


                    break;

            }

            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    string roleName = reader["RoleName"].ToString();

                    // no need or benefit to checking content admins role
                    // since they are not limited by roles except the special case of Admins only role
                    if (roleName == Role.ContentAdministratorsRole) { continue; }
                    // administrators role doensn't need permission, the only reason to show it is so that
                    // an admin can lock the content down to only admins
                    if (roleName == Role.AdministratorsRole) { continue; }

                    

                    ListItem listItem = new ListItem();

                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    

                    switch (currentPermission)
                    {
                        case viewPermission:
                            if (CurrentPage.AuthorizedRoles == "Admins;")
                            {
                                rbAdminsOnly.Checked = true;
                                rbUseRoles.Checked = false;
                            }
                            else
                            {
                                rbAdminsOnly.Checked = false;
                                rbUseRoles.Checked = true;
                                if (CurrentPage.AuthorizedRoles.LastIndexOf(listItem.Value + ";") > -1)
                                {
                                    listItem.Selected = true;
                                }
                            }
                            break;

                        case editPermission:
                            if (CurrentPage.EditRoles == "Admins;")
                            {
                                rbAdminsOnly.Checked = true;
                                rbUseRoles.Checked = false;
                            }
                            else
                            {
                                rbAdminsOnly.Checked = false;
                                rbUseRoles.Checked = true;
                                if (CurrentPage.EditRoles.LastIndexOf(listItem.Value + ";") > -1)
                                {
                                    listItem.Selected = true;
                                }
                            }

                            break;
                        case draftEditPermission:
                            //if (CurrentPage.DraftEditOnlyRoles == "Admins;")
                            //{
                            //    rbAdminsOnly.Checked = true;
                            //    rbUseRoles.Checked = false;
                            //}
                            //else
                            //{
                            //    rbAdminsOnly.Checked = false;
                            //    rbUseRoles.Checked = true;
                                if (CurrentPage.DraftEditOnlyRoles.LastIndexOf(listItem.Value + ";") > -1)
                                {
                                    listItem.Selected = true;
                                }
                            //}
                            break;
						//joe davis
                        case draftApprovalPermission:
                            if (CurrentPage.DraftApprovalRoles.LastIndexOf(listItem.Value + ";") > -1)
                            {
                                listItem.Selected = true;
                            }
                            break;

                        case childEditPermission:
                            if (CurrentPage.CreateChildPageRoles == "Admins;")
                            {
                                rbAdminsOnly.Checked = true;
                                rbUseRoles.Checked = false;
                            }
                            else
                            {
                                rbAdminsOnly.Checked = false;
                                rbUseRoles.Checked = true;
                                if (CurrentPage.CreateChildPageRoles.LastIndexOf(listItem.Value + ";") > -1)
                                {
                                    listItem.Selected = true;
                                }
                            }
                            break;

                    }

                    chkAllowedRoles.Items.Add(listItem);

                }
            }

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            bool reIndexPage = false;

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            string userName = string.Empty;
            if (currentUser != null)
            {
                userName = currentUser.Name;
            }

            if (rbAdminsOnly.Checked)
            {
                switch (currentPermission)
                {
                    case viewPermission:
                        if (CurrentPage.AuthorizedRoles != "Admins;")
                        {
                            log.Info("user " + userName + " changed page View roles for " + CurrentPage.PageName
                                + " to Admins "
                                + " from ip address " + SiteUtils.GetIP4Address());

                            CurrentPage.AuthorizedRoles = "Admins;";
                            reIndexPage = true;
                        }
                        break;

                    case editPermission:
                        if (CurrentPage.EditRoles != "Admins;")
                        {
                            log.Info("user " + userName + " changed page Edit roles for " + CurrentPage.PageName
                                + " to Admins "
                                + " from ip address " + SiteUtils.GetIP4Address());

                            CurrentPage.EditRoles = "Admins;";
                        }
                        break;

                    //case draftEditPermission:
                    //    if (CurrentPage.DraftEditOnlyRoles != "Admins;")
                    //    {
                    //        CurrentPage.DraftEditOnlyRoles = "Admins;";
                    //    }
                    //    break;

                    case childEditPermission:
                        if (CurrentPage.CreateChildPageRoles != "Admins;")
                        {
                            log.Info("user " + userName + " changed page Child Edit Roles roles for " + CurrentPage.PageName
                                + " to Admins "
                                + " from ip address " + SiteUtils.GetIP4Address());

                            CurrentPage.CreateChildPageRoles = "Admins;";
                        }
                        break;
                }
            }
            else
            {
                string authorizedRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();

                switch (currentPermission)
                {
                    case viewPermission:
                        if (CurrentPage.AuthorizedRoles != authorizedRoles)
                        {
                            log.Info("user " + userName + " changed page View roles for " + CurrentPage.PageName
                                + " to " + authorizedRoles
                                + " from ip address " + SiteUtils.GetIP4Address());

                            CurrentPage.AuthorizedRoles = authorizedRoles;
                            reIndexPage = true;
                        }
                        break;

                    case editPermission:
                          CurrentPage.EditRoles = authorizedRoles;
                          log.Info("user " + userName + " changed page Edit roles for " + CurrentPage.PageName
                                  + " to " + authorizedRoles
                                  + " from ip address " + SiteUtils.GetIP4Address());
                        break;

                    case draftEditPermission:
                        CurrentPage.DraftEditOnlyRoles = authorizedRoles;
                        log.Info("user " + userName + " changed page Draft Edit roles for " + CurrentPage.PageName
                                + " to " + authorizedRoles
                                + " from ip address " + SiteUtils.GetIP4Address());
                        break;
					//joe davis
                    case draftApprovalPermission:
                        CurrentPage.DraftApprovalRoles = authorizedRoles;
                        log.Info("user " + userName + " changed page Draft Approval roles for " + CurrentPage.PageName
                                + " to " + authorizedRoles
                                + " from ip address " + SiteUtils.GetIP4Address());
                        break;

                    case childEditPermission:
                        CurrentPage.CreateChildPageRoles = authorizedRoles;
                        log.Info("user " + userName + " changed page Child Page Edit Roles for " + CurrentPage.PageName
                                + " to " + authorizedRoles
                                + " from ip address " + SiteUtils.GetIP4Address());
                        break;

                }
            }

            CurrentPage.Save();
            if (reIndexPage)
            {
                CacheHelper.ResetSiteMapCache();
                mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(CurrentPage);
                SiteUtils.QueueIndexing();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void PopulateLabels()
        {
            heading.Text = string.Format(CultureInfo.InvariantCulture, headingFormat, CurrentPage.PageName);
            Title = heading.Text;

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Admin/PageSettings.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + Resource.PageSettingsPageTitle
                    + "</a>" + crumbs.ItemWrapperBottom + crumbs.Separator + crumbs.ItemWrapperTop 
                    + "<a href='" + SiteRoot + "/Admin/PagePermissionsMenu.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + Resource.PagePermissionsLink
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            rbAdminsOnly.Text = Resource.AdminsOnly;
            rbUseRoles.Text = Resource.RolesAllowed;

            btnSave.Text = Resource.SaveButton;

            AddClassToBody("administration");
        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            currentPermission = WebUtils.ParseStringFromQueryString("p", currentPermission);
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            switch (currentPermission)
            {
                case viewPermission:
                    headingFormat = Resource.PageViewPermissionsFormat;
                    break;

                case editPermission:
                    headingFormat = Resource.PageEditPermissionsFormat;
                    break;

                case draftEditPermission:
                    headingFormat = Resource.PageDraftEditPermissionFormat;
                    break;
				//joe davis
                case draftApprovalPermission:
                    headingFormat = Resource.PageDraftApprovalPermissionFormat;
                    break;

                case childEditPermission:
                    headingFormat = Resource.PageCreateChildPagePermissionFormat;
                    break;

                default:
                    // not found so reset because we redirect if this is empty
                    currentPermission = string.Empty;
                    break;
            }

            if (currentPermission.Length > 0) { SetupRoleToggleScript(); }

            if (!isAdmin)
            {
                // only admins can lock content down to only admins
                rbAdminsOnly.Enabled = false;
                rbUseRoles.Enabled = false;
            }

        }

        private void SetupRoleToggleScript()
        {
			//joe davis
            if (currentPermission == draftEditPermission || currentPermission == draftApprovalPermission) { return; }

            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");

            script.Append("function DeSelectRoles(chkBoxContainer) {");

            //script.Append("alert('function called'); ");

            script.Append("$(chkBoxContainer).find('input[type=checkbox]').each(function(){this.checked = false; }); ");

            //script.Append("}");

            script.Append("} ");

            script.Append("$(document).ready(function() {");

            script.Append("$('#" + rbAdminsOnly.ClientID + "').change(function(){");
            script.Append("var selectedVal = $('#" + rbAdminsOnly.ClientID + "').attr('checked'); ");
            script.Append("if(selectedVal === 'checked'){");
            script.Append("DeSelectRoles('#" + chkAllowedRoles.ClientID + "');}");
            script.Append("});");

            script.Append("}); ");

            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "roletoggle", script.ToString());

        }

        

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);

            AllowSkinOverride = (pageId > -1);
            base.OnPreInit(e);

            if (
                (pageId > -1)
                   && (siteSettings.AllowPageSkins)
                    && (CurrentPage != null)
                    && (CurrentPage.Skin.Length > 0)
                    )
            {

                if (Global.RegisteredVirtualThemes)
                {
                    this.Theme = "pageskin-" + siteSettings.SiteId.ToInvariantString() + CurrentPage.Skin;
                }
                else
                {
                    this.Theme = "pageskin";
                }
            }

        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            SuppressPageMenu();
        }

        

        #endregion
    }
}
