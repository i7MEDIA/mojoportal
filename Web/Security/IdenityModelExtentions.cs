using tokens = Microsoft.IdentityModel.Tokens;
using identityModel = IdentityModel.Jwk;
using System;

namespace mojoPortal.Web.Security;

public static class IdenityModelExtentions
{
	public static tokens.JsonWebKey ToJsonWebKey(this identityModel.JsonWebKey webKey)
	{
		if (webKey is null)
		{
			throw new ArgumentNullException(nameof(webKey));
		}

		return new Microsoft.IdentityModel.Tokens.JsonWebKey
		{
			Alg = webKey.Alg,
			Crv = webKey.Crv,
			D = webKey.D,
			DP = webKey.DP,
			DQ = webKey.DQ,
			E = webKey.E,
			K = webKey.K,
			Kid = webKey.Kid,
			Kty = webKey.Kty,
			N = webKey.N,
			P = webKey.P,
			Q = webKey.Q,
			QI = webKey.QI,
			Use = webKey.Use,
			X = webKey.X,
			Y = webKey.Y,
			X5t = webKey.X5t,
			X5tS256 = webKey.X5tS256,
			X5u = webKey.X5u,
		};
	}
}
