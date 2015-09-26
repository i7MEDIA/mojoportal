// Copyright (c) 2007 Adrian Godong, Ben Maurer
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

//namespace Recaptcha
namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Calls the reCAPTCHA server to validate the answer to a reCAPTCHA challenge. Normally,
    /// you will use the RecaptchaControl class to insert a web control on your page. However
    /// </summary>
    public class RecaptchaValidator
    {
        private const string VerifyUrl = "http://www.google.com/recaptcha/api/verify";

        private string privateKey;
        private string remoteIp;

        private string challenge;
        private string response;

        private IWebProxy proxy;

        public string PrivateKey
        {
            get { return this.privateKey; }
            set { this.privateKey = value; }
        }

        public string RemoteIP
        {
            get
            {
                return this.remoteIp;
            }

            set
            {
                IPAddress ip = IPAddress.Parse(value);

                if (ip == null ||
                    (ip.AddressFamily != AddressFamily.InterNetwork &&
                    ip.AddressFamily != AddressFamily.InterNetworkV6))
                {
                    throw new ArgumentException("Expecting an IP address, got " + ip);
                }

                this.remoteIp = ip.ToString();
            }
        }

        public string Challenge
        {
            get { return this.challenge; }
            set { this.challenge = value; }
        }

        public string Response
        {
            get { return this.response; }
            set { this.response = value; }
        }

        public IWebProxy Proxy
        {
            get { return this.proxy; }
            set { this.proxy = value; }
        }

        private void CheckNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public RecaptchaResponse Validate()
        {
            this.CheckNotNull(this.PrivateKey, "PrivateKey");
            this.CheckNotNull(this.RemoteIP, "RemoteIp");
            this.CheckNotNull(this.Challenge, "Challenge");
            this.CheckNotNull(this.Response, "Response");

            if (this.challenge == string.Empty || this.response == string.Empty)
            {
                return RecaptchaResponse.InvalidSolution;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VerifyUrl);
            request.ProtocolVersion = HttpVersion.Version10;
            request.Timeout = 30 * 1000 /* 30 seconds */;
            request.Method = "POST";
            request.UserAgent = "reCAPTCHA/ASP.NET";
            if (this.proxy != null)
            {
                request.Proxy = this.proxy;
            }

            request.ContentType = "application/x-www-form-urlencoded";

            string formdata = String.Format(
                "privatekey={0}&remoteip={1}&challenge={2}&response={3}",
                                    HttpUtility.UrlEncode(this.PrivateKey),
                                    HttpUtility.UrlEncode(this.RemoteIP),
                                    HttpUtility.UrlEncode(this.Challenge),
                                    HttpUtility.UrlEncode(this.Response));

            byte[] formbytes = Encoding.ASCII.GetBytes(formdata);

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formbytes, 0, formbytes.Length);
            }

            string[] results;

            try
            {
                using (WebResponse httpResponse = request.GetResponse())
                {
                    using (TextReader readStream = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        results = readStream.ReadToEnd().Split(new string[] { "\n", "\\n" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
            }
            catch (WebException ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                return RecaptchaResponse.RecaptchaNotReachable;
            }

            switch (results[0])
            {
                case "true":
                    return RecaptchaResponse.Valid;
                case "false":
                    return new RecaptchaResponse(false, results[1].Trim(new char[] { '\'' }));
                default:
                    throw new InvalidProgramException("Unknown status response.");
            }
        }
    }
}
