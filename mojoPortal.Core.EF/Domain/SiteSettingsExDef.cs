using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SiteSettings")]
	public partial class SiteSettingsExDef
	{
		[Key]
		public string KeyName { get; set; }

		[StringLength(128)]
		public string GroupName { get; set; }

		public string DefaultValue { get; set; }

		public int SortOrder { get; set; }
	}
}
