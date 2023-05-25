// Author:
// Created:       2009-12-19
// Last Modified: 2017-08-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// This adds a div with javascript to enable google tranlsation of the page.
	/// There should only be one instance of this control on a given page.
	/// You should be careful about using this feature and not use it on pages containing sensitive data that you do not want to be passed to google tranlsate.
	/// </summary>
	public class GoogleTranslatePanel : Panel
	{
		private string languageCode = string.Empty;

		/// <summary>
		/// the language of the site
		/// </summary>
		public string LanguageCode
		{
			get { return languageCode; }
			set { languageCode = value; }
		}

		private string includeLanguages = string.Empty;

		/// <summary>
		/// the languages to include in the widget - comma separate like ar,en,fr,de
		/// </summary>
		public string IncludeLanguages
		{
			get { return includeLanguages; }
			set { includeLanguages = value; }
		}

		private bool allowSecurePageTranslation = true;

		/// <summary>
		/// This is false by default so that the widget will not be displayed on pages protected with SSL
		/// However according to the documentation translating secure pages is supported, the content will be sent to google 
		/// for tranlsation over an ssl connection ad will not be logged.
		/// </summary>
		public bool AllowSecurePageTranslation
		{
			get { return allowSecurePageTranslation; }
			set { allowSecurePageTranslation = value; }
		}

		private bool showToolbar = false;

		/// <summary>
		/// equivalent of autoDisplay, determines whether the toolbar is automatically shown when the user's browser language is different than the page
		/// if the user chooses to translate the page it still will show the toolbar unfortuantely
		/// </summary>
		public bool ShowToolbar
		{
			get { return showToolbar; }
			set { showToolbar = value; }
		}

		private bool trackInGoogleAnalytics = false;

		public bool TrackInGoogleAnalytics
		{
			get { return trackInGoogleAnalytics; }
			set { trackInGoogleAnalytics = value; }
		}

		private bool pageContainsMultipleLanguageContent = false;

		public bool PageContainsMultipleLanguageContent
		{
			get { return pageContainsMultipleLanguageContent; }
			set { pageContainsMultipleLanguageContent = value; }
		}

		private string layout = string.Empty;

		/// <summary>
		/// possible values are empty, HORIZONTAL, SIMPLE
		/// </summary>
		public string Layout
		{
			get { return layout; }
			set { layout = value; }
		}


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (WebConfigSettings.DisableGoogleTranslate)
			{
				Visible = false;
			}

			// don't pass secure content to the translater
			if (!allowSecurePageTranslation && SiteUtils.IsSecureRequest())
			{
				Visible = false;
			}

			if (!Visible)
			{
				return;
			}

			SetupMainScript();
			SetupInstanceScript();
		}

		private void SetupInstanceScript()
		{
			// http://translate.google.com/translate_tools

			if (languageCode.Length == 0)
			{
				CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();
				languageCode = defaultCulture.TwoLetterISOLanguageName;
			}

			StringBuilder script = new StringBuilder();

			script.Append("\n<script>\n");
			script.Append("function googleTranslateElementInit() {");
			script.Append("new google.translate.TranslateElement({");
			script.Append("pageLanguage: '" + languageCode + "'");

			if (includeLanguages.Length > 0)
			{
				script.Append(",includedLanguages: '" + includeLanguages + "'");
			}

			if (!showToolbar)
			{
				script.Append(",autoDisplay:false ");
			}

			if (pageContainsMultipleLanguageContent)
			{
				script.Append(",multilanguagePage:true ");
			}

			if (trackInGoogleAnalytics)
			{
				script.Append(",gaTrack:true ");
			}

			switch (layout.ToUpper())
			{
				case "HORIZONTAL":
					script.Append(",layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL ");
					break;

				case "SIMPLE":
					script.Append(",layout: google.translate.TranslateElement.InlineLayout.SIMPLE ");
					break;

				default:
					break;
			}

			script.Append("},");
			script.Append("'" + ClientID + "');}");
			script.Append("\n</script>");

			Page.ClientScript.RegisterClientScriptBlock(
				GetType(),
				UniqueID,
				script.ToString());
		}

		private void SetupMainScript()
		{
			Page.ClientScript.RegisterStartupScript(
				typeof(Page),
				"gtranslate",
				"\n<script src=\"//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit\"></script>"
			);
		}
	}
}
