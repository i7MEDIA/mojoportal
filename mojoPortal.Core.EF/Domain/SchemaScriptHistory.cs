using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_SchemaScriptHistory")]
	public partial class SchemaScriptHistory
	{
		public int ID { get; set; }

		public Guid ApplicationID { get; set; }

		[Required]
		[StringLength(255)]
		public string ScriptFile { get; set; }

		public DateTime RunTime { get; set; }

		public bool ErrorOccurred { get; set; }

		public string ErrorMessage { get; set; }

		public string ScriptBody { get; set; }

		public virtual SchemaVersion SchemaVersion { get; set; }
	}
}
