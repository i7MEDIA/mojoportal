using System;
using System.Collections.Specialized;
using System.Web;

namespace mojoPortal.Web.Framework
{
    public static class CookieHelper
    {
        public static bool UserHasCartCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return false;
            string cartKey = "cart" + storeGuid.ToString();

            return CookieExists(cartKey);

        }

        public static string GetCartKey(Guid storeGuid)
        {
            return "cart" + storeGuid.ToString();
        }

        public static void SetCartCookie(Guid storeGuid, Guid cartGuid)
        {
            if ((storeGuid != Guid.Empty) && (cartGuid != Guid.Empty))
            {
                string cartKey = "cart" + storeGuid.ToString();

                // TODO encrypt, sign?

                SetPersistentCookie(cartKey, cartGuid.ToString());

            }
        }

        public static void ClearCartCookie(Guid storeGuid)
        {
            if (storeGuid != Guid.Empty)
            {
                string cartKey = GetCartKey(storeGuid);
                CookieHelper.SetPersistentCookie(cartKey, string.Empty);
            }

        }

        public static string GetCartCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return string.Empty;

            string cartKey = GetCartKey(storeGuid);

            // TODO: decrypt and verify?

            return CookieHelper.GetCookieValue(cartKey);

        }

        public static bool CookieExists(string cookieName)
        {
            if (HttpContext.Current == null) return false;
            if (String.IsNullOrEmpty(cookieName)) return false;
            return (HttpContext.Current.Request.Cookies[cookieName] != null);
        }

        public static string GetCookieValue(string cookieName)
        {
            if (HttpContext.Current == null) return String.Empty;
            if (String.IsNullOrEmpty(cookieName)) return String.Empty;
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.Cookies.Get(cookieName).Value;
        }

        public static void SetPersistentCookie(String cookieName, String cookieValue)
        {
            if (String.IsNullOrEmpty(cookieName) || String.IsNullOrEmpty(cookieValue)) return;
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void SetCookie(String cookieName, String cookieValue, bool persistent)
        {
            if (String.IsNullOrEmpty(cookieName) || String.IsNullOrEmpty(cookieValue)) return;
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

        public static void SetCookie(String cookieName, String cookieValue)
        {
            if (String.IsNullOrEmpty(cookieName) || String.IsNullOrEmpty(cookieValue)) return;
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
                cookie.HttpOnly = true;
                //cookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void ExpireCookie(String cookieName)
        {
            if (String.IsNullOrEmpty(cookieName)) return;
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = new HttpCookie(cookieName, string.Empty);
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddYears(-5);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void SetSecureCookie(String cookieName, String cookieValue)
        {
            if (String.IsNullOrEmpty(cookieName) || String.IsNullOrEmpty(cookieValue)) return;
            if (HttpContext.Current == null) return;
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.HttpOnly = true;
            //cookie.Expires = DateTime.Now.AddYears(1);
            //CryptoHelper cryptoHelper = new CryptoHelper();
            SignAndSecureCookie(
                    cookie, 
                    HttpContext.Current.Request.ServerVariables);
                
            HttpContext.Current.Response.Cookies.Add(cookie);
            
        }

        public static string GetSecureCookieValue(string cookieName)
        {
            if (HttpContext.Current == null) return String.Empty;
            if (String.IsNullOrEmpty(cookieName)) return String.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookieName);
            if (cookie == null) return string.Empty;

            //CryptoHelper cryptoHelper = new CryptoHelper();

            string value = DecryptAndVerifyCookie(
                cookie,
                HttpContext.Current.Request.ServerVariables);

            return value.ToString();

        }

        public static void SignAndSecureCookie(
            HttpCookie cookie, 
            NameValueCollection 
            serverVariables)
        {
            if (cookie.HasKeys)
                throw (new Exception("Does not support cookies with sub keys"));

            if (cookie.Expires != DateTime.MinValue) // has an expiry date
            {
                cookie.Value = CryptoHelper.SignAndSecureData(new string[] 
					{
						cookie.Value, 
						serverVariables["REMOTE_ADDR"], 
						cookie.Expires.ToString()});
            }
            else
            {
                cookie.Value = CryptoHelper.SignAndSecureData(
                    new string[] { cookie.Value, serverVariables["REMOTE_ADDR"] });
            }
        }

        public static string DecryptAndVerifyCookie(
            HttpCookie cookie, 
            NameValueCollection serverVariables)
        {
            if (cookie == null) return null;

            string[] values;

            if (!CryptoHelper.DecryptAndVerifyData(cookie.Value, out values))
                return null;

            if (values.Length == 3) // 3 values, has an expiry date
            {
                DateTime expireDate = DateTime.Parse(values[2]);
                if (expireDate < DateTime.Now)
                    return null;
            }

            if (values[1] != serverVariables["REMOTE_ADDR"])
                return null;

            return values[0];
        }
    }
}
