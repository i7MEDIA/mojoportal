using Newtonsoft.Json;
using System;

namespace mojoPortal.Web.UI
{
	public class CaptchaVerificationResponse
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("challenge_ts")]
		public DateTime ChallengeTimestamp { get; set; }

		[JsonProperty("hostname")]
		public string Hostname { get; set; }

		[JsonProperty("error-codes")]
		public string[] ErrorCodes { get; set; }
	}
}
