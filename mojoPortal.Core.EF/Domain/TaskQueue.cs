using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_TaskQueue")]
	public partial class TaskQueue
	{
		[Key]
		public Guid Guid { get; set; }

		public Guid SiteGuid { get; set; }

		public Guid QueuedBy { get; set; }

		[Required]
		[StringLength(255)]
		public string TaskName { get; set; }

		public bool NotifyOnCompletion { get; set; }

		[StringLength(255)]
		public string NotificationToEmail { get; set; }

		[StringLength(255)]
		public string NotificationFromEmail { get; set; }

		[StringLength(255)]
		public string NotificationSubject { get; set; }

		public string TaskCompleteMessage { get; set; }

		public DateTime? NotificationSentUTC { get; set; }

		public bool CanStop { get; set; }

		public bool CanResume { get; set; }

		public int UpdateFrequency { get; set; }

		public DateTime QueuedUTC { get; set; }

		public DateTime? StartUTC { get; set; }

		public DateTime? CompleteUTC { get; set; }

		public DateTime? LastStatusUpdateUTC { get; set; }

		public double CompleteRatio { get; set; }

		[StringLength(255)]
		public string Status { get; set; }

		public string SerializedTaskObject { get; set; }

		[StringLength(255)]
		public string SerializedTaskType { get; set; }
	}
}
