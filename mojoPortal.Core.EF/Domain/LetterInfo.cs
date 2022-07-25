using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_LetterInfo")]
	public partial class LetterInfo
	{
		public LetterInfo()
		{
			Letter = new HashSet<Letter>();
		}

		[Key]
		public Guid LetterInfoGuid { get; set; }

		public Guid SiteGuid { get; set; }

		[Required]
		[StringLength(255)]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		public string AvailableToRoles { get; set; }

		public bool Enabled { get; set; }

		public bool AllowUserFeedback { get; set; }

		public bool AllowAnonFeedback { get; set; }

		[Required]
		[StringLength(255)]
		public string FromAddress { get; set; }

		[Required]
		[StringLength(255)]
		public string FromName { get; set; }

		[Required]
		[StringLength(255)]
		public string ReplyToAddress { get; set; }

		public int SendMode { get; set; }

		public bool EnableViewAsWebPage { get; set; }

		public bool EnableSendLog { get; set; }

		public string RolesThatCanEdit { get; set; }

		public string RolesThatCanApprove { get; set; }

		public string RolesThatCanSend { get; set; }

		public int SubscriberCount { get; set; }

		public DateTime CreatedUTC { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime LastModUTC { get; set; }

		public Guid LastModBy { get; set; }

		public bool? AllowArchiveView { get; set; }

		public bool? ProfileOptIn { get; set; }

		public int? SortRank { get; set; }

		public int? UnVerifiedCount { get; set; }

		[StringLength(50)]
		public string DisplayNameDefault { get; set; }

		[StringLength(50)]
		public string FirstNameDefault { get; set; }

		[StringLength(50)]
		public string LastNameDefault { get; set; }

		public virtual ICollection<Letter> Letter { get; set; }
	}
}
