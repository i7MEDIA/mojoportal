using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mojoPortal.Business
{
	public class SystemInfo
	{
		public int SiteCount { get; set; } = 1;
		public KeyValuePair<string, Guid> SiteMappings { get; set; }
	}
}
