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

using mojoPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mojoPortal.Business
{
	public class ContentMetaRespository
	{
		public ContentMetaRespository() { }

		/// <summary>
		/// Persists a ContentMeta
		/// </summary>
		/// <returns></returns>
		public void Save(ContentMeta contentMeta)
		{
			if (contentMeta == null)
			{
				return;
			}

			if (contentMeta.Guid == Guid.Empty)
			{
				contentMeta.Guid = Guid.NewGuid();

				DBContentMeta.Create(
					contentMeta.Guid,
					contentMeta.SiteGuid,
					contentMeta.ModuleGuid,
					contentMeta.ContentGuid,
					contentMeta.Name,
					contentMeta.NameProperty,
					contentMeta.Scheme,
					contentMeta.LangCode,
					contentMeta.Dir,
					contentMeta.MetaContent,
					contentMeta.ContentProperty,
					contentMeta.SortRank,
					contentMeta.CreatedUtc,
					contentMeta.CreatedBy
				);
			}
			else
			{
				DBContentMeta.Update(
					contentMeta.Guid,
					contentMeta.Name,
					contentMeta.NameProperty,
					contentMeta.Scheme,
					contentMeta.LangCode,
					contentMeta.Dir,
					contentMeta.MetaContent,
					contentMeta.ContentProperty,
					contentMeta.SortRank,
					contentMeta.LastModUtc,
					contentMeta.LastModBy
				);
			}
		}


		/// <summary>
		/// Persists a new instance of ContentMetaLink.
		/// </summary>
		/// <returns></returns>
		public void Save(ContentMetaLink contentMetaLink)
		{
			if (contentMetaLink == null) { return; }

			if (contentMetaLink.Guid == Guid.Empty)
			{
				contentMetaLink.Guid = Guid.NewGuid();

				DBContentMetaLink.Create(
					contentMetaLink.Guid,
					contentMetaLink.SiteGuid,
					contentMetaLink.ModuleGuid,
					contentMetaLink.ContentGuid,
					contentMetaLink.Rel,
					contentMetaLink.Href,
					contentMetaLink.HrefLang,
					contentMetaLink.Rev,
					contentMetaLink.Type,
					contentMetaLink.Media,
					contentMetaLink.SortRank,
					contentMetaLink.CreatedUtc,
					contentMetaLink.CreatedBy
				);
			}
			else
			{
				DBContentMetaLink.Update(
					contentMetaLink.Guid,
					contentMetaLink.Rel,
					contentMetaLink.Href,
					contentMetaLink.HrefLang,
					contentMetaLink.Rev,
					contentMetaLink.Type,
					contentMetaLink.Media,
					contentMetaLink.SortRank,
					contentMetaLink.LastModUtc,
					contentMetaLink.LastModBy
				);
			}
		}


		public ContentMeta Fetch(Guid guid)
		{
			using (IDataReader reader = DBContentMeta.GetOne(guid))
			{
				if (reader.Read())
				{
					ContentMeta contentMeta = new ContentMeta();

					contentMeta.Guid = new Guid(reader["Guid"].ToString());
					contentMeta.SiteGuid = new Guid(reader["SiteGuid"].ToString());
					contentMeta.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
					contentMeta.ContentGuid = new Guid(reader["ContentGuid"].ToString());
					contentMeta.Name = reader["Name"].ToString();
					contentMeta.NameProperty = reader["NameProperty"].ToString();
					contentMeta.Scheme = reader["Scheme"].ToString();
					contentMeta.LangCode = reader["LangCode"].ToString();
					contentMeta.Dir = reader["Dir"].ToString();
					contentMeta.MetaContent = reader["MetaContent"].ToString();
					contentMeta.ContentProperty = reader["ContentProperty"].ToString();
					contentMeta.SortRank = Convert.ToInt32(reader["SortRank"]);
					contentMeta.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					contentMeta.CreatedBy = new Guid(reader["CreatedBy"].ToString());
					contentMeta.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
					contentMeta.LastModBy = new Guid(reader["LastModBy"].ToString());

					return contentMeta;
				}
			}

			return null;
		}


		public ContentMetaLink FetchLink(Guid guid)
		{
			using (IDataReader reader = DBContentMetaLink.GetOne(guid))
			{
				if (reader.Read())
				{
					ContentMetaLink contentMetaLink = new ContentMetaLink();

					contentMetaLink.Guid = new Guid(reader["Guid"].ToString());
					contentMetaLink.SiteGuid = new Guid(reader["SiteGuid"].ToString());
					contentMetaLink.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
					contentMetaLink.ContentGuid = new Guid(reader["ContentGuid"].ToString());
					contentMetaLink.Rel = reader["Rel"].ToString();
					contentMetaLink.Href = reader["Href"].ToString();
					contentMetaLink.HrefLang = reader["HrefLang"].ToString();
					contentMetaLink.Rev = reader["Rev"].ToString();
					contentMetaLink.Type = reader["Type"].ToString();
					contentMetaLink.Media = reader["Media"].ToString();
					contentMetaLink.SortRank = Convert.ToInt32(reader["SortRank"]);
					contentMetaLink.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					contentMetaLink.CreatedBy = new Guid(reader["CreatedBy"].ToString());
					contentMetaLink.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
					contentMetaLink.LastModBy = new Guid(reader["LastModBy"].ToString());

					return contentMetaLink;
				}
			}

			return null;
		}


		public List<ContentMeta> FetchByContent(Guid contentGuid)
		{
			IDataReader reader = DBContentMeta.GetByContent(contentGuid);

			return LoadListFromReader(reader);
		}


		public List<ContentMetaLink> FetchLinksByContent(Guid contentGuid)
		{
			IDataReader reader = DBContentMetaLink.GetByContent(contentGuid);

			return LoadLinkListFromReader(reader);
		}


		/// <summary>
		/// Deletes an instance of ContentMeta. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Delete(Guid guid)
		{
			return DBContentMeta.Delete(guid);
		}


		/// <summary>
		/// Deletes an instance of ContentMeta. Returns true on success.
		/// </summary>
		public bool DeleteLink(Guid guid)
		{
			return DBContentMetaLink.Delete(guid);
		}


		/// <summary>
		/// Deletes ContentMeta. Returns true on success.
		/// </summary>
		public bool DeleteBySite(Guid siteGuid)
		{
			DBContentMetaLink.DeleteBySite(siteGuid);

			return DBContentMeta.DeleteBySite(siteGuid);
		}

		/// <summary>
		/// Deletes ContentMeta. Returns true on success.
		/// </summary>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <returns>bool</returns>
		public bool DeleteByModule(Guid moduleGuid)
		{
			DBContentMetaLink.DeleteByModule(moduleGuid);

			return DBContentMeta.DeleteByModule(moduleGuid);
		}

		/// <summary>
		/// Deletes ContentMeta. Returns true on success.
		/// </summary>
		/// <param name="contentGuid"> contentGuid </param>
		/// <returns>bool</returns>
		public bool DeleteByContent(Guid contentGuid)
		{
			DBContentMetaLink.DeleteByContent(contentGuid);

			return DBContentMeta.DeleteByContent(contentGuid);
		}


		/// <summary>
		/// gets the next sort rank
		/// </summary>
		/// <param name="contentGuid"></param>
		/// <returns>int</returns>
		public int GetNextSortRank(Guid contentGuid)
		{
			int nextSort = DBContentMeta.GetMaxSortRank(contentGuid) + 2;

			return nextSort;
		}


		/// <summary>
		/// gets the next sort rank
		/// </summary>
		/// <param name="contentGuid"></param>
		/// <returns>int</returns>
		public int GetNextLinkSortRank(Guid contentGuid)
		{
			int nextSort = DBContentMetaLink.GetMaxSortRank(contentGuid) + 2;

			return nextSort;
		}


		public void ResortMeta(List<ContentMeta> metaList)
		{
			int i = 3;

			foreach (ContentMeta meta in metaList)
			{
				meta.SortRank = i;
				Save(meta);
				i += 2;
			}
		}


		public void ResortMeta(List<ContentMetaLink> metaList)
		{
			int i = 3;

			foreach (ContentMetaLink meta in metaList)
			{
				meta.SortRank = i;
				Save(meta);
				i += 2;
			}
		}


		public string GetMetaString(Guid contentGuid)
		{
			if (contentGuid == null)
			{
				return string.Empty;
			}

			if (contentGuid == Guid.Empty)
			{
				return string.Empty;
			}

			List<ContentMeta> metaList = FetchByContent(contentGuid);

			List<ContentMetaLink> metaLinkList = FetchLinksByContent(contentGuid);

			StringBuilder metaString = new StringBuilder();

			foreach (ContentMetaLink meta in metaLinkList)
			{
				metaString.Append("\r\n");
				metaString.Append("<link ");
				metaString.Append("rel=\"" + meta.Rel.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");

				if (meta.HrefLang.Length > 0)
				{
					metaString.Append("hreflang=\"" + meta.HrefLang.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");
				}

				metaString.Append("href=\"" + meta.Href.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");
				metaString.Append(" />");
			}

			foreach (ContentMeta meta in metaList)
			{
				metaString.Append("\r\n");
				metaString.Append("<meta ");
				metaString.Append(meta.NameProperty + "=\"" + meta.Name.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");

				if (meta.Scheme.Length > 0)
				{
					metaString.Append("scheme=\"" + meta.Scheme.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");
				}

				metaString.Append(meta.ContentProperty + "=\"" + meta.MetaContent.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");

				if (meta.LangCode.Length > 0)
				{
					metaString.Append("lang=\"" + meta.LangCode.Replace("'", "&#39;").Replace("\"", "&#34;") + "\" ");
				}

				if (meta.Dir.Length > 0)
				{
					metaString.Append("dir=\"" + meta.Dir + "\" ");
				}

				metaString.Append(" />");
			}

			return metaString.ToString();
		}


		private List<ContentMeta> LoadListFromReader(IDataReader reader)
		{
			List<ContentMeta> contentMetaList = new List<ContentMeta>();

			try
			{
				while (reader.Read())
				{
					ContentMeta contentMeta = new ContentMeta();

					contentMeta.Guid = new Guid(reader["Guid"].ToString());
					contentMeta.SiteGuid = new Guid(reader["SiteGuid"].ToString());
					contentMeta.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
					contentMeta.ContentGuid = new Guid(reader["ContentGuid"].ToString());
					contentMeta.Name = reader["Name"].ToString();
					contentMeta.NameProperty = reader["NameProperty"].ToString();
					contentMeta.Scheme = reader["Scheme"].ToString();
					contentMeta.LangCode = reader["LangCode"].ToString();
					contentMeta.Dir = reader["Dir"].ToString();
					contentMeta.MetaContent = reader["MetaContent"].ToString();
					contentMeta.ContentProperty = reader["ContentProperty"].ToString();
					contentMeta.SortRank = Convert.ToInt32(reader["SortRank"]);
					contentMeta.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					contentMeta.CreatedBy = new Guid(reader["CreatedBy"].ToString());
					contentMeta.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
					contentMeta.LastModBy = new Guid(reader["LastModBy"].ToString());

					contentMetaList.Add(contentMeta);
				}
			}
			finally
			{
				reader.Close();
			}

			return contentMetaList;
		}

		private List<ContentMetaLink> LoadLinkListFromReader(IDataReader reader)
		{
			List<ContentMetaLink> contentMetaLinkList = new List<ContentMetaLink>();

			try
			{
				while (reader.Read())
				{
					ContentMetaLink contentMetaLink = new ContentMetaLink();

					contentMetaLink.Guid = new Guid(reader["Guid"].ToString());
					contentMetaLink.SiteGuid = new Guid(reader["SiteGuid"].ToString());
					contentMetaLink.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
					contentMetaLink.ContentGuid = new Guid(reader["ContentGuid"].ToString());
					contentMetaLink.Rel = reader["Rel"].ToString();
					contentMetaLink.Href = reader["Href"].ToString();
					contentMetaLink.HrefLang = reader["HrefLang"].ToString();
					contentMetaLink.Rev = reader["Rev"].ToString();
					contentMetaLink.Type = reader["Type"].ToString();
					contentMetaLink.Media = reader["Media"].ToString();
					contentMetaLink.SortRank = Convert.ToInt32(reader["SortRank"]);
					contentMetaLink.CreatedUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					contentMetaLink.CreatedBy = new Guid(reader["CreatedBy"].ToString());
					contentMetaLink.LastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
					contentMetaLink.LastModBy = new Guid(reader["LastModBy"].ToString());

					contentMetaLinkList.Add(contentMetaLink);
				}
			}
			finally
			{
				reader.Close();
			}

			return contentMetaLinkList;
		}
	}
}