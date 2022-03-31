using System;
using System.Collections.Generic;

namespace mojoPortal.Web.UI
{
	public class RecaptchaResponse
	{
		#region Public Properties

		public static readonly RecaptchaResponse Valid = new RecaptchaResponse(true, new string[] { });
		public static readonly RecaptchaResponse InvalidResponse = new RecaptchaResponse(false, new string[] { "Invalid reCAPTCHA request. Missing response value." });
		public static readonly RecaptchaResponse InvalidSolution = new RecaptchaResponse(false, new string[] { "Invalid reCaptcha Solution." });
		public static readonly RecaptchaResponse RecaptchaNotReachable = new RecaptchaResponse(false, new string[] { "The reCAPTCHA server is unavailable." });

		public bool IsValid { get; }
		public string[] ErrorMessages { get; }

		#endregion


		#region Constructors

		internal RecaptchaResponse(bool isValid, string[] errorMessages)
		{
			RecaptchaResponse templateResponse = null;

			if (IsValid)
			{
				templateResponse = Valid;
			}
			else
			{
				var errors = new List<string>(errorMessages);

				if (errors.Contains("invalid-input-response"))
				{
					templateResponse = InvalidSolution;
				}

				if (errors.Contains("missing-input-response"))
				{
					templateResponse = InvalidResponse;
				}

				if (errorMessages == null)
				{
					throw new ArgumentNullException("errorMessages");
				}
			}

			if (templateResponse != null)
			{
				IsValid = templateResponse.IsValid;
				ErrorMessages = templateResponse.ErrorMessages;
			}
			else
			{
				IsValid = isValid;
				ErrorMessages = errorMessages;
			}
		}

		#endregion


		#region Public Methods

		public override bool Equals(object obj)
		{
			var other = (RecaptchaResponse)obj;

			if (other == null)
			{
				return false;
			}

			return other.IsValid == IsValid && other.ErrorMessages == ErrorMessages;
		}


		public override int GetHashCode() => IsValid.GetHashCode() ^ ErrorMessages.GetHashCode();

		#endregion
	}
}
