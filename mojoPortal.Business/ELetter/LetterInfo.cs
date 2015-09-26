// Author:					Joe Audette
// Created:				    2007-09-22
// Last Modified:			2009-02-01
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
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents the definition of a news letter
    /// </summary>
    public class LetterInfo
    {

        #region Constructors

        public LetterInfo()
        { }


        public LetterInfo(
            Guid letterInfoGuid)
        {
            GetLetterInfo(
                letterInfoGuid);
        }

        #endregion

        #region Private Properties

        private Guid letterInfoGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string title = string.Empty;
        private string description = string.Empty;
        private string availableToRoles = string.Empty;
        private bool enabled = false;
        private bool allowUserFeedback = false;
        private bool allowAnonFeedback = false;
        private string fromAddress = string.Empty;
        private string fromName = string.Empty;
        private string replyToAddress = string.Empty;
        private int sendMode = 0;
        private bool enableViewAsWebPage = false;
        private bool enableSendLog = false;
        private string rolesThatCanEdit = string.Empty;
        private string rolesThatCanApprove = string.Empty;
        private string rolesThatCanSend = string.Empty;
        private int subscriberCount = 0;
        private DateTime createdUTC = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;
        private DateTime lastModUTC = DateTime.UtcNow;
        private Guid lastModBy = Guid.Empty;
        private bool allowArchiveView = true;
        private bool profileOptIn = false;
        private int sortRank = 500;
        private int unVerifiedCount = 0;

        
        

        #endregion

        #region Public Properties

        public Guid LetterInfoGuid
        {
            get { return letterInfoGuid; }
            set { letterInfoGuid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string AvailableToRoles
        {
            get { return availableToRoles; }
            set { availableToRoles = value; }
        }
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public bool AllowUserFeedback
        {
            get { return allowUserFeedback; }
            set { allowUserFeedback = value; }
        }
        public bool AllowAnonFeedback
        {
            get { return allowAnonFeedback; }
            set { allowAnonFeedback = value; }
        }
        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }
        public string FromName
        {
            get { return fromName; }
            set { fromName = value; }
        }
        public string ReplyToAddress
        {
            get { return replyToAddress; }
            set { replyToAddress = value; }
        }
        public int SendMode
        {
            get { return sendMode; }
            set { sendMode = value; }
        }
        public bool EnableViewAsWebPage
        {
            get { return enableViewAsWebPage; }
            set { enableViewAsWebPage = value; }
        }
        public bool EnableSendLog
        {
            get { return enableSendLog; }
            set { enableSendLog = value; }
        }
        public string RolesThatCanEdit
        {
            get { return rolesThatCanEdit; }
            set { rolesThatCanEdit = value; }
        }
        public string RolesThatCanApprove
        {
            get { return rolesThatCanApprove; }
            set { rolesThatCanApprove = value; }
        }
        public string RolesThatCanSend
        {
            get { return rolesThatCanSend; }
            set { rolesThatCanSend = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUTC; }
            set { createdUTC = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUTC; }
            set { lastModUTC = value; }
        }
        public Guid LastModBy
        {
            get { return lastModBy; }
            set { lastModBy = value; }
        }

        public int SubscriberCount
        {
            get { return subscriberCount; }

        }

        public int UnVerifiedCount
        {
            get { return unVerifiedCount; }

        }

        public bool AllowArchiveView
        {
            get { return allowArchiveView; }
            set { allowArchiveView = value; }
        }

        public bool ProfileOptIn
        {
            get { return profileOptIn; }
            set { profileOptIn = value; }
        }

        public int SortRank
        {
            get { return sortRank; }
            set { sortRank = value; }
        }

        private string displayNameDefault = string.Empty;

        public string DisplayNameDefault
        {
            get { return displayNameDefault; }
            set { displayNameDefault = value; }
        }

        private string firstNameDefault = string.Empty;

        public string FirstNameDefault
        {
            get { return firstNameDefault; }
            set { firstNameDefault = value; }
        }

        private string lastNameDefault = string.Empty;

        public string LastNameDefault
        {
            get { return lastNameDefault; }
            set { lastNameDefault = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of LetterInfo.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        private void GetLetterInfo(Guid letterInfoGuid)
        {
            using (IDataReader reader = DBLetterInfo.GetOne(letterInfoGuid))
            {
                if (reader.Read())
                {
                    this.letterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    this.availableToRoles = reader["AvailableToRoles"].ToString();
                    this.enabled = Convert.ToBoolean(reader["Enabled"]);
                    this.allowUserFeedback = Convert.ToBoolean(reader["AllowUserFeedback"]);
                    this.allowAnonFeedback = Convert.ToBoolean(reader["AllowAnonFeedback"]);
                    this.fromAddress = reader["FromAddress"].ToString();
                    this.fromName = reader["FromName"].ToString();
                    this.replyToAddress = reader["ReplyToAddress"].ToString();
                    this.sendMode = Convert.ToInt32(reader["SendMode"]);
                    this.enableViewAsWebPage = Convert.ToBoolean(reader["EnableViewAsWebPage"]);
                    this.enableSendLog = Convert.ToBoolean(reader["EnableSendLog"]);
                    this.rolesThatCanEdit = reader["RolesThatCanEdit"].ToString();
                    this.rolesThatCanApprove = reader["RolesThatCanApprove"].ToString();
                    this.rolesThatCanSend = reader["RolesThatCanSend"].ToString();
                    this.subscriberCount = Convert.ToInt32(reader["SubscriberCount"]);
                    this.unVerifiedCount = Convert.ToInt32(reader["UnVerifiedCount"]);
                    this.createdUTC = Convert.ToDateTime(reader["CreatedUTC"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);
                    this.lastModBy = new Guid(reader["LastModBy"].ToString());

                    this.allowArchiveView = Convert.ToBoolean(reader["AllowArchiveView"]);
                    this.profileOptIn = Convert.ToBoolean(reader["ProfileOptIn"]);
                    this.sortRank = Convert.ToInt32(reader["SortRank"]);

                    this.displayNameDefault = reader["DisplayNameDefault"].ToString();
                    this.firstNameDefault = reader["FirstNameDefault"].ToString();
                    this.lastNameDefault = reader["LastNameDefault"].ToString();

                }

            }

        }

        /// <summary>
        /// Persists a new instance of LetterInfo. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.letterInfoGuid = newID;

            int rowsAffected = DBLetterInfo.Create(
                this.letterInfoGuid,
                this.siteGuid,
                this.title,
                this.description,
                this.availableToRoles,
                this.enabled,
                this.allowUserFeedback,
                this.allowAnonFeedback,
                this.fromAddress,
                this.fromName,
                this.replyToAddress,
                this.sendMode,
                this.enableViewAsWebPage,
                this.enableSendLog,
                this.rolesThatCanEdit,
                this.rolesThatCanApprove,
                this.rolesThatCanSend,
                this.createdUTC,
                this.createdBy,
                this.lastModUTC,
                this.lastModBy,
                this.allowArchiveView,
                this.profileOptIn,
                this.sortRank,
                this.displayNameDefault,
                this.firstNameDefault,
                this.lastNameDefault);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of LetterInfo. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBLetterInfo.Update(
                this.letterInfoGuid,
                this.siteGuid,
                this.title,
                this.description,
                this.availableToRoles,
                this.enabled,
                this.allowUserFeedback,
                this.allowAnonFeedback,
                this.fromAddress,
                this.fromName,
                this.replyToAddress,
                this.sendMode,
                this.enableViewAsWebPage,
                this.enableSendLog,
                this.rolesThatCanEdit,
                this.rolesThatCanApprove,
                this.rolesThatCanSend,
                this.createdUTC,
                this.createdBy,
                this.lastModUTC,
                this.lastModBy,
                this.allowArchiveView,
                this.profileOptIn,
                this.sortRank,
                this.displayNameDefault,
                this.firstNameDefault,
                this.lastNameDefault);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of LetterInfo. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.letterInfoGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a count of rows in the mp_LetterInfo table.
        /// </summary>
        public static int GetCount(Guid siteGuid)
        {
            return DBLetterInfo.GetCount(siteGuid);
        }

        /// <summary>
        /// Deletes an instance of LetterInfo. Returns true on success.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid letterInfoGuid)
        {
            return DBLetterInfo.Delete(letterInfoGuid);
        }

        /// <summary>
        /// Updates the subscriber count on an instance of LetterInfo.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool UpdateSubscriberCount(Guid letterInfoGuid)
        {
            return DBLetterInfo.UpdateSubscriberCount(letterInfoGuid);
        }

        /// <summary>
        /// Gets an IList with all instances of LetterInfo.
        /// </summary>
        public static List<LetterInfo> GetAll(Guid siteGuid)
        {
            IDataReader reader = DBLetterInfo.GetAll(siteGuid);
            return LoadFromReader(reader);

        }

        private static List<LetterInfo> LoadFromReader(IDataReader reader)
        {
            List<LetterInfo> letterInfoList = new List<LetterInfo>();
            try
            {
                while (reader.Read())
                {
                    LetterInfo letterInfo = new LetterInfo();
                    letterInfo.letterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letterInfo.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterInfo.title = reader["Title"].ToString();
                    letterInfo.description = reader["Description"].ToString();
                    letterInfo.availableToRoles = reader["AvailableToRoles"].ToString();
                    letterInfo.enabled = Convert.ToBoolean(reader["Enabled"]);
                    letterInfo.allowUserFeedback = Convert.ToBoolean(reader["AllowUserFeedback"]);
                    letterInfo.allowAnonFeedback = Convert.ToBoolean(reader["AllowAnonFeedback"]);
                    letterInfo.fromAddress = reader["FromAddress"].ToString();
                    letterInfo.fromName = reader["FromName"].ToString();
                    letterInfo.replyToAddress = reader["ReplyToAddress"].ToString();
                    letterInfo.sendMode = Convert.ToInt32(reader["SendMode"]);
                    letterInfo.enableViewAsWebPage = Convert.ToBoolean(reader["EnableViewAsWebPage"]);
                    letterInfo.enableSendLog = Convert.ToBoolean(reader["EnableSendLog"]);
                    letterInfo.rolesThatCanEdit = reader["RolesThatCanEdit"].ToString();
                    letterInfo.rolesThatCanApprove = reader["RolesThatCanApprove"].ToString();
                    letterInfo.rolesThatCanSend = reader["RolesThatCanSend"].ToString();
                    letterInfo.createdUTC = Convert.ToDateTime(reader["CreatedUTC"]);
                    letterInfo.createdBy = new Guid(reader["CreatedBy"].ToString());
                    letterInfo.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);
                    letterInfo.lastModBy = new Guid(reader["LastModBy"].ToString());
                    letterInfo.subscriberCount = Convert.ToInt32(reader["SubscriberCount"]);
                    letterInfo.unVerifiedCount = Convert.ToInt32(reader["UnVerifiedCount"]);
                    letterInfo.AllowArchiveView = Convert.ToBoolean(reader["AllowArchiveView"]);
                    letterInfo.ProfileOptIn = Convert.ToBoolean(reader["ProfileOptIn"]);
                    letterInfo.SortRank = Convert.ToInt32(reader["SortRank"]);

                    letterInfo.displayNameDefault = reader["DisplayNameDefault"].ToString();
                    letterInfo.firstNameDefault = reader["FirstNameDefault"].ToString();
                    letterInfo.lastNameDefault = reader["LastNameDefault"].ToString();

                    letterInfoList.Add(letterInfo);
                }
            }
            finally
            {
                reader.Close();
            }

            return letterInfoList;

        }

        /// <summary>
        /// Gets an IList with page of instances of LetterInfo.
        /// </summary>
        public static List<LetterInfo> GetPage(
            Guid siteGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            IDataReader reader
                = DBLetterInfo.GetPage(
                siteGuid,
                pageNumber, 
                pageSize, 
                out totalPages);

            return LoadFromReader(reader);

            

        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByTitle(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.Title.CompareTo(letterInfo2.Title);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByDescription(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.Description.CompareTo(letterInfo2.Description);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByAvailableToRoles(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.AvailableToRoles.CompareTo(letterInfo2.AvailableToRoles);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByFromAddress(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.FromAddress.CompareTo(letterInfo2.FromAddress);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByFromName(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.FromName.CompareTo(letterInfo2.FromName);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByReplyToAddress(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.ReplyToAddress.CompareTo(letterInfo2.ReplyToAddress);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareBySendMode(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.SendMode.CompareTo(letterInfo2.SendMode);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByRolesThatCanEdit(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.RolesThatCanEdit.CompareTo(letterInfo2.RolesThatCanEdit);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByRolesThatCanApprove(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.RolesThatCanApprove.CompareTo(letterInfo2.RolesThatCanApprove);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByRolesThatCanSend(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.RolesThatCanSend.CompareTo(letterInfo2.RolesThatCanSend);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByCreatedUtc(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.CreatedUtc.CompareTo(letterInfo2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of LetterInfo.
        /// </summary>
        public static int CompareByLastModUtc(LetterInfo letterInfo1, LetterInfo letterInfo2)
        {
            return letterInfo1.LastModUtc.CompareTo(letterInfo2.LastModUtc);
        }

        #endregion


    }

}
