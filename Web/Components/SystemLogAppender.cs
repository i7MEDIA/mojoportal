//	Author:                 
//  Created:			    2011-07-23
//	Last Modified:		    2011-07-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	


using log4net.Appender;
using log4net.Core;
using mojoPortal.Business;

//http://netpl.blogspot.com/2008/11/log4net-and-appenders-for-different.html

namespace mojoPortal.Web.Logging
{
    public class SystemLogAppender : BufferingAppenderSkeleton
    {
        public SystemLogAppender()
		{

        }
        
        override protected void SendBuffer(LoggingEvent[] events)
        {
            foreach (LoggingEvent e in events)
            {
                try
                {
                    string ip = null;
                    string culture  = null;
                    string url = null;

                    if (e.Properties["ip"] != null) { ip = e.Properties["ip"].ToString(); }
                    if (e.Properties["culture"] != null) { culture = e.Properties["culture"].ToString(); }
                    if(e.Properties["url"] != null){ url = e.Properties["url"].ToString();}

                    SystemLog.Log(ip, culture, url, e.ThreadName, e.Level.Name, e.LoggerName, e.RenderedMessage + " " + e.GetExceptionString());

                }
                catch { } //can't allow throwing exceptions here and can't log them since it would be recursive and possibly an infinite loop/race condition
            }
        }


    }
}