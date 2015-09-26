//  Author:                     Joe Audette
//  Created:                    2013-04-09
//	Last Modified:              2013-04-09
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
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using WebStore.Business;

namespace WebStore.UI
{
    /// <summary>
    /// Summary description for upload
    /// </summary>
    public class upload : BaseContentUploadHandler, IHttpHandler
    {
        private Guid productGuid = Guid.Empty;
        private Module module = null;
        private WebStoreConfiguration config = new WebStoreConfiguration();

        public void ProcessRequest(HttpContext context)
        {
            base.Initialize(context);

            if (!UserCanEditModule(ModuleId, Store.FeatureGuid))
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

            productGuid = WebUtils.ParseGuidFromQueryString("prod", productGuid);

            if (productGuid == Guid.Empty)
            {
                log.Info("No productGuid provided so returning 404");
                Response.StatusCode = 404;
                return;
            }

            module = GetModule(ModuleId, Store.FeatureGuid);

            if (module == null)
            {
                log.Info("Module is null so returning 404");
                Response.StatusCode = 404;
                return;
            }

            string type = "product";
            if (Request.Params["type"] != null)
            {
                type = Request.Params["type"];
            }

            HttpPostedFile file = Request.Files[0];  // only expecting 1 file

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            if (type == "product")
            {
                //product file
                string upLoadPath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString()
                + "/webstoreproductfiles/";

               ProductFile productFile = new ProductFile(productGuid);
                productFile.FileName = Path.GetFileName(file.FileName);
                productFile.ByteLength = file.ContentLength;
                productFile.Created = DateTime.UtcNow;
                productFile.CreatedBy = CurrentUser.UserGuid;
                productFile.ServerFileName = productGuid.ToString() + ".config";

                string ext = System.IO.Path.GetExtension(file.FileName);
                string mimeType = IOHelper.GetMimeType(ext).ToLower();

                if (productFile.Save())
                {
                    string destPath = upLoadPath + productFile.ServerFileName;
                    FileSystem.DeleteFile(destPath);

                    using (Stream s = file.InputStream)
                    {
                        FileSystem.SaveFile(destPath, s, mimeType, true);
                    }

                    r.Add(new UploadFilesResult()
                    {
                        //Thumbnail_url = 
                        Name = file.FileName,
                        Length = file.ContentLength,
                        Type = mimeType
                    });
                }

            }
            else
            {
                // teaser file
                string upLoadPath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString()
                   + "/webstoreproductpreviewfiles/";

                Product product = new Product(productGuid);
                product.TeaserFile = Path.GetFileName(file.FileName).ToCleanFileName();

                string ext = System.IO.Path.GetExtension(file.FileName);
                string mimeType = IOHelper.GetMimeType(ext).ToLower();

                if (product.Save())
                {
                    string destPath = upLoadPath + product.TeaserFile;
                    FileSystem.DeleteFile(destPath);

                    using (Stream s = file.InputStream)
                    {
                        FileSystem.SaveFile(destPath, s, mimeType, true);
                    }

                    r.Add(new UploadFilesResult()
                    {
                        //Thumbnail_url = 
                        Name = file.FileName,
                        Length = file.ContentLength,
                        Type = mimeType
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