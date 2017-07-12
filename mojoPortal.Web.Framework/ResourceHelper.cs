// Author:					
// Created:				2007-04-29
// Last Modified:			2009-06-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using log4net;

namespace mojoPortal.Web.Framework
{
    
    public static class ResourceHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResourceHelper));

        #region Help System

        /// <summary>
        /// when posssible you should use SiteUtils.GetDefaultUICulture() in preference to this method
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetDefaultUICulture()
        {
            try
            {
                
                // reading config sections not allowed in Medium trust so catch SecurityException
                GlobalizationSection globalizationSection = (GlobalizationSection)ConfigurationManager.GetSection("system.web/globalization");
                if (globalizationSection != null)
                {
                    if (globalizationSection.Culture.Contains(":"))
                    {
                        String cultureString;
                        // this is tryinbg to get UiCulture and expects this format
                        // <globalization culture="auto:en-US" uiCulture="auto:en-US" requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="iso-8859-15" />
                        cultureString = globalizationSection.Culture.Substring(globalizationSection.Culture.LastIndexOf(":") + 1, (globalizationSection.Culture.Length - (globalizationSection.Culture.LastIndexOf(":") + 1)));
                        return new CultureInfo(cultureString);
                    }
                    else
                    {
                        return new CultureInfo(globalizationSection.Culture);
                    }
                }
            }
            catch (ArgumentException ex) 
            {
                log.Info("ResourceHelper swallowed exception", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                log.Info("ResourceHelper swallowed exception", ex);
            }

            return new CultureInfo("en-US"); //this is the default culture in Web.config
        }

        /// <summary>
        /// when posssible you should use SiteUtils.GetDefaultCulture() in preference to this method
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetDefaultCulture()
        {
            try
            {

                // reading config sections not allowed in Medium trust so catch SecurityException
                GlobalizationSection globalizationSection = (GlobalizationSection)ConfigurationManager.GetSection("system.web/globalization");
                if (globalizationSection != null)
                {
                    if (globalizationSection.Culture.Contains(":"))
                    {
                        String cultureString;
                        // this is trying to get culture not uiculture and expects this format 
                        // <globalization culture="auto:en-US" uiCulture="auto:en-US" requestEncoding="utf-8" responseEncoding="utf-8" fileEncoding="iso-8859-15" />
                        cultureString = globalizationSection.Culture.Substring(globalizationSection.Culture.IndexOf(":") + 1, (globalizationSection.Culture.Length - (globalizationSection.Culture.IndexOf(":") + 1)));
                        return new CultureInfo(cultureString);
                    }
                    else
                    {
                        return new CultureInfo(globalizationSection.Culture);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                log.Info("ResourceHelper swallowed exception", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                log.Info("ResourceHelper swallowed exception", ex);
            }

            return new CultureInfo("en-US"); //this is the default culture in Web.config
        }

        /// <summary>
        /// Gets a CultureInfo object suitable for parsing or formatting a specific currency
        /// </summary>
        /// <param name="currencyISOCode"></param>
        /// <returns></returns>
        public static CultureInfo GetCurrencyCulture(string currencyISOCode)
        {
            if (string.IsNullOrEmpty(currencyISOCode)) { return ResourceHelper.GetDefaultCulture(); }

            //this helper caches the lookup list
            CultureInfo currencyCulture = CurrencyHelper.CultureInfoFromCurrencyISO(currencyISOCode);
            if (currencyCulture != null) { return currencyCulture; }

            // this is based on web.config globalization section, en-US
            return ResourceHelper.GetDefaultCulture();

        }

        public static string GetHelpFileText(string helpKey)
        {
            if (String.IsNullOrEmpty(helpKey)) return String.Empty;

            string fileExtension = ".config";
            string helpFolder = GetHelpFolder();
            string helpFile;
            helpFile = GetFullResourceFilePath(CultureInfo.CurrentUICulture, helpFolder, helpKey + fileExtension);

            string message = String.Empty;
            if (File.Exists(helpFile))
            {
                FileInfo file = new FileInfo(helpFile);
                StreamReader sr = file.OpenText();
                message = sr.ReadToEnd();
                sr.Close();
            }
            return message;
        }

        public static void SetHelpFileText(String helpKey, String helpText)
        {
            if (String.IsNullOrEmpty(helpKey) || String.IsNullOrEmpty(helpText)) return;

            string culture = CultureInfo.CurrentUICulture.Name;
            string fileExtension = ".config";
            string helpFolder = GetHelpFolder();
            string helpFile;
            helpFile = helpFolder + culture + "-" + helpKey + fileExtension;

            StreamWriter streamWriter = File.CreateText(helpFile);
            streamWriter.Write(helpText);
            streamWriter.Close();
        }

        public static String GetHelpFolder()
        {
            if (HttpContext.Current == null) return String.Empty;
            return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot()
                    + "/Data/HelpFiles") + Path.DirectorySeparatorChar;
        }

        public static string GetFullResourceFilePath(CultureInfo cultureInfo, string folder, string filename)
        {
            string path = GetResourceFilePath(cultureInfo, folder, filename);
            return
                string.IsNullOrEmpty(path) ?
                string.Format("{0}{1}-{2}", folder, "en-US" /* or using GetDefaultCulture().Name here? */, filename) :
                path;
        }

        public static string GetResourceFilePath(CultureInfo curltureInfo, string folder, string filename)
        {
            if (curltureInfo == null || curltureInfo.LCID.Equals(CultureInfo.InvariantCulture.LCID))
                return string.Empty;

            string path = string.Format("{0}{1}-{2}", folder, curltureInfo.Name, filename);

            if (File.Exists(path))
            {
                return path;
            }

            // default to file most likely to exist
            path = string.Format("{0}{1}-{2}", folder, "en-US", filename);
            return path;

            //return File.Exists(path) ? path : GetResourceFilePath(curltureInfo.Parent, folder, filename);
        }

        #endregion

        #region Message Templates

        public static string GetMessageTemplate(string templateFile)
        {
            return GetMessageTemplate(CultureInfo.CurrentUICulture, templateFile);
        }

        public static string GetMessageTemplate(CultureInfo cultureInfo, string templateFile)
        {
            if (String.IsNullOrEmpty(templateFile)) return String.Empty;

            string templateFolder = GetMessageTemplateFolder();
            string messageFile;

            messageFile = GetFullResourceFilePath(cultureInfo, templateFolder, templateFile);

            if (File.Exists(messageFile))
            {
                string message = File.ReadAllText(messageFile);

                return message;
            }
            else
            {
                return string.Empty;
            }
        }

        

        //public static void SetMessageTemplate(String templateKey, String templateText)
        //{
        //    if (String.IsNullOrEmpty(templateKey) || String.IsNullOrEmpty(templateText)) return;

        //    String culture = CultureInfo.CurrentCulture.Name;
        //    //String fileExtension = ".config";
        //    if (!templateKey.EndsWith(".config"))
        //    {
        //        templateKey += ".config";
        //    }

        //    string templateFolder = GetMessageTemplateFolder();
        //    string messageFile = templateFolder + culture + "-" + templateKey;

        //    StreamWriter streamWriter = File.CreateText(messageFile);
        //    streamWriter.Write(templateText);
        //    streamWriter.Close();
        //}


        public static String GetMessageTemplateFolder()
        {
            if (HttpContext.Current == null) return String.Empty;
            return HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot()
                    + "/Data/MessageTemplates") + Path.DirectorySeparatorChar;
        }


        #endregion

        public static string GetResourceString(
            string resourceFile,
            string resourceKey)
        {
            bool useConfigOverrides = false;

            return GetResourceString(resourceFile, resourceKey, useConfigOverrides);

        }

        public static string GetResourceString(
            string resourceFile,
            string resourceKey,
            bool useConfigOverrides)
        {
            return GetResourceString(
                resourceFile,
                resourceKey,
                null,
                useConfigOverrides);

        }

        public static string GetResourceString(
            string resourceFile,
            string resourceKey,
            CultureInfo culture,
            bool useConfigOverrides)
        {
            if (HttpContext.Current == null) return resourceKey;
            if (resourceFile.Length == 0) resourceFile = "Resource";

            if (ConfigurationManager.AppSettings[resourceKey] != null)
            {
                return ConfigurationManager.AppSettings[resourceKey];
            }

            try
            {
                object resource;
                if (culture != null)
                {
                    resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey, culture);
                }
                else
                {
                    resource = HttpContext.GetGlobalResourceObject(resourceFile, resourceKey);
                }

                if (resource != null) return resource.ToString();
            }
            catch { }

            return resourceKey;

        }

        public static string FormatCategoryLinkText(string category, int postCount)
        {
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                return "(" + postCount.ToString() + ") " + category;
            }

            return category + " (" + postCount.ToString() + ")";

           
        }

    }
}
