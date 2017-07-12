using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace mojoPortal.Web.Controls
{

	/// <summary>
	/// Author:				
	/// Created:			2005-08-20
	/// Last Modified:		2008-09-03
	/// 
	/// Dependencies:	must point the ScriptDirectory property at
	///							a folder containing SmartCombo.js as well as
	///							the Sarissa javascript files: sarissa.js and sarissa_ieemu_xpath.js
	///							which provide a javascript abstration layer around
	///							the different XmlHttpRequest features of the different 
	///							web browsers.
	///							more info:
	///							http://sarissa.sourceforge.net/doc/
	/// 
	/// </summary>
	[DefaultProperty("Value"), ToolboxData("<{0}:SmartCombo runat=server></{0}:SmartCombo>")]
	[ValidationProperty("Value")]
    [Obsolete("this control is old and junky and depends on outdated javascript use mojoPortal.Web.UI.jQueryAutoCompleteTextBox for similar newer better functionality")]
	public class SmartCombo :  WebControl, INamingContainer
	{

		#region Usage Instructions/Comments

		/*
	 Usage Example:
		
		<%@ Register TagPrefix="mp" Namespace="mojoPortal.Web.Controls" Assembly="mojoPortal.Web" %>
			<mp:SmartCombo 
							ID="scUser" runat="server" 
							DataUrl="../Services/UserDropDownXml.aspx?query=" 
							ShowValueField="False" 
							ValueCssClass="TextLabel" 
							ValueColumns="5" 
							ValueLabelText="UserID:" 
							ValueLabelCssClass="txtmedbold" 
							ButtonImageUrl="../Data/SiteImages/DownArrow.gif" 
							ScriptDirectory="~/ClientScript" 
							Columns="45"  
							MaxLength="50" >
			</mp:SmartCombo>		
	 
			DataUrl query= will pass whatever the user has typed in the input
			DataUrl must return an xml document in this format:
			<?xml version="1.0" encoding="utf-8"?>
			<DATA>
			<R>
				<V>1</V>
				<T>admin@admin.com</T>
			</R>
			</DATA>
			
			R = Row
			V = Value
			T = Text
			
			An example implementation of the page to return Xml
			
			public class UserDropDownXml : System.Web.UI.Page
			{
				protected string query = string.Empty;
			
					private void Page_Load(object sender, System.EventArgs e)
					{
						Response.ContentType = "application/xml";
						Encoding encoding = new UTF8Encoding();
			
						XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding);
						xmlTextWriter.Formatting = Formatting.Indented;
			
						xmlTextWriter.WriteStartDocument();
						xmlTextWriter.WriteStartElement("DATA");
			
						if (WebUser.IsAdminOrContentAdmin)
						{
			
							if(Request.Params.Get("query") != null)
							{
								query = Request.Params.Get("query");
			
								int siteID = 1;
								int rowsToGet = 10;
			
								IDataReader reader = SiteUser.GetSmartDropDownData(siteID, query, rowsToGet);
			
								while(reader.Read())
								{
									xmlTextWriter.WriteStartElement("R");
			
									xmlTextWriter.WriteStartElement("V");
									xmlTextWriter.WriteString(reader["UserID"].ToString());
									xmlTextWriter.WriteEndElement();
			
									xmlTextWriter.WriteStartElement("T");
									xmlTextWriter.WriteString(reader["SiteUser"].ToString().Trim());
									xmlTextWriter.WriteEndElement();
			
									xmlTextWriter.WriteEndElement();
								}
			
								reader.Close();
							}
						}
			
						//end of document
						xmlTextWriter.WriteEndElement();
						xmlTextWriter.WriteEndDocument();
			
						xmlTextWriter.Close();
						Response.End();
			
				}
				
		MS SQL Server example stored procedure for fuzzy search on characters being typed into the combo
		that match on email or username
		
		CREATE PROCEDURE mp_Users_SmartDropDown

		@SiteID				int,
		@Query				nvarchar(50),
		@RowsToGet		int


		AS


		SET ROWCOUNT @RowsToGet


		SELECT 		u1.UserID,
						u1.[Name] AS SiteUser

		FROM			mp_Users u1

		WHERE		u1.SiteID = @SiteID
						AND u1.[Name] LIKE @Query + '%'

		UNION

		SELECT 		u2.UserID,
						u2.[Email] As SiteUser

		FROM			mp_Users u2

		WHERE		u2.SiteID = @SiteID
						AND u2.[Email] LIKE @Query + '%'

		ORDER BY	SiteUser


	
	*/

		#endregion

		#region Constructors

		public SmartCombo()
		{
			EnsureChildControls();
			this.btnCombo.ID = this.btnCombo.UniqueID;
			this.txtUserInput.ID = this.txtUserInput.UniqueID;
		}

		#endregion

		#region Control Declarations

		protected TextBox txtUserInput;
		protected Label lblValueLabel;
		protected TextBox txtValue;
		protected HtmlButton btnCombo;
        protected System.Web.UI.HtmlControls.HtmlGenericControl divWrapper;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMain;
		protected System.Web.UI.HtmlControls.HtmlSelect ddSmartDropdown;



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

		[Bindable(true), Category("Appearance"), DefaultValue("")] 
		public string Text
		{	
			get
			{
				EnsureChildControls();
				return txtUserInput.Text;
			}
			set 
			{
				EnsureChildControls();
				txtUserInput.Text = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")] 
		public string Value
		{	
			get
			{
				EnsureChildControls();
				return this.txtValue.Text;
			}
			set 
			{
				EnsureChildControls();
				txtValue.Text = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string DataUrl
		{
			get { return (ViewState["DataUrl"] != null ? (string)ViewState["DataUrl"] : ""); }
			set { ViewState["DataUrl"] = value; }		
		}

//		[Bindable(true), Category("Behavior"), DefaultValue(null)]
//		public Uri DataUrl
//		{
//			get { return (ViewState["DataUrl"] != null ? (Uri)ViewState["DataUrl"] : null); }
//			set { ViewState["DataUrl"] = value; }		
//		}

		public int ValueColumns
		{	
			get
			{
				EnsureChildControls();
				return this.txtValue.Columns;
			}
			set 
			{
				EnsureChildControls();
				txtValue.Columns = value;
			}
		}

		[Bindable(true), Category("Behavior"), DefaultValue(false)]
		public bool ShowValueField
		{	
			get { return (ViewState["ShowValueField"] != null ? (bool)ViewState["ShowValueField"] : false); }
			set { ViewState["ShowValueField"] = value; }	
		}

		public string ValueCssClass
		{	
			get
			{
				EnsureChildControls();
				return this.txtValue.CssClass;
			}
			set 
			{
				EnsureChildControls();
				txtValue.CssClass = value;
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
				this.txtUserInput.Width = Unit.Pixel((int)value.Value - 24);
			}
		}

		public int Columns
		{	
			get
			{
				EnsureChildControls();
				return txtUserInput.Columns;
			}
			set 
			{
				EnsureChildControls();
				txtUserInput.Columns = value;
			}
		}

		public int MaxLength
		{	
			get
			{
				EnsureChildControls();
				return txtUserInput.MaxLength;
			}
			set 
			{
				EnsureChildControls();
				txtUserInput.MaxLength = value;
			}
		}

		public string InputCssClass
		{	
			get
			{
				EnsureChildControls();
				return this.txtUserInput.CssClass;
			}
			set 
			{
				EnsureChildControls();
				txtUserInput.CssClass = value;
			}
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")] 
		public string ValueLabelText
		{	
			get
			{
				EnsureChildControls();
				return this.lblValueLabel.Text;
			}
			set 
			{
				EnsureChildControls();
				this.lblValueLabel.Text = value;
			}
		}

		public string ValueLabelCssClass
		{	
			get
			{
				EnsureChildControls();
				return this.lblValueLabel.CssClass;
			}
			set 
			{
				EnsureChildControls();
				lblValueLabel.CssClass = value;
			}
		}


		[Bindable(false), Category("Appearance"), DefaultValue("border-right: #999999 1px solid; border-top: #999999 1px solid; font-weight: bold; margin-left: -1px; border-left: #999999 1px solid; border-bottom: #999999 1px solid;")]
		public string ButtonStyle
		{
			get { return (ViewState["ButtonStyle"] != null ? (string)ViewState["ButtonStyle"] : "border-right: #999999 1px solid; BORDER-TOP: #999999 1px solid; font-weight: bold; margin-left: -1px; border-left: #999999 1px solid; border-bottom: #999999 1px solid;"); }
			set { ViewState["ButtonStyle"] = value; }		
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ButtonImageUrl
		{
			get { return (ViewState["ButtonImageUrl"] != null ? (string)ViewState["ButtonImageUrl"] : ""); }
			set { ViewState["ButtonImageUrl"] = value; }		
		}

		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string ButtonText
		{
			get { return (ViewState["ButtonText"] != null ? (string)ViewState["ButtonText"] : ""); }
			set { ViewState["ButtonText"] = value; }		
		}


		[Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript")]
		public string ScriptDirectory
		{
			get { return (ViewState["ScriptDirectory"] != null ? (string)ViewState["ScriptDirectory"] : "~/ClientScript"); }
			set { ViewState["ScriptDirectory"] = value; }		
		}


		#endregion

		#region Protected/Private Methods

		// override the standard span and use a div
		protected override HtmlTextWriterTag TagKey
		{
			get {return HtmlTextWriterTag.Div;}
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		protected override void Render(HtmlTextWriter writer)
		{
			if (this.Site != null && this.Site.DesignMode)
			{	// render in the designer
				this.txtUserInput.RenderControl(writer);
				writer.Write("[" + this.ID + "]");
			}
			else
			{	// render in http response
				base.Render(writer);
			}
		}

	
		protected override void OnPreRender(EventArgs e)
		{
			// this is needed because browser caps doesn't think FireFox is uplevel
			// this control doesn't currently downgrade gracefully
			this.Page.ClientTarget = "uplevel";
            this.Attributes.Add("style", "overflow: visible; position: relative; clear:left; ");
			SetupStyles();
			SetupScripts();
			base.OnPreRender (e);

		}

		protected override void CreateChildControls()
		{
			this.txtUserInput = new TextBox();
			this.btnCombo = new HtmlButton();
			this.btnCombo.Attributes.Add("tabIndex","-1");
			Literal beginNoBreak = new Literal();
			beginNoBreak.Text = "<nobr>";
			Literal endNoBreak = new Literal();
			endNoBreak.Text = "</nobr>";
			this.Controls.Add(beginNoBreak);
			this.Controls.Add(this.txtUserInput);
			this.Controls.Add(this.btnCombo);
			this.Controls.Add(endNoBreak);
			this.divMain = new HtmlGenericControl("div");
            this.Controls.Add(this.divMain);
            //divWrapper = new HtmlGenericControl("div");
            //this.Controls.Add(this.divWrapper);
            //divWrapper.Attributes.Add("style", "clear:left;");
            //divWrapper.Controls.Add(this.divMain);
			this.ddSmartDropdown = new HtmlSelect();
			this.ddSmartDropdown.ID = this.ddSmartDropdown.ClientID;
			this.divMain.ID = this.divMain.ClientID;
			this.divMain.Controls.Add(this.ddSmartDropdown);
			this.divMain.Attributes.Add("name", this.divMain.ClientID);
			this.ddSmartDropdown.Attributes.Add("tabIndex","-1");
			this.ddSmartDropdown.Attributes.Add("size","10");
			this.ddSmartDropdown.Attributes.Add("name",this.ddSmartDropdown.ID);
			Literal spacer = new Literal();
			spacer.Text = "&nbsp;&nbsp;";
			this.Controls.Add(spacer);
			this.lblValueLabel = new Label();
			this.lblValueLabel.ID = this.lblValueLabel.ClientID;
			this.Controls.Add(this.lblValueLabel);
			Literal singleSpace = new Literal();
			singleSpace.Text = "&nbsp;";
			this.Controls.Add(singleSpace);
			this.txtValue = new TextBox();
			this.Controls.Add(this.txtValue);
			this.txtValue.ID = this.txtValue.ClientID;
			this.txtValue.Attributes.Add("tabIndex","-1");
			this.txtValue.Attributes.Add("onfocus", "this.blur();");
			
		}

		private void SetupStyles()
		{
			EnsureChildControls();
			this.btnCombo.Attributes.Add("style",this.ButtonStyle);

			if(this.ButtonImageUrl.Length > 0)
			{
				this.btnCombo.InnerHtml = "<img style=\"vertical-align: middle\" src=\"" + this.ButtonImageUrl + "\" />&nbsp;";
			}
			else
			{
				if(this.ButtonText.Length > 0)
				{
					this.btnCombo.InnerText = this.ButtonText;
				}
				else
				{
					this.btnCombo.InnerText = "V";
				}
			}
			if(!this.ShowValueField)
			{
				this.lblValueLabel.Visible = false;
				this.txtValue.Attributes.Add("style","visibility:hidden;");
			}

			// TODO: could expose some of this for custom styling
            this.divMain.Attributes.Add("style", "border-right: gray 2px solid; border-top: gray 2px solid; z-index: 1;  visibility: hidden; border-left: gray 2px solid; border-bottom: gray 2px solid; position: absolute; top: 20px; left: 0px; background-color: white;");
		
			this.ddSmartDropdown.Attributes.Add("style","visibility: hidden;");

		
		}

		private void SetupScripts()
		{

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa", "<script src=\""
                + ResolveUrl(this.ScriptDirectory + "/sarissa/sarissa.js") + "\" type=\"text/javascript\"></script>");

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa_ieemu_xpath", "<script src=\""
                + ResolveUrl(this.ScriptDirectory + "/sarissa/sarissa_ieemu_xpath.js") + "\" type=\"text/javascript\"></script>");


            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SmartCombo", "<script src=\""
                + ResolveUrl(this.ScriptDirectory + "/SmartCombo.js") + "\" type=\"text/javascript\"></script>");

			string hookupInputScript = "<script type=\"text/javascript\">" 
				+ "new SmartCombo( " 
				+ "document.getElementById('" + this.txtUserInput.ClientID + "'),  "
				+ "document.getElementById('" + this.txtValue.ClientID + "'), "
				+ "document.getElementById('" +  this.divMain.ClientID + "'), "
				+ "document.getElementById('" + this.btnCombo.ClientID + "'), "
				+ "document.getElementById('" + this.ddSmartDropdown.ClientID + "'), "
				+ "\"" + this.DataUrl + "\""
				+ ");</script>";
				

			this.Page.ClientScript.RegisterStartupScript(this.GetType(),this.UniqueID, hookupInputScript);

		}

		#endregion

	}
}
