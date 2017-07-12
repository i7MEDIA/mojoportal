///	Created:			    2011-02-25
///	Last Modified:		    2015-04-20 (i7MEDIA)
///
/// 2015-04-20: Removed hard coded previewImageClientId from JS in SetUrl
///             Added textBoxClientId to name of blur script so more than one can be registered.
///             Changed SetUrl JS function to blur textbox after entry of url by browser. This should make the preview image update.
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;

namespace SuperFlexiUI
{
    public class FileBrowserTextBoxExtender : HyperLink
    {
        private SiteSettings siteSettings = null;
        private bool canBrowse = false;
        private string browserType = "file"; //allows browsing files and pages if the user is in a role that can browse and upload, soy uo could link to a page, pdf, zip, etc

        /// <summary>
        /// valid options are folder, file, image, and media
        /// </summary>
        public string BrowserType
        {
            get { return browserType; }
            set { browserType = value; }

        }
        //private string editorType = string.Empty;
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

        //private AttributeCollection attributes;
        //public AttributeCollection Attributes
        //{
        //    get { return attributes; }
        //    set { attributes = value; }
        //}
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CssClass += " cblink";

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { this.Visible = false; return; }

            if ((WebUser.IsAdminOrContentAdmin) || (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles)) || (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles)))
            {
                canBrowse = true;
                mojoBasePage basePage = Page as mojoBasePage;
                if (basePage != null) { basePage.ScriptConfig.IncludeColorBox = true; }
                
            }

            if (!canBrowse) 
            { 
                this.Visible = false; 
                return; 
            }

           // this.ClientClick = "return GB_showCenter(this.title, this.href, 670,670)";

            SetupLink();

        }

        

        private void SetupLink()
        {
            NavigateUrl = SiteUtils.GetNavigationSiteRoot() + WebConfigSettings.FileDialogRelativeUrl + "?type=" + browserType + "&tbi=" + textBoxClientId;

            StringBuilder script = new StringBuilder();
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
                script.ToString(),false);

            if ((previewImageOnBlur) && (textBoxClientId.Length > 0) && (browserType == "image") && (previewImageClientId.Length > 0))
            {
                script = new StringBuilder();
                script.Append("<script type=\"text/javascript\">");

                script.Append("$(document).ready(function () {");

                script.Append("$('#" + textBoxClientId + "').blur(function () { ");

                //script.Append("var imgPrev = document.getElementById('" + previewImageClientId + "'); ");

                script.Append("var url = '" + Page.ResolveUrl(emptyImageUrl) + "';");
                script.Append("if($('#" + textBoxClientId + "').val().length) {");
                script.Append("url = $('#" + textBoxClientId + "').val();");
                script.Append("}");
                script.Append("ChangeImagePreview('" + previewImageClientId + "', url);");
                script.Append("});});</script>");

                ScriptManager.RegisterStartupScript(this,
                    typeof(Page),
                    "imgprevblur_" + textBoxClientId.ToString(),
                    script.ToString(), false);

            }

        }

    }
}