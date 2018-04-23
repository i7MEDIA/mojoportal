// Author:					
// Created:					2009-04-02
// Last Modified:			2016-02-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Editor
{
	public class CKEditorControl : TextBox
	{
		//http://simonbartlett.co.uk/2010/05/ckeditor-and-asp-net/


		private string siteRoot = "~/";
		private string basePath = WebConfigSettings.CKEditorBasePath;
		private string customConfigPath = WebConfigSettings.CKEditorConfigPath;

		private Direction textDirection = Direction.LeftToRight;
		private ToolBar toolBar = ToolBar.AnonymousUser;
		private string editorCSSUrl = string.Empty;
		private bool fullPageMode = false;
		private string templatesJsonUrl = string.Empty;
		private string stylesJsonUrl = string.Empty;

		// kama, v2, office2003
		private string skin = "kama";

		private bool htmlEncode = false;

		public override string Text
		{
			get
			{
				if (htmlEncode && (!string.IsNullOrEmpty(base.Text)))
					base.Text = base.Text.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
				return base.Text;
			}
			set
			{
				if (htmlEncode && (!string.IsNullOrEmpty(value)))
					value = value.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
				base.Text = value;
			}
		}

		public string CustomConfigPath
		{
			get { return customConfigPath; }
			set { customConfigPath = value; }
		}


		public string BasePath
		{
			get { return basePath; }
			set { basePath = value; }
		}

		public string SiteRoot
		{
			get { return siteRoot; }
			set { siteRoot = value; }
		}

		public string StylesJsonUrl
		{
			get { return stylesJsonUrl; }
			set { stylesJsonUrl = value; }
		}

		private string templates = string.Empty;
		public string Templates
		{
			get { return templates; }
			set { templates = value; }
		}

		public string TemplatesJsonUrl
		{
			get { return templatesJsonUrl; }
			set { templatesJsonUrl = value; }
		}

		public string Skin
		{
			get { return skin; }
			set { skin = value; }
		}

		public ToolBar ToolBar
		{
			get { return toolBar; }
			set { toolBar = value; }
		}

		public Direction TextDirection
		{
			get { return textDirection; }
			set { textDirection = value; }
		}

		public string EditorCSSUrl
		{
			get { return editorCSSUrl; }
			set { editorCSSUrl = value; }
		}

		public bool FullPageMode
		{
			get { return fullPageMode; }
			set { fullPageMode = value; }
		}

		private bool enableFileBrowser = false;

		public bool EnableFileBrowser
		{
			get { return enableFileBrowser; }
			set { enableFileBrowser = value; }
		}

		private bool forcePasteAsPlainText = false;

		public bool ForcePasteAsPlainText
		{
			get { return forcePasteAsPlainText; }
			set { forcePasteAsPlainText = value; }
		}

		private string fileManagerUrl = string.Empty;
		public string FileManagerUrl
		{
			get { return fileManagerUrl; }
			set { fileManagerUrl = value; }
		}

		private string dropFileUploadUrl = string.Empty;
		public string DropFileUploadUrl
		{
			get { return dropFileUploadUrl; }
			set { dropFileUploadUrl = value; }
		}

		private bool entityEncode = false;

		public bool EntityEncode
		{
			get { return entityEncode; }
			set { entityEncode = value; }
		}

		private bool useGoodbyePrompt = true;

		public bool UseGoodbyePrompt
		{
			get { return useGoodbyePrompt; }
			set { useGoodbyePrompt = value; }
		}

		private bool disableViewState = true;
		public bool DisableViewState
		{
			get { return disableViewState; }
			set { disableViewState = value; }
		}

		private bool autoFocus = false;

		public bool AutoFocus
		{
			get { return autoFocus; }
			set { autoFocus = value; }
		}

		private string editorBodyCssClass = "wysiwygeditor modulecontent art-postcontent";

		public string EditorBodyCssClass
		{
			get { return editorBodyCssClass; }
			set { editorBodyCssClass = value; }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			TextMode = TextBoxMode.MultiLine;
			Rows = 10;
			Columns = 70;

			htmlEncode = WebConfigSettings.CKeditorEncodeBrackets;

			if (siteRoot.StartsWith("~/"))
			{
				siteRoot = ResolveUrl(siteRoot);
			}

			if (disableViewState)
			{
				EnableViewState = false;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			SetupScripts();
		}

		private string GetLanguageCode(CultureInfo culture)
		{
			switch (culture.Name)
			{
				case "en-AU":
					return "en-au";

				case "en-CA":
					return "en-ca";

				case "en-GB":
					return "en-gb";

				case "fr-CA":
					return "fr-ca";

				case "pt-BR":
					return "pr-br";

				case "zh-CN":
					return "zh-cn";
			}

			return culture.TwoLetterISOLanguageName;
		}

		private void SetupScripts()
		{
			ScriptManager.RegisterClientScriptBlock(
				this,
				this.GetType(),
				"ckeditormain",
				"\n<script type=\"text/javascript\" src=\"" + ResolveUrl(this.BasePath + "ckeditor.js") + "\"></script>",
				false
			);

			StringBuilder script = new StringBuilder();
			script.Append("\n<script type=\"text/javascript\">");
			script.Append("var editor" + this.ClientID + " = CKEDITOR.replace('" + this.ClientID + "'");
			script.Append(", { ");
			script.Append("customConfig : '" + ResolveUrl(customConfigPath) + "' ");
			script.Append(", baseHref : '" + siteRoot + "'");

			if (Height != Unit.Empty)
			{
				script.Append(", height : " + this.Height.ToString().Replace("px", string.Empty));
			}
			else
			{
				script.Append(", height : 350");
			}

			if (autoFocus)
			{
				script.Append(",startupFocus : true");
			}

			script.Append(",skin:'" + skin + "'");
			script.Append(",editorId:'" + ClientID + "'");

			CultureInfo culture;
			if (WebConfigSettings.UseCultureOverride)
			{
				culture = SiteUtils.GetDefaultUICulture();
			}
			else
			{
				culture = CultureInfo.CurrentUICulture;
			}

			if (WebConfigSettings.CKeditorSuppressTitle)
			{
				script.Append(",title:false");
			}

			script.Append(",language:'" + GetLanguageCode(culture) + "'");

			if ((textDirection == Direction.RightToLeft) || (culture.TextInfo.IsRightToLeft))
			{
				script.Append(", contentsLangDirection : 'rtl'");
			}

			if (editorCSSUrl.Length > 0)
			{
				script.Append(", contentsCss : '" + editorCSSUrl + "'");
			}

			script.Append(",bodyClass:'" + editorBodyCssClass + "' ");

			if ((enableFileBrowser) && (fileManagerUrl.Length > 0))
			{
				//script.Append(",filebrowserWindowWidth : 860");
				//script.Append(",filebrowserWindowHeight : 700");
				//script.Append(",filebrowserBrowseUrl:'" + fileManagerUrl + "?ed=ck&type=file' ");
				//script.Append(",filebrowserImageBrowseUrl:'" + fileManagerUrl + "?ed=ck&type=image' ");
				//script.Append(",filebrowserFlashBrowseUrl:'" + fileManagerUrl + "?ed=ck&type=media' ");
				//script.Append(",filebrowserImageBrowseLinkUrl:'" + fileManagerUrl + "?ed=ck&type=file' ");
				//script.Append(",filebrowserWindowFeatures:'location=no,menubar=no,toolbar=no,dependent=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=yes,scrollbars=yes'");

				script.Append(",filebrowserWindowWidth : ~~((80 / 100) * screen.width)"); // 80% of window width
				script.Append(",filebrowserWindowHeight : ~~((80 / 100) * screen.height)"); // 80% of window height
				script.Append(",filebrowserBrowseUrl:'" + fileManagerUrl + "?editor=ckeditor&type=file'");
				script.Append(",filebrowserImageBrowseUrl:'" + fileManagerUrl + "?editor=ckeditor&type=image'");
				script.Append(",filebrowserFlashBrowseUrl:'" + fileManagerUrl + "?editor=ckeditorck&type=media'");
				script.Append(",filebrowserImageBrowseLinkUrl:'" + fileManagerUrl + "?editor=ckeditor&type=file'");
				script.Append(",filebrowserWindowFeatures:'location=no,menubar=no,toolbar=no,dependent=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=yes,scrollbars=yes'");

				if (dropFileUploadUrl.Length > 0)
				{
					script.Append(",dropFileUploadUrl:'" + Page.ResolveUrl(dropFileUploadUrl) + "'");
				}
			}

			// script.Append(",ignoreEmptyParagraph:true");

			if (forcePasteAsPlainText)
			{
				script.Append(",forcePasteAsPlainText:true");
			}

			//if (!entityEncode)
			//{
			//    script.Append(",entities:false");
			//}

			if (htmlEncode)
			{
				script.Append(",htmlEncodeOutput:true");
			}

			if (fullPageMode)
			{
				script.Append(",fullPage : true ");
			}

			SetupToolBar(script);

			script.Append("}");
			script.Append("); ");
			script.Append("function SetupEditor" + this.ClientID + "( editorObj){");

			if (stylesJsonUrl.Length > 0)
			{
				script.Append("editorObj.config.stylesCombo_stylesSet = 'mojo:" + stylesJsonUrl + "';");
			}

			if (templatesJsonUrl.Length > 0)
			{
				script.Append("editorObj.config.templates = 'mojo';");
				script.Append("editorObj.config.templates_files = ['" + templatesJsonUrl + "'];");
				script.Append("editorObj.config.templates_replaceContent = false;");
			}

			script.Append("editorObj.config.smiley_path = '" + Page.ResolveUrl("~/Data/SiteImages/emoticons/") + "';");
			script.Append("editorObj.config.extraPlugins='onchange'; ");

			if (useGoodbyePrompt)
			{
				script.Append("editorObj.on( 'change', function(e) {");
				//script.Append("hookupGoodbyePrompt(\"" + Page.Server.HtmlEncode(Resource.UnSavedChangesPrompt) + "\"); ");
				script.Append(" if (editorObj.checkDirty()){");
				script.Append("hookupGoodbyePrompt(\"" + Resource.UnSavedChangesPrompt.HtmlEscapeQuotes().RemoveLineBreaks() + "\"); ");
				script.Append("} ");
				script.Append(" }); ");
			}

			script.Append("}");
			script.Append("SetupEditor" + this.ClientID + "(editor" + this.ClientID + ");");
			script.Append("</script>");

			ScriptManager.RegisterStartupScript(
				this,
				this.GetType(),
				this.UniqueID,
				script.ToString(),
				false
			);

			//this will help the editor work in updatepanel
			ScriptManager.RegisterOnSubmitStatement(this, this.GetType(), "updateditor" + this.ClientID, "CKEDITOR.instances['" + this.ClientID + "'].updateElement();");
		}

		private void SetupToolBar(StringBuilder script)
		{

			//'basicstyles,blockquote,button,clipboard,colorbutton,contextmenu,elementspath,enterkey,entities,find,flash,font,format,forms,horizontalrule,
			//htmldataprocessor,image,indent,justify,keystrokes,link,list,newpage,pagebreak,pastefromword,pastetext,preview,print,removeformat,save,smiley,showblocks,
			//sourcearea,stylescombo,table,specialchar,tab,templates,toolbar,undo,wysiwygarea,wsc';

			switch (toolBar)
			{
				case ToolBar.FullWithTemplates:
					script.Append(",toolbar:'FullWithTemplates'");
					break;

				case ToolBar.Full:
					script.Append(",toolbar:'Full'");
					break;

				case ToolBar.Newsletter:
					script.Append(",toolbar:'Newsletter'");
					break;

				case ToolBar.Forum:
					script.Append(",toolbar:'Forum'");
					break;

				case ToolBar.ForumWithImages:
					script.Append(",toolbar:'ForumWithImages'");
					break;

				case ToolBar.SimpleWithSource:
					script.Append(",toolbar:'SimpleWithSource'");
					break;

				case ToolBar.AnonymousUser:
				default:
					script.Append(",toolbar:'AnonymousUser'");
					break;

				case ToolBar.Custom1:
					script.Append(",toolbar:'Custom1'");
					break;

				case ToolBar.Custom2:
					script.Append(",toolbar:'Custom2'");
					break;
			}
		}
	}
}
