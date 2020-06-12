/*

NeatHtml- Fighting XSS with JavaScript Judo
Copyright (C) 2007  Dean Brettle

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;
using System.Collections.Specialized;

namespace Brettle.Web.NeatHtml
{
	public class Filter
	{
		public static Filter DefaultFilter = new Filter();
				
		public string ClientSideFilterName = "NeatHtml.DefaultFilter";
		public string NoScriptDownlevelIEWidth = "100%";
		public string NoScriptDownlevelIEHeight = "400px";
		public bool SupportNoScriptTables = false;
		public int MaxComplexity = 10000;
		public Regex TrustedImageUrlRegex = null;
		public Regex SpamFreeLinkUrlRegex = null;

		public string FilterUntrusted(string untrusted)
		{
			return ScriptJail.Jail(untrusted, ClientSideFilterName, NoScriptDownlevelIEWidth, NoScriptDownlevelIEHeight,
									SupportNoScriptTables, MaxComplexity, TrustedImageUrlRegex, SpamFreeLinkUrlRegex);
		}
	}
	
	internal class ScriptJail
	{
		internal static string Jail(string untrusted, string clientSideFilterName, 
									string noScriptDownlevelIEWidth, string noScriptDownlevelIEHeight,
									bool supportNoScriptTables, int maxComplexity, 
		                            Regex trustedImageUrlRegex, Regex spamFreeLinkUrlRegex)
		{
			ScriptJail jail = new ScriptJail(supportNoScriptTables, maxComplexity, trustedImageUrlRegex, spamFreeLinkUrlRegex);
			string jailed = null;
			try
			{
				jailed = JailRE.Replace(untrusted, new MatchEvaluator(jail.GuardJail));

				// If any untrusted tables are still open, close any open attributes and tags
				// and then close all the tables.
				if (jail.UntrustedTables > 0)
				{
					jailed += ParserResetString;
					while (jail.UntrustedTables-- > 0)
						jailed += "</table>";
				}
			}
			catch (ContentTooComplexException)
			{
				jailed = "The content that belongs here is too complex to display securely.";
			}

			return String.Format(Format, clientSideFilterName, jailed, 
			                     noScriptDownlevelIEWidth, noScriptDownlevelIEHeight,
			                     maxComplexity, 
			                     trustedImageUrlRegex == null ? "null" : "new RegExp(\"" + trustedImageUrlRegex + "\")", Guid.NewGuid().ToString());
		}
		
		private ScriptJail(bool supportNoScriptTables, int maxComplexity, 
		                   Regex trustedImageUrlRegex, Regex spamFreeLinkUrlRegex)
		{
			SupportNoScriptTables = supportNoScriptTables;
			MaxComplexity = maxComplexity;
			TrustedImageUrlRegex = trustedImageUrlRegex;
			SpamFreeLinkUrlRegex = spamFreeLinkUrlRegex;
		}
		
		private bool SupportNoScriptTables;
		private int MaxComplexity;
		private Regex TrustedImageUrlRegex;
		private Regex SpamFreeLinkUrlRegex;
		
		private int Complexity;

		private static string[] propsAllowedWhenNoScript
			= {"azimuth","background-attachment","background-color","background-image","background-position",
				"background-repeat","background","border-collapse","border-color","border-spacing","border-style",
				"border-top","border-right","border-bottom","border-left","border-top-color","border-right-color",
				"border-bottom-color","border-left-color","border-top-style","border-right-style","border-bottom-style",
				"border-left-style","border-top-width","border-right-width","border-bottom-width","border-left-width",
				"border-width","border","bottom","caption-side","clear","clip","color","content",
				/* "counter-increment","counter-reset", */ // Don"t allow messing with the counters 
				"cue-after","cue-before","cue","cursor","direction","display","elevation","empty-cells",
				"float","font-family","font-size","font-style","font-variant","font-weight","font","height","left",
				"letter-spacing","line-height","list-style-image","list-style-position","list-style-type","list-style",
				"margin-right","margin-left","margin-top","margin-bottom","margin","max-height","max-width","min-height",
				"min-width","orphans","outline-color","outline-style","outline-width","outline","overflow","padding-top",
				"padding-right","padding-bottom","padding-left","padding","page-break-after","page-break-before",
				"page-break-inside","pause-after","pause-before","pause","pitch-range","pitch","play-during","position",
				"quotes","richness","right","speak-header","speak-numeral","speak-punctuation","speak","speech-rate",
				"stress","table-layout","text-align","text-decoration","text-indent","text-transform","top",
				"unicode-bidi","vertical-align","visibility","voice-family","volume","white-space","widows","width",
				"word-spacing","z-index"
			};
		
		private static string[] attrsAllowedWhenNoScript
			= { "abbr", "accept", "accesskey", "align", "alt", "axis", "bgcolor", "border", "cellpadding",
				"cellspacing", "cite", "class", "classid", "clear", "code", "color", "cols", "colspan", "compact", 
				"datetime", "dir", "disabled", "face", "frame", "frameborder", "headers", "height", "href", "hreflang", "id", 
				"label", "lang", "language", "longdesc", "marginheight", "marginwidth", "name", "noshade", "nowrap", 
				"readonly", "rel", "rev", "rows", "rowspan", "rules", "scope", "size", "span", "start", "summary", 
				"tabindex", "title", "type", "value", "width", "xml:lang", "xml:space"
				};
				
		private static StringDictionary AllowedAttributeNames = GetDict(attrsAllowedWhenNoScript); 
		
		private static StringDictionary AllowedPropertyNames = GetDict(propsAllowedWhenNoScript); 
		
		private static StringDictionary GetDict(string[] keys)
		{
			StringDictionary dict = new StringDictionary();
			for (int i = 0; i < keys.Length; i++)
			{
				dict.Add(keys[i], "true");
			}
			return dict;
		}
		
		// Style property value whitelist.  Note: '&' '\' and '(' [except 'rgb('] are not on it.
		private static string StylePropValueREString = "\\((?<=rgb\\()|[ -%')-9<-\\[\\]-~]";

		private static Regex StyleAttributeValueRE
			= new Regex("^(?: *(-?[_a-z][_a-z0-9-]*) *:(?:" + StylePropValueREString + ")*(?:;|$)){0,"
						+ propsAllowedWhenNoScript.Length + "}$",
						RegexOptions.Compiled | RegexOptions.IgnoreCase);
						
		private static string ParserResetString = "<NeatHtmlParserReset s='' d=\"\" /><script></script>";

		//private static string Format 
		//	= "\n"
		//	//+ "<!--[if gte IE 7]><!-->\n"
		//	+ "<div class='NeatHtml' data-nhContainerId='{6}' style='overflow: hidden; position: relative; border: none; padding: 0; margin: 0;'>\n"
		//	//+ "<!--<![endif]-->\n"
		//	//+ "<!--[if lt IE 7]>\n"
		//	//+ "<div class='NeatHtml' nhContainerId='{6}' style='width: {2}; height: {3}; overflow: auto; position: relative; border: none; padding: 0; margin: 0;'>\n"
		//	//+ "<![endif]-->\n"
		//	+ "<table style='border-spacing: 0;'><tr><td style='padding: 0;'><!-- test comment --><script type='text/javascript' data-nhScriptId='{6}'>\n"
		//	+ "try {{ {0}.BeginUntrusted('{6}'); }} catch (ex) {{ document.writeln('NeatHtml not found\\074!-' + '-'); }}</script>"
		//	+ "<div>{1}</div>"
		//	+ "<input name='NeatHtmlEndUntrusted' type='hidden' value=\"\" /><script type='text/javascript'></script><!-- > --><!-- <xmp></xmp><xml></xml><! --></td></tr></table><script type='text/javascript'>\n"
		//	+ "{0}.ProcessUntrusted({4}, {5});\n"
		//	+ "</script>\n"
		//	+ "</div><script type='text/javascript'>\n"
		//	+ "{0}.ResizeContainer(undefined,'{6}');\n"
		//	+ "</script>\n";

		private static string Format = @"
<div class='NeatHtml' data-nhContainerId='{6}' style='overflow: hidden; position: relative; border: none; padding: 0; margin: 0;'>
	<script type='text/javascript' data-nhScriptId='{6}'>
		try {{ {0}.BeginUntrusted('{6}'); }} catch (ex) {{ document.writeln('NeatHtml not found\\074!-' + '-'); }}
	</script>
	<div>{1}</div>
	<input name='NeatHtmlEndUntrusted' type='hidden' />
	<!-- > -->
	<!-- <xmp></xmp><xml></xml><! -->
	<script type='text/javascript'>{0}.ProcessUntrusted({4}, {5});</script>
</div>
<script type='text/javascript'>{0}.ResizeContainer(undefined,'{6}');</script>";

		private static string[] tagsAllowedWhenNoScript
			= { "a", "abbr", "acronym", "address", "b", "basefont", "bdo", "big", "blockquote", "br",
		 		"caption", "center", "cite", "code", "col", "colgroup", "dd", "del", "dfn", "dir", "div", "dl", "dt", 
		 		"em", "font", "h1", "h2", "h3", "h4", "h5", "h6", "hr", "i", "ins", "kbd", "li", "ol", "p", "pre", "q",
		 		"s", "samp", "small", "span", "strike", "strong", "sub", "sup", "table", "tbody", "td", "tfoot", "th",
		 		"thead", "tr", "tt", "u", "ul", "var",
		 		"script" // OK when script is disabled.  Hides script source from user.
		 		// Do NOT allow "iframe" or "object", unless you are willing to track them like with tables.
		 		// Do NOT allow "xmp" -- it is used to hold the untrusted content on some browsers 
		 		// (eg. Safari/Konqueror).
		 		// Do NOT allow "xml" -- it is used to hold the untrusted content on some browsers 
		 		// (eg. Windows Mobile).
				// Do NOT allow "input", unless you block name='NeatHtmlEndUntrusted'
			};

		private static Hashtable InfoForTag = GetInfoForTags();
		
		private static Hashtable GetInfoForTags()
		{
			Hashtable dict = new Hashtable();
			for (int i = 0; i < tagsAllowedWhenNoScript.Length; i++)
			{
				dict[tagsAllowedWhenNoScript[i]] = TagInfo.NOT_TABLE_RELATED;
			}
			dict["table"] = TagInfo.TABLE;
			dict["td"] = dict["th"] = TagInfo.TABLE_CELL;
			dict["caption"] = dict["colgroup"] = dict["col"] = dict["tbody"] 
				= dict["tfoot"] = dict["thead"] = dict["tr"]
					= TagInfo.TABLE_OTHER;
			return dict;
		}
		
		private static Regex JailRE
			= new Regex("(?=[<-])(?:" 	// Optimization - all of the alternatives start with '<' or '-'
																	// 1: Matches any open angle
									+ "(<"
										+ "(?:"
											+ "!(?:"
																	// 2: HTML Comment (without embedded "--")
												+ "(--[^-]*(?:-[^-]+)*-->)"
																	// 3: Contents of CDATA section
												+ "|(?:\\[CDATA\\[([^\\]]*(?:\\][^\\]]+)*)\\]\\]>)" 
											+ ")"
											+ "|(/)?"				// 4: Matches when an end tag
											+ "("					// 5: Matches the rest of the tag
																	// 6: Matches the tag name
												+ "([a-z][a-z0-9_:]*)?" 
																		
												+ "(?:[ \\t\\n\\r]+"
																	// 7: Matches an attr name
													+ "([_:a-z][_:a-z0-9.]*)" 
													+ "((?:"		// 8: Not empty if attr has a value
														+ "[ \\t\\n\\r]*=[ \\t\\n\\r]*"
																	// 9: Matches the quoted attr value
														+ "(\"[^<\"]*\"|'[^<']*'|[^\"'][^ \\t\\n\\r<>]*)"
													+ ")?)){0," + attrsAllowedWhenNoScript.Length + "}"
																	// 10: Matches if the tag is well-formed
												+ "([ \\t\\n\\r]*/?>)?"				
											+ ")"
										+ ")"
									+ ")"
									+ "|(--)"						// 11: "--" we need to encode to prevent ending a comment prematurely
								+ ")",
								RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		
		private string GuardJail(Match m)
		{
			if (Complexity++ > MaxComplexity) throw new ContentTooComplexException();
			if (m.Groups[11].Success) // "--"
				return "&#45;&#45;";
			if (m.Groups[2].Success) // HTML comment (not containing "--", so it isn't ambiguous)
				return "";
			if (m.Groups[3].Success) // CDATA Section.
				return HttpUtility.HtmlEncode(m.Groups[3].Value);
			if (!m.Groups[10].Success	// No ending '>'
				|| !m.Groups[6].Success // or no tag name
				|| (m.Groups[4].Success // or an end tag with...
					&& (m.Groups[7].Success // an attribute
						|| m.Groups[10].Value.EndsWith("/>")))) // or something like "</foo />"
			{
				// Not a tag or a potentially ill-formed tag so HTML encode it to be sure it doesn't confuse
				// the browser's parser.
				return "<NeatHtmlLt />&lt;" + m.Value.Substring(1);
			}

			// If we get here we have a well-formed tag.
			string lcTagName = m.Groups[6].Value.ToLower();

			// Handle attributes
			StringBuilder attrsBuilder = new StringBuilder();
			int v = 0; // The next quoted value capture index
			for (int a = 0; a < m.Groups[7].Captures.Count; a++)
			{
				if (Complexity++ > MaxComplexity) throw new ContentTooComplexException();
				string attrName = m.Groups[7].Captures[a].Value;
				string equalsQuotedValue = m.Groups[8].Captures[a].Value;
				string quotedValue = "\"" + attrName + "\""; // Default for implicit attrs
				if (equalsQuotedValue.Length > 0)
				{
					quotedValue = m.Groups[9].Captures[v].Value;
					if (quotedValue[0] != '"' && quotedValue[0] != '\'')
						quotedValue = "\"" + quotedValue.Replace("\"", "&quot;") + "\"";
					v++;
				}
				string lcAttrName = attrName.ToLower();
				if (lcAttrName == "href" && equalsQuotedValue.Length > 0)
				{
					string unquotedValue = quotedValue.Substring(1, quotedValue.Length-2);
					if (SpamFreeLinkUrlRegex == null || !SpamFreeLinkUrlRegex.IsMatch(unquotedValue))
					{
						attrsBuilder.Append(" rel=\"nofollow\"");
					}
				}
				if (AllowedAttributeNames.ContainsKey(lcAttrName))
				{
					attrsBuilder.Append(" " + attrName + "=" + quotedValue);
					continue;
				}
				if (lcAttrName == "style" && equalsQuotedValue.Length > 0)
				{
					Match savMatch = StyleAttributeValueRE.Match(quotedValue.Substring(1, quotedValue.Length-2));
					if (savMatch.Success)
					{
						int p = 0;
						for (;p < savMatch.Groups[1].Captures.Count; p++)
						{
							if (Complexity++ > MaxComplexity) throw new ContentTooComplexException();
							string lcPropName =  savMatch.Groups[1].Captures[p].Value.ToLower();
							if (!AllowedPropertyNames.ContainsKey(lcPropName))
								break;
						}
						if (p == savMatch.Groups[1].Captures.Count) // All prop names allowed
						{
							attrsBuilder.Append(" " + attrName + "=" + quotedValue);
							continue;
						}
					}
				}
				if (TrustedImageUrlRegex != null 
				    && lcTagName == "img" && lcAttrName == "src" && equalsQuotedValue.Length > 0)
				{
					string unquotedValue = quotedValue.Substring(1, quotedValue.Length-2);
					if (TrustedImageUrlRegex.IsMatch(unquotedValue))
					{
						attrsBuilder.Append(" " + attrName + "=" + quotedValue);
						continue;
					}
				}
				attrsBuilder.Append(" " + attrName + "_NeatHtmlReplace=" + quotedValue);
			}
			string attrs = attrsBuilder.ToString();
			
			if (TrustedImageUrlRegex != null && lcTagName == "img")
				return "<" + m.Groups[4].Value + m.Groups[6].Value + attrs + m.Groups[10];
			
			TagInfo tagInfo = InfoForTag[lcTagName] as TagInfo;
			if (tagInfo == null						// Unknown tag
				|| (!SupportNoScriptTables 
					&& tagInfo.IsTableRelated)) 	// or tag not supported in this configuration
				return "<" + m.Groups[4].Value + "NeatHtmlReplace_" + m.Groups[6].Value + attrs + m.Groups[10];

			if (!tagInfo.IsTableRelated) // Not a table-related tag
				return "<" + m.Groups[4].Value + m.Groups[6].Value + attrs + m.Groups[10];
			
			// Handle Table related tag
			if (!m.Groups[4].Success) // Table-related start tag
			{
				if (tagInfo.IsTable)
				{
					if (!IsTableAllowed) // table start tag not allowed here.
						return "<" + m.Groups[4].Value + "NeatHtmlReplace_" + m.Groups[6].Value + attrs + m.Groups[10];
					else
					{
						IsTableAllowed = false;
						UntrustedTables++;
						return ParserResetString + "<" + m.Groups[4].Value + m.Groups[6].Value + attrs + m.Groups[10];
					}
				}
				else if (tagInfo.IsTableCell)
				{
					IsTableAllowed = true;
					return ParserResetString + "<" + m.Groups[4].Value + m.Groups[6].Value + attrs + m.Groups[10];
				}
			}
			else // Table-related end tag
			{
				if (UntrustedTables <= 0) // suspicious table-related end tag
					return "<" + m.Groups[4].Value + "NeatHtmlReplace_" + m.Groups[6].Value + attrs + m.Groups[10];
				if (tagInfo.IsTable)
				{
					IsTableAllowed = true;
					UntrustedTables--;
					return ParserResetString + "<" + m.Groups[4].Value + m.Groups[6].Value + attrs + m.Groups[10];
				}
				else
					IsTableAllowed = false;
			}					
			return "<" + m.Groups[4].Value + m.Groups[6].Value + attrs + m.Groups[10];
		}

		private int UntrustedTables = 0;
		private bool IsTableAllowed = true;
	}
	
	internal class TagInfo
	{
		internal bool IsTableRelated = false;
		internal bool IsTable = false;
		internal bool IsTableCell = false;
		internal TagInfo(bool isTableRelated, bool isTable, bool isTableCell)
		{
			IsTableRelated = isTableRelated;
			IsTable = isTable;
			IsTableCell = isTableCell;
		}
		internal static TagInfo NOT_TABLE_RELATED = new TagInfo(false, false, false);
		internal static TagInfo TABLE = new TagInfo(true, true, false);
		internal static TagInfo TABLE_CELL = new TagInfo(true, false, true);
		internal static TagInfo TABLE_OTHER = new TagInfo(true, false, false);
	}
	
	internal class ContentTooComplexException : Exception
	{
	}
}
