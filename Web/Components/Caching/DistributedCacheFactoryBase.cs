using System.Collections.Generic;

namespace mojoPortal.Web.Caching;

public class DistributedCacheFactoryBase
{
	public List<ServerNode> ParseConfig(string configValue)
	{
		var config = new List<ServerNode>();

		if (string.IsNullOrWhiteSpace(configValue))
		{
			return config;
		}

		var endPointList = configValue.Split(',');

		if (endPointList.Length == 0)
		{
			return config;
		}

		foreach (var endpoint in endPointList)
		{
			var endPointComponents = endpoint.Split(':');

			if (endPointComponents.Length < 2)
			{
				continue;
			}

			if (int.TryParse(endPointComponents[1], out int port))
			{
				var cacheEndpoint = new ServerNode(endPointComponents[0], port);

				config.Add(cacheEndpoint);
			}
		}

		return config;
	}
}
