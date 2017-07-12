//  Author:                     
//  Created:                    2009-06-21
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
using System.Data;
using System.Web;

namespace mojoPortal.Features
{
    public class SharedFilesContentDeleteHandler : ContentDeleteHandlerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharedFilesContentDeleteHandler));

        public SharedFilesContentDeleteHandler()
        { }

        public override void DeleteContent(int moduleId, Guid moduleGuid)
        {
            if (SharedFilesConfiguration.DeleteFilesOnModuleDelete)
            {
                try
                {
                    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                    if (siteSettings != null)
                    {

                        string fileVirtualBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/SharedFiles/";

                        FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                        if (p != null)
                        {
                            IFileSystem fileSystem = p.GetFileSystem();
                            if (fileSystem != null)
                            {

                                using (IDataReader reader = SharedFile.GetHistoryByModule(moduleId))
                                {
                                    while (reader.Read())
                                    {
                                        string fileName = reader["ServerFileName"].ToString();
                                        fileSystem.DeleteFile(VirtualPathUtility.Combine(fileVirtualBasePath + "History/", fileName));
                                
                                    }
                                }

                                using (IDataReader reader = SharedFile.GetSharedFiles(moduleId))
                                {
                                    while (reader.Read())
                                    {
                                        string fileName = reader["ServerFileName"].ToString();
                                        fileSystem.DeleteFile(VirtualPathUtility.Combine(fileVirtualBasePath, fileName));

                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }

            SharedFile.DeleteByModule(moduleId);
            
        }
    }
}
