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

    public partial class ModulePermissionsPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ModulePermissionsPage));

        private const string viewPermission = "v";
        private const string editPermission = "e";
        private const string draftEditPermission = "d";
        private const string draftApprovalPermission = "a";
        

        private string currentPermission = string.Empty;

        private int pageId = -1;
        private int moduleId = -1;
        private string headingFormat = string.Empty;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private Module module = null;

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (!isAdmin && !isContentAdmin && !isSiteEditor)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (module == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;

            }

            if ((!isAdmin) && (module.AuthorizedEditRoles == "Admins;"))
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
                    if (module.ViewRoles.LastIndexOf("All Users") > -1)
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
                            if (module.ViewRoles == "Admins;")
                            {
                                rbAdminsOnly.Checked = true;
                                rbUseRoles.Checked = false;
                            }
                            else
                            {
                                rbAdminsOnly.Checked = false;
                                rbUseRoles.Checked = true;
                                if (module.ViewRoles.LastIndexOf(listItem.Value + ";") > -1)
                                {
                                    listItem.Selected = true;
                                }
                            }
                            break;

                        case editPermission:
                            if (module.AuthorizedEditRoles == "Admins;")
                            {
                                rbAdminsOnly.Checked = true;
                                rbUseRoles.Checked = false;
                            }
                            else
                            {
                                rbAdminsOnly.Checked = false;
                                rbUseRoles.Checked = true;
                                if (module.AuthorizedEditRoles.LastIndexOf(listItem.Value + ";") > -1)
                                {
                                    listItem.Selected = true;
                                }
                            }

                            break;
                        case draftEditPermission:
                           

                            if (module.DraftEditRoles.LastIndexOf(listItem.Value + ";") > -1)
                            {
                                listItem.Selected = true;
                            }
                           
                            break;

                        case draftApprovalPermission:
                            if (module.DraftApprovalRoles.LastIndexOf(listItem.Value + ";") > -1)
                            {
                                listItem.Selected = true;
                            }
                            break;
                        

                    }

                    chkAllowedRoles.Items.Add(listItem);

                }
            }

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            bool reIndex = false;

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
                        if (module.ViewRoles != "Admins;")
                        {
                            module.ViewRoles = "Admins;";
                            reIndex = true;

                            log.Info("user " + userName + " changed Module View roles for " + module.ModuleTitle
                                   + " to Admins "
                                   + " from ip address " + SiteUtils.GetIP4Address());
                        }
                        break;

                    case editPermission:
                        if (module.AuthorizedEditRoles != "Admins;")
                        {
                            module.AuthorizedEditRoles = "Admins;";
                        }

                        log.Info("user " + userName + " changed Module Edit roles for " + module.ModuleTitle
                                   + " to Admins "
                                   + " from ip address " + SiteUtils.GetIP4Address());

                        break;

                    
                }
            }
            else
            {
                string authorizedRoles = chkAllowedRoles.Items.SelectedItemsToSemiColonSeparatedString();

                switch (currentPermission)
                {
                    case viewPermission:
                        if (module.ViewRoles != authorizedRoles)
                        {
                            module.ViewRoles = authorizedRoles;
                            reIndex = true;

                            log.Info("user " + userName + " changed Module View roles for " + module.ModuleTitle
                                    + " to " + authorizedRoles
                                    + " from ip address " + SiteUtils.GetIP4Address());
                        }
                        break;

                    case editPermission:
                        module.AuthorizedEditRoles = authorizedRoles;

                        log.Info("user " + userName + " changed Module Edit roles for " + module.ModuleTitle
                                    + " to " + authorizedRoles
                                    + " from ip address " + SiteUtils.GetIP4Address());

                        break;

                    case draftEditPermission:
                        module.DraftEditRoles = authorizedRoles;

                        log.Info("user " + userName + " changed Module Draft Edit roles for " + module.ModuleTitle
                                    + " to " + authorizedRoles
                                    + " from ip address " + SiteUtils.GetIP4Address());
                        break;

                    case draftApprovalPermission:
                        module.DraftApprovalRoles = authorizedRoles;

                        log.Info("user " + userName + " changed Module Draft Approval roles for " + module.ModuleTitle
                                    + " to " + authorizedRoles
                                    + " from ip address " + SiteUtils.GetIP4Address());
                        break;
                   
                }
            }

            module.Save();
            if (reIndex)
            {
                CacheHelper.ClearModuleCache(module.ModuleId);
                if (pageId > -1)
                {
                    mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(CurrentPage);
                }
                SiteUtils.QueueIndexing();
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        private void PopulateLabels()
        {
            heading.Text = string.Format(CultureInfo.InvariantCulture, headingFormat, module.ModuleTitle);
            Title = heading.Text;

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Admin/ModuleSettings.aspx?pageid="
                    + pageId.ToInvariantString()
                    + "&amp;mid=" + moduleId.ToInvariantString()
                    + "' class='unselectedcrumb'>" + Resource.ModuleSettingsSettingsLabel
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
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            
            currentPermission = WebUtils.ParseStringFromQueryString("p", currentPermission);
            isAdmin = WebUser.IsAdmin;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            if (pageId > -1)
            { 
                module = new Module(moduleId, pageId);
            }
            else
            {
                module = new Module(moduleId);
            }

            if (module.SiteId != siteSettings.SiteId) { module = null; return; }

            switch (currentPermission)
            {
                case viewPermission:
                    headingFormat = Resource.ModuleViewPermissionsFormat;
                    break;

                case editPermission:
                    headingFormat = Resource.ModuleEditPermissionsFormat;
                    break;

                case draftEditPermission:
                    headingFormat = Resource.ModuleDraftEditPermissionsFormat;
                    break;
                case draftApprovalPermission:
                    headingFormat = Resource.ModuleDraftApprovalPermissionsFormat;
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
            if (currentPermission == draftEditPermission || currentPermission == draftApprovalPermission) { return; }
            if (!isAdmin) { return; }

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
