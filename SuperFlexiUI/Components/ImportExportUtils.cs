using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Text;
using mojoPortal.Web.Framework;
using CsvHelper;
using System.IO;

namespace SuperFlexiUI.Components
{
    public class ImportExportUtils
    {

        public void ExportItemsToCSV(List<dynamic> objects)
        {
            var context = HttpContext.Current;
            if (context == null) { return; }
            context.Response.ClearHeaders(); // this fixed an issue downloading in IE over ssl
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.ContentType = "text/csv";

            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + "output.csv" + "\"");

            context.Response.ContentEncoding = new UTF8Encoding();

            StreamWriter sw = new StreamWriter(context.Response.OutputStream);

            using (var csv = new CsvWriter(sw))
            {
                csv.WriteRecords(objects);
            }

            //using (MemoryStream Ms = new MemoryStream())
            //    {
            //        using (StreamWriter Sw = new StreamWriter(Ms))
            //        {
            //            CsvWriter OutputCsv = new CsvWriter(Sw);
            //            OutputCsv.WriteRecords(objects);
            //        }
            //        return Ms.ToArray();
            //    }
            



            //var columnNames = ((IDictionary<string, object>)objects[0]).Keys.ToArray();

            //var csvString = new StringBuilder();
            //var headerString = String.Join(",", columnNames);

            //csvString.AppendLine(headerString);

            //foreach (IDictionary<string, object> obj in objects)
            //{
            //    var rowString = new StringBuilder();

            //    foreach (string prop in columnNames)
            //    {
            //        rowString.AppendFormat("{0},", ExportHelper.AutoEscapeStringForCsv(obj[prop].ToString()));
            //    }
            //    csvString.AppendLine(rowString.ToString().TrimEnd(','));
            //}

            //string csv = csvString.ToString();
            //ExportHelper.ExportStringAsFile(HttpContext.Current, Encoding.UTF8, "text/csv", csv, "output.txt");
        }

    }
}