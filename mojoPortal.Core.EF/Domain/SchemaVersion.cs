using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SchemaVersion")]
	public partial class SchemaVersion
	{
		public SchemaVersion()
		{
			SchemaScriptHistory = new HashSet<SchemaScriptHistory>();
		}

		[Key]
		public Guid ApplicationID { get; set; }

		[Required]
		[StringLength(255)]
		public string ApplicationName { get; set; }

		public int Major { get; set; }

		public int Minor { get; set; }

		public int Build { get; set; }

		public int Revision { get; set; }

		public virtual ICollection<SchemaScriptHistory> SchemaScriptHistory { get; set; }
	}
}
