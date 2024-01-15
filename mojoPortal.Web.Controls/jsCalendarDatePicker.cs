using mojoPortal.Web.Controls.DatePicker;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;

/// <summary>
/// .NET wrapper control for jsCalendar http://www.dynarch.com/projects/calendar/
/// </summary>
[DefaultProperty("Text"), ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
[ValidationProperty("Text")]
public class jsCalendarDatePicker : WebControl, INamingContainer
{
	#region Constructors

	public jsCalendarDatePicker()
	{
		EnsureChildControls();
		btnPickDate.InnerText = "...";
		btnPickDate.ID = btnPickDate.UniqueID;
		txtPickDate.ID = txtPickDate.UniqueID;
		txtPickDate.Columns = Columns;
		txtPickDate.MaxLength = MaxLength;
		txtPickDate.CssClass = CssClass;
	}

	#endregion

	#region Control Declarations

	protected TextBox txtPickDate;
	protected HtmlButton btnPickDate;
	protected HtmlInputImage btnImage;

	#endregion

	#region Enums

	public enum CalendarTheme
	{
		CalendarMojo,
		CalendarBlue,
		CalendarBlue2,
		CalendarBrown,
		CalendarGreen,
		CalendarWin2k1,
		CalendarWin2k2,
		CalendarWin2kCold1,
		CalendarWin2kCold2,
		CalendarSystem,
		CalendarTas
	}

	public enum CalendarLanguage
	{
		// to do check for new ones
		af,
		bg,
		br,
		ca,
		cs,
		da,
		de,
		du,
		el,
		en,
		es,
		fa,
		fi,
		fr,
		he,
		hr,
		hu,
		it,
		jp,
		ko,
		lt,
		nl,
		no,
		pl,
		pt,
		ro,
		ru,
		sl,
		si,
		sk,
		sp,
		sv,
		tr,
		zh
	}

	#endregion

	#region Public Properties

	public override ControlCollection Controls
	{
		get
		{
			EnsureChildControls();
			return base.Controls;
		}
	}

	[TypeConverter(typeof(UnitConverter))]
	public override Unit Width
	{
		get
		{
			EnsureChildControls();
			return base.Width;
		}
		set
		{
			EnsureChildControls();
			base.Width = value;
			txtPickDate.Width = Unit.Pixel((int)value.Value - 24);
		}
	}

	public int Columns
	{
		get
		{
			EnsureChildControls();
			return txtPickDate.Columns;
		}
		set
		{
			EnsureChildControls();
			txtPickDate.Columns = value;
		}
	}

	public int MaxLength
	{
		get
		{
			EnsureChildControls();
			return txtPickDate.MaxLength;
		}
		set
		{
			EnsureChildControls();
			txtPickDate.MaxLength = value;
		}
	}

	[Bindable(true), Category("Appearance"), DefaultValue("")]
	public string Text
	{
		get
		{
			EnsureChildControls();
			return txtPickDate.Text;
		}
		set
		{
			EnsureChildControls();
			txtPickDate.Text = value;
		}
	}

	public string ButtonImageUrl { get; set; } = string.Empty;
	public string View { get; set; }
	public string MinView { get; set; }
	public string RelatedPickerControl { get; set; }
	public RelatedPickerRelation RelatedPickerRelation { get; set; }
	public bool ShowTimeOnly { get; set; } = false;
	public string MinDate { get; set; }
	public string MaxDate { get; set; }
	public string OnSelectJS { get; set; }
	public string ExtraSettingsJS { get; set; }

	[Bindable(true), Category("Appearance"), DefaultValue(CalendarTheme.CalendarMojo)]
	public CalendarTheme Theme
	{
		get { return ViewState["Theme"] != null ? (CalendarTheme)ViewState["Theme"] : CalendarTheme.CalendarMojo; }
		set { ViewState["Theme"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue("24")]
	public string ClockHours
	{
		get { return ViewState["ClockHours"] != null ? (string)ViewState["ClockHours"] : "24"; }
		set { ViewState["ClockHours"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript/DatePicker")]
	public string ScriptDirectory
	{
		get { return ViewState["ScriptDirectory"] != null ? (string)ViewState["ScriptDirectory"] : "~/ClientScript/DatePicker"; }
		set { ViewState["ScriptDirectory"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue("~/Data/style")]
	public string StyleDirectory
	{
		get { return ViewState["StyleDirectory"] != null ? (string)ViewState["StyleDirectory"] : "~/Data/style"; }
		set { ViewState["StyleDirectory"] = value; }
	}

	public string Align
	{
		get { return ViewState["Align"] != null ? (string)ViewState["Align"] : "BR"; }
		set { ViewState["Align"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue("en")]
	public string Language
	{
		get { return ViewState["Language"] != null ? (string)ViewState["Language"] : "en"; }
		set { ViewState["Language"] = value; }
	}

	public string LanguageFile
	{
		get { return ViewState["LanguageFile"] != null ? (string)ViewState["LanguageFile"] : "en"; }
		set { ViewState["LanguageFile"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue(false)]
	public bool ShowTime
	{
		get { return ViewState["ShowTime"] != null && (bool)ViewState["ShowTime"]; }
		set { ViewState["ShowTime"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue(false)]
	public bool SingleClick
	{
		get { return ViewState["SingleClick"] != null && (bool)ViewState["SingleClick"]; }
		set { ViewState["SingleClick"] = value; }
	}

	[Bindable(true), Category("Behavior"), DefaultValue(false)]
	public bool CalendarSelectionOnly
	{
		get { return ViewState["CalendarSelectionOnly"] != null && (bool)ViewState["CalendarSelectionOnly"]; }
		set { ViewState["CalendarSelectionOnly"] = value; }
	}

	public string FormatString
	{
		get { return ViewState["FormatString"] != null ? (string)ViewState["FormatString"] : ""; }
		set { ViewState["FormatString"] = value; }
	}

	#endregion

	#region Protected/Private Methods

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
	}


	/// <summary> 
	/// Render this control to the output parameter specified.
	/// </summary>
	protected override void Render(HtmlTextWriter writer)
	{
		if (Site != null && Site.DesignMode)
		{
			// render to the designer
			txtPickDate.RenderControl(writer);
			writer.Write($"[{ID}]");
		}
		else
		{
			// render to the response stream
			base.Render(writer);
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		if (CalendarSelectionOnly)
		{
			txtPickDate.Attributes.Add("readonly", "readonly");
		}
		else
		{
			txtPickDate.Attributes.Remove("readonly");
		}

		txtPickDate.Enabled = Enabled;
		btnPickDate.Disabled = !Enabled;
		btnImage.Disabled = !Enabled;

		if (ButtonImageUrl.Length > 0)
		{
			Attributes.Add("style", "white-space:nowrap;");
			Controls.Remove(btnPickDate);

			btnImage.Src = Page.ResolveUrl(ButtonImageUrl);
			// TO DO: localize
			btnImage.Alt = "Popup Date Picker";
			btnImage.Attributes.Add("style", "padding:0px 0px 0px 3px;display:inline;");
		}
		else
		{
			Controls.Remove(btnImage);
		}

		SetLocalization();
		SetupStyles();
		SetupScripts();
		base.OnPreRender(e);
	}

	protected override void CreateChildControls()
	{
		txtPickDate = new TextBox
		{
			ID = "txtPickDate"
		};
		btnPickDate = new HtmlButton
		{
			ID = "btnPickDate"
		};

		btnImage = new HtmlInputImage
		{
			ID = "btnImage"
		};


		if (Site != null && Site.DesignMode)
		{

		}
		else
		{
			Controls.Add(txtPickDate);
			var spacer = new Literal();
			Controls.Add(spacer);
			Controls.Add(btnImage);
			Controls.Add(btnPickDate);
		}
	}

	private void SetupStyles()
	{
		if (ButtonImageUrl.Length > 0)
		{
			//this.btnImage.Attributes.Add("class", "datepickerbutton");
		}
		else
		{
			btnPickDate.Attributes.Add("class", "datepickerbutton");
		}

		if (Page.Header is null)
		{
			return;
		}

		if (Page.Header.FindControl("jscalcss") is null)
		{
			var csslink = new HtmlLink
			{
				ID = "jscalcss",
				Href = $"{ResolveUrl(StyleDirectory)}/{Theme}.css"
			};
			csslink.Attributes.Add("rel", "stylesheet");
			Page.Header.Controls.Add(csslink);
		}
	}

	private void SetupScripts()
	{
		Page.ClientScript.RegisterClientScriptBlock(GetType(), "jscalendarmain", $"<script type=\"text/javascript\" src=\"{ResolveUrl($"{ScriptDirectory}/calendar.js")}\"></script>");

		Page.ClientScript.RegisterClientScriptBlock(GetType(), "jscalendarculture", $"<script type=\"text/javascript\" src=\"{ResolveUrl($"{ScriptDirectory}/{LanguageFile}")}\"></script>");

		Page.ClientScript.RegisterClientScriptBlock(GetType(), "jscalendarsetup", $"<script type=\"text/javascript\" src=\"{ResolveUrl($"{ScriptDirectory}/calendar-setup.js")}\"></script>");

		/*
		 
		 <script type="text/javascript">
			Calendar.setup({
				inputField     :    "txtStartDate",      // id of the input field
				ifFormat	   :	"%m/%d/%Y %I:%M %p",    // the date format
				showsTime      :    true,
				button         :    "btnPickDate",   // trigger for the calendar (button ID)
				align		   :	"BR",
				singleClick    :    false          // double-click mode
				
			});
		</script>
		
		
		<asp:TextBox id="txtPickDate" runat="server" cssclass="NormalTextBox" Columns="25" maxlength="50"></asp:TextBox>
		
		<button  id="btnPickDate" runat="server" class="CommandButton">...</button>
		
		
		
		*/

		StringBuilder script = new StringBuilder();

		script.Append("<script type='text/javascript'>");
		script.Append("Calendar.setup({inputField : '" + txtPickDate.ClientID + "', ");
		script.Append("ifFormat : '" + FormatString + "', ");
		script.Append("daFormat : '" + FormatString + "', ");
		script.Append("timeFormat : '" + ClockHours + "' , ");
		script.Append("firstDay : 0 ,  ");
		script.Append("weekNumbers : true ,  ");
		script.Append("showsTime : " + ShowTime.ToString().ToLower(CultureInfo.InvariantCulture) + ", ");

		if (ButtonImageUrl.Length > 0)
		{
			script.Append("button : '" + btnImage.ClientID + "',  ");
		}
		else
		{
			script.Append("button : '" + btnPickDate.ClientID + "',  ");
		}

		script.Append("align : '" + Align + "', ");
		script.Append("singleClick : " + SingleClick.ToString().ToLower(CultureInfo.InvariantCulture));

		if (!string.IsNullOrWhiteSpace(ExtraSettingsJS))
		{
			script.Append($",{ExtraSettingsJS}");
		}

		script.Append("});  ");
		script.Append("</script>  ");

		Page.ClientScript.RegisterStartupScript(GetType(), UniqueID, script.ToString());
	}

	private void SetLocalization()
	{
		Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

		try
		{
			CalendarLanguage cl = (CalendarLanguage)Enum.Parse(typeof(CalendarLanguage), Language);
			Language = cl.ToString();
		}
		catch (ArgumentException)
		{
			Language = CalendarLanguage.en.ToString();
		}


		LanguageFile = string.Format(CultureInfo.InvariantCulture, "calendar-{0}.js", Language);

		FormatString = GetFormatString();
	}

	private string GetFormatString()
	{
		string formatString;
		string timeFormat = string.Empty;

		formatString = RegexReplace(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, "y", "%Y", 4);
		formatString = RegexReplace(formatString, "M", "%m", 4);
		formatString = RegexReplace(formatString, "d", "%d", 4);

		if (ShowTime)
		{
			timeFormat = " " + RegexReplace(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern, "H", "%H", 4);
			timeFormat = RegexReplace(timeFormat, "m", "%M", 4);
			timeFormat = RegexReplace(timeFormat, "h", "%I", 4);
			timeFormat = timeFormat.Replace("tt", "%p");
		}

		return formatString + timeFormat;
	}

	private static string RegexReplace(string inputString, string from, string to, int maxOccurances)
	{
		if (maxOccurances <= 0)
		{
			maxOccurances = 4;
		}
		var pattern = from + "{1," + maxOccurances.ToString(CultureInfo.InvariantCulture) + "}";
		var regex = new Regex(pattern, RegexOptions.Compiled);
		return regex.Replace(inputString, to);
	}
	#endregion
}