// Author:					
// Created:				    2007-08-08
// Last Modified:			2011-03-24
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
using System.Web.Hosting;
using System.Xml;
using log4net;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentPageConfiguration
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContentPageConfiguration));

        private Collection<ContentPage> contentPages
            = new Collection<ContentPage>();

        public Collection<ContentPage> ContentPages
        {
            get
            {
                return contentPages;
            }
        }

        public static ContentPageConfiguration GetConfig()
        {
            ContentPageConfiguration contentPageConfig = new ContentPageConfiguration();

            String configFolderName = "~/Setup/initialcontent/pages/";

            string pathToConfigFolder = HostingEnvironment.MapPath(configFolderName);

            if (!Directory.Exists(pathToConfigFolder)) return contentPageConfig;


            DirectoryInfo directoryInfo = new DirectoryInfo(pathToConfigFolder);

            FileInfo[] pageFiles = directoryInfo.GetFiles("*.config");

            foreach (FileInfo fileInfo in pageFiles)
            {
                if (WebConfigSettings.ContentPagesToSkip.Contains(fileInfo.Name)) { continue; }

                var contentConfigFile = Core.Helpers.XmlHelper.GetXmlDocument(fileInfo.FullName);

                ContentPage.LoadPages(
                    contentPageConfig,
                    contentConfigFile.DocumentElement);

            }

            return contentPageConfig;

        }


    }
}
