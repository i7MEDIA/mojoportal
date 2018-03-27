// Author:					
// Created:				    2011-06-09
// Last Modified:			2014-06-10
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.SharedFilesUI
{
    /// <summary>
    /// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class SharedFilesDisplaySettings : WebControl
    {

        private bool hideDescription = false;

        public bool HideDescription
        {
            get { return hideDescription; }
            set { hideDescription = value; }
        }

        private bool hideSize = false;

        public bool HideSize
        {
            get { return hideSize; }
            set { hideSize = value; }
        }

        private bool hideDownloadCount = false;

        public bool HideDownloadCount
        {
            get { return hideDownloadCount; }
            set { hideDownloadCount = value; }
        }


        private bool hideModified = false;

        public bool HideModified
        {
            get { return hideModified; }
            set { hideModified = value; }
        }

        private bool hideUploadedBy = false;

        public bool HideUploadedBy
        {
            get { return hideUploadedBy; }
            set { hideUploadedBy = value; }
        }

        private bool hideFirstColumnIfNotEditable = false;

        public bool HideFirstColumnIfNotEditable
        {
            get { return hideFirstColumnIfNotEditable; }
            set { hideFirstColumnIfNotEditable = value; }
        }

        private bool showClickableFolderPathCrumbs = true;

        public bool ShowClickableFolderPathCrumbs
        {
            get { return showClickableFolderPathCrumbs; }
            set { showClickableFolderPathCrumbs = value; }
        }

        private string pathSeparator = "/";

        public string PathSeparator
        {
            get { return pathSeparator; }
            set { pathSeparator = value; }
        }

        private string newWindowLinkMarkup = "onclick=\"window.open(this.href,'_blank');return false;\"";

        public string NewWindowLinkMarkup
        {
            get { return newWindowLinkMarkup; }
            set { newWindowLinkMarkup = value; }
        }

        private string ieNewWindowLinkMarkup = " target='_blank' ";

        public string IeNewWindowLinkMarkup
        {
            get { return ieNewWindowLinkMarkup; }
            set { ieNewWindowLinkMarkup = value; }
        }

		private string deleteButtonCssClass = "sharedfiles-delete deleteitem";
		public string DeleteButtonCssClass { get => deleteButtonCssClass; set => deleteButtonCssClass = value; }

		private string upFolderButtonCssClass = "sharedfiles-upfolder";
		public string UpFolderButtonCssClass { get => upFolderButtonCssClass; set => upFolderButtonCssClass = value; }

		private string downloadButtonCssClass = "sharedfiles-download";
		public string DownloadButtonCssClass { get => downloadButtonCssClass; set => downloadButtonCssClass = value; }

		private string propertiesButtonCssClass = "sharedfiles-properties";
		public string PropertiesButtonCssClass { get => propertiesButtonCssClass; set => propertiesButtonCssClass = value; }

		protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }
}