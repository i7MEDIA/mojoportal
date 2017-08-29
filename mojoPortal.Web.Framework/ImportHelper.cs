using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mojoPortal.Web.Framework
{
    public static class ImportHelper
    {

        public static List<dynamic> GetDynamicListFromCSV(Stream fileStream, bool hasHeader = true)
        {
            return new CsvReader(
                new StreamReader(fileStream),
                new CsvConfiguration() { HasHeaderRecord = hasHeader })
                .GetRecords<dynamic>().ToList();
        }
    }
}
