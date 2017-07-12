// Author:					
// Created:				    2007-08-07
// Last Modified:			2013-07-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 2013-06-24 / Thomas Nicolaïdès : moduleSettings / moduleGuidxxxx handling (tni-20130624)

using System;
using System.Xml;
using log4net;
using System.Collections.Generic;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentPageItem
    {
        private ContentPageItem()
        {
            moduleSettings = new Dictionary<string, string>();
        }


        private static readonly ILog log = LogManager.GetLogger(typeof(ContentPageItem));

        private Guid featureGuid = Guid.Empty;
        private string contentTitle = string.Empty;
        private string contentTemplate = string.Empty;
        private string location = "center";
        private int sortOrder = 1;
        private int cacheTimeInSeconds = 0;

        private string viewRoles = "All Users;";
        private string editRoles = String.Empty;
        private string draftEditRoles = String.Empty;
        private bool isGlobal = false;
        private bool hideFromAnonymous = false;
        private bool hideFromAuthenticated = false;
        private bool showTitle = true;
        private string headElement = "h2";


        // tni 2013-06-24
        private Guid moduleGuidToPublish = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;

        Dictionary<string, string> moduleSettings;

        public Dictionary<string, string> ModuleSettings
        {
            get { return moduleSettings; }
        }

        public Guid ModuleGuidToPublish { get { return moduleGuidToPublish; } }
        public Guid ModuleGuid { get { return moduleGuid; } }

        //end tni additions 2013-06-24
        

        private IContentInstaller installer = null;

        public IContentInstaller Installer
        {
            get { return installer; }
        }

        private string configInfo = string.Empty;

        public string ConfigInfo
        {
            get { return configInfo; }
        }

        public Guid FeatureGuid
        {
            get { return featureGuid; }
        }

        public string ContentTitle
        {
            get { return contentTitle; }
        }

        public string ContentTemplate
        {
            get { return contentTemplate; } 
        }

        public string Location
        {
            get { return location; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
        }

        public int CacheTimeInSeconds
        {
            get { return cacheTimeInSeconds; }
        }

        public string ViewRoles
        {
            get { return viewRoles; }
        }

        public string EditRoles
        {
            get { return editRoles; }
        }

        public string DraftEditRoles
        {
            get { return draftEditRoles; }
        }

        public bool IsGlobal
        {
            get { return isGlobal; }
        }

        public bool ShowTitle
        {
            get { return showTitle; }
        }

        public string HeadElement
        {
            get { return headElement; }
        }

        public bool HideFromAnonymous
        {
            get { return hideFromAnonymous; }
        }

        public bool HideFromAuthenticated
        {
            get { return hideFromAuthenticated; }
        }

        public static void LoadPageItem(
            ContentPage contentPage,
            XmlNode pageItemNode)
        {
            if (contentPage == null) return;
            if (pageItemNode == null) return;

            if (pageItemNode.Name == "contentFeature")
            {
                ContentPageItem pageItem = new ContentPageItem();

                XmlAttributeCollection attributeCollection = pageItemNode.Attributes;

                if (attributeCollection["featureGuid"] != null)
                {
                    pageItem.featureGuid = new Guid(attributeCollection["featureGuid"].Value);
                }

                if (attributeCollection["contentTitle"] != null)
                {
                    pageItem.contentTitle = attributeCollection["contentTitle"].Value;
                }

                if (attributeCollection["contentTemplate"] != null)
                {
                    pageItem.contentTemplate = attributeCollection["contentTemplate"].Value;
                }

                if (attributeCollection["configInfo"] != null)
                {
                    pageItem.configInfo = attributeCollection["configInfo"].Value;
                }

                if (attributeCollection["viewRoles"] != null)
                {
                    pageItem.viewRoles = attributeCollection["viewRoles"].Value;
                }

                if (attributeCollection["editRoles"] != null)
                {
                    pageItem.editRoles = attributeCollection["editRoles"].Value;
                }

                if (attributeCollection["draftEditRoles"] != null)
                {
                    pageItem.draftEditRoles = attributeCollection["draftEditRoles"].Value;
                }

                if (attributeCollection["headElement"] != null)
                {
                    pageItem.headElement = attributeCollection["headElement"].Value;
                }

                if (
                (attributeCollection["isGlobal"] != null)
                && (attributeCollection["isGlobal"].Value.ToLower() == "true")
                )
                {
                    pageItem.isGlobal = true;
                }

                if (
                (attributeCollection["showTitle"] != null)
                && (attributeCollection["showTitle"].Value.ToLower() == "false")
                )
                {
                    pageItem.showTitle = false;
                }

                if (
                (attributeCollection["hideFromAnonymous"] != null)
                && (attributeCollection["hideFromAnonymous"].Value.ToLower() == "true")
                )
                {
                    pageItem.hideFromAnonymous = true;
                }

                if (
                (attributeCollection["hideFromAuthenticated"] != null)
                && (attributeCollection["hideFromAuthenticated"].Value.ToLower() == "true")
                )
                {
                    pageItem.hideFromAuthenticated = true;
                }

                try
                {
                    if (attributeCollection["contentInstaller"] != null && typeof(IContentInstaller).IsAssignableFrom(Type.GetType(attributeCollection["contentInstaller"].Value)))
                    {
                        pageItem.installer = Activator.CreateInstance(Type.GetType(attributeCollection["contentInstaller"].Value)) as IContentInstaller;
                    }
                }
                catch (Exception ex) // we don't want it to fail during site creation even if not all content is created due to errors here.
                {
                    log.Error(ex);
                }

                

                if (attributeCollection["location"] != null)
                {
                    string location = attributeCollection["location"].Value;
                    switch (location)
                    {
                        case "right":
                        case "rightpane":
                            pageItem.location = "rightpane";
                            break;

                        case "left":
                        case "leftpane":
                            pageItem.location = "leftpane";
                            break;

                        case "top":
                        case "altcontent1":
                            pageItem.location = "altcontent1";
                            break;

                        case "bottom":
                        case "altcontent2":
                            pageItem.location = "altcontent2";
                            break;

                        case "center":
                        case "contentpane":
                        default:
                            pageItem.location = "contentpane";
                            break;

                    }
                }

                if (attributeCollection["sortOrder"] != null)
                {
                    int sort = 1;
                    if (int.TryParse(attributeCollection["sortOrder"].Value,
                        out sort))
                    {
                        pageItem.sortOrder = sort;
                    }
                }

                if (attributeCollection["cacheTimeInSeconds"] != null)
                {
                    int cacheTimeInSeconds = 1;
                    if (int.TryParse(attributeCollection["cacheTimeInSeconds"].Value,
                        out cacheTimeInSeconds))
                    {
                        pageItem.cacheTimeInSeconds = cacheTimeInSeconds;
                    }
                }


                // tni-20130624: handling module settings
                XmlNodeList moduleSettings = pageItemNode.SelectNodes("./moduleSetting");
                foreach (XmlNode moduleSetting in moduleSettings)
                {
#if NET35
                    if (moduleSetting.Attributes["settingKey"] != null &&
                        !string.IsNullOrEmpty(moduleSetting.Attributes["settingKey"].Value) &&
                        moduleSetting.Attributes["settingValue"] != null &&
                        moduleSetting.Attributes["settingValue"].Value != null)
                    {
                        pageItem.moduleSettings[moduleSetting.Attributes["settingKey"].Value] =
                            moduleSetting.Attributes["settingValue"].Value;
                    }
#else

                    if (moduleSetting.Attributes["settingKey"] != null &&
                        !string.IsNullOrWhiteSpace(moduleSetting.Attributes["settingKey"].Value) &&
                        moduleSetting.Attributes["settingValue"] != null &&
                        moduleSetting.Attributes["settingValue"].Value != null)
                    {
                        pageItem.moduleSettings[moduleSetting.Attributes["settingKey"].Value] =
                            moduleSetting.Attributes["settingValue"].Value;
                    }
#endif
                }

                // No parse error handling like done above.. 
                if (attributeCollection["moduleGuid"] != null)
                {
#if NET35
                    pageItem.moduleGuid = new Guid(attributeCollection["moduleGuid"].Value);
#else
                    Guid.TryParse(attributeCollection["moduleGuid"].Value, out pageItem.moduleGuid);
#endif
                }

                if (attributeCollection["moduleGuidToPublish"] != null)
                {
#if NET35
                    pageItem.moduleGuidToPublish = new Guid(attributeCollection["moduleGuidToPublish"].Value);
#else

                    Guid.TryParse(attributeCollection["moduleGuidToPublish"].Value, out pageItem.moduleGuidToPublish);
#endif
                }
                //

                contentPage.PageItems.Add(pageItem);
                
            }


        }

    }
}
