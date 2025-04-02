// Forked from http://weblogs.asp.net/pglavich/archive/2011/07/04/cacheadapter-v2-now-with-memcached-support.aspx
// https://bitbucket.org/glav/cacheadapter
// License: Ms-Pl http://www.opensource.org/licenses/MS-PL
// Forked on 2011-08-03 by 
// Changed namespaces and modified for use in mojoPortal

namespace mojoPortal.Web.Caching;

public static class CacheManager
{
	private static ICacheProvider _cacheProvider;
	private static ICache _cache;

	public static ICacheProvider Cache => _cacheProvider;

	static CacheManager() => PreStartInitialise();

	public static void PreStartInitialise()
	{
		_cache = WebConfigSettings.CacheProviderType switch
		{
			// This was for older than .NET 3.5
			//CacheTypes.AppFabricCache => new AppFabricCacheAdapter(),
			CacheTypes.MemoryCache => new MemoryCacheAdapter(),
			_ => new MemoryCacheAdapter(),
		};
		_cacheProvider = new CacheProvider(_cache);
	}
}
