using log4net;
using SuperFlexiBusiness;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SuperFlexiUI.Components;

public class SuperFlexiCache
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(ClassBuilder));

	public static readonly ConcurrentDictionary<string, Type> ClassCache = [];
	public static readonly ConcurrentDictionary<Guid, List<Field>> FieldCache = [];
	public static readonly ConcurrentDictionary<Guid, SearchDef> SearchDefCache = [];


	static SuperFlexiCache()
	{
		mojoPortal.Web.Global.CacheServicesContainer.Register(typeof(SuperFlexiCache), [ClearAllCaches]);
	}


	public static void ClearFieldCache(Guid fieldDefinitionGuid)
	{
		FieldCache.TryRemove(fieldDefinitionGuid, out _);
		ClassCache.TryRemove(ClassBuilder.GetClassName(fieldDefinitionGuid), out _);
		SearchDefCache.TryRemove(fieldDefinitionGuid, out _);
	}


	public static void ClearClassCache(Guid fieldDefinitionGuid)
	{
		ClassCache.TryRemove(ClassBuilder.GetClassName(fieldDefinitionGuid), out _);
	}


	public static void ClearSearchDefCache(Guid fieldDefinitionGuid)
	{
		SearchDefCache.TryRemove(fieldDefinitionGuid, out _);
	}


	public static void ClearAllCaches()
	{
		FieldCache.Clear();
		ClassCache.Clear();
		SearchDefCache.Clear();
	}


	public static List<Field> GetFields(Guid fieldDefinitionGuid)
	{
		if (FieldCache.TryGetValue(fieldDefinitionGuid, out var fields))
		{
			return fields;
		}
		else
		{
			var fieldsFromDb = Field.GetAllForDefinition(fieldDefinitionGuid);

			if (fieldsFromDb != null)
			{
				FieldCache.TryAdd(fieldDefinitionGuid, fieldsFromDb);

				return fieldsFromDb;
			}
			else
			{
				_log.Error($"failed to get fields for {fieldDefinitionGuid}");

				throw new InvalidOperationException($"failed to get fields for [FieldDefinitionGuid: {fieldDefinitionGuid}]");
			}
		}
	}


	public static SearchDef GetSearchDef(Guid fieldDefinitionGuid)
	{
		if (SearchDefCache.TryGetValue(fieldDefinitionGuid, out var searchDef))
		{
			return searchDef;
		}
		else
		{
			var searchDefFromDb = SearchDef.GetByFieldDefinition(fieldDefinitionGuid);

			if (searchDefFromDb != null)
			{
				SearchDefCache.TryAdd(fieldDefinitionGuid, searchDefFromDb);

				return searchDefFromDb;
			}
			else
			{
				_log.Error($"failed to get search definition for {fieldDefinitionGuid}");

				throw new InvalidOperationException($"failed to get search definition for [FieldDefinitionGuid: {fieldDefinitionGuid}]");
			}
		}
	}


	//public static List<ItemWithValues> GetModuleItemsWithValuesFromCache(ModuleConfiguration config)
	//{
	//	var cacheKey = $"SuperFlexi_ModuleItemsWithValues_{config.Module.ModuleGuid:N}";
	//	try
	//	{
	//		var iwv = CacheManager.Cache.Get<Type>(cacheKey, DateTime.Now.AddDays(7), () =>
	//		{
	//			// This is the anonymous function which gets called if the data is not in the cache.
	//			// This method is executed and whatever is returned, is added to the cache with the passed in expiry time.
	//			return ItemWithValues.GetListForModule(
	//					config.Module.ModuleGuid,
	//					config.FieldDefinitionGuid,
	//					out totalPages,
	//					out totalRows,
	//					pageNumber,
	//					pageSize,
	//					searchTerm,
	//					searchField,
	//					Config.DescendingSort
	//				);
	//		});

	//		return iwv;
	//	}
	//	catch (Exception ex)
	//	{
	//		log.Error($"failed to get {cacheKey} from cache so loading it directly", ex);
	//		return CreateSolutionClass();
	//	}
	//}


	//private List<ItemWithValues> GetDefinitionItemsWithValues(
	//		Guid defGuid,
	//		Guid siteGuid,
	//		out int totalPages,
	//		out int totalRows,
	//		int pageNumber = 1,
	//		int pageSize = 20,
	//		string searchTerm = "",
	//		string searchField = "",
	//		bool descending = false)
	//{

	//	if (config.IsGlobalView)
	//	{
	//		return ItemWithValues.GetListForDefinition(
	//			Config.FieldDefinitionGuid,
	//			siteSettings.SiteGuid,
	//			out totalPages,
	//			out totalRows,
	//			pageNumber,
	//			pageSize,
	//			searchTerm,
	//			searchField,
	//			Config.DescendingSort
	//		);
	//	}
	//	else
	//	{ }
	//}
}
