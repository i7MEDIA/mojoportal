using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using SuperFlexiBusiness;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web;

namespace SuperFlexiUI
{

	public class RequestObject
	{
		public int PageId { get; set; } = -1;
		public int ModuleId { get; set; } = -1;
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public int ItemId { get; set; } = -1;
		public bool SortDescending { get; set; } = false;
		public string SortField { get; set; } = string.Empty;
		public string SearchField { get; set; } = string.Empty;
		public string SearchTerm { get; set; } = string.Empty;
		public IDictionary<string, string> SearchObject { get; set; }
		public string Field { get; set; } = string.Empty;
		//public Guid SolutionGuid { get; set; } = Guid.Empty;
		public bool GetAllForSolution { get; set; } = false;
	}

	public class ReturnObject
	{
		public string Status { get; set; }
		public object Data { get; set; }
		public int TotalPages { get; set; }
		public int TotalRows { get; set; }
		public bool AllowEdit { get; set; }
		public int CmsPageId { get; set; }
		public int CmsModuleId { get; set; }
		public IDictionary<string,string> ExtraData { get; set; }
	}

	public class SuperFlexiObject
	{
		public string FriendlyName { get; set; }
		public string ModuleTitle { get; set; }
		public int GlobalSortOrder { get; set; }
		public List<PopulatedItem> Items { get; set; }
	}

	public class SuperFlexiController : ApiController
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SuperFlexiController));

		private Module module = null;
		private SiteSettings siteSettings = null;
		private PageSettings currentPage = null;
		private SiteUser postUser = null;
		private ModuleConfiguration config = new ModuleConfiguration();
		
		[HttpPost]
		public ReturnObject GetModuleItems(RequestObject r)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentPage = new PageSettings(siteSettings.SiteId, r.PageId);
			bool allowed = false;
			bool canEdit = false;
			if (currentPage != null)
			{
				allowed = WebUser.IsInRoles(currentPage.AuthorizedRoles);
			}
			module = new Module(r.ModuleId);
			if (module != null)
			{
				allowed = WebUser.IsInRoles(module.ViewRoles);
			}
			if (!allowed)
			{
				return new ReturnObject()
				{
					Status = "error",
					ExtraData = new Dictionary<string, string>
					{
						["ErrorCode"] = "100",
						["ErrorMessage"] = "Not Allowed"
					}
				};
			}

			config = new ModuleConfiguration(module);

			int totalPages = 0;
			int totalRows = 0;
			List<Item> items = new List<Item>();
			if (r.SearchObject != null && r.SearchObject.Count > 0)
			{
				foreach (var set in r.SearchObject)
				{
					if (set.Value.Contains(";"))
					{
						foreach (var setA in set.Value.SplitOnCharAndTrim(';'))
						{
							items.AddRange(GetItems(
								module.ModuleGuid,
								1,
								99999,
								out totalPages,
								out totalRows,
								setA,
								set.Key,
								r.GetAllForSolution
							));
						}
					}
					else
					{
						items.AddRange(GetItems(
							module.ModuleGuid,
							1,
							99999,
							out totalPages,
							out totalRows,
							set.Value,
							set.Key,
							r.GetAllForSolution
						));
					}
					//we have to figure out paging with this

				}
				items = items.Distinct(new SimpleItemComparer()).ToList();
			}
			else
			{
				items.AddRange(GetItems(
					module.ModuleGuid,
					r.PageNumber,
					r.PageSize,
					out totalPages,
					out totalRows,
					r.SearchTerm,
					r.SearchField,
					r.GetAllForSolution,
					r.SortDescending));
			}
			
			List<PopulatedItem> popItems = new List<PopulatedItem>();
			SuperFlexiObject sfObject = new SuperFlexiObject()
			{
				FriendlyName = config.ModuleFriendlyName,
				ModuleTitle = module.ModuleTitle,
				GlobalSortOrder = config.GlobalViewSortOrder,
				Items = popItems
			};

			if (items != null && items.Count > 0)
			{
				List<Field> fields = Field.GetAllForDefinition(config.FieldDefinitionGuid);
				var itemGuids = items.Select(x => x.ItemGuid).ToList();
				List<ItemFieldValue> values = ItemFieldValue.GetByItemGuids(itemGuids);
				Module itemModule = null;
				Guid currentModuleGuid = Guid.Empty;
				foreach (Item item in items.OrderBy(x => x.ModuleID).ToList())
				{
					if (item.ModuleGuid != currentModuleGuid)
					{
						currentModuleGuid = item.ModuleGuid;
						itemModule = new Module(item.ModuleGuid);
					}
					if (itemModule == null) continue;
					if (!WebUser.IsInRoles(itemModule.ViewRoles)) continue;
					var populatedItem = new PopulatedItem(item, fields, values.Where(v => v.ItemGuid == item.ItemGuid).ToList(), canEdit);
					if (populatedItem != null)
					{
						if (r.SearchObject != null && r.SearchObject.Count > 0)
						{
							int matchCount = 0;
							foreach (var searchItem in r.SearchObject)
							{
								var value = populatedItem.Values[searchItem.Key];
								List<string> itemValArray = value as List<string>;
								List<string> searchItemValArray = searchItem.Value.SplitOnCharAndTrim(';');
								//log.Info($"[{searchItem.Key}]={searchItem.Value}");

								/*  Check if itemValArray == null because if it is, that means the value is just a plain value, not a List<string>.
								 *  If we try to do a comparison on value.ToString() when value is a List<string>, .ToString() returns System.Collections.Generic...
								 *  and then our comparison is actually looking for matches in "System.Collections.Generic...". We had that happen with the word
								 *  "Collections". Oops.
								 */

								if ((itemValArray == null && value.ToString().ToLower().IndexOf(searchItem.Value.ToLower()) >= 0 )
									|| (itemValArray != null && itemValArray.Any(s => s.Equals(searchItem.Value, StringComparison.OrdinalIgnoreCase)))
									|| (searchItemValArray != null && searchItemValArray.Any(s => s.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))))
								{

									matchCount++;
								}
							}

							if (matchCount == r.SearchObject.Count)
							{
								popItems.Add(populatedItem);
							}
						}
						else
						{
							popItems.Add(populatedItem);
						}
					}
				}
			}

			return new ReturnObject()
			{
				Status = "success",
				Data = sfObject,
				TotalPages = totalPages,
				TotalRows = totalRows == popItems.Count ? totalRows : popItems.Count,
				AllowEdit = ShouldAllowEdit(),
				CmsModuleId = module.ModuleId,
				CmsPageId = module.PageId
			};
		}


		[HttpPost]
		public ReturnObject GetFieldValues(RequestObject r)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentPage = new PageSettings(siteSettings.SiteId, r.PageId);
			bool allowed = false;
			if (currentPage != null)
			{
				allowed = WebUser.IsInRoles(currentPage.AuthorizedRoles);
			}
			module = new Module(r.ModuleId);
			if (module != null)
			{
				allowed = WebUser.IsInRoles(module.ViewRoles);
			}
			if (!allowed)
			{
				return new ReturnObject()
				{
					Status = "error",
					ExtraData = new Dictionary<string, string>
					{
						["ErrorCode"] = "100",
						["ErrorMessage"] = "Not Allowed"
					}
				};
			}

			config = new ModuleConfiguration(module);

			int totalPages = 0;
			int totalRows = 0;

			var fieldValues = ItemFieldValue.GetPageOfValues(
				module.ModuleGuid,
				config.FieldDefinitionGuid,
				r.Field,
				r.PageNumber,
				r.PageSize,
				out totalPages,
				out totalRows);

			//much of the below is temporary, we needed to implement in a hurry
			//to-do: implement distinct on sql side
			List<string> values = new List<string>();

			var dbField = new Field(fieldValues.Select(fv => fv.FieldGuid).FirstOrDefault());
			switch (dbField.ControlType)
			{
				case "DynamicCheckBoxList":
					foreach (var val in fieldValues)
					{
						values.AddRange(val.FieldValue.SplitOnCharAndTrim(';'));
					}
					break;
				default:
					values = fieldValues.Select(fv => fv.FieldValue).ToList();
					break;
					//we will add the other cases later
			}

			values = values.Distinct().OrderBy(v => v).ToList();

			totalRows = values.Count();

			return new ReturnObject()
			{
				Status = "success",
				Data = values,
				TotalPages = totalPages,
				TotalRows = totalRows
			};
		}
		// GET api/<controller>/5
		//public string Get(int id)
		//{
		//	return "value";
		//}

		// POST api/<controller>
		//public void Post([FromBody]RequestObject dataRequest)
		//{

		//	var foo = string.Empty;
		//}

		//// PUT api/<controller>/5
		//public void Put(int id, [FromBody]string value)
		//{
		//}

		//// DELETE api/<controller>/5
		//public void Delete(int id)
		//{
		//}

		private List<Item> GetItems(
			Guid moduleGuid,
			int pageNumber,
			int pageSize,
			out int totalPages,
			out int totalRows,
			string searchTerm = "",
			string searchField = "",
			bool byDefinition = false,
			bool descending = false)
		{

			if (byDefinition)
			{
				return Item.GetPageForDefinition(
					config.FieldDefinitionGuid,
					siteSettings.SiteGuid,
					1,
					99999,
					out totalPages,
					out totalRows,
					searchTerm,
					searchField
				);

			}
			else
			{
				return Item.GetPageOfModuleItems(
					moduleGuid,
					1,
					99999,
					out totalPages,
					out totalRows,
					searchTerm,
					searchField
				);
			}
		}

		private bool ShouldAllowEdit()
		{
			if (WebUser.IsAdmin)
			{
				return true;
			}

			if (module != null)
			{
				if (module.AuthorizedEditRoles == "Admins;") { return false; }
				if (currentPage.EditRoles == "Admins;") { return false; }

				if (WebUser.IsContentAdmin) { return true; }

				if (SiteUtils.UserIsSiteEditor()) { return true; }

				if (WebUser.IsInRoles(module.AuthorizedEditRoles)) { return true; }

				if ((!module.IsGlobal) && WebUser.IsInRoles(currentPage.EditRoles)) { return true; }

			}

			return false;
		}
	}
}