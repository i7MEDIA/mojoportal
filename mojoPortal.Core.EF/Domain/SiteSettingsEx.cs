using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SiteSettingsEx")]
	public partial class SiteSettingsEx
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int SiteID { get; set; }

		public Guid SiteGuid { get; set; }

		[Key]
		[Column(Order = 1)]
		public string KeyName { get; set; }

		public string KeyValue { get; set; }

		[StringLength(128)]
		public string GroupName { get; set; }
	}
}
