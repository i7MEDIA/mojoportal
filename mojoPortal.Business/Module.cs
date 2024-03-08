using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// Represents an instance of a feature
/// </summary>
public class Module : IComparable
{

	#region Constructors

	public Module() { }

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
		PageId = pageId;
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

	public string PaneName { get; set; } = string.Empty;

	public string ModuleTitle { get; set; } = string.Empty;

	public string HeadElement { get; set; } = "h2";

	public string ViewRoles { get; set; } = "All Users;";

	public string AuthorizedEditRoles { get; set; } = string.Empty;

	public string DraftEditRoles { get; set; } = string.Empty;

	public string DraftApprovalRoles { get; set; } = string.Empty;

	public int CacheTime { get; set; } = 0;

	public bool ShowTitle { get; set; } = true;

	public string ControlSource { get; set; } = string.Empty;

	public bool AvailableForMyPage { get; set; } = false;

	public bool AllowMultipleInstancesOnMyPage { get; set; } = true;

	public int CreatedByUserId { get; set; } = -1;

	public DateTime CreatedDate { get; set; } = DateTime.MinValue;

	public string FeatureName { get; private set; } = string.Empty;

	public string Icon { get; set; } = string.Empty;

	public bool HideFromAuthenticated { get; set; } = false;

	public bool HideFromUnauthenticated { get; set; } = false;

	public bool IncludeInSearch { get; set; } = true;

	public bool IsGlobal { get; set; } = false;

	public string AnchorName { get; set; } = string.Empty;

	#endregion

	#region Private Methods

	private void PopulateFromReader(IDataReader reader)
	{
		if (reader.Read())
		{
			ModuleId = Convert.ToInt32(reader["ModuleID"]);
			SiteId = Convert.ToInt32(reader["SiteID"]);
			//this.pageID = Convert.ToInt32(reader["PageID"]);
			ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
			//this.moduleOrder = Convert.ToInt32(reader["ModuleOrder"]);
			//this.paneName = reader["PaneName"].ToString();
			ModuleTitle = reader["ModuleTitle"].ToString();
			AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
			DraftEditRoles = reader["DraftEditRoles"].ToString();
			DraftApprovalRoles = reader["DraftApprovalRoles"].ToString();
			CacheTime = Convert.ToInt32(reader["CacheTime"]);
			ShowTitle = Convert.ToBoolean(reader["ShowTitle"]);
			if (reader["EditUserID"] != DBNull.Value)
			{
				EditUserId = Convert.ToInt32(reader["EditUserID"]);
			}
			AvailableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
			AllowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
			if (reader["CreatedByUserID"] != DBNull.Value)
			{
				CreatedByUserId = Convert.ToInt32(reader["CreatedByUserID"]);
			}
			if (reader["CreatedDate"] != DBNull.Value)
			{
				CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
			}

			Icon = reader["Icon"].ToString();

			ModuleGuid = new Guid(reader["Guid"].ToString());
			FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
			SiteGuid = new Guid(reader["SiteGuid"].ToString());

			string edUserGuid = reader["EditUserGuid"].ToString();
			if (edUserGuid.Length == 36) EditUserGuid = new Guid(edUserGuid);

			HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
			HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);

			ViewRoles = reader["ViewRoles"].ToString();

			//ModuleDefinition moduleDef = new ModuleDefinition(this.moduleDefID);
			//this.controlSource = moduleDef.ControlSrc;
			//this.featureName = moduleDef.FeatureName;

			ControlSource = reader["ControlSrc"].ToString();
			FeatureName = reader["FeatureName"].ToString();

			IncludeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
			IsGlobal = Convert.ToBoolean(reader["IsGlobal"]);
			HeadElement = reader["HeadElement"].ToString();

			PublishMode = Convert.ToInt32(reader["PublishMode"]);

			if (reader["AnchorName"] != DBNull.Value)
			{
				AnchorName = reader["AnchorName"].ToString();
			}
		}
	}

	private void GetModule(Guid moduleGuid)
	{
		using IDataReader reader = DBModule.GetModule(moduleGuid);
		PopulateFromReader(reader);
	}

	private void GetModule(int moduleId)
	{
		using IDataReader reader = DBModule.GetModule(moduleId);
		PopulateFromReader(reader);

	}

	private void GetModule(int moduleId, int pageId)
	{
		using IDataReader reader = DBModule.GetModule(moduleId, pageId);
		PopulateFromReader(reader);
	}

	#endregion

	#region Public Methods

	public int CompareTo(object value)
	{

		if (value == null)
		{
			return 1;
		}

		int compareOrder = ((Module)value).ModuleOrder;

		if (ModuleOrder == compareOrder)
		{
			return 0;
		}

		if (ModuleOrder < compareOrder)
		{
			return -1;
		}

		if (ModuleOrder > compareOrder)
		{
			return 1;
		}

		return 0;
	}

	public bool Save()
	{
		if (ModuleId > -1)
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
		// tni 2013-06-26 forced moduleGuid implementation
		if (ModuleGuid == Guid.Empty)
		{
			ModuleGuid = Guid.NewGuid();
		}

		int newID = DBModule.AddModule(
						PageId,
						SiteId,
						SiteGuid,
						ModuleDefId,
						ModuleOrder,
						PaneName,
						ModuleTitle,
						ViewRoles,
						AuthorizedEditRoles,
						DraftEditRoles,
						DraftApprovalRoles,
						CacheTime,
						ShowTitle,
						AvailableForMyPage,
						AllowMultipleInstancesOnMyPage,
						Icon,
						CreatedByUserId,
						DateTime.UtcNow,
						ModuleGuid,
						FeatureGuid,
						HideFromAuthenticated,
						HideFromUnauthenticated,
						HeadElement,
						PublishMode);
		ModuleId = newID;
		bool created = newID > -1;
		if (created)
		{
			ModuleSettings.CreateDefaultModuleSettings(ModuleId);
		}

		return created;
	}

	private bool Update() => DBModule.UpdateModule(
						ModuleId,
						ModuleDefId,
						ModuleTitle,
						ViewRoles,
						AuthorizedEditRoles,
						DraftEditRoles,
						DraftApprovalRoles,
						CacheTime,
						ShowTitle,
						EditUserId,
						AvailableForMyPage,
						AllowMultipleInstancesOnMyPage,
						Icon,
						HideFromAuthenticated,
						HideFromUnauthenticated,
						IncludeInSearch,
						IsGlobal,
						HeadElement,
						PublishMode);

	#endregion


	#region Static Methods

	/// <summary>
	/// Returns a DataReader of published pagemodules
	/// </summary>
	public static IDataReader GetPageModules(int pageId) => DBModule.GetPageModules(pageId);

	public static DataTable GetPageModulesTable(int moduleId) => DBModule.PageModuleGetByModule(moduleId);

	public static void DeletePageModules(int pageId) => DBModule.PageModuleDeleteByPage(pageId);

	public static bool UpdateModuleOrder(int pageId, int moduleId, int moduleOrder, string paneName) => DBModule.UpdateModuleOrder(pageId, moduleId, moduleOrder, paneName);

	public static bool DeleteModule(int moduleId)
	{
		ModuleSettings.DeleteModuleSettings(moduleId);
		return DBModule.DeleteModule(moduleId);
	}

	public static bool DeleteModuleInstance(int moduleId, int pageId) => DBModule.DeleteModuleInstance(moduleId, pageId);

	public static bool Publish(
		Guid pageGuid,
		Guid moduleGuid,
		int moduleId,
		int pageId,
		string paneName,
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

	public static bool UpdatePage(int oldPageId, int newPageId, int moduleId) => DBModule.UpdatePage(oldPageId, newPageId, moduleId);

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

	public static int GetGlobalCount(int siteId, int moduleDefId, int pageId) => DBModule.GetGlobalCount(siteId, moduleDefId, pageId);

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
		var globalContents = new List<GlobalContent>();
		using (IDataReader reader = DBModule.GetGlobalContent(siteId))
		{
			while (reader.Read())
			{
				globalContents.Add(new GlobalContent
				{
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

	public static IDataReader GetMyPageModules(int siteId) => DBModule.GetMyPageModules(siteId);

	public static bool UpdateCountOfUseOnMyPage(int moduleId, int increment) => DBModule.UpdateCountOfUseOnMyPage(moduleId, increment);

	public static IDataReader GetModulesForSite(int siteId, Guid featureGuid) => DBModule.GetModulesForSite(siteId, featureGuid);

	public static int GetCountByFeature(int moduleDefId) => DBModule.GetCountByFeature(moduleDefId);

	public static List<Module> GetModuleListForSite(int siteId, Guid featureGuid)
	{
		var modules = new List<Module>();
		using (IDataReader reader = GetModulesForSite(siteId, featureGuid))
		{
			while (reader.Read())
			{
				var module = new Module(Convert.ToInt32(reader["ModuleId"]));
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