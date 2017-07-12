// Author:				    
// Created:			        2011-01-22
// Last Modified:		    2011-01-24
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

namespace mojoPortal.Web.XmlUI 
{
    public class XmlConfiguration
    {
        private const string featureGuid = "fa969c0a-6d02-4dcb-86b8-ac69d80c1fb1";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        public XmlConfiguration()
        { }

        public XmlConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("XmlModuleXmlSourceSetting"))
            {
                xmlFileSource = settings["XmlModuleXmlSourceSetting"].ToString();
            }

            if (settings.Contains("XmlModuleXslSourceSetting"))
            {
                xslFileSource = settings["XmlModuleXslSourceSetting"].ToString();
            }

            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

            if (settings.Contains("XmlUrl"))
            {
                xmlUrl = settings["XmlUrl"].ToString();
            }

            if (settings.Contains("XslUrl"))
            {
                xslUrl = settings["XslUrl"].ToString();
            }

            allowExternalImages = WebUtils.ParseBoolFromHashtable(settings, "AllowExternalImages", allowExternalImages);

            trustContent = WebUtils.ParseBoolFromHashtable(settings, "TrustContent", trustContent);
        }

        private string xmlUrl = string.Empty;

        public string XmlUrl
        {
            get { return xmlUrl; }
        }

        private string xslUrl = string.Empty;

        public string XslUrl
        {
            get { return xslUrl; }
        }

        private string xmlFileSource = string.Empty;

        public string XmlFileSource
        {
            get { return xmlFileSource; }
        }

        private string xslFileSource = string.Empty;

        public string XslFileSource
        {
            get { return xslFileSource; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }

        private bool allowExternalImages = false;

        public bool AllowExternalImages
        {
            get { return allowExternalImages; }
        }

        private bool trustContent = false;

        public bool TrustContent
        {
            get { return trustContent; }
        }


        

    }
}