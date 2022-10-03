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

        public Letter() { }


        public Letter(Guid letterGuid)
        {
            GetLetter(letterGuid);
        }

		#endregion

		#region Properties

		public Guid LetterGuid { get; set; } = Guid.Empty;
		public Guid LetterInfoGuid { get; set; } = Guid.Empty;
		public string Subject { get; set; } = string.Empty;
		public string HtmlBody { get; set; } = string.Empty;
		public string TextBody { get; set; } = string.Empty;
		public Guid CreatedBy { get; set; } = Guid.Empty;
		public DateTime CreatedUtc { get; set; }
		public Guid LastModBy { get; set; } = Guid.Empty;
		public DateTime LastModUtc { get; set; }
		public bool IsApproved { get; set; }
		public Guid ApprovedBy { get; set; } = Guid.Empty;
		public DateTime SendClickedUtc { get; set; } = DateTime.MinValue;
		public DateTime SendStartedUtc { get; set; } = DateTime.MinValue;
		public DateTime SendCompleteUtc { get; set; } = DateTime.MinValue;
		public int SendCount { get; set; } = 0;

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
                    this.LetterGuid = new Guid(reader["LetterGuid"].ToString());
                    this.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    this.Subject = reader["Subject"].ToString();
                    this.HtmlBody = reader["HtmlBody"].ToString();
                    this.TextBody = reader["TextBody"].ToString();
                    this.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    this.CreatedUtc = Convert.ToDateTime(reader["CreatedUTC"]);
                    this.LastModBy = new Guid(reader["LastModBy"].ToString());
                    this.LastModUtc = Convert.ToDateTime(reader["LastModUTC"]);
                    this.IsApproved = Convert.ToBoolean(reader["IsApproved"]);
                    this.ApprovedBy = new Guid(reader["ApprovedBy"].ToString());

                    if (reader["SendClickedUTC"] != DBNull.Value)
                        this.SendClickedUtc = Convert.ToDateTime(reader["SendClickedUTC"]);

                    if (reader["SendStartedUTC"] != DBNull.Value)
                        this.SendStartedUtc = Convert.ToDateTime(reader["SendStartedUTC"]);

                    if (reader["SendCompleteUTC"] != DBNull.Value)
                        this.SendCompleteUtc = Convert.ToDateTime(reader["SendCompleteUTC"]);

                    this.SendCount = Convert.ToInt32(reader["SendCount"]);

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

            this.LetterGuid = newID;
            this.CreatedUtc = DateTime.UtcNow;
            this.LastModUtc = DateTime.UtcNow;

            int rowsAffected = DBLetter.Create(
                this.LetterGuid,
                this.LetterInfoGuid,
                this.Subject,
                this.HtmlBody,
                this.TextBody,
                this.CreatedBy,
                this.CreatedUtc,
                this.LastModBy,
                this.LastModUtc,
                this.IsApproved,
                this.ApprovedBy);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of Letter. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            this.LastModUtc = DateTime.UtcNow;

            return DBLetter.Update(
                this.LetterGuid,
                this.LetterInfoGuid,
                this.Subject,
                this.HtmlBody,
                this.TextBody,
                this.LastModBy,
                this.LastModUtc,
                this.IsApproved,
                this.ApprovedBy);

        }


        /// <summary>
        /// Records click of the send button. Sending occurs by a task queue.
        /// </summary>
        /// <returns></returns>
        public bool TrackSendClicked()
        {
            return DBLetter.SendClicked(this.LetterGuid, DateTime.UtcNow);

        }

        /// <summary>
        /// Records the start of sending the letter from the task queue.
        /// </summary>
        /// <returns></returns>
        public bool TrackSendStarted()
        {
            return DBLetter.SendStarted(this.LetterGuid, DateTime.UtcNow);

        }


        /// <summary>
        /// Records the completion of sending the letter from the task queue.
        /// </summary>
        /// <returns></returns>
        public bool TrackSendComplete(int sendCount)
        {
            return DBLetter.SendComplete(this.LetterGuid, DateTime.UtcNow, sendCount);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of Letter. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.LetterGuid != Guid.Empty)
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
                    letter.LetterGuid = new Guid(reader["LetterGuid"].ToString());
                    letter.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letter.Subject = reader["Subject"].ToString();
                    letter.HtmlBody = reader["HtmlBody"].ToString();
                    letter.TextBody = reader["TextBody"].ToString();
                    letter.CreatedBy = new Guid(reader["CreatedBy"].ToString());
                    letter.CreatedUtc = Convert.ToDateTime(reader["CreatedUTC"]);
                    letter.LastModBy = new Guid(reader["LastModBy"].ToString());
                    letter.LastModUtc = Convert.ToDateTime(reader["LastModUTC"]);
                    letter.IsApproved = Convert.ToBoolean(reader["IsApproved"]);
                    letter.ApprovedBy = new Guid(reader["ApprovedBy"].ToString());

                    if (reader["SendClickedUTC"] != DBNull.Value)
                        letter.SendClickedUtc = Convert.ToDateTime(reader["SendClickedUTC"]);

                    if (reader["SendStartedUTC"] != DBNull.Value)
                        letter.SendStartedUtc = Convert.ToDateTime(reader["SendStartedUTC"]);

                    if (reader["SendCompleteUTC"] != DBNull.Value)
                        letter.SendCompleteUtc = Convert.ToDateTime(reader["SendCompleteUTC"]);

                    if (reader["SendCount"] != DBNull.Value)
                        letter.SendCount = Convert.ToInt32(reader["SendCount"]);

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
