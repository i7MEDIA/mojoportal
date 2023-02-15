/*

NeatHtml- Helps prevent XSS attacks by validating HTML against a subset of XHTML.
Copyright (C) 2006  Dean Brettle

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

using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace mojoPortal.Web.Framework
{
	public class XssFilter
	{
		public static XssFilter GetForSchema(string schemaLocation)
		{
			lock (FilterInfoTable.SyncRoot)
			{
				XssFilterInfo filterInfo = FilterInfoTable[schemaLocation] as XssFilterInfo;

				if (filterInfo == null)
				{
					filterInfo = new XssFilterInfo(schemaLocation);
					FilterInfoTable[schemaLocation] = filterInfo;
				}
				return new XssFilter(filterInfo);
			}
		}

		private static Hashtable FilterInfoTable = new Hashtable();

		private XssFilter(XssFilterInfo filterInfo)
		{
			FilterInfo = filterInfo;
		}

		private XssFilterInfo FilterInfo;

		public string FilterFragment(string origHtmlFragment)
		{
			origHtmlFragment = CleanupHtml(origHtmlFragment);

			// Remove duplicate ids because they are invalid but not an attack vector.
			string htmlFragment = RemoveIds(origHtmlFragment);

			// Resolve general entities to character entities so we don't have to use
			// 2 XmlValidatingReaders - one with a DTD to resolve the general entities
			// and one with the schema to validate the document.
			htmlFragment = ResolveGeneralEntities(htmlFragment);

			string page = @"<html xmlns=""" + FilterInfo.Schema.TargetNamespace + @"""><head><title>title</title></head>"
			+ "<body>"
			+ "<div>\n" + htmlFragment + "\n</div>"
			+ "</body>"
			+ "</html>";

			XmlTextReader reader = new XmlTextReader(new StringReader(page));

			try
			{
				XmlValidatingReader validator = new System.Xml.XmlValidatingReader(reader);
				validator.ValidationEventHandler += new ValidationEventHandler(OnValidationError);
				validator.Schemas.Add(FilterInfo.Schema);
				validator.ValidationType = ValidationType.Schema;
				validator.EntityHandling = EntityHandling.ExpandCharEntities;

				while (validator.Read()) {}
			}
			finally
			{
				reader.Close();
			}

			if (FilterInfo.UriAndStyleValidator != null)
			{
				reader = new XmlTextReader(new StringReader(page));

				try
				{
					XmlDocument doc = new XmlDocument
					{
						PreserveWhitespace = true,
						XmlResolver = null
					};
					doc.Load(reader);
					FilterInfo.UriAndStyleValidator.Validate(doc);
				}
				finally
				{
					reader.Close();
				}
			}

			return origHtmlFragment;
		}

		// Replace <br> with <br/>
		// Force all tag names to lowercase.
		// Replace ampersands with "&amp;" if they are not followed by either:
		// a. alphanumerics and a semi
		// b. "#" and decimal digits and a semi
		// c. "#x" or "#X" and hex digits and a semi
		private static readonly Regex CleanupHtmlRegex = new Regex(@"(<[bB][rR](\s*[^>]*)>)|(</?[a-zA-Z]+)|(&(?!([A-Za-z0-9]+|#[0-9]+|#[xX][0-9A-Fa-f]+);))");

		private string CleanupHtml(string htmlFragment)
		{
			// Replace ampersands with "&amp;" if they are not followed by either:
			// a. alphanumerics and a semi
			// b. "#" and decimal digits and a semi
			// c. "#x" or "#X" and hex digits and a semi
			// And force all tag names to lowercase.
			return CleanupHtmlRegex.Replace(htmlFragment, new MatchEvaluator(FixMatch));
		}

		private string FixMatch(Match m)
		{
			if (m.Groups[1].Success && m.Groups[2].Success)
			{
				string beforeCloseBracket = m.Groups[2].Value;

				if (!beforeCloseBracket.EndsWith("/"))
					beforeCloseBracket += "/";

				return "<br" + beforeCloseBracket + ">";
			}
			else if (m.Groups[3].Success)
			{
				return m.Groups[3].Value.ToLower();
			}
			else if (m.Groups[4].Success)
			{
				return "&amp;";
			}
			else
			{
				return null;
			}
		}

		private static readonly Regex IdAttributeRegex = new Regex(@"(?<before>[<][a-zA-Z]+(\s+([^iI][a-zA-Z]*|[iI][^dD][a-zA-Z]*|[iI][dD][a-zA-Z]+)=('[^']*'|""[^""]*""|[^\s>]*))*)(\s+(id|ID)=('[a-zA-Z0-9_][a-zA-Z0-9_.-]*'|""[a-zA-Z0-9_][a-zA-Z0-9_.-]*""|[a-zA-Z0-9_][a-zA-Z0-9_.-]*))(?<after>\s|>)");

		private string RemoveIds(string htmlFragment)
		{
			return IdAttributeRegex.Replace(htmlFragment, "${before}${after}");
		}

		// Replace general entities with character entities.
		private static readonly Regex GeneralEntityRegex = new Regex("&([A-Za-z0-9]+);");

		private string ResolveGeneralEntities(string htmlFragment)
		{
			return GeneralEntityRegex.Replace(htmlFragment, new MatchEvaluator(ResolveEntity));
		}

		private string ResolveEntity(Match m)
		{
			if (m.Groups[1].Success)
			{
				string charEnt = CharEntityFor[m.Groups[1].Value.ToLower()] as string;

				if (charEnt != null)
				{
					return charEnt;
				}
			}
			return m.Value;
		}

		private void OnValidationError(object sender, ValidationEventArgs args)
		{
			if (args.Exception != null)
			{
				throw args.Exception;
			}
			else
			{
				throw new XmlException("Validation error: " + args.Message);
			}
		}

		private static Hashtable CharEntityFor = new Hashtable();

		static XssFilter()
		{
			CharEntityFor["nbsp"] = "&#160;"; /* no-break space = non-breaking space, U+00A0 ISOnum */
			CharEntityFor["iexcl"] = "&#161;"; /* inverted exclamation mark, U+00A1 ISOnum */
			CharEntityFor["cent"] = "&#162;"; /* cent sign, U+00A2 ISOnum */
			CharEntityFor["pound"] = "&#163;"; /* pound sign, U+00A3 ISOnum */
			CharEntityFor["curren"] = "&#164;"; /* currency sign, U+00A4 ISOnum */
			CharEntityFor["yen"] = "&#165;"; /* yen sign = yuan sign, U+00A5 ISOnum */
			CharEntityFor["brvbar"] = "&#166;"; /* broken bar = broken vertical bar, U+00A6 ISOnum */
			CharEntityFor["sect"] = "&#167;"; /* section sign, U+00A7 ISOnum */
			CharEntityFor["uml"] = "&#168;"; /* diaeresis = spacing diaeresis, U+00A8 ISOdia */
			CharEntityFor["copy"] = "&#169;"; /* copyright sign, U+00A9 ISOnum */
			CharEntityFor["ordf"] = "&#170;"; /* feminine ordinal indicator, U+00AA ISOnum */
			CharEntityFor["laquo"] = "&#171;"; /* left-pointing double angle quotation mark = left pointing guillemet, U+00AB ISOnum */
			CharEntityFor["not"] = "&#172;"; /* not sign = angled dash, U+00AC ISOnum */
			CharEntityFor["shy"] = "&#173;"; /* soft hyphen = discretionary hyphen, U+00AD ISOnum */
			CharEntityFor["reg"] = "&#174;"; /* registered sign = registered trade mark sign, U+00AE ISOnum */
			CharEntityFor["macr"] = "&#175;"; /* macron = spacing macron = overline = APL overbar, U+00AF ISOdia */
			CharEntityFor["deg"] = "&#176;"; /* degree sign, U+00B0 ISOnum */
			CharEntityFor["plusmn"] = "&#177;"; /* plus-minus sign = plus-or-minus sign, U+00B1 ISOnum */
			CharEntityFor["sup2"] = "&#178;"; /* superscript two = superscript digit two = squared, U+00B2 ISOnum */
			CharEntityFor["sup3"] = "&#179;"; /* superscript three = superscript digit three = cubed, U+00B3 ISOnum */
			CharEntityFor["acute"] = "&#180;"; /* acute accent = spacing acute, U+00B4 ISOdia */
			CharEntityFor["micro"] = "&#181;"; /* micro sign, U+00B5 ISOnum */
			CharEntityFor["para"] = "&#182;"; /* pilcrow sign = paragraph sign, U+00B6 ISOnum */
			CharEntityFor["middot"] = "&#183;"; /* middle dot = Georgian comma = Greek middle dot, U+00B7 ISOnum */
			CharEntityFor["cedil"] = "&#184;"; /* cedilla = spacing cedilla, U+00B8 ISOdia */
			CharEntityFor["sup1"] = "&#185;"; /* superscript one = superscript digit one, U+00B9 ISOnum */
			CharEntityFor["ordm"] = "&#186;"; /* masculine ordinal indicator, U+00BA ISOnum */
			CharEntityFor["raquo"] = "&#187;"; /* right-pointing double angle quotation mark = right pointing guillemet, U+00BB ISOnum */
			CharEntityFor["frac14"] = "&#188;"; /* vulgar fraction one quarter = fraction one quarter, U+00BC ISOnum */
			CharEntityFor["frac12"] = "&#189;"; /* vulgar fraction one half = fraction one half, U+00BD ISOnum */
			CharEntityFor["frac34"] = "&#190;"; /* vulgar fraction three quarters = fraction three quarters, U+00BE ISOnum */
			CharEntityFor["iquest"] = "&#191;"; /* inverted question mark = turned question mark, U+00BF ISOnum */
			CharEntityFor["Agrave"] = "&#192;"; /* latin capital letter A with grave = latin capital letter A grave, U+00C0 ISOlat1 */
			CharEntityFor["Aacute"] = "&#193;"; /* latin capital letter A with acute, U+00C1 ISOlat1 */
			CharEntityFor["Acirc"] = "&#194;"; /* latin capital letter A with circumflex, U+00C2 ISOlat1 */
			CharEntityFor["Atilde"] = "&#195;"; /* latin capital letter A with tilde, U+00C3 ISOlat1 */
			CharEntityFor["Auml"] = "&#196;"; /* latin capital letter A with diaeresis, U+00C4 ISOlat1 */
			CharEntityFor["Aring"] = "&#197;"; /* latin capital letter A with ring above = latin capital letter A ring, U+00C5 ISOlat1 */
			CharEntityFor["AElig"] = "&#198;"; /* latin capital letter AE = latin capital ligature AE, U+00C6 ISOlat1 */
			CharEntityFor["Ccedil"] = "&#199;"; /* latin capital letter C with cedilla, U+00C7 ISOlat1 */
			CharEntityFor["Egrave"] = "&#200;"; /* latin capital letter E with grave, U+00C8 ISOlat1 */
			CharEntityFor["Eacute"] = "&#201;"; /* latin capital letter E with acute, U+00C9 ISOlat1 */
			CharEntityFor["Ecirc"] = "&#202;"; /* latin capital letter E with circumflex, U+00CA ISOlat1 */
			CharEntityFor["Euml"] = "&#203;"; /* latin capital letter E with diaeresis, U+00CB ISOlat1 */
			CharEntityFor["Igrave"] = "&#204;"; /* latin capital letter I with grave, U+00CC ISOlat1 */
			CharEntityFor["Iacute"] = "&#205;"; /* latin capital letter I with acute, U+00CD ISOlat1 */
			CharEntityFor["Icirc"] = "&#206;"; /* latin capital letter I with circumflex, U+00CE ISOlat1 */
			CharEntityFor["Iuml"] = "&#207;"; /* latin capital letter I with diaeresis, U+00CF ISOlat1 */
			CharEntityFor["ETH"] = "&#208;"; /* latin capital letter ETH, U+00D0 ISOlat1 */
			CharEntityFor["Ntilde"] = "&#209;"; /* latin capital letter N with tilde, U+00D1 ISOlat1 */
			CharEntityFor["Ograve"] = "&#210;"; /* latin capital letter O with grave, U+00D2 ISOlat1 */
			CharEntityFor["Oacute"] = "&#211;"; /* latin capital letter O with acute, U+00D3 ISOlat1 */
			CharEntityFor["Ocirc"] = "&#212;"; /* latin capital letter O with circumflex, U+00D4 ISOlat1 */
			CharEntityFor["Otilde"] = "&#213;"; /* latin capital letter O with tilde, U+00D5 ISOlat1 */
			CharEntityFor["Ouml"] = "&#214;"; /* latin capital letter O with diaeresis, U+00D6 ISOlat1 */
			CharEntityFor["times"] = "&#215;"; /* multiplication sign, U+00D7 ISOnum */
			CharEntityFor["Oslash"] = "&#216;"; /* latin capital letter O with stroke = latin capital letter O slash, U+00D8 ISOlat1 */
			CharEntityFor["Ugrave"] = "&#217;"; /* latin capital letter U with grave, U+00D9 ISOlat1 */
			CharEntityFor["Uacute"] = "&#218;"; /* latin capital letter U with acute, U+00DA ISOlat1 */
			CharEntityFor["Ucirc"] = "&#219;"; /* latin capital letter U with circumflex, U+00DB ISOlat1 */
			CharEntityFor["Uuml"] = "&#220;"; /* latin capital letter U with diaeresis, U+00DC ISOlat1 */
			CharEntityFor["Yacute"] = "&#221;"; /* latin capital letter Y with acute, U+00DD ISOlat1 */
			CharEntityFor["THORN"] = "&#222;"; /* latin capital letter THORN, U+00DE ISOlat1 */
			CharEntityFor["szlig"] = "&#223;"; /* latin small letter sharp s = ess-zed, U+00DF ISOlat1 */
			CharEntityFor["agrave"] = "&#224;"; /* latin small letter a with grave = latin small letter a grave, U+00E0 ISOlat1 */
			CharEntityFor["aacute"] = "&#225;"; /* latin small letter a with acute, U+00E1 ISOlat1 */
			CharEntityFor["acirc"] = "&#226;"; /* latin small letter a with circumflex, U+00E2 ISOlat1 */
			CharEntityFor["atilde"] = "&#227;"; /* latin small letter a with tilde, U+00E3 ISOlat1 */
			CharEntityFor["auml"] = "&#228;"; /* latin small letter a with diaeresis, U+00E4 ISOlat1 */
			CharEntityFor["aring"] = "&#229;"; /* latin small letter a with ring above = latin small letter a ring, U+00E5 ISOlat1 */
			CharEntityFor["aelig"] = "&#230;"; /* latin small letter ae = latin small ligature ae, U+00E6 ISOlat1 */
			CharEntityFor["ccedil"] = "&#231;"; /* latin small letter c with cedilla, U+00E7 ISOlat1 */
			CharEntityFor["egrave"] = "&#232;"; /* latin small letter e with grave, U+00E8 ISOlat1 */
			CharEntityFor["eacute"] = "&#233;"; /* latin small letter e with acute, U+00E9 ISOlat1 */
			CharEntityFor["ecirc"] = "&#234;"; /* latin small letter e with circumflex, U+00EA ISOlat1 */
			CharEntityFor["euml"] = "&#235;"; /* latin small letter e with diaeresis, U+00EB ISOlat1 */
			CharEntityFor["igrave"] = "&#236;"; /* latin small letter i with grave, U+00EC ISOlat1 */
			CharEntityFor["iacute"] = "&#237;"; /* latin small letter i with acute, U+00ED ISOlat1 */
			CharEntityFor["icirc"] = "&#238;"; /* latin small letter i with circumflex, U+00EE ISOlat1 */
			CharEntityFor["iuml"] = "&#239;"; /* latin small letter i with diaeresis, U+00EF ISOlat1 */
			CharEntityFor["eth"] = "&#240;"; /* latin small letter eth, U+00F0 ISOlat1 */
			CharEntityFor["ntilde"] = "&#241;"; /* latin small letter n with tilde, U+00F1 ISOlat1 */
			CharEntityFor["ograve"] = "&#242;"; /* latin small letter o with grave, U+00F2 ISOlat1 */
			CharEntityFor["oacute"] = "&#243;"; /* latin small letter o with acute, U+00F3 ISOlat1 */
			CharEntityFor["ocirc"] = "&#244;"; /* latin small letter o with circumflex, U+00F4 ISOlat1 */
			CharEntityFor["otilde"] = "&#245;"; /* latin small letter o with tilde, U+00F5 ISOlat1 */
			CharEntityFor["ouml"] = "&#246;"; /* latin small letter o with diaeresis, U+00F6 ISOlat1 */
			CharEntityFor["divide"] = "&#247;"; /* division sign, U+00F7 ISOnum */
			CharEntityFor["oslash"] = "&#248;"; /* latin small letter o with stroke, = latin small letter o slash, U+00F8 ISOlat1 */
			CharEntityFor["ugrave"] = "&#249;"; /* latin small letter u with grave, U+00F9 ISOlat1 */
			CharEntityFor["uacute"] = "&#250;"; /* latin small letter u with acute, U+00FA ISOlat1 */
			CharEntityFor["ucirc"] = "&#251;"; /* latin small letter u with circumflex, U+00FB ISOlat1 */
			CharEntityFor["uuml"] = "&#252;"; /* latin small letter u with diaeresis, U+00FC ISOlat1 */
			CharEntityFor["yacute"] = "&#253;"; /* latin small letter y with acute, U+00FD ISOlat1 */
			CharEntityFor["thorn"] = "&#254;"; /* latin small letter thorn, U+00FE ISOlat1 */
			CharEntityFor["yuml"] = "&#255;"; /* latin small letter y with diaeresis, U+00FF ISOlat1 */
			/* Special characters for XHTML */


			/* Portions (C) International Organization for Standardization 1986:
				 Permission to copy in any form is granted for use with
				 conforming SGML systems and applications as defined in
				 ISO 8879, provided this notice is included in all copies.
			*/

			/* Relevant ISO entity set is given unless names are newly introduced.
				 New names (i.e., not in ISO 8879 list) do not clash with any
				 existing ISO 8879 entity names. ISO 10646 character numbers
				 are given for each character, in hex. values are decimal
				 conversions of the ISO 10646 values and refer to the document
				 character set. Names are Unicode names. 
			*/

			/* C0 Controls and Basic Latin */
			CharEntityFor["quot"] = "&#34;"; /*  quotation mark, U+0022 ISOnum */
			CharEntityFor["amp"] = "&#38;#38;"; /*  ampersand, U+0026 ISOnum */
			CharEntityFor["lt"] = "&#38;#60;"; /*  less-than sign, U+003C ISOnum */
			CharEntityFor["gt"] = "&#62;"; /*  greater-than sign, U+003E ISOnum */
			CharEntityFor["apos	"] = "&#39;"; /*  apostrophe = APL quote, U+0027 ISOnum */

			/* Latin Extended-A */
			CharEntityFor["OElig"] = "&#338;"; /*  latin capital ligature OE, U+0152 ISOlat2 */
			CharEntityFor["oelig"] = "&#339;"; /*  latin small ligature oe, U+0153 ISOlat2 */
											   /* ligature is a misnomer, this is a separate character in some languages */
			CharEntityFor["Scaron"] = "&#352;"; /*  latin capital letter S with caron, U+0160 ISOlat2 */
			CharEntityFor["scaron"] = "&#353;"; /*  latin small letter s with caron, U+0161 ISOlat2 */
			CharEntityFor["Yuml"] = "&#376;"; /*  latin capital letter Y with diaeresis, U+0178 ISOlat2 */

			/* Spacing Modifier Letters */
			CharEntityFor["circ"] = "&#710;"; /*  modifier letter circumflex accent, U+02C6 ISOpub */
			CharEntityFor["tilde"] = "&#732;"; /*  small tilde, U+02DC ISOdia */

			/* General Punctuation */
			CharEntityFor["ensp"] = "&#8194;"; /* en space, U+2002 ISOpub */
			CharEntityFor["emsp"] = "&#8195;"; /* em space, U+2003 ISOpub */
			CharEntityFor["thinsp"] = "&#8201;"; /* thin space, U+2009 ISOpub */
			CharEntityFor["zwnj"] = "&#8204;"; /* zero width non-joiner, U+200C NEW RFC 2070 */
			CharEntityFor["zwj"] = "&#8205;"; /* zero width joiner, U+200D NEW RFC 2070 */
			CharEntityFor["lrm"] = "&#8206;"; /* left-to-right mark, U+200E NEW RFC 2070 */
			CharEntityFor["rlm"] = "&#8207;"; /* right-to-left mark, U+200F NEW RFC 2070 */
			CharEntityFor["ndash"] = "&#8211;"; /* en dash, U+2013 ISOpub */
			CharEntityFor["mdash"] = "&#8212;"; /* em dash, U+2014 ISOpub */
			CharEntityFor["lsquo"] = "&#8216;"; /* left single quotation mark, U+2018 ISOnum */
			CharEntityFor["rsquo"] = "&#8217;"; /* right single quotation mark, U+2019 ISOnum */
			CharEntityFor["sbquo"] = "&#8218;"; /* single low-9 quotation mark, U+201A NEW */
			CharEntityFor["ldquo"] = "&#8220;"; /* left double quotation mark, U+201C ISOnum */
			CharEntityFor["rdquo"] = "&#8221;"; /* right double quotation mark, U+201D ISOnum */
			CharEntityFor["bdquo"] = "&#8222;"; /* double low-9 quotation mark, U+201E NEW */
			CharEntityFor["dagger"] = "&#8224;"; /* dagger, U+2020 ISOpub */
			CharEntityFor["Dagger"] = "&#8225;"; /* double dagger, U+2021 ISOpub */
			CharEntityFor["permil"] = "&#8240;"; /* per mille sign, U+2030 ISOtech */
			CharEntityFor["lsaquo"] = "&#8249;"; /* single left-pointing angle quotation mark, U+2039 ISO proposed */
												 /* lsaquo is proposed but not yet ISO standardized */
			CharEntityFor["rsaquo"] = "&#8250;"; /* single right-pointing angle quotation mark, U+203A ISO proposed */
												 /* rsaquo is proposed but not yet ISO standardized */

			/* Currency Symbols */
			CharEntityFor["euro"] = "&#8364;"; /*  euro sign, U+20AC NEW */
											   /* Mathematical, Greek and Symbolic characters for XHTML */

			/* Portions (C) International Organization for Standardization 1986:
				 Permission to copy in any form is granted for use with
				 conforming SGML systems and applications as defined in
				 ISO 8879, provided this notice is included in all copies.
			*/

			/* Relevant ISO entity set is given unless names are newly introduced.
				 New names (i.e., not in ISO 8879 list) do not clash with any
				 existing ISO 8879 entity names. ISO 10646 character numbers
				 are given for each character, in hex. values are decimal
				 conversions of the ISO 10646 values and refer to the document
				 character set. Names are Unicode names. 
			*/

			/* Latin Extended-B */
			CharEntityFor["fnof"] = "&#402;"; /* latin small letter f with hook = function = florin, U+0192 ISOtech */

			/* Greek */
			CharEntityFor["Alpha"] = "&#913;"; /* greek capital letter alpha, U+0391 */
			CharEntityFor["Beta"] = "&#914;"; /* greek capital letter beta, U+0392 */
			CharEntityFor["Gamma"] = "&#915;"; /* greek capital letter gamma, U+0393 ISOgrk3 */
			CharEntityFor["Delta"] = "&#916;"; /* greek capital letter delta, U+0394 ISOgrk3 */
			CharEntityFor["Epsilon"] = "&#917;"; /* greek capital letter epsilon, U+0395 */
			CharEntityFor["Zeta"] = "&#918;"; /* greek capital letter zeta, U+0396 */
			CharEntityFor["Eta"] = "&#919;"; /* greek capital letter eta, U+0397 */
			CharEntityFor["Theta"] = "&#920;"; /* greek capital letter theta, U+0398 ISOgrk3 */
			CharEntityFor["Iota"] = "&#921;"; /* greek capital letter iota, U+0399 */
			CharEntityFor["Kappa"] = "&#922;"; /* greek capital letter kappa, U+039A */
			CharEntityFor["Lambda"] = "&#923;"; /* greek capital letter lamda, U+039B ISOgrk3 */
			CharEntityFor["Mu"] = "&#924;"; /* greek capital letter mu, U+039C */
			CharEntityFor["Nu"] = "&#925;"; /* greek capital letter nu, U+039D */
			CharEntityFor["Xi"] = "&#926;"; /* greek capital letter xi, U+039E ISOgrk3 */
			CharEntityFor["Omicron"] = "&#927;"; /* greek capital letter omicron, U+039F */
			CharEntityFor["Pi"] = "&#928;"; /* greek capital letter pi, U+03A0 ISOgrk3 */
			CharEntityFor["Rho"] = "&#929;"; /* greek capital letter rho, U+03A1 */
											 /* there is no Sigmaf, and no U+03A2 character either */
			CharEntityFor["Sigma"] = "&#931;"; /* greek capital letter sigma, U+03A3 ISOgrk3 */
			CharEntityFor["Tau"] = "&#932;"; /* greek capital letter tau, U+03A4 */
			CharEntityFor["Upsilon"] = "&#933;"; /* greek capital letter upsilon, U+03A5 ISOgrk3 */
			CharEntityFor["Phi"] = "&#934;"; /* greek capital letter phi, U+03A6 ISOgrk3 */
			CharEntityFor["Chi"] = "&#935;"; /* greek capital letter chi, U+03A7 */
			CharEntityFor["Psi"] = "&#936;"; /* greek capital letter psi, U+03A8 ISOgrk3 */
			CharEntityFor["Omega"] = "&#937;"; /* greek capital letter omega, U+03A9 ISOgrk3 */

			CharEntityFor["alpha"] = "&#945;"; /* greek small letter alpha, U+03B1 ISOgrk3 */
			CharEntityFor["beta"] = "&#946;"; /* greek small letter beta, U+03B2 ISOgrk3 */
			CharEntityFor["gamma"] = "&#947;"; /* greek small letter gamma, U+03B3 ISOgrk3 */
			CharEntityFor["delta"] = "&#948;"; /* greek small letter delta, U+03B4 ISOgrk3 */
			CharEntityFor["epsilon"] = "&#949;"; /* greek small letter epsilon, U+03B5 ISOgrk3 */
			CharEntityFor["zeta"] = "&#950;"; /* greek small letter zeta, U+03B6 ISOgrk3 */
			CharEntityFor["eta"] = "&#951;"; /* greek small letter eta, U+03B7 ISOgrk3 */
			CharEntityFor["theta"] = "&#952;"; /* greek small letter theta, U+03B8 ISOgrk3 */
			CharEntityFor["iota"] = "&#953;"; /* greek small letter iota, U+03B9 ISOgrk3 */
			CharEntityFor["kappa"] = "&#954;"; /* greek small letter kappa, U+03BA ISOgrk3 */
			CharEntityFor["lambda"] = "&#955;"; /* greek small letter lamda, U+03BB ISOgrk3 */
			CharEntityFor["mu"] = "&#956;"; /* greek small letter mu, U+03BC ISOgrk3 */
			CharEntityFor["nu"] = "&#957;"; /* greek small letter nu, U+03BD ISOgrk3 */
			CharEntityFor["xi"] = "&#958;"; /* greek small letter xi, U+03BE ISOgrk3 */
			CharEntityFor["omicron"] = "&#959;"; /* greek small letter omicron, U+03BF NEW */
			CharEntityFor["pi"] = "&#960;"; /* greek small letter pi, U+03C0 ISOgrk3 */
			CharEntityFor["rho"] = "&#961;"; /* greek small letter rho, U+03C1 ISOgrk3 */
			CharEntityFor["sigmaf"] = "&#962;"; /* greek small letter final sigma, U+03C2 ISOgrk3 */
			CharEntityFor["sigma"] = "&#963;"; /* greek small letter sigma, U+03C3 ISOgrk3 */
			CharEntityFor["tau"] = "&#964;"; /* greek small letter tau, U+03C4 ISOgrk3 */
			CharEntityFor["upsilon"] = "&#965;"; /* greek small letter upsilon, U+03C5 ISOgrk3 */
			CharEntityFor["phi"] = "&#966;"; /* greek small letter phi, U+03C6 ISOgrk3 */
			CharEntityFor["chi"] = "&#967;"; /* greek small letter chi, U+03C7 ISOgrk3 */
			CharEntityFor["psi"] = "&#968;"; /* greek small letter psi, U+03C8 ISOgrk3 */
			CharEntityFor["omega"] = "&#969;"; /* greek small letter omega, U+03C9 ISOgrk3 */
			CharEntityFor["thetasym"] = "&#977;"; /* greek theta symbol, U+03D1 NEW */
			CharEntityFor["upsih"] = "&#978;"; /* greek upsilon with hook symbol, U+03D2 NEW */
			CharEntityFor["piv"] = "&#982;"; /* greek pi symbol, U+03D6 ISOgrk3 */

			/* General Punctuation */
			CharEntityFor["bull"] = "&#8226;"; /* bullet = black small circle, U+2022 ISOpub  */
											   /* bullet is NOT the same as bullet operator, U+2219 */
			CharEntityFor["hellip"] = "&#8230;"; /* horizontal ellipsis = three dot leader, U+2026 ISOpub  */
			CharEntityFor["prime"] = "&#8242;"; /* prime = minutes = feet, U+2032 ISOtech */
			CharEntityFor["Prime"] = "&#8243;"; /* double prime = seconds = inches, U+2033 ISOtech */
			CharEntityFor["oline"] = "&#8254;"; /* overline = spacing overscore, U+203E NEW */
			CharEntityFor["frasl"] = "&#8260;"; /* fraction slash, U+2044 NEW */

			/* Letterlike Symbols */
			CharEntityFor["weierp"] = "&#8472;"; /* script capital P = power set = Weierstrass p, U+2118 ISOamso */
			CharEntityFor["image"] = "&#8465;"; /* black-letter capital I = imaginary part, U+2111 ISOamso */
			CharEntityFor["real"] = "&#8476;"; /* black-letter capital R = real part symbol, U+211C ISOamso */
			CharEntityFor["trade"] = "&#8482;"; /* trade mark sign, U+2122 ISOnum */
			CharEntityFor["alefsym"] = "&#8501;"; /* alef symbol = first transfinite cardinal, U+2135 NEW */
												  /* alef symbol is NOT the same as hebrew letter alef, U+05D0 although the same glyph could be used to depict both characters */

			/* Arrows */
			CharEntityFor["larr"] = "&#8592;"; /* leftwards arrow, U+2190 ISOnum */
			CharEntityFor["uarr"] = "&#8593;"; /* upwards arrow, U+2191 ISOnum*/
			CharEntityFor["rarr"] = "&#8594;"; /* rightwards arrow, U+2192 ISOnum */
			CharEntityFor["darr"] = "&#8595;"; /* downwards arrow, U+2193 ISOnum */
			CharEntityFor["harr"] = "&#8596;"; /* left right arrow, U+2194 ISOamsa */
			CharEntityFor["crarr"] = "&#8629;"; /* downwards arrow with corner leftwards = carriage return, U+21B5 NEW */
			CharEntityFor["lArr"] = "&#8656;"; /* leftwards double arrow, U+21D0 ISOtech */
											   /*
											    * Unicode does not say that lArr is the same as the 'is implied by' arrow 
												* but also does not have any other character for that function. So lArr can 
												* be used for 'is implied by' as ISOtech suggests
												*/
			CharEntityFor["uArr"] = "&#8657;"; /* upwards double arrow, U+21D1 ISOamsa */
			CharEntityFor["rArr"] = "&#8658;"; /* rightwards double arrow, U+21D2 ISOtech */
											   /* 
											    * Unicode does not say this is the 'implies' character but does not have 
											    * another character with this function so rArr can be used for 'implies'
												* as ISOtech suggests
												*/
			CharEntityFor["dArr"] = "&#8659;"; /* downwards double arrow, U+21D3 ISOamsa */
			CharEntityFor["hArr"] = "&#8660;"; /* left right double arrow, U+21D4 ISOamsa */

			/* Mathematical Operators */
			CharEntityFor["forall"] = "&#8704;"; /* for all, U+2200 ISOtech */
			CharEntityFor["part"] = "&#8706;"; /* partial differential, U+2202 ISOtech  */
			CharEntityFor["exist"] = "&#8707;"; /* there exists, U+2203 ISOtech */
			CharEntityFor["empty"] = "&#8709;"; /* empty set = null set, U+2205 ISOamso */
			CharEntityFor["nabla"] = "&#8711;"; /* nabla = backward difference, U+2207 ISOtech */
			CharEntityFor["isin"] = "&#8712;"; /* element of, U+2208 ISOtech */
			CharEntityFor["notin"] = "&#8713;"; /* not an element of, U+2209 ISOtech */
			CharEntityFor["ni"] = "&#8715;"; /* contains as member, U+220B ISOtech */
			CharEntityFor["prod"] = "&#8719;"; /* n-ary product = product sign, U+220F ISOamsb */
											   /* prod is NOT the same character as U+03A0 'greek capital letter pi' though the same glyph might be used for both */
			CharEntityFor["sum"] = "&#8721;"; /* n-ary summation, U+2211 ISOamsb */
											  /* sum is NOT the same character as U+03A3 'greek capital letter sigma' though the same glyph might be used for both */
			CharEntityFor["minus"] = "&#8722;"; /* minus sign, U+2212 ISOtech */
			CharEntityFor["lowast"] = "&#8727;"; /* asterisk operator, U+2217 ISOtech */
			CharEntityFor["radic"] = "&#8730;"; /* square root = radical sign, U+221A ISOtech */
			CharEntityFor["prop"] = "&#8733;"; /* proportional to, U+221D ISOtech */
			CharEntityFor["infin"] = "&#8734;"; /* infinity, U+221E ISOtech */
			CharEntityFor["ang"] = "&#8736;"; /* angle, U+2220 ISOamso */
			CharEntityFor["and"] = "&#8743;"; /* logical and = wedge, U+2227 ISOtech */
			CharEntityFor["or"] = "&#8744;"; /* logical or = vee, U+2228 ISOtech */
			CharEntityFor["cap"] = "&#8745;"; /* intersection = cap, U+2229 ISOtech */
			CharEntityFor["cup"] = "&#8746;"; /* union = cup, U+222A ISOtech */
			CharEntityFor["int"] = "&#8747;"; /* integral, U+222B ISOtech */
			CharEntityFor["there4"] = "&#8756;"; /* therefore, U+2234 ISOtech */
			CharEntityFor["sim"] = "&#8764;"; /* tilde operator = varies with = similar to, U+223C ISOtech */
											  /* tilde operator is NOT the same character as the tilde, U+007E, although the same glyph might be used to represent both  */
			CharEntityFor["cong"] = "&#8773;"; /* approximately equal to, U+2245 ISOtech */
			CharEntityFor["asymp"] = "&#8776;"; /* almost equal to = asymptotic to, U+2248 ISOamsr */
			CharEntityFor["ne"] = "&#8800;"; /* not equal to, U+2260 ISOtech */
			CharEntityFor["equiv"] = "&#8801;"; /* identical to, U+2261 ISOtech */
			CharEntityFor["le"] = "&#8804;"; /* less-than or equal to, U+2264 ISOtech */
			CharEntityFor["ge"] = "&#8805;"; /* greater-than or equal to, U+2265 ISOtech */
			CharEntityFor["sub"] = "&#8834;"; /* subset of, U+2282 ISOtech */
			CharEntityFor["sup"] = "&#8835;"; /* superset of, U+2283 ISOtech */
			CharEntityFor["nsub"] = "&#8836;"; /* not a subset of, U+2284 ISOamsn */
			CharEntityFor["sube"] = "&#8838;"; /* subset of or equal to, U+2286 ISOtech */
			CharEntityFor["supe"] = "&#8839;"; /* superset of or equal to, U+2287 ISOtech */
			CharEntityFor["oplus"] = "&#8853;"; /* circled plus = direct sum, U+2295 ISOamsb */
			CharEntityFor["otimes"] = "&#8855;"; /* circled times = vector product, U+2297 ISOamsb */
			CharEntityFor["perp"] = "&#8869;"; /* up tack = orthogonal to = perpendicular, U+22A5 ISOtech */
			CharEntityFor["sdot"] = "&#8901;"; /* dot operator, U+22C5 ISOamsb */
											   /* dot operator is NOT the same character as U+00B7 middle dot */

			/* Miscellaneous Technical */
			CharEntityFor["lceil"] = "&#8968;"; /* left ceiling = APL upstile, U+2308 ISOamsc  */
			CharEntityFor["rceil"] = "&#8969;"; /* right ceiling, U+2309 ISOamsc  */
			CharEntityFor["lfloor"] = "&#8970;"; /* left floor = APL downstile, U+230A ISOamsc  */
			CharEntityFor["rfloor"] = "&#8971;"; /* right floor, U+230B ISOamsc  */
			CharEntityFor["lang"] = "&#9001;"; /* left-pointing angle bracket = bra, U+2329 ISOtech */
											   /* lang is NOT the same character as U+003C 'less than sign' or U+2039 'single left-pointing angle quotation mark' */
			CharEntityFor["rang"] = "&#9002;"; /* right-pointing angle bracket = ket, U+232A ISOtech */
											   /* rang is NOT the same character as U+003E 'greater than sign' or U+203A 'single right-pointing angle quotation mark' */

			/* Geometric Shapes */
			CharEntityFor["loz"] = "&#9674;"; /* lozenge, U+25CA ISOpub */

			/* Miscellaneous Symbols */
			CharEntityFor["spades"] = "&#9824;"; /* black spade suit, U+2660 ISOpub */
												 /* black here seems to mean filled as opposed to hollow */
			CharEntityFor["clubs"] = "&#9827;"; /* black club suit = shamrock, U+2663 ISOpub */
			CharEntityFor["hearts"] = "&#9829;"; /* black heart suit = valentine, U+2665 ISOpub */
			CharEntityFor["diams"] = "&#9830;"; /* black diamond suit, U+2666 ISOpub */
		}
	}
}
