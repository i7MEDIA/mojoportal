using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_GeoCountry")]
	public partial class GeoCountry
	{
		public GeoCountry()
		{
			GeoZone = new HashSet<GeoZone>();
		}

		[Key]
		public Guid Guid { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Required]
		[StringLength(2)]
		public string ISOCode2 { get; set; }

		[Required]
		[StringLength(3)]
		public string ISOCode3 { get; set; }

		public virtual ICollection<GeoZone> GeoZone { get; set; }
	}
}
