// Author:					
// Created:				    2007-08-05
// Last Modified:			2017-10-04
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
using System.Xml;
using log4net;

namespace mojoPortal.Web
{
    /// <summary>
    /// Defines a Feature that can plug into the CMS
    /// </summary>
    public class ContentFeature
    {
        private ContentFeature()
        { }

        private static readonly ILog log
            = LogManager.GetLogger(typeof(ContentFeature));

        private Guid featureGuid = Guid.Empty;
        private string supportedDatabases = "MSSQL";
        private string resourceFile = string.Empty;
        private string featureNameReasourceKey = string.Empty;
        private string controlSource = string.Empty;
        private int sortOrder = 300;
        private bool isCacheable = false;
        private int defaultCacheTime = 0;
        private bool excludeFromFeatureList = false;
        private string icon = "blank.gif";
        private bool isSearchable = false;
        private string searchListNameResourceKey = string.Empty;
        private bool supportsPageReuse = true;
        private string deleteProvider = string.Empty;
        private string partialView = string.Empty;
        private string skinFileName = string.Empty;

		private Collection<ContentFeatureSetting> featureSettings
            = new Collection<ContentFeatureSetting>();

        public Guid FeatureGuid
        {
            get { return featureGuid; }
           
        }

        public string SupportedDatabases
        {
            get { return supportedDatabases; }
        }

        public string ResourceFile
        {
            get { return resourceFile; }
        }

        public string FeatureNameReasourceKey
        {
            get { return featureNameReasourceKey; }
        }

        public string SearchListNameResourceKey
        {
            get { return searchListNameResourceKey; }
        }

        public bool IsSearchable
        {
            get { return isSearchable; }
        }

        public bool SupportsPageReuse
        {
            get { return supportsPageReuse; }
            set { supportsPageReuse = value; }
        }

        public string DeleteProvider
        {
            get { return deleteProvider; }
            set { deleteProvider = value; }
        }

        public string PartialView
        {
            get { return partialView; }
        }

        public string ControlSource
        {
            get { return controlSource; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
        }

        public bool IsCacheable
        {
            get { return isCacheable; }
        }

        public int DefaultCacheTime
        {
            get { return defaultCacheTime; }
        }

        public string Icon
        {
            get { return icon; }
        }

        public bool ExcludeFromFeatureList
        {
            get { return excludeFromFeatureList; }
        }

		public string SkinFileName
		{
			get => skinFileName;
		}

        public Collection<ContentFeatureSetting> Settings
        {
            get
            {
                return featureSettings;
            }
        }


        public static void LoadFeature(
            ContentFeatureConfiguration contentFeatureConfig,
            XmlNode documentElement)
        {
            if (HttpContext.Current == null) return;
            if (documentElement.Name != "featureDefinitions") return;

            foreach (XmlNode node in documentElement.ChildNodes)
            {
                if (node.Name == "featureDefinition")
                {
                    ContentFeature feature = new ContentFeature();

                    XmlAttributeCollection attributeCollection
                        = node.Attributes;

                    if (attributeCollection["featureGuid"] != null)
                    {
                        feature.featureGuid = new Guid(attributeCollection["featureGuid"].Value);
                    }

                    if (attributeCollection["supportedDatabases"] != null)
                    {
                        feature.supportedDatabases = attributeCollection["supportedDatabases"].Value;
                    }

                    

                    if (attributeCollection["resourceFile"] != null)
                    {
                        feature.resourceFile = attributeCollection["resourceFile"].Value;
                    }

                    if (attributeCollection["excludeFromFeatureList"] != null)
                    {
                        try
                        {
                            feature.excludeFromFeatureList = Convert.ToBoolean(attributeCollection["excludeFromFeatureList"].Value);
                        }
                        catch { }
                    }

                    if (attributeCollection["featureNameReasourceKey"] != null)
                    {
                        feature.featureNameReasourceKey = attributeCollection["featureNameReasourceKey"].Value;
                    }


                    if (attributeCollection["isSearchable"] != null)
                    {
                        try
                        {
                            feature.isSearchable = Convert.ToBoolean(attributeCollection["isSearchable"].Value);
                        }
                        catch { }
                    }

                    if (attributeCollection["supportsPageReuse"] != null)
                    {
                        try
                        {
                            feature.supportsPageReuse = Convert.ToBoolean(attributeCollection["supportsPageReuse"].Value);
                        }
                        catch { }
                    }

                    if (attributeCollection["isCacheable"] != null)
                    {
                        try
                        {
                            feature.isCacheable = Convert.ToBoolean(attributeCollection["isCacheable"].Value);
                        }
                        catch { }
                    }

                    if (attributeCollection["deleteProvider"] != null)
                    {
                        feature.deleteProvider = attributeCollection["deleteProvider"].Value;
                    }

                    if (attributeCollection["partialView"] != null)
                    {
                        feature.partialView = attributeCollection["partialView"].Value;
                    }

					if (attributeCollection["skinFileName"] != null)
					{
						feature.skinFileName = attributeCollection["skinFileName"].Value;
					}


					if (attributeCollection["searchListNameResourceKey"] != null)
                    {
                        feature.searchListNameResourceKey = attributeCollection["searchListNameResourceKey"].Value;
                    }

                    if (attributeCollection["controlSource"] != null)
                    {
                        feature.controlSource = attributeCollection["controlSource"].Value;
                    }

                    if (!feature.controlSource.EndsWith(".ascx"))
                    {
                        log.Error("could not install feature " + feature.FeatureNameReasourceKey
                            + ". Invalid ControlSource Setting");

                        return;

                    }

                    if (attributeCollection["icon"] != null)
                    {
                        feature.icon = attributeCollection["icon"].Value;
                    }

                    if (attributeCollection["sortOrder"] != null)
                    {
                        int sort = 300;
                        if (int.TryParse(attributeCollection["sortOrder"].Value,
                            out sort))
                        {
                            feature.sortOrder = sort;
                        }
                    }

                    if (attributeCollection["defaultCacheTime"] != null)
                    {
                        int cacheTime = 300;
                        if (int.TryParse(attributeCollection["defaultCacheTime"].Value,
                            out cacheTime))
                        {
                            feature.defaultCacheTime = cacheTime;
                        }
                    }

                    foreach (XmlNode featureSettingNode in node.ChildNodes)
                    {
                        if (featureSettingNode.Name == "featureSetting")
                        {
                            ContentFeatureSetting.LoadFeatureSetting(
                                feature,
                                featureSettingNode);
                        }
                    }

                    if (feature.FeatureGuid == Guid.Empty)
                    {
                        log.Error("could not install feature " + feature.FeatureNameReasourceKey
                        + ". Invalid FeatureGuid.");
                        return;

                    }
                        
                    string controlPath = feature.controlSource;
                    if (!controlPath.StartsWith("/"))
                    {
                        controlPath = "~/" + controlPath;
                    }
                    else
                    {
                        controlPath = "~" + controlPath;
                    }

                    if (File.Exists(HttpContext.Current.Server.MapPath(controlPath)))
                    {
                        contentFeatureConfig.ContentFeatures.Add(feature);
                    }
                    else
                    {
                        log.Error("could not install feature " + feature.FeatureNameReasourceKey
                        + ". Invalid ControlSource Setting. Filenot found: " + controlPath);

                    }

                    
                }

            }


        }

    }
}
