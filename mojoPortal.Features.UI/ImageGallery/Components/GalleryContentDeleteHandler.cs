//  Author:                     
//  Created:                    2009-06-21
//	Last Modified:              2012-07-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Web.Hosting;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.GalleryUI;

namespace mojoPortal.Features
{
    public class GalleryContentDeleteHandler : ContentDeleteHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GalleryContentDeleteHandler));

        public GalleryContentDeleteHandler()
        { }

        public override void DeleteContent(int moduleId, Guid moduleGuid)
        {

            
            if (GalleryConfiguration.DeleteImagesWhenModuleIsDeleted)
            {
                Module m = new Module(moduleId);
                DeleteImages(m.SiteId, moduleId);
            }

            Gallery.DeleteByModule(moduleId);
            
        }

        private void DeleteImages(int siteId, int moduleId)
        {
            try
            {
                string virtualPath = "~/Data/Sites/" + siteId.ToInvariantString()
                    + "/media/GalleryImages/" + moduleId.ToInvariantString();

                string fileSystemPath = HostingEnvironment.MapPath(virtualPath);

                if (Directory.Exists(fileSystemPath))
                {
                    DirectoryInfo d = new DirectoryInfo(fileSystemPath);
                    //d.Empty();
                    d.Delete(true);

                    //Directory.Delete(fileSystemPath, true);
                }


            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }


    }
}
