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
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI 
{
    public partial class UserRoles : UserControl
    {
        private SiteSettings siteSettings = null;
        private bool useSeparatePagesForRoles = false;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;

        private string siteRoot = string.Empty;
        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private int userId = -1;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            if (useSeparatePagesForRoles)
            {
                divRoles.Visible = false;
                lnkRolesDialog.Visible = true;
                SetupDialogScript();
                
            }
            
            if (!IsPostBack)
            {
                BindRoles();
            }
           

        }


        private void BindRoles()
        {
            if (userId == -1) { return; }
            if (siteSettings == null) { return; }

            using (IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, userId))
            {
                userRoles.DataSource = reader;
                userRoles.DataBind();
            }

            if (!useSeparatePagesForRoles)
            {
                using (IDataReader reader = Role.GetRolesUserIsNotIn(siteSettings.SiteId, userId))
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

        }

        private void AddRole_Click(Object sender, EventArgs e)
        {
            if ((userId > -1)&&(siteSettings != null))
            {
                SiteUser user = new SiteUser(siteSettings, userId);
                int roleID = int.Parse(allRoles.SelectedItem.Value, CultureInfo.InvariantCulture);
                Role role = new Role(roleID);
                Role.AddUser(roleID, userId, role.RoleGuid, user.UserGuid);
                user.RolesChanged = true;
                user.Save();

                BindRoles();

                upRoles.Update();

            }

            //WebUtils.SetupRedirect(this, Request.RawUrl);
        }


        private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int roleID = Convert.ToInt32(userRoles.DataKeys[e.Item.ItemIndex]);
            SiteUser user = new SiteUser(siteSettings, userId);

            Role.RemoveUser(roleID, userId);
            userRoles.EditItemIndex = -1;
            if (user.UserId > -1)
            {
                user.RolesChanged = true;
                user.Save();
            }

            BindRoles();

            upRoles.Update();

            //WebUtils.SetupRedirect(this, Request.RawUrl);
            //return;

        }


        void userRoles_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ImageButton btnRemoveRole = e.Item.FindControl("btnRemoveRole") as ImageButton;
            UIHelper.AddConfirmationDialog(btnRemoveRole, Resource.ManageUsersRemoveRoleWarning);
        }

        void btnRefreshRoles_Click(object sender, ImageClickEventArgs e)
        {
            BindRoles();
            upRoles.Update();
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

            lnkRolesDialog.Text = Resource.ManageUserRoles;
            lnkRolesDialog.ToolTip = Resource.ManageUserRoles;
            lnkRolesDialog.NavigateUrl = siteRoot + "/Dialog/UserRolesDialog.aspx?u=" + userId.ToInvariantString();
            btnRefreshRoles.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnRefreshRoles.Attributes.Add("tabIndex", "-1");
        }

        private void SetupDialogScript()
        {
            mojoBasePage basePage = Page as mojoBasePage;
            basePage.ScriptConfig.IncludeColorBox = true;

            StringBuilder script = new StringBuilder();

            script.Append("\nfunction RefreshRoles(){");

            script.Append("var btn = document.getElementById('" + btnRefreshRoles.ClientID + "');  ");
            script.Append("btn.click(); ");

            script.Append("}\n");

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page),
                   "refreshroles", "\n<script type=\"text/javascript\" >"
                   + script.ToString() + "</script>", false);

            script = new StringBuilder();

            script.Append("\n$('#" + lnkRolesDialog.ClientID + "').colorbox(");
            script.Append("{width:'85%', height:'85%', iframe:true, onClosed:RefreshRoles}");

            script.Append(");");

            ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "userrolesdialog", "\n<script type=\"text/javascript\" >"
                   + script.ToString() + "</script>", false);


        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            useSeparatePagesForRoles = (Role.CountOfRoles(siteSettings.SiteId) >= WebConfigSettings.TooManyRolesForModuleSettings);

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            addExisting.Click += new EventHandler(AddRole_Click);
            btnRefreshRoles.Click += new ImageClickEventHandler(btnRefreshRoles_Click);
            userRoles.ItemDataBound += new DataListItemEventHandler(userRoles_ItemDataBound);
            userRoles.ItemCommand += new DataListCommandEventHandler(UserRoles_ItemCommand);
        }

        
        

    }
}