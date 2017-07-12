//-----------------------------------------------------------------------
// <copyright file="WindowsLiveMessenger.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Sample code for Windows Live Messenger
// </summary>
//-----------------------------------------------------------------------

// Modified by  2009-04-15

//namespace WindowsLive
namespace mojoPortal.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Web;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;

    //using WindowsLive;
    //using ConsentToken = WindowsLive.WindowsLiveLogin.ConsentToken;
    using ConsentToken = mojoPortal.Web.WindowsLiveLogin.ConsentToken;

    public class WindowsLiveMessenger
    {
        #region WindowsLiveMessenger Private Fields

        private readonly WindowsLiveLogin windowsLiveLogin;

        #endregion

        #region WindowsLiveMessenger Constructor

        public WindowsLiveMessenger()
        {
            this.windowsLiveLogin = new WindowsLiveLogin(true);
        }

        //added by 
        public WindowsLiveMessenger(WindowsLiveLogin liveLogin)
        {
            this.windowsLiveLogin = liveLogin;
        }

        #endregion

        #region WindowsLiveMessenger Public Properties

        public string ApplicationId
        {
            get { return ConfigurationManager.AppSettings["wll_appid"]; }
        }

        public string ApplicationVerifier
        {
            get { return this.windowsLiveLogin.GetAppVerifier(); }
        }

        public string ConsentOptInUrl
        {
            get
            {
                string optinUrl = ConfigurationManager.AppSettings["wlm_optinurl"];
                if (String.IsNullOrEmpty(optinUrl))
                {
                    optinUrl = "http://consent.messenger.services.live.com/delegation.aspx?ps=Messenger.SignIn&ru={0}&pl={1}&app={2}&appname={3}&applogourl={4}";
                }

                if (windowsLiveLogin == null) { return string.Empty; }

                return string.Format(
                    optinUrl,
                    HttpUtility.UrlEncode(windowsLiveLogin.ReturnUrl),
                    HttpUtility.UrlEncode(windowsLiveLogin.PolicyUrl),
                    this.ApplicationVerifier,
                    HttpUtility.UrlEncode(windowsLiveLogin.AppName),
                    HttpUtility.UrlEncode(windowsLiveLogin.AppLogoUrl));
            }
        }

        public string NonDelegatedSignUpUrl
        {
            get
            {
                string optinUrl = ConfigurationManager.AppSettings["wlm_signupurl"];
                if (String.IsNullOrEmpty(optinUrl))
                {
                    optinUrl = "http://settings.messenger.live.com/applications/websignup.aspx?returnurl={0}&privacyurl={1}";
                }

                if (windowsLiveLogin == null) { return string.Empty; }

                return string.Format(
                    optinUrl,
                    HttpUtility.UrlEncode(windowsLiveLogin.ReturnUrl),
                    HttpUtility.UrlEncode(windowsLiveLogin.PolicyUrl)
                   );
            }
        }

        #endregion

        #region WindowsLiveMessenger Public Methods

        

        public MessengerTicket CreateSignedTicket(string consentToken, params string[] cids)
        {
            if (String.IsNullOrEmpty(consentToken))
            {
                return null;
            }

            ConsentToken consent = this.DecodeToken(consentToken);
            if (consent == null)
            {
                return null;
            }

            ApplicationContactList list = new ApplicationContactList();
            string ticket = list.CreateApplicationContactList(
                consent.CID,
                cids);

            string signature = list.SignApplicationContactList(
                ticket,
                consent.SessionKey);

            return new MessengerTicket(ticket, signature);
        }

        public string CreateAppContactsTag(string consentToken, string[] cids)
        {
            MessengerTicket ticket = this.CreateSignedTicket(consentToken, cids);
            if (ticket == null)
            {
                return string.Empty;
            }

            return string.Format(
                "<msgr:app-contacts contacts=\'{0}\' signature=\'{1}\'></msgr:app-contacts>",
                HttpUtility.HtmlAttributeEncode(ticket.Ticket),
                ticket.Signature);
        }


        public ConsentToken HandleConsentResponse(NameValueCollection response)
        {
            return this.windowsLiveLogin.ProcessConsent(response);
        }

        public ConsentToken DecodeToken(string token)
        {
            return this.windowsLiveLogin.ProcessConsentToken(token);
        }

        public ConsentToken RefreshConsent(ConsentToken consent)
        {
            if (consent == null || consent.IsValid())
            {
                return consent;
            }

            consent = this.windowsLiveLogin.RefreshConsentToken(consent);

            return consent;
        }

        #endregion

        /// <summary>
        /// Signs the parameters using the session key.
        /// </summary>
        /// <param name="key">The session key.</param>
        /// <param name="inviteeName">The invitee's display name.</param>
        /// <param name="inviterId">The inviter's ID.</param>
        /// <param name="inviterName">The inviter's display name.</param>
        /// <returns>The signed parameters.</returns>
        public static string SignParameters(byte[] key, string inviteeName, string inviterId, string inviterName)
        {
            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();

            dict["wl_sig_source"] = inviterId;
            dict["wl_sig_source_display_name"] = inviterName;
            dict["wl_sig_target_display_name"] = inviteeName;

            ComputeSignature(key, dict);

            StringBuilder sb = new StringBuilder();

            bool first = true;

            foreach (KeyValuePair<string, string> pair in dict)
            {
                if (pair.Value.Length > 0)
                {
                    if (!first)
                    {
                        sb.Append("&");
                    }

                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0}={1}",
                        pair.Key,
                        pair.Value);

                    first = false;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Computes a signature for the provided parameters.
        /// </summary>
        /// <param name="key">The session key.</param>
        /// <param name="parameters">The parameters.</param>
        private static void ComputeSignature(byte[] key, SortedDictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> pair in parameters)
            {
                if (pair.Value.Length > 0)
                {
                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0}={1}",
                        pair.Key.Substring("wl_sig_".Length),
                        pair.Value);
                }
            }

            byte[] signature;

            using (HashAlgorithm algorithm = new HMACSHA256(key))
            {
                signature = algorithm.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            }

            StringBuilder hexSignature = new StringBuilder(signature.Length * 2);

            for (int i = 0; i < signature.Length; i++)
            {
                hexSignature.Append(signature[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            parameters.Add("wl_sig", hexSignature.ToString());
        }


    }
}
