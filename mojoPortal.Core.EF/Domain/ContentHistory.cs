using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentHistory")]
	public partial class ContentHistory
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid UserGuid { get; set; }

		public Guid ContentGuid { get; set; }

		[StringLength(255)]
		public string Title { get; set; }

		public string ContentText { get; set; }

		public string CustomData { get; set; }

		public DateTime CreatedUtc { get; set; }

		public DateTime HistoryUtc { get; set; }
	}
}
