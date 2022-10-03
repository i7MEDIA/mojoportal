// The use and distribution terms for this software are covered by the 
// Eclipse Public License 1.0 (https://opensource.org/licenses/eclipse-1.0)  
// which can be found in the file LICENSE.MD at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
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

		public Module() {}

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
            this.PageId = pageId;
            GetModule(moduleId, pageId);
        }

		#endregion

		#region Properties

		public Guid ModuleGuid { get; set; } = Guid.Empty;

		public Guid SiteGuid { get; set; } = Guid.Empty;

		public Guid FeatureGuid { get; set; } = Guid.Empty;

		public int ModuleId { get; set; } = -1;

		public int SiteId { get; set; } = 0;

		public int EditUserId { get; set; } = -1;

		public Guid EditUserGuid { get; set; } = Guid.Empty;

		public int PageId { get; set; } = -1;
		public int ModuleDefId { get; set; }

		public int ModuleOrder { get; set; } = 999;

		public int PublishMode { get; set; } = 0;

		public string PaneName { get; set; } = String.Empty;

		public string ModuleTitle { get; set; } = String.Empty;

		public string HeadElement { get; set; } = "h2";

		public string ViewRoles { get; set; } = "All Users;";

		public string AuthorizedEditRoles { get; set; } = string.Empty;

		public string DraftEditRoles { get; set; } = String.Empty;

		public string DraftApprovalRoles { get; set; } = String.Empty;

		public int CacheTime { get; set; } = 0;

		public bool ShowTitle { get; set; } = true;

		public string ControlSource { get; set; } = string.Empty;

		public bool AvailableForMyPage { get; set; } = false;

		public bool AllowMultipleInstancesOnMyPage { get; set; } = true;

		public int CreatedByUserId { get; set; } = -1;

		public DateTime CreatedDate { get; set; } = DateTime.MinValue;

		public string FeatureName { get; private set; } = String.Empty;

		public string Icon { get; set; } = String.Empty;

		public bool HideFromAuthenticated { get; set; } = false;

		public bool HideFromUnauthenticated { get; set; } = false;

		public bool IncludeInSearch { get; set; } = true;

		public bool IsGlobal { get; set; } = false;

		#endregion

		#region Private Methods

		private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.ModuleId = Convert.ToInt32(reader["ModuleID"]);
                this.SiteId = Convert.ToInt32(reader["SiteID"]);
                //this.pageID = Convert.ToInt32(reader["PageID"]);
                this.ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
                //this.moduleOrder = Convert.ToInt32(reader["ModuleOrder"]);
                //this.paneName = reader["PaneName"].ToString();
                this.ModuleTitle = reader["ModuleTitle"].ToString();
                this.AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
                this.DraftEditRoles = reader["DraftEditRoles"].ToString();
                this.DraftApprovalRoles = reader["DraftApprovalRoles"].ToString();
                this.CacheTime = Convert.ToInt32(reader["CacheTime"]);
                this.ShowTitle = Convert.ToBoolean(reader["ShowTitle"]);
                if (reader["EditUserID"] != DBNull.Value)
                {
                    this.EditUserId = Convert.ToInt32(reader["EditUserID"]);
                }
                this.AvailableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
                this.AllowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
                if (reader["CreatedByUserID"] != DBNull.Value)
                {
                    this.CreatedByUserId = Convert.ToInt32(reader["CreatedByUserID"]);
                }
                if (reader["CreatedDate"] != DBNull.Value)
                {
                    this.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                }

                this.Icon = reader["Icon"].ToString();


                this.ModuleGuid = new Guid(reader["Guid"].ToString());
                this.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
                this.SiteGuid = new Guid(reader["SiteGuid"].ToString());

                string edUserGuid = reader["EditUserGuid"].ToString();
                if (edUserGuid.Length == 36) this.EditUserGuid = new Guid(edUserGuid);

                this.HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
                this.HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);

                this.ViewRoles = reader["ViewRoles"].ToString();

                //ModuleDefinition moduleDef = new ModuleDefinition(this.moduleDefID);
                //this.controlSource = moduleDef.ControlSrc;
                //this.featureName = moduleDef.FeatureName;

                this.ControlSource = reader["ControlSrc"].ToString();
                this.FeatureName = reader["FeatureName"].ToString();
               
                this.IncludeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
                this.IsGlobal = Convert.ToBoolean(reader["IsGlobal"]);
                this.HeadElement = reader["HeadElement"].ToString();

                this.PublishMode = Convert.ToInt32(reader["PublishMode"]);
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
			if(this.ModuleId > -1)
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
            
            if (this.ModuleGuid == Guid.Empty)// tni 2013-06-26 forced moduleGuid implementation
                this.ModuleGuid = Guid.NewGuid();

			newID = DBModule.AddModule(
				this.PageId, 
                this.SiteId,
                this.SiteGuid,
				this.ModuleDefId, 
				this.ModuleOrder, 
				this.PaneName, 
				this.ModuleTitle, 
                this.ViewRoles,
				this.AuthorizedEditRoles,
                this.DraftEditRoles,
                this.DraftApprovalRoles,
				this.CacheTime, 
				this.ShowTitle,
                this.AvailableForMyPage,
                this.AllowMultipleInstancesOnMyPage,
                this.Icon,
                this.CreatedByUserId,
                DateTime.UtcNow,
                this.ModuleGuid,
                this.FeatureGuid,
                this.HideFromAuthenticated,
                this.HideFromUnauthenticated,
                this.HeadElement,
                this.PublishMode); 
			
			this.ModuleId = newID;
			created = (newID > -1);
			if(created)
			{
				ModuleSettings.CreateDefaultModuleSettings(this.ModuleId);
			}
					
			return created;

		}

		private bool Update()
		{

			return DBModule.UpdateModule(
				this.ModuleId, 
				this.ModuleDefId, 
				this.ModuleTitle, 
                this.ViewRoles,
				this.AuthorizedEditRoles, 
                this.DraftEditRoles,
                this.DraftApprovalRoles,
				this.CacheTime, 
				this.ShowTitle,
				this.EditUserId,
                this.AvailableForMyPage,
                this.AllowMultipleInstancesOnMyPage,
                this.Icon,
                this.HideFromAuthenticated,
                this.HideFromUnauthenticated,
                this.IncludeInSearch,
                this.IsGlobal,
                this.HeadElement,
                this.PublishMode); 
				
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

		public static List<GlobalContent> GetGlobalContent(int siteId)
		{
			List<GlobalContent> globalContents = new List<GlobalContent>();
			using (IDataReader reader = DBModule.GetGlobalContent(siteId))
			{
				while (reader.Read())
				{
					globalContents.Add(new GlobalContent {
						ModuleGuid = Guid.Parse(reader["ModuleGuid"].ToString()),
						ModuleID = Convert.ToInt32(reader["ModuleId"]),
						ModuleTitle = reader["ModuleTitle"].ToString(),
						FeatureName = reader["FeatureName"].ToString(),
						ResourceFile = reader["ResourceFile"].ToString(),
						CreatedBy = reader["CreatedBy"].ToString(),
						CreatedById = Convert.ToInt32(reader["CreatedById"]),
						ControlSrc = reader["ContrlSrc"].ToString(),
						UseCount = Convert.ToInt32(reader["UseCount"])
					});
				}
			}
			return globalContents;
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

        public static List<Module> GetModuleListForSite(int siteId, Guid featureGuid)
        {
            List<Module> modules = new List<Module>();
            using (IDataReader reader = Module.GetModulesForSite(siteId, featureGuid))
            {
                while (reader.Read())
                {
                    Module module = new Module(Convert.ToInt32(reader["ModuleId"]));
                    if (module != null)
                    {
                        modules.Add(module);
                    }
                }
            }
            return modules;
        }
		#endregion

		public class GlobalContent
		{
			public int ModuleID { get; set; }
			public Guid ModuleGuid { get; set; }
			public string ModuleTitle { get; set; }
			public string FeatureName { get; set; }
			public string ResourceFile { get; set; }
			public string CreatedBy { get; set; }
			public int CreatedById { get; set; }
			public string ControlSrc { get; set; }
			public int UseCount { get; set; }
		}
	}
}
