using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subkismet.Captcha;

/// <summary>
/// Implements an image based Captcha validator control.
/// </summary>
/// <remarks>
/// In order for the Captcha control to work, you need to map the request for the captcha image 
/// renderer in web.config.
/// <example>
///	&lt;configuration&gt;
/// ...
///   &lt;system.web&gt;
///     &lt;httpHandlers&gt;
///     ...
///       &lt;add verb="*" path="*CaptchaImage.ashx" type="Subkismet.Captcha.CaptchaImageHandler, Subkismet"/&gt;
///     &lt;/httpHandlers&gt;
///   &lt;/system.web&gt;
/// &lt;/configuration&gt;
/// </example>
/// </remarks>
[DefaultProperty("Text")]
public class CaptchaControl : CaptchaBase, INamingContainer, IPostBackDataHandler
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CaptchaControl"/> class.
	/// </summary>
	public CaptchaControl()
	{
		this.LayoutStyle = Layout.CssBased;
		this.InstructionText = "Please enter the characters shown:";
		this.ErrorMessage = "Please enter the correct word";
		this.Display = ValidatorDisplay.Dynamic;
	}

	private void GenerateNewCaptcha()
	{
		if (this.Width.IsEmpty)
			this.Width = Unit.Pixel(180);
		if (this.Height.IsEmpty)
			this.Height = Unit.Pixel(50);
		this.captcha.TextLength = this.CaptchaLength;
	}

	/// <summary>
	/// Loads the post data.
	/// </summary>
	/// <param name="PostDataKey">The post data key.</param>
	/// <param name="Values">The values.</param>
	/// <returns></returns>
	public bool LoadPostData(string PostDataKey, NameValueCollection Values)
	{
		return false;
	}

	/// <summary>
	/// When overridden in a derived class, this method contains the code to determine 
	/// whether the value in the input control is valid.
	/// </summary>
	/// <returns>
	/// true if the value in the input control is valid; otherwise, false.
	/// </returns>
	protected override bool EvaluateIsValid()
	{
		bool isValid = base.EvaluateIsValid();

		if (isValid)
		{
			//We don't want the CAPTCHA to change if the 
			//user specifies a correct answer but some other 
			//field is not valid.
			this.captcha.Text = GetClientSpecifiedAnswer();
		}
		return isValid;
	}

	/// <summary>
	/// Generates the captcha if it hasn't been generated already.
	/// </summary>
	/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
	protected override void OnPreRender(EventArgs e)
	{
		// We store the answer encrypted so it can't be tampered with.
		if (!Page.IsPostBack || !this.IsValid)
		{
			this.GenerateNewCaptcha();
		}

		base.OnPreRender(e);
	}

	/// <summary>
	/// When implemented by a class, signals the server control to notify the ASP.NET application 
	/// that the state of the control has changed.
	/// </summary>
	public void RaisePostDataChangedEvent()
	{
		//Do nothing.
	}

	void RenderHiddenInputForEncryptedAnswer(HtmlTextWriter writer)
	{
		writer.Write("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", this.HiddenEncryptedAnswerFieldName, EncryptAnswer(CaptchaText));
	}

	protected override void Render(HtmlTextWriter writer)
	{
		RenderHiddenInputForEncryptedAnswer(writer);
		writer.Write("<div id=\"{0}\"", this.ClientID);
		if (!String.IsNullOrEmpty(this.CssClass))
		{
			writer.Write(" class=\"{0}\"", this.CssClass);
		}
		else
		{
			writer.Write(" class=\"captcha\"");
		}
		writer.Write(">");

		string src = VirtualPathUtility.ToAbsolute("~/CaptchaImage.ashx");

		writer.Write("<img src=\"" + src);
		if (!IsDesignMode)
		{
			writer.Write("?spec={0}", HttpUtility.UrlEncodeUnicode(captcha.ToEncryptedString()));
		}
		writer.Write("\" border=\"0\"");

		writer.Write(" width=\"{0}\" ", this.Width.Value);
		writer.Write(" height=\"{0}\" ", this.Height.Value);
		if (this.ToolTip.Length > 0)
		{
			writer.Write(" alt='{0}'", this.ToolTip);
		}
		writer.Write(" />");

		if (this.InstructionText.Length > 0)
		{
			writer.Write("<label for=\"{0}\">", this.AnswerFormFieldName);
			writer.Write(this.InstructionText);
			writer.Write("</label>");
			base.Render(writer);
		}

		writer.Write("<input name=\"{0}\" type=\"text\" size=\"", this.AnswerFormFieldName);
		writer.Write(this.captcha.TextLength.ToString());
		writer.Write("\" maxlength=\"" + this.captcha.TextLength + "\"");
		if (this.AccessKey.Length > 0)
		{
			writer.Write(" accesskey=\"{0}\"", this.AccessKey);
		}
		if (!this.Enabled)
		{
			writer.Write(" disabled=\"disabled\"");
		}
		if (this.TabIndex > 0)
		{
			writer.Write(" tabindex=\"" + this.TabIndex + "\"");
		}
		if (Page.IsPostBack && this.IsValid)
			writer.Write(" value=\"{0}\" />", HttpUtility.HtmlEncode(Page.Request.Form[AnswerFormFieldName]));
		else
			writer.Write(" value=\"\" />");

		writer.Write("</div>");
	}

	[DefaultValue(""), Description("Characters used to render CAPTCHA text. A character will be picked randomly from the string."), Category("Captcha")]
	public string CaptchaChars
	{
		get
		{
			return this.captcha.TextChars;
		}
		set
		{
			this.captcha.TextChars = value;
		}
	}

	[Description("Font used to render CAPTCHA text. If font name is blankd, a random font will be chosen."), DefaultValue(""), Category("Captcha")]
	public string CaptchaFont
	{
		get
		{
			return this.captcha.FontFamily;
		}
		set
		{
			this.captcha.FontFamily = value;
		}
	}

	[Category("Captcha"), Description("Amount of random font warping used on the CAPTCHA text"), DefaultValue(typeof(CaptchaImage.FontWarpFactor), "Low")]
	public CaptchaImage.FontWarpFactor CaptchaFontWarping
	{
		get
		{
			return this.captcha.WarpFactor;
		}
		set
		{
			this.captcha.WarpFactor = value;
		}
	}

	[Category("Captcha"), Description("Number of CaptchaChars used in the CAPTCHA text"), DefaultValue(5)]
	public int CaptchaLength
	{
		get
		{
			return this.captcha.TextLength;
		}
		set
		{
			this.captcha.TextLength = value;
		}
	}

	public string InstructionText { get; set; } = "Please enter the characters shown:";

	/// <summary>
	/// The text to render.
	/// </summary>
	private string CaptchaText
	{
		get
		{
			return this.captcha.Text;
		}
	}

	private static bool IsDesignMode
	{
		get
		{
			return (HttpContext.Current == null);
		}
	}

	[Category("Captcha"), DefaultValue(typeof(Layout), "Horizontal"), Description("Determines if image and input area are displayed horizontally, or vertically.")]
	public Layout LayoutStyle
	{
		get
		{
			return this.layoutStyle;
		}
		set
		{
			this.layoutStyle = value;
		}
	}

	private CaptchaInfo captcha = new CaptchaInfo();
	private Layout layoutStyle = Layout.Horizontal;
	//private string text = "Enter the code shown above:";

	public enum Layout
	{
		Horizontal,
		Vertical,
		/// <summary>
		/// Indicates that the layout will be handled by external css.
		/// </summary>
		CssBased
	}
}

[Serializable]
public struct CaptchaInfo
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CaptchaInfo"/> class.
	/// </summary>
	/// <param name="text">The text.</param>
	public CaptchaInfo(string text)
	{
		this.Width = 180;
		this.Height = 50;
		this.randomTextLength = 5;
		this.WarpFactor = CaptchaImage.FontWarpFactor.Low;
		this.FontFamily = string.Empty;
		this.text = text;
		this.validRandomTextChars = defaultValidRandomTextChars;
		this.DateGenerated = DateTime.Now;
		this.FontFamily = RandomFontFamily();
	}

	/// <summary>
	/// Returns a random font family name.
	/// </summary>
	/// <returns></returns>
	private static string RandomFontFamily()
	{
		InstalledFontCollection fontCollection = new InstalledFontCollection();
		FontFamily[] families = fontCollection.Families;
		string fontFamily = "bogus";
		while (goodFontList.IndexOf(fontFamily) == -1)
		{
			fontFamily = families[random.Next(0, fontCollection.Families.Length)].Name.ToLower();
		}
		return fontFamily;
	}

	/// <summary>
	/// Returns a base 64 encrypted serialized representation of this object.
	/// </summary>
	/// <returns></returns>
	public string ToEncryptedString()
	{
		if (this.Width == 0)
			this.Width = 180;

		if (this.Height == 0)
			this.Height = 50;

		return CaptchaBase.EncryptString(this.ToString());
	}

	/// <summary>
	/// Reconstructs an instance of this type from an encrypted serialized string.
	/// </summary>
	/// <param name="encrypted"></param>
	public static CaptchaInfo FromEncryptedString(string encrypted)
	{
		string decrypted = CaptchaBase.DecryptString(encrypted);
		string[] values = decrypted.Split('|');

		CaptchaInfo info = new CaptchaInfo();
		info.Width = int.Parse(values[0]);
		info.Height = int.Parse(values[1]);
		info.WarpFactor = (CaptchaImage.FontWarpFactor)Enum.Parse(typeof(CaptchaImage.FontWarpFactor), values[2]);
		info.FontFamily = values[3];
		info.Text = values[4];
		info.DateGenerated = DateTime.ParseExact(values[5], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
		return info;
	}

	/// <summary>
	/// A string of valid characters to use in the Captcha text.  
	/// A random character will be selected from this string for 
	/// each character.
	/// </summary>
	public string TextChars
	{
		get
		{
			return this.validRandomTextChars ?? defaultValidRandomTextChars;
		}
		set
		{
			this.validRandomTextChars = value;
			this.text = this.GenerateRandomText();
		}
	}

	private string GenerateRandomText()
	{
		StringBuilder builder = new StringBuilder();
		int length = this.TextChars.Length;
		for (int i = 0; i < TextLength; i++)
		{
			builder.Append(this.TextChars.Substring(random.Next(length), 1));
		}
		DateGenerated = DateTime.Now;
		return builder.ToString();
	}

	/// <summary>
	/// Gets or sets the text to render.
	/// </summary>
	/// <value>The text.</value>
	public string Text
	{
		get
		{
			if (String.IsNullOrEmpty(this.text))
			{
				this.text = this.GenerateRandomText();
			}
			return this.text;
		}
		set
		{
			this.text = value;
		}
	}

	/// <summary>
	/// Number of characters to use in the CAPTCHA test.
	/// </summary>
	/// <value>The length of the text.</value>
	public int TextLength
	{
		get
		{
			if (this.randomTextLength <= 0)
				this.randomTextLength = 4;
			return this.randomTextLength;
		}
		set
		{
			this.randomTextLength = value;
			this.text = this.GenerateRandomText();
		}
	}

	/// <summary>
	/// Returns the fully qualified type name of this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="T:System.String"></see> containing a fully qualified type name.
	/// </returns>
	public override string ToString()
	{
		return String.Format("{0}|{1}|{2}|{3}|{4}|{5}"
							 , this.Width
							 , this.Height
							 , this.WarpFactor
							 , this.FontFamily
							 , this.Text
							 , this.DateGenerated.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
	}

	public DateTime DateGenerated;
	public int Width;
	public int Height;
	public string FontFamily;
	private const string goodFontList = "arial; arial black; comic sans ms; courier new; estrangelo edessa; franklin gothic medium; georgia; lucida console; lucida sans unicode; mangal; microsoft sans serif; palatino linotype; sylfaen; tahoma; times new roman; trebuchet ms; verdana;";
	public CaptchaImage.FontWarpFactor WarpFactor;
	private static Random random = new Random();
	private string text;
	private int randomTextLength;
	private string validRandomTextChars;
	private const string defaultValidRandomTextChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
}

