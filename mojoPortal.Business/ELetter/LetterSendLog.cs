// Author:					
// Created:				    2007-09-23
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
    /// Represents a log fro tracking the sending of email news letters.
    /// This allows us to resume if the task is interupted.
    /// </summary>
    public class LetterSendLog
    {

        #region Constructors

        public LetterSendLog()
        { }


        public LetterSendLog(
            int rowId)
        {
            GetLetterSendLog(
                rowId);
        }

        #endregion

        #region Private Properties

        private int rowID = -1;
        private Guid letterGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid subscribeGuid = Guid.Empty;
        private string emailAddress;
        private DateTime uTC = DateTime.UtcNow;
        private bool errorOccurred;
        private string errorMessage;

        #endregion

        #region Public Properties

        public int RowId
        {
            get { return rowID; }
            set { rowID = value; }
        }
        public Guid LetterGuid
        {
            get { return letterGuid; }
            set { letterGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid SubscribeGuid
        {
            get { return subscribeGuid; }
            set { subscribeGuid = value; }
        }
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }
        public DateTime UTC
        {
            get { return uTC; }
            set { uTC = value; }
        }
        public bool ErrorOccurred
        {
            get { return errorOccurred; }
            set { errorOccurred = value; }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of LetterSendLog.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        private void GetLetterSendLog(int rowId)
        {
            using (IDataReader reader = DBLetterSendLog.GetOne(rowId))
            {
                if (reader.Read())
                {
                    this.rowID = Convert.ToInt32(reader["RowID"]);
                    this.letterGuid = new Guid(reader["LetterGuid"].ToString());
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    this.subscribeGuid = new Guid(reader["SubscribeGuid"].ToString());
                    this.emailAddress = reader["EmailAddress"].ToString();
                    this.uTC = Convert.ToDateTime(reader["UTC"]);
                    this.errorOccurred = Convert.ToBoolean(reader["ErrorOccurred"]);
                    this.errorMessage = reader["ErrorMessage"].ToString();

                }

            }

        }

        /// <summary>
        /// Persists a new instance of LetterSendLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBLetterSendLog.Create(
                this.letterGuid,
                this.userGuid,
                this.subscribeGuid,
                this.emailAddress,
                this.uTC,
                this.errorOccurred,
                this.errorMessage);

            this.rowID = newID;

            return (newID > 0);

        }


        /// <summary>
        /// Updates this instance of LetterSendLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBLetterSendLog.Update(
                this.rowID,
                this.letterGuid,
                this.userGuid,
                this.emailAddress,
                this.uTC,
                this.errorOccurred,
                this.errorMessage);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of LetterSendLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowID > -1)
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
        /// Deletes an instance of LetterSendLog. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(int rowId)
        {
            return DBLetterSendLog.Delete(rowId);
        }

        /// <summary>
        /// Deletes from the mp_LetterSendLog table for the letterGuid. Returns true if row deleted.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetter(Guid letterGuid)
        {
            return DBLetterSendLog.DeleteByLetter(letterGuid);
        }

        public static bool DeleteByLetterInfo(Guid letterInfoGuid)
        {
            return DBLetterSendLog.DeleteByLetterInfo(letterInfoGuid);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBLetterSendLog.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Gets a count of subscribers a letter was sent to.
        /// </summary>
        public static int GetCount(Guid letterGuid)
        {
            return DBLetterSendLog.GetCount(letterGuid);

        }

        ///// <summary>
        ///// Gets an IList with all instances of LetterSendLog.
        ///// </summary>
        //public static List<LetterSendLog> GetByLetter(Guid letterGuid)
        //{
        //    List<LetterSendLog> letterSendLogList
        //        = new List<LetterSendLog>();

        //    IDataReader reader
        //        = DBLetterSendLog.GetByLetter(letterGuid);

        //    while (reader.Read())
        //    {
        //        LetterSendLog letterSendLog = new LetterSendLog();
        //        letterSendLog.rowID = Convert.ToInt32(reader["RowID"]);
        //        letterSendLog.letterGuid = new Guid(reader["LetterGuid"].ToString());
        //        letterSendLog.userGuid = new Guid(reader["UserGuid"].ToString());
        //        letterSendLog.emailAddress = reader["EmailAddress"].ToString();
        //        letterSendLog.uTC = Convert.ToDateTime(reader["UTC"]);
        //        letterSendLog.errorOccurred = Convert.ToBoolean(reader["ErrorOccurred"]);
        //        letterSendLog.errorMessage = reader["ErrorMessage"].ToString();
        //        letterSendLogList.Add(letterSendLog);
        //    }
        //    reader.Close();

        //    return letterSendLogList;

        //}

        /// <summary>
        /// Gets an IList with page of instances of LetterSendLog.
        /// </summary>
        public static List<LetterSendLog> GetPage(
            Guid letterGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            List<LetterSendLog> letterSendLogList = new List<LetterSendLog>();

            using (IDataReader reader
                = DBLetterSendLog.GetPage(
                letterGuid,
                pageNumber,
                pageSize,
                out totalPages))
            {
                while (reader.Read())
                {
                    LetterSendLog letterSendLog = new LetterSendLog();
                    letterSendLog.rowID = Convert.ToInt32(reader["RowID"]);
                    letterSendLog.letterGuid = new Guid(reader["LetterGuid"].ToString());
                    letterSendLog.userGuid = new Guid(reader["UserGuid"].ToString());
                    letterSendLog.emailAddress = reader["EmailAddress"].ToString();
                    letterSendLog.uTC = Convert.ToDateTime(reader["UTC"]);
                    letterSendLog.errorOccurred = Convert.ToBoolean(reader["ErrorOccurred"]);
                    letterSendLog.errorMessage = reader["ErrorMessage"].ToString();
                    letterSendLogList.Add(letterSendLog);

                }
            }

            return letterSendLogList;

        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of LetterSendLog.
        /// </summary>
        public static int CompareByRowId(LetterSendLog letterSendLog1, LetterSendLog letterSendLog2)
        {
            return letterSendLog1.RowId.CompareTo(letterSendLog2.RowId);
        }
        /// <summary>
        /// Compares 2 instances of LetterSendLog.
        /// </summary>
        public static int CompareByEmailAddress(LetterSendLog letterSendLog1, LetterSendLog letterSendLog2)
        {
            return letterSendLog1.EmailAddress.CompareTo(letterSendLog2.EmailAddress);
        }
        /// <summary>
        /// Compares 2 instances of LetterSendLog.
        /// </summary>
        public static int CompareByUtc(LetterSendLog letterSendLog1, LetterSendLog letterSendLog2)
        {
            return letterSendLog1.UTC.CompareTo(letterSendLog2.UTC);
        }
        /// <summary>
        /// Compares 2 instances of LetterSendLog.
        /// </summary>
        public static int CompareByErrorMessage(LetterSendLog letterSendLog1, LetterSendLog letterSendLog2)
        {
            return letterSendLog1.ErrorMessage.CompareTo(letterSendLog2.ErrorMessage);
        }

        #endregion


    }

}
