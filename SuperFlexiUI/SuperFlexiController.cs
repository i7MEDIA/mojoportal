using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using SuperFlexiBusiness;
using mojoPortal.Business;
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
		// GET api/<controller>
		public IEnumerable<string> Get()
		{
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