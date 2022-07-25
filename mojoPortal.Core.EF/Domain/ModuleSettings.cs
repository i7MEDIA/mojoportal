using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ModuleSettings")]
	public partial class ModuleSettings
	{
		public int ID { get; set; }

		public int ModuleID { get; set; }

		[Required]
		[StringLength(50)]
		public string SettingName { get; set; }

		public string SettingValue { get; set; }

		[StringLength(50)]
		public string ControlType { get; set; }

		public string RegexValidationExpression { get; set; }

		public Guid? SettingGuid { get; set; }

		public Guid? ModuleGuid { get; set; }

		[StringLength(255)]
		public string ControlSrc { get; set; }

		public int SortOrder { get; set; }

		[StringLength(255)]
		public string HelpKey { get; set; }

		public virtual Module Modules { get; set; }
	}
}
