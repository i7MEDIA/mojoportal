// Author:					
// Created:				    2009-10-29
// Last Modified:			2012-05-18
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
using System.Globalization;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using Resources;

namespace mojoPortal.Web
{
    public static class NewsletterHelper
    {
        public static List<LetterInfo> GetAvailableNewslettersForCurrentUser(Guid siteGuid)
        {
            List<LetterInfo> available = new List<LetterInfo>();
            List<LetterInfo> all = LetterInfo.GetAll(siteGuid);

            foreach (LetterInfo letter in all)
            {
                if (WebUser.IsInRoles(letter.AvailableToRoles))
                {
                    available.Add(letter);
                }
            }


            return available;

        }

        public static List<LetterInfo> GetAvailableNewslettersForSiteMembers(Guid siteGuid)
        {
            List<LetterInfo> available = new List<LetterInfo>();
            List<LetterInfo> all = LetterInfo.GetAll(siteGuid);

            foreach (LetterInfo letter in all)
            {
                if ((letter.AvailableToRoles.Contains("All Users"))||(letter.AvailableToRoles.Contains("Authenticated Users")))
                {
                    available.Add(letter);
                }
            }


            return available;

        }

        /// <summary>
        /// people can subscribe to the newsletters without registering on the site. This method is used to attach those existing subscriptions to the user upon registration
        /// </summary>
        /// <param name="siteUser"></param>
        public static void ClaimExistingSubscriptions(SiteUser siteUser)
        {
            SubscriberRepository subscriptions = new SubscriberRepository();
            List<LetterSubscriber> userSubscriptions = subscriptions.GetListByEmail(siteUser.SiteGuid, siteUser.Email);

            foreach (LetterSubscriber s in userSubscriptions)
            {
                
                if (s.UserGuid != siteUser.UserGuid)
                {
                    s.UserGuid = siteUser.UserGuid;
                    subscriptions.Save(s);
                }

                if (!s.IsVerified)
                {
                    subscriptions.Verify(s.SubscribeGuid, true, Guid.Empty);
                    LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

                }

            }

            List<LetterSubscriber> memberSubscriptions = subscriptions.GetListByUser(siteUser.SiteGuid, siteUser.UserGuid);

            RemoveDuplicates(memberSubscriptions);

            // commented out 2012-11-16 since we now give the user a chance to opt in the registration
            // then we should not force him in if he chose not to opt in

            //if (memberSubscriptions.Count == 0)
            //{
            //    string ipAddress = SiteUtils.GetIP4Address();
            //    //user has no previous subscriptions and just registered
            //    // lets give him the site subscriptions that are configured for opting in new users by default
            //    List<LetterInfo> allNewsletters = LetterInfo.GetAll(siteUser.SiteGuid);
            //    foreach (LetterInfo l in allNewsletters)
            //    {
            //        if ((l.ProfileOptIn) && (l.AvailableToRoles.Contains("All Users;")))
            //        {
            //            LetterSubscriber s = new LetterSubscriber();
            //            s.SiteGuid = siteUser.SiteGuid;
            //            s.LetterInfoGuid = l.LetterInfoGuid;
            //            s.UserGuid = siteUser.UserGuid;
            //            s.EmailAddress = siteUser.Email;
            //            s.IsVerified = true;
            //            s.UseHtml = true;
            //            s.IpAddress = ipAddress;
            //            subscriptions.Save(s);

            //        }


            //    }

            //}

        }

        public static void VerifyExistingSubscriptions(SiteUser siteUser)
        {
            SubscriberRepository subscriptions = new SubscriberRepository();
            List<LetterSubscriber> userSubscriptions = subscriptions.GetListByUser(siteUser.SiteGuid, siteUser.UserGuid);
            foreach (LetterSubscriber s in userSubscriptions)
            {
                
                if (!s.IsVerified)
                {
                    subscriptions.Verify(s.SubscribeGuid, true, Guid.Empty);
                    LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

                }

            }

            

        }

        public static void RemoveDuplicates(List<LetterSubscriber> userSubscriptions)
        {
            SubscriberRepository subscriptions = new SubscriberRepository();
            List<LetterSubscriber> cleanSubscriptions = new List<LetterSubscriber>();
            List<LetterSubscriber> duplicates = new List<LetterSubscriber>();

            foreach (LetterSubscriber s in userSubscriptions)
            {
                if (!ContainsSubscription(cleanSubscriptions, s.LetterInfoGuid))
                {
                    cleanSubscriptions.Add(s);

                }
                else
                {
                    duplicates.Add(s);
                }

            }

            foreach (LetterSubscriber s in duplicates)
            {
                subscriptions.Delete(s, false);
                LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

            }

        }

        private static bool ContainsSubscription(List<LetterSubscriber> userSubscriptions, Guid letterInfoGuid)
        {
            foreach (LetterSubscriber sub in userSubscriptions)
            {
                if (sub.LetterInfoGuid == letterInfoGuid) { return true; }

            }

            return false;

        }

        public static void SendSubscriberVerificationEmail(
            string siteRoot,
            string emailAddress,
            Guid subscribeGuid,
            LetterInfo letter,
            SiteSettings siteSettings)
        {
            string verificationTemplate;
            bool useHtml = WebConfigSettings.NewsletterUseHtmlEmailConfirmation;
            if (useHtml)
            {
                verificationTemplate = ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), "NewsletterVerificationHtmlEmailMessage.config");
            }
            else
            {
                verificationTemplate = ResourceHelper.GetMessageTemplate(SiteUtils.GetDefaultUICulture(), "NewsletterVerificationEmailMessage.config");
            }
            
            string confirmLink = siteRoot + "/eletter/Confirm.aspx?s=" + subscribeGuid.ToString();
            string messageBody = verificationTemplate.Replace("{NewsletterName}", letter.Title).Replace("{ConfirmationLink}", confirmLink).Replace("{SiteLink}", siteRoot);
            string subject = string.Format(CultureInfo.InvariantCulture, Resource.NewsletterVerifySubjectFormat, letter.Title);

            string fromAddress;
            string fromAlias;
            if (letter.FromAddress.Length > 0)
            {
                
                fromAddress = letter.FromAddress;
                fromAlias = letter.FromName;
            }
            else
            {
               
                fromAddress = siteSettings.DefaultEmailFromAddress;
                fromAlias = siteSettings.DefaultFromEmailAlias;
            }
           
            Email.Send(
                    SiteUtils.GetSmtpSettings(),
                    fromAddress,
                    fromAlias,
                    string.Empty,
                    emailAddress,
                    string.Empty,
                    string.Empty,
                    subject,
                    messageBody,
                    useHtml,
                    Email.PriorityNormal);
        }

    }
}
