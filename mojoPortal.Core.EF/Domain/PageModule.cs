using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_PageModules")]
	public partial class PageModule
	{
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int PageID { get; set; }

		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ModuleID { get; set; }

		[Required]
		[StringLength(50)]
		public string PaneName { get; set; }

		public int ModuleOrder { get; set; }

		public DateTime PublishBeginDate { get; set; }

		public DateTime? PublishEndDate { get; set; }

		public Guid? PageGuid { get; set; }

		public Guid? ModuleGuid { get; set; }

		public virtual Module Modules { get; set; }

		public virtual Page Pages { get; set; }
	}
}
