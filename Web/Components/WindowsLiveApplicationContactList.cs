//-----------------------------------------------------------------------
// <copyright file="ApplicationContactList.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Sample code to create a signed Application contact list.
// </summary>
//-----------------------------------------------------------------------
//namespace WindowsLive
namespace mojoPortal.Web
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    /// <summary>
    /// Sample code to create Application ticket
    /// </summary>
    public class ApplicationContactList
    {

        /// <summary>
        /// Creates an Application contact list in XML format.
        /// The expected format of the list is:
        ///     <Ticket>
        ///         <Contact id="4678100456789" />
        ///         <Contact id="-38793459038445" />
        ///         <TS>2006-04-17T14:22:48.2698750-07</TS>
        ///         <CID>6738390403485</CID>
        ///     </Ticket>
        /// </summary>
        /// <param name="cid">
        /// The Cid of the user for which the Application contact list should be created.
        /// </param>
        /// <param name="applicationContacts">
        /// The list of the application contacts.
        /// </param>
        /// <returns>Application contact List</returns>
        public string CreateApplicationContactList(long cid, long[] applicationContacts)
        {
            if (cid == 0)
            {
                throw new ArgumentException("Invalid Identifier", "cid");
            }

            if (applicationContacts == null || applicationContacts.Length == 0)
            {
                throw new ArgumentException("There should be atleast one contact in the applicationContacts");
            }

            string ticketXmlFormat =
                "<Ticket>" +
                "{0}" +
                "<TS>{1}</TS>" +
                "<CID>{2}</CID>" +
                "</Ticket>";

            string contactsXmlFormat = "<Contact id=\"{0}\"/>";

            StringBuilder contactCids = new StringBuilder();
            foreach (long contact in applicationContacts)
            {
                contactCids.Append(string.Format(contactsXmlFormat, contact));
            }

            string applicationContactList =
                String.Format(
                ticketXmlFormat,
                contactCids.ToString(),
                DateTime.UtcNow.ToString("o"),
                cid);

            return applicationContactList;
        }

        /// <summary>
        /// Creates an Application Contact List in XML Format.
        /// </summary>
        /// <param name="cid">
        /// The Cid in the hex format of the user for which the Application contact list should be created.
        /// </param>
        /// <param name="applicationContacts">
        /// The list of the application contacts.
        /// </param>
        /// <returns>Application contact List</returns>
        public string CreateApplicationContactList(string strCid, string[] applicationContacts)
        {
            if (String.IsNullOrEmpty(strCid))
            {
                throw new ArgumentException("Cid cannot be null", "strCid");
            }

            if (applicationContacts == null || applicationContacts.Length == 0)
            {
                throw new ArgumentException("There should be atleast one contact in the applicationContacts");
            }

            long cid;
            if (!long.TryParse(strCid, out cid))
            {
                throw new ArgumentException("Invalid Identifier", "cid");
            }

            List<long> appContactCids = new List<long>(applicationContacts.Length);
            long contactCid;

            foreach (string applicationContact in applicationContacts)
            {
                if (!long.TryParse(applicationContact, out contactCid))
                {
                    throw new ArgumentException("Invalid Identifier", "applicationContact");
                }

                appContactCids.Add(contactCid);
            }

            return this.CreateApplicationContactList(cid, appContactCids.ToArray());
        }

        /// <summary>
        /// Signs the application contact list created 
        /// from <see cref="ApplicationContactList.CreateApplicationContactList" />
        /// using a shared secret
        /// </summary>
        /// <param name="applicationContactList">
        /// The application contact list to be signed.
        /// </param>
        /// <param name="signatureKey">
        /// The signature key, this is the shared secret available in the Delegation token 
        /// of the user for whom the contact list is being created.
        /// </param>
        /// <returns>
        /// Base64 encoded signature of the applicationContactList.
        /// </returns>
        public string SignApplicationContactList(string applicationContactList, byte[] signatureKey)
        {
            if (string.IsNullOrEmpty(applicationContactList))
            {
                throw new ArgumentException("Invalid application contact list", "applicationContactList");
            }

            if (signatureKey == null || signatureKey.Length == 0)
            {
                throw new ArgumentException("Invalid signature Key", "signatureKey");
            }

            using (HMACSHA256 sha256 = new HMACSHA256(signatureKey))
            {
                byte[] signature = sha256.ComputeHash(Encoding.UTF8.GetBytes(applicationContactList));
                return Convert.ToBase64String(signature);
            }
        }
    }
}
