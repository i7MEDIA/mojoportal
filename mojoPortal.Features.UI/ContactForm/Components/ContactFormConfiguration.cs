// Author:				    
// Created:			        2010-05-22
// Last Modified:		    2010-06-01
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
using System.Collections.Generic;
using System.Configuration;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.ContactUI
{
    public class ContactFormConfiguration
    {
         public ContactFormConfiguration()
        { }

         public ContactFormConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("CustomCssClassSetting"))
            {
                instanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

            if (settings.Contains("ContactFormEditorHeightSetting"))
            {
                editorHeight = settings["ContactFormEditorHeightSetting"].ToString();
            }

            if (settings.Contains("ContactFormSubjectPrefixSetting"))
            {
                subjectPrefix = settings["ContactFormSubjectPrefixSetting"].ToString().Trim();
            }

            if (settings.Contains("ContactFormEmailSetting"))
            {
                emailReceiveAddresses = settings["ContactFormEmailSetting"].ToString().Trim();
                emailAddresses = emailReceiveAddresses.SplitOnChar('|');
            }

            if (settings.Contains("ContactFormEmailAliasSetting"))
            {
                emailReceiveAliases = settings["ContactFormEmailAliasSetting"].ToString().Trim();

            }

            emailAliases = emailReceiveAliases.SplitOnChar('|');

            if (settings.Contains("ContactFormEmailBccSetting"))
            {
                emailBccAddresses = settings["ContactFormEmailBccSetting"].ToString();
            }

            if (emailAliases == null) { emailAliases = new List<string>(); }

            useSpamBlocking = WebUtils.ParseBoolFromHashtable(settings, "ContactFormUseCommentSpamBlocker", false);

            appendIPToMessageSetting = WebUtils.ParseBoolFromHashtable(settings, "AppendIPToMessageSetting", appendIPToMessageSetting);
            keepMessages = WebUtils.ParseBoolFromHashtable(settings, "KeepMessagesInDatabase", keepMessages);
            useInputAsFromAddress = WebUtils.ParseBoolFromHashtable(settings, "UseInputAddressAsFromAddress", useInputAsFromAddress);
            //useHeading = WebUtils.ParseBoolFromHashtable(settings, "UseHeading", useHeading);

        }

        private bool useSpamBlocking = false;

        public bool UseSpamBlocking
        {
            get { return useSpamBlocking; }
        }

        private bool appendIPToMessageSetting = true;

        public bool AppendIPToMessageSetting
        {
            get { return appendIPToMessageSetting; }
        }


        private string emailReceiveAddresses = string.Empty;

        private string emailReceiveAliases = string.Empty;

        private List<string> emailAddresses = null;

        public List<string> EmailAddresses
        {
            get { return emailAddresses; }
        }

        private List<string> emailAliases = null;

        public List<string> EmailAliases
        {
            get { return emailAliases; }
        }

        private string emailBccAddresses = string.Empty;

        public string EmailBccAddresses
        {
            get { return emailBccAddresses; }
        }

        private bool useInputAsFromAddress = false;

        public bool UseInputAsFromAddress
        {
            get { return useInputAsFromAddress; }
        }

        //private bool useHeading = true;

        //public bool UseHeading
        //{
        //    get { return useHeading; }
        //}

        private bool keepMessages = true;

        public bool KeepMessages
        {
            get { return keepMessages; }
        }

        private string subjectPrefix = string.Empty;

        public string SubjectPrefix
        {
            get { return subjectPrefix; }
        }

        private string editorHeight = "350";

        public string EditorHeight
        {
            get { return editorHeight; }
        }

        private string instanceCssClass = string.Empty;

        public string InstanceCssClass
        {
            get { return instanceCssClass; }
        }


        public static string OverrideEditorProvider
        {
            get
            {
                if (ConfigurationManager.AppSettings["ContactFormOverrideEditorProvider"] != null)
                {
                    return ConfigurationManager.AppSettings["ContactFormOverrideEditorProvider"];
                }
                return string.Empty;
            }
        }

    }
}