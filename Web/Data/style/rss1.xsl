<?xml version="1.0" encoding="iso-8859-1"?>

<Q:stylesheet version="1.0"
  xmlns:Q = "http://www.w3.org/1999/XSL/Transform"
  xmlns = "http://www.w3.org/1999/xhtml"
>

<Q:output method="html" />
<Q:template match="/">

<Q:element name="html"><Q:attribute name="class">RssToHtmlByXsl</Q:attribute>
<head>
  <Q:element name="meta">
   <Q:attribute name="content-type">text/html; charset=iso-8859-1</Q:attribute>
  </Q:element>
  <Q:element name="link">
   <Q:attribute name="rel">stylesheet</Q:attribute>
   <Q:attribute name="href">Data/style/rss1.css</Q:attribute>
   <Q:attribute name="type">text/css</Q:attribute>
   <Q:attribute name="title">mainlook</Q:attribute>
  </Q:element>

<Q:for-each select="/rss/channel/title">
  <title>RSS: <Q:value-of select="."/></title>
</Q:for-each>


<!--
 Make a nice link-alternate thing so that when viewed in Firefox et al,
 the little "RSS" subscribey-icon appears
-->
<Q:for-each select="/rss/channel">
  <link rel="alternate" type="application/rss+xml">
    <Q:attribute name="href"><Q:value-of select="."/></Q:attribute>
    <Q:choose>
      <Q:when test="/rss/channel/title">
        <Q:attribute name="title"><Q:value-of select="/rss/channel/title"/></Q:attribute>
      </Q:when>
      <Q:otherwise>
        <Q:attribute name="title">This RSS feed</Q:attribute>
      </Q:otherwise>
    </Q:choose>
  </link>

</Q:for-each>

</head>

<body>

This web page is actually a data file that is meant to be read by RSS reader programs. 
<br/>See <a href="http://interglacial.com/rss/about.html">here</a> to learn more about RSS.

<p class='back'><a href="./" accesskey="U" title="Back to Site">Back To Site</a></p>



<h1 class="feedtitle"><a accesskey="0" href="{/rss/channel/link}">
  <Q:value-of select="/rss/channel/title"/>
</a></h1>

<Q:for-each select="/rss/channel/description">
  <Q:if test=". != /rss/channel/title" >
  <!-- no point in printing them both if they're the same -->
    <p class='desc'><Q:value-of select="."/></p>
  </Q:if>
</Q:for-each>

<!--
<Q:if test="/rss/channel/sy:updatePeriod" >
  <p class='updatefreq'>This feed updates

    <Q:variable name="F" select="/rss/channel/sy:updateFrequency" />
    <Q:choose>
      <Q:when test="$F = '' or $F = 1" > once </Q:when>
      <Q:otherwise> <Q:value-of select="$F"/> times </Q:otherwise>
    </Q:choose>

    <Q:value-of select="/rss/channel/sy:updatePeriod"/>.
    Don't poll it any more often than that! 
  </p>
</Q:if>

-->


<Q:variable name="C" select="count(/rss/channel/item)" />
<p class='leadIn'>
  <Q:choose>
    <Q:when test="$C = 0" >No items </Q:when>
    <Q:when test="$C = 1" >The only item </Q:when>
    <Q:otherwise>The <Q:value-of select="$C" /> items </Q:otherwise>
  </Q:choose>
  currently in this feed:
</p>



<dl class='Items'>

<Q:if test='$C = 0'>  <dt>(Empty)</dt> </Q:if>


<Q:for-each select="/rss/channel/item">

<dt>
  <a href="{link}">
    <Q:if test="position() &lt; 10">
      <Q:attribute name='accesskey'><Q:value-of select="position()" /></Q:attribute>
    </Q:if>

    <Q:choose>
      <Q:when test="title"><Q:value-of select="title"/></Q:when>
      <Q:otherwise><em>(No title)</em></Q:otherwise>
    </Q:choose>
  </a>
</dt>
<Q:if test="pubDate" >
	<Q:value-of select="pubDate" />
</Q:if>
	

<Q:if test="description" >
  <dd name="decodeme"
><Q:value-of  disable-output-escaping="yes" select="description" /></dd>
  <!--
   Alas, many implementations can't, and never will, directly
   support disable-output-escaping.  We try to work around that
   with our JavaScript thing.
  -->
</Q:if>
</Q:for-each>
</dl>



</body								>
	</Q:element					>
		</Q:template			>
			</Q:stylesheet>
