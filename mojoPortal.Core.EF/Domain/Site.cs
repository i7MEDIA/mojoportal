using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Sites")]
	public partial class Site
	{
		public Site()
		{
			Pages = new HashSet<Page>();
			Roles = new HashSet<Role>();
			SitePaths = new HashSet<SitePath>();
		}

		[Key]
		public int SiteID { get; set; }

		public Guid SiteGuid { get; set; }

		[StringLength(50)]
		public string SiteAlias { get; set; }

		[Required]
		[StringLength(255)]
		public string SiteName { get; set; }

		[StringLength(100)]
		public string Skin { get; set; }

		[StringLength(50)]
		public string Logo { get; set; }

		[StringLength(50)]
		public string Icon { get; set; }

		public bool AllowUserSkins { get; set; }

		public bool AllowPageSkins { get; set; }

		public bool AllowHideMenuOnPages { get; set; }

		public bool AllowNewRegistration { get; set; }

		public bool UseSecureRegistration { get; set; }

		public bool UseSSLOnAllPages { get; set; }

		[StringLength(255)]
		public string DefaultPageKeyWords { get; set; }

		[StringLength(255)]
		public string DefaultPageDescription { get; set; }

		[StringLength(255)]
		public string DefaultPageEncoding { get; set; }

		[StringLength(255)]
		public string DefaultAdditionalMetaTags { get; set; }

		public bool IsServerAdminSite { get; set; }

		public bool UseLdapAuth { get; set; }

		public bool AutoCreateLdapUserOnFirstLogin { get; set; }

		[StringLength(255)]
		public string LdapServer { get; set; }

		public int LdapPort { get; set; }

		[StringLength(255)]
		public string LdapDomain { get; set; }

		[StringLength(255)]
		public string LdapRootDN { get; set; }

		[Required]
		[StringLength(10)]
		public string LdapUserDNKey { get; set; }

		public bool ReallyDeleteUsers { get; set; }

		public bool UseEmailForLogin { get; set; }

		public bool AllowUserFullNameChange { get; set; }

		[Required]
		[StringLength(50)]
		public string EditorSkin { get; set; }

		[Required]
		[StringLength(50)]
		public string DefaultFriendlyUrlPatternEnum { get; set; }

		public bool AllowPasswordRetrieval { get; set; }

		public bool AllowPasswordReset { get; set; }

		public bool RequiresQuestionAndAnswer { get; set; }

		public int MaxInvalidPasswordAttempts { get; set; }

		public int PasswordAttemptWindowMinutes { get; set; }

		public bool RequiresUniqueEmail { get; set; }

		public int PasswordFormat { get; set; }

		public int MinRequiredPasswordLength { get; set; }

		public int MinReqNonAlphaChars { get; set; }

		public string PwdStrengthRegex { get; set; }

		[StringLength(100)]
		public string DefaultEmailFromAddress { get; set; }

		public bool EnableMyPageFeature { get; set; }

		[StringLength(255)]
		public string EditorProvider { get; set; }

		[StringLength(255)]
		public string CaptchaProvider { get; set; }

		[StringLength(255)]
		public string DatePickerProvider { get; set; }

		[StringLength(255)]
		public string RecaptchaPrivateKey { get; set; }

		[StringLength(255)]
		public string RecaptchaPublicKey { get; set; }

		[StringLength(255)]
		public string WordpressAPIKey { get; set; }

		[StringLength(255)]
		public string WindowsLiveAppID { get; set; }

		[StringLength(255)]
		public string WindowsLiveKey { get; set; }

		public bool AllowOpenIDAuth { get; set; }

		public bool AllowWindowsLiveAuth { get; set; }

		[StringLength(255)]
		public string GmapApiKey { get; set; }

		[StringLength(255)]
		public string ApiKeyExtra1 { get; set; }

		[StringLength(255)]
		public string ApiKeyExtra2 { get; set; }

		[StringLength(255)]
		public string ApiKeyExtra3 { get; set; }

		[StringLength(255)]
		public string ApiKeyExtra4 { get; set; }

		[StringLength(255)]
		public string ApiKeyExtra5 { get; set; }

		public bool? DisableDbAuth { get; set; }

		public virtual ICollection<Page> Pages { get; set; }

		public virtual ICollection<Role> Roles { get; set; }

		public virtual ICollection<SitePath> SitePaths { get; set; }
	}
}
