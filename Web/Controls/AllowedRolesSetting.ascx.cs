/// Author:					
/// Created:				2009-01-14
/// Last Modified:			2009-05-27
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
    public partial class AllowedRolesSetting : UserControl, ISettingControl
    {

        private string selectedRoles = string.Empty;
        private SiteSettings siteSettings = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            this.Load += new EventHandler(Page_Load);
            EnsureItems();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            //if (!Page.IsPostBack)
            //{
            //    BindList();
            //}


        }

        private void EnsureItems()
        {
            if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
            //why is this null here, its declared
            if (chkAllowedRoles == null)
            {
                chkAllowedRoles = new CheckBoxList();
                if (this.Controls.Count == 0) { this.Controls.Add(chkAllowedRoles); }
            }

            if (chkAllowedRoles.Items.Count > 0) { return; }

            
            ListItem allItem = new ListItem();
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";
            chkAllowedRoles.Items.Add(allItem);

            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();
                    chkAllowedRoles.Items.Add(listItem);

                }
            }

        }

        private void BindList()
        {
            if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
            //why is this null here, its declared
            if (chkAllowedRoles == null) 
            { 
                chkAllowedRoles = new CheckBoxList();
                if (this.Controls.Count == 0) { this.Controls.Add(chkAllowedRoles); }
            }

            chkAllowedRoles.Items.Clear();

            ListItem allItem = new ListItem();
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";
            chkAllowedRoles.Items.Add(allItem);

            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    ListItem listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();

                    if ((selectedRoles.LastIndexOf(listItem.Value + ";") > -1))
                    {
                        listItem.Selected = true;
                    }

                    chkAllowedRoles.Items.Add(listItem);

                }
            }

        }

        private void GetSelectedItems()
        {
            selectedRoles = string.Empty;
            foreach (ListItem item in chkAllowedRoles.Items)
            {
                if (item.Selected)
                {
                    selectedRoles = selectedRoles + item.Value + ";";
                }
            }

        }

        private void BindSelection()
        {
            foreach (ListItem item in chkAllowedRoles.Items)
            {
                if ((selectedRoles.LastIndexOf(item.Value + ";") > -1))
                {
                    item.Selected = true;
                }
            }

        }


        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            GetSelectedItems();
            return selectedRoles;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            selectedRoles = val;
            //BindList();
            BindSelection();
        }

        #endregion



    }
}