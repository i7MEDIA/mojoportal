//	Author:                 
//  Created:			    2011-07-23
//	Last Modified:		    2011-07-24
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
using System.Linq;
using System.Text;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    public static class SystemLog
    {

        public static int Log(
            string ipAddress,
            string culture,
            string url,
            string thread,
            string logLevel,
            string logger,
            string message)
        {
            string shortUrl = null;
            if (!string.IsNullOrEmpty(url))
            {
                if (url.Length >= 255)
                {
                    shortUrl = url.Substring(0, 255);
                }
                else
                {
                    shortUrl = url;
                }
            }

            return DBSystemLog.Create(
                DateTime.UtcNow,
                ipAddress,
                culture,
                url,
                shortUrl,
                thread,
                logLevel,
                logger,
                message);
        }


        public static void DeleteAll()
        {
            DBSystemLog.DeleteAll();
        }

        public static bool Delete(int id)
        {
            return DBSystemLog.Delete(id);
        }

        public static bool DeleteOlderThan(DateTime cutoffDate)
        {
            return DBSystemLog.DeleteOlderThan(cutoffDate);
        }

        public static bool DeleteByLevel(string logLevel)
        {
            return DBSystemLog.DeleteByLevel(logLevel);
        }

        public static IDataReader GetPageAscending(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBSystemLog.GetPageAscending(pageNumber, pageSize, out totalPages);
        }

        public static IDataReader GetPageDescending(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBSystemLog.GetPageDescending(pageNumber, pageSize, out totalPages);
        }

    }
}
