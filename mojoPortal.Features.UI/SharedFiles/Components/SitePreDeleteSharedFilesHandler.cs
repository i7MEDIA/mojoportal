// Author:                     
// Created:                    2008-11-12
//	Last Modified:              2013-02-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.SharedFilesUI;
using System;

namespace mojoPortal.Features
{
    
    public class SitePreDeleteSharedFilesHandler : SitePreDeleteHandlerProvider
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(SitePreDeleteSharedFilesHandler));

        public SitePreDeleteSharedFilesHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            SharedFile.DeleteBySite(siteId);

            if (SharedFilesConfiguration.DeleteFilesOnSiteDelete)
            {
                try
                {
                    string fileVirtualBasePath = "~/Data/Sites/" + siteId.ToInvariantString() + "/SharedFiles/";

                    FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                    if (p != null)
                    {
                        IFileSystem fileSystem = p.GetFileSystem();
                        if (fileSystem != null)
                        {
                            fileSystem.DeleteFolder(fileVirtualBasePath);
                        }
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }

        }
    }
}
