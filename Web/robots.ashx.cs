// Author:					
// Created:					2009-06-06
// Last Modified:			2009-06-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.IO;
using System.Web;
using System.Collections;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web
{
    /// <summary>
    /// If you route/rewrite requests for robots.txt to robots.ashx then you can use a different robots file for https
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RobotsHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            RenderRobots(context);
        }

        private void RenderRobots(HttpContext context)
        {
            string robotsFile;
            if (SiteUtils.IsSecureRequest())
            {
                robotsFile = context.Server.MapPath(WebConfigSettings.RobotsSslConfigFile); //robots.ssl.config by default
            }
            else
            {
                robotsFile = context.Server.MapPath(WebConfigSettings.RobotsConfigFile); //robots.config by default
            }

            if (!File.Exists(robotsFile)) return;

            context.Response.ContentType = "text/plain";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;
            FileInfo file = new FileInfo(robotsFile);

            using (StreamReader sr = file.OpenText())
            {
                context.Response.Write(sr.ReadToEnd());
            }

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
