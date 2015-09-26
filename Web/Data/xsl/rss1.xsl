<?xml version="1.0" encoding="iso-8859-1"?>

<Q:stylesheet version="1.0"
  xmlns:Q = "http://www.w3.org/1999/XSL/Transform"
  xmlns = "http://www.w3.org/1999/xhtml"
>

<Q:output method="html" />
<Q:template match="/">

<div>

<!--
<h3 class="feedtitle"><a accesskey="0" href="{/rss/channel/link}">
  <Q:value-of select="/rss/channel/title"/>
</a></h3>


<Q:for-each select="/rss/channel/description">
  <Q:if test=". != /rss/channel/title" >
    <p class='desc'><Q:value-of select="."/></p>
  </Q:if>
</Q:for-each>


<Q:variable name="C" select="count(/rss/channel/item)" />
<p class='leadIn'>
  <Q:choose>
    <Q:when test="$C = 0" >No items </Q:when>
    <Q:when test="$C = 1" >The only item </Q:when>
    <Q:otherwise>The <Q:value-of select="$C" /> items </Q:otherwise>
  </Q:choose>
  currently in this feed:
</p>
-->


<dl class='Items'>
<!--
<Q:if test='$C = 0'>  <dt>(Empty)</dt> </Q:if>
-->


<Q:for-each select="/rss/channel/item">

<dt>
  <h3 class="feedtitle"><a href="{link}">
    <Q:if test="position() &lt; 10">
      <Q:attribute name='accesskey'><Q:value-of select="position()" /></Q:attribute>
    </Q:if>

    <Q:choose>
      <Q:when test="title"><Q:value-of select="title"/></Q:when>
      <Q:otherwise><em>(No title)</em></Q:otherwise>
    </Q:choose>
  </a></h3>
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



</div>
	
		</Q:template>
			</Q:stylesheet>
