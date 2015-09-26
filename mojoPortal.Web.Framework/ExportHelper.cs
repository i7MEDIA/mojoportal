// Author:					Joe Audette
// Created:				2007-07-12
// Last Modified:			2011-01-06
// 
//				
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Framework
{
   
    public static class ExportHelper
    {

        public static void ExportDataTableToCsv(
            HttpContext context, 
            DataTable table, 
            string filename)
	    {
            if (context == null) { return; }
            context.Response.ClearHeaders(); // this fixed an issue downloading in IE over ssl
		    context.Response.Clear();
		    context.Response.Buffer = true;
		    context.Response.ContentType = "text/csv";

		    context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
		    //context.Response.Charset = "";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

		    StreamWriter sw = new StreamWriter(context.Response.OutputStream);
		    
		    int iColCount = table.Columns.Count;
		    int i;
		    for (i = 0; i <= iColCount - 1; i++) {
			    sw.Write(table.Columns[i]);
			    if (i < iColCount - 1)
			    {
				    sw.Write(",");
			    }
		    }
		    sw.Write(sw.NewLine);

		    i = 0;

		    foreach (DataRow row in table.Rows) {

			    for (i = 0; i <= iColCount - 1; i++) {
				    if (!Convert.IsDBNull(row[i]))
				    {
					    sw.Write("\"");
                        // see #7 in this article
                        //http://tools.ietf.org/html/rfc4180
					    sw.Write(row[i].ToString().CsvEscapeQuotes());
					    sw.Write("\"");
				    }
				    else
				    {
					    sw.Write("");
				    }
				    if (i < iColCount - 1)
				    {
					    sw.Write(",");
				    }
			    }
			    sw.Write(sw.NewLine);
		    }
		    sw.Close();

		    context.Response.End();

	    }


        
        public static void ExportDataTableToWord(
            HttpContext context, 
            DataTable table,
            string filename)
        {
            if (context == null) { return; }

            StringBuilder wordString = new StringBuilder();

            wordString.Append(@"<html " +
                    "xmlns:o='urn:schemas-microsoft-com:office:office' " +
                    "xmlns:w='urn:schemas-microsoft-com:office:word'" +
                    "xmlns='http://www.w3.org/TR/REC-html40'>" +
                    "<head><title>Time</title>");

            wordString.Append(@"<!--[if gte mso 9]>" +
                                     "<xml>" +
                                     "<w:WordDocument>" +
                                     "<w:View>Print</w:View>" +
                                     "<w:Zoom>90</w:Zoom>" +
                                     "</w:WordDocument>" +
                                     "</xml>" +
                                     "<![endif]-->");

            wordString.Append(@"<style>" +
                                    "<!-- /* Style Definitions */" +
                                    "@page Section1" +
                                    "   {size:8.5in 11.0in; " +
                                    "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                                    "   mso-header-margin:.5in; " +
                                    "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                                    " div.Section1" +
                                    "   {page:Section1;}" +
                                    "-->" +
                                   "</style></head>");

            wordString.Append(@"<body lang=EN-US style='tab-interval:.5in'>" +
                                    "<div class=Section1>" +
                                    "<h1>Time and tide wait for none</h1>" +
                                    "<p style='color:red'><I>" +
                                    DateTime.Now + "</I></p></div><table border='1'>");

            
            wordString.Append("<tr>");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                wordString.Append("<td>" + table.Columns[i].ColumnName + "</td>");
            }
            wordString.Append("</tr>");

            //Items
            for (int x = 0; x < table.Rows.Count; x++)
            {
                wordString.Append("<tr>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    wordString.Append("<td>" + table.Rows[x][i] + "</td>");
                }
                wordString.Append("</tr>");
            }

            wordString.Append(@"</table></body></html>");

            context.Response.ClearHeaders(); // this fixed an issue downloading in IE over ssl
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ContentType = "application/msword";

            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            //context.Response.Charset = "";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;
            context.Response.Write(wordString.ToString());
            
        }

        //http://www.code.colostate.edu/export-fwp-to-word.aspx

        //public static string DataTable2WordString(System.Data.DataTable pDT)
        //{
        //    StringBuilder sbTop = new StringBuilder();

        //    sbTop.Append("<html " + "xmlns:o='urn:schemas-microsoft-com:office:office' " + "xmlns:w='urn:schemas-microsoft-com:office:word'" + "xmlns='http://www.w3.org/TR/REC-html40'>" + "<head><title>Time</title>");

        //    sbTop.Append("<!--[if gte mso 9]>" + "<xml>" + "<w:WordDocument>" + "<w:View>Print</w:View>" + "<w:Zoom>90</w:Zoom>" + "</w:WordDocument>" + "</xml>" + "<![endif]-->");

        //    sbTop.Append("<style>" + "<!-- /* Style Definitions */" + "@page Section1" + "   {size:8.5in 11.0in; " + "   margin:1.0in 1.25in 1.0in 1.25in ; " + "   mso-header-margin:.5in; " + "   mso-footer-margin:.5in; mso-paper-source:0;}" + " div.Section1" + "   {page:Section1;}" + "-->" + "</style></head>");

        //    sbTop.Append(("<body lang=EN-US style='tab-interval:.5in'>" + "<div class=Section1>" + "<h1>" + pDT.TableName + "</h1>" + "<p style='color:red'><I>") + DateTime.Now + "</I></p></div><table border='0'>");

        //    string bottom = "</table></body></html>";

        //    StringBuilder sb = new StringBuilder();

        //    foreach (DataRow row in pDT.Rows)
        //    {
        //        sb.Append("<tr>");

        //        foreach (DataColumn col in pDT.Columns)
        //        {
        //            if (row[col].ToString() == "Date:")
        //            {
        //                sb.Append("</tr><tr><td>&nbsp;</td><td>&nbsp;</td></tr>");
        //            }

        //            sb.Append("<td>");
        //            sb.Append(row[col]);
        //            sb.Append("</td>");
        //        }
        //        sb.Append("</tr>");
        //    }

        //    string SSxml = sbTop.ToString() + sb.ToString() + bottom;

        //    return SSxml;

        //}


        public static void ExportStringAsFile(
            HttpContext context,
            Encoding encoding,
            string mimeType,
            string content,
            string filename)
        {
            if (context == null) { return; }
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ContentType = mimeType;

            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            
            context.Response.ContentEncoding = encoding;

            StreamWriter sw = new StreamWriter(context.Response.OutputStream);

            sw.Write(content);
            
            sw.Close();

            context.Response.End();

        }

    }

}
