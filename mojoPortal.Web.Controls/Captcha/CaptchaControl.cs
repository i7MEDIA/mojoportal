using mojoPortal.Web.Controls.Captcha;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;


public class CaptchaControl : Panel
{
	private CaptchaProvider captchProvider;
	private ICaptcha captcha = null;
	private string providerName = "SimpleMathCaptchaProvider";

	public string ValidationGroup { get; set; } = string.Empty;

	public ICaptcha Captcha => captcha;

	public bool IsValid
	{
		get
		{
			if (captcha is null || !Visible || !Enabled)
			{
				return true;
			}

			try
			{
				return captcha.IsValid;
			}
			catch (NullReferenceException)
			{
				// this is a hack because of a bug in ReCaptcha where it still tries to valiate even when Enabled = false;
				if (providerName == "RecaptchaCaptchaProvider")
				{
					return true;
				}
			}

			return false;
		}
	}

	public override bool Enabled
	{
		get => captcha.Enabled;
		set => captcha.Enabled = value;
	}

	public string ProviderName
	{
		get => providerName;
		set
		{
			providerName = value;

			if (Site is not null && Site.DesignMode)
			{

			}
			else
			{
				var newcaptchProvider = CaptchaManager.Providers[providerName];

				if (newcaptchProvider != captchProvider)
				{
					captchProvider = newcaptchProvider;
					captcha = captchProvider.GetCaptcha();

					if (ValidationGroup.Length > 0)
					{
						captcha.ValidationGroup = ValidationGroup;
					}

					Controls.Clear();
					Controls.Add(captcha.GetControl());
				}
			}
		}
	}

	public CaptchaProvider Provider => captchProvider;

	protected override void OnInit(EventArgs e)
	{
		if (Site is not null && Site.DesignMode)
		{ }
		else
		{
			base.OnInit(e);
			// an exception always happens here in design mode
			// this try is just to fix the display in design view in VS

			try
			{
				captchProvider = CaptchaManager.Providers[providerName];
				captcha = captchProvider.GetCaptcha();
				Controls.Add(captcha.GetControl());
			}
			catch { }
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
	}

	private string recaptchaPrivateKey = string.Empty;

	/// <summary>
	/// deprecated not used internally properties remain only for backward compat with custom modules
	/// </summary>
	public string RecaptchaPrivateKey
	{
		// this is kind of hacky but recaptch requires a different set
		// of keys for each domain and needed a way to pass that in
		set => recaptchaPrivateKey = value;
	}

	private string recaptchaPublicKey = string.Empty;

	/// <summary>
	/// deprecated not used internally properties remain only for backward compat with custom modules
	/// </summary>
	public string RecaptchaPublicKey
	{
		// this is kind of hacky but recaptch requires a different set
		// of keys for each domain and needed a way to pass that in
		set => recaptchaPublicKey = value;
	}

	protected override void Render(HtmlTextWriter writer)
	{
		if (HttpContext.Current == null)
		{
			writer.Write($"[{ID}]");
		}
		else
		{
			base.Render(writer);
		}
	}
}
