using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperFlexiUI.Models
{
	public class SiteModel
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public string CacheKey { get; set; }
		public Guid CacheGuid { get; set; }
		public string PhysAppRoot { get; set; }
		public string SitePath { get; set; }
		public string SiteUrl { get; set; }
		//public string MediaPath { get; set; }
		public string SkinPath { get; set; }
		public string SkinViewPath { get; set; }
		public TimeZoneInfo TimeZone { get; set; }
	}
}