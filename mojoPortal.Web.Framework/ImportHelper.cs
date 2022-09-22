using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
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
                new CsvConfiguration(CultureInfo.CurrentCulture) { HasHeaderRecord = hasHeader })
                .GetRecords<dynamic>().ToList();
        }
    }
}
