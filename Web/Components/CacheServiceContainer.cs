using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace mojoPortal.Web.Components;

public class CacheServiceContainer
{
	private ConcurrentDictionary<Type, List<Action>> _cacheServices { get; } = [];


	public void Register(Type type, List<Action> actions) => _cacheServices.TryAdd(type, actions);
	public void Unregister(Type type) => _cacheServices.TryRemove(type, out var _);

	public void ClearAll()
	{
		foreach (var item in _cacheServices)
		{
			foreach (var action in item.Value)
			{
				action();
			}
		}
	}

	public static CacheServiceContainer Init() => new();
}
