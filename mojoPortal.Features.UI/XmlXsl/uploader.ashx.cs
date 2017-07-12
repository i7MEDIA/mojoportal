//  Author:                     
//  Created:                    2013-04-05
//	Last Modified:              2013-04-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Web.XmlUI;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace mojoPortal.Features.UI.XmlXsl
{
    /// <summary>
    /// handles file uploads for the xml/xsl feature, called from jQueryFileUpload
    /// </summary>
    public class uploader : BaseContentUploadHandler, IHttpHandler
    {
        private Module module = null;
        private XmlConfiguration config = new XmlConfiguration();

        public void ProcessRequest(HttpContext context)
        {
            base.Initialize(context);

            if (!UserCanEditModule(ModuleId, XmlConfiguration.FeatureGuid))
            {
                log.Info("User has no edit permission so returning 404");
                Response.StatusCode = 404;
                return;
            }

            if (CurrentSite == null)
            {
                log.Info("CurrentSite is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            if (CurrentUser == null)
            {
                log.Info("CurrentUser is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            // this feature only uses the actual system.io file system
            //if (FileSystem == null)
            //{
            //    log.Info("FileSystem is null so returning 404");
            //    Response.StatusCode = 404;
            //    return;
            //}

            if (Request.Files.Count == 0)
            {
                log.Info("Posted File Count is zero so returning 404");
                Response.StatusCode = 404;
                return;
            }

            module = GetModule(ModuleId, XmlConfiguration.FeatureGuid);

            if (module == null)
            {
                log.Info("Module is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);
            config = new XmlConfiguration(moduleSettings);

            HttpPostedFile file = Request.Files[0]; // only expecting one file per post

            string newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!SiteUtils.IsAllowedUploadBrowseFile(ext, ".xml|.xsl"))
            {
                log.Info("file extension was " + ext + " so returning 404");
                Response.StatusCode = 404;

                return;
            }

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            

            string destPath;

            switch (ext)
            {
                case ".xml":
                    string xmlBasePath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString() + "/xml/";
                    destPath = Server.MapPath(xmlBasePath + newFileName);

                    if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                        }

                    file.SaveAs(destPath);

                    break;

                case ".xsl":
                    string xslBasePath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString() + "/xsl/";
                    destPath = Server.MapPath(xslBasePath + newFileName);

                    if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                        }

                    file.SaveAs(destPath);

                    break;
            }

            r.Add(new UploadFilesResult()
            {
                //Thumbnail_url = 
                Name = newFileName,
                Length = file.ContentLength,
                Type = file.ContentType
            });

            var uploadedFiles = new
            {
                files = r.ToArray()
            };

            var jsonObj = js.Serialize(uploadedFiles);
            context.Response.Write(jsonObj.ToString());


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