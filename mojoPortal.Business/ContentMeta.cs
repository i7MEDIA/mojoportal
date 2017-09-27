// Author:
// Created:       2009-12-02
// Last Modified: 2017-09-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;

namespace mojoPortal.Business
{
	public class ContentMeta
	{
		public ContentMeta()
		{ }

		#region Private Properties

		private Guid guid = Guid.Empty;
		private Guid siteGuid = Guid.Empty;
		private Guid moduleGuid = Guid.Empty;
		private Guid contentGuid = Guid.Empty;
		private string name = string.Empty;
		private string nameProperty = "name";
		private string scheme = string.Empty;
		private string langCode = string.Empty;
		private string dir = string.Empty;
		private string metaContent = string.Empty;
		private string contentProperty = "content";
		private int sortRank = 0;
		private DateTime createdUtc = DateTime.UtcNow;
		private Guid createdBy = Guid.Empty;
		private DateTime lastModUtc = DateTime.UtcNow;
		private Guid lastModBy = Guid.Empty;

		#endregion

		#region Public Properties

		public Guid Guid
		{
			get { return guid; }
			set { guid = value; }
		}
		public Guid SiteGuid
		{
			get { return siteGuid; }
			set { siteGuid = value; }
		}
		public Guid ModuleGuid
		{
			get { return moduleGuid; }
			set { moduleGuid = value; }
		}
		public Guid ContentGuid
		{
			get { return contentGuid; }
			set { contentGuid = value; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		public string NameProperty
		{
			get { return nameProperty; }
			set { nameProperty = value; }
		}
		public string Scheme
		{
			get { return scheme; }
			set { scheme = value; }
		}
		public string LangCode
		{
			get { return langCode; }
			set { langCode = value; }
		}
		public string Dir
		{
			get { return dir; }
			set { dir = value; }
		}
		public string MetaContent
		{
			get { return metaContent; }
			set { metaContent = value; }
		}
		public string ContentProperty
		{
			get { return contentProperty; }
			set { contentProperty = value; }
		}
		public int SortRank
		{
			get { return sortRank; }
			set { sortRank = value; }
		}
		public DateTime CreatedUtc
		{
			get { return createdUtc; }
			set { createdUtc = value; }
		}
		public Guid CreatedBy
		{
			get { return createdBy; }
			set { createdBy = value; }
		}
		public DateTime LastModUtc
		{
			get { return lastModUtc; }
			set { lastModUtc = value; }
		}
		public Guid LastModBy
		{
			get { return lastModBy; }
			set { lastModBy = value; }
		}

		#endregion

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByName(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.Name.CompareTo(contentMeta2.Name);
		}
		/// <summary>
		/// Compares 2 instances of NameProperty.
		/// </summary>
		public static int CompareByNameProperty(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.NameProperty.CompareTo(contentMeta2.NameProperty);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByScheme(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.Scheme.CompareTo(contentMeta2.Scheme);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByLangCode(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.LangCode.CompareTo(contentMeta2.LangCode);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByDir(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.Dir.CompareTo(contentMeta2.Dir);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByMetaContent(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.MetaContent.CompareTo(contentMeta2.MetaContent);
		}
		/// <summary>
		/// Compares 2 instances of ContentProperty.
		/// </summary>
		public static int CompareByMetaContentProperty(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.ContentProperty.CompareTo(contentMeta2.ContentProperty);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareBySortRank(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.SortRank.CompareTo(contentMeta2.SortRank);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByCreatedUtc(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.CreatedUtc.CompareTo(contentMeta2.CreatedUtc);
		}
		/// <summary>
		/// Compares 2 instances of ContentMeta.
		/// </summary>
		public static int CompareByLastModUtc(ContentMeta contentMeta1, ContentMeta contentMeta2)
		{
			return contentMeta1.LastModUtc.CompareTo(contentMeta2.LastModUtc);
		}

		#endregion

	}
}
