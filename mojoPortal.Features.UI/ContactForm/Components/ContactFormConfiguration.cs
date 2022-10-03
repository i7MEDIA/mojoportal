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
                InstanceCssClass = settings["CustomCssClassSetting"].ToString();
            }

            if (settings.Contains("ContactFormEditorHeightSetting"))
            {
                EditorHeight = settings["ContactFormEditorHeightSetting"].ToString();
            }

            if (settings.Contains("ContactFormSubjectPrefixSetting"))
            {
                subjectPrefix = settings["ContactFormSubjectPrefixSetting"].ToString().Trim();
            }

            if (settings.Contains("ContactFormEmailSetting"))
            {
                emailReceiveAddresses = settings["ContactFormEmailSetting"].ToString().Trim();
                EmailAddresses = emailReceiveAddresses.SplitOnChar('|');
            }

            if (settings.Contains("ContactFormEmailAliasSetting"))
            {
                emailReceiveAliases = settings["ContactFormEmailAliasSetting"].ToString().Trim();

            }

            EmailAliases = emailReceiveAliases.SplitOnChar('|');

            if (settings.Contains("ContactFormEmailBccSetting"))
            {
                EmailBccAddresses = settings["ContactFormEmailBccSetting"].ToString();
            }

            if (EmailAliases == null) { EmailAliases = new List<string>(); }

            UseSpamBlocking = WebUtils.ParseBoolFromHashtable(settings, "ContactFormUseCommentSpamBlocker", true);
			BlockBadWords = WebUtils.ParseBoolFromHashtable(settings, "BlockBadWords", true);

			AppendIPToMessageSetting = WebUtils.ParseBoolFromHashtable(settings, "AppendIPToMessageSetting", AppendIPToMessageSetting);
            KeepMessages = WebUtils.ParseBoolFromHashtable(settings, "KeepMessagesInDatabase", KeepMessages);
			//can't do this reliably anymore
            //useInputAsFromAddress = WebUtils.ParseBoolFromHashtable(settings, "UseInputAddressAsFromAddress", useInputAsFromAddress);

            //useHeading = WebUtils.ParseBoolFromHashtable(settings, "UseHeading", useHeading);

        }

		public bool UseSpamBlocking { get; private set; } = false;
		public bool BlockBadWords { get; private set; } = true;

		public bool AppendIPToMessageSetting { get; private set; } = true;


		private string emailReceiveAddresses = string.Empty;

        private string emailReceiveAliases = string.Empty;

		public List<string> EmailAddresses { get; private set; } = null;

		public List<string> EmailAliases { get; private set; } = null;

		public string EmailBccAddresses { get; private set; } = string.Empty;

		public bool UseInputAsFromAddress { get; } = false;

		public bool KeepMessages { get; private set; } = true;

		private string subjectPrefix = string.Empty;

        public string SubjectPrefix
        {
            get { return subjectPrefix; }
        }

		public string EditorHeight { get; private set; } = "350";

		public string InstanceCssClass { get; private set; } = string.Empty;


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