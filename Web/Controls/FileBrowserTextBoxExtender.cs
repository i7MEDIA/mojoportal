//	Created:			    2011-02-25
//	Last Modified:		    2017-02-20  @Elijah Fowler
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.UI
{
	public class FileBrowserTextBoxExtender : HyperLink
	{
		private SiteSettings siteSettings = null;
		private bool canBrowse = false;
		private string browserType = "file"; // Allows browsing files and pages if the user is in a role that can browse and upload, so you could link to a page, pdf, zip, etc

		/// <summary>
		/// valid options are folder, file, image, and media
		/// </summary>
		public string BrowserType
		{
			get { return browserType; }
			set { browserType = value; }

		}

		private string textBoxClientId = string.Empty;
		public string TextBoxClientId
		{
			get { return textBoxClientId; }
			set { textBoxClientId = value; }
		}

		private string previewImageClientId = string.Empty;
		public string PreviewImageClientId
		{
			get { return previewImageClientId; }
			set { previewImageClientId = value; }
		}

		private bool previewImageOnBlur = true;
		public bool PreviewImageOnBlur
		{
			get { return previewImageOnBlur; }
			set { previewImageOnBlur = value; }
		}

		private string emptyImageUrl = "~/Data/SiteImages/1x1.gif";
		public string EmptyImageUrl
		{
			get { return emptyImageUrl; }
			set { emptyImageUrl = value; }
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			CssClass += " cblink"; // We need to create a theme.skin setting to override this and add custom attributes

			siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null) {
				Visible = false;
				return;
			}

			if ((WebUser.IsAdminOrContentAdmin) || (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles)) || (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)))
			{
				canBrowse = true;
				mojoBasePage basePage = Page as mojoBasePage;

				if (basePage != null) {
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
			NavigateUrl = 
				SiteUtils.GetNavigationSiteRoot() +
				WebConfigSettings.FileDialogRelativeUrl +
				"?editor=filepicker&type=" +
				browserType +
				"&inputId=" +
				textBoxClientId;

			string filePickerBase =
				@"<script>
					var filePicker = {{
						set: function(url, clientId) {{
							var _inputImg = document.getElementById(clientId); _inputImg.value = url;
							{0}
						}},

						close: function() {{
							$.colorbox.close();
						}}
					}};
				</script>";

			string imagePrevBase = @"var _inputPrev = document.getElementById('{0}'); _inputPrev.src = url;";
			string imagePrev = ((browserType == "image") && (previewImageClientId.Length > 0)) ?
				string.Format(imagePrevBase, (previewImageClientId.Length > 0) ? previewImageClientId : string.Empty) :
				string.Empty;

			string filePicker = string.Format(filePickerBase, imagePrev);

			ScriptManager.RegisterClientScriptBlock(
				this,
				typeof(Page),
				"SetUrl",
				filePicker,
				false
			);

			if (previewImageOnBlur && (textBoxClientId.Length > 0) && (browserType == "image") && (previewImageClientId.Length > 0))
			{
				string filePickerPreviewBase =
					@"<script>
						(function() {{
							var prevInput = document.getElementById('{0}');
							var prevImage = document.getElementById('{1}');

							if (prevInput !== null) {{
								prevInput.addEventListener('blur', function() {{
									if (prevInput.value.length > 0) {{
										prevImage.src = prevInput.value;
									}} else {{
										prevImage.src = '{2}';
									}}
								}});
							}}
						}})();
					</script>";

				string filePickerPreview = string.Format(filePickerPreviewBase, textBoxClientId, previewImageClientId, Page.ResolveUrl(emptyImageUrl));

				ScriptManager.RegisterStartupScript(
					this,
					typeof(Page),
					"FilePickerPreview",
					filePickerPreview,
					false
				);
			}
		}
	}
}