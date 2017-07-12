// Author:					
// Created:				    2009-06-29
// Last Modified:			2012-06-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Web;


namespace mojoPortal.Web.Framework
{
    public static class BrowserHelper
    {
        private const string IE = "IE";
        private const string Version6 = "6";
        private const string Version7 = "7";
        private const string Version8 = "8";
        private const string Version9 = "9";

        public static bool IsIE6()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.Browser != null))
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if ((browser.Browser == IE) && (browser.Version.StartsWith(Version6)))
                {
                    result = true;
                }
                
            }

            return result;

        }

        public static bool IsIE7()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.Browser != null))
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if ((browser.Browser == IE) && (browser.Version.StartsWith(Version7)))
                {
                    result = true;
                }

            }

            return result;

        }

        public static bool IsIE8()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.Browser != null))
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if ((browser.Browser == IE) && (browser.Version.StartsWith(Version8)))
                {
                    result = true;
                }

            }

            return result;

        }

        public static bool IsIE9()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.Browser != null))
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if ((browser.Browser == IE) && (browser.Version.StartsWith(Version9)))
                {
                    result = true;
                }

            }

            return result;

        }

        public static bool IsIE()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {

                if (HttpContext.Current.Request.UserAgent.Contains("MSIE")) 
                {
                    result = true;
                }

            }

            return result;

        }

        public static bool IsFF()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {

                if (HttpContext.Current.Request.UserAgent.Contains("Firefox"))
                {
                    result = true;
                }

            }

            return result;

        }

        public static bool IsSafari()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("safari"));
            }

            return result;

        }

        public static bool IsOpera()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("opera"));
            }

            return result;

        }

        public static bool IsIphone()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone"));
            }

            return result;

        }

        public static bool IsIpad()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("ipad"));
            }

            return result;

        }

        private static bool IsOldIOS()
        {
            // older ios does not support wysiwyg editors

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                if (HttpContext.Current.Request.UserAgent.ToLower().Contains("os 4_")) { return true; }
                if (HttpContext.Current.Request.UserAgent.ToLower().Contains("os 3_")) { return true; }
            }

           
            return false;

        }

        public static bool MobileDeviceSupportsWYSIWYG()
        {
            bool result = false;

            if (
                (IsIpad() || IsIphone())
                && (!IsOldIOS())
                )
            {
                result = true;
            }

            return result;
        }


        public static bool IsSmartPhone()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("iphone"));
                if (result) { return result; }

                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("android"));
                if (result) { return result; }
            }

            return result;

        }

        public static bool IsWindowsLiveWriter()
        {
            bool result = false;

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.UserAgent != null))
            {
                result = (HttpContext.Current.Request.UserAgent.ToLower().Contains("windows live writer"));
            }

            return result;

        }

    }
}
