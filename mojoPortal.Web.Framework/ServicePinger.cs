using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using log4net;

namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// Author:					
    /// Created:				2008-03-16
    /// Last Modified:			2008-03-16
    ///		
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.	
    /// 
    /// </summary>
    public class ServicePinger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServicePinger));

        public ServicePinger(string siteName, string siteUrl, string serviceUrl)
        {
            if (siteName == null)
                throw new ArgumentException("siteName can't be null");

            if (siteUrl == null)
                throw new ArgumentException("siteUrl can't be null");

            if (serviceUrl == null)
                throw new ArgumentException("serviceUrl can't be null");

            if (siteName.Length == 0)
                throw new ArgumentException("siteName can't be empty");

            if (siteUrl.Length == 0)
                throw new ArgumentException("siteUrl can't be empty");

            if (serviceUrl.Length == 0)
                throw new ArgumentException("serviceUrl can't be empty");

            pingingSiteName = siteName;
            pingingSiteUrl = siteUrl;
            serviceUrlToPing = serviceUrl;



        }

        private string pingingSiteName = string.Empty;
        private string pingingSiteUrl = string.Empty;
        private string serviceUrlToPing = string.Empty;
        private int timeoutInMilliseconds = 3000;


        /// <summary>
        /// Does the actual pinging of the service
        /// </summary>
        public void Ping()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUrlToPing);
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Timeout = timeoutInMilliseconds;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;

               
                Stream stream = (Stream)request.GetRequestStream();
                using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.ASCII))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("methodCall");
                    writer.WriteElementString("methodName", "weblogUpdates.ping");
                    writer.WriteStartElement("params");
                    writer.WriteStartElement("param");
                    writer.WriteElementString("value", pingingSiteName);
                    writer.WriteEndElement();
                    writer.WriteStartElement("param");
                    writer.WriteElementString("value", pingingSiteUrl);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }


                request.GetResponse();
            }
            catch (InvalidOperationException ex)
            {
                log.Error(ex);
            }
            catch (NotSupportedException ex)
            {
                log.Error(ex);
            }
            


        }

    }
}
