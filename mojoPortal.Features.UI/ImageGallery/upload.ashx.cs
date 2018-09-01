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
using mojoPortal.Web.Framework;
using mojoPortal.Web.GalleryUI;
using mojoPortal.Web.UI;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace mojoPortal.Features.UI.ImageGallery
{
    /// <summary>
    /// Handles file uploads for the Image Gallery feature, called from jQueryFileUpload.cs
    /// </summary>
    public class upload : BaseContentUploadHandler, IHttpHandler
    {
        private int itemId = -1;
        private Module module = null;
        private GalleryConfiguration config = new GalleryConfiguration();

        public void ProcessRequest(HttpContext context)
        {
            base.Initialize(context);

            if (!UserCanEditModule(ModuleId, Gallery.FeatureGuid))
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

            if (Request.Files.Count > GalleryConfiguration.MaxFilesToUploadAtOnce)
            {
                log.Info("Posted File Count is higher than allowed so returning 404");
                Response.StatusCode = 404;
                return;
            }

            module = GetModule(ModuleId, Gallery.FeatureGuid);

            if (module == null)
            {
                log.Info("Module is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);

            //if (Request.Form.Count > 0)
            //{
            //    string submittedContent = Server.UrlDecode(Request.Form.ToString()); // this gets the full content of the post
            //    log.Info("submitted data: " + submittedContent);
            //}


            Hashtable moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);
            config = new GalleryConfiguration(moduleSettings);

            string imageFolderPath;
            string fullSizeImageFolderPath;
            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                imageFolderPath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString() + "/media/GalleryImages/" + ModuleId.ToInvariantString() + "/";
            }
            else
            {
                imageFolderPath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString() + "/GalleryImages/" + ModuleId.ToInvariantString() + "/";
            }

            fullSizeImageFolderPath = imageFolderPath + "FullSizeImages/";
            string thumbnailPath = imageFolderPath + "Thumbnails/";

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            for (int f = 0; f < Request.Files.Count; f++)
            {
                HttpPostedFile file = Request.Files[f];

                string ext = Path.GetExtension(file.FileName);
                if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                {
                    GalleryImage galleryImage;

                    if((itemId > -1)&&(Request.Files.Count == 1))
                    {
                        galleryImage = new GalleryImage(ModuleId, itemId);
                    }
                    else
                    {
                        galleryImage = new GalleryImage(ModuleId);
                    }

                    galleryImage.ModuleGuid = module.ModuleGuid;
                    galleryImage.WebImageHeight = config.WebSizeHeight;
                    galleryImage.WebImageWidth = config.WebSizeWidth;
                    galleryImage.ThumbNailHeight = config.ThumbnailHeight;
                    galleryImage.ThumbNailWidth = config.ThumbnailWidth;
                    galleryImage.UploadUser = CurrentUser.Name;

                    galleryImage.UserGuid = CurrentUser.UserGuid;

                    string newFileName = Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                    string newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

                    if (galleryImage.ImageFile == newFileName)
                    {
                        // an existing gallery image delete the old one
                        FileSystem.DeleteFile(newImagePath);
                    }
                    else
                    {
                        // this is a new galleryImage instance, make sure we don't use the same file name as any other instance
                        int i = 1;
                        while (FileSystem.FileExists(VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName)))
                        {
                            newFileName = i.ToInvariantString() + newFileName;
                            i += 1;
                        }

                    }

                    newImagePath = VirtualPathUtility.Combine(fullSizeImageFolderPath, newFileName);

                   
                    using (Stream s = file.InputStream)
                    {
                        FileSystem.SaveFile(newImagePath, s, file.ContentType, true);
                    }
                    

                    galleryImage.ImageFile = newFileName;
                    galleryImage.WebImageFile = newFileName;
                    galleryImage.ThumbnailFile = newFileName;
                    galleryImage.Save();
                    GalleryHelper.ProcessImage(galleryImage, FileSystem, imageFolderPath, file.FileName, config.ResizeBackgroundColor);

                    r.Add(new UploadFilesResult()
                    {
                        Thumbnail_url = WebUtils.ResolveServerUrl(thumbnailPath + newFileName),
                        Name = newFileName,
                        Length = file.ContentLength,
                        Type = file.ContentType,
                        ReturnValue = galleryImage.ItemId.ToInvariantString()
                    });

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