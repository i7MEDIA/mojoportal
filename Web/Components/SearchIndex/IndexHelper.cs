using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Util;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using LuceneStore = Lucene.Net.Store;

namespace mojoPortal.SearchIndex;

public static class IndexHelper
{

	private static readonly ILog log = LogManager.GetLogger(typeof(IndexHelper));
	private static bool debugLog = log.IsDebugEnabled;

	public static string GetSiteProviderName(int siteId)
	{
		string siteKey = Invariant($"Site{siteId}-LuceneSettingsProvider");

		if (ConfigurationManager.AppSettings[siteKey] != null)
		{
			return ConfigurationManager.AppSettings[siteKey];
		}

		return "StandardAnalysisProvider";
	}


	public static string GetDataFolder(int siteId)
	{
		return Invariant($"~/Data/Sites/{siteId}/");
	}

	public static string GetSearchIndexPath(int siteId)
	{
		return System.Web.Hosting.HostingEnvironment.MapPath(GetDataFolder(siteId) + "index/");
	}

	public static LuceneStore.Directory GetDirectory(int siteId)
	{
		// here we could implement a provider model to plugin different Directories
		// ie https://github.com/richorama/AzureDirectory
		// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11688~1#post48672

		return new LuceneStore.SimpleFSDirectory(new DirectoryInfo(GetSearchIndexPath(siteId)));
	}

	public static IndexWriter GetIndexWriter(int siteId, LuceneStore.Directory luceneDirectory)
	{
		IndexWriter indexWriter = null;
		LuceneSettingsProvider provider = LuceneSettingsManager.Providers[IndexHelper.GetSiteProviderName(siteId)];
		if (provider == null)
		{
			log.Error("LuceneSettingsProvider could not be obtained");
			return null;
		}

		Analyzer analyzer = provider.GetAnalyzer();

		//if (!DirectoryReader.IndexExists(indexDirectory))
		//{
		try
		{
			indexWriter = new IndexWriter(luceneDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer));

		}
		catch (IOException ex)
		{
			log.Error(ex);
			//var fs = FileSystemHelper.LoadFileSystem();
			//fs.DeleteFile(indexDirectory.)
			//try
			//{
			//	indexWriter = new IndexWriter(indexDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
			//}
			//catch (System.IO.FileNotFoundException ex2)
			//{
			//	log.Error(ex2);
			//}
		}
		//}
		//else
		//{
		//	try
		//	{
		//		indexWriter = new IndexWriter(indexDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
		//	}
		//	catch (System.IO.FileNotFoundException ex2)
		//	{
		//		log.Error(ex2);
		//	}
		//}

		return indexWriter;
	}

	public static IndexItemCollection Browse(
		int siteId,
		Guid featureGuid,
		DateTime modifiedBeginDate,
		DateTime modifiedEndDate,
		int pageNumber,
		int pageSize,
		out int totalHits)
	{
		totalHits = 0;

		var results = new IndexItemCollection();

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			Filter filter = null;
			BooleanQuery filterQuery = null;

			if ((modifiedBeginDate.Date > DateTime.MinValue.Date) || (modifiedEndDate.Date < DateTime.MaxValue.Date))
			{
				filterQuery = new BooleanQuery
				{
					{ dateRangeQuery(nameof(IndexItem.LastModUtc), modifiedBeginDate.Date, modifiedEndDate.Date), Occur.MUST }
				}; // won't be used to score the results
			}

			if (featureGuid != Guid.Empty)
			{
				filterQuery ??= [];

				var featureFilter = new BooleanQuery
				{
					{ new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST }
				};

				filterQuery.Add(featureFilter, Occur.MUST);
			}

			if (filterQuery != null)
			{
				filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores
			}

			var matchAllQuery = new MatchAllDocsQuery();
			IndexReader reader = DirectoryReader.Open(searchDirectory);
			var searcher = new IndexSearcher(reader);

			int maxResults = int.MaxValue;

			var hits = searcher.Search(matchAllQuery, filter, maxResults);

			int startHit = 0;
			if (pageNumber > 1)
			{
				startHit = ((pageNumber - 1) * pageSize);
			}

			totalHits = hits.TotalHits;

			if (startHit > totalHits)
			{
				startHit = totalHits;
			}

			int end = startHit + pageSize;
			if (totalHits <= end)
			{
				end = totalHits;
			}

			int itemsAdded = 0;
			int itemsToAdd = end;

			for (int i = startHit; i < itemsToAdd; i++)
			{
				var doc = searcher.Doc(hits.ScoreDocs[i].Doc);
				var indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

				results.Add(indexItem);
				itemsAdded += 1;

			}

			results.ItemCount = itemsAdded;
			results.PageIndex = pageNumber;

			results.ExecutionTime = DateTime.Now.Ticks; // -0;
		}

		return results;
	}

	public static List<IndexItem> GetRecentCreatedContent(
		int siteId,
		Guid featureGuid,
		DateTime createdSinceDate,
		int maxItems)
	{
		int totalHits = 0;

		var results = new List<IndexItem>();

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			Filter filter = null;
			BooleanQuery filterQuery = null;

			filterQuery = new BooleanQuery(); // won't be used to score the results

			var excludeFilter = new BooleanQuery
			{
				{ new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST }
			};

			filterQuery.Add(excludeFilter, Occur.MUST);

			filterQuery.Add(dateRangeQuery("CreatedUtc", createdSinceDate.Date, DateTime.MaxValue), Occur.MUST);

			// we only want public content, that is both page and module roles must have "All Users"
			// which means even unauthenticated users
			var pageRole = new Term("Role", "All Users");
			var pageRoleFilter = new TermQuery(pageRole);
			filterQuery.Add(pageRoleFilter, Occur.MUST);

			var moduleRole = new Term("ModuleRole", "All Users");
			var moduleRoleFilter = new TermQuery(moduleRole);
			filterQuery.Add(moduleRoleFilter, Occur.MUST);

			if (featureGuid != Guid.Empty)
			{
				var featureFilter = new BooleanQuery
				{
					{ new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST }
				};

				filterQuery.Add(featureFilter, Occur.MUST);
			}

			filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

			var matchAllQuery = new MatchAllDocsQuery();
			IndexReader reader = DirectoryReader.Open(searchDirectory);
			var searcher = new IndexSearcher(reader);

			var maxResults = int.MaxValue;
			var hits = searcher.Search(matchAllQuery, filter, maxResults);
			totalHits = hits.TotalHits;

			for (int i = 0; i < totalHits; i++)
			{
				var doc = searcher.Doc(hits.ScoreDocs[i].Doc);
				var indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);
				results.Add(indexItem);
			}
		}

		// sort all descending on lastmodutc
		results.Sort();

		if (results.Count <= maxItems)
		{
			return results;
		}
		else
		{
			var finalResults = new List<IndexItem>();
			for (int i = 0; i < maxItems; i++)
			{
				finalResults.Add(results[i]);
			}

			return finalResults;
		}
	}


	public static List<IndexItem> GetRecentCreatedContent(
		int siteId,
		Guid[] featureGuids,
		DateTime createdSinceDate,
		int maxItems)
	{
		int totalHits = 0;

		var results = new List<IndexItem>();

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			Filter filter = null;
			BooleanQuery filterQuery = null;

			filterQuery = new BooleanQuery(); // won't be used to score the results

			var excludeFilter = new BooleanQuery
			{
				{ new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST }
			};

			filterQuery.Add(excludeFilter, Occur.MUST);
			filterQuery.Add(dateRangeQuery("CreatedUtc", createdSinceDate.Date, DateTime.MaxValue), Occur.MUST);

			// we only want public content, that is both page and module roles must have "All Users"
			// which means even unauthenticated users
			var pageRole = new Term("Role", "All Users");
			var pageRoleFilter = new TermQuery(pageRole);
			filterQuery.Add(pageRoleFilter, Occur.MUST);

			var moduleRole = new Term("ModuleRole", "All Users");
			var moduleRoleFilter = new TermQuery(moduleRole);

			filterQuery.Add(moduleRoleFilter, Occur.MUST);


			if ((featureGuids != null) && (featureGuids.Length > 0))
			{
				var featureFilter = new BooleanQuery();

				foreach (Guid featureGuid in featureGuids)
				{
					featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.SHOULD);
				}

				filterQuery.Add(featureFilter, Occur.MUST); // at least 1 of the "should"s must match
			}


			filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

			var matchAllQuery = new MatchAllDocsQuery();

			IndexReader reader = DirectoryReader.Open(searchDirectory);
			var searcher = new IndexSearcher(reader);
			var maxResults = int.MaxValue;
			var hits = searcher.Search(matchAllQuery, filter, maxResults);
			totalHits = hits.TotalHits;

			for (int i = 0; i < totalHits; i++)
			{
				var doc = searcher.Doc(hits.ScoreDocs[i].Doc);
				var indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);
				results.Add(indexItem);
			}
		}


		// sort all descending on lastmodutc
		results.Sort();

		if (results.Count <= maxItems)
		{
			return results;
		}
		else
		{
			var finalResults = new List<IndexItem>();

			for (int i = 0; i < maxItems; i++)
			{
				finalResults.Add(results[i]);
			}

			return finalResults;
		}
	}


	public static List<IndexItem> GetRecentModifiedContent(
		int siteId,
		Guid featureGuid,
		DateTime modifiedSinceDate,
		int maxItems)
	{
		int totalHits = 0;

		var results = new List<IndexItem>();

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			Filter filter = null;
			BooleanQuery filterQuery = new BooleanQuery(); // won't be used to score the results

			var excludeFilter = new BooleanQuery();
			excludeFilter.Add(new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST);
			filterQuery.Add(excludeFilter, Occur.MUST);

			filterQuery.Add(dateRangeQuery("LastModUtc", modifiedSinceDate.Date, DateTime.MaxValue), Occur.MUST);

			// we only want public content, that is both page and module roles must have "All Users"
			// which means even unauthenticated users
			var pageRole = new Term("Role", "All Users");
			TermQuery pageRoleFilter = new TermQuery(pageRole);
			filterQuery.Add(pageRoleFilter, Occur.MUST);


			var moduleRole = new Term("ModuleRole", "All Users");
			TermQuery moduleRoleFilter = new TermQuery(moduleRole);
			filterQuery.Add(moduleRoleFilter, Occur.MUST);


			if (featureGuid != Guid.Empty)
			{
				BooleanQuery featureFilter = new BooleanQuery();

				featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST);
				filterQuery.Add(featureFilter, Occur.MUST);

			}

			filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

			MatchAllDocsQuery matchAllQuery = new MatchAllDocsQuery();

			IndexReader reader = DirectoryReader.Open(searchDirectory);
			IndexSearcher searcher = new IndexSearcher(reader);
			int maxResults = int.MaxValue;
			TopDocs hits = searcher.Search(matchAllQuery, filter, maxResults);
			totalHits = hits.TotalHits;

			for (int i = 0; i < totalHits; i++)
			{
				Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
				IndexItem indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

				results.Add(indexItem);

			}
		}

		// sort all descending on lastmodutc
		results.Sort();

		if (results.Count <= maxItems)
		{
			return results;
		}
		else
		{
			var finalResults = new List<IndexItem>();
			for (int i = 0; i < maxItems; i++)
			{
				finalResults.Add(results[i]);
			}

			return finalResults;
		}
	}

	private static TermRangeQuery dateRangeQuery(string field, DateTime lowerTerm, DateTime upperTerm, bool includeLower = true, bool includeUpper = true)
	{
		return new TermRangeQuery(
				field,
				new BytesRef(lowerTerm.ToString("s")),
				new BytesRef(upperTerm.ToString("s")),
				includeLower,
				includeUpper);
	}

	public static List<IndexItem> GetRecentModifiedContent(
		int siteId,
		Guid[] featureGuids,
		DateTime modifiedSinceDate,
		int maxItems)
	{
		int totalHits = 0;

		var results = new List<IndexItem>();

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			Filter filter = null;
			BooleanQuery filterQuery = new BooleanQuery(); // won't be used to score the results

			BooleanQuery excludeFilter = new BooleanQuery();
			excludeFilter.Add(new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST);
			filterQuery.Add(excludeFilter, Occur.MUST);

			filterQuery.Add(dateRangeQuery("LastModUtc", modifiedSinceDate.Date, DateTime.MaxValue), Occur.MUST);

			// we only want public content, that is both page and module roles must have "All Users"
			// which means even unauthenticated users
			Term pageRole = new Term("Role", "All Users");
			TermQuery pageRoleFilter = new TermQuery(pageRole);
			filterQuery.Add(pageRoleFilter, Occur.MUST);


			Term moduleRole = new Term("ModuleRole", "All Users");
			TermQuery moduleRoleFilter = new TermQuery(moduleRole);

			filterQuery.Add(moduleRoleFilter, Occur.MUST);

			if ((featureGuids != null) && (featureGuids.Length > 0))
			{
				BooleanQuery featureFilter = new BooleanQuery();

				foreach (Guid featureGuid in featureGuids)
				{
					featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.SHOULD);

				}

				filterQuery.Add(featureFilter, Occur.MUST);

			}


			filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

			MatchAllDocsQuery matchAllQuery = new MatchAllDocsQuery();

			IndexReader reader = DirectoryReader.Open(searchDirectory);
			IndexSearcher searcher = new IndexSearcher(reader);
			int maxResults = int.MaxValue;
			TopDocs hits = searcher.Search(matchAllQuery, filter, maxResults);
			totalHits = hits.TotalHits;

			for (int i = 0; i < totalHits; i++)
			{
				Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
				IndexItem indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

				results.Add(indexItem);

			}
		}


		// sort all descending on lastmodutc
		results.Sort();

		if (results.Count <= maxItems)
		{
			return results;
		}
		else
		{
			var finalResults = new List<IndexItem>();
			for (int i = 0; i < maxItems; i++)
			{
				finalResults.Add(results[i]);
			}

			return finalResults;

		}
	}


	public static IndexItemCollection Search(
		int siteId,
		bool isAdminContentAdminOrSiteEditor,
		List<string> userRoles,
		Guid[] featureGuids,
		DateTime modifiedBeginDate,
		DateTime modifiedEndDate,
		string queryText,
		bool highlightResults,
		int highlightedFragmentSize,
		int pageNumber,
		int pageSize,
		int maxClauseCount,
		out int totalHits,
		out bool invalidQuery)
	{
		invalidQuery = false;
		totalHits = 0;

		IndexItemCollection results = new IndexItemCollection();

		if (string.IsNullOrEmpty(queryText))
		{
			return results;
		}

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			if (!DirectoryReader.IndexExists(searchDirectory))
			{
				return results;
			}

			long startTicks = DateTime.Now.Ticks;

			try
			{
				if (maxClauseCount != 1024)
				{
					BooleanQuery.MaxClauseCount = maxClauseCount;
				}

				// there are different analyzers for different languages
				// see LuceneSettings.config in the root of the web
				LuceneSettingsProvider provider = LuceneSettingsManager.Providers[GetSiteProviderName(siteId)];
				Analyzer analyzer = provider.GetAnalyzer();

				Query searchQuery = MultiFieldQueryParser.Parse(
					Lucene.Net.Util.LuceneVersion.LUCENE_48,
					new string[] { queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty) },
					new string[] { "Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword" },
					analyzer);

				BooleanQuery filterQuery = new BooleanQuery(); // won't be used to score the results

				if (!isAdminContentAdminOrSiteEditor) // skip role filters for these users
				{
					AddRoleFilters(userRoles, filterQuery);
					AddModuleRoleFilters(userRoles, filterQuery);
				}

				filterQuery.Add(dateRangeQuery("PublishBeginDate", DateTime.MinValue, DateTime.MaxValue), Occur.MUST);
				filterQuery.Add(dateRangeQuery("PublishEndDate", DateTime.UtcNow, DateTime.MaxValue), Occur.MUST);

				if ((modifiedBeginDate.Date > DateTime.MinValue.Date) || (modifiedEndDate.Date < DateTime.MaxValue.Date))
				{
					filterQuery.Add(dateRangeQuery("LastModUtc", modifiedBeginDate.Date, modifiedEndDate.Date), Occur.MUST);
				}

				//if ((!DisableSearchFeatureFilters) && (featureGuid != Guid.Empty))
				//{
				//    BooleanQuery featureFilter = new BooleanQuery();

				//    featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST);

				//    filterQuery.Add(featureFilter, Occur.MUST);
				//}

				if ((featureGuids != null) && (featureGuids.Length > 0))
				{
					BooleanQuery featureFilter = new BooleanQuery();

					foreach (Guid featureGuid in featureGuids)
					{
						featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.SHOULD);
					}

					filterQuery.Add(featureFilter, Occur.MUST);
				}


				Filter filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

				IndexReader reader = DirectoryReader.Open(searchDirectory);
				IndexSearcher searcher = new IndexSearcher(reader);

				//http://stackoverflow.com/questions/9872933/migrating-lucene-hitcollector-2-x-to-collector-3-x
				//TopScoreDocCollector collector = TopScoreDocCollector.Create(maxResults, true);

				int maxResults = int.MaxValue;
				TopDocs hits = searcher.Search(searchQuery, filter, maxResults);

				int startHit = 0;
				if (pageNumber > 1)
				{
					startHit = ((pageNumber - 1) * pageSize);
				}

				totalHits = hits.TotalHits;

				int end = startHit + pageSize;
				if (totalHits <= end)
				{
					end = totalHits;
				}

				int itemsAdded = 0;
				int itemsToAdd = end;

				QueryScorer scorer = new QueryScorer(searchQuery);
				SimpleHTMLFormatter formatter = new SimpleHTMLFormatter("<span class='searchterm'>", "</span>");
				Highlighter highlighter = new Highlighter(formatter, scorer);

				highlighter.TextFragmenter = new SimpleFragmenter(highlightedFragmentSize);

				for (int i = startHit; i < itemsToAdd; i++)
				{
					Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
					IndexItem indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

					if (highlightResults)
					{
						try
						{
							TokenStream stream = analyzer.GetTokenStream("contents", new StringReader(doc.Get("contents")));
							string highlightedResult = highlighter.GetBestFragment(stream, doc.Get("contents"));

							if (highlightedResult != null) { indexItem.Intro = highlightedResult; }
						}
						catch (NullReferenceException) { }
					}

					results.Add(indexItem);
					itemsAdded += 1;

				}

				results.ItemCount = itemsAdded;
				results.PageIndex = pageNumber;

				results.ExecutionTime = DateTime.Now.Ticks - startTicks;



			}
			catch (ParseException ex)
			{
				invalidQuery = true;
				log.Error("handled error for search terms " + queryText, ex);
				// these parser exceptions are generally caused by
				// spambots posting too much junk into the search form
				// heres an option to automatically ban the ip address
				HandleSpam(queryText, ex);


				return results;
			}
			catch (BooleanQuery.TooManyClausesException ex)
			{
				invalidQuery = true;
				log.Error("handled error for search terms " + queryText, ex);
				return results;

			}
			catch (IOException ex)
			{
				invalidQuery = true;
				log.Error("handled error for search terms " + queryText, ex);
				return results;
			}

			return results;
		}
	}


	public static IndexItemCollection Search(
		int siteId,
		bool isAdminContentAdminOrSiteEditor,
		List<string> userRoles,
		Guid featureGuid,
		DateTime modifiedBeginDate,
		DateTime modifiedEndDate,
		string queryText,
		bool highlightResults,
		int highlightedFragmentSize,
		int pageNumber,
		int pageSize,
		int maxClauseCount,
		out int totalHits,
		out bool invalidQuery)
	{
		invalidQuery = false;
		totalHits = 0;

		var results = new IndexItemCollection();

		if (string.IsNullOrEmpty(queryText))
		{
			return results;
		}

		using (LuceneStore.Directory searchDirectory = GetDirectory(siteId))
		{
			if (!DirectoryReader.IndexExists(searchDirectory))
			{
				return results;
			}

			long startTicks = DateTime.Now.Ticks;

			try
			{
				if (maxClauseCount != 1024)
				{
					BooleanQuery.MaxClauseCount = maxClauseCount;
				}

				// there are different analyzers for different languages
				// see LuceneSettings.config in the root of the web
				var provider = LuceneSettingsManager.Providers[GetSiteProviderName(siteId)];
				var analyzer = provider.GetAnalyzer();

				var searchQuery = MultiFieldQueryParser.Parse(
					LuceneVersion.LUCENE_48,
					[queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty)],
					["Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword"],
					analyzer);

				var filterQuery = new BooleanQuery(); // won't be used to score the results

				if (!isAdminContentAdminOrSiteEditor) // skip role filters for these users
				{
					AddRoleFilters(userRoles, filterQuery);
					AddModuleRoleFilters(userRoles, filterQuery);
				}

				filterQuery.Add(dateRangeQuery("PublishBeginDate", DateTime.MinValue, DateTime.UtcNow), Occur.MUST);

				filterQuery.Add(dateRangeQuery("PublishEndDate", DateTime.UtcNow, DateTime.MaxValue), Occur.MUST);


				if ((modifiedBeginDate.Date > DateTime.MinValue.Date) || (modifiedEndDate.Date < DateTime.MaxValue.Date))
				{
					filterQuery.Add(dateRangeQuery("LastModUtc", modifiedBeginDate.Date, modifiedEndDate.Date), Occur.MUST);
				}

				if ((!WebConfigSettings.DisableSearchFeatureFilters) && (featureGuid != Guid.Empty))
				{
					var featureFilter = new BooleanQuery
					{
						{ new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST }
					};
					filterQuery.Add(featureFilter, Occur.MUST);
				}

				Filter filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

				IndexReader reader = DirectoryReader.Open(searchDirectory);
				var searcher = new IndexSearcher(reader);

				int maxResults = int.MaxValue;
				var hits = searcher.Search(searchQuery, filter, maxResults);

				int startHit = 0;
				if (pageNumber > 1)
				{
					startHit = ((pageNumber - 1) * pageSize);
				}

				totalHits = hits.TotalHits;
				int end = startHit + pageSize;
				if (totalHits <= end)
				{
					end = totalHits;
				}

				int itemsAdded = 0;
				int itemsToAdd = end;

				var scorer = new QueryScorer(searchQuery);
				var formatter = new SimpleHTMLFormatter("<span class='searchterm'>", "</span>");
				var highlighter = new Highlighter(formatter, scorer)
				{
					TextFragmenter = new SimpleFragmenter(highlightedFragmentSize)
				};

				for (int i = startHit; i < itemsToAdd; i++)
				{
					var doc = searcher.Doc(hits.ScoreDocs[i].Doc);
					var indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

					if (highlightResults)
					{
						try
						{
							TokenStream stream = analyzer.GetTokenStream("contents", new StringReader(doc.Get("contents")));
							string highlightedResult = highlighter.GetBestFragment(stream, doc.Get("contents"));

							if (highlightedResult != null)
							{
								indexItem.Intro = highlightedResult;
							}
						}
						catch (NullReferenceException) { }
					}

					results.Add(indexItem);
					itemsAdded += 1;
				}

				results.ItemCount = itemsAdded;
				results.PageIndex = pageNumber;
				results.ExecutionTime = DateTime.Now.Ticks - startTicks;

			}
			catch (ParseException ex)
			{
				invalidQuery = true;
				log.Error("handled error for search terms " + queryText, ex);
				// these parser exceptions are generally caused by
				// spambots posting too much junk into the search form
				// heres an option to automatically ban the ip address
				HandleSpam(queryText, ex);
				return results;
			}
			catch (BooleanQuery.TooManyClausesException ex)
			{
				invalidQuery = true;
				log.Error("handled error for search terms " + queryText, ex);
				return results;
			}
			catch (IOException ex)
			{
				invalidQuery = true;
				log.Error("handled error for search terms " + queryText, ex);
				return results;
			}
			return results;
		}
	}


	/// <summary>
	/// avoid calling this method especially in sequence
	/// it was only implemented to support deletion from the IndexBrowser.aspx utility page
	/// it is not meant for general consumption from custom code
	/// </summary>
	/// <param name="siteId"></param>
	/// <param name="key"></param>
	public static void DeleteIndexDoc(int siteId, string key)
	{
		using var indexWriter = GetIndexWriter(siteId, GetDirectory(siteId));
		indexWriter.DeleteDocuments(new Term("Key", key));
	}

	private static void HandleSpam(string queryText, Exception ex)
	{
		bool autoBanSpamBots = ConfigHelper.GetBoolProperty("AutoBanSpambotsOnSearchErrors", false);

		if (autoBanSpamBots && IsSpam(queryText))
		{
			if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
			{
				var b = new BannedIPAddress
				{
					BannedIP = HttpContext.Current.Request.UserHostAddress,
					BannedReason = "spambot autodetected",
					BannedUtc = DateTime.UtcNow
				};
				b.Save();

				log.Info("spambot detected, ip address has been banned: " + HttpContext.Current.Request.UserHostAddress);
			}
		}
		else
		{
			log.Info("spambot possibly detected, ip address was: " + HttpContext.Current.Request.UserHostAddress);
		}
	}

	private static bool IsSpam(string queryText)
	{
		return queryText.ContainsBadWords();
	}

	public static Regex MarkupRegex = new("<[/a-zA-Z]+[^>]*>|<!--(?!-->)*-->");

	public static string ConvertToText(string markup)
	{
		return MarkupRegex.Replace(markup, " ");
	}

	private static void AddRoleFilters(List<string> userRoles, BooleanQuery mainQuery)
	{
		var rolesQuery = new BooleanQuery();
		foreach (var role in userRoles)
		{
			var term = new Term("Role", role);
			var termQuery = new TermQuery(term);
			rolesQuery.Add(termQuery, Occur.SHOULD);
		}
		// in a boolean query with multiple should occur items, at least one must occur
		mainQuery.Add(rolesQuery, Occur.MUST);
	}

	private static void AddModuleRoleFilters(List<string> userRoles, BooleanQuery mainQuery)
	{
		var rolesQuery = new BooleanQuery();
		foreach (var role in userRoles)
		{
			var term = new Term("ModuleRole", role);

			var termQuery = new TermQuery(term);
			rolesQuery.Add(termQuery, Occur.SHOULD);
		}
		// in a boolean query with multiple should occur items, at least one must occur

		mainQuery.Add(rolesQuery, Occur.MUST);
	}

	//private static BooleanQuery BuildQueryFromKeywords(Hashtable keyWords)
	//{
	//	BooleanQuery bQuery = new BooleanQuery();
	//	foreach (DictionaryEntry keywordFilterTerm in keyWords)
	//	{
	//		string field = keywordFilterTerm.Key.ToString();
	//		string keyword = keywordFilterTerm.Value.ToString();
	//		bQuery.Add(new TermQuery(new Term(field, keyword)), Occur.SHOULD);

	//	}

	//	return bQuery;
	//}


	public static void RebuildIndex(IndexItem indexItem)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		if (indexItem == null)
		{
			return;
		}

		if (indexItem.IndexPath.Length > 0)
		{
			RebuildIndex(indexItem, indexItem.IndexPath);
			return;
		}
		else if (indexItem.SiteId > -1)
		{
			RebuildIndex(indexItem, GetSearchIndexPath(indexItem.SiteId));
			return;

		}

		string indexPath = GetSearchIndexPath(indexItem.SiteId);
		RebuildIndex(indexItem, indexPath);
	}


	public static void RebuildIndex(IndexItem indexItem, string indexPath)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		if (indexItem == null)
		{
			log.Info("IndexItem was NULL");
			return;
		}

		if (indexPath == null)
		{
			log.Info("IndexPath was NULL");
			return;
		}

		if (indexPath.Length == 0)
		{
			log.Info("IndexItem was empty");
			return;
		}

		IndexingQueue queueItem = new IndexingQueue();
		queueItem.SiteId = indexItem.SiteId;
		queueItem.IndexPath = indexPath;
		queueItem.ItemKey = indexItem.Key;
		queueItem.RemoveOnly = indexItem.RemoveOnly;
		queueItem.SerializedItem = SerializationHelper.SerializeToString(indexItem);
		queueItem.Save();

		// the above queues the items to be indexed. Edit page must also call SiteUtils.QueueIndexing(); after the content is saved.
	}


	public static void RemoveIndex(IndexItem indexItem)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		if (indexItem == null)
		{
			return;
		}

		if (indexItem.IndexPath.Length > 0)
		{
			RemoveIndex(indexItem, indexItem.IndexPath);
			return;

		}

		indexItem.IndexPath = GetSearchIndexPath(indexItem.SiteId);

		RemoveIndex(indexItem, indexItem.IndexPath);
		return;

	}

	public static void RemoveIndex(IndexItem indexItem, string indexPath)
	{
		if (WebConfigSettings.DisableSearchIndex) { return; }

		if (indexItem == null)
		{
			return;
		}

		if (indexPath == null)
		{
			return;
		}

		if (indexPath.Length == 0)
		{
			return;
		}

		IndexingQueue queueItem = new IndexingQueue();
		queueItem.SiteId = indexItem.SiteId;
		queueItem.IndexPath = indexPath;
		queueItem.ItemKey = indexItem.Key;
		queueItem.RemoveOnly = true;
		queueItem.SerializedItem = SerializationHelper.SerializeToString(indexItem);
		queueItem.Save();

		// the above queues the items to be indexed. Edit page must also call SiteUtils.QueueIndexing(); after the content is deleted.
	}

	public static void RemoveIndexItem(int pageId, int moduleId, int itemId)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		if (CacheHelper.GetCurrentSiteSettings() is not SiteSettings siteSettings)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("IndexHelper.RemoveIndexItem tried to obtain a SiteSettings object but it came back null");
			}

			return;
		}

		var indexItem = new IndexItem
		{
			SiteId = siteSettings.SiteId,
			PageId = pageId,
			ModuleId = moduleId,
			ItemId = itemId,
			IndexPath = GetSearchIndexPath(siteSettings.SiteId)
		};

		RemoveIndex(indexItem);

		if (debugLog)
		{
			log.Debug("Removed Index ");
		}
	}

	//public static void RemoveIndexItem(int pageId, int moduleId, string itemKey)
	//{
	//	if (WebConfigSettings.DisableSearchIndex) { 
	//		return; }


	//	if (CacheHelper.GetCurrentSiteSettings() is not SiteSettings siteSettings)
	//	{
	//		log.Error("IndexHelper.RemoveIndexItem tried to obtain a SiteSettings object but it came back null");
	//		return;
	//	}

	//	var indexItem = new IndexItem
	//	{
	//		SiteId = siteSettings.SiteId,
	//		PageId = pageId,
	//		ModuleId = moduleId,
	//		ItemKey = itemKey,
	//		IndexPath = GetSearchIndexPath(siteSettings.SiteId)
	//	};

	//	RemoveIndex(indexItem);

	//	if (debugLog)
	//	{
	//		log.Debug("Removed Index ");
	//	}
	//}

	//public static void RemoveIndexItem(int siteId, int pageId, int moduleId, int itemId, string indexPath)
	//{
	//	if (WebConfigSettings.DisableSearchIndex) { 
	//		return; }

	//	var indexItem = new IndexItem
	//	{
	//		SiteId = siteId,
	//		PageId = pageId,
	//		ModuleId = moduleId,
	//		ItemId = itemId,
	//		IndexPath = indexPath
	//	};

	//	RemoveIndex(indexItem);

	//	if (debugLog)
	//	{
	//		log.Debug("Removed Index ");
	//	}
	//}

	public static void DeleteSearchIndex(int siteId)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		using (LuceneStore.Directory d = GetDirectory(siteId))
		{
			if (DirectoryReader.IndexExists(d))
			{
				using (var indexWriter = GetIndexWriter(siteId, d))
				{
					indexWriter.DeleteAll();
					indexWriter.Commit();
				}
			}
		}

		//if (!Directory.Exists(indexPath))
		//{
		//	return;
		//}

		//try
		//{
		//	var dir = new DirectoryInfo(indexPath);
		//	var files = dir.GetFiles();
		//	foreach (FileInfo f in files)
		//	{
		//		File.Delete(f.FullName);
		//	}

		//}
		//catch (Exception ex)
		//{
		//	log.Error(ex);
		//}
	}

	public static void RebuildSearchIndex(int siteId)
	{
		IndexingQueue.DeleteAll();
		using var searchDirectory = GetDirectory(siteId);
		searchDirectory.ClearLock(searchDirectory.GetLockID());
		DeleteSearchIndex(siteId);
		VerifySearchIndex(siteId);

		Thread.Sleep(5000); //wait 5 seconds
		SiteUtils.QueueIndexing();
	}

	#region ClearPageIndex


	public static void ClearPageIndexAsync(PageSettings pageSettings)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		pageSettings.IndexPath = GetSearchIndexPath(pageSettings.SiteId);

		if (ThreadPool.QueueUserWorkItem(new WaitCallback(ClearPageIndexAsyncCallback), pageSettings))
		{
			if (debugLog)
			{
				log.Debug("IndexHelper.ClearPageIndexAsyncCallback queued");
			}
		}
		else
		{
			if (debugLog)
			{
				log.Debug("Failed to queue a thread for IndexHelper.ClearPageIndexAsync");
			}
		}
	}

	private static void ClearPageIndexAsyncCallback(object o)
	{
		if (o is not PageSettings)
		{
			return;
		}

		var pageSettings = (PageSettings)o;
		ClearPageIndex(pageSettings);
	}

	private static bool ClearPageIndex(PageSettings pageSettings)
	{

		if (pageSettings == null) { return false; }
		bool result = false;

		try
		{

			using LuceneStore.Directory searchDirectory = GetDirectory(pageSettings.SiteId);

			if (DirectoryReader.IndexExists(searchDirectory))
			{
				using var indexWriter = GetIndexWriter(pageSettings.SiteId, searchDirectory);
				using IndexReader indexReader = DirectoryReader.Open(searchDirectory);
				try
				{
					int tot = indexWriter.NumDocs;

					for (int i = 0; i < tot; i++)
					{
						Document doc = indexReader.Document(i);

						if (doc.GetField("PageID").GetStringValue() ==
							pageSettings.PageId.ToString(CultureInfo.InvariantCulture))

						{
							if (debugLog)
							{
								log.Debug("ClearPageIndex about to delete doc ");
							}

							try
							{
								indexWriter.TryDeleteDocument(indexReader, i);
								result = true;
							}
							catch (IOException ex)
							{
								log.Info("handled error:", ex);
							}
						}
					}
				}
				catch (Exception ex)
				{
					log.Info("handled error:", ex);
				}
			}
		}
		catch (ArgumentException ex)
		{
			log.Info("handled error:", ex);
		}

		return result;
	}

	#endregion


	#region RebuildPageIndex

	public static void RebuildPageIndexAsync(PageSettings pageSettings)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		pageSettings.IndexPath = GetSearchIndexPath(pageSettings.SiteId);

		if (ThreadPool.QueueUserWorkItem(new WaitCallback(RebuildPageIndexAsyncCallback), pageSettings))
		{
			if (debugLog)
			{
				log.Debug("IndexHelper.RebuildPageIndexCallback queued");
			}
		}
		else
		{
			if (debugLog)
			{
				log.Debug("Failed to queue a thread for IndexHelper.RebuildPageIndexAsync");
			}
		}
	}

	private static void RebuildPageIndexAsyncCallback(object o)
	{
		if (o == null)
		{
			return;
		}

		if (o is not PageSettings)
		{
			return;
		}

		try
		{
			var pageSettings = (PageSettings)o;
			RebuildPageIndex(pageSettings);

			// TODO: could add some form of notification to let the admin know if it was able to index all the content
		}
		catch (TypeInitializationException ex)
		{
			if (log.IsErrorEnabled)
			{
				log.Error("IndexHelper.RebuildPageIndexAsyncCallback", ex);
			}
		}
	}

	private static bool RebuildPageIndex(PageSettings pageSettings)
	{
		if (pageSettings == null)
		{
			return false;
		}

		log.Info("IndexHelper.RebuildPageIndex - " + pageSettings.PageName);

		if (IndexBuilderManager.Providers == null)
		{
			log.Info("No IndexBuilderProviders found");
			return false;
		}

		var indexPath = GetSearchIndexPath(pageSettings.SiteId);

		ClearPageIndex(pageSettings);

		foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
		{
			indexBuilder.RebuildIndex(pageSettings, indexPath);
		}

		return true;
	}

	#endregion


	#region RebuildSiteIndex

	public static bool VerifySearchIndex(int siteId)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return false;
		}

		var indexPath = GetSearchIndexPath(siteId);

		if (string.IsNullOrWhiteSpace(indexPath))
		{
			return false;
		}

		if (!Directory.Exists(indexPath))
		{
			Directory.CreateDirectory(indexPath);
		}

		if (Directory.Exists(indexPath))
		{
			var directoryInfo = new DirectoryInfo(indexPath);
			var fileCount = directoryInfo.GetFiles().Length;
			var configFileCount = directoryInfo.GetFiles(".config").Length;
			if (fileCount == 0 || (fileCount == 1 && configFileCount == 1))
			{
				int rowsToIndex = IndexingQueue.GetCount();
				if (rowsToIndex > 0)
				{
					// already started the indexing process
					return true;
				}
				// no search index exists so build it
				RebuildSiteIndexAsync(indexPath, CacheHelper.GetMenuPages());
				return false;
			}
		}

		return true;
	}


	public static void RebuildSiteIndexAsync(string indexPath, IEnumerable<PageSettings> menuPages)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		var arrayList = new ArrayList
		{
			indexPath,
			menuPages
		};

		if (ThreadPool.QueueUserWorkItem(
			new WaitCallback(RebuildSiteIndexAsyncCallback), arrayList))
		{
			if (debugLog)
			{
				log.Debug("IndexHelper.RebuildSiteIndexAsyncCallback queued");
			}
		}
		else
		{
			if (debugLog)
			{
				log.Debug("Failed to queue a thread for IndexHelper.RebuildSiteIndexAsync");
			}
		}
	}


	private static void RebuildSiteIndexAsyncCallback(object objArrayList)
	{

		var arrayList = (ArrayList)objArrayList;
		var indexPath = (string)arrayList[0];

		var menuPages = (IEnumerable<PageSettings>)arrayList[1];

		RebuildSiteIndex(indexPath, menuPages);
	}

	private static void RebuildSiteIndex(string indexPath, IEnumerable<PageSettings> menuPages)
	{
		if (WebConfigSettings.DisableSearchIndex)
		{
			return;
		}

		// clean out index entirely
		if (menuPages is not null && menuPages.Count() > 0)
		{
			DeleteSearchIndex(SiteUtils.ParseSiteIdFromPath(indexPath));
		}

		log.Info("Rebuilding Search index.");

		if (IndexBuilderManager.Providers == null)
		{
			log.Info("No IndexBuilderProviders found");
			return;
		}

		// forums can potentially take  long time to index
		// and possibly even time out so index forums after everything else
		foreach (PageSettings pageSettings in menuPages)
		{
			foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
			{
				if (indexBuilder.Name != "ForumThreadIndexBuilderProvider")
				{
					indexBuilder.RebuildIndex(pageSettings, indexPath);
				}
			}
		}

		log.Info("Finished indexing main features.");

		// now that other modules are done index forums
		bool forumsFound = false;
		foreach (PageSettings pageSettings in menuPages)
		{
			foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
			{
				if (indexBuilder.Name == "ForumThreadIndexBuilderProvider")
				{
					forumsFound = true;
					indexBuilder.RebuildIndex(pageSettings, indexPath);
				}
			}
		}

		if (forumsFound)
		{
			log.Info("Finished indexing Forums.");
		}

		return;
	}

	#endregion

	public static string GetNullSafeString(this Document doc, string fieldName)
	{
		string s = doc.Get(fieldName);
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		return s;
	}
}
