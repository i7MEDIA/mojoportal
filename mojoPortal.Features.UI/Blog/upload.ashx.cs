//  Author:                     
//  Created:                    2013-04-05
//	Last Modified:              2013-04-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Web.BlogUI;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using Resources;

namespace mojoPortal.Web.BlogUI
{
    /// <summary>
    /// Summary description for upload
    /// </summary>
    public class upload : BaseContentUploadHandler, IHttpHandler
    {
        private int itemId = -1;
        private Module module = null;
        private BlogConfiguration config = new BlogConfiguration();
        private Blog blog = null;

        public void ProcessRequest(HttpContext context)
        {
            base.Initialize(context);

            if (!UserCanEditModule(ModuleId, Blog.FeatureGuid))
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

            if (FileSystem == null)
            {
                log.Info("FileSystem is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            if (Request.Files.Count == 0)
            {
                log.Info("Posted File Count is zero so returning 404");
                Response.StatusCode = 404;
                return;
            }

            if (Request.Files.Count > BlogConfiguration.MaxAttachmentsToUploadAtOnce)
            {
                log.Info("Posted File Count is higher than allowed so returning 404");
                Response.StatusCode = 404;
                return;
            }

            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);

            if (itemId == -1)
            {
                log.Info("No ItemID provided so returning 404");
                Response.StatusCode = 404;
                return;
            }

            module = GetModule(ModuleId, Blog.FeatureGuid);

            if (module == null)
            {
                log.Info("Module is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            blog = new Blog(itemId);
            if (blog.ModuleId != ModuleId)
            {
                log.Info("Invalid ItemID for module so returning 404");
                Response.StatusCode = 404;
                return;
            }

            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);
            config = new BlogConfiguration(moduleSettings);

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            SiteUtils.EnsureFileAttachmentFolder(CurrentSite);
            string upLoadPath = SiteUtils.GetFileAttachmentUploadPath();

            for (int f = 0; f < Request.Files.Count; f++)
            {
                HttpPostedFile file = Request.Files[f]; 

                string ext = System.IO.Path.GetExtension(file.FileName);

                if (!SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.AllowedMediaFileExtensions))
                {
                    log.Info("file extension was " + ext + " so discarding file " + file.FileName);

                    r.Add(new UploadFilesResult()
                    {
                        Name = file.FileName,
                        Length = file.ContentLength,
                        Type = file.ContentType,
                        ErrorMessage = string.Format(
                            CultureInfo.InvariantCulture,
                            GalleryResources.InvalidUploadExtensionFormat,
                            file.FileName,
                            WebConfigSettings.AllowedMediaFileExtensions.Replace("|", " "))

                    });

                    continue;
                }

                string mimeType = IOHelper.GetMimeType(ext).ToLower();

                FileAttachment a = new FileAttachment();
                a.CreatedBy = CurrentUser.UserGuid;
                a.FileName = System.IO.Path.GetFileName(file.FileName);
                a.ServerFileName = blog.ItemId.ToInvariantString() + a.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                a.ModuleGuid = blog.ModuleGuid;
                a.SiteGuid = CurrentSite.SiteGuid;
                a.ItemGuid = blog.BlogGuid;
                a.ContentLength = file.ContentLength;
                a.ContentType = mimeType;

                a.Save();

                string destPath = upLoadPath + a.ServerFileName;
                
                using (Stream s = file.InputStream)
                {
                    FileSystem.SaveFile(destPath, s, mimeType, true);
                }

                r.Add(new UploadFilesResult()
                {
                    //Thumbnail_url = 
                    Name = a.FileName,
                    Length = file.ContentLength,
                    Type = mimeType
                });

                if (WebConfigSettings.LogAllFileServiceRequests)
                {
                    string userName = "unauthenticated user";
                    if (CurrentUser != null)
                    {
                        userName = CurrentUser.Name;
                    }
                    log.Info("File " + file.FileName + " uploaded by " + userName + " as a media attachment in the Blog");

                }

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