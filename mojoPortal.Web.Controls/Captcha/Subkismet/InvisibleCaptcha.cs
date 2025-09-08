using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subkismet.Captcha;

/// <summary>
/// <para>Simple CAPTCHA control that requires the browser to perform a 
/// simple calculation via javascript to pass.  
/// </para>
/// <para>
/// If javascript is not enabled, a form is rendered asking the user to add two random 
/// small numbers, unless the <see cref="Accessible" />  property is set to false.
/// </para>
/// </summary>
public class InvisibleCaptcha : CaptchaBase
{
	string directions = string.Empty;
	Random rnd = new Random();

	/// <summary>
	/// If Accessible is true and javascript disabled, this is the 
	/// form field in which the answer would be entered by the user.
	/// </summary>
	string VisibleAnswerFieldName
	{
		get
		{
			return ClientID + "_visibleanswer";
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="InvisibleCaptcha"/> is accessible 
	/// to non-javascript browsers.  If false, then non-javascript browsers will always fail 
	/// validation.
	/// </summary>
	/// <value><c>true</c> if accessible; otherwise, <c>false</c>.</value>
	[Description("Determines whether or not this control will work for non-javascript enabled browsers")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public bool Accessible
	{
		get { return (bool)(ViewState["Accessible"] ?? true); }
		set { ViewState["Accessible"] = value; }
	}

	/// <summary>
	/// Id of the span used to house the visible captcha section.
	/// </summary>
	string CaptchaInputClientId
	{
		get
		{
			return this.ClientID + "_subtext_captcha";
		}
	}

	/// <summary>
	/// Sets up a hashed answer.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e)
	{
		// This is the hidden input the javascript will use to 
		// populate the answer to the little math riddle.
		Page.ClientScript.RegisterHiddenField(this.AnswerFormFieldName, "");
		base.OnInit(e);
	}

	/// <summary>
	/// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
	/// </summary>
	/// <param name="e">A <see cref="T:System.EventArgs"></see> that contains the event data.</param>
	protected override void OnPreRender(EventArgs e)
	{
		int first = rnd.Next(1, 9);
		int second = rnd.Next(1, 9);

		directions = string.Format("Please add {0} and {1} and type the answer here: ", first, second);
		Display = ValidatorDisplay.Dynamic;

		string answer = (first + second).ToString(CultureInfo.InvariantCulture);

		// We store the answer encrypted so it can't be tampered with.
		Page.ClientScript.RegisterHiddenField(this.HiddenEncryptedAnswerFieldName, EncryptAnswer(answer));
		Page.ClientScript.RegisterStartupScript(typeof(InvisibleCaptcha), "MakeCaptchaInvisible", string.Format("<script type=\"text/javascript\">\r\nsubtext_invisible_captcha_hideFromJavascriptEnabledBrowsers('{0}');\r\n</script>", this.CaptchaInputClientId));

		Page.ClientScript.RegisterClientScriptInclude("InvisibleCaptcha",
			 Page.ClientScript.GetWebResourceUrl(this.GetType(), "Subkismet.Resources.InvisibleCaptcha.js"));

		Page.ClientScript.RegisterStartupScript(typeof(InvisibleCaptcha), "ComputeCaptchaAnswer", string.Format("<script type=\"text/javascript\">\r\nsubtext_invisible_captcha_setAnswer({0}, {1}, '{2}');\r\n</script>", first, second, this.AnswerFormFieldName));
		base.OnPreRender(e);
	}

	/// <summary>
	/// Displays the control on the client.
	/// </summary>
	/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that contains the output stream for rendering on the client.</param>
	protected override void Render(HtmlTextWriter writer)
	{
		string answer = Page.Request.Form[AnswerFormFieldName];
		// In an Ajax postback, we don't want to render this if javascript is enabled 
		// because the page won't know to set this span to be invisible.
		if (Accessible && String.IsNullOrEmpty(answer))
		{
			base.Render(writer);
			writer.AddAttribute("id", CaptchaInputClientId);
			if (!string.IsNullOrEmpty(CssClass))
				writer.AddAttribute("class", CssClass);
			writer.RenderBeginTag("span");
			writer.Write(directions);

			writer.Write("<input type=\"text\" name=\"{0}\" value=\"\" />", this.VisibleAnswerFieldName);

			writer.RenderEndTag();
		}
	}

	// Gets the answer from the client, whether entered by 
	// javascript or by the user.
	protected override string GetClientSpecifiedAnswer()
	{
		string answer = base.GetClientSpecifiedAnswer();
		if (String.IsNullOrEmpty(answer))
			answer = Page.Request.Form[this.VisibleAnswerFieldName];
		return answer;
	}
}

