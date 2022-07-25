using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_ModuleDefinitionSettings")]
	public partial class ModuleDefinitionSettings
	{
		public int ID { get; set; }

		public int ModuleDefID { get; set; }

		[Required]
		[StringLength(50)]
		public string SettingName { get; set; }

		public string SettingValue { get; set; }

		[StringLength(50)]
		public string ControlType { get; set; }

		public string RegexValidationExpression { get; set; }

		public Guid FeatureGuid { get; set; }

		[StringLength(255)]
		public string ResourceFile { get; set; }

		[StringLength(255)]
		public string ControlSrc { get; set; }

		public int SortOrder { get; set; }

		[StringLength(255)]
		public string HelpKey { get; set; }

		[StringLength(255)]
		public string GroupName { get; set; }

		[Column(TypeName = "ntext")]
		[Required]
		public string Attributes { get; set; }

		[Column(TypeName = "ntext")]
		[Required]
		public string Options { get; set; }
	}
}
