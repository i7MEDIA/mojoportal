/// Author:				        Joe Audette
/// Created:			        2004-10-03
/// Last Modified:		        2015-02-11 (i7MEDIA)
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{
    public partial class MemberList : NonCmsBasePage
	{

        private int totalPages = 1;
		private int pageNumber = 1;
        private int pageSize = 20;
        protected bool IsAdmin = false;
        protected bool canManageUsers = false;
        private string userNameBeginsWith = string.Empty;
        private string searchText = string.Empty;
        private bool showLocked = false;
        private bool showUnApproved = false;
        
        protected bool ShowWebSiteColumn = false;
        protected bool ShowForumPostColumn = true;
        protected bool ShowEmailInMemberList = false;
        protected bool ShowUserIDInMemberList = false;
        protected bool ShowLoginNameInMemberList = false;
        protected bool ShowJoinDate = true;

        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        
        private bool allowView = false;

        protected string tableClassMarkup = string.Empty;
        protected string tableAttributes = string.Empty;

        private int sortMode = 0; // 0 = displayName Asc, 1 = JoinDate Desc, 2 = Last, First

        //private bool sortByDateDesc = false;
		
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSearchUser.Click += new EventHandler(btnSearchUser_Click);
            btnIPLookup.Click += new EventHandler(btnIPLookup_Click);
            btnFindLocked.Click += new EventHandler(btnFindLocked_Click);
            btnFindNotApproved.Click += new EventHandler(btnFindNotApproved_Click);

            SuppressMenuSelection();
            if (WebConfigSettings.HidePageMenuOnMemberListPage) { SuppressPageMenu(); }
        }

        

        
        #endregion

		private void Page_Load(object sender, EventArgs e)
		{
            if (SiteUtils.SslIsAvailable()) 
            {
                if ((WebConfigSettings.UseSslForMemberList) || (siteSettings.UseSslOnAllPages))
                {
                    SiteUtils.ForceSsl();
                }
                else
                {
                    SiteUtils.ClearSsl();
                }

            }
            

            LoadSettings();
            PopulateLabels();
            
            if (!allowView)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                PopulateControls();
            }
			
		}

        private void PopulateControls()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);

            if ((Page.Header != null) && (CurrentPage.Url.Length > 0))
            {
                Literal link = new Literal();
                link.ID = "pageurl";
                link.Text = "\n<link rel='canonical' href='"
                    + SiteRoot + "/MemberList.aspx' />";
                Page.Header.Controls.Add(link);
            }

            if ((canManageUsers) && (showUnApproved))
            {
                BindNotApprovedUsers();
            }
            else if ((canManageUsers) && (showLocked))
            {
                BindLockedUsers();
            }
            else if (searchText.Length > 0)
            {
                BindForSearch();
               
            }
            else
            {
                BindAlphaList();
            }

            
            

           
        }

        private void BindAlphaList()
        {
            List<SiteUser> siteUserPage = SiteUser.GetPage(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                sortMode,
                out totalPages);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;
                siteUserPage = SiteUser.GetPage(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                userNameBeginsWith,
                sortMode,
                out totalPages);
            }

            if (userNameBeginsWith.Length > 1)
            {
                txtSearchUser.Text = Server.HtmlEncode(userNameBeginsWith);
            }

            AddAlphaPagerLinks();

            string pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "pagenumber={0}"
                + "&amp;letter=" + Server.UrlEncode(Server.HtmlEncode(userNameBeginsWith))
                + "&amp;sd=" + sortMode.ToInvariantString();

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            rptUsers.DataSource = siteUserPage;
            rptUsers.DataBind();


        }

        private void BindForSearch()
        {
            List<SiteUser> siteUserPage;

            if (canManageUsers)
            {
                // admins can also search against email address
                siteUserPage = SiteUser.GetUserAdminSearchPage(
                        siteSettings.SiteId,
                        pageNumber,
                        pageSize,
                        searchText,
                        sortMode,
                        out totalPages);
            }
            else
            {
                siteUserPage = SiteUser.GetUserSearchPage(
                        siteSettings.SiteId,
                        pageNumber,
                        pageSize,
                        searchText,
                        sortMode,
                        out totalPages);
            }
            
            if (pageNumber > totalPages)
            {
                pageNumber = 1;
                
            }

            if (searchText.Length > 0)
            {
                txtSearchUser.Text = Server.HtmlEncode(searchText);
            }

            AddAlphaPagerLinks();

           
            string pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "search=" + Server.UrlEncode(Server.HtmlEncode(searchText))
                + "&amp;pagenumber={0}" 
                +"&amp;sd=" + sortMode.ToInvariantString(); ;

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            rptUsers.DataSource = siteUserPage;
            rptUsers.DataBind();


        }

        private void AddAlphaPagerLinks()
        {
            Literal topPageLinks = new Literal();
            string pageUrl = SiteRoot + "/MemberList.aspx?sd=" + sortMode.ToInvariantString() + "&amp;pagenumber=";
            

            if (displaySettings.AlphaPagerContainerCssClass.Length > 0)
            {
                spnTopPager.Attributes.Add("class", displaySettings.AlphaPagerContainerCssClass);
            }

            string alphaChars;

            if (WebConfigSettings.GetAlphaPagerCharsFromResourceFile)
            {
                alphaChars = Resource.AlphaPagerChars;
            }
            else
            {
                alphaChars = WebConfigSettings.AlphaPagerChars;
            }


            topPageLinks.Text = UIHelper.GetAlphaPagerLinks(
                pageUrl,
                pageNumber,
                alphaChars,
                userNameBeginsWith,
                displaySettings.AlphaPagerCurrentPageCssClass,
                displaySettings.AlphaPagerOtherPageCssClass,
                displaySettings.UseListForAlphaPager,
                BuildAllUsersLink());

            this.spnTopPager.Controls.Add(topPageLinks);
        }

        private string BuildAllUsersLink()
        {
            string cssAttribute = string.Empty;
            if (displaySettings.AllUsersLinkCssClass.Length > 0)
            {
                cssAttribute = " class='" + displaySettings.AllUsersLinkCssClass + "'";
            }
            string firstLink = "<a href='" + SiteRoot + "/MemberList.aspx'" + cssAttribute + ">"
                + Resource.MemberListAllUsersLink + "</a> ";

            return firstLink;

        }

        void btnSearchUser_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot + "/MemberList.aspx?search=" + Server.UrlEncode(Server.HtmlEncode(txtSearchUser.Text)) + "&pagenumber=";

            WebUtils.SetupRedirect(this, pageUrl);

        }

        void btnIPLookup_Click(object sender, EventArgs e)
        {
            pgrMembers.Visible = false;
            List<SiteUser> users = SiteUser.GetByIPAddress(siteSettings.SiteGuid, txtIPAddress.Text);
            rptUsers.DataSource = users;
            rptUsers.DataBind();


        }

        void BindLockedUsers()
        {
            if (!canManageUsers) { return; }

            List<SiteUser> siteUserPage = SiteUser.GetPageLockedUsers(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                out totalPages);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;
                siteUserPage = SiteUser.GetPageLockedUsers(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                out totalPages);
            }

            string pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "pagenumber={0}"
                + "&amp;locked=true";

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            rptUsers.DataSource = siteUserPage;

            rptUsers.DataBind();

        }

        void btnFindLocked_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot + "/MemberList.aspx?locked=true";

            WebUtils.SetupRedirect(this, pageUrl);
            
        }

        void BindNotApprovedUsers()
        {
            if (!canManageUsers) { return; }

            List<SiteUser> siteUserPage = SiteUser.GetNotApprovedUsers(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                out totalPages);

            if ((pageNumber > 1)&&(pageNumber > totalPages))
            {
                pageNumber = 1;
                siteUserPage = SiteUser.GetNotApprovedUsers(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                out totalPages);
            }

            string pageUrl = SiteRoot
                + "/MemberList.aspx?"
                + "pagenumber={0}"
                + "&amp;needapproval=true";

            pgrMembers.PageURLFormat = pageUrl;
            pgrMembers.ShowFirstLast = true;
            pgrMembers.CurrentIndex = pageNumber;
            pgrMembers.PageSize = pageSize;
            pgrMembers.PageCount = totalPages;
            pgrMembers.Visible = (totalPages > 1);


            rptUsers.DataSource = siteUserPage;

            rptUsers.DataBind();

        }

        void btnFindNotApproved_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot + "/MemberList.aspx?needapproval=true";

            WebUtils.SetupRedirect(this, pageUrl);

        }

        protected string FormatName(string displayName, string lastName, string firstName)
        {
            if ((displaySettings.ShowFirstAndLastName)&&(firstName.Length > 0) && (lastName.Length > 0))
            {
                return Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture, Resource.MemberListNameFormat, lastName, firstName));
            }

            return Server.HtmlEncode(displayName);
        }

        private void PopulateLabels()
        {
           
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);

            heading.Text = Resource.MemberListTitleLabel;

            MetaDescription = string.Format(CultureInfo.InvariantCulture, 
                Resource.MetaDescriptionMemberListFormat, siteSettings.SiteName);

            btnIPLookup.Text = Resource.LookupUserByIPAddressButton;

            
            //lnkAllUsers.Text = Resource.MemberListAllUsersLink;
            btnSearchUser.Text = Resource.MemberListSearchButton;
            btnFindLocked.Text = Resource.ShowLockedOutUsers;
            btnFindNotApproved.Text = Resource.ShowNotApprovedUsers;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkMemberList.Text = Resource.MemberListLink;
            lnkMemberList.ToolTip = Resource.MemberListLink;
            lnkMemberList.NavigateUrl = SiteRoot + WebConfigSettings.MemberListUrl;

            if (displaySettings.ShowFirstAndLastName)
            {
                lnkNameSort.Text = Resource.Name;
            }
            else
            {
                lnkNameSort.Text = Resource.MemberListUserNameLabel;
            }

            lnkNameSort.NavigateUrl = SiteRoot + "/MemberList.aspx";

            lnkDateSort.Text = Resource.MemberListDateCreatedLabel;
            lnkDateSort.NavigateUrl = SiteRoot + "/MemberList.aspx?sd=1";
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            //lnkAllUsers.NavigateUrl = SiteRoot + "/MemberList.aspx";
            IsAdmin = WebUser.IsAdmin;

            ShowEmailInMemberList = WebConfigSettings.ShowEmailInMemberList || displaySettings.ShowEmail;
            ShowUserIDInMemberList = WebConfigSettings.ShowUserIDInMemberList || displaySettings.ShowUserId;
            ShowLoginNameInMemberList = WebConfigSettings.ShowLoginNameInMemberList || displaySettings.ShowLoginName;
            ShowJoinDate = displaySettings.ShowJoinDate;

            // this can't be used in related site mode because we can't assume forum posts were in this site.
            ShowForumPostColumn = WebConfigSettings.ShowForumPostsInMemberList && displaySettings.ShowForumPosts && !WebConfigSettings.UseRelatedSiteMode;

            allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);

            if ((IsAdmin) || (WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers)))
            {
                
                canManageUsers = true;
                spnIPLookup.Visible = true;
                btnFindLocked.Visible = true;
            }
            

            btnFindNotApproved.Visible = canManageUsers && siteSettings.RequireApprovalBeforeLogin;

            if (canManageUsers || WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers))
            {
                lnkNewUser.Visible = true;
                lnkNewUser.Text = Resource.MemberListAddUserLabel;
                lnkNewUser.NavigateUrl = SiteRoot + "/Admin/ManageUsers.aspx?userId=-1";
            }

            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            sortMode = WebUtils.ParseInt32FromQueryString("sd", sortMode);

            if((sortMode == 0)&&(displaySettings.ShowFirstAndLastName))
            {
                sortMode = 2; // lastname, firstname
            }

            if (Request.Params["letter"] != null)
            {
                userNameBeginsWith = Request.Params["letter"].Trim();
            }

            if (Request.Params["search"] != null)
            {
                searchText = Request.Params["search"].Trim();
            }

            showLocked = WebUtils.ParseBoolFromQueryString("locked", showLocked);
            showUnApproved = WebUtils.ParseBoolFromQueryString("needapproval", showUnApproved);
            
            pageSize = WebConfigSettings.MemberListPageSize;

            mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                if (profileConfig.Contains("WebSiteUrl"))
                {
                    mojoProfilePropertyDefinition webSiteUrlProperty = profileConfig.GetPropertyDefinition("WebSiteUrl");
                    if(
                        (webSiteUrlProperty.OnlyVisibleForRoles.Length == 0)
                        || (WebUser.IsInRoles(webSiteUrlProperty.OnlyVisibleForRoles))
                        )
                    {
                        ShowWebSiteColumn = true;
                    }

                }
            }

            // displaySettings can be configured from theme.skin
            if (displaySettings.HideWebSiteColumn) { ShowWebSiteColumn = false; }

            if(displaySettings.TableCssClass.Length > 0)
            {
                tableClassMarkup = " class='" + displaySettings.TableCssClass + "'";
            }

            tableAttributes = displaySettings.TableAttributes;

            if (!ShowWebSiteColumn) { thWebLink.Visible = false; }
            if (!ShowJoinDate) { thJoinDate.Visible = false; }



            if (IsAdmin) { pnlAdminCrumbs.Visible = true; }

            if (!ShowForumPostColumn) { thForumPosts.Visible = false; }

            //this page has no content other than nav
            SiteUtils.AddNoIndexFollowMeta(Page);

            AddClassToBody("memberlist");

            if (displaySettings.TableCssClass == "jqtable")
            {
                ScriptConfig.IncludeJQTable = true;
            }
        }

		
		
		
	}
}

namespace mojoPortal.Web.UI
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class MemberListDisplaySettings : WebControl
    {
        
        private bool hideWebSiteColumn = false;

        public bool HideWebSiteColumn
        {
            get { return hideWebSiteColumn; }
            set { hideWebSiteColumn = value; }
        }

        private bool useListForAlphaPager = false;

        /// <summary>
        /// if true wraps a ul with li elements around the alpha pager links
        /// </summary>
        public bool UseListForAlphaPager
        {
            get { return useListForAlphaPager; }
            set { useListForAlphaPager = value; }
        }

        private bool showForumPosts = true;

        public bool ShowForumPosts
        {
            get { return showForumPosts; }
            set { showForumPosts = value; }
        }

        private bool showEmail = false;

        public bool ShowEmail
        {
            get { return showEmail; }
            set { showEmail = value; }
        }

        private bool showLoginName = false;

        public bool ShowLoginName
        {
            get { return showLoginName; }
            set { showLoginName = value; }
        }

        private bool showFirstAndLastName = false;

        public bool ShowFirstAndLastName
        {
            get { return showFirstAndLastName; }
            set { showFirstAndLastName = value; }
        }

        private bool showJoinDate = true;
        public bool ShowJoinDate
        {
            get { return showJoinDate; }
            set { showJoinDate = value; }
        }

        private bool showUserId = false;

        public bool ShowUserId
        {
            get { return showUserId; }
            set { showUserId = value; }
        }

        private string alphaPagerContainerCssClass = string.Empty;

        public string AlphaPagerContainerCssClass
        {
            get { return alphaPagerContainerCssClass; }
            set { alphaPagerContainerCssClass = value; }
        }

        private string allUsersLinkCssClass = "ModulePager";

        public string AllUsersLinkCssClass
        {
            get { return allUsersLinkCssClass; }
            set { allUsersLinkCssClass = value; }
        }

        private string alphaPagerCurrentPageCssClass = "SelectedPage";

        public string AlphaPagerCurrentPageCssClass
        {
            get { return alphaPagerCurrentPageCssClass; }
            set { alphaPagerCurrentPageCssClass = value; }
        }

        private string alphaPagerOtherPageCssClass = "ModulePager";

        public string AlphaPagerOtherPageCssClass
        {
            get { return alphaPagerOtherPageCssClass; }
            set { alphaPagerOtherPageCssClass = value; }
        }

        private string tableCssClass = string.Empty;

        public string TableCssClass
        {
            get { return tableCssClass; }
            set { tableCssClass = value; }
        }

        private string tableAttributes = "cellspacing='0' width='100%'";

        public string TableAttributes
        {
            get { return tableAttributes; }
            set { tableAttributes = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }

}

