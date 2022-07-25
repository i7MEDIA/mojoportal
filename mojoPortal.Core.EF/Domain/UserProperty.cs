using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_UserProperties")]
	public partial class UserProperty
	{
		[Key]
		public Guid PropertyID { get; set; }

		public Guid UserGuid { get; set; }

		[StringLength(255)]
		public string PropertyName { get; set; }

		public string PropertyValueString { get; set; }

		public byte[] PropertyValueBinary { get; set; }

		public DateTime LastUpdatedDate { get; set; }

		public bool IsLazyLoaded { get; set; }
	}
}
