//	Created:			    2011-02-25
//	Last Modified:		    2014-02-06
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

            script.Append("function SetUrl (URL, clientId) {");

            //script.Append("GB_hide();");
            //script.Append("var txtUrl = document.getElementById('" + textBoxClientId + "'); ");
            script.Append("var txtUrl = document.getElementById(clientId); ");
            script.Append("txtUrl.value = URL;");
            //script.Append("alert(URL);");
            if((browserType == "image")&&(previewImageClientId.Length > 0))
            {
                script.Append("var imgPrev = document.getElementById('" + previewImageClientId + "'); ");
                script.Append("imgPrev.src = URL;");

            }

            script.Append("$.colorbox.close(); ");

            script.Append("}");

            

            script.Append("\n</script>");

            ScriptManager.RegisterClientScriptBlock(this,
                typeof(Page),
                "SetUrl",
                script.ToString(),false);

            if ((previewImageOnBlur) && (textBoxClientId.Length > 0) && (browserType == "image") && (previewImageClientId.Length > 0))
            {
                script = new StringBuilder();
                script.Append("\n<script type=\"text/javascript\">");

                script.Append("$(document).ready(function () {");

                script.Append("$('#" + textBoxClientId + "').blur(function () { ");

                script.Append("var imgPrev = document.getElementById('" + previewImageClientId + "'); ");

                script.Append("if($('#" + textBoxClientId + "').val().length) {");

                //script.Append("alert('not empty');");

                script.Append("imgPrev.src = $('#" + textBoxClientId + "').val(); ");

                script.Append("} else { ");

                //script.Append("alert('empty');");
                script.Append("imgPrev.src = '" + Page.ResolveUrl(emptyImageUrl) + "';");

                script.Append("} });");

                script.Append("});	");

                script.Append("\n</script>");

                ScriptManager.RegisterStartupScript(this,
                    typeof(Page),
                    "imgprevblur",
                    script.ToString(), false);

            }

        }

    }
}