using System;


namespace mojoPortal.Business
{
	public class Tag
	{
		#region Public Properties

		public Guid Guid { get; set; } = Guid.Empty;
		public Guid SiteGuid { get; set; } = Guid.Empty;
		public Guid FeatureGuid { get; set; } = Guid.Empty;
		public Guid ModuleGuid { get; set; } = Guid.Empty;
		public string TagText { get; set; } = string.Empty;
		public int ItemCount { get; set; } = 0;
		public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
		public Guid CreatedBy { get; set; } = Guid.Empty;
		public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
		public Guid ModifiedBy { get; set; } = Guid.Empty;
		public Guid VocabularyGuid { get; set; } = Guid.Empty;

		#endregion


		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of Tag by text.
		/// </summary>
		/// <param name="tag1"> tag1 </param>
		/// <param name="tag2"> tag2 </param>
		/// <returns>int</returns>
		public static int CompareByTag(Tag tag1, Tag tag2) => tag1.TagText.CompareTo(tag2.TagText);


		/// <summary>
		/// Compares 2 instances of Tag by created date.
		/// </summary>
		/// <param name="tag1"> tag1 </param>
		/// <param name="tag2"> tag2 </param>
		/// <returns>int</returns>
		public static int CompareByCreatedUtc(Tag tag1, Tag tag2) => tag1.CreatedUtc.CompareTo(tag2.CreatedUtc);


		/// <summary>
		/// Compares 2 instances of Tag by modified date.
		/// </summary>
		/// <param name="tag1"> tag1 </param>
		/// <param name="tag2"> tag2 </param>
		/// <returns>int</returns>
		public static int CompareByModifiedUtc(Tag tag1, Tag tag2) => tag1.ModifiedUtc.CompareTo(tag2.ModifiedUtc);

		#endregion


		#region Constructors

		public Tag()
		{ }

		#endregion
	}
}