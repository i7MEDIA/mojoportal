// Author:				    
// Created:			        2010-05-31
// Last Modified:		    2014-06-10
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
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.SharedFilesUI
{
    public class SharedFilesConfiguration
    {
        public SharedFilesConfiguration()
        { }

        public SharedFilesConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            showDownloadCountToAllUsers = WebUtils.ParseBoolFromHashtable(settings, "ShowDownloadCountToAllUsers", showDownloadCountToAllUsers);

            showDescription = WebUtils.ParseBoolFromHashtable(settings, "ShowDescription", showDescription);

            showSize = WebUtils.ParseBoolFromHashtable(settings, "ShowSize", showSize);

            showModified = WebUtils.ParseBoolFromHashtable(settings, "ShowModified", showModified);

            showUploadedBy = WebUtils.ParseBoolFromHashtable(settings, "ShowUploadedBy", showUploadedBy);

            showObjectCount = WebUtils.ParseBoolFromHashtable(settings, "ShowObjectCount", showObjectCount);

            enableVersioning = WebUtils.ParseBoolFromHashtable(settings, "SharedFilesEnableVersioningSetting", enableVersioning);

            

            if (settings.Contains("DefaultSortSetting"))
            {
                defaultSort = settings["DefaultSortSetting"].ToString();
            }

            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

        }

        private bool enableVersioning = false;

        public bool EnableVersioning
        {
            get { return enableVersioning; }
        }

        private bool showDownloadCountToAllUsers = false;

        public bool ShowDownloadCountToAllUsers
        {
            get { return showDownloadCountToAllUsers; }
        }

        // showDescription is false by default because a common scenario is to paste the full text
        // content from a pdf or word doc so that it is indexed in the search index and the file can be found in search
        // displaying this large text would not be pretty
        // probably should add a new field for abstract or brief description at some point.
        private bool showDescription = false;

        public bool ShowDescription
        {
            get { return showDescription; }
        }

        private bool showSize = true;

        public bool ShowSize
        {
            get { return showSize; }
        }

        private bool showModified = true;

        public bool ShowModified
        {
            get { return showModified; }
        }

        private bool showUploadedBy = true;

        public bool ShowUploadedBy
        {
            get { return showUploadedBy; }
        }

        private bool showObjectCount = true;

        public bool ShowObjectCount
        {
            get { return showObjectCount; }
        }

        private string defaultSort = "ASC";

        public string DefaultSort
        {
            get { return defaultSort; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }


        public static bool TreatPdfAsAttachment
        {
            get { return ConfigHelper.GetBoolProperty("SharedFiles:TreatPdfAsAttachment", false); }
        }

        public static bool DownloadLinksOpenNewWindow
        {
            get { return ConfigHelper.GetBoolProperty("SharedFiles:DownloadLinksOpenNewWindow", true); }
        }

        public static bool DeleteFilesOnModuleDelete
        {
            get { return ConfigHelper.GetBoolProperty("SharedFiles:DeleteFilesOnModuleDelete", true); }
        }

        public static bool DeleteFilesOnSiteDelete
        {
            get { return ConfigHelper.GetBoolProperty("SharedFiles:DeleteFilesOnSiteDelete", true); }
        }

        public static int MaxFilesToUploadAtOnce
        {
            get { return ConfigHelper.GetIntProperty("SharedFiles:MaxFilesToUploadAtOnce", 10); }
        }

        /// <summary>
        /// most files in shared files download as attachments ie file save as prompt
        /// but certain file extensions such as pdf are configured not to download as attachment since the
        /// web browser can load them. In this case the file is stored in the web browser cache
        /// this setting tells the browser for how long they can cache the file in Days
        /// 0 means no cache, default is 10 days
        /// the implementation is to add this number to the current time and set it as the expires header
        /// http://blog.tylerholmes.com/2008/05/http-headers-and-caching-cache-control.html
        /// </summary>
        public static int NonAttachmentDownloadExpireDays
        {
            get { return ConfigHelper.GetIntProperty("SharedFiles:NonAttachmentDownloadExpireDays", 10); }
        }

        /// <summary>
        /// same as above but defaults to no cache, files downloaded with save as are not stored in the browser cache
        /// so there is no point to setting a cache header
        /// </summary>
        public static int AttachmentDownloadExpireDays
        {
            get { return ConfigHelper.GetIntProperty("SharedFiles:AttachmentDownloadExpireDays", 0); }
        }
        

    }
}