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

// 2016-01-06 i7MEDIA upgraded to v2 using concepts from http://www.codeproject.com/Tips/884193/Google-ReCaptcha-ASP-net-Control

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Script.Serialization;
//namespace Recaptcha
namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Calls the captcha server to validate the answer to a reCAPTCHA challenge. Normally,
    /// you will use the RecaptchaControl class to insert a web control on your page
    /// </summary>
    public class RecaptchaValidator
    {

		private string remoteIp;

        private string challenge;

		public string VerifyUrl { get; set; }

		public string PrivateKey { get; set; }

		public string RemoteIP
        {
            get
            {
                return remoteIp;
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

                remoteIp = ip.ToString();
            }
        }

		public string Response { get; set; }

		public IWebProxy Proxy { get; set; }

		private void CheckNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public RecaptchaResponse Validate()
        {
            CheckNotNull(PrivateKey, "PrivateKey");
            CheckNotNull(RemoteIP, "RemoteIp");
            CheckNotNull(Response, "Response");

            if (Response == string.Empty)
            {
                return RecaptchaResponse.InvalidSolution;
            }

            GoogleVerificationResponseOutput gOutput;

            try
            {
                WebClient client = new WebClient();

                string googleReply = client.DownloadString(string.Format(VerifyUrl +
                    "?secret={0}&remoteip={1}&response={2}",
                                    HttpUtility.UrlEncode(PrivateKey),
                                    HttpUtility.UrlEncode(RemoteIP),
                                    HttpUtility.UrlEncode(Response)));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                gOutput = serializer.Deserialize<GoogleVerificationResponseOutput>(googleReply);
            }
            catch (WebException ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                return RecaptchaResponse.RecaptchaNotReachable;
            }

            switch (gOutput.Success)
            {
                case true:
                    return RecaptchaResponse.Valid;
                case false:
                    return new RecaptchaResponse(false, gOutput.ErrorCodes);
                default:
                    throw new InvalidProgramException("Unknown status response.");
            }
        }
    }

    public class GoogleVerificationResponseOutput
    {
        public bool Success { get; set; }
        public string ErrorCodes { get; set; }
    }
}
