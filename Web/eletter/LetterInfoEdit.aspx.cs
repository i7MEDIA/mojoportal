/// Author:					
/// Created:				2007-09-23
/// Last Modified:			2014-03-11
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
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

    public partial class LetterInfoEditPage : NonCmsBasePage
    {
        private Guid letterInfoGuid = Guid.Empty;
        private SiteUser currentUser;
        private bool isSiteEditor = false;
        private SubscriberRepository subscriptions = new SubscriberRepository();


        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();
            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (!IsPostBack) PopulateLetterInfoControls();
        }

        private void PopulateLetterInfoControls()
        {
            //if (letterInfoGuid == Guid.Empty) return;

            LetterInfo letterInfo = new LetterInfo(letterInfoGuid);

            if (letterInfo.Title.Length > 0)
            {
                lnkThisPage.Text = letterInfo.Title;
            }
            

            txtTitle.Text = letterInfo.Title;
            edDescription.Text = letterInfo.Description;
            //txtAvailableToRoles.Text = letterInfo.AvailableToRoles.ToString();
            chkEnabled.Checked = letterInfo.Enabled;
            chkAllowUserFeedback.Checked = letterInfo.AllowUserFeedback;
            chkAllowAnonFeedback.Checked = letterInfo.AllowAnonFeedback;
            txtFromAddress.Text = letterInfo.FromAddress;
            txtFromName.Text = letterInfo.FromName;
            txtReplyToAddress.Text = letterInfo.ReplyToAddress;
            //txtSendMode.Text = letterInfo.SendMode.ToString();
            chkEnableViewAsWebPage.Checked = letterInfo.EnableViewAsWebPage;
            chkEnableSendLog.Checked = letterInfo.EnableSendLog;

            chkAllowArchiveView.Checked = letterInfo.AllowArchiveView;
            chkProfileOptIn.Checked = letterInfo.ProfileOptIn;
            txtSortRank.Text = letterInfo.SortRank.ToInvariantString();

            txtDisplayNameDefault.Text = letterInfo.DisplayNameDefault;
            txtFirstNameDefault.Text = letterInfo.FirstNameDefault;
            txtLastNameDefault.Text = letterInfo.LastNameDefault;

            if (letterInfoGuid != Guid.Empty)
            {
                lblLastModified.Text = letterInfo.LastModUtc.ToString();
            }


            ListItem allItem = new ListItem();
            // localize display
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";

            if (
                (letterInfo.AvailableToRoles.LastIndexOf("All Users") > -1)
                ||(letterInfoGuid == Guid.Empty)
                )
            {
                allItem.Selected = true;
            }


            chkListAvailableToRoles.Items.Clear();
            chkListAvailableToRoles.Items.Add(allItem);

            chkListEditRoles.Items.Clear();
            chkListApproveRoles.Items.Clear();
            chkListSendMailRoles.Items.Clear();

            using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (reader.Read())
                {

                    ListItem listItem = new ListItem();
                    listItem.Text = reader["DisplayName"].ToString();
                    listItem.Value = reader["RoleName"].ToString();

                    if ((letterInfo.AvailableToRoles.LastIndexOf(listItem.Value + ";")) > -1)
                    {
                        listItem.Selected = true;
                    }

                    chkListAvailableToRoles.Items.Add(listItem);

                    // edit roles

                    ListItem editItem = new ListItem();
                    editItem.Text = reader["DisplayName"].ToString();
                    editItem.Value = reader["RoleName"].ToString();

                    if ((letterInfo.RolesThatCanEdit.LastIndexOf(editItem.Value + ";")) > -1)
                    {
                        editItem.Selected = true;
                    }

                    chkListEditRoles.Items.Add(editItem);

                    // approve roles
                    ListItem approveItem = new ListItem();
                    approveItem.Text = reader["DisplayName"].ToString();
                    approveItem.Value = reader["RoleName"].ToString();

                    if ((letterInfo.RolesThatCanApprove.LastIndexOf(approveItem.Value + ";")) > -1)
                    {
                        approveItem.Selected = true;
                    }

                    chkListApproveRoles.Items.Add(approveItem);

                    // send roles
                    ListItem sendItem = new ListItem();
                    sendItem.Text = reader["DisplayName"].ToString();
                    sendItem.Value = reader["RoleName"].ToString();

                    if ((letterInfo.RolesThatCanSend.LastIndexOf(sendItem.Value + ";")) > -1)
                    {
                        sendItem.Selected = true;
                    }

                    chkListSendMailRoles.Items.Add(sendItem);


                }
            }
            
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("newsletteredit");
            if (Page.IsValid)
            {
                Save();
                WebUtils.SetupRedirect(this, 
                    SiteRoot 
                    + "/eletter/LetterInfoEdit.aspx?l=" 
                    + letterInfoGuid.ToString());

            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            Letter.DeleteByLetterInfo(letterInfoGuid);
            //LetterSubscriber.DeleteByLetter(letterInfoGuid);
            subscriptions.DeleteByLetter(letterInfoGuid);
            
            LetterInfo.Delete(letterInfoGuid);

            WebUtils.SetupRedirect(this,
                    SiteRoot
                    + "/eletter/Admin.aspx");

        }


        private void Save()
        {
            LetterInfo letterInfo = new LetterInfo(letterInfoGuid);

            letterInfo.SiteGuid = siteSettings.SiteGuid;
            letterInfo.Title = txtTitle.Text;
            letterInfo.Description = edDescription.Text;
            
            letterInfo.Enabled = chkEnabled.Checked;
            letterInfo.AllowUserFeedback = chkAllowUserFeedback.Checked;
            letterInfo.AllowAnonFeedback = chkAllowAnonFeedback.Checked;
            letterInfo.FromAddress = txtFromAddress.Text;
            letterInfo.FromName = txtFromName.Text;
            letterInfo.ReplyToAddress = txtReplyToAddress.Text;
            
            letterInfo.EnableViewAsWebPage = chkEnableViewAsWebPage.Checked;
            letterInfo.EnableSendLog = chkEnableSendLog.Checked;

            letterInfo.AllowArchiveView = chkAllowArchiveView.Checked;
            letterInfo.ProfileOptIn = chkProfileOptIn.Checked;
            int sort = letterInfo.SortRank;
            int.TryParse(txtSortRank.Text, out sort);
            letterInfo.SortRank = sort;

            letterInfo.DisplayNameDefault = txtDisplayNameDefault.Text;
            letterInfo.FirstNameDefault = txtFirstNameDefault.Text;
            letterInfo.LastNameDefault = txtLastNameDefault.Text;
            
            if (letterInfoGuid == Guid.Empty)
            {
                letterInfo.CreatedBy = currentUser.UserGuid;
                letterInfo.CreatedUtc = DateTime.UtcNow;
            }
            letterInfo.LastModUtc = DateTime.UtcNow;
            letterInfo.LastModBy = currentUser.UserGuid;

            string availableRoles = string.Empty;
            string editRoles = string.Empty;
            string approveRoles = string.Empty;
            string sendRoles = string.Empty;

            foreach (ListItem item in chkListAvailableToRoles.Items)
            {
                if (item.Selected)
                {
                    availableRoles = availableRoles + item.Value + ";";
                }
            }

            letterInfo.AvailableToRoles = availableRoles;

            foreach (ListItem item in chkListEditRoles.Items)
            {
                if (item.Selected)
                {
                    editRoles = editRoles + item.Value + ";";
                }
            }

            letterInfo.RolesThatCanEdit = editRoles;

            foreach (ListItem item in chkListApproveRoles.Items)
            {
                if (item.Selected)
                {
                    approveRoles = approveRoles + item.Value + ";";
                }
            }

            letterInfo.RolesThatCanApprove = approveRoles;

            foreach (ListItem item in chkListSendMailRoles.Items)
            {
                if (item.Selected)
                {
                    sendRoles = sendRoles + item.Value + ";";
                }
            }

            letterInfo.RolesThatCanSend = sendRoles;

            // TODO:
            //letterInfo.SendMode = txtSendMode.Text;

            letterInfo.Save();

            letterInfoGuid = letterInfo.LetterInfoGuid;

            

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.LetterInfoEditHeading);

            heading.Text = Resource.LetterInfoEditHeading;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkLetterAdmin.Text = Resource.LetterInfoAdminHeading;
            lnkLetterAdmin.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";

            lnkThisPage.Text = Resource.LetterInfoNewLink;
            lnkThisPage.NavigateUrl = Request.RawUrl;

            litSettingsTab.Text = Resource.LetterInfoSettingsTab;
            litDescriptionTab.Text = Resource.LetterInfoDescriptionTab;
            litSecurityTab.Text = Resource.LetterInfoSecurityTab;

            litSubscribeRolesTab.Text = Resource.LetterInfoRolesThatCanSubscribeTab;
            litEditRolesTab.Text = Resource.LetterInfoRolesThatCanEditTab;
            litApproveRolesTab.Text = Resource.LetterInfoRolesThatCanApproveTab;
            litSendRolesTab.Text = Resource.LetterInfoRolesThatCanSendTab;


            lnkEditRoles.HRef = "#" + tabEditRoles.ClientID;
            lnkApproveRoles.HRef = "#" + tabApproveRoles.ClientID;
            lnkSendRoles.HRef = "#" + tabSendRoles.ClientID;

            reqTitle.ErrorMessage = Resource.TitleRequiredWarning;

            btnSave.Text = Resource.LetterInfoSaveButton;
            btnDelete.Text = Resource.DeleteButton;
            UIHelper.AddConfirmationDialog(btnDelete, Resource.LetterInfoDeleteWarning);

            
            edDescription.WebEditor.ToolBar = ToolBar.Full;
            edDescription.WebEditor.Width = Unit.Percentage(100);
            edDescription.WebEditor.Height = Unit.Parse("320px");
            
            

        }

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin || WebUser.IsNewsletterAdmin || SiteUtils.UserIsSiteEditor();
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;
            
            AddClassToBody("administration");
            AddClassToBody("letterinfoedit");
        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            SuppressMenuSelection();
            SuppressPageMenu();

            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, this);

            
            
        }

        

        

        #endregion
    }
}
