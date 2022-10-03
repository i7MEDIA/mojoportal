// Author:					    
// Created:				        2007-08-30
// Last Modified:			    2013-01-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

// links

//http://stackoverflow.com/questions/3721020/lucene-what-is-the-difference-between-query-and-filter

//http://www.codeproject.com/KB/cs/lucene_analysis.aspx
//http://www.ifdefined.com/blog/post/2009/02/Full-Text-Search-in-ASPNET-using-LuceneNET.aspx
//http://www.codeproject.com/KB/library/IntroducingLucene.aspx
//http://www.aspfree.com/c/a/BrainDump/Working-with-Lucene-dot-Net/
//http://davidpodhola.blogspot.com/2008/02/how-to-highlight-phrase-on-results-from.html
//http://linqtolucene.codeplex.com/

//example database implementation
//http://www.codeproject.com/KB/database/FulltextFirebird.aspx

//http://vijay.screamingpens.com/archive/2008/07/21/linq-amp-lambda-part-4-lucene.net.aspx
//http://lucene.apache.org/solr/

//http://sujitpal.blogspot.com/2009/02/summarization-with-lucene.html

//http://lucene.apache.org/core/3_6_1/api/all/org/apache/lucene/facet/doc-files/userguide.html

// 2013-01-04 updated to https://cwiki.apache.org/LUCENENET/lucenenet-303.html
// new api required changes in quite a few places
// moved these indexing classes out of mojoPortal.Business.WebHelpers project into
// mojoPortal.Web project but kept the namespaces as mojoPortal.Business.WebHelpers for compatibility
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using ParseException=Lucene.Net.QueryParsers.ParseException;
using FSDirectory = Lucene.Net.Store.FSDirectory;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;


namespace mojoPortal.SearchIndex
{
    public static class IndexHelper
    {
       
        private static readonly ILog log = LogManager.GetLogger(typeof(IndexHelper));
        private static bool debugLog = log.IsDebugEnabled;
        
        public static string GetSiteProviderName(int siteId)
        {
            string siteKey = "Site" + siteId.ToInvariantString() + "-LuceneSettingsProvider";

            if (ConfigurationManager.AppSettings[siteKey] != null)
            {
                return ConfigurationManager.AppSettings[siteKey];
            }

            return "StandardAnalysisProvider";

        }

        
        public static string GetDataFolder(int siteId)
        {
            return "~/Data/Sites/" + siteId.ToInvariantString() + "/";
        }

        public static string GetSearchIndexPath(int siteId)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(GetDataFolder(siteId) + "index/");
        }

        public static Lucene.Net.Store.Directory GetDirectory(int siteId)
        {
            // here we could implement a provider model to plugin different Directories
            // ie https://github.com/richorama/AzureDirectory
            // https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11688~1#post48672

            return new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(GetSearchIndexPath(siteId)));
        }


        public static List<IndexTerm> GetIndexTerms(int siteId, int minFrequency, int maxFrequency)
        {

            List<IndexTerm> indexTerms = new List<IndexTerm>();

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {

                using (IndexReader reader = IndexReader.Open(searchDirectory, false))
                {

                    Term contentsField = new Term("contents");

                    TermEnum terms = reader.Terms(contentsField);
                    while (terms.Next())
                    {
                        Term term = terms.Term;
                        int frequency = reader.DocFreq(term);
                        if (frequency >= minFrequency)
                        {
                            if ((maxFrequency == -1) ||(frequency <= maxFrequency)) // -1 is no maximum
                            {
                                IndexTerm t = new IndexTerm();
                                t.Term = term.Text;
                                t.Frequency = frequency;
                                indexTerms.Add(t);
                            }
                        }
                    }

                }



            }

            indexTerms.Sort();

            return indexTerms;

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
            
            IndexItemCollection results = new IndexItemCollection();

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                Filter filter = null;
                BooleanQuery filterQuery = null;

                if ((modifiedBeginDate.Date > DateTime.MinValue.Date) || (modifiedEndDate.Date < DateTime.MaxValue.Date))
                {
                    filterQuery = new BooleanQuery(); // won't be used to score the results

                    TermRangeQuery lastModifiedDateFilter = new TermRangeQuery(
                        "LastModUtc",
                        modifiedBeginDate.Date.ToString("s"),
                        modifiedEndDate.Date.ToString("s"),
                        true,
                        true);

                    filterQuery.Add(lastModifiedDateFilter, Occur.MUST);

                }

                if (featureGuid != Guid.Empty)
                {
                    if (filterQuery == null) { filterQuery = new BooleanQuery(); }

                    BooleanQuery featureFilter = new BooleanQuery();

                    featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST);

                    filterQuery.Add(featureFilter, Occur.MUST);
                }

                if (filterQuery != null)
                {
                    filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores
                }

                MatchAllDocsQuery matchAllQuery = new MatchAllDocsQuery();

                using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                {
                    int maxResults = int.MaxValue;

                    TopDocs hits = searcher.Search(matchAllQuery, filter, maxResults);

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
                        Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                        IndexItem indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

                        results.Add(indexItem);
                        itemsAdded += 1;

                    }

                    results.ItemCount = itemsAdded;
                    results.PageIndex = pageNumber;

                    results.ExecutionTime = DateTime.Now.Ticks; // -0;


                }

                //    using (IndexReader reader = IndexReader.Open(searchDirectory, false))
                //    {

                //        totalHits = reader.NumDocs();
                //        int startHit = 0;
                //        int itemsToAdd = pageSize;
                //        if (pageNumber > 1)
                //        {
                //            startHit = ((pageNumber - 1) * pageSize);
                //            int end = startHit + pageSize;
                //            if (totalHits <= end)
                //            {
                //                end = totalHits;
                //            }
                //            itemsToAdd = end;
                //        }

                //        for (int i = startHit; i < itemsToAdd; i++)
                //        {
                //            Document doc = reader.Document(i);
                //            IndexItem indexItem = new IndexItem(doc, 1);
                //            results.Add(indexItem);
                //        }

                //    }
                
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

            List<IndexItem> results = new List<IndexItem>();

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                Filter filter = null;
                BooleanQuery filterQuery = null;

                filterQuery = new BooleanQuery(); // won't be used to score the results

                BooleanQuery excludeFilter = new BooleanQuery();
                excludeFilter.Add(new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST);
                filterQuery.Add(excludeFilter, Occur.MUST);

                TermRangeQuery createdDateFilter = new TermRangeQuery(
                    "CreatedUtc",
                    createdSinceDate.Date.ToString("s"),
                    DateTime.MaxValue.ToString("s"),
                    true,
                    true);

                filterQuery.Add(createdDateFilter, Occur.MUST);

                // we only want public content, that is both page and module roles must have "All Users"
                // which means even unauthenticated users
                Term pageRole = new Term("Role", "All Users");
                TermQuery pageRoleFilter = new TermQuery(pageRole);
                filterQuery.Add(pageRoleFilter, Occur.MUST);

                Term moduleRole = new Term("ModuleRole", "All Users");
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

                using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                {
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

            }

            // sort all descending on lastmodutc
            results.Sort();

            if (results.Count <= maxItems)
            {
                return results;
            }
            else
            {
                List<IndexItem> finalResults = new List<IndexItem>();
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

            List<IndexItem> results = new List<IndexItem>();

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                Filter filter = null;
                BooleanQuery filterQuery = null;

                filterQuery = new BooleanQuery(); // won't be used to score the results

                BooleanQuery excludeFilter = new BooleanQuery();
                excludeFilter.Add(new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST);
                filterQuery.Add(excludeFilter, Occur.MUST);

                TermRangeQuery createdDateFilter = new TermRangeQuery(
                    "CreatedUtc",
                    createdSinceDate.Date.ToString("s"),
                    DateTime.MaxValue.ToString("s"),
                    true,
                    true);

                filterQuery.Add(createdDateFilter, Occur.MUST);


                // we only want public content, that is both page and module roles must have "All Users"
                // which means even unauthenticated users
                Term pageRole = new Term("Role", "All Users");
                TermQuery pageRoleFilter = new TermQuery(pageRole);
                filterQuery.Add(pageRoleFilter, Occur.MUST);

                Term moduleRole = new Term("ModuleRole", "All Users");
                TermQuery moduleRoleFilter = new TermQuery(moduleRole);

                filterQuery.Add(moduleRoleFilter, Occur.MUST);


                if ((featureGuids != null)&&(featureGuids.Length > 0))
                {
                    BooleanQuery featureFilter = new BooleanQuery();

                    foreach (Guid featureGuid in featureGuids)
                    {
                        featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.SHOULD);
                    }

                    filterQuery.Add(featureFilter, Occur.MUST); // at least 1 of the "should"s must match
                }


                filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

                MatchAllDocsQuery matchAllQuery = new MatchAllDocsQuery();


                using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                {

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

            }


            // sort all descending on lastmodutc
            results.Sort();

            if (results.Count <= maxItems)
            {
                return results;
            }
            else
            {
                List<IndexItem> finalResults = new List<IndexItem>();
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

            List<IndexItem> results = new List<IndexItem>();

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                Filter filter = null;
                BooleanQuery filterQuery = filterQuery = new BooleanQuery(); // won't be used to score the results

                BooleanQuery excludeFilter = new BooleanQuery();
                excludeFilter.Add(new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST);
                filterQuery.Add(excludeFilter, Occur.MUST);

                TermRangeQuery lastModifiedDateFilter = new TermRangeQuery(
                    "LastModUtc",
                    modifiedSinceDate.Date.ToString("s"),
                    DateTime.MaxValue.ToString("s"),
                    true,
                    true);

                filterQuery.Add(lastModifiedDateFilter, Occur.MUST);

                // we only want public content, that is both page and module roles must have "All Users"
                // which means even unauthenticated users
                Term pageRole = new Term("Role", "All Users");
                TermQuery pageRoleFilter = new TermQuery(pageRole);
                filterQuery.Add(pageRoleFilter, Occur.MUST);


                Term moduleRole = new Term("ModuleRole", "All Users");
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

                using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                {
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


            }

            // sort all descending on lastmodutc
            results.Sort();

            if (results.Count <= maxItems)
            {
                return results;
            }
            else
            {
                List<IndexItem> finalResults = new List<IndexItem>();
                for (int i = 0; i < maxItems; i++)
                {
                    finalResults.Add(results[i]);
                }

                return finalResults;

            }
        }



        public static List<IndexItem> GetRecentModifiedContent(
            int siteId,
            Guid[] featureGuids,
            DateTime modifiedSinceDate,
            int maxItems)
        {
            int totalHits = 0;

            List<IndexItem> results = new List<IndexItem>();

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                Filter filter = null;
                BooleanQuery filterQuery = new BooleanQuery(); // won't be used to score the results

                BooleanQuery excludeFilter = new BooleanQuery();
                excludeFilter.Add(new TermQuery(new Term("ExcludeFromRecentContent", "false")), Occur.MUST);
                filterQuery.Add(excludeFilter, Occur.MUST);

                TermRangeQuery lastModifiedDateFilter = new TermRangeQuery(
                    "LastModUtc",
                    modifiedSinceDate.Date.ToString("s"),
                    DateTime.MaxValue.ToString("s"),
                    true,
                    true);

                filterQuery.Add(lastModifiedDateFilter, Occur.MUST);

                // we only want public content, that is both page and module roles must have "All Users"
                // which means even unauthenticated users
                Term pageRole = new Term("Role", "All Users");
                TermQuery pageRoleFilter = new TermQuery(pageRole);
                filterQuery.Add(pageRoleFilter, Occur.MUST);


                Term moduleRole = new Term("ModuleRole", "All Users");
                TermQuery moduleRoleFilter = new TermQuery(moduleRole);

                filterQuery.Add(moduleRoleFilter, Occur.MUST);

                if ((featureGuids != null)&&(featureGuids.Length > 0))
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

                using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                {

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



            }


            // sort all descending on lastmodutc
            results.Sort();

            if (results.Count <= maxItems)
            {
                return results;
            }
            else
            {
                List<IndexItem> finalResults = new List<IndexItem>();
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
       
            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                if (!IndexReader.IndexExists(searchDirectory)) { return results; }

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
                        Lucene.Net.Util.Version.LUCENE_30,
                        new string[] { queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty) },
                        new string[] { "Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword" },
                        analyzer);

                    BooleanQuery filterQuery = new BooleanQuery(); // won't be used to score the results

                    if (!isAdminContentAdminOrSiteEditor) // skip role filters for these users
                    {
                        AddRoleFilters(userRoles, filterQuery);
                        AddModuleRoleFilters(userRoles, filterQuery);
                    }

                    TermRangeQuery beginDateFilter = new TermRangeQuery(
                        "PublishBeginDate",
                        DateTime.MinValue.ToString("s"),
                        DateTime.UtcNow.ToString("s"),
                        true,
                        true);

                    filterQuery.Add(beginDateFilter, Occur.MUST);

                    TermRangeQuery endDateFilter = new TermRangeQuery(
                        "PublishEndDate",
                        DateTime.UtcNow.ToString("s"),
                        DateTime.MaxValue.ToString("s"),
                        true,
                        true);

                    filterQuery.Add(endDateFilter, Occur.MUST);

                    if ((modifiedBeginDate.Date > DateTime.MinValue.Date) || (modifiedEndDate.Date < DateTime.MaxValue.Date))
                    {
                        TermRangeQuery lastModifiedDateFilter = new TermRangeQuery(
                            "LastModUtc",
                            modifiedBeginDate.Date.ToString("s"),
                            modifiedEndDate.Date.ToString("s"),
                            true,
                            true);

                        filterQuery.Add(lastModifiedDateFilter, Occur.MUST);
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

                    using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                    { 
                     
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
                                    TokenStream stream = analyzer.TokenStream("contents", new StringReader(doc.Get("contents")));
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
                catch (BooleanQuery.TooManyClauses ex)
                {
                    invalidQuery = true;
                    log.Error("handled error for search terms " + queryText, ex);
                    return results;

                }
                catch (System.IO.IOException ex)
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
            
            IndexItemCollection results = new IndexItemCollection();

            if (string.IsNullOrEmpty(queryText))
            {
                return results;
            }

            bool DisableSearchFeatureFilters = WebConfigSettings.DisableSearchFeatureFilters;

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                if (!IndexReader.IndexExists(searchDirectory)) {  return results;  }

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
                        Lucene.Net.Util.Version.LUCENE_30,
                        new string[] { queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty) },
                        new string[] { "Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword" },
                        analyzer);

                    BooleanQuery filterQuery = new BooleanQuery(); // won't be used to score the results

                    if (!isAdminContentAdminOrSiteEditor) // skip role filters for these users
                    {
                        AddRoleFilters(userRoles, filterQuery);
                        AddModuleRoleFilters(userRoles, filterQuery);
                    }
                 
                    TermRangeQuery beginDateFilter = new TermRangeQuery(
                        "PublishBeginDate",
                        DateTime.MinValue.ToString("s"),
                        DateTime.UtcNow.ToString("s"),
                        true,
                        true);

                    filterQuery.Add(beginDateFilter, Occur.MUST);

                    TermRangeQuery endDateFilter = new TermRangeQuery(
                        "PublishEndDate",
                        DateTime.UtcNow.ToString("s"),
                        DateTime.MaxValue.ToString("s"),
                        true,
                        true);

                    filterQuery.Add(endDateFilter, Occur.MUST);


                    if ((modifiedBeginDate.Date > DateTime.MinValue.Date) || (modifiedEndDate.Date < DateTime.MaxValue.Date))
                    {
                        TermRangeQuery lastModifiedDateFilter = new TermRangeQuery(
                            "LastModUtc",
                            modifiedBeginDate.Date.ToString("s"),
                            modifiedEndDate.Date.ToString("s"),
                            true,
                            true);

                        filterQuery.Add(lastModifiedDateFilter, Occur.MUST);

                    }
                   
                    if ((!DisableSearchFeatureFilters) && (featureGuid != Guid.Empty))
                    {
                        BooleanQuery featureFilter = new BooleanQuery();
                        featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST);
                        filterQuery.Add(featureFilter, Occur.MUST);
                    }

                    Filter filter = new QueryWrapperFilter(filterQuery); // filterQuery won't affect result scores

                    using (IndexSearcher searcher = new IndexSearcher(searchDirectory))
                    { 
                        
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
                                    TokenStream stream = analyzer.TokenStream("contents", new StringReader(doc.Get("contents")));
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
                catch (BooleanQuery.TooManyClauses ex)
                {
                    invalidQuery = true;
                    log.Error("handled error for search terms " + queryText, ex);
                    return results;

                }
                catch (System.IO.IOException ex)
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
            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(siteId))
            {
                using (IndexReader reader = IndexReader.Open(searchDirectory, false))
                {
                    Term term = new Term("Key", key);
                    reader.DeleteDocuments(term);
                }

            }


        }



        //implementation as of 2013-01-10, new version is above

        //public static IndexItemCollection Search(
        //    int siteId,
        //    bool isAdmin,
        //    List<string> userRoles,
        //    Guid featureGuid,
        //    string queryText,
        //    bool highlightResults,
        //    int highlightedFragmentSize,
        //    int pageNumber,
        //    int pageSize,
        //    int maxClauseCount,
        //    bool sortByPubDateDescending,
        //    out int totalHits,
        //    out bool invalidQuery)
        //{
        //    invalidQuery = false;
        //    totalHits = 0;
        //    string indexPath = GetIndexPath(siteId);
        //    IndexItemCollection results = new IndexItemCollection();

        //    if (string.IsNullOrEmpty(queryText))
        //    {
        //        return results;
        //    }

        //    bool useBackwardCompatibilityMode = WebConfigSettings.SearchUseBackwardCompatibilityMode;
        //    bool DisableSearchFeatureFilters = WebConfigSettings.DisableSearchFeatureFilters;
        //    bool IncludeModuleRoleFilters = WebConfigSettings.SearchIncludeModuleRoleFilters;

        //    Lucene.Net.Store.Directory d = new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(indexPath));

        //    //if (IndexReader.IndexExists(indexPath))
        //    if (IndexReader.IndexExists(d))
        //    {

        //        if (debugLog)
        //        {
        //            log.Debug("Entered Search, indexPath = " + indexPath);
        //        }

        //        long startTicks = DateTime.Now.Ticks;

        //        try
        //        {
        //            if (maxClauseCount != 1024)
        //            {
        //                //BooleanQuery.SetMaxClauseCount(maxClauseCount);
        //                BooleanQuery.MaxClauseCount = maxClauseCount;
        //            }
        //            BooleanQuery mainQuery = new BooleanQuery();

        //            if ((!isAdmin) && (!useBackwardCompatibilityMode))
        //            {
        //                AddRoleQueries(userRoles, mainQuery);
        //            }

        //            if ((!isAdmin) && (IncludeModuleRoleFilters))
        //            {
        //                AddModuleRoleQueries(userRoles, mainQuery);
        //            }


        //            //Query multiQuery = MultiFieldQueryParser.Parse(
        //            //    new string[] { queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty) },
        //            //    new string[] { "Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword" },
        //            //    new StandardAnalyzer());

        //            Query multiQuery = MultiFieldQueryParser.Parse(
        //                Lucene.Net.Util.Version.LUCENE_30,
        //                new string[] { queryText, queryText, queryText, queryText, queryText, queryText.Replace("*", string.Empty) },
        //                new string[] { "Title", "ModuleTitle", "contents", "PageName", "PageMetaDesc", "Keyword" },
        //                new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));



        //            //mainQuery.Add(multiQuery, BooleanClause.Occur.MUST);
        //            mainQuery.Add(multiQuery, Occur.MUST);


        //            if (!useBackwardCompatibilityMode)
        //            {
        //                Term beginDateStart = new Term("PublishBeginDate", DateTime.MinValue.ToString("s"));
        //                Term beginDateEnd = new Term("PublishBeginDate", DateTime.UtcNow.ToString("s"));
        //                //RangeQuery beginDateQuery = new RangeQuery(beginDateStart, beginDateEnd, true);
        //                TermRangeQuery beginDateQuery = new TermRangeQuery(
        //                    "PublishBeginDate",
        //                    DateTime.MinValue.ToString("s"),
        //                    DateTime.UtcNow.ToString("s"),
        //                    true,
        //                    true);

        //                //mainQuery.Add(beginDateQuery, BooleanClause.Occur.MUST);
        //                mainQuery.Add(beginDateQuery, Occur.MUST);

        //                Term endDateStart = new Term("PublishEndDate", DateTime.UtcNow.ToString("s"));
        //                Term endDateEnd = new Term("PublishEndDate", DateTime.MaxValue.ToString("s"));
        //                //RangeQuery endDateQuery = new RangeQuery(endDateStart, endDateEnd, true);
        //                TermRangeQuery endDateQuery = new TermRangeQuery(
        //                    "PublishEndDate",
        //                    DateTime.UtcNow.ToString("s"),
        //                    DateTime.MaxValue.ToString("s"),
        //                    true,
        //                    true);

        //                //mainQuery.Add(endDateQuery, BooleanClause.Occur.MUST);
        //                mainQuery.Add(endDateQuery, Occur.MUST);
        //            }

        //            if ((!DisableSearchFeatureFilters) && (featureGuid != Guid.Empty))
        //            {
        //                BooleanQuery featureFilter = new BooleanQuery();
        //                //featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), BooleanClause.Occur.MUST);
        //                featureFilter.Add(new TermQuery(new Term("FeatureId", featureGuid.ToString())), Occur.MUST);
        //                //mainQuery.Add(featureFilter, BooleanClause.Occur.MUST);
        //                mainQuery.Add(featureFilter, Occur.MUST);
        //            }

        //            Lucene.Net.Store.SimpleFSDirectory dir = new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(indexPath));

        //            //IndexSearcher searcher = new IndexSearcher(indexPath);
        //            IndexSearcher searcher = new IndexSearcher(dir);
        //            // a 0 based colection
        //            //Hits hits = searcher.Search(mainQuery);
        //            int maxResults = 500;

        //            //http://stackoverflow.com/questions/9872933/migrating-lucene-hitcollector-2-x-to-collector-3-x
        //            //TopScoreDocCollector collector = TopScoreDocCollector.Create(maxResults, true);

        //            TopDocs hits = searcher.Search(mainQuery, maxResults);

        //            int startHit = 0;
        //            if (pageNumber > 1)
        //            {
        //                startHit = ((pageNumber - 1) * pageSize);
        //            }
        //            //totalHits = hits.Length();
        //            totalHits = hits.TotalHits;

        //            int end = startHit + pageSize;
        //            if (totalHits <= end)
        //            {
        //                end = totalHits;
        //            }
        //            int itemsAdded = 0;
        //            int itemsToAdd = end;

        //            // in backward compatibility mode if multiple pages of results are found we may not be showing every user the correct
        //            // number of hits they can see as we only filter out the current page
        //            //we may decrement total hits if filtering results so keep the original count
        //            int actualHits = totalHits;

        //            if (!useBackwardCompatibilityMode)
        //            {
        //                // this new way is much cleaner
        //                //all filtering is done by query so the hitcount is true
        //                //whereas with the old way it could be wrong since there
        //                // were possibly results filtered out after the query returned.

        //                QueryScorer scorer = new QueryScorer(multiQuery);
        //                SimpleHTMLFormatter formatter = new SimpleHTMLFormatter("<span class='searchterm'>", "</span>");
        //                Highlighter highlighter = new Highlighter(formatter, scorer);
        //                //highlighter.SetTextFragmenter(new SimpleFragmenter(highlightedFragmentSize));
        //                highlighter.TextFragmenter = new SimpleFragmenter(highlightedFragmentSize);


        //                for (int i = startHit; i < itemsToAdd; i++)
        //                {
        //                    //IndexItem indexItem = new IndexItem(hits.Doc(i), hits.Score(i));

        //                    Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
        //                    IndexItem indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

        //                    if (highlightResults)
        //                    {
        //                        try
        //                        {
        //                            //TokenStream stream = new StandardAnalyzer().TokenStream("contents", new StringReader(hits.Doc(i).Get("contents")));
        //                            TokenStream stream = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30).TokenStream("contents", new StringReader(doc.Get("contents")));

        //                            //string highlightedResult = highlighter.GetBestFragment(stream, hits.Doc(i).Get("contents"));
        //                            string highlightedResult = highlighter.GetBestFragment(stream, doc.Get("contents"));
        //                            if (highlightedResult != null) { indexItem.Intro = highlightedResult; }
        //                        }
        //                        catch (NullReferenceException) { }

        //                    }

        //                    results.Add(indexItem);
        //                    itemsAdded += 1;

        //                }


        //            }
        //            else
        //            {
        //                //backward compatible with old indexes
        //                int filteredItems = 0;
        //                for (int i = startHit; i < itemsToAdd; i++)
        //                {

        //                    bool needToDecrementTotalHits = false;
        //                    Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);

        //                    if (
        //                        (isAdmin)
        //                        || (WebUser.IsContentAdmin)
        //                        //|| (WebUser.IsInRoles(hits.Doc(i).Get("ViewRoles")))
        //                        || (WebUser.IsInRoles(doc.Get("ViewRoles")))
        //                        )
        //                    {
        //                        //IndexItem indexItem = new IndexItem(hits.Doc(i), hits.Score(i));


        //                        IndexItem indexItem = new IndexItem(doc, hits.ScoreDocs[i].Score);

        //                        if (
        //                        (DateTime.UtcNow > indexItem.PublishBeginDate)
        //                        && (DateTime.UtcNow < indexItem.PublishEndDate)
        //                        )
        //                        {
        //                            results.Add(indexItem);
        //                        }
        //                        else
        //                        {
        //                            needToDecrementTotalHits = true;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        needToDecrementTotalHits = true;
        //                    }

        //                    //filtered out a result so need to decrement
        //                    if (needToDecrementTotalHits)
        //                    {
        //                        filteredItems += 1;
        //                        totalHits -= 1;

        //                        //we also are not getting as many results as the page size so if there are more items
        //                        //we should increment itemsToAdd
        //                        if ((itemsAdded + filteredItems) < actualHits)
        //                        {
        //                            itemsToAdd += 1;
        //                        }
        //                    }

        //                }
        //            }

        //            //searcher.Close();
        //            searcher.Dispose();

        //            results.ItemCount = itemsAdded;
        //            results.PageIndex = pageNumber;

        //            results.ExecutionTime = DateTime.Now.Ticks - startTicks;
        //        }
        //        catch (ParseException ex)
        //        {
        //            invalidQuery = true;
        //            log.Error("handled error for search terms " + queryText, ex);
        //            // these parser exceptions are generally caused by
        //            // spambots posting too much junk into the search form
        //            // heres an option to automatically ban the ip address
        //            HandleSpam(queryText, ex);


        //            return results;
        //        }
        //        catch (BooleanQuery.TooManyClauses ex)
        //        {
        //            invalidQuery = true;
        //            log.Error("handled error for search terms " + queryText, ex);
        //            return results;

        //        }

        //    }

        //    return results;
        //}

       

        private static void HandleSpam(string queryText, Exception ex)
        {
            bool autoBanSpamBots = ConfigHelper.GetBoolProperty("AutoBanSpambotsOnSearchErrors", false);
           
            if ((autoBanSpamBots)&&(IsSpam(queryText)))
            {
                if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
                {
                    BannedIPAddress b = new BannedIPAddress();
                    b.BannedIP = HttpContext.Current.Request.UserHostAddress;
                    b.BannedReason = "spambot autodetected";
                    b.BannedUtc = DateTime.UtcNow;
                    b.Save();

                    //String pathToCacheDependencyFile
                    //        = HttpContext.Current.Server.MapPath(
                    //    "~/Data/bannedipcachedependency.config");

                    //CacheHelper.TouchCacheFile(pathToCacheDependencyFile);

                    //log.Error(queryText, ex);
                    log.Info("spambot detected, ip address has been banned: " + HttpContext.Current.Request.UserHostAddress);
                }
            }
            else
            {
                //log.Error(queryText, ex);
                log.Info("spambot possibly detected, ip address was: " + HttpContext.Current.Request.UserHostAddress);
            
            }

           


        }

        private static bool IsSpam(string queryText)
        {
            // Commented out the below on 2009-05-25 because now that we are using query string params for search instead
            // of a form field, we can no longer assume abuse simply by the query being longer than 255 chars
            //if (queryText.Length > 255) { return true; }

            // TODO: determine by key words?

            return false;
        }


        


        public static Regex MarkupRegex = new Regex("<[/a-zA-Z]+[^>]*>|<!--(?!-->)*-->");

        public static string ConvertToText(string markup)
        {
            return MarkupRegex.Replace(markup, " ");
        }

        private static void AddRoleFilters(List<string> userRoles, BooleanQuery mainQuery)
        {
            BooleanQuery rolesQuery = new BooleanQuery();
            foreach (string role in userRoles)
            {
                Term term = new Term("Role", role);
                TermQuery termQuery = new TermQuery(term);
                rolesQuery.Add(termQuery, Occur.SHOULD);             
            }
            // in a boolean query with multiple should occur items, at least one must occur
            mainQuery.Add(rolesQuery, Occur.MUST);      
        }

        private static void AddModuleRoleFilters(List<string> userRoles, BooleanQuery mainQuery)
        {
            BooleanQuery rolesQuery = new BooleanQuery();
            foreach (string role in userRoles)
            {
                Term term = new Term("ModuleRole", role);

                TermQuery termQuery = new TermQuery(term);
                rolesQuery.Add(termQuery, Occur.SHOULD);

            }
            // in a boolean query with multiple should occur items, at least one must occur

            mainQuery.Add(rolesQuery, Occur.MUST);

        }

        //private static BooleanQuery BuildRoleQuery(List<string> userRoles)
        //{
        //    BooleanQuery bQuery = new BooleanQuery();
        //    foreach (string role in userRoles)
        //    {        
        //        bQuery.Add(new TermQuery(new Term("ViewRoles2", role)), Occur.SHOULD);

        //    }
        //    // in a boolean query with multiple should occur items, at least one must occur

        //    return bQuery;
        //}



        private static BooleanQuery BuildQueryFromKeywords(Hashtable keyWords)
        {
            BooleanQuery bQuery = new BooleanQuery();
            foreach (DictionaryEntry keywordFilterTerm in keyWords)
            {
                string field = keywordFilterTerm.Key.ToString();
                string keyword = keywordFilterTerm.Value.ToString();
                bQuery.Add( new TermQuery(new Term(field, keyword)), Occur.SHOULD);

            }

            return bQuery;
        }


        public static void RebuildIndex(IndexItem indexItem)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (indexItem == null) return;

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
            if (WebConfigSettings.DisableSearchIndex) { return; }

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

            if (indexItem == null) return;

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

            if (indexItem == null) return;
            if (indexPath == null) return;
            if (indexPath.Length == 0) return;

            IndexingQueue queueItem = new IndexingQueue();
            queueItem.SiteId = indexItem.SiteId;
            queueItem.IndexPath = indexPath;
            queueItem.ItemKey = indexItem.Key;
            queueItem.RemoveOnly = true;
            queueItem.SerializedItem = SerializationHelper.SerializeToString(indexItem);
            queueItem.Save();

            // the above queues the items to be indexed. Edit page must also call SiteUtils.QueueIndexing(); after the content is deleted.

           
        }


   
        public static void RemoveIndexItem(
            int pageId,
            int moduleId, 
            int itemId)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            
            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("IndexHelper.RemoveIndexItem tried to obtain a SiteSettings object but it came back null");
                return;
            }

            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteSettings.SiteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemId = itemId;
            indexItem.IndexPath = GetSearchIndexPath(siteSettings.SiteId);

            RemoveIndex(indexItem);

            if (debugLog) log.Debug("Removed Index ");
        }

        public static void RemoveIndexItem(
            int pageId,
            int moduleId,
            string itemKey)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null)
            {
                log.Error("IndexHelper.RemoveIndexItem tried to obtain a SiteSettings object but it came back null");
                return;
            }

            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteSettings.SiteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemKey = itemKey;
            indexItem.IndexPath = GetSearchIndexPath(siteSettings.SiteId);

            RemoveIndex(indexItem);

            if (debugLog) log.Debug("Removed Index ");
        }

        public static void RemoveIndexItem(
            int siteId,
            int pageId,
            int moduleId,
            int itemId,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            IndexItem indexItem = new IndexItem();
            indexItem.SiteId = siteId;
            indexItem.PageId = pageId;
            indexItem.ModuleId = moduleId;
            indexItem.ItemId = itemId;
            indexItem.IndexPath = indexPath;

            RemoveIndex(indexItem);

            if (debugLog) log.Debug("Removed Index ");
        }


        

        public static void DeleteSearchIndex(SiteSettings siteSettings)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            string indexPath = GetSearchIndexPath(siteSettings.SiteId);
            if (indexPath.Length == 0) { return; }
            if (!Directory.Exists(indexPath)) { return; }

            try
            {
                DirectoryInfo dir = new DirectoryInfo(indexPath);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo f in files)
                {
                    File.Delete(f.FullName);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }


        #region ClearPageIndex


        public static void ClearPageIndexAsync(PageSettings pageSettings)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            pageSettings.IndexPath = GetSearchIndexPath(pageSettings.SiteId);

            if (ThreadPool.QueueUserWorkItem(new WaitCallback(ClearPageIndexAsyncCallback), pageSettings))
            {
                if (debugLog) log.Debug("IndexHelper.ClearPageIndexAsyncCallback queued");
            }
            else
            {
                if (debugLog) log.Debug("Failed to queue a thread for IndexHelper.ClearPageIndexAsync");
            }
        }


        private static void ClearPageIndexAsyncCallback(object o)
        {
            if (o == null) return;
            if (!(o is PageSettings)) return;

            //try
            //{
                PageSettings pageSettings = (PageSettings)o;
                IndexHelper.ClearPageIndex(pageSettings);
            //}
            //catch (Exception ex)
            //{
            //    if (log.IsErrorEnabled) log.Error("IndexHelper.ClearPageIndexAsyncCallback", ex);
            //}
        }


        private static bool ClearPageIndex(PageSettings pageSettings)
        {
           
            if (pageSettings == null) { return false; }
            bool result = false;

            try
            {

            using (Lucene.Net.Store.Directory searchDirectory = GetDirectory(pageSettings.SiteId))
            {

            if (IndexReader.IndexExists(searchDirectory))
                    {

                        using (IndexReader reader = IndexReader.Open(searchDirectory, false))
                        {


                            try
                            {
                                int tot = reader.NumDocs();

                                for (int i = 0; i < tot; i++)
                                {
                                    Document doc = reader.Document(i);

                                    if (doc.GetField("PageID").StringValue ==
                                        pageSettings.PageId.ToString(CultureInfo.InvariantCulture))

                                    {
                                        if (debugLog) log.Debug("ClearPageIndex about to delete doc ");
                                        try
                                        {
                                            reader.DeleteDocument(i);
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
            if (WebConfigSettings.DisableSearchIndex) { return; }

            pageSettings.IndexPath = GetSearchIndexPath(pageSettings.SiteId);

            if (ThreadPool.QueueUserWorkItem(
                new WaitCallback(RebuildPageIndexAsyncCallback), pageSettings))
            {
                if (debugLog) log.Debug("IndexHelper.RebuildPageIndexCallback queued");
            }
            else
            {
                if (debugLog) log.Debug("Failed to queue a thread for IndexHelper.RebuildPageIndexAsync");
            }
        }


        private static void RebuildPageIndexAsyncCallback(object o)
        {
            if (o == null) return;
            if (!(o is PageSettings)) return;

            try
            {
                PageSettings pageSettings = (PageSettings)o;
                IndexHelper.RebuildPageIndex(pageSettings);

                // TODO: could add some form of notification to let the admin know if
                // it was able to index all the content
            }
            catch (TypeInitializationException ex)
            {
                if (log.IsErrorEnabled) log.Error("IndexHelper.RebuildPageIndexAsyncCallback", ex);
            }
        }

        private static bool RebuildPageIndex(PageSettings pageSettings)
        {
            if (pageSettings == null) return false;

            log.Info("IndexHelper.RebuildPageIndex - " + pageSettings.PageName);

            if (IndexBuilderManager.Providers == null)
            {
                log.Info("No IndexBuilderProviders found");
                return false;
            }


            string indexPath = GetSearchIndexPath(pageSettings.SiteId);

            ClearPageIndex(pageSettings);

            foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
            {
                indexBuilder.RebuildIndex(pageSettings, indexPath);
            }

            return true;
        }


       
        #endregion


        #region RebuildSiteIndex

        public static bool VerifySearchIndex(SiteSettings siteSettings)
        {
            if (WebConfigSettings.DisableSearchIndex) { return false; }

            string indexPath = GetSearchIndexPath(siteSettings.SiteId);
            if (indexPath.Length == 0) { return false; }

            if (!Directory.Exists(indexPath))
            {
                Directory.CreateDirectory(indexPath);
            }

            if (Directory.Exists(indexPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(indexPath);
                int fileCount = directoryInfo.GetFiles().Length;
                int configFileCount = directoryInfo.GetFiles(".config").Length;
                if (
                    (fileCount == 0)
                    || (
                        (fileCount == 1)
                        && (configFileCount == 1)
                        )
                    )
                {
                    int rowsToIndex = IndexingQueue.GetCount();
                    if (rowsToIndex > 0)
                    {
                        // already started the indexing process
                        return true;
                    }
                    // no search index exists so build it
                    IndexHelper.RebuildSiteIndexAsync(indexPath, CacheHelper.GetMenuPages());
                    return false;
                }
            }

            return true;
        }


        public static void RebuildSiteIndexAsync(
            string indexPath, IEnumerable<PageSettings> menuPages)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            ArrayList arrayList = new ArrayList();
            arrayList.Add(indexPath);
            arrayList.Add(menuPages);

            if (ThreadPool.QueueUserWorkItem(
                new WaitCallback(RebuildSiteIndexAsyncCallback), arrayList))
            {
                if (debugLog) log.Debug("IndexHelper.RebuildSiteIndexAsyncCallback queued");
            }
            else
            {
                if (debugLog) log.Debug("Failed to queue a thread for IndexHelper.RebuildSiteIndexAsync");
            }
        }


        private static void RebuildSiteIndexAsyncCallback(object objArrayList)
        {

            ArrayList arrayList = (ArrayList)objArrayList;
            string indexPath = (string)arrayList[0];

            IEnumerable<PageSettings> menuPages
                = (IEnumerable<PageSettings>)arrayList[1];

            RebuildSiteIndex(indexPath, menuPages);
        }

        private static void RebuildSiteIndex(
            string indexPath,
            IEnumerable<PageSettings> menuPages)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            // clean out index entirely

            using (Lucene.Net.Store.SimpleFSDirectory d = new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(indexPath)))
            {

                if (IndexReader.IndexExists(d))
                {

                    using (IndexReader reader = IndexReader.Open(d, false))
                    {

                        for (int i = 0; i < reader.NumDocs(); i++)
                        {
                            reader.DeleteDocument(i);
                        }
         
                    }

                }

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
            foreach (PageSettings pageSettings in menuPages)
            {
                foreach (IndexBuilderProvider indexBuilder in IndexBuilderManager.Providers)
                {
                    if (indexBuilder.Name == "ForumThreadIndexBuilderProvider")
                    {
                        indexBuilder.RebuildIndex(pageSettings, indexPath);
                    }
                }

            }

            log.Info("Finished indexing Forums.");

            return;
        }


        


        #endregion

        public static string GetNullSafeString(this Lucene.Net.Documents.Document doc, string fieldName)
        {
            string s = doc.Get(fieldName);
            if (string.IsNullOrEmpty(s)) { return string.Empty; }
            return s;

        }

        
    }
}
