using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_TaxRateHistory")]
	public partial class TaxRateHistory
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid TaxRateGuid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid GeoZoneGuid { get; set; }

		public Guid TaxClassGuid { get; set; }

		public int Priority { get; set; }

		public decimal Rate { get; set; }

		[StringLength(255)]
		public string Description { get; set; }

		public DateTime Created { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime? LastModified { get; set; }

		public Guid? ModifiedBy { get; set; }

		public DateTime LogTime { get; set; }
	}
}
