using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SystemLog")]
	public partial class SystemLog
	{
		public int ID { get; set; }

		public DateTime LogDate { get; set; }

		[StringLength(50)]
		public string IpAddress { get; set; }

		[StringLength(10)]
		public string Culture { get; set; }

		public string Url { get; set; }

		[StringLength(255)]
		public string ShortUrl { get; set; }

		[Required]
		[StringLength(255)]
		public string Thread { get; set; }

		[Required]
		[StringLength(20)]
		public string LogLevel { get; set; }

		[Required]
		[StringLength(255)]
		public string Logger { get; set; }

		[Required]
		public string Message { get; set; }
	}
}
