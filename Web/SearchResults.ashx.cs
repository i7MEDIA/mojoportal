// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//
// Author:					
// Created:				    2009-05-22
// Last Modified:			2012-12-26
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using Resources;
using mojoPortal.Web.Framework;
using log4net;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Returns search results in OpenSearch response Atom 1.0 format
    /// http://www.opensearch.org/Specifications/OpenSearch/1.1#OpenSearch_response_elements
    /// </summary>
    public class SearchResultsHandler : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SearchResultsHandler));

        private string query = string.Empty;
        private int pageNumber = 1;
        private int pageSize = WebConfigSettings.SearchResultsPageSize;
        private int totalHits = 0;
        private int totalPages = 1;
        private int start = 1;
        private int end = 1;
        //private bool indexVerified = false;
        private bool showModuleTitleInResultLink = WebConfigSettings.ShowModuleTitleInSearchResultLink;
        private bool isSiteEditor = false;
        private Guid featureGuid = Guid.Empty;
        private SiteSettings siteSettings = null;
        private mojoPortal.SearchIndex.IndexItemCollection searchResults = null;
        private string SiteRoot = string.Empty;
        private bool queryErrorOccurred = false;
        private DateTime modifiedBeginDate = DateTime.MinValue;
        private DateTime modifiedEndDate = DateTime.MaxValue;


        public void ProcessRequest(HttpContext context)
        {
            LoadSettings(context);

            if (WebConfigSettings.DisableSearchIndex)
            {
                context.Response.Redirect(SiteRoot);
                return;
            }

           
            DoSearch(context);

            if (searchResults != null)
            {
                RenderSearchResults(context, searchResults);
                return;
            }

            ProcessError(context);

        }


        private void RenderSearchResults(HttpContext context, mojoPortal.SearchIndex.IndexItemCollection searchResults)
        {
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            string sanitizedQuery = SecurityHelper.RemoveMarkup(query);

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("feed");
                xmlTextWriter.WriteAttributeString("xmlns", "http://www.w3.org/2005/Atom");
                xmlTextWriter.WriteAttributeString("xmlns:opensearch", "http://a9.com/-/spec/opensearch/1.1/");
                
                xmlTextWriter.WriteStartElement("title");
                xmlTextWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, Resource.SearchTitleFormat, siteSettings.SiteName, sanitizedQuery));
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("link");
                xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.ashx?q=" + context.Server.UrlEncode(sanitizedQuery));
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("updated");
                xmlTextWriter.WriteValue(DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T"));
                xmlTextWriter.WriteEndElement();


                xmlTextWriter.WriteStartElement("author");

                xmlTextWriter.WriteStartElement("name");
                xmlTextWriter.WriteValue(siteSettings.CompanyName);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("id");
                xmlTextWriter.WriteValue("urn:uuid:" + siteSettings.SiteGuid.ToString());
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("opensearch:totalResults");
                xmlTextWriter.WriteValue(totalHits.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("opensearch:startIndex");
                xmlTextWriter.WriteValue(start.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("opensearch:itemsPerPage");
                xmlTextWriter.WriteValue(pageSize.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("opensearch:Query");
                xmlTextWriter.WriteAttributeString("role", "request");
                xmlTextWriter.WriteAttributeString("searchTerms", sanitizedQuery);
                xmlTextWriter.WriteAttributeString("startPage", "1");
                xmlTextWriter.WriteEndElement();

                //this is a link to the xhtml search page
                xmlTextWriter.WriteStartElement("link");
                xmlTextWriter.WriteAttributeString("rel", "alternate");
                xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.aspx?q=" 
                    + context.Server.UrlEncode(sanitizedQuery) 
                    + "&p=" + pageNumber.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteAttributeString("type", "text/html");
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("link");
                xmlTextWriter.WriteAttributeString("rel", "self");
                xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.ashx?q="
                    + context.Server.UrlEncode(sanitizedQuery)
                    + "&p=" + pageNumber.ToString(CultureInfo.InvariantCulture));
                xmlTextWriter.WriteAttributeString("type", "application/atom+xml");
                xmlTextWriter.WriteEndElement();

                if (totalPages > 1)
                {
                    xmlTextWriter.WriteStartElement("link");
                    xmlTextWriter.WriteAttributeString("rel", "first");
                    xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.ashx?q="
                        + context.Server.UrlEncode(sanitizedQuery)
                        + "&p=1");
                    xmlTextWriter.WriteAttributeString("type", "application/atom+xml");
                    xmlTextWriter.WriteEndElement();

                    if (pageNumber > 1)
                    {
                        xmlTextWriter.WriteStartElement("link");
                        xmlTextWriter.WriteAttributeString("rel", "previous");
                        xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.ashx?q="
                            + context.Server.UrlEncode(sanitizedQuery)
                            + "&p=" + (pageNumber -1).ToString(CultureInfo.InvariantCulture));
                        xmlTextWriter.WriteAttributeString("type", "application/atom+xml");
                        xmlTextWriter.WriteEndElement();

                    }

                    if (pageNumber < totalPages)
                    {
                        xmlTextWriter.WriteStartElement("link");
                        xmlTextWriter.WriteAttributeString("rel", "next");
                        xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.ashx?q="
                            + context.Server.UrlEncode(sanitizedQuery)
                            + "&p=" + (pageNumber + 1).ToString(CultureInfo.InvariantCulture));
                        xmlTextWriter.WriteAttributeString("type", "application/atom+xml");
                        xmlTextWriter.WriteEndElement();

            
                    }

                    xmlTextWriter.WriteStartElement("link");
                    xmlTextWriter.WriteAttributeString("rel", "last");
                    xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchResults.ashx?q="
                        + context.Server.UrlEncode(sanitizedQuery)
                        + "&p=" + totalPages.ToString(CultureInfo.InvariantCulture));
                    xmlTextWriter.WriteAttributeString("type", "application/atom+xml");
                    xmlTextWriter.WriteEndElement();

                    
                }

                ////this is a  link to the description
                xmlTextWriter.WriteStartElement("link");
                xmlTextWriter.WriteAttributeString("rel", "search");
                xmlTextWriter.WriteAttributeString("type", "application/opensearchdescription+xml");
                xmlTextWriter.WriteAttributeString("href", SiteRoot + "/SearchEngineInfo.ashx");
                xmlTextWriter.WriteEndElement();

                
                foreach (mojoPortal.SearchIndex.IndexItem item in searchResults)
                {
                    xmlTextWriter.WriteStartElement("entry");

                    xmlTextWriter.WriteStartElement("link");
                    xmlTextWriter.WriteValue(BuildUrl(item));
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("id");
                    xmlTextWriter.WriteValue(BuildUrl(item));
                    xmlTextWriter.WriteEndElement();

                   
                    //xmlTextWriter.WriteRaw("<updated>2003-12-13T18:30:02Z</updated>");

                    xmlTextWriter.WriteStartElement("content");
                    xmlTextWriter.WriteAttributeString("type", "text");
                    xmlTextWriter.WriteCData(item.Intro);    
                    xmlTextWriter.WriteEndElement();

 
                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndElement();
            }



        }

        private void DoSearch(HttpContext context)
        {
            // this is only to make sure its initialized
            // before indexing is queued on a thread
            // because there is no HttpContext on
            // external threads and httpcontext is needed to initilaize
            // once initialized its cached
            IndexBuilderProviderCollection
                indexProviders = IndexBuilderManager.Providers;


            searchResults = mojoPortal.SearchIndex.IndexHelper.Search(
                siteSettings.SiteId,
                isSiteEditor,
                GetUserRoles(context),
                featureGuid,
                modifiedBeginDate,
                modifiedEndDate,
                query,
                false,
                WebConfigSettings.SearchResultsFragmentSize,
                pageNumber,
                pageSize,
                WebConfigSettings.SearchMaxClauseCount,
                out totalHits,
                out queryErrorOccurred);

            start = 1;
            if (pageNumber > 1)
            {
                start = ((pageNumber - 1) * pageSize) + 1;
            }

            end = pageSize;
            if (start > 1) { end += start; }

            if (end > totalHits)
            {
                end = totalHits;
            }

            totalPages = 1;
            int pageLowerBound = (pageSize * pageNumber) - pageSize;

            if (pageSize > 0) { totalPages = totalHits / pageSize; }

            if (totalHits <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalHits, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

        }

		public string BuildUrl(IndexItem indexItem)
		{
			string value = string.Empty;
			if (indexItem.UseQueryStringParams)
			{
				value = "/" + indexItem.ViewPage
					+ "?pageid="
					+ indexItem.PageId.ToInvariantString()
					+ "&mid="
					+ indexItem.ModuleId.ToInvariantString()
					+ "&ItemID="
					+ indexItem.ItemId.ToInvariantString()
					+ indexItem.QueryStringAddendum;

			}
			else
			{
				value = "/" + indexItem.ViewPage;
			}

			if (value.StartsWith("/"))
			{
				value = SiteRoot + value;
			}

			return value;

		}

		private void ProcessError(HttpContext context)
        {
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            //string imageSiteRoot = WebUtils.GetSiteRoot();

            context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            context.Response.Write("<feed xmlns=\"http://www.w3.org/2005/Atom\" xmlns:opensearch=\"http://a9.com/-/spec/opensearch/1.1/\">");

            //using (StreamReader sr = file.OpenText())
            //{
            //    context.Response.Write(sr.ReadToEnd());
            //}

            context.Response.Write("</feed>");



        }

        private void LoadSettings(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            SiteRoot = SiteUtils.GetNavigationSiteRoot();

            if (context.Request.QueryString.Get("q") != null)
            {

                query = context.Request.QueryString.Get("q");
            }

            
            pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);

            if (!WebConfigSettings.DisableSearchFeatureFilters)
            {
                featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);
            }

            isSiteEditor = WebUser.IsAdminOrContentAdmin || (SiteUtils.UserIsSiteEditor());

            

        }

        private List<string> GetUserRoles(HttpContext context)
        {
            List<string> userRoles = new List<string>();

            userRoles.Add("All Users");

            if ((context.Request.IsAuthenticated)&&(siteSettings != null))
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    using (IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId))
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(reader["RoleName"].ToString());
                        }

                    }

                }


            }

            return userRoles;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
