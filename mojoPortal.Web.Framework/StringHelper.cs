// Author:					
// Created:				2007-11-20
// Last Modified:			2017-10-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;


namespace mojoPortal.Web.Framework
{
    
    public static class StringHelper
    {

        public static bool IsCaseInsensitiveMatch(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
            //return (string.Compare(str1, str2, false, CultureInfo.InvariantCulture) == 0);

        }

        public static bool ContainsCaseInsensitive(this string source, string value)
        {
            int results = source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
            return results == -1 ? false : true;
        }

        public static string ToJsonString(object jsonObj)
        {
            return new JavaScriptSerializer().Serialize(jsonObj);
        }

        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// 
        /// The string returned includes outer quotes 
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string JsonEscape(this string s)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            //sb.Append("\"");

            return sb.ToString();
        }

        //private static string SafeJson(string sIn)
        //{
        //    StringBuilder sbOut = new StringBuilder(sIn.Length);
        //    foreach (char ch in sIn)
        //    {
        //        if (Char.IsControl(ch) || ch == '\'')
        //        {
        //            int ich = (int)ch;
        //            sbOut.Append(@"\u" + ich.ToString("x4"));
        //            continue;
        //        }
        //        else if (ch == '\"' || ch == '\\' || ch == '/')
        //        {
        //            sbOut.Append('\\');
        //        }
        //        sbOut.Append(ch);
        //    }
        //    return sbOut.ToString();
        //}

        
        /////  FUNCTION Enquote Public Domain 2002 JSON.org
        /////  @author JSON.org
        /////  @version 0.1
        /////  Ported to C# by Are Bjolseth, teleplan.no
        //public static string JsonEncode(this string s)
        //{
        //    if (s == null || s.Length == 0)
        //    {
        //        //return "\"\"";
        //        return s;
        //    }
        //    char c;
        //    int i;
        //    int len = s.Length;
        //    StringBuilder sb = new StringBuilder(len + 4);
        //    string t;

        //    sb.Append('"');
        //    for (i = 0; i < len; i += 1)
        //    {
        //        c = s[i];
        //        if ((c == '\\') || (c == '"') || (c == '>'))
        //        {
        //            sb.Append('\\');
        //            sb.Append(c);
        //        }
        //        else if (c == '\b')
        //            sb.Append("\\b");
        //        else if (c == '\t')
        //            sb.Append("\\t");
        //        else if (c == '\n')
        //            sb.Append("\\n");
        //        else if (c == '\f')
        //            sb.Append("\\f");
        //        else if (c == '\r')
        //            sb.Append("\\r");
        //        else
        //        {
        //            if (c < ' ')
        //            {
        //                //t = "000" + Integer.toHexString(c);
        //                string tmp = new string(c, 1);
        //                t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
        //                sb.Append("\\u" + t.Substring(t.Length - 4));
        //            }
        //            else
        //            {
        //                sb.Append(c);
        //            }
        //        }
        //    }
        //    sb.Append('"');
        //    return sb.ToString();
        //}


        //private static string EscapHtmlToJson(string html)
        //{
        //    if(string.IsNullOrEmpty(html)){ return html;}

        //    return "'" + html.Replace("</", "' + '</' + '").Replace("<", "' + '<' + '").Replace(">", "' + '>' + '").Replace("+ '\"}","}");

        //}

        public static string DecodeBase64String(string base64String, Encoding encoding)
        {
            if (string.IsNullOrEmpty(base64String)) { return base64String; }
            byte[] encodedBytes = Convert.FromBase64String(base64String);
            return encoding.GetString(encodedBytes, 0, encodedBytes.Length);

        }

        public static string GetBase64EncodedUnicodeString(string unicodeString)
        {
            Encoding encoding = Encoding.Unicode;
            byte[] bytes = encoding.GetBytes(unicodeString);

            return System.Convert.ToBase64String(bytes);

        }

        public static string GetBase64EncodedAsciiString(string unicodeString)
        {
            Encoding encoding = Encoding.ASCII;
            byte[] bytes = encoding.GetBytes(unicodeString);

            return System.Convert.ToBase64String(bytes);

        }



        #region string Extension Methods

        public static string ToSerialDate(this string s)
        {
            if (s.Length == 8)
            {
                //char[] chars = s.ToCharArray();
                return s.Substring(0, 4) + "-" + s.Substring(4, 2) + "-" + s.Substring(6, 2);
            }
            return s;
        }


		//public static string JsonEscapeHtml(this string s)
		//{
		//    if (string.IsNullOrEmpty(s)) { return s; }

		//    return EscapHtmlToJson(s);

		//}

        public static string HtmlEscapeQuotes(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("'","&#39;").Replace("\"","&#34;");

        }

        public static string RemoveCDataTags(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("<![CDATA[", string.Empty).Replace("]]>", string.Empty);

        }

        public static string CsvEscapeQuotes(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("\"", "\"\"");

        }

        public static string RemoveAngleBrackets(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("<", string.Empty).Replace(">", string.Empty);

        }

        public static string Coalesce(this string s, string alt)
        {
            if (string.IsNullOrEmpty(s)) { return alt; }
            return s;
        }

        /// <summary>
        /// Converts a unicode string into its closest ascii equivalent
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToAscii(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            
            try
            {
                
                string normalized = s.Normalize(NormalizationForm.FormKD);

                Encoding ascii = Encoding.GetEncoding(
                      "us-ascii",
                      new EncoderReplacementFallback(string.Empty),
                      new DecoderReplacementFallback(string.Empty));

                byte[] encodedBytes = new byte[ascii.GetByteCount(normalized)];
                int numberOfEncodedBytes = ascii.GetBytes(normalized, 0, normalized.Length,
                encodedBytes, 0);

                string newString = ascii.GetString(encodedBytes);

                return newString;

                //Encoding ascii = Encoding.ASCII;
                //Encoding unicode = Encoding.Unicode;
                //byte[] unicodeBytes = unicode.GetBytes(s);
                //byte[] asciiBytes = Encoding.Convert(unicode, ascii, unicodeBytes);

                //// Convert the new byte[] into a char[] and then into a string.
                //// This is a slightly different approach to converting to illustrate
                //// the use of GetCharCount/GetChars.
                //char[] asciiChars = new char[ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                //ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                //string asciiString = new string(asciiChars);
                //return asciiString;
            }
            catch
            {
                return s;
            }


        }

        /// <summary>
        /// Converts a unicode string into its closest ascii equivalent.
        /// If the ascii encode string length is less than or equal to 1 returns the original string
        /// as this means the string is probably in a language with no ascii equivalents
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToAsciiIfPossible(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            //http://www.mojoportal.com/Forums/Thread.aspx?thread=8974&mid=34&pageid=5&ItemID=9
            s = s.Replace("æ", "ae");
            s = s.Replace("Æ", "ae");
            s = s.Replace("å", "aa");
            s = s.Replace("Å", "aa");

            // based on:
            //http://www.mojoportal.com/Forums/Thread.aspx?thread=9176&mid=34&pageid=5&ItemID=9&pagenumber=1#post38114

            int len = s.Length;
            StringBuilder sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = s[i];
                
                if ((int)c >= 128)
                {
                    sb.Append(RemapInternationalCharToAscii(c));
                  
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();


        }

        /// <summary>
        /// Remap International Chars To Ascii
        /// http://meta.stackoverflow.com/questions/7435/non-us-ascii-characters-dropped-from-full-profile-url/7696#7696
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string RemoveNonNumeric(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            char[] result = new char[s.Length];
            int resultIndex = 0;
            foreach (char c in s)
            {
                if (char.IsNumber(c))
                    result[resultIndex++] = c;
            }
            if (0 == resultIndex)
                s = string.Empty;
            else if (result.Length != resultIndex)
                s = new string(result, 0, resultIndex);
            
            return s;
        }

        public static string RemoveLineBreaks(this string s, string Replacement = "")
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.Replace("\r\n", Replacement).Replace("\n", Replacement).Replace("\r", Replacement);
        }

        /// <summary>
        /// Removes Sequential Spaces in a string. RegEx isn't necessarily faster.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="UseRegEx"></param>
        /// <returns></returns>
        public static string RemoveMultipleSpaces(this string s, bool UseRegEx = false)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            if (UseRegEx)
            {
				Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
				return regex.Replace(s, " ");
			}

            StringBuilder sb = new StringBuilder(s.Length);

            int i = 0;
            foreach (char c in s)
            {
                if (c != ' ' || i == 0 || s[i-1] != ' ')
                {
                    sb.Append(c);
                }
                i++;
            }
            return sb.ToString();
        }

        public static string EscapeXml(this string s)
        {
            string xml = s;
            if (!string.IsNullOrEmpty(xml))
            {
                // replace literal values with entities
                xml = xml.Replace("&", "&amp;");
                xml = xml.Replace("&lt;", "&lt;");
                xml = xml.Replace("&gt;", "&gt;");
                xml = xml.Replace("\"", "&quot;");
                xml = xml.Replace("'", "&apos;");
            }
            return xml;
        }

        public static string UnescapeXml(this string s)
        {
            string unxml = s;
            if (!string.IsNullOrEmpty(unxml))
            {
                // replace entities with literal values
                unxml = unxml.Replace("&apos;", "'");
                unxml = unxml.Replace("&quot;", "\"");
                unxml = unxml.Replace("&gt;", "&gt;");
                unxml = unxml.Replace("&lt;", "&lt;");
                unxml = unxml.Replace("&amp;", "&");
            }
            return unxml;
        }


        public static List<string> SplitOnChar(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

        public static List<string> SplitOnCharAndTrim(this string s, char c)
        {
            var list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item.Trim()); }
            }

            return list;
        }

		public static List<int> SplitIntStringOnCharAndTrim(this string s, char c)
		{
			var list = new List<int>();
			if (string.IsNullOrEmpty(s)) { return list; }

			string[] a = s.Split(c);
			foreach (string item in a)
			{
				if (!string.IsNullOrEmpty(item)) { list.Add(Convert.ToInt32(item.Trim())); }
			}

			return list;
		}

		public static List<string> SplitOnNewLineAndTrim (this string s)
		{
			var list = new List<string>();
			if (string.IsNullOrWhiteSpace(s)) { return list; }
			string[] a = s.Split(
				new[] { "\r\n", "\r", "\n" },
				StringSplitOptions.RemoveEmptyEntries
			);
			foreach (string b in a)
			{
				if (!string.IsNullOrWhiteSpace(b)) { list.Add(b.Trim()); }
			}
			return list;
		}

        public static List<string> SplitOnPipes(string s)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split('|');
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

		/// <summary>
		/// Determines if a string is in an array of strings.
		/// </summary>
		/// <returns>bool</returns>
		/// <example>
		/// string color = "blue";
		/// string[] colors = {"blue", "green", "red"};
		/// bool validColor = false;
		/// validColor = color.IsIn(colors);
		/// </example>
		public static bool IsIn<T>(this T source, params T[] values)
		{
			return values.Contains(source);
		}

        #endregion

    }
}
