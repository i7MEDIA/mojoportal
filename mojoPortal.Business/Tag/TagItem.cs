using System;


namespace mojoPortal.Business
{
	public class TagItem
	{
		#region Public Properties

		public Guid TagItemGuid { get; set; } = Guid.Empty;
		public Guid TagGuid { get; set; } = Guid.Empty;
		public Guid RelatedItemGuid { get; set; } = Guid.Empty;
		public Guid SiteGuid { get; set; } = Guid.Empty;
		public Guid FeatureGuid { get; set; } = Guid.Empty;
		public Guid ModuleGuid { get; set; } = Guid.Empty;
		public Guid ExtraGuid { get; set; } = Guid.Empty;
		public Guid TaggedBy { get; set; } = Guid.Empty;
		public string TagText { get; set; } = string.Empty;

		#endregion


		#region Constructors

		public TagItem()
		{ }

		#endregion
	}
}