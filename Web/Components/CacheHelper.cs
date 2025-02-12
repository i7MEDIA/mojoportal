using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using log4net;
using mojoPortal.Business.Statistics;
using mojoPortal.Web;
using mojoPortal.Web.Caching;
using mojoPortal.Web.Framework;


//Resources to understand caching techniques
//
//http://stackoverflow.com/questions/39112/what-is-the-best-way-to-lock-cache-in-asp-net
//
//creating a custom cache dependency
//http://books.google.com/books?id=cND87IlQ9WMC&pg=PA481&lpg=PA481&dq=custom%2Bcache%2Bdependency&source=bl&ots=TSfH1sHw7b&sig=_PILUBm1TRfy0Nr2ZJXYKrEf4II&hl=en&ei=w0QASvDZKYb6_AawzYDzBg&sa=X&oi=book_result&ct=result&resnum=9#v=onepage&q=custom%2Bcache%2Bdependency&f=false

// page output caching with donut holes
//http://weblogs.asp.net/scottgu/archive/2006/11/28/tip-trick-implement-donut-caching-with-the-asp-net-2-0-output-cache-substitution-feature.aspx


namespace mojoPortal.Business.WebHelpers;

public static class CacheHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(CacheHelper));
	private static readonly bool debugLog = log.IsDebugEnabled;

	public static void SetClientCaching(HttpContext context, DateTime lastModified)
	{
		if (context is not null)
		{
			context.Response.Cache.SetETag(lastModified.Ticks.ToString());
			context.Response.Cache.SetLastModified(lastModified);
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
			context.Response.Cache.SetSlidingExpiration(true);
		}
	}


	public static void SetFileCaching(HttpContext context, string fileName)
	{
		if (fileName is not null && !string.IsNullOrWhiteSpace(fileName) && context is not null)
		{
			context.Response.AddFileDependency(fileName);
			context.Response.Cache.SetETagFromFileDependencies();
			context.Response.Cache.SetLastModifiedFromFileDependencies();
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
			context.Response.Cache.SetSlidingExpiration(true);
		}
	}

	#region cache dependency files

	public static string GetPathToCacheDependencyFile(string cacheKey)
	{
		var sysFilesPath = "~/Data/systemfiles";
		Directory.CreateDirectory(sysFilesPath); //will create directory if it doesn't exist
		return HostingEnvironment.MapPath($"{sysFilesPath}/{cacheKey}_cachedependency.config");
	}

	public static CacheDependency GetSiteMapCacheDependency()
	{
		// we don't need this on a single server nor on a large web farm (where a distributed cache would be used)
		// the only time it is useful is in a small web cluster with a shared drive
		// in that scenario touching the dependency files clears the cache for all nodes sharing the file system

		if (WebConfigSettings.UseCacheDependencyFiles)
		{
			var pathToDependencyFile = GetPathToSiteMapCacheDependencyFile();
			if (!string.IsNullOrWhiteSpace(pathToDependencyFile))
			{
				EnsureCacheFile(pathToDependencyFile);
				var c = new CacheDependency(pathToDependencyFile);
				return c;
			}
		}

		return null;
	}


	public static void EnsureCacheFile(string pathToCacheFile)
	{
		if (pathToCacheFile is null)
		{
			return;
		}

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
			if (pathToFile is not null)
			{
				TouchCacheFile(pathToFile);
			}
		}
	}


	public static string GetPathToThemeCacheDependencyFile()
	{
		if (HttpContext.Current is not null && GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			var cacheKey = Invariant($"theme_{siteSettings.SiteId}");
			var path = GetPathToCacheDependencyFile(cacheKey);

			if (!File.Exists(path))
			{
				TouchCacheFile(path);
			}
			return path;
		}

		return null;
	}

	public static void TouchCacheFile(string pathToCacheFile)
	{
		if (pathToCacheFile is null)
		{
			return;
		}

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
		string cacheKey = GetSiteMapCacheKey();
		return GetPathToCacheDependencyFile(cacheKey);
	}

	#endregion

	#region clearing the cache

	public static void ClearRelatedSiteCache(int relatedSiteId)
	{
		var siteIds = SiteSettings.GetSiteIdList();

		foreach (DataRow row in siteIds.Rows)
		{
			int siteId = Convert.ToInt32(row["SiteID"]);

			if (siteId != relatedSiteId)
			{
				ClearSiteSettingsCache(siteId);
			}
		}
	}

	public static void ClearSiteSettingsCache()
	{
		if (GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			ClearSiteSettingsCache(siteSettings.SiteId);
		}
	}

	public static void ClearSiteSettingsCache(int siteId)
	{
		var cachekey = Invariant($"SiteSettings_{siteId}");

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
			log.Error($"failed to clear cache for key {cachekey}", ex);
		}
	}

	public static void ResetSiteMapCache()
	{
		var cacheKey = GetSiteMapCacheKey();

		if (WebConfigSettings.UseCacheDependencyFiles)
		{
			TouchCacheFile(GetPathToCacheDependencyFile(cacheKey));
		}

		if (HttpRuntime.Cache[cacheKey] is not null)
		{
			HttpRuntime.Cache.Remove(cacheKey);
		}
	}

	public static void ResetSiteMapCache(int siteId)
	{
		// this only clears the cache of the current node if using a web farm
		var cacheKey = Invariant($"SiteMap{siteId}");

		if (WebConfigSettings.UseCacheDependencyFiles)
		{
			TouchCacheFile(GetPathToCacheDependencyFile(cacheKey));
		}

		if (HttpRuntime.Cache[cacheKey] is not null)
		{
			HttpRuntime.Cache.Remove(cacheKey);
		}
	}

	public static string GetSiteMapCacheKey()
	{
		if (HttpContext.Current is not null && GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			return Invariant($"SiteMap{siteSettings.SiteId}");
		}

		return null;
	}

	public static void ClearModuleCache(int moduleId)
	{
		// clear the version for users who can edit
		var cacheKey = Invariant($"Module-{moduleId}{true}");

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
			log.Error($"failed to clear cahce for key {cacheKey}", ex);

		}

		// clear the version for users who cannot edit
		cacheKey = Invariant($"Module-{moduleId}{false}");

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
			log.Error($"failed to clear cahce for key {cacheKey}", ex);

		}
	}


	public static void ClearMembershipStatisticsCache()
	{
		var cachekey = GetMembershipStatisticsCacheKey();
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
			log.Error($"failed to clear cache for key {cachekey}", ex);
		}
	}


	public static void ClearHttpRuntimeCache()
	{
		var enumerator = HttpRuntime.Cache.GetEnumerator();

		while (enumerator.MoveNext())
		{
			HttpRuntime.Cache.Remove(enumerator.Key.ToString());
		}
	}

	#endregion

	public static List<TimeZoneInfo> GetTimeZones()
	{
		var cacheKey = "tzlist";
		var absoluteExpiration = DateTime.Now.AddHours(1);

		try
		{
			var timeZones = CacheManager.Cache.Get(cacheKey, absoluteExpiration, () =>
			{
				// This is the anonymous function which gets called if the data is not in the cache.
				// This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
				var tz = DateTimeHelper.GetTimeZoneList();
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
		return GetCurrentSiteSettings(-1);
	}

	public static SiteSettings GetCurrentSiteSettings(int siteId = -1)
	{
		if (siteId == -1)
		{
			return GetSiteSettingsFromContext();
		}

		return new SiteSettings(siteId);
	}

	private static SiteSettings GetSiteSettingsFromContext()
	{
		if (HttpContext.Current is null)
		{
			return null;
		}

		if (HttpContext.Current.Items["SiteSettings"] is not SiteSettings siteSettings)
		{
			siteSettings = GetSiteSettingsFromCache();
			if (siteSettings is not null)
			{
				HttpContext.Current.Items["SiteSettings"] = siteSettings;
			}
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
			if (siteFolderName.Length == 0)
			{
				siteFolderName = "root";
			}

			if (!Global.SiteHostMap.TryGetValue(siteFolderName, out siteId))
			{
				siteId = SiteSettings.GetSiteIdByFolder(siteFolderName);
				Global.SiteHostMap.TryAdd(siteFolderName, siteId);
			}
		}
		else
		{
			var hostName = WebUtils.GetHostName();

			if (!Global.SiteHostMap.TryGetValue(hostName, out siteId))
			{
				if (DatabaseHelper.ExistingSiteCount() > 0)
				{
					siteId = SiteSettings.GetSiteIdByHostName(hostName);
					Global.SiteHostMap.TryAdd(hostName, siteId);
				}
			}
		}

		cachekey = Invariant($"SiteSettings_{siteId}");

		var expiration = DateTime.Now.AddSeconds(WebConfigSettings.SiteSettingsCacheDurationInSeconds);

		try
		{
			var siteSettings = CacheManager.Cache.Get(cachekey, expiration, () =>
			{
				// This is the anonymous function which gets called if the data is not in the cache.
				// This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
				var site = LoadSiteSettings();
				return site;
			});

			return siteSettings;
		}
		catch (Exception ex)
		{
			log.Error("failed to get siteSettings from cache, loading directly", ex);

			return LoadSiteSettings();
		}
	}

	private static SiteSettings LoadSiteSettings()
	{
		SiteSettings siteSettings = null;

		try
		{
			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

			string siteFolderName = string.Empty;
			if (useFolderForSiteDetection)
			{
				siteFolderName = VirtualFolderEvaluator.VirtualFolderName();
				Guid siteGuid = SiteFolder.GetSiteGuid(siteFolderName);
				siteSettings = new SiteSettings(siteGuid);
			}
			else
			{
				siteSettings = new SiteSettings(WebUtils.GetHostName());
			}

			if (siteSettings.SiteId > 0)
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
	public static SiteSettings GetSiteSettings(int siteId) => GetSiteSettings(siteId, true);

	public static SiteSettings GetSiteSettings(int siteId, bool useHttpContext) => useHttpContext ? GetSiteSettingsFromContext(siteId) : GetSiteSettingsFromCache(siteId);

	private static SiteSettings GetSiteSettingsFromContext(int siteId)
	{
		if (HttpContext.Current is null)
		{
			return GetSiteSettingsFromCache(siteId);
		}

		var contextKey = Invariant($"SiteSettings{siteId}");

		if (HttpContext.Current.Items[contextKey] is not SiteSettings siteSettings)
		{
			siteSettings = GetSiteSettingsFromCache(siteId);
			if (siteSettings is not null)
			{
				HttpContext.Current.Items[contextKey] = siteSettings;
			}
		}
		return siteSettings;
	}

	private static SiteSettings GetSiteSettingsFromCache(int siteId)
	{
		if (siteId == -1)
		{
			return null;
		}

		string cachekey = Invariant($"SiteSettings_{siteId}");

		var expiration = DateTime.Now.AddSeconds(WebConfigSettings.SiteSettingsCacheDurationInSeconds);

		try
		{
			var siteSettings = CacheManager.Cache.Get(cachekey, expiration, () =>
			{
				// This is the anonymous function which gets called if the data is not in the cache.
				// This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
				var site = new SiteSettings(siteId);
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

	public static MembershipStatistics GetCurrentMembershipStatistics() => GetMembershipStatisticsFromCache();


	private static string GetMembershipStatisticsCacheKey() => $"MembershipStatistics_{WebUtils.GetHostName()}";


	private static MembershipStatistics GetMembershipStatisticsFromCache()
	{
		var cachekey = GetMembershipStatisticsCacheKey();

		var expiration = DateTime.Now.AddSeconds(WebConfigSettings.SiteSettingsCacheDurationInSeconds);

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
		if (GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			return new MembershipStatistics(
				siteSettings,
				DateTime.Today.ToUniversalTime().AddHours(DateTimeHelper.GetPreferredGmtOffset()));
		}
		return null;
	}

	#endregion

	#region SiteMap/Menu/PageSettings


	public static PageSettings GetCurrentPage()
	{
		if (HttpContext.Current is null)
		{
			return null;
		}

		if (HttpContext.Current.Items["CurrentPage"] is not PageSettings currentPage)
		{
			currentPage = LoadCurrentPage();
			if (currentPage is not null)
			{
				HttpContext.Current.Items["CurrentPage"] = currentPage;
			}
		}

		return currentPage;
	}


	public static PageSettings GetPage(int pageId)
	{
		if (HttpContext.Current is null)
		{
			return null;
		}

		string key = Invariant($"page_{pageId}");

		if (HttpContext.Current.Items[key] is not PageSettings p)
		{
			p = LoadPage(pageId);
			if (p is not null)
			{
				HttpContext.Current.Items[key] = p;
			}
		}

		return p;
	}


	public static PageSettings GetPage(Guid pageGuid)
	{
		if (HttpContext.Current is null)
		{
			return null;
		}

		string key = $"page_{pageGuid}";

		if (HttpContext.Current.Items[key] is not PageSettings p)
		{
			p = LoadPage(pageGuid);

			if (p is not null)
			{
				HttpContext.Current.Items[key] = p;
			}
		}
		return p;
	}


	private static PageSettings LoadCurrentPage() => LoadPage(WebUtils.ParseInt32FromQueryString("pageid", -1));


	private static PageSettings LoadPage(int pageID)
	{
		if (debugLog)
		{
			log.Debug("CacheHelper.cs LoadPage(pageID)");
		}

		if (GetCurrentSiteSettings() is SiteSettings siteSettings)
		{
			if (pageID == -1)
			{
				pageID = siteSettings.HomePageOverride;
			}

			return LoadPage(new PageSettings(siteSettings.SiteId, pageID));
		}

		return null;
	}

	private static PageSettings LoadPage(Guid pageGuid) => LoadPage(new PageSettings(pageGuid));

	private static PageSettings LoadPage(PageSettings pageSettings)
	{
		if (GetCurrentSiteSettings() is not SiteSettings siteSettings)
		{
			return null;
		}

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

		var currentPage = pageSettings;

		if (currentPage.SiteId != siteSettings.SiteId)
		{   // probably url manipulation trying to use a pageid that
			// doesn't belong to the site so just return the home page
			currentPage = new PageSettings(siteSettings.SiteId, siteSettings.HomePageOverride);
		}

		if (useFolderForSiteDetection
			&& !string.IsNullOrWhiteSpace(virtualFolder)
			&& currentPage.Url.StartsWith("~/"))
		{
			currentPage.Url = currentPage.Url.Replace("~/", $"~/{virtualFolder}/");
			currentPage.UrlHasBeenAdjustedForFolderSites = true;
		}

		if (useFolderForSiteDetection
			&& !string.IsNullOrWhiteSpace(virtualFolder)
			&& !currentPage.UseUrl
			)
		{
			currentPage.Url = $"~/{virtualFolder}/Default.aspx".ToLinkBuilder().PageId(currentPage.PageId).ToString();
			currentPage.UseUrl = true;
			currentPage.UrlHasBeenAdjustedForFolderSites = true;
		}

		LoadPageModule(currentPage);

		return currentPage;
	}

	private static void LoadPageModule(PageSettings pageSettings)
	{
		using IDataReader reader = Module.GetPageModules(pageSettings.PageId);
		while (reader.Read())
		{
			var m = new Module
			{
				ModuleId = Convert.ToInt32(reader["ModuleID"]),
				SiteId = Convert.ToInt32(reader["SiteID"]),
				ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]),
				ModuleTitle = reader["ModuleTitle"].ToString(),
				AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString(),
				CacheTime = Convert.ToInt32(reader["CacheTime"]),
				//ShowTitle = reader["ShowTitle"].ToString() == "True" || reader["ShowTitle"].ToString() == "1",
				ShowTitle = reader["ShowTitle"].ToString() is "True" or "1",
				CreatedByUserId = Convert.ToInt32(reader["CreatedByUserID"]),
				ModuleGuid = new Guid(reader["Guid"].ToString()),
				FeatureGuid = new Guid(reader["FeatureGuid"].ToString()),
				SiteGuid = new Guid(reader["SiteGuid"].ToString()),
				HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]),
				HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]),
				ViewRoles = reader["ViewRoles"].ToString(),
				DraftEditRoles = reader["DraftEditRoles"].ToString(),
				IncludeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]),
				IsGlobal = Convert.ToBoolean(reader["IsGlobal"]),
				HeadElement = reader["HeadElement"].ToString(),
				PublishMode = Convert.ToInt32(reader["PublishMode"]),
				DraftApprovalRoles = reader["DraftApprovalRoles"].ToString(),
				PageId = Convert.ToInt32(reader["PageID"]),
				PaneName = reader["PaneName"].ToString(),
				ModuleOrder = Convert.ToInt32(reader["ModuleOrder"]),
				ControlSource = reader["ControlSrc"].ToString()
			};

			if (reader["EditUserID"] != DBNull.Value)
			{
				m.EditUserId = Convert.ToInt32(reader["EditUserID"]);
			}

			if (reader["CreatedDate"] != DBNull.Value)
			{
				m.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
			}

			if (reader["EditUserGuid"] != DBNull.Value)
			{
				m.EditUserGuid = new Guid(reader["EditUserGuid"].ToString());
			}

			pageSettings.Modules.Add(m);
		}
	}

	//called by sitemapprovider
	//called by IndexHelper
	public static Collection<PageSettings> GetMenuPages() => GetMenuPagesFromContext();

	private static Collection<PageSettings> GetMenuPagesFromContext()
	{
		if (HttpContext.Current is null)
		{
			return null;
		}

		if (HttpContext.Current.Items["MenuPages"] is not Collection<PageSettings> menuPages)
		{
			menuPages = LoadMenuPages();
			if (menuPages is not null)
			{
				HttpContext.Current.Items["MenuPages"] = menuPages;
			}
		}
		return menuPages;
	}

	private static Collection<PageSettings> LoadMenuPages()
	{
		var menuPages = new Collection<PageSettings>();

		if (GetCurrentSiteSettings() is not SiteSettings siteSettings)
		{
			return menuPages;
		}

		bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;
		string virtualFolder = string.Empty;

		if (useFolderForSiteDetection)
		{
			virtualFolder = VirtualFolderEvaluator.VirtualFolderName();
		}

		using (IDataReader reader = PageSettings.GetPageList(siteSettings.SiteId))
		{
			int i = 0;
			while (reader.Read())
			{
				var pageDetails = new PageSettings
				{
					SiteId = siteSettings.SiteId,
					PageId = Convert.ToInt32(reader["PageID"]),
					ParentId = Convert.ToInt32(reader["ParentID"]),
					PageName = reader["PageName"].ToString(),
					PageMetaDescription = reader["PageDescription"].ToString(),
					MenuDescription = reader["MenuDesc"].ToString(),
					MenuImage = reader["MenuImage"].ToString(),
					PageOrder = Convert.ToInt32(reader["PageOrder"]),
					AuthorizedRoles = reader["AuthorizedRoles"].ToString(),
					EditRoles = reader["EditRoles"].ToString(),
					DraftEditOnlyRoles = reader["DraftEditRoles"].ToString(),
					CreateChildPageRoles = reader["CreateChildPageRoles"].ToString(),
					UseUrl = Convert.ToBoolean(reader["UseUrl"]),
					Url = reader["Url"].ToString(),
					UnmodifiedUrl = reader["Url"].ToString(),
					LinkRel = reader["LinkRel"].ToString(),
					IncludeInMenu = Convert.ToBoolean(reader["IncludeInMenu"]),
					IncludeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]),
					ExpandOnSiteMap = Convert.ToBoolean(reader["ExpandOnSiteMap"]),
					IncludeInSearchMap = Convert.ToBoolean(reader["IncludeInSearchMap"]),
					IsClickable = Convert.ToBoolean(reader["IsClickable"]),
					ShowHomeCrumb = Convert.ToBoolean(reader["ShowHomeCrumb"]),
					RequireSsl = Convert.ToBoolean(reader["RequireSSL"]),
					OpenInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]),
					ShowChildPageMenu = Convert.ToBoolean(reader["ShowChildPageMenu"]),
					ShowChildPageBreadcrumbs = Convert.ToBoolean(reader["ShowChildBreadCrumbs"]),
					PageIndex = i,
					ChangeFrequency = reader["ChangeFrequency"].ToString() switch
					{
						"Always" => PageChangeFrequency.Always,
						"Hourly" => PageChangeFrequency.Hourly,
						"Daily" => PageChangeFrequency.Daily,
						"Monthly" => PageChangeFrequency.Monthly,
						"Yearly" => PageChangeFrequency.Yearly,
						"Never" => PageChangeFrequency.Never,
						_ => PageChangeFrequency.Weekly,
					},
					PageGuid = new Guid(reader["PageGuid"].ToString()),
					ParentGuid = new Guid(reader["ParentGuid"].ToString()),
					HideAfterLogin = Convert.ToBoolean(reader["HideAfterLogin"]),
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					CompiledMeta = reader["CompiledMeta"].ToString(),
					IsPending = Convert.ToBoolean(reader["IsPending"]),

					PubTeamId = new Guid(reader["PubTeamId"].ToString()),
					IncludeInChildSiteMap = Convert.ToBoolean(reader["IncludeInChildSiteMap"]),

					BodyCssClass = reader["BodyCssClass"].ToString(),
					MenuCssClass = reader["MenuCssClass"].ToString(),

					PublishMode = Convert.ToInt32(reader["PublishMode"])
				};

				if (reader["PubDateUtc"] != DBNull.Value)
				{
					pageDetails.PubDateUtc = Convert.ToDateTime(reader["PubDateUtc"]);
				}

				if (useFolderForSiteDetection
					&& !string.IsNullOrWhiteSpace(virtualFolder)
					&& pageDetails.Url.StartsWith("~/")
					)
				{
					pageDetails.Url = pageDetails.Url.Replace("~/", $"~/{virtualFolder}/");
					pageDetails.UrlHasBeenAdjustedForFolderSites = true;
				}

				if (useFolderForSiteDetection
					&& !string.IsNullOrWhiteSpace(virtualFolder)
					&& !pageDetails.UseUrl
					)
				{
					pageDetails.UnmodifiedUrl = Invariant($"~/Default.aspx?pageid={pageDetails.PageId}");

					pageDetails.Url = $"~/{virtualFolder}/Default.aspx".ToLinkBuilder().PageId(pageDetails.PageId).ToString();
					pageDetails.UseUrl = true;
					pageDetails.UrlHasBeenAdjustedForFolderSites = true;
				}

				var smp = reader["SiteMapPriority"].ToString().Trim();
				if (smp.Length > 0)
				{
					pageDetails.SiteMapPriority = smp;
				}

				if (reader["LastModifiedUTC"] != DBNull.Value)
				{
					pageDetails.LastModifiedUtc = Convert.ToDateTime(reader["LastModifiedUTC"]);
				}

				if (reader["CompiledMetaUtc"] != DBNull.Value)
				{
					pageDetails.CompiledMetaUtc = Convert.ToDateTime(reader["CompiledMetaUtc"]);
				}

				menuPages.Add(pageDetails);
				i++;
			}
		}

		return menuPages;
	}

	#endregion

	public static string GetPathToWebConfigFile() => HostingEnvironment.MapPath("~/web.config");
}
