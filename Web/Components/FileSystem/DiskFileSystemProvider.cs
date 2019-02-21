// Author:					
// Created:				    2009-12-30
// Last Modified:			2011-11-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using log4net;

namespace mojoPortal.FileSystem
{
    public class DiskFileSystemProvider : FileSystemProvider
    {
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private const long bytesPerMegabyte = 1048576;
		private ILog log = LogManager.GetLogger(typeof(DiskFileSystemProvider));
        
        public override IFileSystem GetFileSystem()
        {
			siteSettings = CacheHelper.GetCurrentSiteSettings();
            IFileSystemPermission p = GetFileSystemPermission();
            if (p == null) { return null; }
            if(string.IsNullOrEmpty(p.VirtualRoot)) { return null; }

            return DiskFileSystem.GetFileSystem(p);
        }

		public override IFileSystem GetFileSystem(int siteId)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings == null)
			{
				siteSettings = new SiteSettings(siteId);
			}
			return GetFileSystem();
		}

		public override IFileSystem GetFileSystem(IFileSystemPermission permission)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            //IFileSystemPermission p = GetFileSystemPermission();
            if (permission == null) { return null; }
            if (string.IsNullOrEmpty(permission.VirtualRoot)) { return null; }

            return DiskFileSystem.GetFileSystem(permission);
        }

        private IFileSystemPermission GetFileSystemPermission()
        {
           
            return new FileSystemPermission()
            {
                UserHasUploadPermission = UserHasUploadPermission(),
				UserHasBrowsePermission = UserHasBrowsePermission(),
                VirtualRoot = GetVirtualPath(),
                Quota = GetQuota(),
                MaxSizePerFile = GetMaxSizePerFile(),
                MaxFiles = GetMaxFiles(),
                MaxFolders = GetMaxFolders(),
                AllowedExtensions = GetAllowedExtensions()
            };
        }

        
        private string GetVirtualPath()
        {
            string virtualPath = string.Empty;
           
            if (siteSettings == null)
            {
				log.Error("Cannot load file system because Site Settings could not be loaded.");
                throw new ArgumentNullException("could not load SiteSettings");
            }

            int siteId = siteSettings.SiteId;

            if (WebConfigSettings.UseRelatedSiteMode && WebConfigSettings.UseSameContentFolderForRelatedSiteMode)
            {
                siteId = WebConfigSettings.RelatedSiteID;
            }

            virtualPath = "~/Data/Sites/" + siteId.ToInvariantString() + "/media/";

            if ((WebUser.IsAdminOrContentAdmin)||(SiteUtils.UserIsSiteEditor()))
            {
                if (WebConfigSettings.ForceAdminsToUseMediaFolder)
                {
                    virtualPath = "~/Data/Sites/" + siteId.ToInvariantString() + "/media/";

                }
                else
                {
                    if ((siteSettings.IsServerAdminSite)&&(WebConfigSettings.AllowAdminsToUseDataFolder) && (WebUser.IsAdmin))
                    {
                        virtualPath = "~/Data/";
                    }
                    else
                    {
                        virtualPath = "~/Data/Sites/" + siteId.ToInvariantString() + "/";

                    }
                }
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                virtualPath = "~/Data/Sites/" + siteId.ToInvariantString() + "/media/";

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null)
                {
					log.Error("Cannot load file system because Site User could not be loaded.");
					throw new ArgumentNullException("could not load current SiteUser");
                }

                virtualPath = "~/Data/Sites/" + siteId.ToInvariantString() + "/userfiles/" + currentUser.UserId.ToInvariantString() + "/";

            }


            return virtualPath;

        }

        private bool UserHasUploadPermission()
        {
            bool result = false;
			if (siteSettings == null) return false;

            if ((WebUser.IsAdminOrContentAdmin)||SiteUtils.UserIsSiteEditor())
            {
                result = true;
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
               result = true;
            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                result = true;
            }

            return result;

        }

		private bool UserHasBrowsePermission()
		{
			if (siteSettings == null) return false;
			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor() || WebUser.IsInRoles(siteSettings.GeneralBrowseRoles))
			{
				return true;
			}

			return false;
		}

        private int GetMaxFiles()
        {
            if ((WebUser.IsAdminOrContentAdmin) || SiteUtils.UserIsSiteEditor())
            {
                return WebConfigSettings.AdminMaxNumberOfFiles;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderMaxNumberOfFiles;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderMaxNumberOfFiles;
            }

            return 0;
        }

        private int GetMaxFolders()
        {
            if ((WebUser.IsAdminOrContentAdmin) || SiteUtils.UserIsSiteEditor())
            {
                return WebConfigSettings.AdminMaxNumberOfFolders;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderMaxNumberOfFolders;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderMaxNumberOfFolders;
            }

            return 0;
        }

        private long GetMaxSizePerFile()
        {
            if ((WebUser.IsAdminOrContentAdmin) || SiteUtils.UserIsSiteEditor())
            {
                return WebConfigSettings.AdminMaxSizePerFileInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderMaxSizePerFileInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderMaxSizePerFileInMegaBytes * bytesPerMegabyte;
            }

            return 0;
        }

        private long GetQuota()
        {
            if ((WebUser.IsAdminOrContentAdmin) || SiteUtils.UserIsSiteEditor())
            {
                return WebConfigSettings.AdminDiskQuotaInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.MediaFolderDiskQuotaInMegaBytes * bytesPerMegabyte;
            }

            if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.UserFolderDiskQuotaInMegaBytes * bytesPerMegabyte;
            }

            return 0;
        }

        private IEnumerable<string> GetAllowedExtensions()
        {

            if ((WebUser.IsAdminOrContentAdmin) || SiteUtils.UserIsSiteEditor())
            {
                return WebConfigSettings.AllowedUploadFileExtensions.SplitOnChar('|');
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                return WebConfigSettings.AllowedUploadFileExtensions.SplitOnChar('|');

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                return WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions.SplitOnChar('|');

            }

            return new string[0];
        }

    }
}
