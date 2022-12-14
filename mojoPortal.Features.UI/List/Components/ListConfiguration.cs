// Author:				    
// Created:			        2010-12-06
// Last Modified:		    2011-02-25
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


namespace mojoPortal.Web.LinksUI
{
    public class ListConfiguration
    {
        public ListConfiguration()
        { }

        public ListConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            useDescription = WebUtils.ParseBoolFromHashtable(settings, "LinksShowDescriptionSetting", useDescription);

            descriptionOnly = WebUtils.ParseBoolFromHashtable(settings, "LinksShowOnlyDescriptionSetting", descriptionOnly);

            if (descriptionOnly) { useDescription = true; }

            //showDeleteIcon = WebUtils.ParseBoolFromHashtable(settings, "LinksShowDeleteIconSetting", showDeleteIcon);

            enablePager = WebUtils.ParseBoolFromHashtable(settings, "LinksEnablePagingSetting", enablePager);

            pageSize = WebUtils.ParseInt32FromHashtable(settings, "LinksPageSizeSetting", pageSize);

            if (settings.Contains("LinksExtraCssClassSetting"))
            {
                instanceCssClass = settings["LinksExtraCssClassSetting"].ToString();
            }

            useAjaxPaging = WebUtils.ParseBoolFromHashtable(settings, "UseAjaxPaging", useAjaxPaging);

            showPageLeftContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageLeftContentSetting", showPageLeftContent);

            showPageRightContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageRightContentSetting", showPageRightContent);

            if (settings.Contains("IntroContent"))
            {
                introContent = settings["IntroContent"].ToString();
            }
        }

        private string introContent = string.Empty;

        public string IntroContent
        {
            get { return introContent; }
        }

        private bool useDescription = false;

        public bool UseDescription
        {
            get { return useDescription; }
        }

        private bool descriptionOnly = false;

        public bool DescriptionOnly
        {
            get { return descriptionOnly; }
        }

        //private bool showDeleteIcon = false;

        //public bool ShowDeleteIcon
        //{
        //    get { return showDeleteIcon; }
        //}

        private bool enablePager = false;

        public bool EnablePager
        {
            get { return enablePager; }
        }

        private bool useAjaxPaging = true;

        public bool UseAjaxPaging
        {
            get { return useAjaxPaging; }
        }

        private int pageSize = 50;

        public int PageSize
        {
            get { return pageSize; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

        //ShowPageLeftContentSetting

        private bool showPageLeftContent = true;

        public bool ShowPageLeftContent
        {
            get { return showPageLeftContent; }
        }

        private bool showPageRightContent = true;

        public bool ShowPageRightContent
        {
            get { return showPageRightContent; }
        }


        public static bool UseProtocolDropdown
        {
            get { return ConfigHelper.GetBoolProperty("List:UseProtocolDropdown", false); }
        }

    }
}