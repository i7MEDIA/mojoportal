// Author:					
// Created:				    2011-03-23
// Last Modified:			2011-03-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI
{
    public class FolderGalleryContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            string basePath = "~/Data/Sites/" + module.SiteId.ToInvariantString() + "/media/FolderGalleries/";

            if(!Directory.Exists(HostingEnvironment.MapPath(basePath)))
            {
                Directory.CreateDirectory(HostingEnvironment.MapPath(basePath));
            }

            IOHelper.CopyFolderContents(HostingEnvironment.MapPath(configInfo), HostingEnvironment.MapPath(basePath));

        }
    }
}