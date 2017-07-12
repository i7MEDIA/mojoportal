// Author:				    
// Created:			        2013-04-04
// Last Modified:		    2011-04-04
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.GalleryUI
{
    public class FolderGalleryConfiguration
    {
        private const string featureGuid = "9e58fcda-90de-4ed7-abc7-12f096f5c58f";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        public FolderGalleryConfiguration()
        { }

        public FolderGalleryConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("FolderGalleryRootFolder"))
            {
                folderGalleryRootFolder = settings["FolderGalleryRootFolder"].ToString();
            }

            if (settings.Contains("CustomCssClassSetting"))
            {
                customCssClassSetting = settings["CustomCssClassSetting"].ToString();
            }

            showPermaLinksSetting = WebUtils.ParseBoolFromHashtable(settings, "ShowPermaLinksSetting", showPermaLinksSetting);

            showMetaDetailsSetting = WebUtils.ParseBoolFromHashtable(settings, "ShowMetaDetailsSetting", showMetaDetailsSetting);

            allowEditUsersToChangeFolderPath = WebUtils.ParseBoolFromHashtable(settings, "AllowEditUsersToChangeFolderPath", allowEditUsersToChangeFolderPath);

            allowEditUsersToUpload = WebUtils.ParseBoolFromHashtable(settings, "AllowEditUsersToUpload", allowEditUsersToUpload);

        }

        private string customCssClassSetting = string.Empty;

        public string CustomCssClass
        {
            get { return customCssClassSetting; }
        }

        private string folderGalleryRootFolder = string.Empty;

        public string GalleryRootFolder
        {
            get { return folderGalleryRootFolder; }
        }

        private bool allowEditUsersToChangeFolderPath = true;

        public bool AllowEditUsersToChangeFolderPath
        {
            get { return allowEditUsersToChangeFolderPath; }
        }

        private bool allowEditUsersToUpload = true;

        public bool AllowEditUsersToUpload
        {
            get { return allowEditUsersToUpload; }
        }

        private bool showPermaLinksSetting = false;

        public bool ShowPermaLinks
        {
            get { return showPermaLinksSetting; }
        }

        private bool showMetaDetailsSetting = false;

        public bool ShowMetaDetails
        {
            get { return showMetaDetailsSetting; }
        }


        public static int MaxFilesToUploadAtOnce
        {
            get { return ConfigHelper.GetIntProperty("FolderGallery:MaxFilesToUploadAtOnce", 20); }
        }

        public static string BasePathFormat
        {
            get { return ConfigHelper.GetStringProperty("FolderGallery:BasePathFormat", "~/Data/Sites/{0}/media/FolderGalleries/"); }
        }
    }
}