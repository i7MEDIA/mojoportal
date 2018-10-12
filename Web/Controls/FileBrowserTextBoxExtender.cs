//	Created:			    2011-02-25
//	Last Modified:		    2018-10-08  @Elijah Fowler
// 

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.UI
{
	public class FileBrowserTextBoxExtender : HyperLink
	{
		private SiteSettings siteSettings = null;
		private bool canBrowse = false;

		/// <summary>
		/// valid options are folder, file, image, and media
		/// </summary>
		public string BrowserType { get; set; } = "file";
		public string TextBoxClientId { get; set; } = string.Empty;
		public string PreviewImageClientId { get; set; } = string.Empty;
		public bool PreviewImageOnBlur { get; set; } = true;
		public string EmptyImageUrl { get; set; } = "~/Data/SiteImages/1x1.gif";
		public string Editor { get; set; } = "filepicker";
		public string StartFolder { get; set; } = string.Empty;
		public bool ReturnFullPath { get; set; } = true;


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
				$"?editor={Editor}&type={BrowserType}&inputId={TextBoxClientId}&startFolder={StartFolder}" +
				(!ReturnFullPath ? "&returnFullPath=false" : string.Empty);

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
			string imagePrev = ((BrowserType == "image") && (PreviewImageClientId.Length > 0)) ?
				string.Format(imagePrevBase, (PreviewImageClientId.Length > 0) ? PreviewImageClientId : string.Empty) :
				string.Empty;

			string filePicker = string.Format(filePickerBase, imagePrev);

			ScriptManager.RegisterClientScriptBlock(
				this,
				typeof(Page),
				"SetUrl",
				filePicker,
				false
			);

			if (PreviewImageOnBlur && (TextBoxClientId.Length > 0) && (BrowserType == "image") && (PreviewImageClientId.Length > 0))
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

				string filePickerPreview = string.Format(filePickerPreviewBase, TextBoxClientId, PreviewImageClientId, Page.ResolveUrl(EmptyImageUrl));

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