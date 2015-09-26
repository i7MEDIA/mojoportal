using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mojoPortal.Web
{
    /// <summary>
    /// Summary description for channel
    /// The channel file addresses some issues with cross domain communication in certain browsers. 
    /// http://developers.facebook.com/docs/reference/javascript/
    /// </summary>
    public class channel : IHttpHandler
    {
        private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(WebConfigSettings.ChannelFileCacheInDays);

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
            context.Response.Cache.SetMaxAge(CACHE_DURATION);

            context.Response.Write("<script src=\"//connect.facebook.net/en_US/all.js\"></script>");
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