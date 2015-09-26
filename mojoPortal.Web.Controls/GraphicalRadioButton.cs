using System;
using System.Text;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace mojoPortal.Web.Controls
{
	/// <summary>
    /// http://www.codeproject.com/aspnet/graphicalcontrols.asp
    /// 
	/// Graphical Asp.Net checkbox (with customizable images)
	/// </summary>
	[ToolboxData("<{0}:GraphicalRadioButton runat=\"server\"></{0}:GraphicalRadioButton>")]
	public class GraphicalRadioButton : System.Web.UI.WebControls.WebControl,
		IPostBackDataHandler
	{
		#region Local variables

		private bool _autopost=false,_globalstg=false;
		private string checkImg,uncheckImg,group="",checkImgOver="",uncheckImgOver="",
			checkImgDis,uncheckImgDis,title="",text="";

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
		/// Is radiobutton checked ?
		/// </summary>
		[Category("Misc"),Description("Is radiobutton checked?"),DefaultValue(false)]
		public bool Checked
		{
			get 
			{
				if (ViewState["check"]==null) ViewState["check"]=false;
				return (bool)ViewState["check"]; 
			}
			set {	ViewState["check"]=value;	}
		}


		/// <summary>
		/// Custom image url for checked radiobutton
		/// </summary>
		[Category("Appearance"),Description("Custom image url for checked radiobutton")]
		public string CheckedImg
		{
			get { return checkImg; }
			set { checkImg=value; }
		}


		/// <summary>
		/// Custom image url for unchecked radiobutton
		/// </summary>
		[Category("Appearance"),Description("Custom image url for unchecked radiobutton")]
		public string UncheckedImg
		{
			get { return uncheckImg; }
			set { uncheckImg=value; }
		}


		/// <summary>
		/// Custom image url for checked radiobutton
		/// </summary>
		[Category("Appearance"),Description("Custom image url for checked active radiobutton")]
		public string CheckedOverImg
		{
			get { return checkImgOver; }
			set { checkImgOver=value; }
		}


		/// <summary>
		/// Custom image url for unchecked radiobutton
		/// </summary>
		[Category("Appearance"),Description("Custom image url for unchecked active radiobutton")]
		public string UncheckedOverImg
		{
			get { return uncheckImgOver; }
			set { uncheckImgOver=value; }
		}


		/// <summary>
		/// Custom image url for checked disabled radiobutton
		/// </summary>
		[Category("Appearance"),Description("Custom image url for checked disabled radiobutton")]
		public string CheckedDisImg
		{
			get { return checkImgDis; }
			set { checkImgDis=value; }
		}


		/// <summary>
		/// Custom image url for unchecked disabled radiobutton
		/// </summary>
		[Category("Appearance"),Description("Custom image url for unchecked disabled radiobutton")]
		public string UncheckedDisImg
		{
			get { return uncheckImgDis; }
			set { uncheckImgDis=value; }
		}


		/// <summary>
		/// Radiobutton title (Used as alt attribute for image)
		/// </summary>
		[Category("Appearance"),Description("Radiobutton title (Used as alt attribute for image)")]
		public string Title
		{
			get { return title; }
			set { title=value; }
		}


		/// <summary>
		/// Radiobutton text
		/// </summary>
		[Category("Appearance"),Description("Radiobutton text")]
		public string Text
		{
			get { return text; }
			set { text=value; }
		}

		/// <summary>
		/// Radio buttons group
		/// </summary>
		[Category("Misc"),Description("Radio buttons group")]
		public string Group
		{
			get { return group; }
			set { group=value; }
		}

		#endregion
		#region Overriden


		/// <summary>
		/// Register client script
		/// </summary>
		protected override void OnLoad(EventArgs e)
		{
			if (!Page.ClientScript.IsStartupScriptRegistered("RadioButtonInit_"+group))
			{
                Page.ClientScript.RegisterStartupScript(this.GetType(), "RadioButtonInit_" + group,
					string.Format(@"
<script type=""text/javascript"">
//<![CDATA[
	var radio{0}=null,tmp{0}=false;
	function radio{0}Sel(el) {{
		if (radio{0}!=null) {{ 
			tmp{0}=true; radio{0}.onclick(); tmp{0}=false;
			if (radio{0}.onmouseout!=null) radio{0}.onmouseout(); }}
		radio{0}=el;
	}}
//]]>
</script>
",group
					));
			}
			if (Checked)
			{
				Page.ClientScript.RegisterStartupScript(this.GetType(), UniqueID+"_check",
					string.Format(@"
<script type=""text/javascript"">
//<![CDATA[
	var old{1}_load=window.onload;
	window.onload=body{1}_load;
	function body{1}_load() {{
		if (old{1}_load!=null) old{1}_load();
		radio{0}=document.getElementById('{1}_span');
	}}
//]]>
</script>
",group,UniqueID
					));
			}
			base.OnLoad(e);
		}


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

				if (stg.RadioCheckedDisImg!=null) CheckedDisImg=stg.RadioCheckedDisImg;
				if (stg.RadioCheckedImg!=null) CheckedImg=stg.RadioCheckedImg;
				if (stg.RadioCheckedOverImg!=null) CheckedOverImg=stg.RadioCheckedOverImg;
				if (stg.RadioUncheckedDisImg!=null) UncheckedDisImg=stg.RadioUncheckedDisImg;
				if (stg.RadioUncheckedImg!=null) UncheckedImg=stg.RadioUncheckedImg;
				if (stg.RadioUncheckedOverImg!=null) UncheckedOverImg=stg.RadioUncheckedOverImg;
			}

			string st=this.Attributes["style"];
			if (st==null) st=""; if (st.Length!=0) st+=";";
			StringBuilder sb=new StringBuilder(256);
			base.RenderBeginTag(new HtmlTextWriter(new System.IO.StringWriter(sb)));
			string tmp=Regex.Match(sb.ToString(),@"style=\""[^\""]*\"""
				).Value; st+=tmp.Substring(7,tmp.Length-8);

			if (Enabled)
			{
				string pb="";
				if (_autopost)
					pb=Page.ClientScript.GetPostBackEventReference(this,"");

				output.Write("<input type=\"hidden\" id=\"{0}\" name=\"{0}\""+
					" value=\"{1}\" />",UniqueID,Checked?"1":"0");
				output.Write("<span ");

				bool bAllowOver=checkImgOver!=""&&uncheckImgOver!="";
				if (bAllowOver)
					output.Write(
						" onmouseover=\"var el=document.getElementById('{0}'); "+
						"var im=document.getElementById('{1}');"+
						"im.src=(el.value==1)?'{4}':'{5}';\""+					
						" onmouseout=\"var el=document.getElementById('{0}'); "+
						"var im=document.getElementById('{1}');"+
						"im.src=(el.value==1)?'{3}':'{2}';\"",
						UniqueID,UniqueID+"_img",uncheckImg,checkImg,
						checkImgOver,uncheckImgOver);
        output.Write(
					" onclick=\"var el=document.getElementById('{0}'); "+
					"var im=document.getElementById('{1}');"+
					"if (!tmp{9}&amp;&amp;el.value==1) return;"+
					"if (el.value==1) {{ el.value=0; im.src='{2}'; }} else"+
					" {{ el.value=1; im.src='{3}'; }} if (el.value==1) radio{9}Sel(this); "+
					"if (tmp{9}) return; {7}\""+
					" style=\"cursor:default;{8}\" id=\"{0}_span\">"+
					"<img id=\"{1}\" src=\"{4}\" alt=\"{5}\" /> {6}</span>",
					UniqueID,UniqueID+"_img",bAllowOver?uncheckImgOver:uncheckImg,
					bAllowOver?checkImgOver:checkImg,
					Checked?checkImg:uncheckImg,title,text,pb,st,group);
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
			bool bPost=postedValue=="1";

			if (ViewState["check"]==null) Checked=false;
			if (bPost!=(bool)ViewState["check"]) 
			{
				Checked=bPost;
				return true;
			}
			Checked=bPost;
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
}