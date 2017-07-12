/// Author:				    
/// Created:			    2004-08-28
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	
/// 
///	2005-09-24 added RSS link feature provided by Philip Gear
///		for enhanced RSS access in FireFox and Netscape
/// 
/// 2009-05-26 added autodiscovery link for OpenSearch support https://developer.mozilla.org/en/Creating_OpenSearch_plugins_for_Firefox
/// 2009-07-27 added config option for content type so its possible to serve as text/html in anticipation of moving to Html 5
/// 2010-04-27 added logic to automatically add <meta name="viewport" content="width=670, initial-scale=0.45, minimum-scale=0.45"/> for iphone
/// 2011-11-23 added option to automatically add open graph protocol description based on regular meta description, enabled by default
/// 2012-06-13 added support for facebook app id meta

using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
	public partial class MetaContent : UserControl
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(MetaContent));

        //private const string metaEncodingXhtml = "\n<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />";
        //private const string metaEncodingHtml = "\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";

       
        private string keywordCsv = string.Empty;
        private string description = string.Empty;
        private string additionalMetaMarkup = string.Empty;
        private StringBuilder keywords = null;
        private SiteSettings siteSettings = null;
        private bool preZoomForIPhone = true;

        public bool PreZoomForIPhone
        {
            get { return preZoomForIPhone; }
            set { preZoomForIPhone = value; }
        }

        public string KeywordCsv
        {
            get { return keywordCsv; }
            set { keywordCsv = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private bool addOpenGraphDescription = true;
        /// <summary>
        /// see
        /// http://www.mojoportal.com/Forums/Thread.aspx?thread=9301&mid=34&pageid=5&ItemID=2&pagenumber=1#post38648
        /// </summary>
        public bool AddOpenGraphDescription
        {
            get { return addOpenGraphDescription; }
            set { addOpenGraphDescription = value; }
        }

        public string AdditionalMetaMarkup
        {
            get { return additionalMetaMarkup; }
            set { additionalMetaMarkup = value; }
        }

        private bool disableContentType = false;
        public bool DisableContentType
        {
            get { return disableContentType; }
            set { disableContentType = value; }
        }

        private bool includeFacebookAppId = true;
        public bool IncludeFacebookAppId
        {
            get { return includeFacebookAppId; }
            set { includeFacebookAppId = value; }
        }

        public void AddKeword(string keyword)
        {
            if (keyword == null) { return; }

            if (keywords == null)
            {
                keywords = new StringBuilder();
                keywords.Append(keyword);
                return;
            }

            if (keywords.Length > 0)
            {
                keywords.Append("," + keyword);
            }
            else
            {
                keywords.Append(keyword);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (WebConfigSettings.AutoSetContentType) { AddEncoding(); }

            AddDescription();
            AddKeywords();

            if (additionalMetaMarkup.Length > 0)
            {
                Literal additionalMeta = new Literal();
                additionalMeta.Text = additionalMetaMarkup;
                this.Controls.Add(additionalMeta);
            }

            AddOpenSearchLink();
            AddIPhoneZoom();

            if (includeFacebookAppId)
            {
                AddFacebookAppId();
            }

        }

        private void AddIPhoneZoom()
        {
            if (!preZoomForIPhone) { return; }
            if (HttpContext.Current == null) { return; }
            if (Request.UserAgent == null) { return; }
            if (Request.UserAgent.Length == 0) { return; }
            if (!Request.UserAgent.Contains("iPhone")) { return; }

            //http://developer.apple.com/library/ios/#technotes/tn2010/tn2262/_index.html
            //<meta name="viewport" content="width=device-width" />

            Literal lit = new Literal();
            lit.Text = "\n<meta name=\"viewport\" content=\"width=670, initial-scale=0.45, minimum-scale=0.45\" />";
            this.Controls.Add(lit);

        }

        private void AddKeywords()
        {
            if (keywords != null)
            {
                if (keywordCsv.Length > 0)
                {
                    keywordCsv = keywordCsv + "," + keywords.ToString();
                }
                else
                {
                    keywordCsv = keywords.ToString();
                }
            }

            if (keywordCsv.Length == 0) { return; }

            Literal metaKeywordsLiteral = new Literal();
            metaKeywordsLiteral.Text = "\n<meta name=\"keywords\" content=\"" + keywordCsv + "\" />"; 
            this.Controls.Add(metaKeywordsLiteral);

        }

        //https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12155~1#post50489

        // "<meta property=\"og:description\" content=\"{0}\" />";
        // "<meta name=\"og:description\" content=\"{0}\" />";
        private string openGraphDescriptionFormat = "<meta property=\"og:description\" content=\"{0}\" />";

        public string OpenGraphDescriptionFormat
        {
            get { return openGraphDescriptionFormat; }
            set { openGraphDescriptionFormat = value; }
        }

        private void AddDescription()
        {
            if (description.Length == 0) { return; }

            Literal metaDescriptionLiteral = new Literal();
            metaDescriptionLiteral.Text = "\n<meta name=\"description\" content=\"" + description + "\" />";
            if (addOpenGraphDescription)
            {
                metaDescriptionLiteral.Text += "\n" + string.Format(CultureInfo.InvariantCulture, openGraphDescriptionFormat, description);
            }
            this.Controls.Add(metaDescriptionLiteral);

            
        }

        private void AddEncoding()
        {
            if (disableContentType) { return; }

            string contentTypeMeta = "\n<meta http-equiv=\"Content-Type\" content=\"" 
                + WebConfigSettings.ContentMimeType 
                + "; charset=" + WebConfigSettings.ContentEncoding + "\" />";

            Literal metaEncodingLiteral = new Literal();
            metaEncodingLiteral.Text = contentTypeMeta;
            this.Controls.Add(metaEncodingLiteral);
        }

        private void AddOpenSearchLink()
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (WebConfigSettings.DisableOpenSearchAutoDiscovery) { return; }

            if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
            if (siteSettings == null) { return; }

            string searchTitle;
            if (siteSettings.OpenSearchName.Length > 0)
            {
                searchTitle = siteSettings.OpenSearchName;
            }
            else
            {
                searchTitle = string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName);
            }

            Literal openSearchLink = new Literal();
            openSearchLink.Text = "\n<link rel=\"search\" type=\"application/opensearchdescription+xml\" title=\""
                + searchTitle + "\" href=\"" + SiteUtils.GetNavigationSiteRoot() + "/SearchEngineInfo.ashx" + "\" />";

            this.Controls.Add(openSearchLink);
        }

        private void AddFacebookAppId()
        {
            

            string fbAppId = string.Empty;

            if (WebConfigSettings.FacebookAppId.Length > 0)
            {
                fbAppId = WebConfigSettings.FacebookAppId;
            }
            else
            {
                if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
                if (siteSettings == null) { return; }
                fbAppId = siteSettings.FacebookAppId;
            }

            if (fbAppId.Length == 0) { return; }

            string meta = "<meta property=\"fb:app_id\" content=\"" + siteSettings.FacebookAppId + "\"/>";

            Literal metaLiteral = new Literal();
            metaLiteral.Text = meta;
            this.Controls.Add(metaLiteral);

        }

        // TODO: http://www.w3.org/P3P/validator/20020128/document implement xml
        //http://www.w3.org/P3P/validator.html
        //private void AddP3PLink()
        //{
        //    if (WebConfigSettings.DisableSearchIndex) { return; }
        //    if (WebConfigSettings.DisableOpenSearchAutoDiscovery) { return; }

        //    if (siteSettings == null) { siteSettings = CacheHelper.GetCurrentSiteSettings(); }
        //    if (siteSettings == null) { return; }

        //    string searchTitle = string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName);

        //    Literal openSearchLink = new Literal();
        //    openSearchLink.Text = "\n<link rel=\"P3Pv1\" href=\"" + SiteUtils.GetNavigationSiteRoot() + "/SearchEngineInfo.ashx" + "\" />";

        //    this.Controls.Add(openSearchLink);
        //}

        

	}
}
