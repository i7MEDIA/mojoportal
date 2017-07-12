// Author:					
// Created:					2012-08-10
// Last Modified:			2012-08-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace mojoPortal.Business
{

    public class Comment
    {

        public const byte ModerationApproved = 1;
        public const byte ModerationPending = 0;
        public const byte ModerationSpam = 2;
        public const byte ModerationRejected = 3;

        public Comment()
        { }


        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid parentGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid contentGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private string title = string.Empty;
        private string userComment = string.Empty;
        private string userName = string.Empty;
        private string userEmail = string.Empty;
        private string userUrl = string.Empty;
        private string userIp = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        private byte moderationStatus = 1; //default to approved but can be set by feature 
        private Guid moderatedBy = Guid.Empty;
        private string moderationReason = string.Empty;

        //external properties
        private string postAuthor = string.Empty;
        private string authorEmail = string.Empty;
        private decimal userRevenue = 0;
        private bool trusted = false;
        private string postAuthorAvatar = string.Empty;
        private string postAuthorWebSiteUrl = string.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid ParentGuid
        {
            get { return parentGuid; }
            set { parentGuid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public Guid ContentGuid
        {
            get { return contentGuid; }
            set { contentGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string UserComment
        {
            get { return userComment; }
            set { userComment = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }
        public string UserUrl
        {
            get { return userUrl; }
            set { userUrl = value; }
        }
        public string UserIp
        {
            get { return userIp; }
            set { userIp = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }
        public byte ModerationStatus
        {
            get { return moderationStatus; }
            set { moderationStatus = value; }
        }
        public Guid ModeratedBy
        {
            get { return moderatedBy; }
            set { moderatedBy = value; }
        }
        public string ModerationReason
        {
            get { return moderationReason; }
            set { moderationReason = value; }
        }


        //external properties
        

        public string PostAuthor
        {
            get { return postAuthor; }
            set { postAuthor = value; }
        }

        public string AuthorEmail
        {
            get { return authorEmail; }
            set { authorEmail = value; }
        }


        public decimal UserRevenue
        {
            get { return userRevenue; }
            set { userRevenue = value; }
        }

        public bool Trusted
        {
            get { return trusted; }
            set { trusted = value; }
        }

        public string PostAuthorAvatar
        {
            get { return postAuthorAvatar; }
            set { postAuthorAvatar = value; }
        }

        public string PostAuthorWebSiteUrl
        {
            get { return postAuthorWebSiteUrl; }
            set { postAuthorWebSiteUrl = value; }
        }

        private int userId = -1;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        #endregion

        


    }

}

