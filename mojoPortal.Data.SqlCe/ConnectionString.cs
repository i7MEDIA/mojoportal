using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mojoPortal.Data
{
    public static class ConnectionString
    {
        public static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        // these methods are only for compatibility with import and upgrade utils
        // there is no replication supported for SQL CE so there is no real need for different connection
        // strings for read/write
        public static String GetReadConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        public static String GetWriteConnectionString()
        {
            return DBPortal.GetConnectionString();
        }
    }
}
