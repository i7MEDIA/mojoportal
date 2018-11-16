// Author:		        
// Created:            2009-08-31
// Last Modified:      2018-10-31
//
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.IO;
using System.Web;
using System.Collections;

//using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Editor;
using Resources;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Returns styles in json format for CKeditor
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CKeditorStyles : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        private string comma = string.Empty;


        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {

                return;
            }
            RenderJsonList(context);

        }

        private void RenderJsonList(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            context.Response.Write("try{CKEDITOR.addStylesSet('mojo',[");

            if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates)
            {
                RenderSystemStyles(context);
            }

            using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
            {
                while (reader.Read())
                {
                    context.Response.Write(comma);
                    context.Response.Write("{\"name\":\"" + reader["Name"].ToString().JsonEscape() + "\"");
                    context.Response.Write(",\"element\":\"" + reader["Element"].ToString().JsonEscape() + "\"");
                    context.Response.Write(",\"attributes\":{\"class\":\"" + reader["CssClass"].ToString().JsonEscape() + "\"}");
                    context.Response.Write("}");

                    comma = ",";

                }

            }


            if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates)
            {
                RenderSystemStyles(context);
            }

            context.Response.Write("]);\r\n}catch(err){}");

        }

        private void RenderSystemStyles(HttpContext context)
        {
            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Image on Right\"");
            context.Response.Write(",\"element\":\"img\"");
            context.Response.Write(",\"attributes\":{\"class\":\"image-right\"}");
            context.Response.Write("}");

            comma = ",";

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Image on Left\"");
            context.Response.Write(",\"element\":\"img\"");
            context.Response.Write(",\"attributes\":{\"class\":\"image-left\"}");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Document Block aka div\"");
            context.Response.Write(",\"element\":\"div\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Preformatted Text\"");
            context.Response.Write(",\"element\":\"pre\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Address\"");
            context.Response.Write(",\"element\":\"address\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Inline Quotation\"");
            context.Response.Write(",\"element\":\"q\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Cited Work\"");
            context.Response.Write(",\"element\":\"c\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Emphasis\"");
            context.Response.Write(",\"element\":\"em\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Code\"");
            context.Response.Write(",\"element\":\"code\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Keyboard Input\"");
            context.Response.Write(",\"element\":\"kbd\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Sample Text\"");
            context.Response.Write(",\"element\":\"samp\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Term Definition\"");
            context.Response.Write(",\"element\":\"dfn\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Variable\"");
            context.Response.Write(",\"element\":\"var\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Deleted Text\"");
            context.Response.Write(",\"element\":\"del\"");
            context.Response.Write("}");

            context.Response.Write(comma);
            context.Response.Write("{\"name\":\"Inserted Text\"");
            context.Response.Write(",\"element\":\"ins\"");
            context.Response.Write("}");

           

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
