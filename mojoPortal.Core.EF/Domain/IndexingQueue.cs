using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_IndexingQueue")]
	public partial class IndexingQueue
	{
		[Key]
		public long RowId { get; set; }

		[Required]
		[StringLength(255)]
		public string IndexPath { get; set; }

		public string SerializedItem { get; set; }

		[Required]
		[StringLength(255)]
		public string ItemKey { get; set; }

		public bool RemoveOnly { get; set; }

		public int SiteID { get; set; }
	}
}
