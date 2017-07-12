// Author:					
// Created:				    2011-05-01
// Last Modified:			2017-06-07
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
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using mojoPortal.Net;

namespace mojoPortal.Web.ForumUI
{
    public class ForumConfiguration
    {
        public ForumConfiguration()
        { }

        public ForumConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            enableRSSAtModuleLevel = WebUtils.ParseBoolFromHashtable(settings, "ForumEnableRSSAtModuleLevel", enableRSSAtModuleLevel);
            enableRSSAtForumLevel = WebUtils.ParseBoolFromHashtable(settings, "ForumEnableRSSAtForumLevel", enableRSSAtForumLevel);
            enableRSSAtThreadLevel = WebUtils.ParseBoolFromHashtable(settings, "ForumEnableRSSAtThreadLevel", enableRSSAtThreadLevel);
            showSubscriberCount = WebUtils.ParseBoolFromHashtable(settings, "ShowSubscriberCount", showSubscriberCount);
            showForumSearchBox = WebUtils.ParseBoolFromHashtable(settings, "ShowForumSearchBox", showForumSearchBox);
            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
            }
            linkGravatarToUserProfile = WebUtils.ParseBoolFromHashtable(settings, "LinkGravatarToUserProfile", linkGravatarToUserProfile);

            useSpamBlockingForAnonymous = WebUtils.ParseBoolFromHashtable(settings, "ForumEnableAntiSpamSetting", useSpamBlockingForAnonymous);
            includePostBodyInNotification = WebUtils.ParseBoolFromHashtable(settings, "IncludePostBodyInNotificationEmail", includePostBodyInNotification);

            includeCurrentUserInNotifications = WebUtils.ParseBoolFromHashtable(settings, "IncludeCurrentUserInNotifications", includeCurrentUserInNotifications);

            suppressNotificationOfPostEdits = WebUtils.ParseBoolFromHashtable(settings, "SuppressNotificationOfPostEdits", suppressNotificationOfPostEdits);

            allowEditingPostsLessThanMinutesOld = WebUtils.ParseInt32FromHashtable(settings, "AllowEditingPostsLessThanMinutesOld", allowEditingPostsLessThanMinutesOld);

            closeThreadsOlderThanDays = WebUtils.ParseInt32FromHashtable(settings, "CloseThreadsOlderThanDays", closeThreadsOlderThanDays);

            showLeftContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageLeftContentSetting", showLeftContent);

            showRightContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageRightContentSetting", showRightContent);

            if (settings.Contains("OverrideNotificationFromAddress"))
            {
                overrideNotificationFromAddress = settings["OverrideNotificationFromAddress"].ToString();
            }

            if (settings.Contains("OverrideNotificationFromAlias"))
            {
                overrideNotificationFromAlias = settings["OverrideNotificationFromAlias"].ToString();
            }

        }

        private string overrideNotificationFromAddress = string.Empty;

        public string OverrideNotificationFromAddress
        {
            get { return overrideNotificationFromAddress; }
        }

        private string overrideNotificationFromAlias = string.Empty;

        public string OverrideNotificationFromAlias
        {
            get { return overrideNotificationFromAlias; }
        }

        private bool showLeftContent = false;

        public bool ShowLeftContent
        {
            get { return showLeftContent; }
        }


        private bool showRightContent = false;

        public bool ShowRightContent
        {
            get { return showRightContent; }
        }

        private int closeThreadsOlderThanDays = -1;

        public int CloseThreadsOlderThanDays
        {
            get { return closeThreadsOlderThanDays; }
        }

        private int allowEditingPostsLessThanMinutesOld = 60;

        public int AllowEditingPostsLessThanMinutesOld
        {
            get { return allowEditingPostsLessThanMinutesOld; }
        }

        private bool suppressNotificationOfPostEdits = false;

        public bool SuppressNotificationOfPostEdits
        {
            get { return suppressNotificationOfPostEdits; }
        }

        private bool includePostBodyInNotification = false;

        public bool IncludePostBodyInNotification
        {
            get { return includePostBodyInNotification; }
        }

        private bool useSpamBlockingForAnonymous = true;

        public bool UseSpamBlockingForAnonymous
        {
            get { return useSpamBlockingForAnonymous; }
        }

        private bool linkGravatarToUserProfile = true;

        public bool LinkGravatarToUserProfile
        {
            get { return linkGravatarToUserProfile; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

        private bool showForumSearchBox = true;

        public bool ShowForumSearchBox
        {
            get { return showForumSearchBox; }
        }

        private bool includeCurrentUserInNotifications = true;

        public bool IncludeCurrentUserInNotifications
        {
            get { return includeCurrentUserInNotifications; }
        }

        private bool showSubscriberCount = false;

        public bool ShowSubscriberCount
        {
            get { return showSubscriberCount; }
        }

        private bool enableRSSAtModuleLevel = false;

        public bool EnableRSSAtModuleLevel
        {
            get { return enableRSSAtModuleLevel; }
        }


        private bool enableRSSAtForumLevel = false;

        public bool EnableRSSAtForumLevel
        {
            get { return enableRSSAtForumLevel; }
        }

        private bool enableRSSAtThreadLevel = false;

        public bool EnableRSSAtThreadLevel
        {
            get { return enableRSSAtThreadLevel; }
        }





        public static bool UsePageNameInThreadTitle
        {
            get { return ConfigHelper.GetBoolProperty("Forum:UsePageNameInThreadTitle", false); }
        }
        

        public static bool FilterContentFromTrustedUsers
        {
            get { return ConfigHelper.GetBoolProperty("Forum:FilterContentFromTrustedUsers", false); }
        }

        /// <summary>
        /// when we see reports of forums in google analytics the urls give no indication of what the topic is
        /// which makes them less useful especially in the new real time reports where we might like to see what threads are being viewed
        /// this is false by default though becuase if set to true the url we track is not the real url
        /// so clicking the url will result in a 404 page not found
        /// </summary>
        public static bool TrackFakeTopicUrlInAnalytics
        {
            get { return ConfigHelper.GetBoolProperty("Forum:TrackFakeTopicUrlInAnalytics", false); }
        }

        /// <summary>
        /// Some bots including google search bot will follow urls in the javascript even though google should know better
        /// since we may track urls that do not exist.
        /// by using /nofollow/ as the first segment then we can add Disallow: /nofollow/ in robots.txt to tell bots not to crawl the fake urls
        /// 
        /// </summary>
        public static string FakeTrackingBaseUrl
        {
            get { return ConfigHelper.GetStringProperty("Forum:FakeTrackingBaseUrl", "/nofollow/forum/"); }
        }

        public static string ForumThreadImage
        {
            get { return ConfigHelper.GetStringProperty("ForumThreadImage", "folder.png"); }
        }

        public static string SmtpServer
        {
            get { return ConfigHelper.GetStringProperty("Forum:SmtpServer", string.Empty); }
        }

        public static int SmtpServerPort
        {
            get { return ConfigHelper.GetIntProperty("Forum:SmtpServerPort", 25); }
        }

        public static string SmtpUser
        {
            get { return ConfigHelper.GetStringProperty("Forum:SmtpUser", string.Empty); }
        }

        public static string SmtpUserPassword
        {
            get { return ConfigHelper.GetStringProperty("Forum:SmtpUserPassword", string.Empty); }
        }

        public static bool SmtpRequiresAuthentication
        {
            get { return ConfigHelper.GetBoolProperty("Forum:SmtpRequiresAuthentication", false); }
        }

        public static bool SmtpUseSsl
        {
            get { return ConfigHelper.GetBoolProperty("Forum:SmtpUseSsl", false); }
        }

        public static string SmtpPreferredEncoding
        {
            get { return ConfigHelper.GetStringProperty("Forum:SmtpPreferredEncoding", string.Empty); }
        }

        public static bool AggregateSearchIndexPerThread
        {
            get { return ConfigHelper.GetBoolProperty("Forum:AggregateSearchIndexPerThread", false); }
        }

        public static bool EnableSiteMap
        {
            get { return ConfigHelper.GetBoolProperty("Forum:EnableSiteMap", true); }
        }

        public static int SiteMapCacheMinutes
        {
            get { return ConfigHelper.GetIntProperty("Forum:SiteMapCacheMinutes", 15); }
        }

        /// <summary>
        /// we use NeatHtml to protect from cross site scripting and by default we only allow relative urls for images
        /// because there can be security risks to allowing external images
        /// not to mention browser warnings about insecure content if you are using SSL (and you should use SSL)
        /// setting this to true will allow external images in forum posts
        /// </summary>
        public static bool AllowExternalImages
        {
            get { return ConfigHelper.GetBoolProperty("Forum:AllowExternalImages", false); }
        }

        public static bool UseMetaDescriptionOnThreads
        {
            get { return ConfigHelper.GetBoolProperty("Forum:UseMetaDescriptionOnThreads", false); }
        }

        public static bool CombineUrlParams
        {
            get { return ConfigHelper.GetBoolProperty("Forum:CombineUrlParams", true); }
        }

        public static bool ShowPagerViewAllLink
        {
            get { return ConfigHelper.GetBoolProperty("Forum:ShowPagerViewAllLink", true); }
        }

        /// <summary>
        /// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11417~1#post47516
        /// </summary>
        public static bool UseOldParamsForGoogleReader
        {
            get { return ConfigHelper.GetBoolProperty("Forum:UseOldParamsForGoogleReader", true); }
        }

        public static bool DisableThreadCanonicalUrl
        {
            get { return ConfigHelper.GetBoolProperty("Forum:DisableThreadCanonicalUrl", false); }
        }

        /// <summary>
        /// if true and the skin is using altcontent1 it will load the page content for that in the blog detail view
        /// </summary>
        public static bool ShowTopContent
        {
            get { return ConfigHelper.GetBoolProperty("Forum:ShowTopContent", false); }
        }

        /// <summary>
        /// if true and the skin is using altcontent2 it will load the page content for that in the blog detail view
        /// </summary>
        public static bool ShowBottomContent
        {
            get { return ConfigHelper.GetBoolProperty("Forum:ShowBottomContent", false); }
        }

        public static SmtpSettings GetSmtpSettings()
        {
            if (ForumConfiguration.SmtpServer.Length > 0)
            {
                SmtpSettings smtpSettings = new SmtpSettings();
                smtpSettings.Server = ForumConfiguration.SmtpServer;
                smtpSettings.Port = ForumConfiguration.SmtpServerPort;
                smtpSettings.User = ForumConfiguration.SmtpUser;
                smtpSettings.Password = ForumConfiguration.SmtpUserPassword;
                smtpSettings.UseSsl = ForumConfiguration.SmtpUseSsl;
                smtpSettings.RequiresAuthentication = ForumConfiguration.SmtpRequiresAuthentication;
                smtpSettings.PreferredEncoding = ForumConfiguration.SmtpPreferredEncoding;

                return smtpSettings;

            }


            return SiteUtils.GetSmtpSettings();
        }
    }
}