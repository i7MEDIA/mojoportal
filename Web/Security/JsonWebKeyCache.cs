#nullable enable
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace mojoPortal.Web.Security;
public class JsonWebKeyCache
{
	private Dictionary<string, JsonWebKey> _Keys = [];
	private DiscoveryCache _discoveryCache;

	public JsonWebKeyCache(DiscoveryCache? discoveryCache = null)
	{
		_discoveryCache = discoveryCache ?? new DiscoveryCache(AppConfig.OAuth.Authority);
	}

	public IEnumerable<JsonWebKey> Keys => _Keys.Select(x => x.Value);


	public async Task VerifyCache(string kid)
	{
		if (_Keys.ContainsKey(kid))
		{
			return;
		}

		_Keys = (await GetJsonWebKeys()).ToDictionary(x => x.Kid);
	}


	private async Task<IEnumerable<JsonWebKey>> GetJsonWebKeys()
	{
		var disco = await _discoveryCache.GetAsync();
		var client = new HttpClient();
		var webKeySet = await client.GetJsonWebKeySetAsync(disco.JwksUri);

		return webKeySet?.KeySet?.Keys.Select(k => k.ToJsonWebKey()) ?? [];
	}
}
