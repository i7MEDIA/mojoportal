// Author:					
// Created:				    2004-12-26
// Last Modified:			2017-10-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Data;
using System.Collections.Generic;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents a feature that can plug into the content management system.
	/// </summary>
	public class ModuleDefinition
	{

		#region Constructors

		public ModuleDefinition()
		{}
	    
	
		public ModuleDefinition(int moduleDefId) 
		{
			GetModuleDefinition(moduleDefId); 
		}

        public ModuleDefinition(Guid featureGuid)
        {
            providedFeatureGuid = featureGuid;
            GetModuleDefinition(featureGuid);
        }

		#endregion

		#region Private Properties

		private int moduleDefID = -1;
        private Guid featureGuid = Guid.Empty;
        private Guid providedFeatureGuid = Guid.Empty;
		private int siteID = -1;
        private string resourceFile = "Resource";
		private string featureName = string.Empty; 
		private string controlSrc = string.Empty; 
		private int sortOrder = 500;
        private bool isCacheable = false;
        private int defaultCacheTime = 0;
		private bool isAdmin = false;
        private String icon = String.Empty;
        private bool isSearchable = false;
        private string searchListName = string.Empty;
        private bool supportsPageReuse = true;
        private string deleteProvider = string.Empty;
        private string partialView = string.Empty;
        private string skinFileName = string.Empty;



		#endregion

		#region Public Properties

		public int ModuleDefId 
		{
			get { return moduleDefID; }
			set { moduleDefID = value; }
		}

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public string ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

		public string FeatureName 
		{
			get { return featureName; }
			set { featureName = value; }
		}

        public bool IsCacheable
        {
            get { return isCacheable; }
            set { isCacheable = value; }
        }

        public bool IsSearchable
        {
            get { return isSearchable; }
            set { isSearchable = value; }
        }

        public string SearchListName
        {
            get { return searchListName; }
            set { searchListName = value; }
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

		public string ControlSrc 
		{
			get { return controlSrc; }
			set { controlSrc = value; }
		}
		public int SortOrder 
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}
        public int DefaultCacheTime
        {
            get { return defaultCacheTime; }
            set { defaultCacheTime = value; }
        }
		public bool IsAdmin 
		{
			get { return isAdmin; }
			set { isAdmin = value; }
		}

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public string PartialView
        {
            get { return partialView; }
            set { partialView = value; }
        }

		public string SkinFileName
		{
			get => skinFileName;
			set => skinFileName = value;
		}

		#endregion

		#region Private Methods

        private void GetModuleDefinition(Guid featureGuid)
        {
            using (IDataReader reader = DBModuleDefinition.GetModuleDefinition(featureGuid))
            {
                GetModuleDefinition(reader);
            }
        }

		private void GetModuleDefinition(int moduleDefId) 
		{
            using (IDataReader reader = DBModuleDefinition.GetModuleDefinition(moduleDefId))
            {
                GetModuleDefinition(reader);
            }
			
		}

        private void GetModuleDefinition(IDataReader reader)
        {
            if (reader.Read())
            {
                this.moduleDefID = Convert.ToInt32(reader["ModuleDefID"]);
                //this.siteID = Convert.ToInt32(reader["SiteID"]);

                this.featureName = reader["FeatureName"].ToString();
                this.controlSrc = reader["ControlSrc"].ToString();
                this.sortOrder = Convert.ToInt32(reader["SortOrder"]);
                this.defaultCacheTime = Convert.ToInt32(reader["DefaultCacheTime"]);
                this.isAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                this.icon = reader["Icon"].ToString();
                this.featureGuid = new Guid(reader["Guid"].ToString());
                this.resourceFile = reader["ResourceFile"].ToString();

                this.searchListName = reader["SearchListName"].ToString();
                if(reader["IsCacheable"] != DBNull.Value)
                {
                    this.isCacheable = Convert.ToBoolean(reader["IsCacheable"]);
                }

                if (reader["IsSearchable"] != DBNull.Value)
                {
                    this.isSearchable = Convert.ToBoolean(reader["IsSearchable"]);
                }

                if (reader["SupportsPageReuse"] != DBNull.Value)
                {
                    this.supportsPageReuse = Convert.ToBoolean(reader["SupportsPageReuse"]);
                }

                this.deleteProvider = reader["DeleteProvider"].ToString();

                this.partialView = reader["PartialView"].ToString();

				this.skinFileName = reader["SkinFileName"].ToString();
            }

            

        }

		private bool Create()
		{ 
			int newID = -1;

            if (this.featureGuid == Guid.Empty)
            {
                if (this.providedFeatureGuid != Guid.Empty)
                {
                    this.featureGuid = this.providedFeatureGuid;
                }
                else
                {
                    this.featureGuid = Guid.NewGuid();
                }
            }
			
			newID = DBModuleDefinition.AddModuleDefinition(
                this.featureGuid,
				this.siteID, 
				this.featureName, 
				this.controlSrc, 
				this.sortOrder,
                this.defaultCacheTime,
                this.icon,
				this.isAdmin,
                this.resourceFile,
                this.isCacheable,
                this.isSearchable,
                this.searchListName,
                this.supportsPageReuse,
                this.deleteProvider,
                this.partialView,
				this.skinFileName); 
			
			this.moduleDefID = newID;
					
			return (newID > -1);

		}

		private bool Update()
		{

			return DBModuleDefinition.UpdateModuleDefinition(
				this.moduleDefID, 
				this.featureName, 
				this.controlSrc, 
				this.sortOrder, 
                this.defaultCacheTime,
                this.icon,
				this.isAdmin,
                this.resourceFile,
                this.isCacheable,
                this.isSearchable,
                this.searchListName,
                this.supportsPageReuse,
                this.deleteProvider,
                this.partialView,
				this.skinFileName); 
				
		}


		#endregion

		#region Public Methods

	
		public bool Save()
		{
			if(this.moduleDefID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}
		
		#endregion

		#region Static Methods

		public static bool DeleteModuleDefinition(int moduleDefId)
		{
            DBModuleDefinition.DeleteModuleDefinitionFromSites(moduleDefId);
			return DBModuleDefinition.DeleteModuleDefinition(moduleDefId);
		}

        public static bool DeleteSettingsByFeature(int moduleDefId)
        {
            return DBModuleDefinition.DeleteSettingsByFeature(moduleDefId);
        }

        public static IDataReader GetModuleDefinitions(Guid siteGuid)
		{
            return DBModuleDefinition.GetModuleDefinitions(siteGuid);
		}

        public static DataTable GetModuleDefinitionsBySite(Guid siteGuid)
        {
            return DBModuleDefinition.GetModuleDefinitionsBySite(siteGuid);
        }

        public static SiteModuleDefinition GetSiteFeature(Guid siteGuid, int moduleDefId)
        {
            DataTable features = GetModuleDefinitionsBySite(siteGuid);

            foreach (DataRow row in features.Rows)
            {
                int id = Convert.ToInt32(row["ModuleDefID"]);
                if (id == moduleDefId) 
                {
                    SiteModuleDefinition feature = new SiteModuleDefinition();
                    feature.ModueDefId = id;
                    feature.FeatureGuid = new Guid(row["FeatureGuid"].ToString());
                    feature.FeatureName = row["FeatureName"].ToString();
                    feature.AuthorizedRoles = row["AuthorizedRoles"].ToString();
                    return feature; 
                }
            }

            return null;

        }

		public static ModuleDefinition GetModuleDefinitionBySkinFileName (string skinFileName)
		{
			ModuleDefinition md = new ModuleDefinition();
			md.GetModuleDefinition(DBModuleDefinition.GetModuleDefinitionBySkinFileName(skinFileName));
			return md;
		}

		public static List<String> GetAllModuleSkinFileNames()
		{
			List<string> list = new List<string>();
			using (IDataReader reader = DBModuleDefinition.GetAllModuleSkinFileNames())
			{
				while (reader.Read())
				{
					list.Add(reader["SkinFileName"].ToString());
				}
			}

			return list;
		}

		public static IDataReader GetUserModules(int siteId)
		{
			return DBModuleDefinition.GetUserModules(siteId);
		}

        public static IDataReader GetSearchableModules(int siteId)
        {
            return DBModuleDefinition.GetSearchableModules(siteId);
        }

        public static bool UpdateModuleDefinitionSetting(
            Guid featureGuid,
            int moduleDefId, 
            string resourceFile,
            string groupName,
            string settingName, 
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder,
			string attributes,
			string options)
        {
            return DBModuleDefinition.UpdateModuleDefinitionSetting(
                featureGuid,
                moduleDefId, 
                resourceFile,
                groupName,
                settingName, 
                settingValue,
                controlType,
                regexValidationExpression,
                controlSrc,
                helpKey,
                sortOrder,
				attributes,
				options);

        }

        public static bool UpdateModuleDefinitionSettingById(
            int id,
            int moduleDefId,
            string resourceFile,
            string groupName,
            string settingName,
            string settingValue,
            string controlType,
            string regexValidationExpression,
            string controlSrc,
            string helpKey,
            int sortOrder,
			string attributes,
			string options)
        {
            return DBModuleDefinition.UpdateModuleDefinitionSettingById(
                id,
                moduleDefId,
                resourceFile,
                groupName,
                settingName,
                settingValue,
                controlType,
                regexValidationExpression,
                controlSrc,
                helpKey,
                sortOrder,
				attributes,
				options);

        }

        /// <summary>
        /// update instance setting properties to match definition settings
        /// for controltype, controlsrc, regexvalidationexpression ans sort
        /// this is called at the end of an upgrade
        /// </summary>
        //public static void SyncDefinitions()
        //{
        //    DBModuleDefinition.SyncDefinitions();
        //}

        //public static void SyncDefinitions(object o)
        //{
        //    DBModuleDefinition.SyncDefinitions();
        //}

        public static bool DeleteSettingById(int id)
        {
            return DBModuleDefinition.DeleteSettingById(id);

        }

        public static bool SettingExists(Guid featureGuid, string settingName)
        {
            bool result = false;

            using (IDataReader reader = DBModuleDefinition.ModuleDefinitionSettingsGetSetting(
                featureGuid,
                settingName))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;
        }

        public static void EnsureInstallationInAdminSites()
        {
            DBModuleDefinition.EnsureInstallationInAdminSites();
        }

        public static bool UpdateSiteModulePermissions(int siteId, int moduleDefId, string authorizedRoles)
        {
            return DBModuleDefinition.UpdateSiteModulePermissions(siteId, moduleDefId, authorizedRoles);
        }

		#endregion


	}

    
	
}
