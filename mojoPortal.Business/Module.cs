// Author:					Joe Audette
// Created:				    2004-12-26
// Last Modified:			2013-04-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents an instance of a feature
	/// </summary>
	public class Module : IComparable 
	{

		#region Constructors

		public Module()
		{}

        public Module(Guid moduleGuid)
        {
            GetModule(moduleGuid);
        }


        public Module(int moduleId)
        {
            GetModule(moduleId);
        }

        public Module(int moduleId, int pageId)
        {
            this.pageID = pageId;
            GetModule(moduleId, pageId);
        }

		#endregion

		#region Private Properties

		private int moduleID = -1;
        private Guid guid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private int siteID = 0;
		private int pageID = -1; 
		private int moduleDefID; 
		private int moduleOrder = 999; 
		private string paneName = String.Empty; 
		private string moduleTitle = String.Empty;
        private string viewRoles = "All Users;";
		private string authorizedEditRoles = String.Empty;
        private string draftEditRoles = String.Empty;
        private string draftApprovalRoles = String.Empty;
		private int cacheTime = 0; 
		private bool showTitle = true; 
		private string controlSource = string.Empty;
		private int editUserID = -1;
        private Guid editUserGuid = Guid.Empty;
        private bool availableForMyPage = false;
        private bool allowMultipleInstancesOnMyPage = true;
        private String icon = String.Empty;
        private int createdByUserID = -1;
        private DateTime createdDate = DateTime.MinValue;
        private String featureName = String.Empty;
        private bool hideFromAuthenticated = false;
        private bool hideFromUnauthenticated = false;
        private bool includeInSearch = true;
        private bool isGlobal = false;

        private string headElement = "h2";
        private int publishMode = 0; //All
        
      
		#endregion

		#region Public Properties

        public Guid ModuleGuid
        {
            get { return guid; }
            set { guid = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

		public int ModuleId 
		{
			get { return moduleID; }
			set { moduleID = value; }
		}

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

		public int EditUserId 
		{
			get { return editUserID; }
			set { editUserID = value; }
		}

        public Guid EditUserGuid
        {
            get { return editUserGuid; }
            set { editUserGuid = value; }
        }

		public int PageId 
		{
			get { return pageID; }
			set { pageID = value; }
		}
		public int ModuleDefId 
		{
			get { return moduleDefID; }
			set { moduleDefID = value; }
		}

		public int ModuleOrder 
		{
			get { return moduleOrder; }
			set { moduleOrder = value; }
		}

        public int PublishMode
        {
            get { return publishMode; }
            set { publishMode = value; }
        }

		public string PaneName 
		{
			get { return paneName; }
			set { paneName = value; }
		}
		public string ModuleTitle 
		{
			get { return moduleTitle; }
			set { moduleTitle = value; }
		}

        public string HeadElement
        {
            get { return headElement; }
            set { headElement = value; }
        }

        public string ViewRoles
        {
            get { return viewRoles; }
            set { viewRoles = value; }
        }

		public string AuthorizedEditRoles 
		{
			get { return authorizedEditRoles; }
			set { authorizedEditRoles = value; }
		}

        public string DraftEditRoles
        {
            get { return draftEditRoles; }
            set { draftEditRoles = value; }
        }

        public string DraftApprovalRoles
        {
            get { return draftApprovalRoles; }
            set { draftApprovalRoles = value; }
        }

		public int CacheTime 
		{
			get { return cacheTime; }
			set { cacheTime = value; }
		}
		public bool ShowTitle 
		{
			get { return showTitle; }
			set { showTitle = value; }
		}

		public string ControlSource 
		{
			get {return controlSource;}
			set {controlSource = value;}
		}

        public bool AvailableForMyPage
        {
            get { return availableForMyPage; }
            set { availableForMyPage = value; }
        }

        public bool AllowMultipleInstancesOnMyPage
        {
            get { return allowMultipleInstancesOnMyPage; }
            set { allowMultipleInstancesOnMyPage = value; }
        }

        public int CreatedByUserId
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
           
        }

        public string FeatureName
        {
            get { return this.featureName; }
           
        }

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public bool HideFromAuthenticated
        {
            get { return hideFromAuthenticated; }
            set { hideFromAuthenticated = value; }
        }

        public bool HideFromUnauthenticated
        {
            get { return hideFromUnauthenticated; }
            set { hideFromUnauthenticated = value; }
        }

        public bool IncludeInSearch
        {
            get { return includeInSearch; }
            set { includeInSearch = value; }
        }

        public bool IsGlobal
        {
            get { return isGlobal; }
            set { isGlobal = value; }
        }

		#endregion

		#region Private Methods

        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                this.siteID = Convert.ToInt32(reader["SiteID"]);
                //this.pageID = Convert.ToInt32(reader["PageID"]);
                this.moduleDefID = Convert.ToInt32(reader["ModuleDefID"]);
                //this.moduleOrder = Convert.ToInt32(reader["ModuleOrder"]);
                //this.paneName = reader["PaneName"].ToString();
                this.moduleTitle = reader["ModuleTitle"].ToString();
                this.authorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
                this.draftEditRoles = reader["DraftEditRoles"].ToString();
                this.draftApprovalRoles = reader["DraftApprovalRoles"].ToString();
                this.cacheTime = Convert.ToInt32(reader["CacheTime"]);
                this.showTitle = Convert.ToBoolean(reader["ShowTitle"]);
                if (reader["EditUserID"] != DBNull.Value)
                {
                    this.editUserID = Convert.ToInt32(reader["EditUserID"]);
                }
                this.availableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
                this.allowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
                if (reader["CreatedByUserID"] != DBNull.Value)
                {
                    this.createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }
                if (reader["CreatedDate"] != DBNull.Value)
                {
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                }

                this.icon = reader["Icon"].ToString();


                this.guid = new Guid(reader["Guid"].ToString());
                this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());

                string edUserGuid = reader["EditUserGuid"].ToString();
                if (edUserGuid.Length == 36) this.editUserGuid = new Guid(edUserGuid);

                this.hideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
                this.hideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);

                this.viewRoles = reader["ViewRoles"].ToString();

                //ModuleDefinition moduleDef = new ModuleDefinition(this.moduleDefID);
                //this.controlSource = moduleDef.ControlSrc;
                //this.featureName = moduleDef.FeatureName;

                this.controlSource = reader["ControlSrc"].ToString();
                this.featureName = reader["FeatureName"].ToString();
               
                this.includeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
                this.isGlobal = Convert.ToBoolean(reader["IsGlobal"]);
                this.headElement = reader["HeadElement"].ToString();

                this.publishMode = Convert.ToInt32(reader["PublishMode"]);
            }

        }

        private void GetModule(Guid moduleGuid)
        {
            using (IDataReader reader = DBModule.GetModule(moduleGuid))
            {
                PopulateFromReader(reader);
            }
        }

		private void GetModule(int moduleId) 
		{
            using (IDataReader reader = DBModule.GetModule(moduleId))
            {
                PopulateFromReader(reader);
               
            }

		}

        private void GetModule(int moduleId, int pageId)
        {
            using (IDataReader reader = DBModule.GetModule(moduleId, pageId))
            {
                PopulateFromReader(reader);
            }

            

        }


		#endregion

		#region Public Methods

		public int CompareTo(object value) 
		{

			if (value == null) return 1;

			int compareOrder = ((Module)value).ModuleOrder;
            
			if (this.ModuleOrder == compareOrder) return 0;
			if (this.ModuleOrder < compareOrder) return -1;
			if (this.ModuleOrder > compareOrder) return 1;
			return 0;
		}

		public bool Save()
		{
			if(this.moduleID > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
		}

		private bool Create()
		{ 
			bool created = false;
			int newID = -1;
            
            if (this.guid == Guid.Empty)// tni 2013-06-26 forced moduleGuid implementation
                this.guid = Guid.NewGuid();

			newID = DBModule.AddModule(
				this.pageID, 
                this.siteID,
                this.siteGuid,
				this.moduleDefID, 
				this.moduleOrder, 
				this.paneName, 
				this.moduleTitle, 
                this.viewRoles,
				this.authorizedEditRoles,
                this.draftEditRoles,
                this.draftApprovalRoles,
				this.cacheTime, 
				this.showTitle,
                this.availableForMyPage,
                this.allowMultipleInstancesOnMyPage,
                this.icon,
                this.createdByUserID,
                DateTime.UtcNow,
                this.guid,
                this.featureGuid,
                this.hideFromAuthenticated,
                this.hideFromUnauthenticated,
                this.headElement,
                this.publishMode); 
			
			this.moduleID = newID;
			created = (newID > -1);
			if(created)
			{
				ModuleSettings.CreateDefaultModuleSettings(this.moduleID);
			}
					
			return created;

		}

		private bool Update()
		{

			return DBModule.UpdateModule(
				this.moduleID, 
				this.moduleDefID, 
				this.moduleTitle, 
                this.viewRoles,
				this.authorizedEditRoles, 
                this.draftEditRoles,
                this.draftApprovalRoles,
				this.cacheTime, 
				this.showTitle,
				this.editUserID,
                this.availableForMyPage,
                this.allowMultipleInstancesOnMyPage,
                this.icon,
                this.hideFromAuthenticated,
                this.hideFromUnauthenticated,
                this.includeInSearch,
                this.isGlobal,
                this.headElement,
                this.publishMode); 
				
		}
		
		#endregion


		#region Static Methods

        /// <summary>
        /// Returns a DataReader of published pagemodules
        /// </summary>
		public static IDataReader GetPageModules(int pageId) 
		{
			return DBModule.GetPageModules(pageId);
		}

        public static DataTable GetPageModulesTable(int moduleId)
        {
            return DBModule.PageModuleGetByModule(moduleId);
        }

        public static void DeletePageModules(int pageId)
        {
            DBModule.PageModuleDeleteByPage(pageId);
            //IDataReader result = GetPageModules(pageId);
            //ArrayList modules = new ArrayList();

            //while (result.Read())
            //{
            //    Module m = new Module();
            //    m.ModuleId = Convert.ToInt32(result["ModuleID"]);
            //    m.ModuleDefId = Convert.ToInt32(result["ModuleDefID"]);
            //    m.PageId = Convert.ToInt32(result["PageID"]);
            //    m.PaneName = result["PaneName"].ToString();
            //    m.ModuleTitle = result["ModuleTitle"].ToString();
            //    m.AuthorizedEditRoles = result["AuthorizedEditRoles"].ToString();
            //    m.CacheTime = Convert.ToInt32(result["CacheTime"]);
            //    m.ModuleOrder = Convert.ToInt32(result["ModuleOrder"]);
            //    if (result["EditUserID"] != DBNull.Value)
            //    {
            //        m.EditUserId = Convert.ToInt32(result["EditUserID"]);
            //    }


            //    string showTitle = result["ShowTitle"].ToString();
            //    m.ShowTitle = (showTitle == "True" || showTitle == "1");
            //    m.ControlSource = result["ControlSrc"].ToString();


            //    modules.Add(m);


            //}
            //result.Close();

            //foreach (Module m in modules)
            //{
            //    DeleteModuleInstance(m.ModuleId, pageId);

            //}


        }

        public static bool UpdateModuleOrder(int pageId, int moduleId, int moduleOrder, string paneName)
        {
            return DBModule.UpdateModuleOrder(pageId, moduleId, moduleOrder, paneName);
        }

		public static bool DeleteModule(int moduleId) 
		{
            
			ModuleSettings.DeleteModuleSettings(moduleId);
			return DBModule.DeleteModule(moduleId);
			
		}

        public static bool DeleteModuleInstance(int moduleId, int pageId)
        {
            return DBModule.DeleteModuleInstance(moduleId, pageId);
        }

        public static bool Publish(
            Guid pageGuid,
            Guid moduleGuid,
            int moduleId,
            int pageId,
            String paneName,
            int moduleOrder,
            DateTime publishBeginDate,
            DateTime publishEndDate)
        {
            return DBModule.Publish(
                pageGuid,
                moduleGuid,
                moduleId, 
                pageId, 
                paneName, 
                moduleOrder, 
                publishBeginDate, 
                publishEndDate);

        }

        public static bool UpdatePage(int oldPageId, int newPageId, int moduleId)
        {
            return DBModule.UpdatePage(oldPageId, newPageId, moduleId);
        }

        public static DataTable SelectPage(
            int siteId,
            int moduleDefId,
            string title,
            int pageNumber,
            int pageSize,
            bool sortByModuleType,
            bool sortByAuthor,
            out int totalPages)
        {
            return DBModule.SelectPage(
                siteId, 
                moduleDefId,
                title,
                pageNumber, 
                pageSize, 
                sortByModuleType,
                sortByAuthor,
                out totalPages);
        }

        public static int GetGlobalCount(int siteId, int moduleDefId, int pageId)
        {
            return DBModule.GetGlobalCount(siteId, moduleDefId, pageId);
        }


        public static DataTable SelectGlobalPage(
            int siteId,
            int moduleDefId,
            int pageId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBModule.SelectGlobalPage(
                siteId,
                moduleDefId,
                pageId,
                pageNumber,
                pageSize,
                out totalPages);
        }


        public static IDataReader GetMyPageModules(int siteId)
        {
            return DBModule.GetMyPageModules(siteId);

        }

        public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment)
        {
            return DBModule.UpdateCountOfUseOnMyPage(moduleId, increment);
        }

        public static IDataReader GetModulesForSite(int siteId, Guid featureGuid)
        {
            return DBModule.GetModulesForSite(siteId, featureGuid);
        }

        public static int GetCountByFeature(int moduleDefId)
        {
            return DBModule.GetCountByFeature(moduleDefId);
        }


		#endregion


	}
	
}
