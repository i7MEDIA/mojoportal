// Author:					
// Created:				    2007-08-05
// Last Modified:			2009-06-21
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

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentFeatureConfiguration
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(ContentFeatureConfiguration));


        private Collection<ContentFeature> contentFeatures
            = new Collection<ContentFeature>();

        public Collection<ContentFeature> ContentFeatures
        {
            get
            {
                return contentFeatures;
            }
        }

        
        public ContentFeature GetFeature(Guid featureGuid)
        {
            foreach (ContentFeature feature in this.contentFeatures)
            {
                if (feature.FeatureGuid == featureGuid)
                {
                    return feature;
                }

            }

            return null;
        }


        public static ContentFeatureConfiguration GetConfig(string applicationFolderName)
        {
            ContentFeatureConfiguration contentFeatureConfig = null;
            //if (
            //    (HttpRuntime.Cache["contentFeatureConfig-" + applicationFolderName] != null)
            //    && (HttpRuntime.Cache["contentFeatureConfig-" + applicationFolderName] is ContentFeatureConfiguration)
            //)
            //{
            //    return (ContentFeatureConfiguration)HttpRuntime.Cache["contentFeatureConfig-" + applicationFolderName];
            //}
            //else
            //{
                contentFeatureConfig = new ContentFeatureConfiguration();

                String configFolderName = "~/Setup/applications/" 
                    + applicationFolderName + "/FeatureDefinitions/";

                string pathToConfigFolder
                    = HttpContext.Current.Server.MapPath(configFolderName);

                if (!Directory.Exists(pathToConfigFolder)) return contentFeatureConfig;


                DirectoryInfo directoryInfo
                    = new DirectoryInfo(pathToConfigFolder);

                FileInfo[] featureFiles = directoryInfo.GetFiles("*.config");

                foreach (FileInfo fileInfo in featureFiles)
                {
                    var featureConfigFile = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);

                    LoadFeature(
                        contentFeatureConfig, 
                        featureConfigFile.DocumentElement);

                }

                // cache can be cleared by touching Web.config
                //CacheDependency cacheDependency
                //    = new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config"));

               
                //HttpRuntime.Cache.Insert(
                //    "contentFeatureConfig-" + applicationFolderName,
                //    contentFeatureConfig,
                //    cacheDependency,
                //    DateTime.Now.AddYears(1),
                //    TimeSpan.Zero,
                //    CacheItemPriority.Default,
                //    null);

                //return (ContentFeatureConfiguration)HttpRuntime.Cache["contentFeatureConfig-" + applicationFolderName];

                return contentFeatureConfig;

            //}

        }

        private static void LoadFeature(
            ContentFeatureConfiguration contentFeatureConfig,
            XmlNode node
            )
        {
            ContentFeature.LoadFeature(contentFeatureConfig, node);


        }

    }
}
