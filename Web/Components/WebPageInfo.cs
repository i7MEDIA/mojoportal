// Author:					
// Created:				    2007-04-11
// Last Modified:			2008-08-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Caching;
using log4net;
using mojoPortal.Web.Framework;


namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class WebPageInfo
    {
        private static readonly ILog log 
            = LogManager.GetLogger(typeof(WebPageInfo));

        #region Constructors

        public WebPageInfo(FileInfo webPageFile)
        {
            
            this.webPageFile = webPageFile;
        }

        #endregion

        #region Private Properties

   
        private FileInfo webPageFile;

        #endregion

        #region Public Properties

       
        #endregion

        #region Public Methods

        public bool Equals(string url)
        {
            bool result = false;
            if (HttpContext.Current != null)
            {
                //if (HttpContext.Current.Server.MapPath(url).ToLower() 
                //    == this.webPageFile.FullName.ToLower())
                //    result = true;
                try
                {
                    if (StringHelper.IsCaseInsensitiveMatch(HttpContext.Current.Server.MapPath(url), webPageFile.FullName))
                    {
                        result = true;
                    }
                }
                catch (HttpException) { }
            }

            return result;
        }

        #endregion

        #region static methods

        public static Collection<WebPageInfo> GetPhysicalPages()
        {
            return GetPhysicalPages("*.aspx");
        }

        public static Collection<WebPageInfo> GetPhysicalPages(
            string fileExtensionPattern)
        {
            Collection<WebPageInfo> physicalPages = null;
            string cachekey = "physicalwebpages" + fileExtensionPattern;

            if (
                (HttpContext.Current != null)
                && (HttpRuntime.Cache[cachekey] == null)
                )
            {
                log.Debug("couldn't find cache item " + cachekey + " creating cache item now.");

                physicalPages = LoadPhysicalPages(fileExtensionPattern);

                AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
                aggregateCacheDependency.Add(new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config")));

                DateTime absoluteExpiration = DateTime.Now.AddMinutes(WebConfigSettings.WebPageInfoCacheMinutes);
                TimeSpan slidingExpiration = TimeSpan.Zero;
                CacheItemPriority priority = CacheItemPriority.Default;
                CacheItemRemovedCallback callback = null;

                HttpRuntime.Cache.Insert(
                    cachekey,
                    physicalPages,
                    aggregateCacheDependency,
                    absoluteExpiration,
                    slidingExpiration,
                    priority,
                    callback);

            }

            physicalPages = HttpRuntime.Cache[cachekey] as Collection<WebPageInfo>;

            return physicalPages;

        }

        private static Collection<WebPageInfo> LoadPhysicalPages(
            string fileExtensionPattern)
        {
            Collection<WebPageInfo> physicalPages = new Collection<WebPageInfo>();
            if (HttpContext.Current != null)
            {
                try
                {
                    PopulatePagesCollection(
                        HttpContext.Current,
                        physicalPages,
                        fileExtensionPattern);
                }
                catch (UnauthorizedAccessException) { }
                catch (System.Security.SecurityException) { }
            }

            return physicalPages;

        }

        private static void PopulatePagesCollection(
            HttpContext httpContext,
            Collection<WebPageInfo> webPages,
            string fileExtensionPattern)
        {
            if (httpContext == null) return;

            string siteBasePath = httpContext.Server.MapPath("~/");
            DirectoryInfo rootDirectory = new DirectoryInfo(siteBasePath);
            AddPhysicalPages(webPages, rootDirectory, fileExtensionPattern);

            //  recurse through sub folders
            // how deep is too deep?
            // I don't think it should be endless
            // start with 4 levels which should satify most use cases
            // while minimizing performance issues
            DirectoryInfo[] firstLevelSubDirectories;
            try
            {
                firstLevelSubDirectories = rootDirectory.GetDirectories();

                foreach (DirectoryInfo firstLevelSubDirectory in firstLevelSubDirectories)
                {
                    try
                    {
                        AddPhysicalPages(webPages, firstLevelSubDirectory, fileExtensionPattern);
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (System.Security.SecurityException) { }
                    // catch these errors because there can be other folders in the root that are not part of mojoportal
                    // don't fail to read the folders we can read just because we can't read others
                    //http://www.mojoportal.com/ForumThreadView.aspx?thread=1875&forumid=2&ItemID=2&pageid=5&pagenumber=1#post7654
                }

                foreach (DirectoryInfo firstLevelSubDirectory in firstLevelSubDirectories)
                {
                    try
                    {
                        DirectoryInfo[] secondLevelSubDirectories
                        = firstLevelSubDirectory.GetDirectories();

                        foreach (DirectoryInfo secondLevelSubDirectory in secondLevelSubDirectories)
                        {
                            try
                            {
                                AddPhysicalPages(webPages, secondLevelSubDirectory, fileExtensionPattern);
                            }
                            catch (UnauthorizedAccessException) { }
                            catch (System.Security.SecurityException) { }
                        }

                        foreach (DirectoryInfo secondLevelSubDirectory in secondLevelSubDirectories)
                        {
                            try
                            {
                                DirectoryInfo[] thirdLevelSubDirectories
                                    = secondLevelSubDirectory.GetDirectories();

                                foreach (DirectoryInfo thirdLevelSubDirectory in thirdLevelSubDirectories)
                                {
                                    try
                                    {
                                        AddPhysicalPages(webPages, thirdLevelSubDirectory, fileExtensionPattern);
                                    }
                                    catch (UnauthorizedAccessException) { }
                                    catch (System.Security.SecurityException) { }
                                }

                                foreach (DirectoryInfo thirdLevelSubDirectory in thirdLevelSubDirectories)
                                {
                                    try
                                    {
                                        DirectoryInfo[] fourthLevelSubDirectories
                                            = thirdLevelSubDirectory.GetDirectories();

                                        foreach (DirectoryInfo fourthLevelSubDirectory in fourthLevelSubDirectories)
                                        {
                                            try
                                            {
                                                AddPhysicalPages(webPages, fourthLevelSubDirectory, fileExtensionPattern);
                                            }
                                            catch (UnauthorizedAccessException) { }
                                            catch (System.Security.SecurityException) { }
                                        }
                                    }
                                    catch (UnauthorizedAccessException) { }
                                    catch (System.Security.SecurityException) { }

                                }
                            }
                            catch (UnauthorizedAccessException) { }
                            catch (System.Security.SecurityException) { }
                        }
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (System.Security.SecurityException) { }
                }

            }
            catch (UnauthorizedAccessException) { }
            catch (System.Security.SecurityException) { }
            // catch these errors because there can be other folders in the root that are not part of mojoportal
            // don't fail to read the folders we can read just because we can't read others

            
        }

        private static void AddPhysicalPages(
            Collection<WebPageInfo> physicalPages,
            DirectoryInfo directoryInfo,
            string fileExtensionPattern)
        {
            FileInfo[] matchingFiles = directoryInfo.GetFiles(fileExtensionPattern);
            foreach (FileInfo matchingFile in matchingFiles)
            {
                WebPageInfo webPageInfo = new WebPageInfo(matchingFile);
                physicalPages.Add(webPageInfo);
            }
        }

        public static bool IsPhysicalWebPage(string url)
        {
            bool result = false;
            Collection<WebPageInfo> physicalPages = GetPhysicalPages();
            foreach (WebPageInfo webPage in physicalPages)
            {
                if (webPage.Equals(url)) result = true;
            }

            return result;
        }

        #endregion
    }
}
