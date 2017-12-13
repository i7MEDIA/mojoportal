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
		private int pageId = -1;
		public int PageId { get => pageId; set => pageId = value; }

		private int moduleId = -1;
		public int ModuleId { get => moduleId; set => moduleId = value; }

		private int pageNumber = 1;
		public int PageNumber { get => pageNumber; set => pageNumber = value; }

		private int pageSize = 10;
		public int PageSize { get => pageSize; set => pageSize = value; }

		private int itemId = -1;
		public int ItemId { get => itemId; set => itemId = value; }

		private bool sortDescending = false;
		public bool SortDescending { get => sortDescending; set => sortDescending = value; }

		private string sortField = string.Empty;
		public string SortField { get => sortField; set => sortField = value; }

		private string searchField = string.Empty;
		public string SearchField { get => searchField; set => searchField = value; }

		private string searchTerm = string.Empty;
		public string SearchTerm { get => searchTerm; set => searchTerm = value; }

		private IDictionary<string,string> searchObject;
		public IDictionary<string,string> SearchObject { get => searchObject; set => searchObject = value; }

		private string field = string.Empty;
		public string Field { get => field; set => field = value; }
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
							items.AddRange(Item.GetPageOfModuleItems(
								module.ModuleGuid,
								1,
								99999,
								out totalPages,
								out totalRows,
								setA,
								set.Key
							));
						}
					}
					else
					{
						items.AddRange(Item.GetPageOfModuleItems(
							module.ModuleGuid,
							1,
							99999,
							out totalPages,
							out totalRows,
							set.Value,
							set.Key
						));
					}
					//we have to figure out paging with this

				}
				items = items.Distinct(new SimpleItemComparer()).ToList();
			}
			else
			{
				items = Item.GetPageOfModuleItems(
				module.ModuleGuid,
				r.PageNumber,
				r.PageSize,
				out totalPages,
				out totalRows,
				r.SearchTerm,
				r.SearchField,
				r.SortDescending);
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

				foreach (Item item in items)
				{
					var popItem = new PopulatedItem(item, fields, values.Where(v => v.ItemGuid == item.ItemGuid).ToList(), canEdit);
					if (popItem != null)
					{
						if (r.SearchObject != null && r.SearchObject.Count > 0)
						{
							int matchCount = 0;
							foreach (var set in r.SearchObject)
							{
								var value = popItem.Values[set.Key];
								List<string> itemValArray = value as List<string>;
								List<string> setValArray = set.Value.SplitOnCharAndTrim(';');
								if (value.ToString().ToLower().IndexOf(set.Value.ToLower()) >= 0 
									|| (itemValArray != null && itemValArray.Any(s => s.Equals(set.Value, StringComparison.OrdinalIgnoreCase)))
									|| (setValArray != null && setValArray.Any(s => s.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase))))
								{
									matchCount++;
								}
							}

							if (matchCount == r.SearchObject.Count)
							{
								popItems.Add(popItem);
							}
						}
						else
						{
							popItems.Add(popItem);
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
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public void Post([FromBody]RequestObject dataRequest)
		{

			var foo = string.Empty;
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
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