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

	public class DataRequest
	{
		[HttpBindRequired]
		public int PageId { get; set; }
		[HttpBindRequired]
		public int ModuleId { get; set; }
		[HttpBindRequired]
		public int PageNumber { get; set; }
		[HttpBindRequired]
		public int PageSize { get; set; }
		[HttpBindRequired]
		public int ItemId { get; set; }
		[HttpBindRequired]
		public string SortDirection { get; set; }
		[HttpBindRequired]
		public string SortField { get; set; }
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
		public IEnumerable<string> GetModuleItems(int moduleId, int pageId, int pageIndex, int pageSize)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentPage = new PageSettings(siteSettings.SiteId, pageId);
			bool allowed = false;
			if (currentPage != null)
			{
				allowed = WebUser.IsInRoles(currentPage.AuthorizedRoles);
			}
			module = new Module(moduleId);
			if (module != null)
			{
				allowed = WebUser.IsInRoles(module.ViewRoles);
			}
			if (!allowed) return new string[] { "error"};

			config = new ModuleConfiguration(module);

			List<Item> items = Item.GetModuleItems(moduleId);
			if (items != null && items.Count > 0)
			{
				List<Field> fields = Field.GetAllForDefinition(config.FieldDefinitionGuid);
				var itemGuids = items.Select(x => x.ItemGuid).ToList();
				List<ItemFieldValue> values = ItemFieldValue.GetByItemGuids(itemGuids);

				foreach (Item item in items)
				{

				}
			}
			

			return new string[] { "value1", "value2" };
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public void Post([FromBody]DataRequest dataRequest)
		{


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