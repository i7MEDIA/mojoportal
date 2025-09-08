using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Subkismet.Captcha;

/// <summary>
/// Simple Spam fighting validator. This renders a text box that should be left blank. 
/// Bots are likely to fill it out by accident.
/// </summary>
public class HoneypotCaptcha : BaseValidator
{
	/// <summary>
	/// Displays the control on the client.
	/// </summary>
	/// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that contains the output stream for rendering on the client.</param>
	protected override void Render(System.Web.UI.HtmlTextWriter writer)
	{
		writer.Write("<div id=\"{0}\"", this.ID);
		if (!String.IsNullOrEmpty(this.CssClass))
		{
			writer.Write(" class=\"{0}\"", this.CssClass);
		}
		if (UseInlineStyleToHide)
		{
			writer.Write(" style=\"display:none;\"");
		}
		writer.Write(">");
		writer.Write(this.Text);
		writer.Write("<input type=\"text\" name=\"{0}\" value=\"{1}\" />", this.ClientID, SubmittedValue);
		writer.Write("</div>");

		if (!this.IsValid)
		{
			if (this.ErrorCssClass.Length == 0)
				writer.Write("<span style=\"color:#c00;\">{0}</span>", this.ErrorMessage);
			else
				writer.Write("<span style=\"{0}\">{1}</span>", this.ErrorCssClass, this.ErrorMessage);
		}
	}

	/// <summary>
	/// Gets or sets the CSS class for the error message.
	/// </summary>
	/// <value>The error CSS class.</value>
	public string ErrorCssClass
	{
		get
		{
			return ((string)ViewState["ErrorCssClass"]) ?? string.Empty;
		}
		set
		{
			ViewState["ErrorCssClass"] = value;
		}
	}

	/// <summary>
	/// Gets a value indicating whether to use style="display: none;" to hide 
	/// this honeypot captcha. If this is set to false, it is up to the developer 
	/// to use CSS or some other means to hide it.
	/// </summary>
	/// <value>
	/// 	<c>true</c> if [use inline style to hide]; otherwise, <c>false</c>.
	/// </value>
	public bool UseInlineStyleToHide
	{
		get
		{
			return (bool)(ViewState["UseInlineStyleToHide"] ?? true);
		}
	}

	/// <summary>
	/// The text to display for non-css enabled browsers.
	/// </summary>
	[Description("The text to display for non-css enabled browsers.")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	public override string Text
	{
		get { return this.text; }
		set { this.text = value; }
	}
	string text = "Leave this field blank.";

	private string SubmittedValue
	{
		get
		{
			return Page.Request.Form[this.ClientID] ?? string.Empty;
		}
	}

	///<summary>
	///When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
	///</summary>
	///
	///<returns>
	///true if the value in the input control is valid; otherwise, false.
	///</returns>
	///
	protected override bool EvaluateIsValid()
	{
		return SubmittedValue.Length == 0;
	}

	/// <summary>Checks the properties of the control for valid values.</summary>
	/// <returns>true if the control properties are valid; otherwise, false.</returns>
	protected override bool ControlPropertiesValid()
	{
		if (!String.IsNullOrEmpty(ControlToValidate))
		{
			CheckControlValidationProperty(ControlToValidate, "ControlToValidate");
		}
		return true;
	}
}
