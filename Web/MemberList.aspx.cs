/// Author:				        
/// Created:			        2004-10-03
/// Last Modified:		        2018-11-14
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
using mojoPortal.Web.Components;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using Resources;
using log4net;

namespace mojoPortal.Web.UI.Pages
{
    public partial class MemberList : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(MemberList));

		private int totalPages = 1;
		private int pageNumber = 1;
        private int pageSize = 20;
        protected bool IsAdmin = false;
        protected bool canManageUsers = false;
        private string filterLetter = string.Empty;
        private string searchText = string.Empty;
        private string ipSearchText = string.Empty;
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

		private Models.MemberListModel model;
		private List<SiteUser> siteUserPage;

		//private bool sortByDateDesc = false;

		#region OnInit
		override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            //this.btnSearchUser.Click += new EventHandler(btnSearchUser_Click);
            //btnSearchIP.Click += new EventHandler(btnSearchIP_Click);

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
            else if (searchText.Length > 0 || ipSearchText.Length > 0)
            {
                BindForSearch();
               
            }
            else
            {
                BindAlphaList();
            }


			if (pageNumber > totalPages)
			{
				pageNumber = 1;

			}

			PopulateModel();

			try
			{
				theLit.Text = RazorBridge.RenderPartialToString("MemberList", model, "Shared");
			}
			catch (System.Web.HttpException ex)
			{
				log.Error($"layout (MemberList) was not found in skin {SiteUtils.GetSkinBaseUrl(true, Page)}. perhaps it is in a different skin. Error was: {ex}");
			}


		}

        private void BindAlphaList()
        {
            siteUserPage = SiteUser.GetPage(
                siteSettings.SiteId,
                pageNumber,
                pageSize,
                filterLetter,
                sortMode,
                out totalPages);

            if (pageNumber > totalPages)
            {
                pageNumber = 1;
                siteUserPage = SiteUser.GetPage(
					siteSettings.SiteId,
					pageNumber,
					pageSize,
					filterLetter,
					sortMode,
					out totalPages);
            }

            //if (userNameBeginsWith.Length > 1)
            //{
            //    txtSearchUser.Text = Server.HtmlEncode(userNameBeginsWith);
            //}

            //AddAlphaPagerLinks();

            //string pageUrl = SiteRoot
            //    + "/MemberList.aspx?"
            //    + "pagenumber={0}"
            //    + "&amp;letter=" + Server.UrlEncode(Server.HtmlEncode(userNameBeginsWith))
            //    + "&amp;sd=" + sortMode.ToInvariantString();

            //pgrMembers.PageURLFormat = pageUrl;
            //pgrMembers.ShowFirstLast = true;
            //pgrMembers.CurrentIndex = pageNumber;
            //pgrMembers.PageSize = pageSize;
            //pgrMembers.PageCount = totalPages;
            //pgrMembers.Visible = (totalPages > 1);


            //rptUsers.DataSource = siteUserPage;
            //rptUsers.DataBind();




        }

        private void BindForSearch()
        {
			if (canManageUsers)
            {
				if (ipSearchText.Length > 0)
				{
					siteUserPage = SiteUser.GetByIPAddress(siteSettings.SiteGuid, ipSearchText);
				}
				else
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
            

            //if (searchText.Length > 0)
            //{
            //    txtSearchUser.Text = Server.HtmlEncode(searchText);
            //}

            //AddAlphaPagerLinks();

           
            //string pageUrl = SiteRoot
            //    + "/MemberList.aspx?"
            //    + "search=" + Server.UrlEncode(Server.HtmlEncode(searchText))
            //    + "&amp;pagenumber={0}" 
            //    +"&amp;sd=" + sortMode.ToInvariantString(); ;

            //pgrMembers.PageURLFormat = pageUrl;
            //pgrMembers.ShowFirstLast = true;
            //pgrMembers.CurrentIndex = pageNumber;
            //pgrMembers.PageSize = pageSize;
            //pgrMembers.PageCount = totalPages;
            //pgrMembers.Visible = (totalPages > 1);


            //rptUsers.DataSource = siteUserPage;
            //rptUsers.DataBind();



		}

		private void PopulateModel()
		{
			model = new Models.MemberListModel
			{
				Users = siteUserPage,
				DisplaySettings = displaySettings,
				PagerInfo = new Models.PagerInfo
				{
					CurrentIndex = pageNumber,
					PageSize = pageSize,
					PageCount = totalPages,
					LinkFormat = SiteRoot + "/MemberList.aspx?pagenumber={0}&letter=" + filterLetter + "&sd=" + sortMode.ToString()
				}
			};
		}

        //private void AddAlphaPagerLinks()
        //{
        //    Literal topPageLinks = new Literal();
        //    string pageUrl = SiteRoot + "/MemberList.aspx?sd=" + sortMode.ToInvariantString() + "&amp;pagenumber=";
            

        //    if (displaySettings.AlphaPagerContainerCssClass.Length > 0)
        //    {
        //        spnTopPager.Attributes.Add("class", displaySettings.AlphaPagerContainerCssClass);
        //    }

        //    string alphaChars;

        //    if (WebConfigSettings.GetAlphaPagerCharsFromResourceFile)
        //    {
        //        alphaChars = Resource.AlphaPagerChars;
        //    }
        //    else
        //    {
        //        alphaChars = WebConfigSettings.AlphaPagerChars;
        //    }


        //    topPageLinks.Text = UIHelper.GetAlphaPagerLinks(
        //        pageUrl,
        //        pageNumber,
        //        alphaChars,
        //        userNameBeginsWith,
        //        displaySettings.AlphaPagerCurrentPageCssClass,
        //        displaySettings.AlphaPagerOtherPageCssClass,
        //        displaySettings.UseListForAlphaPager,
        //        BuildAllUsersLink());

        //    this.spnTopPager.Controls.Add(topPageLinks);
        //}

        //private string BuildAllUsersLink()
        //{
        //    string cssAttribute = string.Empty;
        //    if (displaySettings.AllUsersLinkCssClass.Length > 0)
        //    {
        //        cssAttribute = " class='" + displaySettings.AllUsersLinkCssClass + "'";
        //    }
        //    string firstLink = "<a href='" + SiteRoot + "/MemberList.aspx'" + cssAttribute + ">"
        //        + Resource.MemberListAllUsersLink + "</a> ";

        //    return firstLink;

        //}

        //void btnSearchUser_Click(object sender, EventArgs e)
        //{
        //    string pageUrl = SiteRoot + "/MemberList.aspx?search=" + Server.UrlEncode(Server.HtmlEncode(txtSearchUser.Text)) + "&pagenumber=";

        //    WebUtils.SetupRedirect(this, pageUrl);

        //}

        //void btnSearchIP_Click(object sender, EventArgs e)
        //{
        //    pgrMembers.Visible = false;
        //    List<SiteUser> users = SiteUser.GetByIPAddress(siteSettings.SiteGuid, txtIPAddress.Text);
        //    rptUsers.DataSource = users;
        //    rptUsers.DataBind();


        //}

        void BindLockedUsers()
        {
            if (!canManageUsers) { return; }

            siteUserPage = SiteUser.GetPageLockedUsers(
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

            //string pageUrl = SiteRoot
            //    + "/MemberList.aspx?"
            //    + "pagenumber={0}"
            //    + "&amp;locked=true";

            //pgrMembers.PageURLFormat = pageUrl;
            //pgrMembers.ShowFirstLast = true;
            //pgrMembers.CurrentIndex = pageNumber;
            //pgrMembers.PageSize = pageSize;
            //pgrMembers.PageCount = totalPages;
            //pgrMembers.Visible = (totalPages > 1);


            //rptUsers.DataSource = siteUserPage;

            //rptUsers.DataBind();
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

            //string pageUrl = SiteRoot
            //    + "/MemberList.aspx?"
            //    + "pagenumber={0}"
            //    + "&amp;needapproval=true";

            //pgrMembers.PageURLFormat = pageUrl;
            //pgrMembers.ShowFirstLast = true;
            //pgrMembers.CurrentIndex = pageNumber;
            //pgrMembers.PageSize = pageSize;
            //pgrMembers.PageCount = totalPages;
            //pgrMembers.Visible = (totalPages > 1);


            //rptUsers.DataSource = siteUserPage;

            //rptUsers.DataBind();
		}

        //protected string FormatName(string displayName, string lastName, string firstName)
        //{
        //    if ((displaySettings.ShowFirstAndLastName)&&(firstName.Length > 0) && (lastName.Length > 0))
        //    {
        //        return Server.HtmlEncode(string.Format(CultureInfo.InvariantCulture, Resource.MemberListNameFormat, lastName, firstName));
        //    }

        //    return Server.HtmlEncode(displayName);
        //}

        private void PopulateLabels()
        {
           
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.MemberListLink);

            //heading.Text = Resource.MemberListTitleLabel;

            MetaDescription = string.Format(CultureInfo.InvariantCulture, 
                Resource.MetaDescriptionMemberListFormat, siteSettings.SiteName);

   //         btnSearchIP.Text = Resource.MemberListSearchByIPLabel;

            
   //         btnSearchUser.Text = Resource.MemberListSearchButton;
			//litSearchHeader.Text = string.Format(displaySettings.SearchPanelHeadingMarkup, Resource.MemberListSearchHeading);
			//litIPSearchHeader.Text = string.Format(displaySettings.IPSearchPanelHeadingMarkup, Resource.MemberListSearchByIPHeading);
			//litOtherActionsHeader.Text = string.Format(displaySettings.OtherActionsHeadingMarkup, Resource.MemberListOtherActionsHeading);

			//lnkAdminMenu.Text = Resource.AdminMenuLink;
   //         lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
   //         lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

   //         lnkMemberList.Text = Resource.MemberListLink;
   //         lnkMemberList.ToolTip = Resource.MemberListLink;
   //         lnkMemberList.NavigateUrl = SiteRoot + WebConfigSettings.MemberListUrl;

   //         if (displaySettings.ShowFirstAndLastName)
   //         {
   //             lnkNameSort.Text = Resource.Name;
   //         }
   //         else
   //         {
   //             lnkNameSort.Text = Resource.MemberListUserNameLabel;
   //         }

   //         lnkNameSort.NavigateUrl = SiteRoot + "/MemberList.aspx";

   //         lnkDateSort.Text = Resource.MemberListDateCreatedLabel;
   //         lnkDateSort.NavigateUrl = SiteRoot + "/MemberList.aspx?sd=1";
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
            //ShowForumPostColumn = WebConfigSettings.ShowForumPostsInMemberList && displaySettings.ShowForumPosts && !WebConfigSettings.UseRelatedSiteMode;

            allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);

			//if (IsAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageUsers))
			//{
			//	canManageUsers = true;
			//	fgpOtherActions.Visible = true;
			//}

			//if (canManageUsers || WebUser.IsInRoles(siteSettings.RolesThatCanCreateUsers))
			//{
			//	fgpOtherActions.Controls.Add(new Literal
			//	{
			//		Text = string.Format(displaySettings.NewUserLinkFormat, SiteRoot + "/Admin/ManageUsers.aspx?userId=-1", Resource.MemberListAddUserTooltip, Resource.MemberListAddUserLabel)
			//	});
			//}

			//if (canManageUsers)
   //         {
   //             fgpIPSearch.Visible = true;
			//	fgpOtherActions.Controls.Add(new Literal
			//	{
			//		Text = string.Format(displaySettings.LockedUsersLinkFormat, SiteRoot + "/MemberList.aspx?locked=true", Resource.ShowLockedOutUsersTooltip, Resource.ShowLockedOutUsers)
			//	});
			//}
            
			//if (canManageUsers && siteSettings.RequireApprovalBeforeLogin)
			//{
			//	fgpOtherActions.Controls.Add(new Literal
			//	{
			//		Text = string.Format(displaySettings.UnapprovedUsersLinkFormat, SiteRoot + "/MemberList.aspx?needapproval=true", Resource.ShowNotApprovedUsersTooltip, Resource.ShowNotApprovedUsers)
			//	});
			//}
            
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            sortMode = WebUtils.ParseInt32FromQueryString("sd", sortMode);

            if((sortMode == 0)&&(displaySettings.ShowFirstAndLastName))
            {
                sortMode = 2; // lastname, firstname
            }

            if (Request.Params["letter"] != null)
            {
                filterLetter = Request.Params["letter"].Trim();
            }

            if (Request.Params["search"] != null)
            {
                searchText = Request.Params["search"].Trim();
            }
			ipSearchText = WebUtils.ParseStringFromQueryString("ips", ipSearchText);
            showLocked = WebUtils.ParseBoolFromQueryString("locked", showLocked);
            showUnApproved = WebUtils.ParseBoolFromQueryString("needapproval", showUnApproved);

			//if (showLocked || showUnApproved || !string.IsNullOrWhiteSpace(searchText) || !string.IsNullOrWhiteSpace(userNameBeginsWith))
			//{
			//	fgpOtherActions.Controls.Add(new Literal
			//	{
			//		Text = string.Format(displaySettings.ShowAllUsersLinkFormat, SiteRoot + "/MemberList.aspx", Resource.MemberListShowAllTooltip, Resource.MemberListShowAllLabel)
			//	});
			//}

			pageSize = WebConfigSettings.MemberListPageSize;

            //mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
            //if (profileConfig != null)
            //{
            //    if (profileConfig.Contains("WebSiteUrl"))
            //    {
            //        mojoProfilePropertyDefinition webSiteUrlProperty = profileConfig.GetPropertyDefinition("WebSiteUrl");
            //        if(
            //            (webSiteUrlProperty.OnlyVisibleForRoles.Length == 0)
            //            || (WebUser.IsInRoles(webSiteUrlProperty.OnlyVisibleForRoles))
            //            )
            //        {
            //            ShowWebSiteColumn = true;
            //        }

            //    }
            //}

            // displaySettings can be configured from theme.skin
            //if (displaySettings.HideWebSiteColumn) { ShowWebSiteColumn = false; }

            //if(displaySettings.TableCssClass.Length > 0)
            //{
            //    tableClassMarkup = " class='" + displaySettings.TableCssClass + "'";
            //}

            //tableAttributes = displaySettings.TableAttributes;

            //if (!ShowWebSiteColumn) { thWebLink.Visible = false; }
            //if (!ShowJoinDate) { thJoinDate.Visible = false; }



            //if (IsAdmin) { pnlAdminCrumbs.Visible = true; }

            //if (!ShowForumPostColumn) { thForumPosts.Visible = false; }

            //this page has no content other than nav
            SiteUtils.AddNoIndexFollowMeta(Page);

            AddClassToBody("memberlist");

            //if (displaySettings.TableCssClass == "jqtable")
            //{
            //    ScriptConfig.IncludeJQTable = true;
            //}
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

		public bool HideWebSiteColumn { get; set; } = false;

		/// <summary>
		/// if true wraps a ul with li elements around the alpha pager links
		/// </summary>
		public bool UseListForAlphaPager { get; set; } = false;

		public bool ShowForumPosts { get; set; } = true;

		public bool ShowEmail { get; set; } = false;

		public bool ShowLoginName { get; set; } = false;

		public bool ShowFirstAndLastName { get; set; } = false;
		public bool ShowJoinDate { get; set; } = true;

		public bool ShowUserId { get; set; } = false;

		public string AlphaPagerContainerCssClass { get; set; } = string.Empty;

		public string AllUsersLinkCssClass { get; set; } = "ModulePager";

		public string AlphaPagerCurrentPageCssClass { get; set; } = "SelectedPage";

		public string AlphaPagerOtherPageCssClass { get; set; } = "ModulePager";

		public string TableCssClass { get; set; } = string.Empty;

		public string TableAttributes { get; set; } = "cellspacing='0' width='100%'";

		//public string NewUserLinkFormat { get; set; } = "<span class=\"fa fa-user-plus\"></span> <a href=\"{0}\" title=\"{1}\">{2}</a>";
		//public string LockedUsersLinkFormat { get; set; } = "<span class=\"fa fa-user-times\"></span> <a href=\"{0}\" title=\"{1}\">{2}</a>";
		//public string UnapprovedUsersLinkFormat { get; set; } = "<span class=\"fa fa-user-o\"></span> <a href=\"{0}\" title=\"{1}\">{2}</a>";
		//public string ShowAllUsersLinkFormat { get; set; } = "<span class=\"fa fa-users\"></span> <a href=\"{0}\" title=\"{1}\">{2}</a>";
		//public string SearchPanelHeadingMarkup { get; set; } = "<h3>{0}</h3>";
		//public string IPSearchPanelHeadingMarkup { get; set; } = "<h3>{0}</h3>";
		//public string OtherActionsHeadingMarkup { get; set; } = "<h3>{0}</h3>";

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

