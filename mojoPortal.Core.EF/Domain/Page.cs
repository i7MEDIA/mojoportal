using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoPortal.Core.EF.Domain
{
	[Table("mp_Pages")]
	public partial class Page
	{
		public Page()
		{
			PageModules = new HashSet<PageModule>();
		}

		[Key]
		public int PageID { get; set; }

		public int? ParentID { get; set; }

		public int PageOrder { get; set; }

		public int SiteID { get; set; }

		[StringLength(255)]
		public string PageName { get; set; }

		[StringLength(255)]
		public string PageTitle { get; set; }

		public string AuthorizedRoles { get; set; }

		public string EditRoles { get; set; }

		public string CreateChildPageRoles { get; set; }

		public bool RequireSSL { get; set; }

		public bool AllowBrowserCache { get; set; }

		public bool ShowBreadcrumbs { get; set; }

		[StringLength(1000)]
		public string PageKeyWords { get; set; }

		[StringLength(255)]
		public string PageDescription { get; set; }

		[StringLength(255)]
		public string PageEncoding { get; set; }

		[StringLength(255)]
		public string AdditionalMetaTags { get; set; }

		[StringLength(50)]
		public string MenuImage { get; set; }

		public bool UseUrl { get; set; }

		[StringLength(255)]
		public string Url { get; set; }

		public bool OpenInNewWindow { get; set; }

		public bool ShowChildPageMenu { get; set; }

		public bool ShowChildBreadCrumbs { get; set; }

		[StringLength(100)]
		public string Skin { get; set; }

		public bool HideMainMenu { get; set; }

		public bool IncludeInMenu { get; set; }

		[StringLength(20)]
		public string ChangeFrequency { get; set; }

		[StringLength(10)]
		public string SiteMapPriority { get; set; }

		public DateTime? LastModifiedUTC { get; set; }

		public Guid PageGuid { get; set; }

		public Guid ParentGuid { get; set; }

		public bool HideAfterLogin { get; set; }

		public Guid? SiteGuid { get; set; }

		public string CompiledMeta { get; set; }

		public DateTime? CompiledMetaUtc { get; set; }

		public bool? IncludeInSiteMap { get; set; }

		public bool? IsClickable { get; set; }

		public bool? ShowHomeCrumb { get; set; }

		public string DraftEditRoles { get; set; }

		public bool IsPending { get; set; }

		[StringLength(255)]
		public string CanonicalOverride { get; set; }

		public bool? IncludeInSearchMap { get; set; }

		public bool? EnableComments { get; set; }

		public string CreateChildDraftRoles { get; set; }

		public bool? IncludeInChildSiteMap { get; set; }

		public Guid? PubTeamId { get; set; }

		[StringLength(50)]
		public string BodyCssClass { get; set; }

		[StringLength(50)]
		public string MenuCssClass { get; set; }

		public bool? ExpandOnSiteMap { get; set; }

		public int? PublishMode { get; set; }

		public DateTime PCreatedUtc { get; set; }

		public Guid? PCreatedBy { get; set; }

		[StringLength(36)]
		public string PCreatedFromIp { get; set; }

		public DateTime PLastModUtc { get; set; }

		public Guid? PLastModBy { get; set; }

		[StringLength(36)]
		public string PLastModFromIp { get; set; }

		public string MenuDesc { get; set; }

		public string DraftApprovalRoles { get; set; }

		[StringLength(20)]
		public string LinkRel { get; set; }

		[StringLength(255)]
		public string PageHeading { get; set; }

		public bool ShowPageHeading { get; set; }

		public DateTime PubDateUtc { get; set; }

		public virtual ICollection<PageModule> PageModules { get; set; }

		public virtual Site Sites { get; set; }
	}
}
