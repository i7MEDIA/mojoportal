// Author:					
// Created:				    2011-06-09
// Last Modified:			2016-02-03
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.ForumUI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class ForumDisplaySettings : WebControl
    {
        private bool useOldTableAttributes = true;
        public bool UseOldTableAttributes
        {
            get { return useOldTableAttributes; }
            set { useOldTableAttributes = value; }
        }

        private string forumListCssClass = "forumlist";
        public string ForumListCssClass
        {
            get { return forumListCssClass; }
            set { forumListCssClass = value; }
        }

        private string postListCssClass = "postlist";
        public string PostListCssClass
        {
            get { return postListCssClass; }
            set { postListCssClass = value; }
        }

        private string threadListCssClass = "threadlist";
        public string ThreadListCssClass
        {
            get { return threadListCssClass; }
            set { threadListCssClass = value; }
        }

        private string userThreadListCssClass = "threadlist userthreadlist";
        public string UserThreadListCssClass
        {
            get { return userThreadListCssClass; }
            set { userThreadListCssClass = value; }
        }

        private bool useAltForumList = false;

        public bool UseAltForumList
        {
            get { return useAltForumList; }
            set { useAltForumList = value; }
        }

        private bool useAltThreadList = false;

        public bool UseAltThreadList
        {
            get { return useAltThreadList; }
            set { useAltThreadList = value; }
        }

        private bool useAltPostList = false;

        public bool UseAltPostList
        {
            get { return useAltPostList; }
            set { useAltPostList = value; }
        }

        private bool useAltUserThreadList = false;

        public bool UseAltUserThreadList
        {
            get { return useAltUserThreadList; }
            set { useAltUserThreadList = value; }
        }

        private bool hideHeadingOnThreadView = false;

        public bool HideHeadingOnThreadView
        {
            get { return hideHeadingOnThreadView; }
            set { hideHeadingOnThreadView = value; }
        }

        private bool hideCurrentCrumbOnThreadcrumbs = false;

        public bool HideCurrentCrumbOnThreadcrumbs
        {
            get { return hideCurrentCrumbOnThreadcrumbs; }
            set { hideCurrentCrumbOnThreadcrumbs = value; }
        }

        private bool hideNotificationLinkOnPostList = false;

        public bool HideNotificationLinkOnPostList
        {
            get { return hideNotificationLinkOnPostList; }
            set { hideNotificationLinkOnPostList = value; }
        }

        private bool hideForumDescriptionOnPostList = false;

        public bool HideForumDescriptionOnPostList
        {
            get { return hideForumDescriptionOnPostList; }
            set { hideForumDescriptionOnPostList = value; }
        }

        private bool useBottomSearchOnForumList = false;

        public bool UseBottomSearchOnForumList
        {
            get { return useBottomSearchOnForumList; }
            set { useBottomSearchOnForumList = value; }
        }

        private bool hideSearchOnForumList = false;

        public bool HideSearchOnForumList
        {
            get { return hideSearchOnForumList; }
            set { hideSearchOnForumList = value; }
        }

        private bool useBottomSearchOnForumView = false;

        public bool UseBottomSearchOnForumView
        {
            get { return useBottomSearchOnForumView; }
            set { useBottomSearchOnForumView = value; }
        }

        private bool hideSearchOnForumView = false;

        public bool HideSearchOnForumView
        {
            get { return hideSearchOnForumView; }
            set { hideSearchOnForumView = value; }
        }

        private bool forumViewHideForumDescription = false;

        public bool ForumViewHideForumDescription
        {
            get { return forumViewHideForumDescription; }
            set { forumViewHideForumDescription = value; }
        }


        private bool forumViewHideStartedBy = false;

        public bool ForumViewHideStartedBy
        {
            get { return forumViewHideStartedBy; }
            set { forumViewHideStartedBy = value; }
        }

        private bool forumViewHideTotalViews = false;

        public bool ForumViewHideTotalViews
        {
            get { return forumViewHideTotalViews; }
            set { forumViewHideTotalViews = value; }
        }

        private bool forumViewHideTotalReplies = false;

        public bool ForumViewHideTotalReplies
        {
            get { return forumViewHideTotalReplies; }
            set { forumViewHideTotalReplies = value; }
        }

        private bool forumViewHideLastPostDate = false;

        public bool ForumViewHideLastPostDate
        {
            get { return forumViewHideLastPostDate; }
            set { forumViewHideLastPostDate = value; }
        }

        private bool forumViewHideLastPostUser = false;

        public bool ForumViewHideLastPostUser
        {
            get { return forumViewHideLastPostUser; }
            set { forumViewHideLastPostUser = value; }
        }

        private bool hideAvatars = false;

        public bool HideAvatars
        {
            get { return hideAvatars; }
            set { hideAvatars = value; }
        }

        private bool hideFeedLinks = false;

        public bool HideFeedLinks
        {
            get { return hideFeedLinks; }
            set { hideFeedLinks = value; }
        }


        private bool hideUserTotalPosts = false;

        public bool HideUserTotalPosts
        {
            get { return hideUserTotalPosts; }
            set { hideUserTotalPosts = value; }
        }

        private string overrideThreadHeadingElement = string.Empty;

        public string OverrideThreadHeadingElement
        {
            get { return overrideThreadHeadingElement; }
            set { overrideThreadHeadingElement = value; }
        }


        private bool hideForumDescriptionOnPostEdit = false;

        public bool HideForumDescriptionOnPostEdit
        {
            get { return hideForumDescriptionOnPostEdit; }
            set { hideForumDescriptionOnPostEdit = value; }
        }

        //private int overrideTopicsPerPage = 0;

        //public int OverrideTopicsPerPage
        //{
        //    get { return overrideTopicsPerPage; }
        //    set { overrideTopicsPerPage = value; }
        //}

        //private int overridePostsPerPage = 0;

        //public int OverridePostsPerPage
        //{
        //    get { return overridePostsPerPage; }
        //    set { overridePostsPerPage = value; }
        //}





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