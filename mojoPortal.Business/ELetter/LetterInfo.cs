// Author:					
// Created:				    2007-09-22
// Last Modified:			2018-10-25
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

        public LetterInfo() { }

        public LetterInfo(Guid letterInfoGuid)
        {
            GetLetterInfo(letterInfoGuid);
        }

		#endregion

		#region Properties

		public Guid LetterInfoGuid { get; set; } = Guid.Empty;
		public Guid SiteGuid { get; set; } = Guid.Empty;
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string AvailableToRoles { get; set; } = string.Empty;
		public bool Enabled { get; set; } = false;
		public bool AllowUserFeedback { get; set; } = false;
		public bool AllowAnonFeedback { get; set; } = false;
		public string FromAddress { get; set; } = string.Empty;
		public string FromName { get; set; } = string.Empty;
		public string ReplyToAddress { get; set; } = string.Empty;
		public int SendMode { get; set; } = 0;
		public bool EnableViewAsWebPage { get; set; } = false;
		public bool EnableSendLog { get; set; } = false;
		public string RolesThatCanEdit { get; set; } = string.Empty;
		public string RolesThatCanApprove { get; set; } = string.Empty;
		public string RolesThatCanSend { get; set; } = string.Empty;
		public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
		public Guid CreatedBy { get; set; } = Guid.Empty;
		public DateTime LastModUtc { get; set; } = DateTime.UtcNow;
		public Guid LastModBy { get; set; } = Guid.Empty;
		public DateTime LastSentUtc { get; set; } = DateTime.MaxValue;
		public int SubscriberCount { get; private set; } = 0;
		public int UnVerifiedCount { get; private set; } = 0;
		public bool AllowArchiveView { get; set; } = true;
		public bool ProfileOptIn { get; set; } = false;
		public int SortRank { get; set; } = 500;
		public string DisplayNameDefault { get; set; } = string.Empty;
		public string FirstNameDefault { get; set; } = string.Empty;
		public string LastNameDefault { get; set; } = string.Empty;

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
                    this.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    this.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.Title = reader["Title"].ToString();
                    this.Description = reader["Description"].ToString();
                    this.AvailableToRoles = reader["AvailableToRoles"].ToString();
                    this.Enabled = Convert.ToBoolean(reader["Enabled"]);
                    this.AllowUserFeedback = Convert.ToBoolean(reader["AllowUserFeedback"]);
                    this.AllowAnonFeedback = Convert.ToBoolean(reader["AllowAnonFeedback"]);
                    this.FromAddress = reader["FromAddress"].ToString();
                    this.FromName = reader["FromName"].ToString();
                    this.ReplyToAddress = reader["ReplyToAddress"].ToString();
                    this.SendMode = Convert.ToInt32(reader["SendMode"]);
                    this.EnableViewAsWebPage = Convert.ToBoolean(reader["EnableViewAsWebPage"]);
                    this.EnableSendLog = Convert.ToBoolean(reader["EnableSendLog"]);
                    this.RolesThatCanEdit = reader["RolesThatCanEdit"].ToString();
                    this.RolesThatCanApprove = reader["RolesThatCanApprove"].ToString();
                    this.RolesThatCanSend = reader["RolesThatCanSend"].ToString();
                    this.SubscriberCount = Convert.ToInt32(reader["SubscriberCount"]);
                    this.UnVerifiedCount = Convert.ToInt32(reader["UnVerifiedCount"]);
                    this.CreatedUtc = Convert.ToDateTime(reader["CreatedUTC"]);
                    this.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    this.LastModUtc = Convert.ToDateTime(reader["LastModUTC"]);
                    this.LastModBy = new Guid(reader["LastModBy"].ToString());

                    this.AllowArchiveView = Convert.ToBoolean(reader["AllowArchiveView"]);
                    this.ProfileOptIn = Convert.ToBoolean(reader["ProfileOptIn"]);
                    this.SortRank = Convert.ToInt32(reader["SortRank"]);

                    this.DisplayNameDefault = reader["DisplayNameDefault"].ToString();
                    this.FirstNameDefault = reader["FirstNameDefault"].ToString();
                    this.LastNameDefault = reader["LastNameDefault"].ToString();

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

            this.LetterInfoGuid = newID;

            int rowsAffected = DBLetterInfo.Create(
                this.LetterInfoGuid,
                this.SiteGuid,
                this.Title,
                this.Description,
                this.AvailableToRoles,
                this.Enabled,
                this.AllowUserFeedback,
                this.AllowAnonFeedback,
                this.FromAddress,
                this.FromName,
                this.ReplyToAddress,
                this.SendMode,
                this.EnableViewAsWebPage,
                this.EnableSendLog,
                this.RolesThatCanEdit,
                this.RolesThatCanApprove,
                this.RolesThatCanSend,
                this.CreatedUtc,
                this.CreatedBy,
                this.LastModUtc,
                this.LastModBy,
                this.AllowArchiveView,
                this.ProfileOptIn,
                this.SortRank,
                this.DisplayNameDefault,
                this.FirstNameDefault,
                this.LastNameDefault);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of LetterInfo. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBLetterInfo.Update(
                this.LetterInfoGuid,
                this.SiteGuid,
                this.Title,
                this.Description,
                this.AvailableToRoles,
                this.Enabled,
                this.AllowUserFeedback,
                this.AllowAnonFeedback,
                this.FromAddress,
                this.FromName,
                this.ReplyToAddress,
                this.SendMode,
                this.EnableViewAsWebPage,
                this.EnableSendLog,
                this.RolesThatCanEdit,
                this.RolesThatCanApprove,
                this.RolesThatCanSend,
                this.CreatedUtc,
                this.CreatedBy,
                this.LastModUtc,
                this.LastModBy,
                this.AllowArchiveView,
                this.ProfileOptIn,
                this.SortRank,
                this.DisplayNameDefault,
                this.FirstNameDefault,
                this.LastNameDefault);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of LetterInfo. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.LetterInfoGuid != Guid.Empty)
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
                    letterInfo.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letterInfo.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterInfo.Title = reader["Title"].ToString();
                    letterInfo.Description = reader["Description"].ToString();
                    letterInfo.AvailableToRoles = reader["AvailableToRoles"].ToString();
                    letterInfo.Enabled = Convert.ToBoolean(reader["Enabled"]);
                    letterInfo.AllowUserFeedback = Convert.ToBoolean(reader["AllowUserFeedback"]);
                    letterInfo.AllowAnonFeedback = Convert.ToBoolean(reader["AllowAnonFeedback"]);
                    letterInfo.FromAddress = reader["FromAddress"].ToString();
                    letterInfo.FromName = reader["FromName"].ToString();
                    letterInfo.ReplyToAddress = reader["ReplyToAddress"].ToString();
                    letterInfo.SendMode = Convert.ToInt32(reader["SendMode"]);
                    letterInfo.EnableViewAsWebPage = Convert.ToBoolean(reader["EnableViewAsWebPage"]);
                    letterInfo.EnableSendLog = Convert.ToBoolean(reader["EnableSendLog"]);
                    letterInfo.RolesThatCanEdit = reader["RolesThatCanEdit"].ToString();
                    letterInfo.RolesThatCanApprove = reader["RolesThatCanApprove"].ToString();
                    letterInfo.RolesThatCanSend = reader["RolesThatCanSend"].ToString();
                    letterInfo.CreatedUtc = Convert.ToDateTime(reader["CreatedUTC"]);
                    letterInfo.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    letterInfo.LastModUtc = Convert.ToDateTime(reader["LastModUTC"]);
                    letterInfo.LastModBy = new Guid(reader["LastModBy"].ToString());
                    letterInfo.SubscriberCount = Convert.ToInt32(reader["SubscriberCount"]);
                    letterInfo.UnVerifiedCount = Convert.ToInt32(reader["UnVerifiedCount"]);
                    letterInfo.AllowArchiveView = Convert.ToBoolean(reader["AllowArchiveView"]);
                    letterInfo.ProfileOptIn = Convert.ToBoolean(reader["ProfileOptIn"]);
                    letterInfo.SortRank = Convert.ToInt32(reader["SortRank"]);

                    letterInfo.DisplayNameDefault = reader["DisplayNameDefault"].ToString();
                    letterInfo.FirstNameDefault = reader["FirstNameDefault"].ToString();
                    letterInfo.LastNameDefault = reader["LastNameDefault"].ToString();
					if (reader["SendClickedUTC"] != DBNull.Value)
					{
						letterInfo.LastSentUtc = Convert.ToDateTime(reader["SendClickedUTC"]);
					}
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
