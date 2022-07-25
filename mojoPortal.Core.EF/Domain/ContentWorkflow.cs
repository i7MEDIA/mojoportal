using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ContentWorkflow")]
	public partial class ContentWorkflow
	{
		public ContentWorkflow()
		{
			ContentWorkflowAuditHistory = new HashSet<ContentWorkflowAuditHistory>();
		}

		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid ModuleGuid { get; set; }

		public DateTime CreatedDateUtc { get; set; }

		public Guid UserGuid { get; set; }

		public Guid? LastModUserGuid { get; set; }

		public DateTime? LastModUtc { get; set; }

		[Required]
		[StringLength(20)]
		public string Status { get; set; }

		public string ContentText { get; set; }

		public string CustomData { get; set; }

		public int? CustomReferenceNumber { get; set; }

		public Guid? CustomReferenceGuid { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ContentWorkflowAuditHistory> ContentWorkflowAuditHistory { get; set; }
	}
}
