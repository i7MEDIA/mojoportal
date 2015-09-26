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
    /// Represents an instance of a news letter edition
    /// </summary>
    public class Letter
    {
        public const string UnsubscribeToken = "#unsubscribe#";
        public const string UserNameToken = "#username#";
        public const string UserEmailToken = "#email#";
        public const string WebPageLinkToken = "#viewaswebpage#";
        public const string UserDisplayNameToken = "#displayname#";
        public const string UserFirstNameToken = "#firstname#";
        public const string UserLastNameToken = "#lastname#";

        #region Constructors

        public Letter()
        { }


        public Letter(
            Guid letterGuid)
        {
            GetLetter(
                letterGuid);
        }

        #endregion

        #region Private Properties

        private Guid letterGuid = Guid.Empty;
        private Guid letterInfoGuid = Guid.Empty;
        private string subject = string.Empty;
        private string htmlBody = string.Empty;
        private string textBody = string.Empty;
        private Guid createdBy = Guid.Empty;
        private DateTime createdUTC;
        private Guid lastModBy = Guid.Empty;
        private DateTime lastModUTC;
        private bool isApproved;
        private Guid approvedBy = Guid.Empty;
        private DateTime sendClickedUTC = DateTime.MinValue;
        private DateTime sendStartedUTC = DateTime.MinValue;
        private DateTime sendCompleteUTC = DateTime.MinValue;
        private int sendCount = 0;

        #endregion

        #region Public Properties

        public Guid LetterGuid
        {
            get { return letterGuid; }
            set { letterGuid = value; }
        }
        public Guid LetterInfoGuid
        {
            get { return letterInfoGuid; }
            set { letterInfoGuid = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string HtmlBody
        {
            get { return htmlBody; }
            set { htmlBody = value; }
        }
        public string TextBody
        {
            get { return textBody; }
            set { textBody = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUTC; }
            set { createdUTC = value; }
        }
        public Guid LastModBy
        {
            get { return lastModBy; }
            set { lastModBy = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUTC; }
            set { lastModUTC = value; }
        }
        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }
        public Guid ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }
        public DateTime SendClickedUtc
        {
            get { return sendClickedUTC; }
            set { sendClickedUTC = value; }
        }
        public DateTime SendStartedUtc
        {
            get { return sendStartedUTC; }
            set { sendStartedUTC = value; }
        }
        public DateTime SendCompleteUtc
        {
            get { return sendCompleteUTC; }
            set { sendCompleteUTC = value; }
        }
        public int SendCount
        {
            get { return sendCount; }
            set { sendCount = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of Letter.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        private void GetLetter(Guid letterGuid)
        {
            using (IDataReader reader = DBLetter.GetOne(letterGuid))
            {
                if (reader.Read())
                {
                    this.letterGuid = new Guid(reader["LetterGuid"].ToString());
                    this.letterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    this.subject = reader["Subject"].ToString();
                    this.htmlBody = reader["HtmlBody"].ToString();
                    this.textBody = reader["TextBody"].ToString();
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                    this.createdUTC = Convert.ToDateTime(reader["CreatedUTC"]);
                    this.lastModBy = new Guid(reader["LastModBy"].ToString());
                    this.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);
                    this.isApproved = Convert.ToBoolean(reader["IsApproved"]);
                    this.approvedBy = new Guid(reader["ApprovedBy"].ToString());

                    if (reader["SendClickedUTC"] != DBNull.Value)
                        this.sendClickedUTC = Convert.ToDateTime(reader["SendClickedUTC"]);

                    if (reader["SendStartedUTC"] != DBNull.Value)
                        this.sendStartedUTC = Convert.ToDateTime(reader["SendStartedUTC"]);

                    if (reader["SendCompleteUTC"] != DBNull.Value)
                        this.sendCompleteUTC = Convert.ToDateTime(reader["SendCompleteUTC"]);

                    this.sendCount = Convert.ToInt32(reader["SendCount"]);

                }

            }

        }

        

        /// <summary>
        /// Persists a new instance of Letter. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.letterGuid = newID;
            this.createdUTC = DateTime.UtcNow;
            this.lastModUTC = DateTime.UtcNow;

            int rowsAffected = DBLetter.Create(
                this.letterGuid,
                this.letterInfoGuid,
                this.subject,
                this.htmlBody,
                this.textBody,
                this.createdBy,
                this.createdUTC,
                this.lastModBy,
                this.lastModUTC,
                this.isApproved,
                this.approvedBy);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of Letter. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            this.lastModUTC = DateTime.UtcNow;

            return DBLetter.Update(
                this.letterGuid,
                this.letterInfoGuid,
                this.subject,
                this.htmlBody,
                this.textBody,
                this.lastModBy,
                this.lastModUTC,
                this.isApproved,
                this.approvedBy);

        }


        /// <summary>
        /// Records click of the send button. Sending occurs by a task queue.
        /// </summary>
        /// <returns></returns>
        public bool TrackSendClicked()
        {
            return DBLetter.SendClicked(this.letterGuid, DateTime.UtcNow);

        }

        /// <summary>
        /// Records the start of sending the letter from the task queue.
        /// </summary>
        /// <returns></returns>
        public bool TrackSendStarted()
        {
            return DBLetter.SendStarted(this.letterGuid, DateTime.UtcNow);

        }


        /// <summary>
        /// Records the completion of sending the letter from the task queue.
        /// </summary>
        /// <returns></returns>
        public bool TrackSendComplete(int sendCount)
        {
            return DBLetter.SendComplete(this.letterGuid, DateTime.UtcNow, sendCount);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of Letter. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.letterGuid != Guid.Empty)
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
        /// Deletes an instance of Letter. Returns true on success.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid letterGuid)
        {
            LetterSendLog.DeleteByLetter(letterGuid);
            return DBLetter.Delete(letterGuid);
        }

        /// <summary>
        /// Deletes a row from the mp_Letter table. Returns true if row deleted.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            return DBLetter.DeleteByLetterInfo(letterInfoGuid);
        }

        private static List<Letter> PopulateFromReader(IDataReader reader)
        {
            List<Letter> letterList = new List<Letter>();
            try
            {
                while (reader.Read())
                {
                    Letter letter = new Letter();
                    letter.letterGuid = new Guid(reader["LetterGuid"].ToString());
                    letter.letterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letter.subject = reader["Subject"].ToString();
                    letter.htmlBody = reader["HtmlBody"].ToString();
                    letter.textBody = reader["TextBody"].ToString();
                    letter.createdBy = new Guid(reader["CreatedBy"].ToString());
                    letter.createdUTC = Convert.ToDateTime(reader["CreatedUTC"]);
                    letter.lastModBy = new Guid(reader["LastModBy"].ToString());
                    letter.lastModUTC = Convert.ToDateTime(reader["LastModUTC"]);
                    letter.isApproved = Convert.ToBoolean(reader["IsApproved"]);
                    letter.approvedBy = new Guid(reader["ApprovedBy"].ToString());

                    if (reader["SendClickedUTC"] != DBNull.Value)
                        letter.sendClickedUTC = Convert.ToDateTime(reader["SendClickedUTC"]);

                    if (reader["SendStartedUTC"] != DBNull.Value)
                        letter.sendStartedUTC = Convert.ToDateTime(reader["SendStartedUTC"]);

                    if (reader["SendCompleteUTC"] != DBNull.Value)
                        letter.sendCompleteUTC = Convert.ToDateTime(reader["SendCompleteUTC"]);

                    if (reader["SendCount"] != DBNull.Value)
                        letter.sendCount = Convert.ToInt32(reader["SendCount"]);

                    letterList.Add(letter);
                }
            }
            finally
            {
                reader.Close();
            }

            return letterList;


        }

        /// <summary>
        /// Gets an IList with all instances of Letter.
        /// </summary>
        public static List<Letter> GetAll(Guid letterInfoGuid)
        {
            List<Letter> letterList
                = new List<Letter>();

            IDataReader reader = DBLetter.GetAll(letterInfoGuid);

            return PopulateFromReader(reader);
            
        }

        /// <summary>
        /// Gets an IList with page of instances of Letter.
        /// </summary>
        public static List<Letter> GetPage(
            Guid letterInfoGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            IDataReader reader = DBLetter.GetPage(
                letterInfoGuid,
                pageNumber, 
                pageSize, 
                out totalPages);

            return PopulateFromReader(reader);

            
        }

        /// <summary>
        /// Gets an IList with page of instances of Letter.
        /// </summary>
        public static List<Letter> GetDrafts(
            Guid letterInfoGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;

            IDataReader reader = DBLetter.GetDrafts(
                letterInfoGuid,
                pageNumber,
                pageSize,
                out totalPages);

            return PopulateFromReader(reader);


        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareBySubject(Letter letter1, Letter letter2)
        {
            return letter1.Subject.CompareTo(letter2.Subject);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareByHtmlBody(Letter letter1, Letter letter2)
        {
            return letter1.HtmlBody.CompareTo(letter2.HtmlBody);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareByTextBody(Letter letter1, Letter letter2)
        {
            return letter1.TextBody.CompareTo(letter2.TextBody);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareByCreatedUtc(Letter letter1, Letter letter2)
        {
            return letter1.CreatedUtc.CompareTo(letter2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareByLastModUtc(Letter letter1, Letter letter2)
        {
            return letter1.LastModUtc.CompareTo(letter2.LastModUtc);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareBySendClickedUtc(Letter letter1, Letter letter2)
        {
            return letter1.SendClickedUtc.CompareTo(letter2.SendClickedUtc);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareBySendStartedUtc(Letter letter1, Letter letter2)
        {
            return letter1.SendStartedUtc.CompareTo(letter2.SendStartedUtc);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareBySendCompleteUtc(Letter letter1, Letter letter2)
        {
            return letter1.SendCompleteUtc.CompareTo(letter2.SendCompleteUtc);
        }
        /// <summary>
        /// Compares 2 instances of Letter.
        /// </summary>
        public static int CompareBySendCount(Letter letter1, Letter letter2)
        {
            return letter1.SendCount.CompareTo(letter2.SendCount);
        }

        #endregion


    }

}
