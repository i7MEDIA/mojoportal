// Author:					
// Created:					2009-03-09
// Last Modified:			2009-03-09
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
using mojoPortal.Data;

namespace mojoPortal.Business
{

    public class EmailSendQueue
    {

        #region Constructors

        public EmailSendQueue()
        { }


        //public EmailSendQueue(Guid guid)
        //{
        //    GetEmailSendQueue(guid);
        //}

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid specialGuid1 = Guid.Empty;
        private Guid specialGuid2 = Guid.Empty;
        private string fromAddress = string.Empty;
        private string replyTo = string.Empty;
        private string toAddress = string.Empty;
        private string ccAddress = string.Empty;
        private string bccAddress = string.Empty;
        private string subject = string.Empty;
        private string textBody = string.Empty;
        private string htmlBody = string.Empty;
        private string type = string.Empty;
        private DateTime dateToSend;
        private DateTime createdUtc;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid SpecialGuid1
        {
            get { return specialGuid1; }
            set { specialGuid1 = value; }
        }
        public Guid SpecialGuid2
        {
            get { return specialGuid2; }
            set { specialGuid2 = value; }
        }
        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }
        public string ReplyTo
        {
            get { return replyTo; }
            set { replyTo = value; }
        }
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }
        public string CcAddress
        {
            get { return ccAddress; }
            set { ccAddress = value; }
        }
        public string BccAddress
        {
            get { return bccAddress; }
            set { bccAddress = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string TextBody
        {
            get { return textBody; }
            set { textBody = value; }
        }
        public string HtmlBody
        {
            get { return htmlBody; }
            set { htmlBody = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public DateTime DateToSend
        {
            get { return dateToSend; }
            set { dateToSend = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }

        #endregion

        #region Private Methods

        ///// <summary>
        ///// Gets an instance of EmailSendQueue.
        ///// </summary>
        ///// <param name="guid"> guid </param>
        //private void GetEmailSendQueue(
        //    Guid guid)
        //{
        //    using (IDataReader reader = DBEmailSendQueue.GetOne(
        //        guid))
        //    {
        //        PopulateFromReader(reader);
        //    }

        //}


        //private void PopulateFromReader(IDataReader reader)
        //{
        //    if (reader.Read())
        //    {
        //        this.guid = new Guid(reader["Guid"].ToString());
        //        this.siteGuid = new Guid(reader["SiteGuid"].ToString());
        //        this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
        //        this.userGuid = new Guid(reader["UserGuid"].ToString());
        //        this.specialGuid1 = new Guid(reader["SpecialGuid1"].ToString());
        //        this.specialGuid2 = new Guid(reader["SpecialGuid2"].ToString());
        //        this.fromAddress = reader["FromAddress"].ToString();
        //        this.replyTo = reader["ReplyTo"].ToString();
        //        this.toAddress = reader["ToAddress"].ToString();
        //        this.ccAddress = reader["CcAddress"].ToString();
        //        this.bccAddress = reader["BccAddress"].ToString();
        //        this.subject = reader["Subject"].ToString();
        //        this.textBody = reader["TextBody"].ToString();
        //        this.htmlBody = reader["HtmlBody"].ToString();
        //        this.type = reader["Type"].ToString();
        //        this.dateToSend = Convert.ToDateTime(reader["DateToSend"]);
        //        this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);

        //    }

        //}

        /// <summary>
        /// Persists a new instance of EmailSendQueue. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBEmailSendQueue.Create(
                this.guid,
                this.siteGuid,
                this.moduleGuid,
                this.userGuid,
                this.specialGuid1,
                this.specialGuid2,
                this.fromAddress,
                this.replyTo,
                this.toAddress,
                this.ccAddress,
                this.bccAddress,
                this.subject,
                this.textBody,
                this.htmlBody,
                this.type,
                this.dateToSend,
                this.createdUtc);

            return (rowsAffected > 0);

        }


        ///// <summary>
        ///// Updates this instance of EmailSendQueue. Returns true on success.
        ///// </summary>
        ///// <returns>bool</returns>
        //private bool Update()
        //{

        //    return DBEmailSendQueue.Update(
        //        this.guid,
        //        this.siteGuid,
        //        this.moduleGuid,
        //        this.userGuid,
        //        this.specialGuid1,
        //        this.specialGuid2,
        //        this.fromAddress,
        //        this.replyTo,
        //        this.toAddress,
        //        this.ccAddress,
        //        this.bccAddress,
        //        this.subject,
        //        this.textBody,
        //        this.htmlBody,
        //        this.type,
        //        this.dateToSend,
        //        this.createdUtc);

        //}





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of EmailSendQueue. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            //if (this.guid != Guid.Empty)
            //{
            //    return Update();
            //}
            //else
            //{
                return Create();
            //}
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of EmailSendQueue. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBEmailSendQueue.Delete(guid);
        }

        public static IDataReader GetEmailToSend()
        {
            return DBEmailSendQueue.GetEmailToSend(DateTime.UtcNow);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_EmailSendQueue table where DateToSend >= CurrentTime.
        /// </summary>
        /// <param name="currentTime"> currentTime </param>
        public static IDataReader GetEmailToSend(DateTime currentTime)
        {
            return DBEmailSendQueue.GetEmailToSend(currentTime);


        }


        

        private static List<EmailSendQueue> LoadListFromReader(IDataReader reader)
        {
            List<EmailSendQueue> emailSendQueueList = new List<EmailSendQueue>();
            try
            {
                while (reader.Read())
                {
                    EmailSendQueue emailSendQueue = new EmailSendQueue();
                    emailSendQueue.guid = new Guid(reader["Guid"].ToString());
                    emailSendQueue.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    emailSendQueue.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    emailSendQueue.userGuid = new Guid(reader["UserGuid"].ToString());
                    emailSendQueue.specialGuid1 = new Guid(reader["SpecialGuid1"].ToString());
                    emailSendQueue.specialGuid2 = new Guid(reader["SpecialGuid2"].ToString());
                    emailSendQueue.fromAddress = reader["FromAddress"].ToString();
                    emailSendQueue.replyTo = reader["ReplyTo"].ToString();
                    emailSendQueue.toAddress = reader["ToAddress"].ToString();
                    emailSendQueue.ccAddress = reader["CcAddress"].ToString();
                    emailSendQueue.bccAddress = reader["BccAddress"].ToString();
                    emailSendQueue.subject = reader["Subject"].ToString();
                    emailSendQueue.textBody = reader["TextBody"].ToString();
                    emailSendQueue.htmlBody = reader["HtmlBody"].ToString();
                    emailSendQueue.type = reader["Type"].ToString();
                    emailSendQueue.dateToSend = Convert.ToDateTime(reader["DateToSend"]);
                    emailSendQueue.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    emailSendQueueList.Add(emailSendQueue);

                }
            }
            finally
            {
                reader.Close();
            }

            return emailSendQueueList;

        }

        


        #endregion

        


    }

}
