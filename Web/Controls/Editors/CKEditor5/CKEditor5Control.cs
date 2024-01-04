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
	public class CKEditor5Control : TextBox
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

		public string CustomConfigPath { get; set; } = WebConfigSettings.CKEditor5ConfigPath;


		public string BasePath { get; set; } = WebConfigSettings.CKEditor5BasePath;

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

			htmlEncode = WebConfigSettings.CKeditor5EncodeBrackets;

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
				"ckeditor5main",
				"\n<script data-loader=\"ckeditor5control\" src=\"" + ResolveUrl(this.BasePath + "ckeditor.js") + "\"></script>",
				false
			);

			StringBuilder script = new StringBuilder();
			script.Append("\n<script data-loader=\"ckeditor5control\">");
			//script.Append("var editor" + this.ClientID + " = CKEDITOR5.replace('" + this.ClientID + "'");

			script.Append("ClassicEditor\n");
			script.Append("   .create( document.querySelector( '#" + this.ClientID + "' ), {\n");
			script.Append("   // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]\n");
			script.Append("   } )\n");
			script.Append("   .then(editor => {\n");
			script.Append("   window.editor = editor;\n");
			script.Append("   } )\n");
			script.Append("   .catch(err => {\n");
			script.Append("   console.error(err.stack );\n");
			script.Append("   } );\n");
			script.Append(" </script> ");



			ScriptManager.RegisterStartupScript(
				this,
				this.GetType(),
				this.UniqueID,
				script.ToString(),
				false
			);

			//this will help the editor work in updatepanel
			ScriptManager.RegisterOnSubmitStatement(this, this.GetType(), "updateditor" + this.ClientID, "CKEDITOR5.instances['" + this.ClientID + "'].updateElement();");
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
