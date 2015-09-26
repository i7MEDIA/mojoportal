using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using mojoPortal.Net;

namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    public class GNotificationTester
    {
        protected string merchantID;
        protected string merchantKey;
        protected string requestUrl;
        protected string requestXml;
        protected int timeoutMilliseconds;

        public GNotificationTester(
            string MerchantID,
            string MerchantKey,
            string RequestUrl,
            string RequestXml,
            int TimeoutMilliseconds)
        {
            merchantID = MerchantID;
            merchantKey = MerchantKey;
            requestUrl = RequestUrl;
            requestXml = RequestXml;
            timeoutMilliseconds = TimeoutMilliseconds;



        }

        private static string GetAuthorization(string user, string password)
        {
            return Convert.ToBase64String(StringToUtf8Bytes(
              string.Format("{0}:{1}", user, password)));
        }

        private static byte[] StringToUtf8Bytes(string inString)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);
            return utf8encoder.GetBytes(inString);
        }

        public string Test()
        {
            StringBuilder results = new StringBuilder();
            results.Append("\n\n");
            DateTime StartTime = DateTime.MinValue;
            byte[] Data = StringToUtf8Bytes(requestXml);

            //ServicePointManager.CertificatePolicy =
            //  new TrustAllCertificatePolicy();

            RemoteCertificateValidationCallback callback = new RemoteCertificateValidationCallback(ValidateAnyServerCertificate);

            ServicePointManager.ServerCertificateValidationCallback = callback;

            HttpWebRequest webRequest =
              (HttpWebRequest)WebRequest.Create(requestUrl);

            webRequest.Method = "POST";
            webRequest.ContentLength = Data.Length;

            webRequest.Headers.Add("Authorization",
              string.Format("Basic {0}",
              GetAuthorization(merchantID, merchantKey)));

            webRequest.ContentType = "application/xml";
            webRequest.Accept = "application/xml";
            webRequest.Timeout = timeoutMilliseconds;

            try
            {
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(Data, 0, Data.Length);
                requestStream.Close();


                try
                {
                    StartTime = DateTime.Now;
                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                    results.Append(string.Format("Status code: {0}\n", webResponse.StatusCode));

                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(responseStream);

                    results.Append(responseReader.ReadToEnd());
                    responseReader.Close();

                    results.Append(string.Format("\nResponse time: {0} ms",
                        DateTime.Now.Subtract(StartTime).TotalMilliseconds));
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        HttpWebResponse errorResponse = (HttpWebResponse)ex.Response;
                        results.Append(string.Format("Status code: {0}\n", errorResponse.StatusCode));

                        StreamReader sr = new
                          StreamReader(errorResponse.GetResponseStream());

                        results.Append(sr.ReadToEnd());
                        sr.Close();

                        results.Append(string.Format("\nResponse time: {0} ms",
                            DateTime.Now.Subtract(StartTime).TotalMilliseconds));
                    }
                }
            }
            catch (WebException ex)
            {
                results.Append(ex.Message);
            }

            return results.ToString();
        }

        public static bool ValidateAnyServerCertificate(
            Object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
