using System.ComponentModel.DataAnnotations;

namespace mojoPortal.Core.EF.Domain
{
	public class BaseEntity
	{
		[Key]
		public int ID { get; set; }
	}
}
