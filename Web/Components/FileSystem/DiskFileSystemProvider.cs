using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;

namespace mojoPortal.FileSystem
{
	public class DiskFileSystemProvider : FileSystemProvider
	{
		private const long bytesPerMegabyte = 1048576;
		private ILog log = LogManager.GetLogger(typeof(DiskFileSystemProvider));


		public override IFileSystem GetFileSystem()
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			IFileSystemPermission p = GetFileSystemPermission();

			if (p == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(p.VirtualRoot))
			{
				return null;
			}

			return DiskFileSystem.GetFileSystem(p);
		}


		public override IFileSystem GetFileSystem(int siteId)
		{
			var siteSettings =  new SiteSettings(siteId);
			if (siteSettings == null)
			{
				log.Error($"Site Settings is NULL!!!! Passed SiteId={siteId} from ");
			}
			IFileSystemPermission p = GetFileSystemPermission(siteId);

			if (p == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(p.VirtualRoot))
			{
				return null;
			}

			return DiskFileSystem.GetFileSystem(p);
		}

		public override IFileSystem GetFileSystem(IFileSystemPermission permission)
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (permission == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(permission.VirtualRoot))
			{
				return null;
			}

			return DiskFileSystem.GetFileSystem(permission);
		}


		private IFileSystemPermission GetFileSystemPermission(int siteId = -1)
		{
			return new FileSystemPermission()
			{
				UserHasUploadPermission = UserHasUploadPermission(),
				UserHasBrowsePermission = UserHasBrowsePermission(),
				VirtualRoot = GetVirtualPath(siteId),
				Quota = GetQuota(),
				MaxSizePerFile = GetMaxSizePerFile(),
				MaxFiles = GetMaxFiles(),
				MaxFolders = GetMaxFolders(),
				AllowedExtensions = GetAllowedExtensions(),
				UserFolder = GetUserFolder()
			};
		}


		private string GetVirtualPath(int siteId = -1)
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteId == -1 && siteSettings == null)
			{
				log.Error("Cannot load file system because Site Settings could not be loaded.");
				throw new ArgumentNullException("could not load SiteSettings");
				
			}
			else if (siteId == -1)
			{
				siteId = siteSettings.SiteId;
			}
			
			//int siteId = siteSettings.SiteId;

			if (siteId == -1 && WebConfigSettings.UseRelatedSiteMode && WebConfigSettings.UseSameContentFolderForRelatedSiteMode)
			{
				siteId = WebConfigSettings.RelatedSiteID;
			}

			var virtualPath = $"~/Data/Sites/{siteId.ToInvariantString()}/media/";

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
			{
				if (WebConfigSettings.ForceAdminsToUseMediaFolder)
				{
					virtualPath = $"~/Data/Sites/{siteId.ToInvariantString()}/media/";
				}
				else
				{
					if (siteSettings.IsServerAdminSite && WebConfigSettings.AllowAdminsToUseDataFolder && WebUser.IsAdmin)
					{
						virtualPath = "~/Data/";
					}
					else
					{
						virtualPath = $"~/Data/Sites/{siteId.ToInvariantString()}/";
					}
				}
			}
			else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
			{
				virtualPath = $"~/Data/Sites/{siteId.ToInvariantString()}/media/";
			}
			else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
			{
				var currentUser = SiteUtils.GetCurrentSiteUser();

				if (currentUser == null)
				{
					log.Error("Cannot load file system because Site User could not be loaded.");
					throw new ArgumentNullException("could not load current SiteUser");
				}

				virtualPath = $"~/Data/Sites/{siteId.ToInvariantString()}/userfiles/{currentUser.UserId.ToInvariantString()}/";
			}


			return virtualPath;
		}


		private string GetUserFolder()
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				log.Error("Cannot load file system because Site Settings could not be loaded.");
				throw new ArgumentNullException("could not load SiteSettings");
			}

			if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
			{
				int siteId = siteSettings.SiteId;

				if (WebConfigSettings.UseRelatedSiteMode && WebConfigSettings.UseSameContentFolderForRelatedSiteMode)
				{
					siteId = WebConfigSettings.RelatedSiteID;
				}

				var currentUser = SiteUtils.GetCurrentSiteUser();

				if (currentUser == null)
				{
					log.Error("Cannot load file system because Site User could not be loaded.");
					throw new ArgumentNullException("could not load current SiteUser");
				}

				return $"~/Data/Sites/{siteId.ToInvariantString()}/userfiles/{currentUser.UserId.ToInvariantString()}/";
			}

			return string.Empty;
		}


		private bool UserHasUploadPermission()
		{
			bool result = false;

			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				return false;
			}

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
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
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				return false;
			}

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor() || WebUser.IsInRoles(siteSettings.GeneralBrowseRoles))
			{
				return true;
			}

			return false;
		}


		private int GetMaxFiles()
		{
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			
			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
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
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
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
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
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
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
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
			var siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
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
