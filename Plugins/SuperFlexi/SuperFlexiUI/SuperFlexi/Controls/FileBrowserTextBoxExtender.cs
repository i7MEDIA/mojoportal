using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;

namespace SuperFlexiUI;

public class FileBrowserTextBoxExtender : HyperLink
{
	private SiteSettings siteSettings = null;
	private bool canBrowse = false;

	/// <summary>
	/// allows browsing files and pages if the user is in a role that can browse and upload, soy uo could link to a page, pdf, zip, etc
	/// valid options are folder, file, image, and media
	/// </summary>
	public string BrowserType { get; set; } = "file";

	public string TextBoxClientId { get; set; } = string.Empty;

	public string PreviewImageClientId { get; set; } = string.Empty;

	public bool PreviewImageOnBlur { get; set; } = true;

	public string EmptyImageUrl { get; set; } = "~/Data/SiteImages/1x1.gif";

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);

		CssClass += " cblink";

		siteSettings = CacheHelper.GetCurrentSiteSettings();
		if (siteSettings == null) { Visible = false; return; }

		if ((WebUser.IsAdminOrContentAdmin) || (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles)) || (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)))
		{
			canBrowse = true;
			if (Page is mojoBasePage basePage)
			{
				basePage.ScriptConfig.IncludeColorBox = true;
			}
		}

		if (!canBrowse)
		{
			Visible = false;
			return;
		}

		SetupLink();
	}

	private void SetupLink()
	{
		NavigateUrl = $"{SiteUtils.GetNavigationSiteRoot()}{WebConfigSettings.FileDialogRelativeUrl}?type={BrowserType}&tbi={TextBoxClientId}";

		var script = new StringBuilder();
		script.Append("\n<script type=\"text/javascript\">");

		script.Append("\nfunction SetUrl (URL, clientId) {");
		script.Append("var txtUrl = document.getElementById(clientId); ");
		script.Append("txtUrl.value = URL;");
		script.Append("$('#' + clientId).blur();");
		script.Append("$.colorbox.close(); ");
		script.Append("}");
		script.Append("function ChangeImagePreview (imgPrevId, url) {");
		script.Append("var imgPrev = document.getElementById(imgPrevId); ");
		script.Append("imgPrev.src = url;");
		script.Append("}");
		script.Append("\n</script>");

		ScriptManager.RegisterClientScriptBlock(this,
			typeof(Page),
			"SetUrl",
			script.ToString(), false);

		if (PreviewImageOnBlur && (TextBoxClientId.Length > 0) && (BrowserType == "image") && (PreviewImageClientId.Length > 0))
		{
			script = new StringBuilder();
			script.Append("<script type=\"text/javascript\">");
			script.Append("$(document).ready(function () {");
			script.Append($"$('#{TextBoxClientId}').blur(function () {{ ");
			script.Append($"var url = '{Page.ResolveUrl(EmptyImageUrl)}';");
			script.Append($"if($('#{TextBoxClientId}').val().length) {{");
			script.Append($"url = $('#{TextBoxClientId}').val();");
			script.Append("}");
			script.Append($"ChangeImagePreview('{PreviewImageClientId}', url);");
			script.Append("});});</script>");

			ScriptManager.RegisterStartupScript(this,
				typeof(Page),
				"imgprevblur_" + TextBoxClientId.ToString(),
				script.ToString(), false);
		}
	}
}