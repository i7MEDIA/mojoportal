// Author:				    Joe Audette
// Created:			        2010-11-28
// Last Modified:		    2010-11-28
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
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Flickr.UI
{
    public class FlickrGalleryConfiguration
    {
        public FlickrGalleryConfiguration()
        { }

        public FlickrGalleryConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("FlickrApiKey"))
            {
                flickrApiKey = settings["FlickrApiKey"].ToString();
            }

            if (settings.Contains("FlickrUserID"))
            {
                flickrUserId = settings["FlickrUserID"].ToString();
            }

            if (settings.Contains("FlickrSetID"))
            {
                flickrSetId = settings["FlickrSetID"].ToString();
            }

            if (settings.Contains("Theme"))
            {
                theme = settings["Theme"].ToString();
            }

            pageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", pageSize);

        }

        private string flickrApiKey = string.Empty;

        public string FlickrApiKey
        {
            get { return flickrApiKey; }
        }

        private string flickrUserId = string.Empty;

        public string FlickrUserId
        {
            get { return flickrUserId; }
        }

        private string flickrSetId = string.Empty;

        public string FlickrSetId
        {
            get { return flickrSetId; }
        }

        private int pageSize = 40;

        public int PageSize
        {
            get { return pageSize; }
        }

        private string theme = "white";

        public string Theme
        {
            get { return theme; }
        }


        public static string AvailableThemes
        {
            get { return ConfigHelper.GetStringProperty("FlickrGallery:AvailableThemes", "white,black"); }
        }

    }
}