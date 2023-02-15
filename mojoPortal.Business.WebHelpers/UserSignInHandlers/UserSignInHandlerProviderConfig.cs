//  Author:                     
//  Created:                    2008-08-28
//	Last Modified:              2008-08-28
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
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace mojoPortal.Business.WebHelpers.UserSignInHandlers
{
    /// <summary>
    ///  
    /// </summary>
    public class UserSignInHandlerProviderConfig
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(UserSignInHandlerProviderConfig));


        private ProviderSettingsCollection providerSettingsCollection
            = new ProviderSettingsCollection();

        public ProviderSettingsCollection Providers
        {
            get { return providerSettingsCollection; }
        }

        public static UserSignInHandlerProviderConfig GetConfig()
        {
            try
            {
                if (
                    (HttpRuntime.Cache["UserSignInHandlerProviderConfig"] != null)
                    && (HttpRuntime.Cache["UserSignInHandlerProviderConfig"] is UserSignInHandlerProviderConfig)
                )
                {
                    return (UserSignInHandlerProviderConfig)HttpRuntime.Cache["UserSignInHandlerProviderConfig"];
                }

                UserSignInHandlerProviderConfig config
                    = new UserSignInHandlerProviderConfig();

                String configFolderName = "~/Setup/ProviderConfig/usersigninhandlers/";

                string pathToConfigFolder
                    = HttpContext.Current.Server.MapPath(configFolderName);


                if (!Directory.Exists(pathToConfigFolder)) return config;

                DirectoryInfo directoryInfo
                    = new DirectoryInfo(pathToConfigFolder);

                FileInfo[] configFiles = directoryInfo.GetFiles("*.config");

                foreach (FileInfo fileInfo in configFiles)
                {
                    var configXml = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);
                    config.LoadValuesFromConfigurationXml(configXml.DocumentElement);
                }

                AggregateCacheDependency aggregateCacheDependency
                    = new AggregateCacheDependency();

                string pathToWebConfig
                    = HttpContext.Current.Server.MapPath("~/Web.config");

                aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

                System.Web.HttpRuntime.Cache.Insert(
                    "UserSignInHandlerProviderConfig",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (UserSignInHandlerProviderConfig)HttpRuntime.Cache["UserSignInHandlerProviderConfig"];

            }
            catch (HttpException ex)
            {
                log.Error(ex);

            }
            catch (System.Xml.XmlException ex)
            {
                log.Error(ex);

            }
            catch (ArgumentException ex)
            {
                log.Error(ex);

            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);

            }

            return null;


        }

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    foreach (XmlNode providerNode in child.ChildNodes)
                    {
                        if (
                            (providerNode.NodeType == XmlNodeType.Element)
                            && (providerNode.Name == "add")
                            )
                        {
                            if (
                                (providerNode.Attributes["name"] != null)
                                && (providerNode.Attributes["type"] != null)
                                )
                            {
                                ProviderSettings providerSettings
                                    = new ProviderSettings(
                                    providerNode.Attributes["name"].Value,
                                    providerNode.Attributes["type"].Value);

                                providerSettingsCollection.Add(providerSettings);
                            }

                        }
                    }

                }
            }
        }

    }
}
