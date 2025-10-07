using System.Collections.Generic;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// This class represents an instance of a ContentModule published on
/// a page. It is a bridge between page class and module class and corresponds
/// to the mp_PageModules table
/// </summary>
public class PageModule
{
	public int ModuleId { get; private set; } = -1;

	public Guid FeatureGuid { get; private set; } = Guid.Empty;

	public int PageId { get; private set; } = -1;

	public int ModuleOrder { get; private set; } = 999;
	public string PaneName { get; private set; } = String.Empty;

	public DateTime PublishBeginDate { get; private set; } = DateTime.MinValue;

	public DateTime PublishEndDate { get; private set; } = DateTime.MaxValue;

	public string ModuleTitle { get; } = string.Empty;

	public string PageName { get; private set; } = string.Empty;

	public string PageUrl { get; private set; } = string.Empty;

	private static List<PageModule> LoadListFromReader(IDataReader reader)
	{
		List<PageModule> pageModules = new List<PageModule>();

		while (reader.Read())
		{
			var pageModule = new PageModule();
			pageModule.ModuleId = Convert.ToInt32(reader["ModuleID"]);
			pageModule.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
			pageModule.PageId = Convert.ToInt32(reader["PageID"]);
			pageModule.PaneName = reader["PaneName"].ToString();
			pageModule.ModuleOrder = Convert.ToInt32(reader["ModuleOrder"]);

			if (reader["PublishBeginDate"] != DBNull.Value)
			{
				pageModule.PublishBeginDate
					= Convert.ToDateTime(reader["PublishBeginDate"]);
			}

			if (reader["PublishEndDate"] != DBNull.Value)
			{
				pageModule.PublishEndDate
					= Convert.ToDateTime(reader["PublishEndDate"]);
			}

			pageModule.PageName = reader["PageName"].ToString();
			bool useUrl = Convert.ToBoolean(reader["UseUrl"]);
			if (useUrl)
			{
				pageModule.PageUrl = reader["Url"].ToString();
			}
			else
			{
				pageModule.PageUrl = Invariant($"~/Default.aspx?pageid={pageModule.PageId}");
			}

			pageModules.Add(pageModule);
		}

		return pageModules;
	}


	/// <summary>
	/// Returns all PageModules for the given pageId
	/// including unpublished ones
	/// </summary>
	public static List<PageModule> GetPageModulesByPage(int pageId)
	{
		var pageModules = new List<PageModule>();

		using (IDataReader reader = DBModule.PageModuleGetReaderByPage(pageId))
		{
			pageModules = LoadListFromReader(reader);
		}

		return pageModules;
	}

	/// <summary>
	/// Returns all PageModules for the given pageId and featureGuid
	/// including unpublished ones
	/// </summary>
	public static List<PageModule> GetPageModules(int pageId, Guid featureGuid)
	{
		var pageModules = new List<PageModule>();

		using (IDataReader reader = DBModule.GetPageModules(pageId, featureGuid))
		{
			pageModules = LoadListFromReader(reader);
		}

		return pageModules;
	}

	public static List<PageModule> GetPageModulesByModule(int moduleId)
	{
		var pageModules = new List<PageModule>();

		using (IDataReader reader = DBModule.PageModuleGetReaderByModule(moduleId))
		{
			pageModules = LoadListFromReader(reader);
		}

		return pageModules;
	}
}
