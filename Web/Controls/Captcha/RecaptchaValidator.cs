using log4net;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;

namespace mojoPortal.Web.UI
{
	public class RecaptchaValidator
	{
		#region Fields

		private static readonly ILog log = LogManager.GetLogger(typeof(RecaptchaValidator));
		private string remoteIp;
		private string challenge;

		#endregion


		#region Public Properties

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
				var ip = IPAddress.Parse(value);

				if (ip == null || ip.AddressFamily != AddressFamily.InterNetwork && ip.AddressFamily != AddressFamily.InterNetworkV6)
				{
					throw new ArgumentException("Expecting an IP address, got " + ip);
				}

				remoteIp = ip.ToString();
			}
		}

		public string Response { get; set; }

		public IWebProxy Proxy { get; set; }

		#endregion


		#region Private Methods

		private void CheckNotNull(object obj, string name)
		{
			if (obj == null)
			{
				var error = new ArgumentNullException(name);

				log.Error(error);

				throw error;
			}
		}

		#endregion


		#region Public Methods

		public async Task<RecaptchaResponse> Validate()
		{
			CheckNotNull(PrivateKey, "PrivateKey");
			CheckNotNull(RemoteIP, "RemoteIp");
			CheckNotNull(Response, "Response");

			if (Response == string.Empty)
			{
				return RecaptchaResponse.InvalidSolution;
			}

			CaptchaVerificationResponse captchaVerfication;

			try
			{
				using (var client = new HttpClient())
				{
					var url = $"{VerifyUrl}?secret={HttpUtility.UrlEncode(PrivateKey)}&remoteip={HttpUtility.UrlEncode(RemoteIP)}&response={HttpUtility.UrlEncode(Response)}";
					var response = await client.PostAsync(url, null).ConfigureAwait(false);
					var jsonString = await response.Content.ReadAsStringAsync();

					captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);
				}
			}
			catch (WebException e)
			{
				log.Error(e);

				return RecaptchaResponse.RecaptchaNotReachable;
			}

			if (captchaVerfication.Success)
			{
				return RecaptchaResponse.Valid;
			}
			else
			{
				return new RecaptchaResponse(false, captchaVerfication.ErrorCodes);
			}

			throw new InvalidProgramException("Unknown status response.");
		}

		#endregion
	}
}
