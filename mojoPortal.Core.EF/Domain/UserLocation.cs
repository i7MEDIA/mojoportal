using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_UserLocation")]
	public partial class UserLocation
	{
		[Key]
		public Guid RowID { get; set; }

		public Guid UserGuid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(50)]
		public string IPAddress { get; set; }

		public long IPAddressLong { get; set; }

		[StringLength(255)]
		public string Hostname { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }

		[StringLength(255)]
		public string ISP { get; set; }

		[StringLength(255)]
		public string Continent { get; set; }

		[StringLength(255)]
		public string Country { get; set; }

		[StringLength(255)]
		public string Region { get; set; }

		[StringLength(255)]
		public string City { get; set; }

		[StringLength(255)]
		public string TimeZone { get; set; }

		public int CaptureCount { get; set; }

		public DateTime FirstCaptureUTC { get; set; }

		public DateTime LastCaptureUTC { get; set; }
	}
}
