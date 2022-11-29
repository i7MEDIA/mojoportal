using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Editor;
using SuperFlexiBusiness;
using mojoPortal.Web.UI;
using System.IO;

namespace SuperFlexiUI
{
    public partial class Wysiwyg : UserControl, ICustomField, InterfaceControl
    {
        //private bool runningInMediumTrust = false;
        private Field controlField = new Field();
        private Dictionary<string, string> attributes = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {

            ConfigurEditor();
        }

        private void ConfigurEditor()
        {
            //editor1.Visible = false;
            //editor1.Enabled = false;
            //txtPlain.Visible = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            
            string siteRoot = SiteUtils.GetNavigationSiteRoot();

			string skinRootFolder = SiteUtils.GetSiteSkinFolderPath();

			string currentSkin = siteSettings.Skin;
			var currentPage = CacheHelper.GetCurrentPage();

			if (currentPage != null && !string.IsNullOrEmpty(currentPage.Skin))
			{
				currentSkin = currentPage.Skin;
			}
			//ed1.SiteRoot = siteRoot;

			FileInfo skinTemplates = new FileInfo($"{skinRootFolder + currentSkin}\\config\\ckeditortemplates.js");
			var skinUrl = SiteUtils.DetermineSkinBaseUrl(currentSkin);

			if (WebConfigSettings.UseSkinCssInEditor)
            {
                ed1.EditorCSSUrl = SiteUtils.GetEditorStyleSheetUrl(true, true, Page);
            }

            if (attributes.ContainsKey("$EditorConfigPath"))
            {
                ed1.CustomConfigPath = attributes["$EditorConfigPath"].ToString();
            }
            ed1.ToolBar = ToolBar.FullWithTemplates;
            ed1.Height = Unit.Pixel(300);
            ed1.Skin = WebConfigSettings.CKEditorSkin;

            if (siteSettings != null)
            {
                if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles + ";" + siteSettings.UserFilesBrowseAndUploadRoles))
                {
                    ed1.FileManagerUrl = siteRoot + WebConfigSettings.FileDialogRelativeUrl;
                    ed1.EnableFileBrowser = true;
                }
            }
            var cacheCrusher = Guid.NewGuid().ToString().Replace("-", string.Empty); //prevent caching with a guid param

			ed1.TemplatesJsonUrl = $"{siteRoot}/Services/CKeditorTemplates.ashx?cb={cacheCrusher}"; 
            ed1.StylesJsonUrl = $"{siteRoot}/Services/CKeditorStyles.ashx?cb={cacheCrusher}";
			if (skinTemplates.Exists)
			{

				ed1.SkinTemplatesUrl = $"{skinUrl}config/ckeditortemplates.js?cb={cacheCrusher}";
			}


			CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();
            if (defaultCulture.TextInfo.IsRightToLeft)
            {
                ed1.TextDirection = Direction.RightToLeft;
            }
        }


        #region ISettingControl

        public string GetValue()
        {
            return ed1.Text;
        }

        public void SetValue(string val)
        {
            ed1.Text = val;
        }

        public new void Attributes(IDictionary<string, string> attribs)
        {
            //AttributeCollection attribCol = ed1.Attributes;

            //FieldUtils.GetFieldAttributes(attribs, out attribCol);

            //foreach (string key in attribCol.Keys)
            //{
            //    ed1.Attributes.Add(key, (string)attribCol[key]);
            //}
        }

        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //http://ajaxcontroltoolkit.codeplex.com/workitem/26772

            //runningInMediumTrust = !mojoSetup.RunningInFullTrust();

            //if (runningInMediumTrust)
            //{
            //    editor1.Visible = false;
            //    editor1.Enabled = false;
            //    txtPlain.Visible = true;
                
            //}
        }
        #region InterfaceControl

        public void ControlField(Field field)
        {
            controlField = field;
        }

        #endregion
    }
}