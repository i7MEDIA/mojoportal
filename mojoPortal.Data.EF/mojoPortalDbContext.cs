using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace mojoPortal.Data.EF
{
	public partial class mojoPortalDbContext : DbContext
	{
		public mojoPortalDbContext() : base(ConnectionString.GetWriteConnectionString())
		{ }

		//public virtual DbSet<AuthorizeNetLog> AuthorizeNetLog { get; set; }
		public virtual DbSet<BannedIPAddresses> BannedIPAddresses { get; set; }
		//public virtual DbSet<Comments> Comments { get; set; }
		//public virtual DbSet<CommerceReport> CommerceReport { get; set; }
		//public virtual DbSet<CommerceReportOrders> CommerceReportOrders { get; set; }
		//public virtual DbSet<ContentHistory> ContentHistory { get; set; }
		//public virtual DbSet<ContentMeta> ContentMeta { get; set; }
		//public virtual DbSet<ContentMetaLink> ContentMetaLink { get; set; }
		//public virtual DbSet<ContentRating> ContentRating { get; set; }
		//public virtual DbSet<ContentStyle> ContentStyle { get; set; }
		//public virtual DbSet<ContentTemplate> ContentTemplate { get; set; }
		//public virtual DbSet<ContentWorkflow> ContentWorkflow { get; set; }
		//public virtual DbSet<ContentWorkflowAuditHistory> ContentWorkflowAuditHistory { get; set; }
		//public virtual DbSet<Currency> Currency { get; set; }
		//public virtual DbSet<EmailSendLog> EmailSendLog { get; set; }
		//public virtual DbSet<EmailSendQueue> EmailSendQueue { get; set; }
		//public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
		//public virtual DbSet<FileAttachment> FileAttachment { get; set; }
		//public virtual DbSet<FriendlyUrls> FriendlyUrls { get; set; }
		//public virtual DbSet<GeoCountry> GeoCountry { get; set; }
		//public virtual DbSet<GeoZone> GeoZone { get; set; }
		//public virtual DbSet<GoogleCheckoutLog> GoogleCheckoutLog { get; set; }
		//public virtual DbSet<HtmlContent> HtmlContent { get; set; }
		//public virtual DbSet<IndexingQueue> IndexingQueue { get; set; }
		//public virtual DbSet<Language> Language { get; set; }
		//public virtual DbSet<Letter> Letter { get; set; }
		//public virtual DbSet<LetterHtmlTemplate> LetterHtmlTemplate { get; set; }
		//public virtual DbSet<LetterInfo> LetterInfo { get; set; }
		//public virtual DbSet<LetterSendLog> LetterSendLog { get; set; }
		//public virtual DbSet<LetterSubscribe> LetterSubscribe { get; set; }
		//public virtual DbSet<LetterSubscribeHx> LetterSubscribeHx { get; set; }
		//public virtual DbSet<ModuleDefinitions> ModuleDefinitions { get; set; }
		//public virtual DbSet<ModuleDefinitionSettings> ModuleDefinitionSettings { get; set; }
		//public virtual DbSet<Modules> Modules { get; set; }
		//public virtual DbSet<ModuleSettings> ModuleSettings { get; set; }
		//public virtual DbSet<PageModules> PageModules { get; set; }
		//public virtual DbSet<Pages> Pages { get; set; }
		//public virtual DbSet<PaymentLog> PaymentLog { get; set; }
		//public virtual DbSet<PayPalLog> PayPalLog { get; set; }
		//public virtual DbSet<PlugNPayLog> PlugNPayLog { get; set; }
		//public virtual DbSet<RedirectList> RedirectList { get; set; }
		//public virtual DbSet<Roles> Roles { get; set; }
		//public virtual DbSet<SavedQuery> SavedQuery { get; set; }
		//public virtual DbSet<SchemaScriptHistory> SchemaScriptHistory { get; set; }
		//public virtual DbSet<SchemaVersion> SchemaVersion { get; set; }
		//public virtual DbSet<SiteFolders> SiteFolders { get; set; }
		//public virtual DbSet<SiteHosts> SiteHosts { get; set; }
		//public virtual DbSet<SiteModuleDefinitions> SiteModuleDefinitions { get; set; }
		//public virtual DbSet<SitePaths> SitePaths { get; set; }
		//public virtual DbSet<SitePersonalizationAllUsers> SitePersonalizationAllUsers { get; set; }
		//public virtual DbSet<SitePersonalizationPerUser> SitePersonalizationPerUser { get; set; }
		//public virtual DbSet<Sites> Sites { get; set; }
		//public virtual DbSet<SiteSettingsEx> SiteSettingsEx { get; set; }
		//public virtual DbSet<SiteSettingsExDef> SiteSettingsExDef { get; set; }
		//public virtual DbSet<SystemLog> SystemLog { get; set; }
		//public virtual DbSet<Tag> Tag { get; set; }
		//public virtual DbSet<TagItem> TagItem { get; set; }
		//public virtual DbSet<TagVocabulary> TagVocabulary { get; set; }
		//public virtual DbSet<TaskQueue> TaskQueue { get; set; }
		//public virtual DbSet<TaxClass> TaxClass { get; set; }
		//public virtual DbSet<TaxRate> TaxRate { get; set; }
		//public virtual DbSet<TaxRateHistory> TaxRateHistory { get; set; }
		//public virtual DbSet<UserClaims> UserClaims { get; set; }
		//public virtual DbSet<UserLocation> UserLocation { get; set; }
		//public virtual DbSet<UserLogins> UserLogins { get; set; }
		//public virtual DbSet<UserPages> UserPages { get; set; }
		//public virtual DbSet<UserProperties> UserProperties { get; set; }
		//public virtual DbSet<UserRoles> UserRoles { get; set; }
		//public virtual DbSet<Users> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.ResponseCode)
			//		.IsFixedLength();

			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.CcvCode)
			//		.IsFixedLength();

			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.CavCode)
			//		.IsFixedLength();

			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.Amount)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.Tax)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.Duty)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<AuthorizeNetLog>()
			//		.Property(e => e.Freight)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<CommerceReport>()
			//		.Property(e => e.Price)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<CommerceReport>()
			//		.Property(e => e.SubTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<CommerceReportOrders>()
			//		.Property(e => e.SubTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<CommerceReportOrders>()
			//		.Property(e => e.TaxTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<CommerceReportOrders>()
			//		.Property(e => e.ShippingTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<CommerceReportOrders>()
			//		.Property(e => e.OrderTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<ContentWorkflow>()
			//		.HasMany(e => e.ContentWorkflowAuditHistory)
			//		.WithRequired(e => e.ContentWorkflow)
			//		.HasForeignKey(e => e.ContentWorkflowGuid)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<Currency>()
			//		.Property(e => e.Code)
			//		.IsFixedLength();

			//	modelBuilder.Entity<Currency>()
			//		.Property(e => e.DecimalPointChar)
			//		.IsFixedLength();

			//	modelBuilder.Entity<Currency>()
			//		.Property(e => e.ThousandsPointChar)
			//		.IsFixedLength();

			//	modelBuilder.Entity<Currency>()
			//		.Property(e => e.DecimalPlaces)
			//		.IsFixedLength();

			//	modelBuilder.Entity<Currency>()
			//		.Property(e => e.Value)
			//		.HasPrecision(13, 8);

			//	modelBuilder.Entity<GeoCountry>()
			//		.Property(e => e.ISOCode2)
			//		.IsFixedLength();

			//	modelBuilder.Entity<GeoCountry>()
			//		.Property(e => e.ISOCode3)
			//		.IsFixedLength();

			//	modelBuilder.Entity<GeoCountry>()
			//		.HasMany(e => e.GeoZone)
			//		.WithRequired(e => e.GeoCountry)
			//		.HasForeignKey(e => e.CountryGuid)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.AuthAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.DiscountTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.ShippingTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.TaxTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.OrderTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.LatestChgAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.TotalChgAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.LatestRefundAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.TotalRefundAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.LatestChargeback)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<GoogleCheckoutLog>()
			//		.Property(e => e.TotalChargeback)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<Language>()
			//		.Property(e => e.Code)
			//		.IsFixedLength();

			//	modelBuilder.Entity<LetterInfo>()
			//		.HasMany(e => e.Letter)
			//		.WithRequired(e => e.LetterInfo)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<Modules>()
			//		.HasMany(e => e.PageModules)
			//		.WithRequired(e => e.Modules)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<Pages>()
			//		.HasMany(e => e.PageModules)
			//		.WithRequired(e => e.Pages)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.ResponseCode)
			//		.IsFixedLength();

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.CcvCode)
			//		.IsFixedLength();

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.CavCode)
			//		.IsFixedLength();

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.Amount)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.Tax)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.Duty)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PaymentLog>()
			//		.Property(e => e.Freight)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PayPalLog>()
			//		.Property(e => e.ExchangeRate)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PayPalLog>()
			//		.Property(e => e.CartTotal)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PayPalLog>()
			//		.Property(e => e.PayPalAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PayPalLog>()
			//		.Property(e => e.TaxAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PayPalLog>()
			//		.Property(e => e.FeeAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PayPalLog>()
			//		.Property(e => e.SettleAmt)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PlugNPayLog>()
			//		.Property(e => e.Amount)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PlugNPayLog>()
			//		.Property(e => e.Tax)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PlugNPayLog>()
			//		.Property(e => e.Duty)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<PlugNPayLog>()
			//		.Property(e => e.Freight)
			//		.HasPrecision(15, 4);

			//	modelBuilder.Entity<SchemaVersion>()
			//		.HasMany(e => e.SchemaScriptHistory)
			//		.WithRequired(e => e.SchemaVersion)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<Sites>()
			//		.HasMany(e => e.SitePaths)
			//		.WithRequired(e => e.Sites)
			//		.WillCascadeOnDelete(false);

			//	modelBuilder.Entity<Tag>()
			//		.HasMany(e => e.TagItem)
			//		.WithRequired(e => e.Tag)
			//		.HasForeignKey(e => e.TagGuid);

			//	modelBuilder.Entity<TaxRate>()
			//		.Property(e => e.Rate)
			//		.HasPrecision(18, 4);

			//	modelBuilder.Entity<TaxRateHistory>()
			//		.Property(e => e.Rate)
			//		.HasPrecision(18, 4);

			//	modelBuilder.Entity<Users>()
			//		.Property(e => e.Gender)
			//		.IsFixedLength();

			//	modelBuilder.Entity<Users>()
			//		.Property(e => e.TotalRevenue)
			//		.HasPrecision(15, 4);
		}
	}
}
