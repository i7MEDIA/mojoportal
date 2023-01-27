// Author:					
// Created:				    2007-05-14
// Last Modified:			2017-10-09
// 
// 07/05/2007  Alexander Yushchenko added confirmation dialog functions
//				
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.Framework
{
    
    public static class UIHelper
    {
        public const string CenterColumnId = "divCenter";
        public const string LeftColumnId = "divLeft";
        public const string RightColumnId = "divRight";


        public const string ArtisteerPostMetaHeader = "art-PostMetadataHeader";
        public const string ArtPostHeader = "art-PostHeader";
        public const string ArtisteerBlockHeader = "art-BlockHeader";

        public const string ArtisteerPost = "art-Post";
        public const string ArtisteerPostContent = "art-PostContent";
        public const string ArtisteerBlock = "art-Block";
        public const string ArtisteerBlockContent = "art-BlockContent";

        public const string ArtisteerPostMetaHeaderLower = "art-postmetadataheader";
        public const string ArtPostHeaderLower = "art-postheader";
        public const string ArtisteerBlockHeaderLower = "art-blockheader";

        public const string ArtisteerPostLower = "art-post";
        public const string ArtisteerPostContentLower = "art-postcontent";
        public const string ArtisteerBlockLower = "art-block";
        public const string ArtisteerBlockContentLower = "art-blockcontent";

        private const string ConfirmationDialogReturn = "if(!confirm(this.dataset.confirmation)) {return};";
        private const string ConfirmationDialogReturnValue = "return confirm(this.dataset.confirmation);";
        private const string ConfirmationDialogWithClearExitCode = "unHookGoodbyePrompt(); return confirm(this.dataset.confirmation);";
        private const string ClearPageExitCodeWithReturn = "unHookGoodbyePrompt(); return true;";
        private const string ClearPageExitCode = "unHookGoodbyePrompt();";
		private const string DisableAfterClick = "this.value=dataset.disableAfterClick;this.disabled = true;";
		public static string GetColumnId(this Control c)
        {
            Control parent = c.Parent;
            if (parent == null) { return CenterColumnId; }
            while (parent != null)
            {
                if (parent.ID == CenterColumnId) { return CenterColumnId; }
                if (parent.ID == LeftColumnId) { return LeftColumnId; }
                if (parent.ID == RightColumnId) { return RightColumnId; }

                parent = parent.Parent;
            }

            return CenterColumnId;

        }

        public static void SetOrAppendCss(this WebControl c, string cssClass)
        {
            if (string.IsNullOrEmpty(cssClass)) { return; }

            if (c.CssClass.Length == 0)
            {
                c.CssClass = cssClass;
                return;
            }

            var classes = c.CssClass.SplitOnCharAndTrim(' ');

            if (classes != null && !classes.Contains(cssClass))
            {
                c.CssClass += " " + cssClass;
            }

        }

        public static void RemoveCss(this WebControl c, string cssClass)
        {
			if (string.IsNullOrEmpty(cssClass) || c.CssClass.Length == 0) { return; }

            var classes = c.CssClass.SplitOnCharAndTrim(' ');

            if (classes != null && classes.Contains(cssClass))
            {
                classes.Remove(cssClass);
            }

            c.CssClass = string.Join(" ", classes);
		}


		public static string ToInvariantString(this int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static string ToInvariantString(this float i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

		public static string ToInvariantString(this decimal i)
		{
			return i.ToString(CultureInfo.InvariantCulture);
		}

        /// <summary>
        /// for easily converting an Enum to an IDictionary for data binding ie for a dropdown list
        /// </summary>
        public static IDictionary<int, string> EnumToDictionary<TEnum>() where TEnum : struct
        {
            Type enumerationType = typeof(TEnum);

            if (!enumerationType.IsEnum)
                throw new ArgumentException("Enumeration type is expected.");

            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (int value in Enum.GetValues(enumerationType))
            {
                var name = Enum.GetName(enumerationType, value);
                dictionary.Add(value, name);
            }

            return dictionary;
        }

		/// <summary>
		/// convert a Semi-Colon and Pipe delimited string to a string,string dictionary
		/// example input string: key|value;key|value;key|value;
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static IDictionary<string, string> GetDictionaryFromString(string str)
		{
			List<string> keyValuePairs = str.SplitOnChar(';');
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string kvp in keyValuePairs)
			{
				List<string> kv = kvp.SplitOnCharAndTrim('|');
				dictionary.Add(kv[0], kv[1]);
			}
			return dictionary;
		}
		public static string SelectedItemsToSemiColonSeparatedString(this ListItemCollection list)
        {
            StringBuilder result = new StringBuilder();
            foreach (ListItem item in list)
            {
                if (item.Selected)
                {
                    result.Append(item.Value + ";");
                }
            }

            return result.ToString();
        }

        public static string SelectedItemsToCommaSeparatedString(this ListItemCollection list)
        {
            StringBuilder result = new StringBuilder();
            string comma = string.Empty;
            foreach (ListItem item in list)
            {
                if (item.Selected)
                {
                    result.Append(comma + item.Value);
                    comma = ",";
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Convert a hex string to a .NET Color object. Returns Color.Blue on error
        /// </summary>
        /// <param name="hexColor">a hex string: "FFFFFF", "#000000"</param>
        public static Color HexStringToColor(string hexColor)
        {
            string hc = ExtractHexDigits(hexColor);
            if (hc.Length != 6)
            {
                return Color.Blue;
            }
            string r = hc.Substring(0, 2);
            string g = hc.Substring(2, 2);
            string b = hc.Substring(4, 2);
            Color color = Color.Blue;
            try
            {
                int ri
                   = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
                int gi
                   = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
                int bi
                   = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
                color = Color.FromArgb(ri, gi, bi);
            }
            catch
            {
                return Color.Blue;
            }
            return color;
        }
        /// <summary>
        /// Extract only the hex digits from a string.
        /// </summary>
        public static string ExtractHexDigits(string input)
        {
            // remove any characters that are not digits (like #)
            Regex isHexDigit = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
            string newnum = string.Empty;
            foreach (char c in input)
            {
                if (isHexDigit.IsMatch(c.ToString()))
                    newnum += c.ToString();
            }
            return newnum;
        }


        /// <summary>
        /// Throws exception if the given string is null or empty.
        /// </summary>
        /// <param name="value">
        /// The value to check.
        /// </param>
        /// <param name="name">
        /// The argument name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the given value is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the given value is empty.
        /// </exception>
        public static void ValidateNotNullOrEmpty(string value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            if (value.Length == 0)
                throw new ArgumentException(name + " cannot be empty.", name);
        }

        /// <summary>
        /// Throws exception if the given value is null.
        /// </summary>
        /// <param name="value">
        /// The value to check.
        /// </param>
        /// <param name="name">
        /// The argument name.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If the given value is null.
        /// </exception>
        public static void ValidateNotNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }


        public static String BuildHtmlErrorPage(Exception ex)
        {
            String errorHtml = "<html><head><title>Error</title>" 
                + "<link id='Link1' rel='stylesheet' href='" + WebUtils.GetSiteRoot() + "/Data/style/setup.css' type='text/css' /></head>"
                + "<body><div class='settingrow'><label class='settinglabel' >An Error Occurred:</label>" 
                + ex.Message + "</div>"
                + "<div class='settingrow'><label class='settinglabel' >Source:</label>" + ex.Source + "</div>"
                + "<div class='settingrow'><label class='settinglabel' >Stack Trace</label>" + ex.StackTrace + "</div>"
                + "</body></html>";

            return errorHtml;

        }

        public static string GetPagerLinks(
            string pageUrl, 
            int fromPage, 
            int toPage, 
            int selectedPage)
        {
            if (String.IsNullOrEmpty(pageUrl)) return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = fromPage; i < toPage + 1; i++)
            {
                string cssClass = (selectedPage == i) ? "SelectedPage" : "ModulePager";
                stringBuilder.AppendFormat
                    ("<a class='{0}' href='{1}{2}'>{3}</a>&nbsp;", cssClass, pageUrl, i, i);

                if ((i % 20) == 0)
                {
                    stringBuilder.Append("<br />");
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// This pager added by Juliano Barbosa
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        /// <param name="prevPageAlt"></param>
        /// <param name="nextPageAlt"></param>
        /// <param name="firstPageAlt"></param>
        /// <param name="lastPageAlt"></param>
        /// <returns>page links</returns>
        public static string CreatePagerLinks(
            string pageUrl, 
            int currentPage, 
            int totalPages,
            string prevPageAlt,
            string nextPageAlt,
            string firstPageAlt,
            string lastPageAlt)
        {

            StringBuilder sbPager = new StringBuilder();
            if (currentPage != 1)
            {
                if ((currentPage - 1) != 1)
                {
                    // first page link
                    sbPager.Append("<a title='" + firstPageAlt + "' alt='"
                        + firstPageAlt + "' href='");
                    sbPager.Append(pageUrl);
                    sbPager.Append("1");
                    sbPager.Append("'>|&lt;</a> ");
                }

                // previous page link
                sbPager.Append("<a title='" + prevPageAlt + "' alt='"
                        + prevPageAlt + "' href='");
                sbPager.Append(pageUrl);
                sbPager.Append(currentPage - 1);
                sbPager.Append("' >&lt;&lt;</a>  ");
            }

           
            if (currentPage != totalPages)
            {
                // next page link
                sbPager.Append("<a title='" + nextPageAlt + "' alt='"
                        + nextPageAlt + "' href='");
                sbPager.Append(pageUrl);
                sbPager.Append(currentPage + 1);
                sbPager.Append("'>&gt;&gt;</a>  ");

                // last page link
                sbPager.Append("<a title='" + lastPageAlt + "' alt='"
                        + lastPageAlt + "' href='");
                sbPager.Append(pageUrl);
                sbPager.Append(totalPages);
                sbPager.Append("'>&gt;|</a>");
            }

            return sbPager.ToString();
        }

        //public static string GetPagerLinksWithPrevNext(
        //    string pageUrl,
        //    int fromPage,
        //    int toPage,
        //    int selectedPage)
        //{
        //    return GetPagerLinksWithPrevNext(pageUrl, fromPage, toPage, selectedPage, "ModulePager", "SelectedPage");
        //}


        public static string GetPagerLinksWithPrevNext(
            string pageUrl,
            int fromPage,
            int toPage,
            int selectedPage,
            string mainStyle,
            string selectedStyle)
        {
            if (String.IsNullOrEmpty(pageUrl)) return String.Empty;

            string mainCssClass =
                String.IsNullOrEmpty(mainStyle) ? String.Empty : "class='" + mainStyle + "' ";
            string selectedCssClass =
                String.IsNullOrEmpty(selectedStyle) ? String.Empty : "class='" + selectedStyle + "' ";

            StringBuilder stringBuilder = new StringBuilder();
            int previousPage = selectedPage - 1;
            int nextPage = selectedPage + 1;
            if (previousPage > 0)
            {
                stringBuilder.AppendFormat
                    ("<a {0} href='{1}{2}'>&lt;&lt;&lt;</a>&nbsp;", mainCssClass, pageUrl, previousPage);
            }
            if (nextPage <= toPage)
            {
                stringBuilder.AppendFormat
                    ("<a {0} href='{1}{2}'>&gt;&gt;&gt;</a>&nbsp;", mainCssClass, pageUrl, nextPage);
            }

            for (int i = fromPage; i < toPage + 1; i++)
            {
                string cssClass = (selectedPage == i) ? selectedCssClass : mainCssClass;
                stringBuilder.AppendFormat
                    ("<a {0} href='{1}{2}'>{3}</a>&nbsp;", cssClass, pageUrl, i, i);
                if ((i % 20) == 0)
                {
                    stringBuilder.Append("<br />");
                }
            }
            return stringBuilder.ToString();
        }


        public static string GetAlphaPagerLinks(
            string pageUrl,
            int pageNumber,
            string sourceCharString,
            string selectedLetter)
        {
            if (String.IsNullOrEmpty(pageUrl)) return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in sourceCharString.ToCharArray())
            {
                String strChar = c.ToString();
                string cssClass = (selectedLetter == strChar) ? "SelectedPage" : "ModulePager";
                stringBuilder.AppendFormat
                    ("<a class='{0}' href='{1}1&amp;letter={2}'>{3}</a> ", cssClass, pageUrl, strChar, strChar);
            }

            return stringBuilder.ToString();
        }

        public static string GetAlphaPagerLinks(
            string pageUrl,
            int pageNumber,
            string sourceCharString,
            string selectedLetter,
            string selectedPageClass,
            string otherPageClass,
            bool useList,
            string firstLink)
        {
            if (string.IsNullOrEmpty(pageUrl)) return String.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            if (useList) { stringBuilder.Append("<ul>"); }

            if (!string.IsNullOrEmpty(firstLink))
            {
                if (useList) { stringBuilder.Append("<li>"); }
                stringBuilder.Append(firstLink);
                if (useList) { stringBuilder.Append("</li>"); }
            }

            foreach (char c in sourceCharString.ToCharArray())
            {
                if (useList) { stringBuilder.Append("<li>"); }
                string strChar = c.ToString();
                string cssClass = (selectedLetter == strChar) ? selectedPageClass : otherPageClass;
                if (cssClass.Length > 0)
                {
                    stringBuilder.AppendFormat
                        ("<a class='{0}' href='{1}1&amp;letter={2}'>{3}</a> ", cssClass, pageUrl, strChar, strChar);
                }
                else
                {
                    stringBuilder.AppendFormat
                        ("<a href='{0}1&amp;letter={1}'>{2}</a> ", pageUrl, strChar, strChar);
                }
                if (useList) { stringBuilder.Append("</li>"); }
            }

            if (useList) { stringBuilder.Append("</ul>"); }

            return stringBuilder.ToString();
        }

		#region Confirmation Dialog functions

		public static void AddConfirmationDialog(WebControl button, string confirmationText)
		{
			if (button == null) return;
            //var onclick = string.Format("return confirm('{0}');", confirmationText);

			if (button.Attributes["onclick"] != null)
            {
                button.Attributes["onclick"] = ConfirmationDialogReturnValue + button.Attributes["onclick"];
			}
            else
            {
			    button.Attributes.Add("onclick", ConfirmationDialogReturnValue);
            }
            button.Attributes.Add("data-confirmation", confirmationText);
		}

		public static void AddConfirmationDialog(HtmlButton button, string confirmationText)
		{
			if (button == null) return;
            //var onclick = string.Format("if(!confirm('{0}')) {{return}};", confirmationText);
			if (button.Attributes["onclick"] != null)
			{
				button.Attributes["onclick"] = ConfirmationDialogReturn + button.Attributes["onclick"];
			}
			else
			{
			    button.Attributes.Add("onclick", ConfirmationDialogReturn);
			}
			button.Attributes.Add("data-confirmation", confirmationText);
		}

		public static void AddConfirmationDialogWithClearExitCode(WebControl button, string confirmationText)
        {
            if (button == null) return;
            //var onclick = string.Format("", confirmationText);

			if (button.Attributes["onclick"] != null)
			{
				button.Attributes["onclick"] = ConfirmationDialogWithClearExitCode + button.Attributes["onclick"];
			}
			else
			{
				button.Attributes.Add("onclick", ConfirmationDialogWithClearExitCode);
			}
			button.Attributes.Add("data-confirmation", confirmationText);

		}

		public static void RemoveConfirmationDialog(WebControl button)
        {
            if (button == null) return;
            if (button.Attributes["onclick"] != null)
            {
                button.Attributes["onclick"] = button.Attributes["onclick"]
                    .Replace(ConfirmationDialogReturn, string.Empty)
                    .Replace(ConfirmationDialogReturnValue, string.Empty)
                    .Replace(ConfirmationDialogWithClearExitCode, string.Empty);
			}
        }

        public static void AddClearPageExitCode(WebControl button)
        {
            if (button == null) return;
            //var onclick = "unHookGoodbyePrompt(); return true;";

			if (button.Attributes["onclick"] != null)
			{
				button.Attributes["onclick"] = ClearPageExitCodeWithReturn + button.Attributes["onclick"];
			}
			else
			{
				button.Attributes.Add("onclick", ClearPageExitCodeWithReturn);
			}
		}

        #endregion


        /// <summary>
        /// Sets up a button so that once clicked, it can't be clicked again until complete.
        /// It displays the passed in text while disabled. After finishing the work you are
        /// required to set the button text back to its original text.
        /// Example usage:
        /// UIHelper.DisableButtonAfterClick(
        ///        btnSavePreferences,
        ///        Resource.ButtonDisabledPleaseWait,
        ///        Page.ClientScript.GetPostBackEventReference(this.btnSavePreferences, string.Empty)
        ///        );
        ///  You should not disable the button if you are using client side validation as this can result in the button remaining disabled
        ///  after client side valdiation prevents postback
        /// </summary>
        /// <param name="button"></param>
        /// <param name="disabledText"></param>
        /// <param name="postbackEventReference"></param>
        public static void DisableButtonAfterClick(WebControl button, string disabledText, string postbackEventReference)
        {
            if (button == null) return;
			if (button.Attributes["onclick"] != null)
			{
				button.Attributes["onclick"] = DisableAfterClick + postbackEventReference + button.Attributes["onclick"];
			}
			else
			{
				button.Attributes.Add("onclick", DisableAfterClick + postbackEventReference);

			}

			button.Attributes.Add("data-disable-after-click", disabledText);

		}

		public static void DisableButtonAfterClickAndClearExitCode(WebControl button, string disabledText, string postbackEventReference)
        {
            if (button == null) return;

			if (button.Attributes["onclick"] != null)
			{
				button.Attributes["onclick"] = ClearPageExitCode + DisableAfterClick + postbackEventReference + button.Attributes["onclick"];
			}
			else
			{
			    button.Attributes.Add("onclick", ClearPageExitCode + DisableAfterClick + postbackEventReference);
			}

			button.Attributes.Add("data-disable-after-click", disabledText);

		}

		//button.OnClientClick = "this.value='"
		//    + disabledText
		//    + "';this.disabled = true;"
		//    + postbackEventReference;

		//public static void DisableButtonAfterClick(
		//    Button button,
		//    string disabledText)
		//{
		//    if (button == null) return;
		//    button.OnClientClick = "this.value='"
		//        + disabledText
		//        + "';this.disabled = true;";

		//    //button.Attributes.Add("onclick", "this.value='"
		//    //    + disabledText
		//    //    + "';this.disabled = true;"
		//    //    + postbackEventReference);
		//}



		public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            // if __EVENTTARGET is null, the control is a button type and we need to 
            // iterate over the form collection to find it
            else
            {
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton controls ...
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, ctl.Length - 2);
                        c = page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = page.FindControl(ctl);
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                             c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        /// <summary>
        /// creates a plain text excerpt of the passed in html content without any markup and with a length less than or equal to the passed in length.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="excerptLength"></param>
        /// <returns></returns>
        public static string CreateExcerpt(string content, int excerptLength)
        {
            return CreateExcerpt(content, excerptLength, string.Empty);
        }

        /// <summary>
        /// creates a plain text excerpt of the passed in html content without any markup and with a length less than or equal to the passed in length.
        /// </summary>
        public static string CreateExcerpt(string content, int excerptLength, string suffix)
        {
            if (content == null) { return content; }

            string result = SecurityHelper.RemoveMarkup(content);
            if (result.Length <= excerptLength) { return result; }
            result = result.Substring(0, excerptLength);
            if (!result.EndsWith(" "))
            {
                if (result.IndexOf(" ") > -1)
                {
                    result = result.Substring(0, result.LastIndexOf(" "));
                }
            }

            result += suffix;

            return result;
        }


        public static string FormatPlainTextAsHtml(string text)
        {
            text = HttpUtility.HtmlEncode(text);
            text = Regex.Replace(text, "\n\n", "<p />");
            text = Regex.Replace(text, "\n", "<br />");

            return text;
        }

        public static string ConvertHtmlBreaksToTextBreaks(string text)
        {
            text = Regex.Replace(text, "<p>", "\n\n");
            text = Regex.Replace(text, "<br />", "\n");

            return text;
        }

        /// <summary>
        /// removes white space from css to reduce network bandwidth usage and therefore improve performance
        /// 2008-11-01 found this method breaks some presentation.
        /// use CssMinify.cs instead.
        /// </summary>
        /// <param name="css"></param>
        /// <returns></returns>
        public static string CompressCss(string css)
        {

            css = Regex.Replace(css, "/\\*.+?\\*/", "", RegexOptions.Singleline);
            css = css.Replace("  ", string.Empty);
            css = css.Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, string.Empty);
            css = css.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            css = css.Replace(Environment.NewLine, string.Empty);
            css = css.Replace("\\t", string.Empty);
            css = css.Replace(" {", "{");
            css = css.Replace(" :", ":");
            css = css.Replace(": ", ":");
            css = css.Replace(", ", ",");
            css = css.Replace("; ", ";");
            css = css.Replace(";}", "}");
            css = Regex.Replace(css, "/\\*[^\\*]*\\*+([^/\\*]*\\*+)*/", "$1");
            css = Regex.Replace(css, "(?<=[>])\\s{2,}(?=[<])|(?<=[>])\\s{2,}(?=&nbsp;)|(?<=&ndsp;)\\s{2,}(?=[<])", string.Empty);

            return css;

        }

        public static int CompareFileNames(FileInfo f1, FileInfo f2)
        {
            return f1.FullName.CompareTo(f2.FullName);
        }


        

    }
}
