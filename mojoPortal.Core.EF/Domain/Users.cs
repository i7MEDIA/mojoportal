using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_")]
	public partial class Users
	{
		[Key]
		public int UserID { get; set; }

		public int SiteID { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[StringLength(50)]
		public string LoginName { get; set; }

		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		[StringLength(100)]
		public string LoweredEmail { get; set; }

		[StringLength(255)]
		public string PasswordQuestion { get; set; }

		[StringLength(255)]
		public string PasswordAnswer { get; set; }

		[StringLength(10)]
		public string Gender { get; set; }

		public bool ProfileApproved { get; set; }

		public Guid? RegisterConfirmGuid { get; set; }

		public bool ApprovedForForums { get; set; }

		public bool Trusted { get; set; }

		public bool? DisplayInMemberList { get; set; }

		[StringLength(100)]
		public string WebSiteURL { get; set; }

		[StringLength(100)]
		public string Country { get; set; }

		[StringLength(100)]
		public string State { get; set; }

		[StringLength(100)]
		public string Occupation { get; set; }

		[StringLength(100)]
		public string Interests { get; set; }

		[StringLength(50)]
		public string MSN { get; set; }

		[StringLength(50)]
		public string Yahoo { get; set; }

		[StringLength(50)]
		public string AIM { get; set; }

		[StringLength(50)]
		public string ICQ { get; set; }

		public int TotalPosts { get; set; }

		[StringLength(255)]
		public string AvatarUrl { get; set; }

		public int TimeOffsetHours { get; set; }

		public string Signature { get; set; }

		public DateTime DateCreated { get; set; }

		public Guid? UserGuid { get; set; }

		[StringLength(100)]
		public string Skin { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? LastActivityDate { get; set; }

		public DateTime? LastLoginDate { get; set; }

		public DateTime? LastPasswordChangedDate { get; set; }

		public DateTime? LastLockoutDate { get; set; }

		public int? FailedPasswordAttemptCount { get; set; }

		public DateTime? FailedPwdAttemptWindowStart { get; set; }

		public int? FailedPwdAnswerAttemptCount { get; set; }

		public DateTime? FailedPwdAnswerWindowStart { get; set; }

		public bool IsLockedOut { get; set; }

		[StringLength(16)]
		public string MobilePIN { get; set; }

		[StringLength(128)]
		public string PasswordSalt { get; set; }

		public string Comment { get; set; }

		[StringLength(255)]
		public string OpenIDURI { get; set; }

		[StringLength(36)]
		public string WindowsLiveID { get; set; }

		public Guid? SiteGuid { get; set; }

		public decimal? TotalRevenue { get; set; }

		[StringLength(100)]
		public string FirstName { get; set; }

		[StringLength(100)]
		public string LastName { get; set; }

		[StringLength(1000)]
		public string Pwd { get; set; }

		public bool? MustChangePwd { get; set; }

		[StringLength(100)]
		public string NewEmail { get; set; }

		[StringLength(100)]
		public string EditorPreference { get; set; }

		public Guid? EmailChangeGuid { get; set; }

		[StringLength(32)]
		public string TimeZoneId { get; set; }

		public Guid? PasswordResetGuid { get; set; }

		public bool? RolesChanged { get; set; }

		public string AuthorBio { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public int PwdFormat { get; set; }

		public bool EmailConfirmed { get; set; }

		public string PasswordHash { get; set; }

		public string SecurityStamp { get; set; }

		[StringLength(50)]
		public string PhoneNumber { get; set; }

		public bool PhoneNumberConfirmed { get; set; }

		public bool TwoFactorEnabled { get; set; }

		public DateTime? LockoutEndDateUtc { get; set; }
	}
}
