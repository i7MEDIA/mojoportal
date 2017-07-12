// Author:					
// Created:					2009-10-11
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
    public class SubscriberRepository
    {
        public SubscriberRepository()
        { }

        public void Save(LetterSubscriber subsciption)
        {
            if (subsciption == null) { return; }
            if (subsciption.SubscribeGuid == Guid.Empty)
            {
                subsciption.SubscribeGuid = Guid.NewGuid();

                DBLetterSubscription.Create(
                    subsciption.SubscribeGuid,
                    subsciption.SiteGuid,
                    subsciption.LetterInfoGuid,
                    subsciption.UserGuid,
                    subsciption.EmailAddress.ToLower(),
                    subsciption.IsVerified,
                    subsciption.VerifyGuid,
                    DateTime.UtcNow,
                    subsciption.UseHtml,
                    subsciption.IpAddress);
            }
            else
            {
                DBLetterSubscription.Update(
                    subsciption.SubscribeGuid,
                    subsciption.UserGuid,
                    subsciption.UseHtml);

            }
        }

        public LetterSubscriber Fetch(Guid guid)
        {
            using (IDataReader reader = DBLetterSubscription.GetOne(guid))
            {
                if (reader.Read())
                {
                    LetterSubscriber letterSubscriber = new LetterSubscriber();
                    letterSubscriber.SubscribeGuid = new Guid(reader["Guid"].ToString());
                    letterSubscriber.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterSubscriber.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letterSubscriber.VerifyGuid = new Guid(reader["VerifyGuid"].ToString());
                    letterSubscriber.UserGuid = new Guid(reader["UserGuid"].ToString());
                    letterSubscriber.UseHtml = Convert.ToBoolean(reader["UseHtml"]);
                    letterSubscriber.BeginUtc = Convert.ToDateTime(reader["BeginUtc"]);
                    letterSubscriber.EmailAddress = reader["Email"].ToString();
                    letterSubscriber.Name = reader["Name"].ToString();
                    letterSubscriber.IsVerified = Convert.ToBoolean(reader["IsVerified"]);
                    letterSubscriber.IpAddress = reader["IpAddress"].ToString();

                    letterSubscriber.FirstName = reader["FirstName"].ToString();
                    letterSubscriber.LastName = reader["LastName"].ToString();

                    return letterSubscriber;
                }
            }

            return null;
        }

        public LetterSubscriber Fetch(Guid siteGuid, Guid letterInfoGuid, string email)
        {
            using (IDataReader reader = DBLetterSubscription.GetByEmail(siteGuid, letterInfoGuid, email.ToLower()))
            {
                if (reader.Read())
                {
                    LetterSubscriber letterSubscriber = new LetterSubscriber();
                    letterSubscriber.SubscribeGuid = new Guid(reader["Guid"].ToString());
                    letterSubscriber.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterSubscriber.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letterSubscriber.VerifyGuid = new Guid(reader["VerifyGuid"].ToString());
                    letterSubscriber.UserGuid = new Guid(reader["UserGuid"].ToString());
                    letterSubscriber.UseHtml = Convert.ToBoolean(reader["UseHtml"]);
                    letterSubscriber.BeginUtc = Convert.ToDateTime(reader["BeginUtc"]);
                    letterSubscriber.EmailAddress = reader["Email"].ToString();
                    letterSubscriber.Name = reader["Name"].ToString();
                    letterSubscriber.IsVerified = Convert.ToBoolean(reader["IsVerified"]);
                    letterSubscriber.IpAddress = reader["IpAddress"].ToString();

                    letterSubscriber.FirstName = reader["FirstName"].ToString();
                    letterSubscriber.LastName = reader["LastName"].ToString();

                    return letterSubscriber;
                }
            }

            return null;
        }

        

        public bool Exists(Guid letterInfoGuid, string email)
        {
            return DBLetterSubscription.Exists(letterInfoGuid, email.ToLower());
        }

        public bool Verify(
            Guid guid,
            bool isVerified,
            Guid verifyGuid)
        {
            return DBLetterSubscription.Verify(
                guid,
                isVerified,
                verifyGuid);
        }

        public bool Delete(LetterSubscriber subscription)
        {
            return Delete(subscription, true);
        }

        public bool Delete(LetterSubscriber subscription, bool createHistory)
        {
            if (subscription == null) { return false; }

            if (createHistory)
            {
                DBLetterSubscription.CreateHistory(
                    Guid.NewGuid(),
                    subscription.SiteGuid,
                    subscription.SubscribeGuid,
                    subscription.LetterInfoGuid,
                    subscription.UserGuid,
                    subscription.EmailAddress,
                    subscription.IsVerified,
                    subscription.UseHtml,
                    subscription.BeginUtc,
                    DateTime.UtcNow,
                    subscription.IpAddress);
            }



            return DBLetterSubscription.Delete(subscription.SubscribeGuid);
        }

        public bool DeleteByLetter(Guid letterInfoGuid)
        {
            return DBLetterSubscription.DeleteByLetter(letterInfoGuid);
        }

        public bool DeleteUnverified(Guid letterInfoGuid, DateTime olderThanUtc)
        {
            return DBLetterSubscription.DeleteUnverified(letterInfoGuid, olderThanUtc);
        }

        public bool DeleteByUser(Guid userGuid)
        {
           
            return DBLetterSubscription.DeleteByUser(userGuid);
        }

        public bool DeleteBySite(Guid siteGuid)
        {
            return DBLetterSubscription.DeleteBySite(siteGuid);
        }

        public bool DeleteHistoryBySite(Guid siteGuid)
        {
            return DBLetterSubscription.DeleteHistoryBySite(siteGuid);
        }

        public bool DeleteHistoryByLetterInfo(Guid letterInfoGuid)
        {
            return DBLetterSubscription.DeleteHistoryByLetterInfo(letterInfoGuid);
        }

        public IDataReader GetOne(Guid guid)
        {
            return DBLetterSubscription.GetOne(guid);
        }

        public IDataReader GetByLetter(Guid letterInfoGuid)
        {
            return DBLetterSubscription.GetByLetter(letterInfoGuid);
        }

        public DataTable GetVerifiedSubscribersForExport(Guid letterInfoGuid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("UseHtml", typeof(bool));
            dt.Columns.Add("BeginUtc", typeof(DateTime));
            dt.Columns.Add("IsVerified", typeof(bool));
            dt.Columns.Add("UserGuid", typeof(string));

            using (IDataReader reader = GetByLetter(letterInfoGuid))
            {
                while (reader.Read())
                {
                    
                    DataRow row = dt.NewRow();

                    bool isVerified = Convert.ToBoolean(reader["IsVerified"]);

                    row["Email"] = reader["Email"];
                    row["Name"] = reader["Name"];
                    row["UseHtml"] = Convert.ToBoolean(reader["UseHtml"]);
                    row["BeginUtc"] = Convert.ToDateTime(reader["BeginUtc"]);
                    row["IsVerified"] = isVerified;
                    row["UserGuid"] = reader["UserGuid"].ToString();
                    if (isVerified)
                    {
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

        public IDataReader GetByUser(Guid siteGuid, Guid userGuid)
        {
            return DBLetterSubscription.GetByUser(siteGuid, userGuid);
        }

        public IDataReader GetByEmail(Guid siteGuid, string email)
        {
            return DBLetterSubscription.GetByEmail(siteGuid, email.ToLower());
        }

        public static int CountUsersNotSubscribedByLetter(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            return DBLetterSubscription.CountUsersNotSubscribedByLetter(siteGuid, letterInfoGuid, excludeIfAnyUnsubscribeHx);
        }

        public static DataTable GetTop1000UsersNotSubscribed(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("UserGuid", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));

            using (IDataReader reader = DBLetterSubscription.GetTop1000UsersNotSubscribed(siteGuid, letterInfoGuid, excludeIfAnyUnsubscribeHx))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["UserID"] = Convert.ToInt32(reader["UserID"]);
                    row["UserGuid"] = reader["UserGuid"].ToString();
                    row["Email"] = reader["Email"].ToString();
                    dataTable.Rows.Add(row);

                }

            }

            return dataTable;
        }

        public List<LetterSubscriber> GetListByEmail(Guid siteGuid, string email)
        {
            IDataReader reader = DBLetterSubscription.GetByEmail(siteGuid, email.ToLower());
            return LoadFromReader(reader);
        }

        public List<LetterSubscriber> GetListByUser(Guid siteGuid, Guid userGuid)
        {
            IDataReader reader = DBLetterSubscription.GetByUser(siteGuid, userGuid);
            return LoadFromReader(reader);
        }

        public List<LetterSubscriber> GetSubscribersNotSentYet(
            Guid letterGuid,
            Guid letterInfoGuid)
        {
            IDataReader reader = DBLetterSubscription.GetSubscribersNotSentYet(letterGuid, letterInfoGuid);
            return LoadFromReader(reader);
        }

        public IDataReader Search(
            Guid letterInfoGuid,
            string emailOrIpAddress)
        {
            return DBLetterSubscription.Search(letterInfoGuid, emailOrIpAddress);
            
        }

        public int GetCountByLetter(Guid letterInfoGuid)
        {
            return DBLetterSubscription.GetCountByLetter(letterInfoGuid);
        }

        public IDataReader GetPage(
            Guid letterInfoGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBLetterSubscription.GetPage(
                letterInfoGuid,
                pageNumber,
                pageSize,
                out totalPages);

        }

        //public List<LetterSubscriber> GetListByUser(Guid siteGuid, Guid userGuid)
        //{
        //    IDataReader reader = DBLetterSubscription.GetByUser(siteGuid, userGuid);
        //    return LoadFromReader(reader);

        //}


        private List<LetterSubscriber> LoadFromReader(IDataReader reader)
        {
            List<LetterSubscriber> letterSubscriberList
                = new List<LetterSubscriber>();
            try
            {
                while (reader.Read())
                {
                    LetterSubscriber letterSubscriber = new LetterSubscriber();
                    letterSubscriber.SubscribeGuid = new Guid(reader["Guid"].ToString());
                    letterSubscriber.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    letterSubscriber.LetterInfoGuid = new Guid(reader["LetterInfoGuid"].ToString());
                    letterSubscriber.VerifyGuid = new Guid(reader["VerifyGuid"].ToString());
                    letterSubscriber.UserGuid = new Guid(reader["UserGuid"].ToString());
                    letterSubscriber.UseHtml = Convert.ToBoolean(reader["UseHtml"]);
                    letterSubscriber.BeginUtc = Convert.ToDateTime(reader["BeginUtc"]);
                    letterSubscriber.EmailAddress = reader["Email"].ToString();
                    
                    letterSubscriber.Name = reader["Name"].ToString();
                    letterSubscriber.IsVerified = Convert.ToBoolean(reader["IsVerified"]);
                    letterSubscriber.IpAddress = reader["IpAddress"].ToString();

                    letterSubscriber.FirstName = reader["FirstName"].ToString();
                    letterSubscriber.LastName = reader["LastName"].ToString();

                    letterSubscriberList.Add(letterSubscriber);
                }
            }
            finally
            {
                reader.Close();
            }

            return letterSubscriberList;

        }

    }
}
