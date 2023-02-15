// Author:					
// Created:				    2007-08-10
// Last Modified:			2018-10-31
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
        private static readonly ILog log = LogManager.GetLogger(typeof(ContentAdminLinksConfiguration));

		public Collection<ContentAdminLink> AdminLinks { get; } = new Collection<ContentAdminLink>();

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
                    var configFile = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);

                    ContentAdminLink.LoadLinksFromXml(
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
                        var configFile = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);

                        ContentAdminLink.LoadLinksFromXml(
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
