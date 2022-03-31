using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class RecaptchaControl : WebControl, IValidator
	{
		#region Fields

		private static readonly ILog log = LogManager.GetLogger(typeof(RecaptchaControl));
		private RecaptchaResponse recaptchaResponse;

		#endregion


		#region Public Properties

		public string ValidationGroup { get; set; } = string.Empty;
		public string PublicKey { get; set; }
		public string PrivateKey { get; set; }
		public string Theme { get; set; } = "light";
		public string Language { get; set; }
		public Dictionary<string, string> CustomTranslations { get; set; }
		public string CustomThemeWidget { get; set; }
		public string VerifyUrl { get; set; }
		public string ClientScriptUrl { get; set; }
		public string Param { get; set; }
		public string ResponseField { get; set; }
		public bool SkipRecaptcha { get; set; }
		public bool AllowMultipleInstances { get; set; }
		public bool OverrideSecureMode { get; set; }
		public IWebProxy Proxy { get; set; }
		public bool RegisterWithScriptManager { get; set; } = true;

		#region IValidator Properties

		public string ErrorMessage
		{
			get
			{
				if (recaptchaResponse != null)
				{
					if (CustomTranslations != null && !string.IsNullOrEmpty(CustomTranslations["incorrect_try_again"]))
					{
						return CustomTranslations["incorrect_try_again"];
					}

					return string.Join(", ", recaptchaResponse.ErrorMessages);
				}

				return null;
			}
			set
			{
				throw new NotImplementedException("ErrorMessage property is not settable. To customize reCAPTCHA error message, use custom translation settings.");
			}
		}

		public bool IsValid
		{
			get
			{
				return recaptchaResponse == null || this.recaptchaResponse.IsValid;
			}
			set
			{
				throw new NotImplementedException("IsValid property is not settable. This property is populated automatically.");
			}
		}

		#endregion

		#endregion


		#region Constructors

		public RecaptchaControl()
		{ }

		#endregion


		#region Override Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (string.IsNullOrEmpty(PublicKey) || string.IsNullOrEmpty(PrivateKey))
			{
				throw new ApplicationException("CAPTCHA needs to be configured with a secret & site key.");
			}

			if (AllowMultipleInstances || !CheckIfRecaptchaExists())
			{
				Page.Validators.Add(this);
			}
		}


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (RegisterWithScriptManager) { SetupAjaxScripts(); }
		}


		protected override void Render(HtmlTextWriter writer)
		{
			if (SkipRecaptcha)
			{
				writer.WriteLine("reCAPTCHA validation is skipped. Set SkipRecaptcha property to false to enable validation.");
			}
			else
			{
				RenderContents(writer);
			}
		}


		protected override void RenderContents(HtmlTextWriter output)
		{
			output.AddAttribute("id", "recaptcha_" + ClientID);
			output.AddAttribute("class", Param);
			output.AddAttribute("data-sitekey", PublicKey);
			output.AddAttribute("data-theme", Theme);
			output.AddAttribute("data-tabindex", base.TabIndex.ToString());

			output.RenderBeginTag(HtmlTextWriterTag.Div);
			output.RenderEndTag();
		}

		#endregion


		#region Public Methods

		public void Validate()
		{
			if (Page.IsPostBack && Visible && Enabled && !SkipRecaptcha)
			{
				if (recaptchaResponse == null)
				{
					if (Visible && Enabled)
					{
						RecaptchaValidator validator = new RecaptchaValidator
						{
							VerifyUrl = VerifyUrl,
							PrivateKey = PrivateKey,
							RemoteIP = Page.Request.UserHostAddress,
							Response = Context.Request.Form[ResponseField],
							Proxy = Proxy
						};

						if (validator.Response == null)
						{
							recaptchaResponse = RecaptchaResponse.InvalidResponse;
						}
						else
						{
							recaptchaResponse = validator.Validate().GetAwaiter().GetResult();
						}
					}
				}
			}
			else
			{
				recaptchaResponse = RecaptchaResponse.Valid;
			}
		}

		#endregion


		#region Private Methods

		private bool CheckIfRecaptchaExists()
		{
			foreach (var validator in Page.Validators)
			{
				if (validator is RecaptchaControl)
				{
					return true;
				}
			}

			return false;
		}


		private void SetupAjaxScripts()
		{
			//need to include async and defer on the script element. ScriptManager isn't allowing that so we'll try this.
			ScriptManager.RegisterClientScriptBlock(
				this,
				typeof(Page),
				"captcharemotesript",
				"var script = document.createElement('script'); script.type='text/javascript'; script.src='" + ClientScriptUrl + "';"
				+ " script.setAttribute('async',''); script.setAttribute('defer',''); document.head.appendChild(script);",
				true);
		}

		#endregion
	}
}
