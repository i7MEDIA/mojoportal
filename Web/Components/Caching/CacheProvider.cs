// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by 
// Changed namespaces and modified for easier use in mojoPortal
//
// Change history for this file since original fork:

using log4net;
using System;

namespace mojoPortal.Web.Caching;

/// <summary>
/// This class acts as a cache provider that will attempt to retrieve items from a cache, and if they do not exist,
/// execute the passed in delegate to perform a data retrieval, then place the item into the cache before returning it.
/// Subsequent accesses will get the data from the cache until it expires.
/// </summary>
public class CacheProvider(ICache cache) : ICacheProvider
{
	private readonly ICache _cache = cache;
	private static readonly ILog log = LogManager.GetLogger(typeof(CacheProvider));
	private static readonly bool debugLog = WebConfigSettings.LogCacheActivity;


	#region ICacheProvider Members

	public object GetObject(string cacheKey)
	{
		return _cache.GetObject(cacheKey);
	}


	public T GetOrSetItem<T>(string cacheKey, Func<T> fetch, DateTime? expires = null)
	{
		var cachedItem = _cache.GetObject(cacheKey);

		if (cachedItem is null)
		{
			cachedItem = fetch();

			_cache.Add(cacheKey, expires ?? DateTime.Now.AddDays(1), cachedItem);
		}

		return (T)cachedItem;
	}


	public T Get<T>(
		string cacheKey,
		DateTime expiryDate,
		GetDataToCacheDelegate<T> getData,
		bool addToPerRequestCache = false
	) where T : class
	{
		//Get data from cache
		T data = _cache.Get<T>(cacheKey);

		if (data == null)
		{
			//get data from source
			data = getData();

			//only add non null data to the cache.
			if (data != null)
			{
				if (addToPerRequestCache)
				{
					_cache.AddToPerRequestCache(cacheKey, data);
				}
				else
				{
					_cache.Add(cacheKey, expiryDate, data);

					if (debugLog)
					{
						log.Info($"Adding item [{cacheKey}] to cache with expiry date/time of [{expiryDate:dd/MM/yyyy hh:mm:ss}].");
					}
				}
			}
		}
		else
		{
			if (debugLog)
			{
				log.Info($"Retrieving item [{cacheKey}] from cache.");
			}
		}

		return data;
	}


	public T Get<T>(
		string cacheKey,
		TimeSpan slidingExpiryWindow,
		GetDataToCacheDelegate<T> getData,
		bool addToPerRequestCache = false
	) where T : class
	{
		//Get data from cache
		T data = _cache.Get<T>(cacheKey);

		if (data == null)
		{
			//get data from source
			data = getData();

			//only add non null data to the cache.
			if (data != null)
			{
				if (addToPerRequestCache)
				{
					_cache.AddToPerRequestCache(cacheKey, data);
				}
				else
				{
					_cache.Add(cacheKey, slidingExpiryWindow, data);

					if (debugLog)
					{
						log.Info($"Adding item [{cacheKey}] to cache with sliding sliding expiry window in seconds [{slidingExpiryWindow.TotalSeconds}].");
					}
				}
			}
		}
		else
		{
			if (debugLog)
			{
				log.Info(string.Format("Retrieving item [{0}] from cache.", cacheKey));
			}
		}

		return data;
	}


	public void InvalidateCacheItem(string cacheKey)
	{
		_cache.InvalidateCacheItem(cacheKey);

		if (debugLog)
		{
			log.Info(string.Format("Invalidated cache item [{0}].", cacheKey));
		}
	}

	#endregion


	public void Add(string cacheKey, DateTime absoluteExpiryDate, object dataToAdd)
	{
		_cache.Add(cacheKey, absoluteExpiryDate, dataToAdd);

		if (debugLog)
		{
			log.Info($"Added item [{cacheKey}] to the cache with absoluteExpiryDate.");
		}
	}


	public void Add(string cacheKey, TimeSpan slidingExpiryWindow, object dataToAdd)
	{
		_cache.Add(cacheKey, slidingExpiryWindow, dataToAdd);

		if (debugLog)
		{
			log.Info($"Added item [{cacheKey}] to the cache with slidingExpiryWindow.");
		}
	}


	public void AddToPerRequestCache(string cacheKey, object dataToAdd)
	{
		_cache.AddToPerRequestCache(cacheKey, dataToAdd);

		if (debugLog)
		{
			log.Info($"Added item [{cacheKey}] to the per request cache.");
		}
	}
}
