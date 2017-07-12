// Author:					
// Created:					2009-10-21
// Last Modified:			2010-01-29
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Text;
using System.Web.Services;
using mojoPortal.Web.Framework;
using mojoPortal.Business;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TaskProgress : IHttpHandler
    {

        private Guid taskGuid = Guid.Empty;
        private TaskQueue task = null;
        //private string taskGuidString = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            GetParams(context);
            RenderJson(context);
        }

        private void RenderJson(HttpContext context)
        {

            context.Response.Expires = -1;
            context.Response.ContentType = "application/json";
            //context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            if (task != null)
            {
                int progress = Convert.ToInt32(task.CompleteRatio * 100);
                if (progress > 100) { progress = 100; }
                if (progress < 0) { progress = 0; }
                context.Response.Write("{");
                context.Response.Write("\"status\":\"" + task.Status + "\"");
                context.Response.Write(",\"percentComplete\":" + progress.ToInvariantString());
                context.Response.Write("}");

            }
            else
            {
                context.Response.Write("{");
                context.Response.Write("\"status\":\"\"");
                context.Response.Write(",\"percentComplete\": 100");
                context.Response.Write("}");
            }


        }

        private void GetParams(HttpContext context)
        {
            taskGuid = WebUtils.ParseGuidFromQueryString("t", taskGuid);
            task = new TaskQueue(taskGuid);
            if (task.Guid == Guid.Empty) { task = null; }

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
