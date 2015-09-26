<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html"/>
<xsl:template match="/">
	<table border="0" cellspacing="4">
		
  <xsl:apply-templates select="/MetaData"/>
  </table>
</xsl:template>

<xsl:template match="MetaData">
    <tr>
		<th align="left">
			Date Picture Taken
		</th>
		<td align="left">
			<xsl:value-of select="@Datetime"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			Camera
		</th>
		<td align="left">
			<xsl:value-of select="@EquipModel"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			Dimensions
		</th>
		<td align="left">
			<xsl:value-of select="@ExifPixXDim"/>x<xsl:value-of select="@ExifPixYDim"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			ExifExposureTime
		</th>
		<td align="left">
			<xsl:value-of select="@ExifExposureTime"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			ExifShutterSpeed
		</th>
		<td align="left">
			<xsl:value-of select="@ExifShutterSpeed"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			ExifAperture
		</th>
		<td align="left">
			<xsl:value-of select="@ExifAperture"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			ExifMeteringMode
		</th>
		<td align="left">
			<xsl:value-of select="@ExifMeteringMode"/>
		</td>
    </tr>
    <tr>
		<th align="left">
			ExifSensingMethod
		</th>
		<td align="left">
			<xsl:value-of select="@ExifSensingMethod"/>
		</td>
    </tr>
    
</xsl:template>
</xsl:stylesheet>


