// Author:					
// Created:				    2010-02-16
// Last Modified:		    2010-02-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Handles posts from the content rating control, returns the current avg rating for the content
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ContentRatingService : IHttpHandler
    {
        private int rating = -1;
        private Guid contentGuid = Guid.Empty;
        private string comments = string.Empty;
        private string emailAddress = string.Empty;
        private bool isValidPost = false;
        private const int minRating = 0;
        private const int maxRating = 5;
        private int avgRating = 0;
        private int totalVotes = 0;

        public void ProcessRequest(HttpContext context)
        {
            LoadParams(context);
            if (isValidPost) { DoRating(context); }

            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            context.Response.Write("{");
           
            context.Response.Write("\"avg\":" + avgRating.ToInvariantString());
            context.Response.Write(",\"votes\":\"" + string.Format(CultureInfo.InvariantCulture, Resource.RatingCountFormat, totalVotes) + "\"");
            

            context.Response.Write("}");

        }

        private void DoRating(HttpContext context)
        {
            
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            Guid userGuid = Guid.Empty;
           
            if (context.Request.IsAuthenticated)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    userGuid = currentUser.UserGuid;
                    if (emailAddress.Length == 0) { emailAddress = currentUser.Email; }
                }
            }

            ContentRating.RateContent(
                siteSettings.SiteGuid,
                contentGuid,
                userGuid,
                rating,
                emailAddress,
                comments,
                SiteUtils.GetIP4Address(),
                WebConfigSettings.MinutesBetweenAnonymousRatings
                );

            ContentRatingStats ratingStats = ContentRating.GetStats(contentGuid);

            avgRating = ratingStats.AverageRating;
            totalVotes = ratingStats.TotalVotes;
            

        }


        private void LoadParams(HttpContext context)
        {
            // don't accept get requests as ratings
            if (context.Request.HttpMethod != "POST") { return; }

            string requestBody = context.Request.GetRequestBody(); 

            NameValueCollection postParams = HttpUtility.ParseQueryString(requestBody);

            if (postParams.Get("cid") != null)
            {
                string cg = postParams.Get("cid");
                if (cg.Length == 36)
                {
                    try
                    {
                        contentGuid = new Guid(cg);
                    }
                    catch (FormatException) { }
                }
            }

            if (postParams.Get("r") != null)
            {
                int.TryParse(postParams.Get("r"), out rating);
            }

            if (postParams.Get("comments") != null)
            {
                comments = postParams.Get("comments");
            }

            if (postParams.Get("email") != null)
            {
                emailAddress = postParams.Get("email");
            }

            if ((contentGuid != Guid.Empty) && (rating >= minRating) && (rating <= maxRating)) { isValidPost = true; }


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
