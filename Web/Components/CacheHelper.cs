// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Hosting;
using log4net;
using mojoPortal.Business.Statistics;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Caching;


//Resources to understand caching techniques
//
//http://stackoverflow.com/questions/39112/what-is-the-best-way-to-lock-cache-in-asp-net
//
//creating a custom cache dependency
//http://books.google.com/books?id=cND87IlQ9WMC&pg=PA481&lpg=PA481&dq=custom%2Bcache%2Bdependency&source=bl&ots=TSfH1sHw7b&sig=_PILUBm1TRfy0Nr2ZJXYKrEf4II&hl=en&ei=w0QASvDZKYb6_AawzYDzBg&sa=X&oi=book_result&ct=result&resnum=9#v=onepage&q=custom%2Bcache%2Bdependency&f=false

// page output caching with donut holes
//http://weblogs.asp.net/scottgu/archive/2006/11/28/tip-trick-implement-donut-caching-with-the-asp-net-2-0-output-cache-substitution-feature.aspx


namespace mojoPortal.Business.WebHelpers
{
    public static class CacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CacheHelper));
        private static bool debugLog = log.IsDebugEnabled;

       
        public static void SetClientCaching(HttpContext context, DateTime lastModified)
        {
            if (context == null) return;

            context.Response.Cache.SetETag(lastModified.Ticks.ToString());
            context.Response.Cache.SetLastModified(lastModified);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            context.Response.Cache.SetSlidingExpiration(true);
        }


        public static void SetFileCaching(HttpContext context, string fileName)
        {
            if (fileName == null) return;
            if (context == null) return;
            if (fileName.Length == 0) return;

            context.Response.AddFileDependency(fileName);
            context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            context.Response.Cache.SetSlidingExpiration(true);
        }

        #region cache dependency files

        public static string GetPathToCacheDependencyFile(string cacheKey)
        {
            EnsureDirectory(HostingEnvironment.MapPath("~/Data/systemfiles/"));

            return HostingEnvironment.MapPath(
                "~/Data/systemfiles/" + cacheKey + "_cachedependency.config");
        }

        private static void EnsureDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath)) return;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }


        public static CacheDependency GetSiteMapCacheDependency()
        {
            // we don't need this on a single server nor on a large web farm (where a distributed cache would be used)
            // the only time it is useful is in a small web cluster with a shared drive
            // in that scenario touching the dependency files clears the cache for all nodes sharing the file system

            if (WebConfigSettings.UseCacheDependencyFiles)
            {
                string pathToDependencyFile = GetPathToSiteMapCacheDependencyFile();
                if (pathToDependencyFile.Length > 0)
                {
                    EnsureCacheFile(pathToDependencyFile);
                    CacheDependency c = new CacheDependency(pathToDependencyFile);
                    return c;
                }
            }

            return null;
        }

        //[Obsolete("This method is obsolete we no longer use cache dependency files call CacheHelper.ClearModuleCache(moduleId); instead.")]
        //public static void TouchCacheDependencyFile(string cacheDependencyKey)
        //{
        //    if (HttpContext.Current == null) return;

        //    SiteSettings siteSettings = GetCurrentSiteSettings();
        //    if (siteSettings == null) return;

        //    string pathToCacheDependencyFile = HttpContext.Current.Server.MapPath(
        //            "~/Data/Sites/"
        //            + siteSettings.SiteId.ToInvariantString()
        //            + "/systemfiles/" + cacheDependencyKey + "cachedependecy.config");

        //    TouchCacheFile(pathToCacheDependencyFile);
        //}


        public static void EnsureCacheFile(string pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            if (!File.Exists(pathToCacheFile))
            {
                TouchCacheFile(pathToCacheFile);
            }
        }

        

        public static void ResetThemeCache()
        {
            if (!WebConfigSettings.FileSystemIsWritable) { return; }
            if (WebConfigSettings.UseCacheDependencyFiles)
            {
                string pathToFile = GetPathToThemeCacheDependencyFile();
                if (pathToFile != null)
                {
                    TouchCacheFile(pathToFile);
                }
            }
        }


        public static string GetPathToThemeCacheDependencyFile()
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) { return null; }

            string cacheKey = "theme_" + siteSettings.SiteId.ToInvariantString();

            //string path = HttpContext.Current.Server.MapPath(
            //    "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString()
            //    + "/systemfiles/themecachedependecy.config");
            string path = GetPathToCacheDependencyFile(cacheKey);

            if (!File.Exists(path)) { TouchCacheFile(path); }

            return path;
        }

        public static void TouchCacheFile(String pathToCacheFile)
        {
            if (pathToCacheFile == null) return;

            try
            {
                if (File.Exists(pathToCacheFile))
                {
                    File.SetLastWriteTimeUtc(pathToCacheFile, DateTime.UtcNow);
                }
                else
                {
                    File.CreateText(pathToCacheFile).Close();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error(ex);
            }
        }

        private static string GetPathToSiteMapCacheDependencyFile()
        {

            //SiteSettings siteSettings = GetCurrentSiteSettings();
            //if (siteSettings == null) { return string.Empty; }

            string cacheKey = GetSiteMapCacheKey();

            return GetPathToCacheDependencyFile(cacheKey);
        }

        //private static string GetPathToSiteMapCacheDependencyFile(int siteId)
        //{
        //    return System.Web.Hosting.HostingEnvironment.MapPath(
        //        "~/Data/Sites/" + siteId.ToInvariantString()
        //        + "/systemfiles/sitemapcachedependecy.config");
        //}


        #endregion

        #region clearing the cache

        public static void ClearRelatedSiteCache(int relatedSiteId)
        {
            DataTable siteIds = SiteSettings.GetSiteIdList();

            foreach (DataRow row in siteIds.Rows)
            {
                int siteId = Convert.ToInt32(row["SiteID"]);

                if (siteId != relatedSiteId) { ClearSiteSettingsCache(siteId); }

            }

        }

        public static void ClearSiteSettingsCache()
        {
            //if (HttpContext.Current == null) return;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return;

            ClearSiteSettingsCache(siteSettings.SiteId);
        }

        public static void ClearSiteSettingsCache(int siteId)
        {
            //if (HttpContext.Current == null) return;

            string cachekey = "SiteSettings_" + siteId.ToInvariantString();

            try
            {
                if (WebConfigSettings.UseCacheDependencyFiles)
                {
                    TouchCacheFile(GetPathToCacheDependencyFile(cachekey));
                }
                CacheManager.Cache.InvalidateCacheItem(cachekey);
                
            }
            catch (Exception ex)
            {
                log.Error("failed to clear cache for key " + cachekey, ex);
            }
        }

        public static void ResetSiteMapCache()
        {
            string cacheKey = GetSiteMapCacheKey();

            if (WebConfigSettings.UseCacheDependencyFiles)
            {
                TouchCacheFile(GetPathToCacheDependencyFile(cacheKey));
            }

            if (HttpRuntime.Cache[cacheKey] != null) { HttpRuntime.Cache.Remove(cacheKey); }
        }

        public static void ResetSiteMapCache(int siteId)
        {
            // this only clears the cache of the current node if using a web farm
            string cacheKey = "SiteMap" + siteId.ToInvariantString();

            if (WebConfigSettings.UseCacheDependencyFiles)
            {
                TouchCacheFile(GetPathToCacheDependencyFile(cacheKey));
            }

            if (HttpRuntime.Cache[cacheKey] != null) { HttpRuntime.Cache.Remove(cacheKey); }
        }

        public static string GetSiteMapCacheKey()
        {
            if (HttpContext.Current == null) return null;

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return null;

            return "SiteMap" + siteSettings.SiteId.ToInvariantString();
        }

        public static void ClearModuleCache(int moduleId)
        {
            // clear the version for userds who can edit
            string cacheKey = "Module-" + moduleId.ToInvariantString() + true.ToString(CultureInfo.InvariantCulture);
            //if (HttpRuntime.Cache[cacheKey] != null) { HttpRuntime.Cache.Remove(cacheKey); }
            try
            {
                if (WebConfigSettings.UseCacheDependencyFiles)
                {
                    TouchCacheFile(GetPathToCacheDependencyFile(cacheKey));
                }
                CacheManager.Cache.InvalidateCacheItem(cacheKey);
            }
            catch (Exception ex)
            {
                log.Error("failed to clear cahce for key " + cacheKey, ex);

            }

            // clear the version for users who cannot edit
            cacheKey = "Module-" + moduleId.ToInvariantString() + false.ToString(CultureInfo.InvariantCulture);
            //if (HttpRuntime.Cache[cacheKey] != null) { HttpRuntime.Cache.Remove(cacheKey); }
            try
            {
                if (WebConfigSettings.UseCacheDependencyFiles)
                {
                    TouchCacheFile(GetPathToCacheDependencyFile(cacheKey));
                }
                CacheManager.Cache.InvalidateCacheItem(cacheKey);
            }
            catch (Exception ex)
            {
                log.Error("failed to clear cahce for key " + cacheKey, ex);

            }
        }

        public static void ClearMembershipStatisticsCache()
        {
            string cachekey = GetMembershipStatisticsCacheKey();
            try
            {
                if (WebConfigSettings.UseCacheDependencyFiles)
                {
                    TouchCacheFile(GetPathToCacheDependencyFile(cachekey));
                }
                CacheManager.Cache.InvalidateCacheItem(cachekey);
                
            }
            catch (Exception ex)
            {
                log.Error("failed to clear cache for key " + cachekey, ex);

            }
        }


        #endregion


        public static List<TimeZoneInfo> GetTimeZones()
        {
            string cacheKey = "tzlist";
            DateTime absoluteExpiration = DateTime.Now.AddHours(1);

            try
            {
                List<TimeZoneInfo> timeZones = CacheManager.Cache.Get<List<TimeZoneInfo>>(cacheKey, absoluteExpiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    List<TimeZoneInfo> tz = DateTimeHelper.GetTimeZoneList();
                    return tz;
                });

                return timeZones;
            }
            catch (Exception ex)
            {
                log.Error("failed to get timeZones from cache so loading it directly", ex);
                return DateTimeHelper.GetTimeZoneList();
            }

        }


        #region SiteSettings

        public static SiteSettings GetCurrentSiteSettings()
        {
            return GetSiteSettingsFromContext();
        }

        private static SiteSettings GetSiteSettingsFromContext()
        {
            if (HttpContext.Current == null) return null;

			if (HttpContext.Current.Items["SiteSettings"] is not SiteSettings siteSettings)
			{
				siteSettings = GetSiteSettingsFromCache();
				if (siteSettings != null)
					HttpContext.Current.Items["SiteSettings"] = siteSettings;
			}
			return siteSettings;
        }

        private static SiteSettings GetSiteSettingsFromCache()
        {
            bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
            string cachekey;
            int siteId;

            

            if (useFolderForSiteDetection)
            {
                string siteFolderName = VirtualFolderEvaluator.VirtualFolderName();
                if (siteFolderName.Length == 0) siteFolderName = "root";

				if (!Global.SiteHostMap.TryGetValue(siteFolderName, out siteId))
                {
					siteId = SiteSettings.GetSiteIdByFolder(siteFolderName);
                    Global.SiteHostMap.Add(siteFolderName, siteId);
                }
            }
            else
            {
                String hostName = WebUtils.GetHostName();
				if (!Global.SiteHostMap.TryGetValue(hostName, out siteId))
                {
					siteId = SiteSettings.GetSiteIdByHostName(hostName);
				    Global.SiteHostMap.Add(hostName, siteId);
                }
			}
			cachekey = "SiteSettings_" + siteId.ToInvariantString();

			DateTime expiration = DateTime.Now.AddSeconds(WebConfigSettings.SiteSettingsCacheDurationInSeconds);

            try
            {
                SiteSettings siteSettings = CacheManager.Cache.Get<SiteSettings>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    SiteSettings site = LoadSiteSettings();
                    return site;
                });

                return siteSettings;
            }
            catch (Exception ex)
            {
                log.Error("failed to get siteSettings from cache so loading it directly", ex);
                return LoadSiteSettings();
            }

        }

        
        private static SiteSettings LoadSiteSettings()
        {
            if (debugLog) log.Debug("CacheHelper.cs LoadSiteSettings");

            SiteSettings siteSettings = null;

            try
            {
                bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
                
                string siteFolderName;
                if (useFolderForSiteDetection)
                {
                    siteFolderName = VirtualFolderEvaluator.VirtualFolderName();
                }
                else
                {
                    siteFolderName = string.Empty;
                }
                
                if (useFolderForSiteDetection)
                {
                    Guid siteGuid = SiteFolder.GetSiteGuid(siteFolderName);
                    siteSettings = new SiteSettings(siteGuid);
                }
                else
                {
                    siteSettings = new SiteSettings(WebUtils.GetHostName());
                }

                if (siteSettings.SiteId > -1)
                {
                    siteSettings.ReloadExpandoProperties();
                    siteSettings.SiteRoot = WebUtils.GetSiteRoot();
                    if (useFolderForSiteDetection)
                    {
                        siteSettings.SiteFolderName = siteFolderName;
                    }
                    
                }
                else
                {
                    siteSettings = null;
                    log.Error("CacheHelper failed to load siteSettings");
                }
            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error("Error trying to obtain siteSettings", ex);
            }
            catch (InvalidOperationException ex)
            {
                log.Error("Error trying to obtain siteSettings", ex);
            }
            catch (IndexOutOfRangeException ex)
            {
                log.Error("Error trying to obtain siteSettings", ex);
            }

            return siteSettings;
        }

        /// <summary>
        /// This method should not normally be used, typically you should use the overload with no inputs.
        /// This one is only for supporting mutli sites with the same users across sites. In that case all users
        /// are attached to a specific site, so the membership provider calls this method
        /// to get a sitesettings object with the global security settings.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteSettings GetSiteSettings(int siteId)
        {
            bool useHttpContext = true;
            return GetSiteSettings(siteId, useHttpContext);
        }

        public static SiteSettings GetSiteSettings(int siteId, bool useHttpContext)
        {
            if (useHttpContext)
            {
                return GetSiteSettingsFromContext(siteId);
            }
            else
            {
                return GetSiteSettingsFromCache(siteId);
            }
        }

        private static SiteSettings GetSiteSettingsFromContext(int siteId)
        {
            if (HttpContext.Current == null) return GetSiteSettingsFromCache(siteId);

            string contextKey = "SiteSettings" + siteId.ToInvariantString();

            SiteSettings siteSettings = HttpContext.Current.Items[contextKey] as SiteSettings;
            if (siteSettings == null)
            {
                siteSettings = GetSiteSettingsFromCache(siteId);
                if (siteSettings != null)
                    HttpContext.Current.Items[contextKey] = siteSettings;
            }
            return siteSettings;
        }

        private static SiteSettings GetSiteSettingsFromCache(int siteId)
        {
            if (siteId == -1) { return null; }

            string cachekey = "SiteSettings_" + siteId.ToInvariantString();

            DateTime expiration = DateTime.Now.AddSeconds(WebConfigSettings.SiteSettingsCacheDurationInSeconds);

            try
            {
                SiteSettings siteSettings = CacheManager.Cache.Get<SiteSettings>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    SiteSettings site = new SiteSettings(siteId);
                    return site;
                });

                return siteSettings;
            }
            catch (Exception ex)
            {
                log.Error("failed to get siteSettings from cache so loading it directly", ex);
                return new SiteSettings(siteId);
            }
           
        }

        
        

        #endregion

        #region MembershipStatistics

        public static MembershipStatistics GetCurrentMembershipStatistics()
        {
            return GetMembershipStatisticsFromCache();
        }


        private static string GetMembershipStatisticsCacheKey()
        {
            String hostName = WebUtils.GetHostName();
            return "MembershipStatistics_" + hostName;
        }

        private static MembershipStatistics GetMembershipStatisticsFromCache()
        {

            string cachekey = GetMembershipStatisticsCacheKey();

            DateTime expiration = DateTime.Now.AddSeconds(WebConfigSettings.SiteSettingsCacheDurationInSeconds);

            try
            {
                MembershipStatistics memberStats = CacheManager.Cache.Get<MembershipStatistics>(cachekey, expiration, () =>
                {
                    // This is the anonymous function which gets called if the data is not in the cache.
                    // This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
                    MembershipStatistics stats = LoadMembershipStatistics();
                    return stats;
                });

                return memberStats;
            }
            catch (Exception ex)
            {
                log.Error("failed to get memberStats from cache so loading it directly ", ex);
                return LoadMembershipStatistics();
            }

        }

        
        private static MembershipStatistics LoadMembershipStatistics()
        {
            if (debugLog) log.Debug("CacheHelper.cs LoadMembershipStatistics");

            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) { return null; }

            return new MembershipStatistics(
                siteSettings,
                DateTime.Today.ToUniversalTime().AddHours(DateTimeHelper.GetPreferredGmtOffset()));
        }


        

        

        #endregion

        #region SiteMap/Menu/PageSettings

        
        public static PageSettings GetCurrentPage()
        {
            if (HttpContext.Current == null) return null;

            PageSettings currentPage = HttpContext.Current.Items["CurrentPage"] as PageSettings;
            if (currentPage == null)
            {
                currentPage = LoadCurrentPage();
                if (currentPage != null)
                    HttpContext.Current.Items["CurrentPage"] = currentPage;
            }
            return currentPage;
        }

        public static PageSettings GetPage(int pageId)
        {
            if (HttpContext.Current == null) return null;

            string key = "page_" + pageId.ToInvariantString();

            PageSettings p = HttpContext.Current.Items[key] as PageSettings;
            if (p == null)
            {
                p = LoadPage(pageId);
                if (p != null)
                    HttpContext.Current.Items[key] = p;
            }
            return p;
        }


        private static PageSettings LoadCurrentPage()
        {
            if (debugLog) log.Debug("CacheHelper.cs LoadCurrentPage");

            int pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);

            return LoadPage(pageID);

            
        }

        public static PageSettings GetPage(Guid pageGuid)
        {
            if (HttpContext.Current == null) return null;

            string key = "page_" + pageGuid.ToString();

			if (!(HttpContext.Current.Items[key] is PageSettings p))
			{
				p = LoadPage(pageGuid);
				if (p != null)
					HttpContext.Current.Items[key] = p;
			}
			return p;
        }

		private static PageSettings LoadPage(int pageID)
		{
			if (debugLog) log.Debug("CacheHelper.cs LoadPage(pageID)");

			SiteSettings siteSettings = GetCurrentSiteSettings();

			if (siteSettings == null) return null;

			if (pageID == -1)
			{
				pageID = siteSettings.HomePageOverride;
			}

			PageSettings currentPage = new PageSettings(siteSettings.SiteId, pageID);
			return LoadPage(currentPage);
		}

        private static PageSettings LoadPage(Guid pageGuid)
        {
			if (debugLog) log.Debug("CacheHelper.cs LoadPage(pageGuid)");

			PageSettings currentPage = new PageSettings(pageGuid);
			return LoadPage(currentPage);           
        }

		private static PageSettings LoadPage (PageSettings page)
		{
			SiteSettings siteSettings = GetCurrentSiteSettings();
			if (siteSettings == null) return null;

			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
			string virtualFolder;

			if (useFolderForSiteDetection)
			{
				virtualFolder = VirtualFolderEvaluator.VirtualFolderName();
			}
			else
			{
				virtualFolder = string.Empty;
			}


			//PageSettings currentPage = new PageSettings(siteSettings.SiteId, pageID);
			PageSettings currentPage = page;

			if (currentPage.SiteId != siteSettings.SiteId)
			{   // probably url manipulation trying to use a pageid that
				// doesn't belong to the site so just return the home page
				currentPage = new PageSettings(siteSettings.SiteId, siteSettings.HomePageOverride);
			}

			if (
				(useFolderForSiteDetection)
				&& (virtualFolder.Length > 0)
				&& (currentPage.Url.StartsWith("~/"))
				)
			{
				currentPage.Url = currentPage.Url.Replace("~/", "~/" + virtualFolder + "/");

				currentPage.UrlHasBeenAdjustedForFolderSites = true;
			}

			if (
				(useFolderForSiteDetection)
				&& (virtualFolder.Length > 0)
				&& (!currentPage.UseUrl)
				)
			{
				currentPage.Url = "~/" + virtualFolder + "/Default.aspx?pageid=" + currentPage.PageId.ToString();
				currentPage.UseUrl = true;
				currentPage.UrlHasBeenAdjustedForFolderSites = true;
			}

			LoadPageModule(currentPage);

			return currentPage;
		}

        private static void LoadPageModule(PageSettings pageSettings)
        {

            using (IDataReader reader = Module.GetPageModules(pageSettings.PageId))
            {
                while (reader.Read())
                {
                    Module m = new Module();
                    m.ModuleId = Convert.ToInt32(reader["ModuleID"]);
					m.SiteId = Convert.ToInt32(reader["SiteID"]);
                    m.ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
                    m.ModuleTitle = reader["ModuleTitle"].ToString();
                    m.AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
                    m.CacheTime = Convert.ToInt32(reader["CacheTime"]);
					string showTitle = reader["ShowTitle"].ToString();
                    m.ShowTitle = (showTitle == "True" || showTitle == "1");
                    if (reader["EditUserID"] != DBNull.Value)
                    {
                        m.EditUserId = Convert.ToInt32(reader["EditUserID"]);
                    }
					//m.AvailableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
					//m.AllowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
					//m.Icon = reader["Icon"].ToString();
					m.CreatedByUserId = Convert.ToInt32(reader["CreatedByUserID"]);
					if (reader["CreatedDate"] != DBNull.Value)
					{
						m.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
					}
					//m.CountOfUseOnMyPage
					m.ModuleGuid = new Guid(reader["Guid"].ToString());
                    m.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
					m.SiteGuid = new Guid(reader["SiteGuid"].ToString());
					if (reader["EditUserGuid"] != DBNull.Value)
					{
						m.EditUserGuid = new Guid(reader["EditUserGuid"].ToString());
					}
                    m.HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);
                    m.HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
					m.ViewRoles = reader["ViewRoles"].ToString();
                    m.DraftEditRoles = reader["DraftEditRoles"].ToString();
                    m.IncludeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
                    m.IsGlobal = Convert.ToBoolean(reader["IsGlobal"]);
                    m.HeadElement = reader["HeadElement"].ToString();
                    m.PublishMode = Convert.ToInt32(reader["PublishMode"]);
                    m.DraftApprovalRoles = reader["DraftApprovalRoles"].ToString();

                    m.PageId = Convert.ToInt32(reader["PageID"]);
                    m.PaneName = reader["PaneName"].ToString();
                    m.ModuleOrder = Convert.ToInt32(reader["ModuleOrder"]);
                    m.ControlSource = reader["ControlSrc"].ToString();
					
                    pageSettings.Modules.Add(m);
                }
            }

           
        }


        //called by sitemapprovider
        //called by IndexHelper
        public static Collection<PageSettings> GetMenuPages()
        {
            return GetMenuPagesFromContext();  
        }

        private static Collection<PageSettings> GetMenuPagesFromContext()
        {
            if (HttpContext.Current == null) { return null; }

            Collection<PageSettings> menuPages = HttpContext.Current.Items["MenuPages"] as Collection<PageSettings>;
            if (menuPages == null)
            {
                menuPages = LoadMenuPages();
                if (menuPages != null)
                    HttpContext.Current.Items["MenuPages"] = menuPages;
            }
            return menuPages;
        }

        

        

        

        

        

        
        private static Collection<PageSettings> LoadMenuPages()
        {
            Collection<PageSettings> menuPages = new Collection<PageSettings>();
            
            SiteSettings siteSettings = GetCurrentSiteSettings();
            if (siteSettings == null) return menuPages;

            bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
            string virtualFolder;
            if (useFolderForSiteDetection)
            {
                virtualFolder = VirtualFolderEvaluator.VirtualFolderName();
            }
            else
            {
                virtualFolder = string.Empty;
            }


            using (IDataReader reader = PageSettings.GetPageList(siteSettings.SiteId))
            {

                int i = 0;
                while (reader.Read())
                {
                    PageSettings pageDetails = new PageSettings();
                    pageDetails.SiteId = siteSettings.SiteId;
                    pageDetails.PageId = Convert.ToInt32(reader["PageID"]);
                    pageDetails.ParentId = Convert.ToInt32(reader["ParentID"]);
                    pageDetails.PageName = reader["PageName"].ToString();
                    pageDetails.PageMetaDescription = reader["PageDescription"].ToString();
                    pageDetails.MenuDescription = reader["MenuDesc"].ToString();
                    pageDetails.MenuImage = reader["MenuImage"].ToString();
                    pageDetails.PageOrder = Convert.ToInt32(reader["PageOrder"]);
                    pageDetails.AuthorizedRoles = reader["AuthorizedRoles"].ToString();
                    pageDetails.EditRoles = reader["EditRoles"].ToString();
                    pageDetails.DraftEditOnlyRoles = reader["DraftEditRoles"].ToString();
                    pageDetails.CreateChildPageRoles = reader["CreateChildPageRoles"].ToString();

            
                    pageDetails.UseUrl = Convert.ToBoolean(reader["UseUrl"]);
                    pageDetails.Url = reader["Url"].ToString();
                    pageDetails.UnmodifiedUrl = reader["Url"].ToString();
                    pageDetails.LinkRel = reader["LinkRel"].ToString();
                    pageDetails.IncludeInMenu = Convert.ToBoolean(reader["IncludeInMenu"]);
                    pageDetails.IncludeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
                    pageDetails.ExpandOnSiteMap = Convert.ToBoolean(reader["ExpandOnSiteMap"]);
                    pageDetails.IncludeInSearchMap = Convert.ToBoolean(reader["IncludeInSearchMap"]);
                    pageDetails.IsClickable = Convert.ToBoolean(reader["IsClickable"]);
                    pageDetails.ShowHomeCrumb = Convert.ToBoolean(reader["ShowHomeCrumb"]);
                    pageDetails.RequireSsl = Convert.ToBoolean(reader["RequireSSL"]);
                    if(reader["PubDateUtc"] != DBNull.Value)
                    {
                        pageDetails.PubDateUtc = Convert.ToDateTime(reader["PubDateUtc"]);
                    }

                    if (
                        (useFolderForSiteDetection)
                        && (virtualFolder.Length > 0)
                        && (pageDetails.Url.StartsWith("~/"))
                        )
                    {
                        pageDetails.Url
                            = pageDetails.Url.Replace("~/", "~/" + virtualFolder + "/");
                        pageDetails.UrlHasBeenAdjustedForFolderSites = true;
                    }

                    if (
                        (useFolderForSiteDetection)
                        && (virtualFolder.Length > 0)
                        && (!pageDetails.UseUrl)
                        )
                    {
                        pageDetails.UnmodifiedUrl = "~/Default.aspx?pageid="
                            + pageDetails.PageId.ToString(CultureInfo.InvariantCulture);

                        pageDetails.Url
                            = "~/" + virtualFolder + "/Default.aspx?pageid="
                            + pageDetails.PageId.ToString(CultureInfo.InvariantCulture);
                        pageDetails.UseUrl = true;
                        pageDetails.UrlHasBeenAdjustedForFolderSites = true;
                    }

                    
                    pageDetails.OpenInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]);
                    pageDetails.ShowChildPageMenu = Convert.ToBoolean(reader["ShowChildPageMenu"]);
                    pageDetails.ShowChildPageBreadcrumbs = Convert.ToBoolean(reader["ShowChildBreadCrumbs"]);
                    pageDetails.PageIndex = i;

                    string cf = reader["ChangeFrequency"].ToString();
                    switch (cf)
                    {
                        case "Always":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Always;
                            break;

                        case "Hourly":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Hourly;
                            break;

                        case "Daily":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Daily;
                            break;

                        case "Monthly":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Monthly;
                            break;

                        case "Yearly":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Yearly;
                            break;

                        case "Never":
                            pageDetails.ChangeFrequency = PageChangeFrequency.Never;
                            break;

                        case "Weekly":
                        default:
                            pageDetails.ChangeFrequency = PageChangeFrequency.Weekly;
                            break;


                    }

                    string smp = reader["SiteMapPriority"].ToString().Trim();
                    if (smp.Length > 0) pageDetails.SiteMapPriority = smp;

                    if (reader["LastModifiedUTC"] != DBNull.Value)
                    {
                        pageDetails.LastModifiedUtc = Convert.ToDateTime(reader["LastModifiedUTC"]);
                    }

                    pageDetails.PageGuid = new Guid(reader["PageGuid"].ToString());
                    pageDetails.ParentGuid = new Guid(reader["ParentGuid"].ToString());

               
                    pageDetails.HideAfterLogin = Convert.ToBoolean(reader["HideAfterLogin"]);

                    pageDetails.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                    pageDetails.CompiledMeta = reader["CompiledMeta"].ToString();
                    if (reader["CompiledMetaUtc"] != DBNull.Value)
                    {
                        pageDetails.CompiledMetaUtc = Convert.ToDateTime(reader["CompiledMetaUtc"]);
                    }

                    pageDetails.IsPending = Convert.ToBoolean(reader["IsPending"]);

                    pageDetails.PubTeamId = new Guid(reader["PubTeamId"].ToString());
                    pageDetails.IncludeInChildSiteMap = Convert.ToBoolean(reader["IncludeInChildSiteMap"]);

                    pageDetails.BodyCssClass = reader["BodyCssClass"].ToString();
                    pageDetails.MenuCssClass = reader["MenuCssClass"].ToString();

                    pageDetails.PublishMode = Convert.ToInt32(reader["PublishMode"]);

                    menuPages.Add(pageDetails);
                    i++;
                }
            }

            return menuPages;
        }

        

        #endregion

        

        

        

        

        public static string GetPathToWebConfigFile()
        {
            return HostingEnvironment.MapPath("~/Web.config");
        }

        

        //public static void ClearSiteMap(Page page, int siteId)
        //{
        //    SiteMapDataSource siteMapDataSource = (SiteMapDataSource)page.Master.FindControl("SiteMapData");
        //    if (siteMapDataSource != null)
        //    {
        //        siteMapDataSource.SiteMapProvider = "mojosite" + siteId.ToInvariantString();
        //        mojoSiteMapProvider mojoSiteMap = siteMapDataSource.Provider as mojoSiteMapProvider;
        //        if (mojoSiteMap != null) { mojoSiteMap.ClearSiteMap(); }
        //    }
        //}

        

        

        

        //private static string GetPathToMembershipStatisticsCacheDependencyFile()
        //{
        //    if (HttpContext.Current == null) return null;

        //    SiteSettings siteSettings = GetCurrentSiteSettings();
        //    if (siteSettings == null) return null;

        //    return HttpContext.Current.Server.MapPath(
        //        "~/Data/Sites/"
        //        + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
        //        + "/systemfiles/membershipstatisticscachedependecy.config");
        //}

        //public static int GetDefaultModuleCacheTime()
        //{
        //    int cacheTime;
        //    if (!int.TryParse(ConfigurationManager.AppSettings["DefaultModuleCacheDurationInSeconds"], out cacheTime))
        //        cacheTime = 360;
        //    return cacheTime;
        //}

        //public static String GetPathToCacheDependencyFile(String cacheDependencyKey)
        //{
        //    if (HttpContext.Current == null) return null;

        //    SiteSettings siteSettings = GetCurrentSiteSettings();
        //    if (siteSettings == null) return null;

        //    return HttpContext.Current.Server.MapPath(
        //        "~/Data/Sites/"
        //        + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
        //        + "/systemfiles/" + cacheDependencyKey + "cachedependecy.config");
        //}

        //private static void RefreshMembershipStatisticsCache(String cacheKey, int cacheTimeout)
        //{
        //    if (HttpContext.Current == null) return;

        //    MembershipStatistics membershipStatistics = LoadMembershipStatistics();
        //    if (membershipStatistics == null) return;

        //    //String pathToCacheDependencyFile 
        //    //    = GetPathToMembershipStatisticsCacheDependencyFile();

        //    //if (pathToCacheDependencyFile != null)
        //    //{
        //    //    EnsureCacheFile(pathToCacheDependencyFile);
        //    //}

        //    //CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyFile);
        //    CacheDependency cacheDependency = null;
        //    DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
        //    //TimeSpan slidingExpiration = TimeSpan.Zero;
        //    //CacheItemPriority priority = CacheItemPriority.Default;
        //    CacheItemRemovedCallback callback = null;

        //    HttpRuntime.Cache.Insert(
        //        cacheKey,
        //        membershipStatistics,
        //        cacheDependency,
        //        absoluteExpiration,
        //        Cache.NoSlidingExpiration,
        //        CacheItemPriority.Default,
        //        callback);
        //}

        

        //private static void RefreshSiteSettingsCache(String cacheKey, int cacheTimeout)
        //{
        //    if (HttpContext.Current == null) return;

        //    SiteSettings siteSettings = LoadSiteSettings();
        //    if (siteSettings == null) return;

        //    //string pathToCacheDependencyDirectory = HttpContext.Current.Server.MapPath(
        //    //    "~/Data/Sites/"
        //    //    + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
        //    //    + "/systemfiles/");

        //    //EnsureDirectory(pathToCacheDependencyDirectory);
        //    //EnsureCacheFile(pathToCacheDependencyDirectory + "sitesettingscachedependecy.config");

        //    //CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyDirectory + "sitesettingscachedependecy.config");
        //    CacheDependency cacheDependency = null;

        //    DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
        //    //TimeSpan slidingExpiration = TimeSpan.Zero;
        //    //CacheItemPriority priority = CacheItemPriority.Default;
        //    CacheItemRemovedCallback callback = null;

        //    HttpRuntime.Cache.Insert(
        //        cacheKey,
        //        siteSettings,
        //        cacheDependency,
        //        absoluteExpiration,
        //        Cache.NoSlidingExpiration,
        //        CacheItemPriority.Default,
        //        callback);
        //}

        //private static void RefreshSiteSettingsCache(int siteId, String cacheKey, int cacheTimeout)
        //{
        //    if (HttpContext.Current == null) return;

        //    SiteSettings siteSettings = new SiteSettings(siteId);


        //    //string pathToCacheDependencyDirectory = HttpContext.Current.Server.MapPath(
        //    //    "~/Data/Sites/"
        //    //    + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)
        //    //    + "/systemfiles/");

        //    //EnsureDirectory(pathToCacheDependencyDirectory);
        //    //EnsureCacheFile(pathToCacheDependencyDirectory + "sitesettingscachedependecy.config");

        //    //CacheDependency cacheDependency = new CacheDependency(pathToCacheDependencyDirectory + "sitesettingscachedependecy.config");
        //    CacheDependency cacheDependency = null;



        //    DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
        //    //TimeSpan slidingExpiration = TimeSpan.Zero;
        //    //CacheItemPriority priority = CacheItemPriority.Default;
        //    CacheItemRemovedCallback callback = null;

        //    HttpRuntime.Cache.Insert(
        //        cacheKey,
        //        siteSettings,
        //        cacheDependency,
        //        absoluteExpiration,
        //        Cache.NoSlidingExpiration,
        //        CacheItemPriority.Default,
        //        callback);
        //}

    }
}
