// Author:					Joe Audette
// Created:				    2007-08-10
// Last Modified:			2007-08-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentAdminLinksConfiguration
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(ContentAdminLinksConfiguration));


        private Collection<ContentAdminLink> adminLinks
            = new Collection<ContentAdminLink>();

        public Collection<ContentAdminLink> AdminLinks
        {
            get
            {
                return adminLinks;
            }
        }

        public static ContentAdminLinksConfiguration GetConfig(int siteId)
        {
            ContentAdminLinksConfiguration config = null;
            string cacheKey = "ContentAdminLinksConfiguration-" + siteId.ToString();
            if (
                (HttpRuntime.Cache[cacheKey] != null)
                && (HttpRuntime.Cache[cacheKey] is ContentAdminLinksConfiguration)
            )
            {
                return (ContentAdminLinksConfiguration)HttpRuntime.Cache[cacheKey];
            }
            else
            {
                config = new ContentAdminLinksConfiguration();

                String configFolderName = "~/Setup/initialcontent/supplementaladminmenulinks";

                string pathToConfigFolder
                    = HttpContext.Current.Server.MapPath(configFolderName);

                if (!Directory.Exists(pathToConfigFolder)) return config;


                DirectoryInfo directoryInfo
                    = new DirectoryInfo(pathToConfigFolder);

                FileInfo[] files = directoryInfo.GetFiles("*.config");

                foreach (FileInfo fileInfo in files)
                {
                    XmlDocument configFile = new XmlDocument();
                    
                    configFile.Load(fileInfo.FullName);

                    ContentAdminLink.LoadLinks(
                        config,
                        configFile.DocumentElement);
                    
                }

                // now look for site specific links
                configFolderName = "~/Data/Sites/" 
                    + siteId.ToInvariantString()
                    + "/supplementaladminmenulinks";

                pathToConfigFolder
                    = HttpContext.Current.Server.MapPath(configFolderName);

                if (Directory.Exists(pathToConfigFolder))
                {
                    directoryInfo
                        = new DirectoryInfo(pathToConfigFolder);

                    files = directoryInfo.GetFiles("*.config");

                    foreach (FileInfo fileInfo in files)
                    {
                        XmlDocument configFile = new XmlDocument();
                        
                        configFile.Load(fileInfo.FullName);

                        ContentAdminLink.LoadLinks(
                            config,
                            configFile.DocumentElement);
                        

                    }
                }

                // cache can be cleared by touching Web.config
                CacheDependency cacheDependency
                    = new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config"));


                HttpRuntime.Cache.Insert(
                    cacheKey,
                    config,
                    cacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    CacheItemPriority.Default,
                    null);

                return (ContentAdminLinksConfiguration)HttpRuntime.Cache[cacheKey];


            }

        }


    }
}
