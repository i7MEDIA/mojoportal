// Author:					
// Created:				    2007-09-22
// Last Modified:			2012-11-06
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
    /// Represents a user subscription to a news letter.
    /// </summary>
    public class LetterSubscriber
    {

        #region Constructors

        public LetterSubscriber()
        { }


        //public LetterSubscriber(Guid letterGuid, Guid userGuid)
        //{
        //    passedLetterGuid = letterGuid;
        //    passedUserGuid = userGuid;
        //    GetLetterSubscriber(passedLetterGuid, passedUserGuid);
        //}

        #endregion

        #region Private Properties

        private Guid subscribeGuid = Guid.Empty;
        private Guid passedLetterGuid = Guid.Empty;
        private Guid passedUserGuid = Guid.Empty;

        private Guid siteGuid = Guid.Empty;
        private Guid letterInfoGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid verifyGuid = Guid.Empty;
        private bool useHtml = true;
        private DateTime beginUTC = DateTime.UtcNow;
        private string emailAddress = string.Empty;
        private string name = string.Empty;
        private bool isVerified = false;
        private string ipAddress = string.Empty;


        

        #endregion

        #region Public Properties

        public Guid SubscribeGuid
        {
            get { return subscribeGuid; }
            set { subscribeGuid = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid LetterInfoGuid
        {
            get { return letterInfoGuid; }
            set { letterInfoGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public Guid VerifyGuid
        {
            get { return verifyGuid; }
            set { verifyGuid = value; }
        }

        public bool UseHtml
        {
            get { return useHtml; }
            set { useHtml = value; }
        }

        public bool IsVerified
        {
            get { return isVerified; }
            set { isVerified = value; }
        }

        public DateTime BeginUtc
        {
            get { return beginUTC; }
            set { beginUTC = value; }
        }


        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        private string firstName = string.Empty;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName = string.Empty;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        #endregion

        //#region Private Methods

        ///// <summary>
        ///// Gets an instance of LetterSubscriber.
        ///// </summary>
        ///// <param name="letterGuid"> letterGuid </param>
        ///// <param name="userGuid"> userGuid </param>
        //private void GetLetterSubscriber(Guid letterGuid, Guid userGuid)
        //{
        //    using (IDataReader reader = DBLetterSubscriber.GetOne(letterGuid, userGuid))
        //    {
        //        if (reader.Read())
        //        {
        //            this.letterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
        //            this.userGuid = new Guid(reader["UserGuid"].ToString());
        //            this.useHtml = Convert.ToBoolean(reader["UseHtml"]);
        //            this.beginUTC = Convert.ToDateTime(reader["BeginUTC"]);
        //            this.emailAddress = reader["Email"].ToString();
        //            this.name = reader["Name"].ToString();

        //        }
        //    }
        //}

        ///// <summary>
        ///// Persists a new instance of LetterSubscriber. Returns true on success.
        ///// </summary>
        ///// <returns></returns>
        //private bool Create()
        //{
        //    int rowsAffected = 0;

        //    rowsAffected = DBLetterSubscriber.Create(
        //        this.passedLetterGuid,
        //        this.passedUserGuid,
        //        this.useHtml,
        //        this.beginUTC);

        //    return (rowsAffected > 0);

        //}


        


        //#endregion

        //#region Public Methods

        ///// <summary>
        ///// Saves this instance of LetterSubscriber. Returns true on success.
        ///// </summary>
        ///// <returns>bool</returns>
        //public bool Save()
        //{
        //    //if(this.userGuid == Guid.Empty)
        //    //{
        //       return Create();
        //    //}
        //    // TODO: delete and create?
        //    //return false;
        //}




        //#endregion

        //#region Static Methods

        ///// <summary>
        ///// Deletes an instance of LetterSubscriber. Returns true on success.
        ///// </summary>
        ///// <param name="rowID"> rowID </param>
        ///// <returns>bool</returns>
        //public static bool Delete(Guid letterInfoGuid, Guid userGuid)
        //{
        //    LetterSubscriber subscriber = new LetterSubscriber(letterInfoGuid, userGuid);
        //    if (subscriber.LetterInfoGuid != Guid.Empty)
        //    {
        //        DBLetterSubscriber.CreateHistory(
        //            subscriber.letterInfoGuid,
        //            subscriber.UserGuid,
        //            subscriber.UseHtml,
        //            subscriber.BeginUtc,
        //            DateTime.UtcNow);
        //    }

        //    bool result = DBLetterSubscriber.Delete(letterInfoGuid, userGuid);
        //    if (result) LetterInfo.UpdateSubscriberCount(letterInfoGuid);

        //    return result;
        //}

        ///// <summary>
        ///// Deletes a row from the mp_LetterSubscriber table. Returns true if row deleted.
        ///// </summary>
        ///// <param name="letterGuid"> letterGuid </param>
        ///// <returns>bool</returns>
        //public static bool DeleteByLetter(Guid letterInfoGuid)
        //{
        //    return DBLetterSubscriber.DeleteByLetter(letterInfoGuid);

        //}

        //private static List<LetterSubscriber> LoadFromReader(IDataReader reader)
        //{
        //    List<LetterSubscriber> letterSubscriberList
        //        = new List<LetterSubscriber>();
        //    try
        //    {
        //        while (reader.Read())
        //        {
        //            LetterSubscriber letterSubscriber = new LetterSubscriber();
        //            letterSubscriber.letterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
        //            letterSubscriber.userGuid = new Guid(reader["UserGuid"].ToString());
        //            letterSubscriber.useHtml = Convert.ToBoolean(reader["UseHtml"]);
        //            letterSubscriber.beginUTC = Convert.ToDateTime(reader["BeginUTC"]);
        //            letterSubscriber.emailAddress = reader["Email"].ToString();
        //            letterSubscriber.name = reader["Name"].ToString();
        //            letterSubscriberList.Add(letterSubscriber);
        //        }
        //    }
        //    finally
        //    {
        //        reader.Close();
        //    }
            
        //    return letterSubscriberList;


        //}

        ///// <summary>
        ///// Gets an IList with instances of LetterSubscriber for the user.
        ///// </summary>
        ///// <param name="userGuid"> userGuid </param>
        //public static List<LetterSubscriber> GetByUser(Guid userGuid)
        //{
        //    IDataReader reader = DBLetterSubscriber.GetByUser(userGuid);
        //    return LoadFromReader(reader);
            
        //}

        ///// <summary>
        ///// Gets an IList instances of LetterSubscriber for the letter.
        ///// </summary>
        ///// <param name="letterInfoGuid"> letterInfoGuid </param>
        //public static List<LetterSubscriber> GetByLetter(Guid letterInfoGuid)
        //{
        //    IDataReader reader = DBLetterSubscriber.GetByLetter(letterInfoGuid);
        //    return LoadFromReader(reader);
            
        //}

        ///// <summary>
        ///// Gets a list of subscribers who have not yet been sent the specified letter
        ///// as indicated in the mp_Letter_SendLog table
        ///// </summary>
        ///// <param name="letterGuid"></param>
        ///// <param name="letterInfoGuid"></param>
        ///// <returns></returns>
        //public static List<LetterSubscriber> GetSubscribersNotSentYet(
        //    Guid letterGuid,
        //    Guid letterInfoGuid)
        //{
        //    IDataReader reader = DBLetterSubscriber.GetSubscribersNotSentYet(letterGuid, letterInfoGuid);
        //    return LoadFromReader(reader);
           
        //}

        ///// <summary>
        ///// Gets an IList with page of instances of LetterSubscriber.
        ///// </summary>
        ///// <param name="letterInfoGuid"> letterInfoGuid </param>
        //public static List<LetterSubscriber> GetPage(
        //    Guid letterInfoGuid,
        //    int pageNumber, 
        //    int pageSize, 
        //    out int totalPages)
        //{
        //    totalPages = 1;

        //    IDataReader reader
        //        = DBLetterSubscriber.GetPage(
        //        letterInfoGuid,
        //        pageNumber,
        //        pageSize,
        //        out totalPages);
            
        //    return LoadFromReader(reader);
            
        //}



        //#endregion

        


    }

}