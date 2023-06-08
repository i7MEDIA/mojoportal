
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
//using Brettle.Web.NeatHtml;
using log4net;

namespace mojoPortal.Web.Framework
{
    public static class SecurityHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SecurityHelper));

        //^[/.].*$
        //public const string RegexRelativeImageUrlPatern = @"^[/.].*$";
        public const string RegexRelativeImageUrlPatern = @"^/.*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";
        public const string RegexAnyImageUrlPatern = @"^.*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";


        public const string RegexAnyHttpOrHttpsUrl = @"^(http|https)://([^\s]+)/?";

        //** Email Validation
        /// <summary>
        /// a regular expression for validating email addresses, efficient but not completely RFC 822 compliant
        /// </summary>
        public const string RegexEmailValidationPattern
            = @"^([0-9a-zA-Z](['-.\w]*[_0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$|^$";

        // 2014-03-18 this expression hangs with patriciarichards5395@yahoo.comgetgoing
        // but is still the best one I've found because the others fail to accept foo+test@foo.com
        // @"^([0-9a-zA-Z](['-.\w]*[_0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";

        // 2014-03-18 tried this one http://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp
        // @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
        // but it doesn't accept + signs like foo+test@gmail.com

        // http://stackoverflow.com/questions/201323/using-a-regular-expression-to-validate-an-email-address
        // ^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$
        // this one also fails to accept foo+test@foo.com

        /// <summary>
        /// it is better to use this method than to directly use the constant above becuase this allows you to override the experession if needed
        /// to solve a problem
        /// </summary>
        /// <returns></returns>
        public static string GetEmailRegexExpression()
        {
            string overrideRegex = Core.Configuration.ConfigHelper.GetStringProperty("CustomEmailRegex", string.Empty);
            if (overrideRegex.Length > 0) { return overrideRegex; }
            

            return RegexEmailValidationPattern;
        }


        public static bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email)) { return false; }

            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException) { }

            return false;
        }
                                                          

        // this one I modified from the one I was using for Register.aspx. I modified it to accept apostrophe
        //^([0-9a-zA-Z](['-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$

        // this is the one I was using on Secure/Register.aspx 
        //^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$
        // this is the one I was using on ManageUsers.aspx                  
        //^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$

        // this one allows apostrophes which are valid according to the spec
        //^(['_a-z0-9-]+)(\.['_a-z0-9-]+)*@([a-z0-9-]+)(\.[a-z0-9-]+)*(\.[a-z]{2,5})$

        //from http://www.regular-expressions.info/email.html, this is supposedly compliant with the rfc
        //(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])

        //***

        // /(\.|\/)(zip|gif|jpe?g|png)$/i

        public static string GetRegexValidationForAllowedExtensions(string pipeSeparatedExtensions)
        {
            StringBuilder regex = new StringBuilder();

            // (([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG); *)*(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG))?$

            regex.Append("(([^.;]*[.])+(");

            List<string> allowedExtensions = StringHelper.SplitOnPipes(pipeSeparatedExtensions);
            string pipe = string.Empty;
            foreach (string ext in allowedExtensions)
            {
                regex.Append(pipe + ext.Replace(".",string.Empty));
                pipe = "|";
                regex.Append(pipe + ext.Replace(".", string.Empty).ToUpper());
            }

            regex.Append("); *)*(([^.;]*[.])+(");

            pipe = string.Empty;
            foreach (string ext in allowedExtensions)
            {
                regex.Append(pipe + ext.Replace(".", string.Empty));
                pipe = "|";
                regex.Append(pipe + ext.Replace(".", string.Empty).ToUpper());
            }

            regex.Append("))?$");

            return regex.ToString();


        }

        public static string GetRegexValidationForAllowedExtensionsJqueryFileUploader(string pipeSeparatedExtensions)
        {
            StringBuilder regex = new StringBuilder();

            // @"/(\.|\/)(gif|jpe?g|png)$/i"

            regex.Append(@"/(\.|\/)(");

            List<string> allowedExtensions = StringHelper.SplitOnPipes(pipeSeparatedExtensions);
            string pipe = string.Empty;
            foreach (string ext in allowedExtensions)
            {
                regex.Append(pipe + ext.Replace(".", string.Empty));
                pipe = "|";
                regex.Append(pipe + ext.Replace(".", string.Empty).ToUpper());
            }

            //regex.Append("); *)*(([^.;]*[.])+(");

            //pipe = string.Empty;
            //foreach (string ext in allowedExtensions)
            //{
            //    regex.Append(pipe + ext.Replace(".", string.Empty));
            //    pipe = "|";
            //    regex.Append(pipe + ext.Replace(".", string.Empty).ToUpper());
            //}

            regex.Append(")$/i");

            return regex.ToString();


        }

        //http://www.codeproject.com/KB/aspnet/LengthValidation.aspx
        public static string GetMaxLengthRegexValidationExpression(int length)
        {
            return "[\\s\\S]{0," + length.ToInvariantString() + "}";
        }


        public static string PreventCrossSiteScripting(String html)
        {
            
            String errorHeader = ResourceHelper.GetMessageTemplate("NeatHtmlValidationErrorHeader.config");
            return PreventCrossSiteScripting(html, errorHeader);

        }

        public static string PreventCrossSiteScripting(String html, String errorHeader)
        {
           
            return PreventCrossSiteScripting(html, errorHeader, false);
           
        }

        public static string PreventCrossSiteScripting(String html, String errorHeader, bool removeMarkupOnFailure)
        {
            
            try
            {
                XssFilter filter = GetXssFilter();

                if (filter == null)
                {
                    log.Info("XssFilter was null");
                    return html.Replace("script", "s cript");
                }

            
                return filter.FilterFragment(html);
            }
            catch (Exception ex)
            {
                if (removeMarkupOnFailure)
                {
                    return String.Format(@"<span style=""color: #ff0000;"">{0}</span><br />{1}", errorHeader,
                                         HttpUtility.HtmlEncode(RemoveMarkup(html)));
                }
                else
                {
                    return String.Format(@"<span style=""color: #ff0000;"">{0}{1}</span>:<br />{2}", errorHeader,
                                         HttpUtility.HtmlEncode(ex.Message), HttpUtility.HtmlEncode(html));
                }
            }
        }

        

        public static string SanitizeHtml(String html)
        {
           
            try
            {
                XssFilter filter = GetXssFilter();

                if (filter == null)
                {
                    log.Info("XssFilter was null");
                    //return html.Replace("script", "s cript");
                    return RemoveMarkup(html);
                }

            
                return filter.FilterFragment(html);
            }
            catch (Exception)
            {
                
               return RemoveMarkup(html);
                
            }
        }

        private static XssFilter GetXssFilter()
        {
            if (HttpContext.Current == null) return null;

            string key = "xssfilter";

            if (HttpContext.Current.Items[key] != null)
            {
                return (XssFilter)HttpContext.Current.Items[key];
            }
            else
            {
                string schemaFolder = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/NeatHtml/schema");
                string schemaFile = Path.Combine(schemaFolder, "NeatHtml.xsd");

                XssFilter filter = XssFilter.GetForSchema(schemaFile);

                HttpContext.Current.Items[key] = filter;

                return filter;  
            }

           
        }

        


        //private static XssFilter GetXssFilter()
        //{

        //    string cachekey = "xssfilter";

           
        //    if (HttpRuntime.Cache[cachekey] == null)
        //    {
        //        int cacheTimeout = 120;

        //        RefreshSchemaFilterCache(cachekey, cacheTimeout);
        //    }

        //    // Return object from cache, or null if it is not there for some reason
        //    return HttpRuntime.Cache[cachekey] as XssFilter;
        //}

        //private static void RefreshSchemaFilterCache(String cacheKey, int cacheTimeout)
        //{
        //    if (HttpContext.Current == null) return;

        //    string schemaFolder = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/NeatHtml/schema");
        //    string schemaFile = Path.Combine(schemaFolder, "NeatHtml.xsd");

        //    XssFilter filter = XssFilter.GetForSchema(schemaFile);

        //    CacheDependency cacheDependency = new CacheDependency(schemaFile);

        //    DateTime absoluteExpiration = DateTime.Now.AddSeconds(cacheTimeout);
        //    TimeSpan slidingExpiration = TimeSpan.Zero;
        //    CacheItemPriority priority = CacheItemPriority.Default;
        //    CacheItemRemovedCallback callback = null;

        //    HttpRuntime.Cache.Insert(
        //        cacheKey,
        //        filter,
        //        cacheDependency,
        //        absoluteExpiration,
        //        slidingExpiration,
        //        priority,
        //        callback);
        //}

       


        public static string RemoveMarkup(string text)
        {
            if (string.IsNullOrEmpty(text)) { return text; }
            text = text.Replace("javascript", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
            text = Regex.Replace(text, @"&nbsp;", " ", RegexOptions.IgnoreCase);
            //text = Regex.Replace(text, @"javascript", " ", RegexOptions.IgnoreCase);
            //text = Regex.Replace(text, @"{", "", RegexOptions.IgnoreCase);
            //text = Regex.Replace(text, @"}", "", RegexOptions.IgnoreCase);
            return Regex.Replace(text.Replace("  ", " "), @"<.+?>", "", RegexOptions.Singleline);
        }

        public static string RemoveAngleBrackets(string text)
        {
            if (string.IsNullOrEmpty(text)) { return text; }

            return text.Replace("<", string.Empty).Replace(">", string.Empty);
        }


        public static string GetRandomASPNET20machinekey()
        {
            StringBuilder machinekey = new StringBuilder();
            string key64byte = GetRandomKey(64);
            string key32byte = GetRandomKey(32);
            machinekey.Append("<machineKey \n");
            machinekey.Append("validationKey=\"" + key64byte + "\"\n");
            machinekey.Append("decryptionKey=\"" + key32byte + "\"\n");
            machinekey.Append("validation=\"SHA1\" decryption=\"AES\"\n");
            machinekey.Append("/>\n");
            return machinekey.ToString();
        }

        

        public static string GetRandomKey(int bytelength)
        {
            byte[] buff = new byte[bytelength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
			StringBuilder sb = new(bytelength * 2);
            for (int i = 0; i < buff.Length; i++)
            {
                sb.Append(string.Format("{0:X2}", buff[i]));
            }
            return sb.ToString();
        }

        public static void DisableBrowserCache()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cache.SetExpires(new DateTime(1942, 12, 30, 12, 0, 0, DateTimeKind.Utc));
                HttpContext.Current.Response.Cache.SetNoStore();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");

            }
        }

        public static void DisableDownloadCache()
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));

            // no-store makes firefox reload page
            // no-cache makes firefox reload page only over SSL
            // IE will fail when downloading a file over SSL if no-store or no-cache is set
            HttpBrowserCapabilities oBrowser 
                = HttpContext.Current.Request.Browser;

            if (!oBrowser.Browser.ToLower().Contains("ie"))
            {
                HttpContext.Current.Response.Cache.SetNoStore();
            }
            
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // private cache allows IE to download over SSL with no-store set. 
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Private);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");

        }

        /// <summary>
        /// Gets the current Trust Level
        /// </summary>
        /// <returns></returns>
        public static AspNetHostingPermissionLevel GetCurrentTrustLevel()
        {
            foreach (AspNetHostingPermissionLevel trustLevel in
                    new AspNetHostingPermissionLevel[] {
                AspNetHostingPermissionLevel.Unrestricted,
                AspNetHostingPermissionLevel.High,
                AspNetHostingPermissionLevel.Medium,
                AspNetHostingPermissionLevel.Low,
                AspNetHostingPermissionLevel.Minimal 
            })
            {
                try
                {
                    new AspNetHostingPermission(trustLevel).Demand();
                }
                catch (System.Security.SecurityException)
                {
                    continue;
                }

                return trustLevel;
            }

            return AspNetHostingPermissionLevel.None;
        }


        

    }
}
