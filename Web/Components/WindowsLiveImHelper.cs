
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace mojoPortal.Web
{
    /// <summary>
    /// Exposes functionality for the Windows Live Messenger IM Control.
    /// </summary>
    public static class MessengerIMControl
    {
        /// <summary>
        /// The URL format for the IM Control.
        /// </summary>
        private const string UrlFormat = "http://settings.messenger.live.com/conversation/imme.aspx?invitee=@C@{0}&mkt={1}&dt={2}";

        /// <summary>
        /// Gets an IM Control URL.
        /// </summary>
        /// <param name="cid">The invitee's CID.</param>
        /// <param name="delegationToken">The invitee's delegation token.</param>
        /// <returns>An IM Control URL.</returns>
        public static string GetUrl(
            long cid,
            string delegationToken)
        {
            return GetUrl(cid, delegationToken, "en-US");
        }

        /// <summary>
        /// Gets an IM Control URL.
        /// </summary>
        /// <param name="cid">The invitee's CID.</param>
        /// <param name="delegationToken">The invitee's delegation token.</param>
        /// <param name="market">The invitee's market.</param>
        /// <returns>An IM Control URL.</returns>
        public static string GetUrl(
            long cid,
            string delegationToken,
            string market)
        {
            string url = String.Format(
                UrlFormat,
                HttpUtility.UrlEncode(cid.ToString(CultureInfo.InvariantCulture)),
                HttpUtility.UrlEncode(market),
                HttpUtility.UrlEncode(delegationToken));

            return url;
        }

        /// <summary>
        /// Gets an IM Control URL.
        /// </summary>
        /// <param name="cid">The invitee's CID.</param>
        /// <param name="delegationToken">The invitee's delegation token.</param>
        /// <param name="market">The user's market.</param>
        /// <param name="sessionKey">The session key found in the consent token.</param>
        /// <param name="inviteeName">The invitee's display name.</param>
        /// <param name="inviterId">The inviter's ID.</param>
        /// <param name="inviterName">The inviter's display name.</param>
        /// <returns></returns>
        public static string GetUrl(
            long cid,
            string delegationToken,
            string market,
            byte[] sessionKey,
            string inviteeName,
            string inviterId,
            string inviterName)
        {
            string signedParams = SignParameters(
                sessionKey,
                inviteeName,
                inviterId,
                inviterName);

            string url = String.Format(
                UrlFormat,
                HttpUtility.UrlEncode(cid.ToString(CultureInfo.InvariantCulture)),
                HttpUtility.UrlEncode(market),
                HttpUtility.UrlEncode(delegationToken));

            return url + "&sparams=" + Uri.EscapeDataString(signedParams);
        }

        /// <summary>
        /// Signs the parameters using the session key.
        /// </summary>
        /// <param name="key">The session key.</param>
        /// <param name="inviteeName">The invitee's display name.</param>
        /// <param name="inviterId">The inviter's ID.</param>
        /// <param name="inviterName">The inviter's display name.</param>
        /// <returns>The signed parameters.</returns>
        private static string SignParameters(byte[] key, string inviteeName, string inviterId, string inviterName)
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
                sb.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}={1}",
                    pair.Key.Substring("wl_sig_".Length),
                    pair.Value);
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
