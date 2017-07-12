/// Author:                 
/// Created:                2006-01-10
/// Last Modified:          2009-01-09

using System;

namespace mojoPortal.Business
{
    
    public class SharedFileHistory
    {
        public SharedFileHistory()
        { }

        private String friendlyName = String.Empty;
        private String serverFileName = String.Empty;

        public String FriendlyName
        {
            get { return friendlyName; }
            set { friendlyName = value; }
        }

        public String ServerFileName
        {
            get { return serverFileName; }
            set { serverFileName = value; }
        }


    }
}
