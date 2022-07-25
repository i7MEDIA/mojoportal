using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SiteFolders")]
	public partial class SiteFolder
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string FolderName { get; set; }
	}
}
