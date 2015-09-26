

using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace sts.Analytics.Data
{
    public static class Utility
    {
        /// <summary>
        /// returns an int in the format yyyymmdd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int DateTonInteger(DateTime date)
        {
            return Convert.ToInt32(date.ToString("yyyyMMdd", CultureInfo.InvariantCulture));

        }
    }
}
