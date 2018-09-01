//  Author:                     
//  Created:                    2013-04-01
//	Last Modified:              2013-04-02
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
using mojoPortal.Web.GalleryUI;
using mojoPortal.Web.UI;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace mojoPortal.Features.UI.FolderGallery
{
    /// <summary>
    /// Summary description for upload
    /// </summary>
    public class upload : BaseContentUploadHandler, IHttpHandler
    {
        private Module module = null;
        private FolderGalleryConfiguration config = new FolderGalleryConfiguration();

        public void ProcessRequest(HttpContext context)
        {
            base.Initialize(context);

            if (!UserCanEditModule(ModuleId, FolderGalleryConfiguration.FeatureGuid))
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

            if (Request.Files.Count > FolderGalleryConfiguration.MaxFilesToUploadAtOnce)
            {
                log.Info("Posted File Count is higher than configured limit so returning 404");
                Response.StatusCode = 404;
                return;
            }

            module = GetModule(ModuleId, FolderGalleryConfiguration.FeatureGuid);

            if (module == null)
            {
                log.Info("Module is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);
            config = new FolderGalleryConfiguration(moduleSettings);

            string imageFolderPath;
            if (config.GalleryRootFolder.Length > 0)
            {
                imageFolderPath = config.GalleryRootFolder;
            }
            else
            {
                imageFolderPath = string.Format(CultureInfo.InvariantCulture,
                    FolderGalleryConfiguration.BasePathFormat,
                    CurrentSite.SiteId);
            }

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            try
            {
                for (int f = 0; f < Request.Files.Count; f++)
                {
                    HttpPostedFile file = Request.Files[f];

                    string ext = Path.GetExtension(file.FileName);
                    string newFileName = Path.GetFileName(file.FileName);
                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                    {
                        string newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                        if(File.Exists(Server.MapPath(newImagePath)))
                        {
                            File.Delete(Server.MapPath(newImagePath));
                        }

                        file.SaveAs(Server.MapPath(newImagePath));

                        //using (Stream s = file.InputStream)
                        //{
                        //    //FileSystem.SaveFile(newImagePath, s, file.ContentType, true);
                        
                        //}


                        r.Add(new UploadFilesResult()
                        {
                            //Thumbnail_url = 
                            Name = newFileName,
                            Length = file.ContentLength,
                            Type = file.ContentType
                        });

                    }

                }

                
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

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