using System;

namespace mojoPortal.Business
{
	class TagVocabulary
	{
		#region Public Properties

		public Guid Guid { get; set; } = Guid.Empty;
		public Guid SiteGuid { get; set; } = Guid.Empty;
		public Guid FeatureGuid { get; set; } = Guid.Empty;
		public Guid ModuleGuid { get; set; } = Guid.Empty;
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime CreatedUtc { get; set; } = DateTime.Now;
		public Guid CreatedBy { get; set; } = Guid.Empty;
		public DateTime ModifiedUtc { get; set; } = DateTime.Now;
		public Guid ModifiedBy { get; set; } = Guid.Empty;

		#endregion


		#region Constructors

		public TagVocabulary()
		{}

		#endregion
	}
}