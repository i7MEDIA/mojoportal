// Author:					    
// Created:				        2012-04-09
// Last Modified:			    2012-04-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Security;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Net;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class UserRolesDialog : mojoDialogBasePage
    {
        private int userId = -1;
        private SiteUser siteUser = null;
        
        private bool isAdmin = false;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            
            if (
                    (
                   ((WebUser.IsInRoles(SiteInfo.RolesThatCanManageUsers)) || (WebUser.IsInRoles(SiteInfo.RolesThatCanCreateUsers)))
                    && !isAdmin)
                    )
            {
                // only admins can edit admins
                if (siteUser.IsInRoles("Admins"))
                {
                    SiteUtils.RedirectToAccessDeniedPage();
                    return;
                }

               // HideAdminControls();

            }
            else
            {
                if (!isAdmin)
                {
                    if (!Request.IsAuthenticated)
                    {
                        SiteUtils.RedirectToLoginPage(this);
                        return;
                    }

                    SiteUtils.RedirectToAccessDeniedPage();
                    return;
                }
            }

            PopulateLabels();

            if (siteUser != null)
            {
                if (!IsPostBack)
                {
                    BindRoles();
                }

            }


        }

        private void BindRoles()
        {
            if (userId == -1) { return; }
            if (SiteInfo == null) { return; }

            using (IDataReader reader = SiteUser.GetRolesByUser(SiteInfo.SiteId, userId))
            {
                userRoles.DataSource = reader;
                userRoles.DataBind();
            }

            
            using (IDataReader reader = Role.GetRolesUserIsNotIn(SiteInfo.SiteId, userId))
            {
                if (WebUser.IsAdmin)
                {
                    allRoles.DataSource = reader;
                    allRoles.DataBind();
                }
                else
                {
                    while (reader.Read())
                    {
                        string roleName = reader["RoleName"].ToString();
                        // only admins can add users to Admins role or Role Admins role
                        if ((roleName != "Admins") && (roleName != "Role Admins"))
                        {
                            if ((roleName != "Content Administrators") || (WebConfigSettings.AllowRoleAdminsToCreateContentManagers))
                            {
                                ListItem item = new ListItem(reader["DisplayName"].ToString(), reader["RoleID"].ToString());
                                allRoles.Items.Add(item);
                            }
                        }

                    }
                }
            }

            if (allRoles.Items.Count == 0)
            {
                allRoles.Enabled = false;
                addExisting.Enabled = false;
                addExisting.Text = Resource.ManageUsersUserIsInAllRolesMessage;

            }
            

        }

        private void AddRole_Click(Object sender, EventArgs e)
        {
            if ((userId > -1) && (SiteInfo != null))
            {
                SiteUser user = new SiteUser(SiteInfo, userId);
                int roleID = int.Parse(allRoles.SelectedItem.Value, CultureInfo.InvariantCulture);
                Role role = new Role(roleID);
                Role.AddUser(roleID, userId, role.RoleGuid, user.UserGuid);
                user.RolesChanged = true;
                user.Save();

                BindRoles();

            }

            //WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int roleID = Convert.ToInt32(userRoles.DataKeys[e.Item.ItemIndex]);
            SiteUser user = new SiteUser(SiteInfo, userId);

            Role.RemoveUser(roleID, userId);
            userRoles.EditItemIndex = -1;
            if (user.UserId > -1)
            {
                user.RolesChanged = true;
                user.Save();
            }

            BindRoles();

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            //return;

        }


        void userRoles_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnRemoveRole = e.Item.FindControl("btnRemoveRole") as ImageButton;
            UIHelper.AddConfirmationDialog(btnRemoveRole, Resource.ManageUsersRemoveRoleWarning);
        }

        protected bool CanDeleteUserFromRole(string roleName)
        {
            if (WebUser.IsAdmin) { return true; }

            if (roleName == "Admins") { return false; }
            if (roleName == "Role Admins") { return false; }

            return true;
        }

        private void PopulateLabels()
        {
            addExisting.Text = Resource.ManageUsersAddToRoleButton;
            addExisting.ToolTip = Resource.ManageUsersAddToRoleButton;
            SiteUtils.SetButtonAccessKey(addExisting, AccessKeys.ManageUsersAddToRoleButtonAccessKey);

            
        }

        private void LoadSettings()
        {
            
            userId = WebUtils.ParseInt32FromQueryString("u", true, userId);
            isAdmin = WebUser.IsAdmin;

            if (userId > -1)
            {
                siteUser = new SiteUser(SiteInfo, userId);
                if (siteUser.UserId == -1) { siteUser = null; }
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            addExisting.Click += new EventHandler(AddRole_Click);
            userRoles.ItemDataBound += new DataListItemEventHandler(userRoles_ItemDataBound);
            userRoles.ItemCommand += new DataListCommandEventHandler(UserRoles_ItemCommand);
        }

    }
}