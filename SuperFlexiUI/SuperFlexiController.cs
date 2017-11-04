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


	}

	public class ReturnObject
	{
		public string Status { get; set; }
		public object Data { get; set; }
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
			List<Item> items = Item.GetPageOfModuleItems(
				module.ModuleGuid, 
				r.PageNumber, 
				r.PageSize, 
				out totalPages, 
				out totalRows,
				r.SearchTerm, 
				r.SearchField, 
				r.SortDescending);
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
					var popItem = new PopulatedItem(item, fields, values);
					if (popItem != null)
					{
						popItems.Add(popItem);
					}
				}
			}

			if (sfObject.Items.Count > 0)
			{
				return new ReturnObject()
				{
					Status = "success",
					Data = sfObject,
					ExtraData = new Dictionary<string, string>
					{
						["TotalPages"] = totalPages.ToString(),
						["TotalRows"] = totalRows.ToString()
					}
				};
			}

			return new ReturnObject()
			{
				Status = "error",
				Data = sfObject,
				ExtraData = new Dictionary<string,string>
				{
					["ErrorCode"] = "200",
					["ErrorMessage"] = "No Items Found"
				}
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
	}
}