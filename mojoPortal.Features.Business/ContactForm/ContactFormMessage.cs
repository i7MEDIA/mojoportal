// Author:					
// Created:				    2008-03-28
// Last Modified:			2009-06-22
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
    /// Represents a message posted in the contact form.
    /// </summary>
    public class ContactFormMessage
    {
        private const string featureGuid = "99019eb8-db13-4bb3-81ea-073571a60dba";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors

        public ContactFormMessage()
        { }


        public ContactFormMessage(Guid rowGuid)
        {
            GetContactFormMessage(rowGuid);
        }

        #endregion

        #region Private Properties

        private Guid rowGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private string email = string.Empty;
        private string userName = string.Empty;
        private string subject = string.Empty;
        private string message = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private string createdFromIpAddress = string.Empty;
        private Guid userGuid = Guid.Empty;

        #endregion

        #region Public Properties

        public Guid RowGuid
        {
            get { return rowGuid; }
            set { rowGuid = value; }
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
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public string CreatedFromIpAddress
        {
            get { return createdFromIpAddress; }
            set { createdFromIpAddress = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ContactFormMessage.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        private void GetContactFormMessage(
            Guid rowGuid)
        {
            using (IDataReader reader = DBContactFormMessage.GetOne(rowGuid))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.rowGuid = new Guid(reader["RowGuid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.email = reader["Email"].ToString();
                this.userName = reader["Url"].ToString();
                this.subject = reader["Subject"].ToString();
                this.message = reader["Message"].ToString();
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.createdFromIpAddress = reader["CreatedFromIpAddress"].ToString();
                this.userGuid = new Guid(reader["UserGuid"].ToString());

            }
            
        }

        /// <summary>
        /// Persists a new instance of ContactFormMessage. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            if (rowGuid == Guid.Empty)
                rowGuid = Guid.NewGuid();

            int rowsAffected = DBContactFormMessage.Create(
                this.rowGuid,
                this.siteGuid,
                this.moduleGuid,
                this.email,
                this.userName,
                this.subject,
                this.message,
                this.createdUtc,
                this.createdFromIpAddress,
                this.userGuid);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of ContactFormMessage. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBContactFormMessage.Update(
                this.rowGuid,
                this.siteGuid,
                this.moduleGuid,
                this.email,
                this.userName,
                this.subject,
                this.message,
                this.createdUtc,
                this.createdFromIpAddress,
                this.userGuid);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ContactFormMessage. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowGuid != Guid.Empty)
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
        /// Deletes an instance of ContactFormMessage. Returns true on success.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid rowGuid)
        {
            return DBContactFormMessage.Delete(rowGuid);
        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBContactFormMessage.DeleteByModule(moduleGuid);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBContactFormMessage.DeleteBySite(siteId);
        }


        /// <summary>
        /// Gets a count of ContactFormMessage. 
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            return DBContactFormMessage.GetCount(moduleGuid);
        }

        private static List<ContactFormMessage> LoadListFromReader(IDataReader reader)
        {
            List<ContactFormMessage> contactFormMessageList = new List<ContactFormMessage>();
            try
            {
                while (reader.Read())
                {
                    ContactFormMessage contactFormMessage = new ContactFormMessage();
                    contactFormMessage.rowGuid = new Guid(reader["RowGuid"].ToString());
                    contactFormMessage.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    contactFormMessage.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    contactFormMessage.email = reader["Email"].ToString();
                    contactFormMessage.userName = reader["Url"].ToString();
                    contactFormMessage.subject = reader["Subject"].ToString();
                    contactFormMessage.message = reader["Message"].ToString();
                    contactFormMessage.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    contactFormMessage.createdFromIpAddress = reader["CreatedFromIpAddress"].ToString();
                    contactFormMessage.userGuid = new Guid(reader["UserGuid"].ToString());
                    contactFormMessageList.Add(contactFormMessage);

                }
            }
            finally
            {
                reader.Close();
            }

            return contactFormMessageList;

        }

        ///// <summary>
        ///// Gets an IList with all instances of ContactFormMessage.
        ///// </summary>
        //public static List<ContactFormMessage> GetAll()
        //{
        //    IDataReader reader = DBContactFormMessage.GetAll();
        //    return LoadListFromReader(reader);

        //}


        /// <summary>
        /// Gets an IList with page of instances of ContactFormMessage.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPageReader(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            return DBContactFormMessage.GetPage(moduleGuid, pageNumber, pageSize, out totalPages);

        }

        /// <summary>
        /// Gets an IList with page of instances of ContactFormMessage.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<ContactFormMessage> GetPage(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBContactFormMessage.GetPage(moduleGuid, pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }




        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of ContactFormMessage.
        /// </summary>
        public static int CompareByEmail(ContactFormMessage contactFormMessage1, ContactFormMessage contactFormMessage2)
        {
            return contactFormMessage1.Email.CompareTo(contactFormMessage2.Email);
        }
        /// <summary>
        /// Compares 2 instances of ContactFormMessage.
        /// </summary>
        public static int CompareByUserName(ContactFormMessage contactFormMessage1, ContactFormMessage contactFormMessage2)
        {
            return contactFormMessage1.UserName.CompareTo(contactFormMessage2.UserName);
        }
        /// <summary>
        /// Compares 2 instances of ContactFormMessage.
        /// </summary>
        public static int CompareBySubject(ContactFormMessage contactFormMessage1, ContactFormMessage contactFormMessage2)
        {
            return contactFormMessage1.Subject.CompareTo(contactFormMessage2.Subject);
        }
        /// <summary>
        /// Compares 2 instances of ContactFormMessage.
        /// </summary>
        public static int CompareByMessage(ContactFormMessage contactFormMessage1, ContactFormMessage contactFormMessage2)
        {
            return contactFormMessage1.Message.CompareTo(contactFormMessage2.Message);
        }
        /// <summary>
        /// Compares 2 instances of ContactFormMessage.
        /// </summary>
        public static int CompareByCreatedUtc(ContactFormMessage contactFormMessage1, ContactFormMessage contactFormMessage2)
        {
            return contactFormMessage1.CreatedUtc.CompareTo(contactFormMessage2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of ContactFormMessage.
        /// </summary>
        public static int CompareByCreatedFromIpAddress(ContactFormMessage contactFormMessage1, ContactFormMessage contactFormMessage2)
        {
            return contactFormMessage1.CreatedFromIpAddress.CompareTo(contactFormMessage2.CreatedFromIpAddress);
        }

        #endregion


    }

}
