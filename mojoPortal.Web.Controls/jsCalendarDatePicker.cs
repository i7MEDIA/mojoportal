using mojoPortal.Web.Controls.DatePicker;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{
	/// <summary>
	/// .NET wrapper control for jsCalendar http://www.dynarch.com/projects/calendar/
	/// </summary>
	[DefaultProperty("Text"), ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
	[ValidationProperty("Text")]
	public class jsCalendarDatePicker :  WebControl, INamingContainer
	{
	
		#region Constructors

        public jsCalendarDatePicker()
		{
			EnsureChildControls();
			
			// TODO: make it an option to use an image for the button
			this.btnPickDate.InnerText = "...";
			this.btnPickDate.ID = this.btnPickDate.UniqueID;
			this.txtPickDate.ID = this.txtPickDate.UniqueID;
			this.txtPickDate.Columns = this.Columns;
			this.txtPickDate.MaxLength = this.MaxLength;
			this.txtPickDate.CssClass = this.CssClass;	
		}

		#endregion

		#region Control Declarations

		protected TextBox txtPickDate;
		protected HtmlButton btnPickDate;
        protected HtmlInputImage btnImage;
        private string imageUrl = string.Empty;

        

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
				this.txtPickDate.Width = Unit.Pixel((int)value.Value - 24);
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

        public string ButtonImageUrl
        {
            get
            {

                return imageUrl;
            }
            set
            {

                imageUrl = value;
            }
        }

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
			get { return (ViewState["Theme"] != null ? (CalendarTheme)ViewState["Theme"] : CalendarTheme.CalendarMojo); }
			set { ViewState["Theme"] = value; }	
		}

		[Bindable(true), Category("Behavior"), DefaultValue("24")]
		public string ClockHours
		{	
			get { return (ViewState["ClockHours"] != null ? (string)ViewState["ClockHours"] : "24"); }
			set { ViewState["ClockHours"] = value; }	
		}



        [Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript/DatePicker")]
		public string ScriptDirectory
		{
            get { return (ViewState["ScriptDirectory"] != null ? (string)ViewState["ScriptDirectory"] : "~/ClientScript/DatePicker"); }
			set { ViewState["ScriptDirectory"] = value; }		
		}

		[Bindable(true), Category("Behavior"), DefaultValue("~/Data/style")]
		public string StyleDirectory
		{
			get { return (ViewState["StyleDirectory"] != null ? (string)ViewState["StyleDirectory"] : "~/Data/style"); }
			set { ViewState["StyleDirectory"] = value; }		
		}

		public string Align
		{	
			get { return (ViewState["Align"] != null ? (string)ViewState["Align"] : "BR"); }
			set { ViewState["Align"] = value; }	
		}

		[Bindable(true), Category("Behavior"), DefaultValue("en")]
		public string Language
		{	
			get { return (ViewState["Language"] != null ? (string)ViewState["Language"] : "en"); }
			set { ViewState["Language"] = value; }	
		}

		public string LanguageFile
		{	
			get { return (ViewState["LanguageFile"] != null ? (string)ViewState["LanguageFile"] : "en"); }
			set { ViewState["LanguageFile"] = value; }	
		}


		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool ShowTime
		{	
			get { return (ViewState["ShowTime"] != null ? (bool)ViewState["ShowTime"] : false); }
			set { ViewState["ShowTime"] = value; }	
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool SingleClick
		{	
			get { return (ViewState["SingleClick"] != null ? (bool)ViewState["SingleClick"] : false); }
			set { ViewState["SingleClick"] = value; }	
		}
        [Bindable(true), Category("Behavior"), DefaultValue(false)]
        public bool CalendarSelectionOnly
        {
            get { return (ViewState["CalendarSelectionOnly"] != null ? (bool)ViewState["CalendarSelectionOnly"] : false); }
            set { this.ViewState["CalendarSelectionOnly"] = value; }
        }

		
		public string FormatString
		{	
			get { return (ViewState["FormatString"] != null ? (string)ViewState["FormatString"] : ""); }
			set { ViewState["FormatString"] = value; }	
		}

        

		
		#endregion

		#region Protected/Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //if (ConfigurationManager.AppSettings["ExtJsBasePath"] != null)
            //{
            //    ExtJsBasePath = ConfigurationManager.AppSettings["ExtJsBasePath"];
            //}

        }


		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Site != null && this.Site.DesignMode)
			{
				// render to the designer
				this.txtPickDate.RenderControl(writer);
				writer.Write("[" + this.ID + "]");
			}
			else
			{
				// render to the response stream
				base.Render(writer);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
            if (this.CalendarSelectionOnly)
                this.txtPickDate.Attributes.Add("readonly", "readonly");
            else
                this.txtPickDate.Attributes.Remove("readonly");
            this.txtPickDate.Enabled = this.Enabled;
            this.btnPickDate.Disabled = !this.Enabled;
            this.btnImage.Disabled = !this.Enabled;

            if (imageUrl.Length > 0)
            {
                this.Attributes.Add("style", "white-space:nowrap;");
                this.Controls.Remove(btnPickDate);
                
                btnImage.Src = Page.ResolveUrl(imageUrl);
                // TO DO: localize
                btnImage.Alt = "Popup Date Picker";
                btnImage.Attributes.Add("style", "padding:0px 0px 0px 3px;display:inline;");

                
            }
            else
            {
                this.Controls.Remove(btnImage);
            }
            
			SetLocalization();
			SetupStyles();
			SetupScripts();
			base.OnPreRender (e);

		}

		protected override void CreateChildControls()
		{
            txtPickDate = new TextBox();
            txtPickDate.ID = "txtPickDate";
            btnPickDate = new HtmlButton();
            btnPickDate.ID = "btnPickDate";

            btnImage = new HtmlInputImage();
            btnImage.ID = "btnImage";
            

            if (this.Site != null && this.Site.DesignMode)
            {
                
            }
            else
            {
                this.Controls.Add(this.txtPickDate);
                Literal spacer = new Literal();
                //spacer.Text = "&nbsp;";
                this.Controls.Add(spacer);
                this.Controls.Add(btnImage);
                this.Controls.Add(btnPickDate);

                
                
            }
			
		}

		private void SetupStyles()
		{
            //string calendarCss = "<style type=\"text/css\">@import url(" 
            //    + ResolveUrl(this.StyleDirectory) + "/" 
            //    + this.Theme.ToString() +".css);</style>\n";

            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "calendarcss", calendarCss);

            if (imageUrl.Length > 0)
            {
                //this.btnImage.Attributes.Add("class", "datepickerbutton");
            }
            else
            {
                this.btnPickDate.Attributes.Add("class", "datepickerbutton");
            }

            

            if (Page.Header == null) return;

            

            if (Page.Header.FindControl("jscalcss") == null)
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
			this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "jscalendarmain","<script type=\"text/javascript\" src=\"" 
				+ ResolveUrl(this.ScriptDirectory + "/calendar.js") + "\"></script>");

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "jscalendarculture", "<script type=\"text/javascript\" src=\""
                + ResolveUrl(this.ScriptDirectory + "/" + this.LanguageFile) + "\"></script>");

			this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(),"jscalendarsetup","<script type=\"text/javascript\" src=\"" 
				+ ResolveUrl(this.ScriptDirectory + "/calendar-setup.js") + "\"></script>");

			/*
			 
			 <script type="text/javascript">
				Calendar.setup({
					inputField     :    "txtStartDate",      // id of the input field
					ifFormat	   : "%m/%d/%Y %I:%M %p",    // the date format
					showsTime      :    true,
					button         :    "btnPickDate",   // trigger for the calendar (button ID)
					align		   :	"BR",
					singleClick    :    false          // double-click mode
					
				});
			</script>
			
			
			<asp:TextBox id="txtPickDate" runat="server" cssclass="NormalTextBox" Columns="25" maxlength="50"></asp:TextBox>
			
			<button  id="btnPickDate" runat="server" class="CommandButton">...</button>
			
			
			
			*/

			// TODO: remove hard coded settings and make properties
			// though I can't think of a reason to change the last 2 hard coded ones

            StringBuilder script = new StringBuilder();

            script.Append("<script type='text/javascript'>");
            script.Append("Calendar.setup({inputField : '" + this.txtPickDate.ClientID + "', ");
            script.Append("ifFormat : '" + this.FormatString + "', ");
            script.Append("daFormat : '" + this.FormatString + "', ");
            script.Append("timeFormat : '" + this.ClockHours + "' , ");
            script.Append("firstDay : 0 ,  ");
            script.Append("weekNumbers : true ,  ");
            script.Append("showsTime : " + this.ShowTime.ToString().ToLower(CultureInfo.InvariantCulture) + ", ");

            if (imageUrl.Length > 0)
            {
                script.Append("button : '" + this.btnImage.ClientID + "',  ");
            }
            else
            {
                script.Append("button : '" + this.btnPickDate.ClientID + "',  ");
            }
            
            script.Append("align : '" + this.Align + "', ");
            script.Append("singleClick : " + this.SingleClick.ToString().ToLower(CultureInfo.InvariantCulture));
			
			if (!string.IsNullOrWhiteSpace(ExtraSettingsJS))
			{
				script.Append($",{ExtraSettingsJS}");
			}

            script.Append("});  ");
            script.Append("</script>  ");

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, script.ToString());

            //string hookupInputScript = "<script type=\"text/javascript\">" 
            //    + "Calendar.setup({inputField : \"" + this.txtPickDate.ClientID + "\", " 
            //    + "ifFormat : \"" + this.FormatString + "\", "
            //    + "daFormat : \"" + this.FormatString + "\", "
            //    + "timeFormat : \"" + this.ClockHours + "\" , "
            //    + "firstDay : 0 , "
            //    + "weekNumbers : true , "
            //    + "showsTime : " + this.ShowTime.ToString().ToLower(CultureInfo.InvariantCulture) + ", "
            //    + "button : \"" + this.btnPickDate.ClientID + "\"," 
            //    + "align : \"" + this.Align + "\", "
            //    + "singleClick : " + this.SingleClick.ToString().ToLower(CultureInfo.InvariantCulture) + "});</script>";

            //this.Page.ClientScript.RegisterStartupScript(this.GetType(),this.UniqueID, hookupInputScript);


		}


		private void SetLocalization()
		{
			this.Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

			try
			{
				CalendarLanguage cl = (CalendarLanguage)Enum.Parse(typeof(CalendarLanguage), this.Language);
				this.Language = cl.ToString();
			}
			catch(ArgumentException)
			{
				this.Language = CalendarLanguage.en.ToString();
			}


			this.LanguageFile = string.Format(CultureInfo.InvariantCulture, "calendar-{0}.js", this.Language);

			this.FormatString = GetFormatString();
		}

		private string GetFormatString()
		{
			string formatString;
			string timeFormat = string.Empty;

			formatString = RegexReplace(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, "y", "%Y", 4);
			formatString = RegexReplace(formatString, "M", "%m", 4);
			formatString = RegexReplace(formatString, "d", "%d", 4);

			if(this.ShowTime)
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
			if(maxOccurances <= 0)
			{
				maxOccurances = 4;
			}
			string pattern = from + "{1," + maxOccurances.ToString(CultureInfo.InvariantCulture) + "}";
			Regex regex = new Regex(pattern, RegexOptions.Compiled);
			return regex.Replace(inputString, to);
		}

		

		#endregion
		
	}
}
