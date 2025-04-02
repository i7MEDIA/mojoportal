// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by 
// Changed namespaces and modified for easier use in mojoPortal
//
// Change history for this file since original fork:
// 2011-08-03  modified to support .NET 3.5
// 2012-03-22 added support for use of Cache Dependency files (.net 3.5) and HostFileChangeMonitor (.net 4)

using log4net;
using mojoPortal.Business.WebHelpers;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace mojoPortal.Web.Caching;

public class MemoryCacheAdapter : ICache
{
	private static readonly ILog log = LogManager.GetLogger(typeof(MemoryCacheAdapter));
	private static readonly bool debugLog = log.IsDebugEnabled;
	private readonly MemoryCache _cache = MemoryCache.Default;


	private HostFileChangeMonitor GetHostFileChangeMonitor(string cacheKey)
	{
		var pathToDependency = CacheHelper.GetPathToCacheDependencyFile(cacheKey);

		CacheHelper.EnsureCacheFile(pathToDependency);

		var filePaths = new List<string>
		{
			pathToDependency
		};

		return new HostFileChangeMonitor(filePaths);
	}


	public void Add(string cacheKey, DateTime expiry, object dataToAdd)
	{
		var policy = new CacheItemPolicy
		{
			AbsoluteExpiration = new DateTimeOffset(expiry)
		};

		if (WebConfigSettings.UseCacheDependencyFiles)
		{
			policy.ChangeMonitors.Add(GetHostFileChangeMonitor(cacheKey));
		}

		if (dataToAdd is null)
		{
			return;
		}

		_cache.Add(cacheKey, dataToAdd, policy);

		if (debugLog)
		{
			log.Debug($"Adding data to cache with cache key: {cacheKey}, expiry date {expiry:yyyy/MM/dd hh:mm:ss}");
		}
	}


	public object GetObject(string cacheKey) => _cache.Get(cacheKey);


	public T Get<T>(string cacheKey) where T : class => _cache.Get(cacheKey) as T;


	public void InvalidateCacheItem(string cacheKey) => _cache.Remove(cacheKey);


	public void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
	{
		if (dataToAdd != null)
		{
			var item = new CacheItem(cacheKey, dataToAdd);
			var policy = new CacheItemPolicy()
			{
				SlidingExpiration = slidingExpiryWindow
			};

			if (WebConfigSettings.UseCacheDependencyFiles)
			{
				policy.ChangeMonitors.Add(GetHostFileChangeMonitor(cacheKey));
			}

			_cache.Add(item, policy);

			if (debugLog)
			{
				log.Debug(string.Format("Adding data to cache with cache key: {0}, sliding expiry window in seconds {1}", cacheKey, slidingExpiryWindow.TotalSeconds));
			}
		}
	}


	public void AddToPerRequestCache(string cacheKey, object dataToAdd)
	{
		// memory cache does not have a per request concept nor does it need to since all cache nodes should be in sync
		// You could simulate this in code with a dependency on the ASP.NET framework and its inbuilt request
		// objects but we wont do that here. We simply add it into the cache for 1 second.
		// Its hacky but this behaviour will be specific to the scenario at hand.
		Add(cacheKey, TimeSpan.FromSeconds(1), dataToAdd);
	}


	public CacheSetting CacheType
	{
		get { return CacheSetting.Memory; }
	}
}
