
using mojoPortal.Core.Configuration;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
	public class CKEditorControl : TextBox
	{
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

		public string CustomConfigPath { get; set; } = WebConfigSettings.CKEditorConfigPath;


		public string BasePath { get; set; } = WebConfigSettings.CKEditorBasePath;

		public string SiteRoot { get; set; } = "~/";

		public string StylesJsonUrl { get; set; } = string.Empty;

		public string Templates { get; set; } = string.Empty;

		public string TemplatesJsonUrl { get; set; } = string.Empty;

		public string SkinTemplatesUrl { get; set; } = string.Empty;

		public string MojoSkinPath { get; set; } = string.Empty;

		public string Skin { get; set; } = "moono-lisa";

		public ToolBar ToolBar { get; set; } = ToolBar.AnonymousUser;

		public Direction TextDirection { get; set; } = Direction.LeftToRight;

		public string EditorCSSUrl { get; set; } = string.Empty;

		public bool FullPageMode { get; set; } = false;

		public bool EnableFileBrowser { get; set; } = false;

		public bool ForcePasteAsPlainText { get; set; } = false;
		public string FileManagerUrl { get; set; } = string.Empty;
		public string DropFileUploadUrl { get; set; } = string.Empty;

		public bool EntityEncode { get; set; } = false;

		public bool UseGoodbyePrompt { get; set; } = true;
		public bool DisableViewState { get; set; } = true;

		public bool AutoFocus { get; set; } = false;

		public string EditorBodyCssClass { get; set; } = "wysiwygeditor modulecontent art-postcontent";

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			TextMode = TextBoxMode.MultiLine;
			Rows = 10;
			Columns = 70;

			htmlEncode = WebConfigSettings.CKeditorEncodeBrackets;

			if (SiteRoot.StartsWith("~/"))
			{
				SiteRoot = ResolveUrl(SiteRoot);
			}

			if (DisableViewState)
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
				"\n<script data-loader=\"ckeditorcontrol\" src=\"" + ResolveUrl(this.BasePath + "ckeditor.js") + "\"></script>",
				false
			);

			StringBuilder script = new StringBuilder();
			script.Append("\n<script data-loader=\"ckeditorcontrol\">");
			script.Append("var editor" + this.ClientID + " = CKEDITOR.replace('" + this.ClientID + "'");
			script.Append(", { ");
			script.Append("customConfig : '" + ResolveUrl(CustomConfigPath) + "' ");
			script.Append(", baseHref : '" + SiteRoot + "'");

			if (Height != Unit.Empty)
			{
				script.Append(", height : " + this.Height.ToString().Replace("px", string.Empty));
			}
			else
			{
				script.Append(", height : 350");
			}

			if (AutoFocus)
			{
				script.Append(",startupFocus : true");
			}

			script.Append(",skin:'" + Skin + "'");
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

			if ((TextDirection == Direction.RightToLeft) || (culture.TextInfo.IsRightToLeft))
			{
				script.Append(", contentsLangDirection : 'rtl'");
			}

			if (EditorCSSUrl.Length > 0)
			{
				script.Append(", contentsCss : '" + EditorCSSUrl + "'");
			}

			script.Append(",bodyClass:'" + EditorBodyCssClass + "' ");

			if ((EnableFileBrowser) && (FileManagerUrl.Length > 0))
			{
				script.Append(",filebrowserWindowWidth : ~~((80 / 100) * screen.width)"); // 80% of window width
				script.Append(",filebrowserWindowHeight : ~~((80 / 100) * screen.height)"); // 80% of window height
				script.Append(",filebrowserBrowseUrl:'" + FileManagerUrl + "?editor=ckeditor&type=file'");
				script.Append(",filebrowserImageBrowseUrl:'" + FileManagerUrl + "?editor=ckeditor&type=image'");
				//script.Append(",filebrowserFlashBrowseUrl:'" + FileManagerUrl + "?editor=ckeditorck&type=media'");
				script.Append(",filebrowserImageBrowseLinkUrl:'" + FileManagerUrl + "?editor=ckeditor&type=file'");
				script.Append(",filebrowserWindowFeatures:'location=no,menubar=no,toolbar=no,dependent=yes,minimizable=no,modal=yes,alwaysRaised=yes,resizable=yes,scrollbars=yes'");

				if (DropFileUploadUrl.Length > 0)
				{
					script.Append(",dropFileUploadUrl:'" + Page.ResolveUrl(DropFileUploadUrl) + "'");
				}
			}

			// script.Append(",ignoreEmptyParagraph:true");

			if (ForcePasteAsPlainText)
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

			if (FullPageMode)
			{
				script.Append(",fullPage : true ");
			}

			SetupToolBar(script);

			script.Append("}");
			script.Append("); ");
			script.Append("function SetupEditor" + this.ClientID + "( editorObj){");

			if (!string.IsNullOrWhiteSpace(StylesJsonUrl))
			{
				script.Append("editorObj.config.stylesCombo_stylesSet = 'mojo:" + StylesJsonUrl + "';");
			}

			if (!string.IsNullOrWhiteSpace(TemplatesJsonUrl) || !string.IsNullOrWhiteSpace(SkinTemplatesUrl))
			{
				var templates = string.Empty;
				var templatesFiles = string.Empty;
				if (!string.IsNullOrWhiteSpace(TemplatesJsonUrl))
				{
					templates = "mojo";
					templatesFiles = $"'{TemplatesJsonUrl}'";
				}

				if (!string.IsNullOrWhiteSpace(SkinTemplatesUrl))
				{
					templates += ",skin";
					templatesFiles += $",'{SkinTemplatesUrl}'";
				}

				//script.Append($"editorObj.config.templates = '{AppConfig.EditorTemplatesOrder}';");
				script.Append($"editorObj.config.templates_files = ['{TemplatesJsonUrl}'];");
				script.Append("editorObj.config.templates_replaceContent = false;");
			}

			script.Append("editorObj.config.smiley_path = '" + Page.ResolveUrl("~/Data/SiteImages/emoticons/") + "';");
			//script.Append("editorObj.config.extraPlugins='onchange'; ");

			if (UseGoodbyePrompt)
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
			script.Append($"var mojoSkinPath='{MojoSkinPath}';");

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
			switch (ToolBar)
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
