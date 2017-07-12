using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace mojoPortal.Web.Controls
{
	/// <summary>
    /// http://www.codeproject.com/aspnet/graphicalcontrols.asp
	/// Graphical Asp.Net checkbox (with customizable images)
	/// </summary>
	[ToolboxData("<{0}:GraphicalCheckBox runat=\"server\"></{0}:GraphicalCheckBox>")]
	public class GraphicalCheckBox : System.Web.UI.WebControls.WebControl,
		IPostBackDataHandler
	{
		#region Local variables

		private bool _autopost=false,_globalstg=false,_threestate=false;
		private string checkImg,uncheckImg,checkImgOver="",uncheckImgOver="",
			checkImgDis,uncheckImgDis,title="",text="",indetImg,indetImgOver;

		#endregion
		#region Public properties

		/// <summary>
		/// Load global graphical control settings from Application?
		/// </summary>
		[Category("Behavior"),Description("Should control load global graphical control settings from Application?"),DefaultValue(false)]
		public bool LoadGlobalSettings
		{
			get { return _globalstg; }
			set { _globalstg=value; }
		}


		/// <summary>
		/// Automaticly post-back when control is clicked
		/// </summary>
		[Category("Behavior"),Description("Automaticly post-back when control is clicked."),DefaultValue(false)]
		public bool AutoPostBack
		{
			get { return _autopost; }
			set { _autopost=value; }
		}


		/// <summary>
		/// Is checkbox checked ?
		/// </summary>
		[Category("Misc"),Description("Is checkbox checked?"),DefaultValue(false)]
		public bool Checked
		{
			get { return CheckState!=CheckState.Unchecked; }
			set {	CheckState=value?CheckState.Checked:CheckState.Unchecked;	}
		}


		/// <summary>
		/// Gets or sets a value indicating whether the check box will allow three check states rather than two.
		/// </summary>
		[Category("Behavior"),DefaultValue(false),
		Description("Gets or sets a value indicating whether the check box will allow three check states rather than two.")]
		public bool ThreeState
		{
			get { return _threestate; }
			set { _threestate=value; }
		}

		
		/// <summary>
		/// State of checkbox (Can be checked, unchecked and Indeterminate).
		/// </summary>
		[Category("Misc"),Description("State of checkbox (Can be checked, unchecked and Indeterminate)."),DefaultValue(CheckState.Unchecked)]
		public CheckState CheckState
		{
			get 
			{
				if (ViewState["check"]==null) ViewState["check"]=0;
				return (CheckState)((int)ViewState["check"]); 
			}
			set 
			{
				ViewState["check"]=(int)value;	
			}
		}


		/// <summary>
		/// Custom image url for checked checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for checked checkbox")]
		public string CheckedImg
		{
			get { return checkImg; }
			set { checkImg=value; }
		}


		/// <summary>
		/// Custom image url for indeterminate state of checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for indeterminate state of checkbox")]
		public string IndeterminateImg
		{
			get { return indetImg; }
			set { indetImg=value; }
		}


		/// <summary>
		/// Custom image url for indeterminate state of active checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for indeterminate state of active checkbox")]
		public string IndeterminateOverImg
		{
			get { return indetImgOver; }
			set { indetImgOver=value; }
		}


		/// <summary>
		/// Custom image url for unchecked checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for unchecked checkbox")]
		public string UncheckedImg
		{
			get { return uncheckImg; }
			set { uncheckImg=value; }
		}


		/// <summary>
		/// Custom image url for checked checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for checked active checkbox")]
		public string CheckedOverImg
		{
			get { return checkImgOver; }
			set { checkImgOver=value; }
		}


		/// <summary>
		/// Custom image url for unchecked checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for unchecked active checkbox")]
		public string UncheckedOverImg
		{
			get { return uncheckImgOver; }
			set { uncheckImgOver=value; }
		}


		/// <summary>
		/// Custom image url for checked disabled checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for checked disabled checkbox")]
		public string CheckedDisImg
		{
			get { return checkImgDis; }
			set { checkImgDis=value; }
		}


		/// <summary>
		/// Custom image url for unchecked disabled checkbox
		/// </summary>
		[Category("Appearance"),Description("Custom image url for unchecked disabled checkbox")]
		public string UncheckedDisImg
		{
			get { return uncheckImgDis; }
			set { uncheckImgDis=value; }
		}


		/// <summary>
		/// Checkbox title (Used as alt attribute for image)
		/// </summary>
		[Category("Appearance"),Description("Checkbox title (Used as alt attribute for image)")]
		public string Title
		{
			get { return title; }
			set { title=value; }
		}


		/// <summary>
		/// Checkbox text
		/// </summary>
		[Category("Appearance"),Description("Checkbox text")]
		public string Text
		{
			get { return text; }
			set { text=value; }
		}

		#endregion
		#region Overriden

		/// <summary> 
		/// Render hidden input control and img element
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			if (_globalstg&&GraphicalControlsSettings.Load()!=null)
			{
				GraphicalControlsSettings stg=
					GraphicalControlsSettings.Load();

				if (stg.CheckboxCheckedDisImg!=null) CheckedDisImg=stg.CheckboxCheckedDisImg;
				if (stg.CheckboxCheckedImg!=null) CheckedImg=stg.CheckboxCheckedImg;
				if (stg.CheckboxCheckedOverImg!=null) CheckedOverImg=stg.CheckboxCheckedOverImg;
				if (stg.CheckboxUncheckedDisImg!=null) UncheckedDisImg=stg.CheckboxUncheckedDisImg;
				if (stg.CheckboxUncheckedImg!=null) UncheckedImg=stg.CheckboxUncheckedImg;
				if (stg.CheckboxUncheckedOverImg!=null) UncheckedOverImg=stg.CheckboxUncheckedOverImg;
				if (stg.CheckboxIndeterminateImg!=null) IndeterminateImg=stg.CheckboxIndeterminateImg;
				if (stg.CheckboxIndeterminateOverImg!=null) IndeterminateOverImg=stg.CheckboxIndeterminateOverImg;
			}

			
			string st=this.Attributes["style"];
			if (st==null) st=""; if (st.Length!=0) st+=";";
			StringBuilder sb=new StringBuilder(256);
			base.RenderBeginTag(new HtmlTextWriter(new System.IO.StringWriter(sb)));
			string tmp=Regex.Match(sb.ToString(),@"style=\""[^\""]*\"""
				).Value; 
            // 2007-09-29  added length check here to fix error
            if(tmp.Length >=8)st+=tmp.Substring(7,tmp.Length-8);

			if (Enabled)
			{
				string pb="";
				if (_autopost)
					pb=Page.ClientScript.GetPostBackEventReference(this,"");

				output.Write("<input type=\"hidden\" id=\"{0}\" name=\"{0}\""+
					" value=\"{1}\" />",UniqueID,((int)CheckState).ToString());
				output.Write("<span ");

				/***** FIRST - if control has all over states set generate mouseover and mouseout *****/
				bool bAllowOver=checkImgOver!=""&&uncheckImgOver!=""&&(!_threestate||indetImgOver!="");
				if (bAllowOver)
				{
					if (ThreeState) // THREE STATE CHECKBOX
					{
						output.Write(
							" onmouseover=\"var el=document.getElementById('{0}'); "+
							"var im=document.getElementById('{1}');"+
							"if (el.value==0) im.src='{5}'; else if (el.value==1) im.src='{6}'; else im.src='{7}';\""+
							" onmouseout=\"var el=document.getElementById('{0}'); "+
							"var im=document.getElementById('{1}');"+
							"if (el.value==0) im.src='{2}'; else if (el.value==1) im.src='{3}'; else im.src='{4}';\"",
							UniqueID,UniqueID+"_img",uncheckImg,checkImg,indetImg,
							uncheckImgOver,checkImgOver,indetImgOver);
					}
					else         // TWO STATE CHECKBOX
					{
						output.Write(
							" onmouseover=\"var el=document.getElementById('{0}'); "+
							"var im=document.getElementById('{1}');"+
							"im.src=(el.value==1)?'{4}':'{5}';\""+					
							" onmouseout=\"var el=document.getElementById('{0}'); "+
							"var im=document.getElementById('{1}');"+
							"im.src=(el.value==1)?'{3}':'{2}';\"",
							UniqueID,UniqueID+"_img",uncheckImg,checkImg,
							checkImgOver,uncheckImgOver);
					}
				}

				/***** SECOND - generate click event handler *****/
				if (ThreeState) // THREE STATE CHECKBOX
				{
					string currentImg=uncheckImg;
					if (CheckState==CheckState.Checked) currentImg=checkImg;
					if (CheckState==CheckState.Indeterminate) currentImg=indetImg;

					output.Write(
						" onclick=\"var el=document.getElementById('{0}'); "+
						"var im=document.getElementById('{1}');"+
						"if (el.value==1) {{ el.value=2; im.src='{4}'; }} else"+
						" if (el.value==2) {{ el.value=0; im.src='{2}'; }} else"+
						" {{ el.value=1; im.src='{3}'; }} {8}\""+
						" style=\"cursor:default;{9}\">"+
						"<img id=\"{1}\" src=\"{5}\" alt=\"{6}\" /> {7}</span>",
						UniqueID,UniqueID+"_img",bAllowOver?uncheckImgOver:uncheckImg,
						bAllowOver?checkImgOver:checkImg,bAllowOver?indetImgOver:indetImg,
						currentImg,title,text,pb,st);

				}
				else         // TWO STATE CHECKBOX
				{
					output.Write(
						" onclick=\"var el=document.getElementById('{0}'); "+
						"var im=document.getElementById('{1}');"+
						"if (el.value==1) {{ el.value=0; im.src='{2}'; }} else"+
						" {{ el.value=1; im.src='{3}'; }} {7}\""+
						" style=\"cursor:default;{8}\">"+
						"<img id=\"{1}\" src=\"{4}\" alt=\"{5}\" /> {6}</span>",
						UniqueID,UniqueID+"_img",bAllowOver?uncheckImgOver:uncheckImg,
						bAllowOver?checkImgOver:checkImg,
						Checked?checkImg:uncheckImg,title,text,pb,st);
				}
			}
			else
			{
				output.Write(
					"<span disabled=\"disabled\" style=\"cursor:default;{4}\">"+
					"<img id=\"{0}\" src=\"{1}\" alt=\"{2}\" /> {3}</span>",
					UniqueID+"_img",Checked?checkImgDis:uncheckImgDis,title,text,st);
			}
		}


		/// <summary>
		/// Raise checked-changed event
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
			OnCheckedChanged(EventArgs.Empty);
		}


		/// <summary>
		/// Load data from POST (load checked from hidden input)
		/// </summary>
		/// <param name="postDataKey">Key</param>
		/// <param name="postCollection">Data</param>
		/// <returns>True when value changed</returns>
		public bool LoadPostData(string postDataKey,
			System.Collections.Specialized.NameValueCollection postCollection)
		{
			string postedValue=postCollection[postDataKey];
			int iPost=Int32.Parse(postedValue);

			if (ViewState["check"]==null) Checked=false;
			if (iPost!=(int)ViewState["check"]) 
			{
				CheckState=(CheckState)iPost;
				return true;
			}
			CheckState=(CheckState)iPost;
			return false;
		}


		/// <summary>
		/// Raise check-changed event
		/// </summary>
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			if (CheckedChanged!=null)
				CheckedChanged(this,e);
		}

		#endregion
		#region Events

		/// <summary>
		/// Is raised when value is changed
		/// </summary>
		[Category("Action"),Description("Value changed")]
		public event EventHandler CheckedChanged;

		#endregion
	}


	/// <summary>
	/// State of checkbox (for three-state checkbox)
	/// </summary>
	public enum CheckState
	{
		#region Members

		/// <summary> The control is unchecked. </summary>
		Unchecked=0,
		/// <summary> The control is checked. </summary>
		Checked=1,
		/// <summary> The control is indeterminate. 
		/// An indeterminate control generally has a shaded appearance. </summary>
		Indeterminate=2,

		#endregion
	}
}