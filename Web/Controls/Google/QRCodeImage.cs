// Author:					
// Created:				    2012-01-31
// Last Modified:			2012-01-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using log4net;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Uses the google charts api to generate a QR code image for the TextToEncode Property
    /// Set AutoDetectPageUrl to true to automatically set the TextToEncode property based on the current page url
    /// </summary>
    public class QRCodeImage : Image
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(QRCodeImage));

        private const string GoogleQrCodeUrlFormat = "https://chart.googleapis.com/chart?chs={0}x{1}&cht=qr&chl={2}";

        private int sizeInPixels = 150;

        public int SizeInPixels
        {
            get { return sizeInPixels; }
            set { sizeInPixels = value; }
        }

        private string textToEncode = string.Empty;

        public string TextToEncode
        {
            get { return textToEncode; }
            set { textToEncode = value; }
        }

        private bool autoDetectPageUrl = false;

        public bool AutoDetectPageUrl
        {
            get { return autoDetectPageUrl; }
            set { autoDetectPageUrl = value; }
        }

        private bool removeSkinParam = true;

        public bool RemoveSkinParam
        {
            get { return removeSkinParam; }
            set { removeSkinParam = value; }
        }

        private void SetTextFromPageUrl()
        {
            if (removeSkinParam)
            {
                try
                {
                    textToEncode = FilteredUrl(WebUtils.GetSiteRoot() + Page.Request.RawUrl);
                }
                catch (Exception ex)
                {
                    log.Error("handled error ", ex);
                }
            }
            else
            {
                textToEncode = WebUtils.GetSiteRoot() + Page.Request.RawUrl;
            }

           
        }

        private string FilteredUrl(string rawUrl)
        {
            if (string.IsNullOrEmpty(rawUrl)) { return rawUrl; }
            if (rawUrl.IndexOf("?") == -1) { return rawUrl; }

            if (rawUrl.IndexOf("?") == (rawUrl.Length - 1))
            {
                return  rawUrl.Replace("?", string.Empty); 
            }

            string baseUrl = rawUrl.Substring(0, rawUrl.IndexOf("?"));
            string queryString = rawUrl.Substring(rawUrl.IndexOf("?"), (rawUrl.Length - rawUrl.IndexOf("?")));
            queryString = WebUtils.RemoveQueryStringParam(queryString, "skin");
            if (queryString.Length > 0)
            {
                return baseUrl + "?" + queryString;
            }

            return baseUrl;

        }

        

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (autoDetectPageUrl)
            {
                SetTextFromPageUrl();
            }

            if (textToEncode.Length == 0) { return; }

            ImageUrl = string.Format(CultureInfo.InvariantCulture, GoogleQrCodeUrlFormat, sizeInPixels, sizeInPixels, Page.Server.UrlEncode(textToEncode));
            AlternateText = textToEncode;
            ToolTip = textToEncode;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // don't render if the text to encode has not been set
            if (textToEncode.Length > 0)
            {
                base.Render(writer);
            }
        }
    }
}