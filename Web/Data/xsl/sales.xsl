<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
    
    <xsl:template match="/">
    <table width="210" border="1pt" cellspacing="0" cellpadding="3" bordercolor="#dddddd" style="border-collapse:collapse;">
        <tr>
            <th align="left">Product <br/>Category</th>
            <th>Revenue (Millions)</th>
            <th>Growth</th>
        </tr>
        <xsl:for-each select='sales/product'>
            <tr>
                <td class="Normal" width="100">
                    <i><xsl:value-of select='@id'/></i>
                </td>
                <td class="Normal">
                    <CENTER>
                        <xsl:value-of select='revenue'/>
                    </CENTER>
                </td>
                <td class="Normal">
                    <xsl:if test='growth &lt; 0'>
                        <xsl:attribute name='style'>
                            <xsl:text>color:red</xsl:text>
                        </xsl:attribute>
                    </xsl:if>
                    <CENTER>
                        <xsl:value-of select='growth'/>
                    </CENTER>
                </td>
            </tr>
        </xsl:for-each>
    </table>
    </xsl:template>
    
</xsl:stylesheet>
