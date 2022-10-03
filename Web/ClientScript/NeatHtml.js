/*
NeatHtml - for preventing XSS in untrusted HTML
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

/*

Simplest usage (note that comments and absence of whitespace between tags can be significant):
	
<!--[if gte IE 7]><!-->
	<div class="NeatHtml" style="overflow: hidden; position: relative; border: none; padding: 0; margin: 0;">
<!--<![endif]-->
<!--[if lt IE 7]>
	<div class="NeatHtml" style="width: NOSCRIPT_IE6_WIDTH; height: NOSCRIPT_IE6_HEIGHT; overflow: auto; position: relative; border: none; padding: 0; margin: 0;">
<![endif]-->
		<table style='border-spacing: 0;'><tr><td style='padding: 0;'><!-- test comment --><script type="text/javascript">
			try { NeatHtml.DefaultFilter.BeginUntrusted(); } catch (ex) { document.writeln('NeatHtml not found\074!-' + '-'); }</script><div>
				PREPROCESSED_UNTRUSTED_CONTENT
		</div><input name='NeatHtmlEndUntrusted' type='hidden' value="" /><script type='text/javascript'></script><!-- > --><!-- <xmp></xmp><xml></xml><! --></td></tr></table>
	<script type="text/javascript">NeatHtml.DefaultFilter.ProcessUntrusted(MAX_COMPLEXITY, TRUSTED_IMAGE_URL_REGEXP);</script>
	</div><script type='text/javascript'>NeatHtml.DefaultFilter.ResizeContainer();</script>
	
where:

	PREPROCESSED_UNTRUSTED_CONTENT has been preprocessed on the server.  See ../dotnet/Brettle.Web.NeatHtml/Filter.cs
		for a	sample implementation.
				
	NOSCRIPT_IE6_WIDTH and NOSCRIPT_IE6_HEIGHT are the desired dimensions of the div that will display the
		untrusted content in IE6 and earlier when script is disabled.  If the untrusted content is larger, 
		scrollbars will be added to the div.  These values have no effect on other browsers nor do they have
		an effect when script is enabled.
	
	MAX_COMPLEXITY is the maximum number of regular expression matches which should be done while processing the
		content.  This limits the effectiveness of DoS attacks.  This is optional.  It defaults to 10000.
		
	TRUSTED_IMAGE_URL_PATTERN is the RegExp object which an image URL must match for it
		to be displayed.  This is optional.  By default, no images are trusted.
		
To change the way various tags and attributes are handled:

	1. Create your own filter:
			var filter = new NeatHtml.Filter();
	2. Configure it by modifying or replacing the default ElemActions, AttrActions, or StyleActions members.  
		These are associative arrays which map element or attribute names to functions which
		manipulate elements/attributes/styles with those names.  The following functions are predefined:
			NeatHtml.Filter.prototype.AllowElem - Allows the element and filters it's content.
			NeatHtml.Filter.prototype.RemoveElem - Removes the element and all of it's content.
			NeatHtml.Filter.prototype.AllowAttr - Allows the attribute unchanged.
			NeatHtml.Filter.prototype.RemoveAttr - Removes the attribute.
			NeatHtml.Filter.prototype.AddPrefixToValue - Prepends filter.IdPrefix to the attribute value.  Used for
							ID attributes to prevent the untrusted content from stealing element IDs. 
			NeatHtml.Filter.prototype.HandleUrl - Removes the attribute if the value doesn't start with one of
							"http:", "https", "ftp:", "mailto:", or "#".  If it starts with "#", it adds filter.IdPrefix
							after the "#", so that the untrusted content can link to it's own anchors
			NeatHtml.Filter.prototype.HandleStyle - Handles styles according to StyleActions.
			NeatHtml.Filter.prototype.AllowStyle - Allows the style unchanged.
			NeatHtml.Filter.prototype.RemoveStyle - Removes the style.
		For example, you could just allow basic text formatting by doing:
			filter.ElemActions = { 
				b: filter.AllowElem,
				i: filter.AllowElem,
				u: filter.AllowElem,
				span: filter.AllowElem 
			};
			filter.AttrActions = { style: filter.HandleStyle };
			filter.StyleActions = { 
				'font-family': filter.AllowStyle,
				'font-size': filter.AllowStyle,
				'font-style': filter.AllowStyle,
				'font-variant': filter.AllowStyle,
				'font-weight': filter.AllowStyle,
				'font': filter.AllowStyle
			};
	3. Change "NeatHtml.DefaultFilter" to "filter" in both places in the simple usage example above.
*/


NeatHtml = {};

NeatHtml.ContentTooComplexException = "The content that belongs here is too complex to display securely.";

NeatHtml.Filter = function(elemActions, attrActions, styleActions, entityCharCodeMap)
{
	var my = this;
	
	this.MaxComplexity = 1000;
	this.Complexity = 0;
	this.ElemActions = elemActions || GetDefaultElemActions();
	this.AttrActions = attrActions || GetDefaultAttrActions();
	this.StyleActions = styleActions || GetDefaultStyleActions();
	this.EntityCharCodeMap = entityCharCodeMap || GetDefaultEntityCharCodeMap();
	this.IdPrefix = "NeatHtml_";
	
	this.StyleDeclRe = /(-?[_a-z][_a-z0-9-]*)[ \t\r\n\f]*:[ \t\r\n\f]*((([-+]?([0-9]+|[0-9]*\.[0-9]+)(%|[_a-z][_a-z0-9-]*)?|[_a-z][_a-z0-9-]*|"[^\n\r\f"]*"|'[^\n\r\f']*'|#([0-9a-f]{3}|[0-9a-f]{6})|rgb\( *[0-9]*%? *, *[0-9]*%? *, *[0-9]*%? *\)))([ \t\r\n\f]+[,\/]?([-+]?([0-9]+|[0-9]*\.[0-9]+)(%|[_a-z][_a-z0-9-]*)?|[_a-z][_a-z0-9-]*|"[^\n\r\f"]*"|'[^\n\r\f']*'|#([0-9a-f]{3}|[0-9a-f]{6})|rgb\( *[0-9]*%? *, *[0-9]*%? *, *[0-9]*%? *\)))*[ \t\r\n\f]*)(;|$)/igm;    // "

	function GetDefaultElemActions()
	{
		// These tags and their content is allowed.
		var allowedTags 
			= [ "a", "abbr", "acronym", "address", "b", "basefont", "bdo", "big", "blockquote", "br",
		 		"caption", "center", "cite", "code", "col", "colgroup", "dd", "del", "dfn", "dir", "div", "dl", "dt", 
		 		"em", "font", "h1", "h2", "h3", "h4", "h5", "h6", "hr", "i", "ins", "kbd", "li", "ol", "p", "pre", "q",
		 		"s", "samp", "small", "span", "strike", "strong", "sub", "sup", "table", "tbody", "td", "tfoot", "th",
		 		"thead", "tr", "tt", "u", "ul", "var" ];

		// These tags are removed along with their content
		var prohibitedTags = [ "script", "style" ];

		var elemActions = {};
		for (var i = 0; i < allowedTags.length ; i++)
		{
			var t = allowedTags[i];
			elemActions[t] = my.AllowElem; 
		}
	
		for (var i = 0; i < prohibitedTags.length ; i++)
		{
			var t = prohibitedTags[i];
			elemActions[t] = my.RemoveElem; 
		}
		
		elemActions["img"] = my.HandleImg;
		
		return elemActions;
	}
	
	function GetDefaultAttrActions()
	{
		// These attributes are allowed
		var allowedAttrs 
			= [ "abbr", "accept", "accesskey", "align", "alt", "axis", "bgcolor", "border", "cellpadding",
				"cellspacing", "cite", "class", "classid", "clear", "code", "color", "cols", "colspan", "compact", 
				"datetime", "dir", "disabled", "face", "frame", "frameborder", "headers", "height", "hreflang", "id", 
				"label", "lang", "language", "longdesc", "marginheight", "marginwidth", "name", "noshade", "nowrap", 
				"readonly", "rel", "rev", "rows", "rowspan", "rules", "scope", "size", "span", "start", "summary", 
				"tabindex", "title", "type", "value", "width", "xml:lang", "xml:space" ];

		var attrActions = {};
		for (var i = 0; i < allowedAttrs.length ; i++)
		{
			var a = allowedAttrs[i];
			attrActions[a] = my.AllowAttr; 
		}
	
		attrActions["href"] = my.HandleUrl; 
		attrActions["src"] = my.HandleSrc; 
		attrActions["style"] = my.HandleStyle;
		attrActions["id"] = my.AddPrefixToValue;
		
		return attrActions;
	}

	function GetDefaultStyleActions()
	{
		// These attributes are allowed
		var allowedProps
			= ['azimuth','background-attachment','background-color','background-image','background-position',
				'background-repeat','background','border-collapse','border-color','border-spacing','border-style',
				'border-top','border-right','border-bottom','border-left','border-top-color','border-right-color',
				'border-bottom-color','border-left-color','border-top-style','border-right-style','border-bottom-style',
				'border-left-style','border-top-width','border-right-width','border-bottom-width','border-left-width',
				'border-width','border','bottom','caption-side','clear','clip','color','content',
				/* 'counter-increment','counter-reset', */ // Don't allow messing with the counters 
				'cue-after','cue-before','cue','cursor','direction','display','elevation','empty-cells',
				'float','font-family','font-size','font-style','font-variant','font-weight','font','height','left',
				'letter-spacing','line-height','list-style-image','list-style-position','list-style-type','list-style',
				'margin-right','margin-left','margin-top','margin-bottom','margin','max-height','max-width','min-height',
				'min-width','orphans','outline-color','outline-style','outline-width','outline','overflow','padding-top',
				'padding-right','padding-bottom','padding-left','padding','page-break-after','page-break-before',
				'page-break-inside','pause-after','pause-before','pause','pitch-range','pitch','play-during','position',
				'quotes','richness','right','speak-header','speak-numeral','speak-punctuation','speak','speech-rate',
				'stress','table-layout','text-align','text-decoration','text-indent','text-transform','top',
				'unicode-bidi','vertical-align','visibility','voice-family','volume','white-space','widows','width',
				'word-spacing','z-index'
			];

		var styleActions = {};
		for (var i = 0; i < allowedProps.length ; i++)
		{
			var p = allowedProps[i];
			styleActions[p] = my.AllowStyle; 
		}
		return styleActions;
	}
	
	function GetDefaultEntityCharCodeMap()
	{
		return { 
			quot:34,amp:38,lt:60,gt:62,apos:39,
			nbsp:160,iexcl:161,cent:162,pound:163,curren:164,yen:165,brvbar:166,
			sect:167,uml:168,copy:169,ordf:170,laquo:171,not:172,shy:173,reg:174,
			macr:175,deg:176,plusmn:177,sup2:178,sup3:179,acute:180,micro:181,
			para:182,middot:183,cedil:184,sup1:185,ordm:186,raquo:187,frac14:188,
			frac12:189,frac34:190,iquest:191,Agrave:192,Aacute:193,Acirc:194,
			Atilde:195,Auml:196,Aring:197,AElig:198,Ccedil:199,Egrave:200,Eacute:201,
			Ecirc:202,Euml:203,Igrave:204,Iacute:205,Icirc:206,Iuml:207,ETH:208,
			Ntilde:209,Ograve:210,Oacute:211,Ocirc:212,Otilde:213,Ouml:214,times:215,
			Oslash:216,Ugrave:217,Uacute:218,Ucirc:219,Uuml:220,Yacute:221,
			THORN:222,szlig:223,agrave:224,aacute:225,acirc:226,atilde:227,
			auml:228,aring:229,aelig:230,ccedil:231,egrave:232,eacute:233,
			ecirc:234,euml:235,igrave:236,iacute:237,icirc:238,iuml:239,eth:240,
			ntilde:241,ograve:242,oacute:243,ocirc:244,otilde:245,ouml:246,
			divide:247,oslash:248,ugrave:249,uacute:250,ucirc:251,uuml:252,yacute:253,
			thorn:254,yuml:255,fnof:402,Alpha:913,Beta:914,Gamma:915,Delta:916,
			Epsilon:917,Zeta:918,Eta:919,Theta:920,Iota:921,Kappa:922,Lambda:923,
			Mu:924,Nu:925,Xi:926,Omicron:927,Pi:928,Rho:929,Sigma:931,Tau:932,
			Upsilon:933,Phi:934,Chi:935,Psi:936,Omega:937,alpha:945,beta:946,
			gamma:947,delta:948,epsilon:949,zeta:950,eta:951,theta:952,iota:953,
			kappa:954,lambda:955,mu:956,nu:957,xi:958,omicron:959,pi:960,rho:961,
			sigmaf:962,sigma:963,tau:964,upsilon:965,phi:966,chi:967,psi:968,
			omega:969,thetasym:977,upsih:978,piv:982,bull:8226,hellip:8230,prime:8242,
			Prime:8243,oline:8254,frasl:8260,weierp:8472,image:8465,real:8476,
			trade:8482,alefsym:8501,larr:8592,uarr:8593,rarr:8594,darr:8595,harr:8596,
			crarr:8629,lArr:8656,uArr:8657,rArr:8658,dArr:8659,hArr:8660,
			forall:8704,part:8706,exist:8707,empty:8709,nabla:8711,isin:8712,
			notin:8713,ni:8715,prod:8719,sum:8721,minus:8722,lowast:8727,radic:8730,
			prop:8733,infin:8734,ang:8736,"and":8743,"or":8744,cap:8745,cup:8746,"int":8747,
			there4:8756,sim:8764,cong:8773,asymp:8776,ne:8800,equiv:8801,le:8804,
			ge:8805,sub:8834,sup:8835,nsub:8836,sube:8838,supe:8839,oplus:8853,otimes:8855,
			perp:8869,sdot:8901,lceil:8968,rceil:8969,lfloor:8970,rfloor:8971,
			lang:9001,rang:9002,loz:9674,spades:9824,clubs:9827,hearts:9829,diams:9830,
			OElig:338,oelig:339,Scaron:352,scaron:353,Yuml:376,circ:710,tilde:732,
			ensp:8194,emsp:8195,thinsp:8201,zwnj:8204,zwj:8205,lrm:8206,rlm:8207,
			ndash:8211,mdash:8212,lsquo:8216,rsquo:8217,sbquo:8218,ldquo:8220,rdquo:8221,
			bdquo:8222,dagger:8224,Dagger:8225,permil:8240,lsaquo:8249,rsaquo:8250,euro:8364 
		};
	}
};

NeatHtml.Filter.prototype.AllowAttr = function(tagName, attr)
{
	return true;
};
		
NeatHtml.Filter.prototype.RemoveAttr = function(tagName, attr)
{
	return false;
};
		
NeatHtml.Filter.prototype.AllowStyle = function(tagName, prop)
{
	// Do nothing
};
		
NeatHtml.Filter.prototype.RemoveStyle = function(tagName, prop)
{
	prop.Name = null;
};

NeatHtml.Filter.prototype.HandleUrl = function(tagName, attr)
{
	if (! /^(http:|https:|ftp:|mailto:|[#\.\/])/i.test(attr.value))
	{
		return false;
	}
	if (attr.value.charAt(0) == "#")
	{
		attr.value = "#" + this.IdPrefix + attr.value.substring(1, attr.value.length);
	}
	return true;
};
		
NeatHtml.Filter.prototype.HandleSrc = function(tagName, attr)
{
	if (this.TrustedImageUrlRegExp != null && tagName == "img" && this.TrustedImageUrlRegExp.test(attr.value))
	{
		return true;
	}
	return false;
};
		
NeatHtml.Filter.prototype.AddPrefixToValue = function(tagName, attr)
{
	attr.value = this.IdPrefix + attr.value;
	return true;
};

NeatHtml.Filter.prototype.HandleStyle = function(tagName, attr, filter)
{
	// Look for safe style declarations in the attribute value and replace the attribute value with just
	// those safe style declarations.
	// Safe declarations are a subset of CSS style declaration that accepts most inline styles.  
	// If the attribute contains comments, escapes outside of strings, or function calls other than rgb(), 
	// the result will not allow script to run but might not be the same as the style parsed by a standard CSS parser.
	var s = attr.value;		
	var match = null;
	var newStyle = ""
	var prop = { Name: null, Value: null };
	this.StyleDeclRe.lastIndex = 0;
	while (null != (match = this.StyleDeclRe.exec(attr.value)))
	{
		if (filter.Complexity++ > filter.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		prop.Name = match[1];
		prop.Value = match[2];
		var action = this.StyleActions[prop.Name] || this.RemoveStyle;
		action.call(this, tagName, prop);
		if (prop.Name != null)
			newStyle += prop.Name + ": " + prop.Value + "; ";
	}
	attr.value = newStyle;
	return true;
};

NeatHtml.Filter.prototype.RemoveTag = function(tagInfo)
{
	tagInfo.bOutputTags = false;
};

NeatHtml.Filter.prototype.AllowElem = function(tagInfo, filter)
{
	var my = this;

	// Parse the attributes
	var attrArray = [];
	var attrRe = /[ \t\n\r]+([A-Z:_a-z][A-Z:_a-z0-9._]*)(=("[^"]*"|'[^']*'|[^"'][^ \t\r\n]*))?/gm;    // '
	var match = null;
	attrRe.lastIndex = 0;
	while (null != (matches = attrRe.exec(tagInfo.attrs)))
	{
		if (filter.Complexity++ > filter.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		HandleAttr.apply(this, matches);
	}

	ProcessAttributes(tagInfo.tagName);
	
	var newAttrs = "";
	for (var i = 0; i < attrArray.length; i++)
	{
		newAttrs += " " + attrArray[i].name 
			+ '="' + this.HtmlEncodeAttribute(attrArray[i].value, filter) + '"';   // '
	}
	
	tagInfo.attrs = newAttrs;
	return;

	/* Local Functions */
	function HandleAttr(match, attrName, hasValue, attrValue)
	{
		if (!hasValue)
		{
			attrValue = attrName;
		}
		attrName = attrName.replace(/_NeatHtmlReplace$/g, "");

		var firstChar = attrValue.charAt(0)
		if (firstChar == '"' || firstChar == "'")
			attrValue = attrValue.substring(1, attrValue.length-1);
		attrValue = my.HtmlDecode(attrValue, filter);
		attrArray.push({name: attrName, value: attrValue});			
	}
	
	function ProcessAttributes(tagName)
	{
		for (var i = 0; i < attrArray.length;)
		{
			var attr = attrArray[i];
			var action = my.AttrActions[attr.name] || my.AttrActions[attr.name.toLowerCase()] || my.RemoveAttr;
			if (!action.call(my, tagName, attr, filter))
			{
				attrArray.splice(i, 1);
			}
			else
			{
				i++;
			}
		}
	}
};

NeatHtml.Filter.prototype.HandleImg = function(tagInfo, filter)
{
	if (this.TrustedImageUrlRegExp != null)
		return this.AllowElem(tagInfo, filter);
	return this.RemoveTag(tagInfo);	
};
		
NeatHtml.Filter.prototype.RemoveElem = function(tagInfo)
{
	tagInfo.bOutputTags = tagInfo.bOutputContent = null;
};
	
NeatHtml.Filter.prototype.BeginUntrusted = function(nhScriptId) {
	try
	{
		// Inject markup to prevent the untrusted content from being parsed as HTML.  
		// If we are able to extract content from comments, start a comment.
		// Otherwise, if scripts don't run in <xml> elements (e.g. Windows Mobile), start an <xml> element.
		// Otherwise (e.g. Safari and Konqueror), start an <xmp> element.
	
		// Find the calling script element and remember it so we can use it to find the untrusted content.
		
		//var scriptElems = document.getElementsByTagName("script");
		//var offset = 1;
		//this.BeginUntrustedScript = scriptElems[scriptElems.length - offset];
		
		// JA added 2014-11-12 this was finding the addthis script instead of the one it was looking for
		// so check if the src has length, the one we are looking for does not
		//while(this.BeginUntrustedScript.src.length > 0)
		//{
		//	offset++;
		//	this.BeginUntrustedScript = scriptElems[scriptElems.length - offset];
		//}
		
		var scriptElem = document.querySelector('[data-nhScriptId="' + nhScriptId + '"]');
		this.BeginUntrustedScript = scriptElem;
		
		// The calling script element must be preceded by an HTML comment (i.e. <!-- something -->).
		var prevSibling = this.BeginUntrustedScript.previousSibling || GetPreviousSibling(this.BeginUntrustedScript);
		if (prevSibling && prevSibling.nodeType == 8 /* Node.COMMENT_NODE */)
		{
			document.write("<!--");
		}
		else
		{
			var ua = navigator.userAgent.toLowerCase();
			if (ua.indexOf("iemobile") != -1) // Windows Mobile 6+
			{
				document.write("<xml>");
			}
			else if (ua.indexOf("webkit") != -1 || ua.indexOf("khtml") != -1 || ua.indexOf("gecko") != -1)
			{
				document.write("<xmp>");
			}
			else
			{
				document.writeln("NeatHtml error: unrecognized browser: " + navigator.userAgent);
				document.write("<!--"); // Hide the content in a comment.
				return;
			}
		}
	}
	catch (ex)
	{
		document.writeln("NeatHtml error: browser too old");
		document.write("<!--"); // Hide the content in a comment.
		return;
	}
	return;

	function GetPreviousSibling(n)
	{
		var p = n.parentNode;
		var children = p.childNodes;
		for (var i=1; i < children.length; i++)
		{
			if (children.item(i) == n)
				return children.item(i-1);
		}
		return null;
	}
};

NeatHtml.Filter.prototype.ProcessUntrusted = function(maxComplexity, trustedImageUrlRegExp) {
	var my = this;
	if (typeof(maxComplexity) == "undefined")
	{
		maxComplexity = 1000;
	}
	this.MaxComplexity = maxComplexity;
	if (typeof(trustedImageUrlRegExp) == "undefined")
	{
		trustedImageUrlRegExp = null;
	}
	this.TrustedImageUrlRegExp = trustedImageUrlRegExp;
	var containingDiv = FindNhContainingDiv(this.BeginUntrustedScript);
	var xmlStr;
	try
	{
		var untrustedContent = GetUntrustedContent();
		
		xmlStr = this.FilterFirstElement(untrustedContent);

		// Make the result available for use by tests 
		this.FilteredContent = xmlStr;

		// Replace the original untrusted content (and surrounding table)
		containingDiv.innerHTML = xmlStr;
	}
	catch (ex)
	{
		if (ex == NeatHtml.ContentTooComplexException)
			containingDiv.innerHTML = this.FilteredContent = "<div>" + ex.toString() + "</div>";
		else
			var errMsg = ex.toString();
			containingDiv.innerHTML = this.FilteredContent = "<div><pre>" + errMsg.replace(/</g, "&lt;").replace(/&/g, "&amp;") + "</pre></div>";
	}
	
	return this.FilteredContent;
	
	/***** Local Functions ******/
	
	function FindNhContainingDiv(n) 
	{
		while (n.tagName != "DIV")
		{
			n = n.parentNode;
		}
		return n;
	}

	function GetNextSibling(n)
	{
		var p = n.parentNode;
		var children = p.childNodes;
		for (var i=0; i < children.length-1; i++)
		{
			if (children.item(i) == n)
				return children.item(i+1);
		}
		return null;
	}

	function GetUntrustedContent()
	{
		// The untrusted content is in the node that immediately follows the script element that called BeginUntrusted. 
		var n = my.BeginUntrustedScript.nextSibling || GetNextSibling(my.BeginUntrustedScript);
		var s;
		if (n.nodeType == 8 /* Node.COMMENT_NODE */)
		{
			s = n.data;
		}
		else if (n.tagName == "XMP")
		{
	 		s = n.innerHTML;
	 		// Unquote the HTML special characters.
			s = my.HtmlDecode(s, my);
		}
		else if (n.tagName == "XML")
		{
	 		s = n.innerText;
		}
		var endIndex = s.indexOf("<input name='NeatHtmlEndUntrusted'");
		if (endIndex == -1)
			endIndex = s.length;
		s = s.substring(0, endIndex);
		
		// Make the result available for use by tests 
		my.UnfilteredContent = s;
		
		s = s.replace(/(<[!\?\/]?)NeatHtmlReplace_([a-z]?)/g, "$1$2");

		s = s.replace(/<NeatHtmlLt \/>&lt;/g, "<");

		s = s.replace(/&#45;&#45;/g, "--");

//		alert(s);
		return s;
	}
};

NeatHtml.Filter.prototype.Filter = function(s, destDiv)
{
	var result = this.FilterFirstElement("<div>" + s + "</div>");
	result = result.substring("<div>".length, result.length - "</div>".length);
	if (typeof(destDiv) != "undefined")
	{
		destDiv.innerHTML = '<!--[if gte IE 7]><!-->'
			+ '<div class="NeatHtml" style="overflow: hidden; position: relative; border: none; padding: 0; margin: 0;">'
			+ '<!--<![endif]-->'
			+ '<!--[if lt IE 7]>'
			+ '<div class="NeatHtml" style="overflow: auto; position: relative; border: none; padding: 0; margin: 0;">'
			+ '<![endif]-->'
			+ result
			+ '</div>';
		for (var i = 0; i < destDiv.childNodes.length; i++)
		{
			if (destDiv.childNodes.item(i).tagName == "DIV")
			{
				this.ResizeContainer(destDiv.childNodes.item(i));
				break;
			}
		}
	}
	return result;
}

NeatHtml.Filter.prototype.FilterFirstElement = function(s)
{
	var my = this;
	my.Complexity = 0;
	// According to HTML 3.2
	var endTagsForbidden = { br:1, hr:1, meta:1, col:1, isindex:1, img:1, link:1, area:1, basefont:1, param:1, input:1, base:1 };
	var endTagsOptional = { li:1, p:1, dt:1, dd:1, thead:1, tfoot:1, tbody:1, colgroup:1, tr:1, th:1, td:1, plaintext:1, option:1 };
	var openTagInfos = [];
	var matches = null;
	var filteredXml = "";
	var tagSoupRe = /(&((#[0-9]{1,10};|#x[0-9a-fA-F]{1,8};|amp;|lt;|gt;|quot;)|([A-Z:_a-z][A-Z:_a-z0-9._]{0,10};|(#X[0-9a-fA-F]{1,8};|))))|(<(\/?)(([!\?A-Z:_a-z][^ \t\n\r\/>]*)([^>]*)>|([^!\?A-Z:_a-z])))/gm; 
	tagSoupRe.lastIndex = 0;
	var searchStartIndex = 0;
	while (null != (matches = tagSoupRe.exec(s)))
	{
		if (my.Complexity++ > my.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		var text = s.substring(searchStartIndex, matches.index);
		// Text should not be the start of the output 
		if (openTagInfos.length && openTagInfos[openTagInfos.length-1].bOutputContent)
			filteredXml += text;
		
		filteredXml += HandleAmpOrOpenAngle.apply(this, matches);
		searchStartIndex = tagSoupRe.lastIndex;
	}
	
	// Close any remaining open tags
	while (openTagInfos.length)
	{
	 	var openTagInfo = openTagInfos.pop();
	 	if (openTagInfo.bOutputTags)
			filteredXml += "</" + openTagInfo.outTagName + ">";
	}
// alert(s);
	return filteredXml;

	/* Local functions */
	function HandleAmpOrOpenAngle(match, isAmp, afterAmp, isValidXmlEntityRef, charEntityRef, upperCaseHexEntityRef, isOpenAngle, isEndTag, raw, tagName, attrs, isNotEncoded) 
	{
		if (isAmp) return HandleAmpersand(match, isValidXmlEntityRef, afterAmp, charEntityRef, upperCaseHexEntityRef);
		if (isNotEncoded)	return my.HtmlEncode(match, my);
		if (isOpenAngle) return HandleOpenAngle(match, isEndTag, tagName, attrs);
	}

	function HandleAmpersand(match, isValidXmlEntityRef, afterAmp, charEntityRef, upperCaseHexEntityRef)
	{
		if (isValidXmlEntityRef)
			return match;
		if (!charEntityRef)
			return my.HtmlEncode(match, my);
		if (upperCaseHexEntityRef)
			return match.toLowerCase();
		// It is a character entity reference but it isn't supported in XML
		if (charEntityRef)
		{
			// Strip the trailing semicolon
			charEntityRef = charEntityRef.substring(0, charEntityRef.length-1);
		}
		var charCode = my.EntityCharCodeMap[charEntityRef];
		if (!charCode)
			return my.HtmlEncode(match, my);
		return "&#" + charCode + ";";
	}

	function HandleOpenAngle(match, isEndTag, tagName, attrs)
	{
		if (/^!.*$/.test(tagName))
		{
			if (/^!\[CDATA\[.*$/.test(tagName))
			{
				tagSoupRe.lastIndex = s.indexOf("]]>", matches.index + "<![CDATA[".length);
				if (tagSoupRe.lastIndex == -1) return my.HtmlEncode(match, my);
				tagSoupRe.lastIndex += "]]>".length;
				return "";
			}
			tagSoupRe.lastIndex = matches.index + "<!".length;
			while (tagSoupRe.lastIndex != -1)
			{
				var dashDashIndex = s.indexOf("--", tagSoupRe.lastIndex);
				var closeAngleIndex = s.indexOf(">", tagSoupRe.lastIndex);
				if (closeAngleIndex == -1) break;
				if (closeAngleIndex < dashDashIndex)
				{
					tagSoupRe.lastIndex = closeAngleIndex + 1;
					break;
				}
				tagSoupRe.lastIndex = dashDashIndex;
				if (tagSoupRe.lastIndex == -1) break;
				tagSoupRe.lastIndex += 2;
				tagSoupRe.lastIndex = s.indexOf("--", tagSoupRe.lastIndex);
				if (tagSoupRe.lastIndex == -1) break;
				tagSoupRe.lastIndex += 2;
			}
				
			if (tagSoupRe.lastIndex == -1) return my.HtmlEncode(match, my);
			return "";
		}
		if (/^\?.*$/.test(tagName))
		{
			tagSoupRe.lastIndex = s.indexOf("?>", matches.index + "<?".length);
			if (tagSoupRe.lastIndex == -1) return my.HtmlEncode(match, my);
			tagSoupRe.lastIndex += "?>".length;
			return "";
		}
		// If it doesn't look like a tag then it is probably an unencoded '<'.
		if (! /^[A-Z:_a-z][A-Z:_a-z0-9._]*$/.test(tagName) || isEndTag && openTagInfos.length == 0) 
			return my.HtmlEncode(match, my);
		// Otherwise it is a begin or end tag
		if (isEndTag)
			return HandleEndTag(match, tagName);
		else
		{
			return HandleBeginTag(match, tagName, attrs);
		}
	}
		
	function HandleBeginTag(match, tagName, attrs)
	{
		var lcTagName = tagName.toLowerCase();
		var output = "";
		// If this tag has an optional end tag and the current open elem has the same tag name,
		// close the one that is open
		if (endTagsOptional[lcTagName] && openTagInfos.length)
		{
			var openTagInfo = openTagInfos[openTagInfos.length-1];
			if (openTagInfo.tagName.toLowerCase() == lcTagName)
			{
				if (openTagInfo.bOutputTags)
					output = "</" + openTagInfo.outTagName + ">";
				openTagInfos.pop();
				// If there aren't any open tags left, ignore everything else
				if (openTagInfo.length == 0)
					tagSoupRe.lastIndex = s.length;
			}
		}
		
		attrs = attrs.replace(/&((#[0-9]{1,10};|#x[0-9a-fA-F]{1,8};|amp;|lt;|gt;|quot;|apos;)|([A-Z:_a-z][A-Z:_a-z0-9._]{0,10};|(#X[0-9a-fA-F]{1,8};|)))/gm,
											HandleAmpersand);

		var action = my.ElemActions[tagName] || my.ElemActions[lcTagName] || my.RemoveTag;
		var tagInfo = { };
		tagInfo.tagName = tagInfo.outTagName = tagName;
		tagInfo.attrs = attrs;
		tagInfo.bOutputTags = tagInfo.bOutputContent = true;

		action.call(my, tagInfo, my);
		if (openTagInfos.length && !openTagInfos[openTagInfos.length-1].bOutputContent)
			tagInfo.bOutputTags = tagInfo.bOutputContent = false;
		
		var newTag = "<" + tagInfo.outTagName + tagInfo.attrs;
		if (attrs.charAt(attrs.length-1) == "/" || endTagsForbidden[lcTagName])
		{
			newTag += " />";
		}
		else
		{
			newTag += ">";
			openTagInfos.push(tagInfo);
		}			
		if (tagInfo.bOutputTags)
			output += newTag;
		return output;
	}
	
	function HandleEndTag(match, tagName)
	{
		var lcTagName = tagName.toLowerCase();
		var result = null;
		var tagIndex = openTagInfos.length - 1;
		for (var closeTags = ""; tagIndex >= 0; tagIndex--)
		{
		 	var openTagInfo = openTagInfos[tagIndex];
			lcOpenTagName = openTagInfo.tagName.toLowerCase();
			if (openTagInfo.bOutputTags)
			{
				closeTags += "</" + openTagInfo.outTagName + ">";
			}
			if (lcOpenTagName == lcTagName)
			{
				result = closeTags;
				break;
			}
			// If the open tag must be explicitly closed, ignore those close tag.
			if (!endTagsOptional[lcOpenTagName])
			{
				break;
			}
			if (my.Complexity++ > my.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		}
		// If we didn't find a matching open tag, then remove the close tag
		if (result == null)
		{
			return "";
		}
		// Remove the tags from the stack of open tags
		openTagInfos.splice(tagIndex, openTagInfos.length - tagIndex);
		// If there aren't any open tags left, ignore everything else
		if (openTagInfos.length == 0)
		{
			tagSoupRe.lastIndex = s.length;
		}
		return result;
	}
};

NeatHtml.Filter.prototype.ResizeContainer = function(container, nhContainerId) {
    var my = this;
	var parent;
	if (typeof(container) != "undefined")
		parent = container;
	else
	{
	    // Find the calling script element and remember it so we can use it to find the container.
	    //var scriptElems = document.getElementsByTagName("script");
	    //parent = scriptElems[scriptElems.length - 1].previousSibling;
				
		var scriptElem = document.querySelector('[data-nhContainerId="' + nhContainerId + '"]');
		parent = scriptElem;
	}

    // For IE6 and earlier, we used conditional comments to set the overflow to "auto".  For all other browsers,
    // the overflow was set to "hidden".  We only need to resize the container in IE6 and earlier.  The 
    // resizing code below does not always work properly in at least Firefox 1.5, 2.0, and Netscape 7.2,
    // so skip it for all browsers exception IE6 and earlier.
    var overflow = parent.style.overflow;
    // The style.overflow property is empty on WebKit-based browsers (e.g. Safari 3.2.1), but is available
    // via getComputedStyle(), so use that if necessary.
    if (!overflow && document.defaultView && document.defaultView.getComputedStyle)
        overflow = document.defaultView.getComputedStyle(parent, null).getPropertyValue("overflow");
    if (overflow == "hidden")
        return;
 
    // Limit the width of the content based on the width of the containing div, and
    // set the height of the div based on the new content.  This allows IE6 to hide overflow that was
    // absolutely positioned.
    //parent.firstChild.style.width = parent.style.width;
    //parent.style.height = parent.firstChild.scrollHeight + "px";
    //parent.style.overflow = "hidden";
};

NeatHtml.Filter.prototype.HtmlEncode = function(s, filter)
{
	return s.replace(/[<>&"']/g,    // " 
						function (c) { 
		if (filter && filter.Complexity++ > filter.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		switch (c)
		{
			case '<': return "&lt;";  
			case '>': return "&gt;";  
			case '&': return "&amp;";  
			case '"': return "&quot;";
			case "'": return "&#39;";
		}
	});  
}
	
NeatHtml.Filter.prototype.HtmlEncodeAttribute = function(s, filter)
{
	s = s.replace(/[<>&"']|[^ -~]/gm,    // " 
					HtmlEncodeChar);
	// Check Regex capabilities because
	// [^ -~] does not match non-ascii in Safari 1.2/1.3 and Konqueror 3.4.
	if ((String.fromCharCode(0) + String.fromCharCode(65535)).match(/[^ -~][^ -~]/))
	{	
		return s;
	}
	else
	{
		var newS = "";
		var lastIndex = 0;
		for (var i = 0; i < s.length; i++)
		{
			if (s.charCodeAt(i) <= 31 || s.charCodeAt(i) >= 127)
			{
				newS += s.substring(lastIndex, i);
				newS += HtmlEncodeChar(s.charAt(i));
				lastIndex = i + 1;
			}
		}
		newS += s.substring(lastIndex, s.length);
		return newS;
	}
						
	function HtmlEncodeChar(c)
	{ 
		if (filter && filter.Complexity++ > filter.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		switch (c)
		{
			case '<': return "&lt;";  
			case '>': return "&gt;";  
			case '&': return "&amp;";  
			case '"': return "&quot;";
			case "'": return "&#39;";
			default: return "&#" + c.charCodeAt(0) + ";";
		}
	}
}
	
NeatHtml.Filter.prototype.HtmlDecode = function(s, filter)
{
	var my = this;
	return s.replace(/&(#(0|[1-9][0-9]{0,9})|#[xX]([0-9a-fA-F]{1,8})|([A-Z:_a-z][A-Z:_a-z0-9._]{0,10}));/g, 
						function (match, isEntity, decDigits, hexDigits, name) {
		if (filter && filter.Complexity++ > filter.MaxComplexity) throw NeatHtml.ContentTooComplexException;
		if (decDigits != null && decDigits.length > 0)
			return String.fromCharCode(parseInt(decDigits, 10));
		if (hexDigits != null && hexDigits.length > 0)
			return String.fromCharCode(parseInt(hexDigits, 16));
		if (name != null && name.length > 0)
			return String.fromCharCode(my.EntityCharCodeMap[name]);
		return match;
	});  
}

NeatHtml.DefaultFilter = new NeatHtml.Filter();
