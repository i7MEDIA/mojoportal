using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentWorkflowAuditHistory")]
	public partial class ContentWorkflowAuditHistory
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid ContentWorkflowGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public Guid UserGuid { get; set; }

		public DateTime CreatedDateUtc { get; set; }

		[StringLength(20)]
		public string NewStatus { get; set; }

		public string Notes { get; set; }

		public bool? Active { get; set; }

		public virtual ContentWorkflow ContentWorkflow { get; set; }
	}
}
