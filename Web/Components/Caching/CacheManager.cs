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
			CacheTypes.AppFabricCache => new AppFabricCacheAdapter(),
			CacheTypes.MemoryCache => new MemoryCacheAdapter(),
			_ => new MemoryCacheAdapter(),
		};

		_cacheProvider = new CacheProvider(_cache);
	}
}
