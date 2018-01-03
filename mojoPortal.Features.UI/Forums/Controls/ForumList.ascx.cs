// Author:					
// Created:				    2011-06-13
// Last Modified:			2012-06-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class ForumList : UserControl
    {
        #region Properties

        private ForumConfiguration config = null;
        protected Double TimeOffset = 0;
        private TimeZoneInfo timeZone = null;
        protected string notificationUrl = string.Empty;
        protected string notificationLink = string.Empty;
        private SiteUser siteUser;
        private int userId = -1;
        private ArrayList forumIDsToSubscribe = new ArrayList();
        private ArrayList forumIDsToUnsubscribe = new ArrayList();

        private int pageId = -1;

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private bool isEditable = false;

        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        public ForumConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        private string siteRoot = string.Empty;

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        private string nonSslSiteRoot = string.Empty;

        public string NonSslSiteRoot
        {
            get { return nonSslSiteRoot; }
            set { nonSslSiteRoot = value; }
        }

        private string imageSiteRoot = string.Empty;

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        private bool showSubscribeCheckboxes = false;

        public bool ShowSubscribeCheckboxes
        {
            get { return showSubscribeCheckboxes; }
            set { showSubscribeCheckboxes = value; }
        }

        private Hashtable ForumIdForControlId
        {
            get
            {
                if (ViewState["forumIDForControlID"] == null)
                {
                    ViewState["forumIDForControlID"] = new Hashtable();
                }
                return (Hashtable)ViewState["forumIDForControlID"];
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible) { return; }

            LoadSettings();
            PopulateLabels();
            PopulateControls();
            
        }

        private void PopulateControls()
        {
            if (showSubscribeCheckboxes && IsPostBack) { return; }

            using (IDataReader reader = Forum.GetForums(ModuleId, userId))
            {
                rptForums.DataSource = reader;
#if MONO
			rptForums.DataBind();
#else
                this.DataBind();
#endif
            }

            if (rptForums.Items.Count == 0)
            {
                pnlForumList.Visible = false;
            }

        }

        private void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e == null || e.Item == null) { return; }
            CheckBox checkBox = e.Item.FindControl("chkSubscribed") as CheckBox;
            if (checkBox == null)
                checkBox = e.Item.FindControl("chkSubscribedAlt") as CheckBox;
            if (checkBox == null)
                return;
            int forumId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ItemID"));
            ForumIdForControlId[checkBox.UniqueID] = forumId;
        }

        protected void Subscribed_CheckedChanged(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            CheckBox checkBox = sender as CheckBox;
            int forumId = Convert.ToInt32(ForumIdForControlId[checkBox.UniqueID]);
            if (checkBox.Checked)
            {
                forumIDsToSubscribe.Add(forumId);
            }
            else
            {
                forumIDsToUnsubscribe.Add(forumId);
            }
        }

        protected string FormatUrl(int itemId)
        {
            if (ForumConfiguration.CombineUrlParams)
            {
                return SiteRoot + "/Forums/ForumView.aspx?pageid="
                + PageId.ToInvariantString()
                + "&f=" + ForumParameterParser.FormatCombinedParam(itemId, 1);
            }
            
            return SiteRoot + "/Forums/ForumView.aspx?pageid=" 
                + PageId.ToInvariantString() 
                + "&mid=" + ModuleId.ToInvariantString() + "&ItemID=" + itemId.ToInvariantString();
        }

        protected string FormatSubscriberCount(int subscriberCount)
        {
            return string.Format(CultureInfo.InvariantCulture, ForumResources.SubscriberCountFormat, subscriberCount);

        }

        protected string FormatDate(object o)
        {
            if ((o == null) || (o == DBNull.Value)) { return string.Empty; }

            DateTime startDate = Convert.ToDateTime(o);

            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(startDate, timeZone).ToString();

            }

            return startDate.AddHours(TimeOffset).ToString();

        }

        private void LoadSettings()
        {
            TimeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            divEditSubscriptions.Visible = tdSubscribedHead.Visible = Page.Request.IsAuthenticated;
            lnkModuleRSS.Visible = config.EnableRSSAtModuleLevel;

            notificationUrl = SiteRoot + "/Forums/EditSubscriptions.aspx?mid="
                + moduleId.ToInvariantString()
                + "&pageid=" + pageId.ToInvariantString();

            notificationLink = "<a title='" + ForumResources.ForumModuleEditSubscriptionsLabel
               + "' href='" + SiteRoot + "/Forums/EditSubscriptions.aspx?mid="
               + ModuleId.ToInvariantString()
               + "&amp;pageid=" + PageId.ToInvariantString()
               + "'><img src='" + ImageSiteRoot + "/Data/SiteImages/email.png' /></a>";

            lnkModuleRSS.NavigateUrl = NonSslSiteRoot
                + "/Forums/RSS.aspx?pageid=" + PageId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

            editSubscriptionsLink.NavigateUrl = notificationUrl;

            siteUser = SiteUtils.GetCurrentSiteUser();

            if (siteUser != null) userId = siteUser.UserId;

            trSubscribeButtons.Visible = showSubscribeCheckboxes;

            if (showSubscribeCheckboxes)
            {
                rptForums.ItemDataBound += new RepeaterItemEventHandler(Repeater_ItemDataBound);
                editSubscriptionsLink.Visible = false;
            }
            else
            {
#if !NET35
            //http://www.4guysfromrolla.com/articles/071410-1.aspx
            //optimize viewstate for .NET 4
            this.ViewStateMode = ViewStateMode.Disabled;
            rptForums.ViewStateMode = ViewStateMode.Disabled;
#else
                rptForums.EnableViewState = false;
#endif


            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (int forumId in forumIDsToSubscribe)
            {
                Forum forum = new Forum(forumId);
                forum.Subscribe(siteUser.UserId);
            }
            foreach (int forumId in forumIDsToUnsubscribe)
            {
                Forum forum = new Forum(forumId);
                forum.Unsubscribe(siteUser.UserId);
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

        private void PopulateLabels()
        {
            lnkModuleRSS.ToolTip = ForumResources.ModuleFeedLinkTooltip;
            editSubscriptionsLink.Text = ForumResources.ForumModuleEditSubscriptionsLabel;

            btnSave.Text = ForumResources.ForumEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnSave, ForumResources.ForumEditUpdateButtonAccessKey);

            btnCancel.Text = ForumResources.ForumEditCancelButton;
            SiteUtils.SetButtonAccessKey(btnCancel, ForumResources.ForumEditCancelButtonAccessKey);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }
        
    }
}