using System;
using System.Collections.Specialized;
using System.Web;

namespace mojoPortal.Web.Framework;

public static class CookieHelper
{
	public static bool UserHasCartCookie(Guid storeGuid)
	{
		if (storeGuid == Guid.Empty)
		{
			return false;
		}

		var cartKey = "cart" + storeGuid.ToString();

		return CookieExists(cartKey);
	}


	public static string GetCartKey(Guid storeGuid)
	{
		return "cart" + storeGuid.ToString();
	}


	public static void SetCartCookie(Guid storeGuid, Guid cartGuid)
	{
		if (storeGuid != Guid.Empty && cartGuid != Guid.Empty)
		{
			var cartKey = "cart" + storeGuid.ToString();

			//TODO: encrypt, sign cart cookie?
			SetPersistentCookie(cartKey, cartGuid.ToString());
		}
	}


	public static void ClearCartCookie(Guid storeGuid)
	{
		if (storeGuid != Guid.Empty)
		{
			var cartKey = GetCartKey(storeGuid);

			SetPersistentCookie(cartKey, string.Empty);
		}
	}


	public static string GetCartCookie(Guid storeGuid)
	{
		if (storeGuid == Guid.Empty)
		{
			return string.Empty;
		}

		var cartKey = GetCartKey(storeGuid);

		// TODO: decrypt and verify cart cookie?
		return GetCookieValue(cartKey);
	}


	public static bool CookieExists(string cookieName)
	{
		if (HttpContext.Current == null || string.IsNullOrEmpty(cookieName))
		{
			return false;
		}

		return HttpContext.Current.Request.Cookies[cookieName] != null;
	}


	public static string GetCookieValue(string cookieName)
	{
		if (
			HttpContext.Current == null ||
			string.IsNullOrEmpty(cookieName) ||
			HttpContext.Current.Request.Cookies[cookieName] == null
		)
		{
			return string.Empty;
		}

		return HttpContext.Current.Request.Cookies.Get(cookieName).Value;
	}


	public static void SetPersistentCookie(string cookieName, string cookieValue)
	{
		if (string.IsNullOrEmpty(cookieName) || string.IsNullOrEmpty(cookieValue))
		{
			return;
		}

		if (HttpContext.Current != null)
		{
			var cookie = new HttpCookie(cookieName, cookieValue)
			{
				HttpOnly = true,
				Expires = DateTime.Now.AddYears(1)
			};

			HttpContext.Current.Response.Cookies.Add(cookie);
		}
	}


	public static void SetCookie(string cookieName, string cookieValue, bool persistent)
	{
		if (string.IsNullOrEmpty(cookieName) || string.IsNullOrEmpty(cookieValue))
		{
			return;
		}

		if (HttpContext.Current != null)
		{
			if (persistent)
			{
				SetPersistentCookie(cookieName, cookieValue);
			}
			else
			{
				SetCookie(cookieName, cookieValue);
			}
		}
	}


	public static void SetCookie(string cookieName, string cookieValue)
	{
		if (string.IsNullOrEmpty(cookieName) || string.IsNullOrEmpty(cookieValue))
		{
			return;
		}

		if (HttpContext.Current != null)
		{
			HttpCookie cookie = new HttpCookie(cookieName, cookieValue)
			{
				HttpOnly = true
			};

			HttpContext.Current.Response.Cookies.Add(cookie);
		}
	}


	public static void ExpireCookie(string cookieName)
	{
		if (string.IsNullOrEmpty(cookieName))
		{
			return;
		}

		if (HttpContext.Current != null)
		{
			var cookie = new HttpCookie(cookieName, string.Empty)
			{
				HttpOnly = true,
				Expires = DateTime.Now.AddYears(-5)
			};

			HttpContext.Current.Response.Cookies.Add(cookie);
		}
	}


	public static void SetSecureCookie(string cookieName, string cookieValue)
	{
		if (
			string.IsNullOrEmpty(cookieName) ||
			string.IsNullOrEmpty(cookieValue) ||
			HttpContext.Current == null
		)
		{
			return;
		}

		var cookie = new HttpCookie(cookieName, cookieValue)
		{
			HttpOnly = true
		};

		SignAndSecureCookie(cookie, HttpContext.Current.Request.ServerVariables);
		HttpContext.Current.Response.Cookies.Add(cookie);
	}


	public static string GetSecureCookieValue(string cookieName)
	{
		if (HttpContext.Current == null || string.IsNullOrEmpty(cookieName))
		{
			return string.Empty;
		}

		var cookie = HttpContext.Current.Request.Cookies.Get(cookieName);

		if (cookie == null)
		{
			return string.Empty;
		}

		string value = DecryptAndVerifyCookie(cookie, HttpContext.Current.Request.ServerVariables);

		return value.ToString();
	}


	public static void SignAndSecureCookie(HttpCookie cookie, NameValueCollection serverVariables)
	{
		if (cookie.HasKeys)
		{
			throw (new Exception("Does not support cookies with sub keys"));
		}

		if (cookie.Expires != DateTime.MinValue) // has an expiry date
		{
			cookie.Value = CryptoHelper.SignAndSecureData([
				cookie.Value,
				serverVariables["REMOTE_ADDR"],
				cookie.Expires.ToString()
			]);
		}
		else
		{
			cookie.Value = CryptoHelper.SignAndSecureData([cookie.Value, serverVariables["REMOTE_ADDR"]]);
		}
	}


	public static string DecryptAndVerifyCookie(HttpCookie cookie, NameValueCollection serverVariables)
	{
		if (cookie == null)
		{
			return null;
		}

		if (!CryptoHelper.DecryptAndVerifyData(cookie.Value, out string[] values))
		{
			return null;
		}

		if (values.Length == 3) // 3 values, has an expiry date
		{
			var expireDate = DateTime.Parse(values[2]);

			if (expireDate < DateTime.Now)
			{
				return null;
			}
		}

		if (values[1] != serverVariables["REMOTE_ADDR"])
		{
			return null;
		}

		return values[0];
	}
}
