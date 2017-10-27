// Author:				
// Created:			    2004-12-26
// Last Modified:		2017-10-26

using System;

namespace mojoPortal.Business
{
	public class CustomModuleSetting
	{

        public CustomModuleSetting(
            Guid featureGuid,
            int defSettingId,
            string resourceFile,
            string settingName,
            string defaultValue,
            string controlType,
            string settingValidationRegex,
            string controlSrc,
            string helpKey,
            int sortOrder,
			string attributes,
			string options)
        {
            this.featureGuid = featureGuid;
            this.defSettingID = defSettingId;
            this.resourceFile = resourceFile;
            this.settingName = settingName;
            this.settingValue = defaultValue;
            this.settingControlType = controlType;
            this.settingValidationRegex = settingValidationRegex;
            this.controlSrc = controlSrc;
            this.helpKey = helpKey;
            this.sortOrder = sortOrder;
			this.attributes = attributes;
			this.options = options;
		}

        private int defSettingID = -1;
        private Guid featureGuid = Guid.Empty;
        private string resourceFile = "Resource";
		private string settingName = string.Empty;
		private string settingValue = string.Empty;
		private string settingControlType = string.Empty;
		private string settingValidationRegex = string.Empty;
        private string controlSrc = string.Empty;
        private string helpKey = string.Empty;
        private int sortOrder = 100;
        private string groupName = string.Empty;
		private string attributes = string.Empty;
		private string options = string.Empty;



		public int DefSettingId
        {
            get { return defSettingID; }
            set { defSettingID = value; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        public string ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

		public string SettingName
		{	
			get {return settingName;}
		}

		public string SettingValue
		{	
			get {return settingValue;}
		}

		public string SettingControlType
		{	
			get {return settingControlType;}
		}

		public string SettingValidationRegex
		{	
			get {return settingValidationRegex;}
		}

        public string ControlType
        {
            get { return settingControlType; }
        }

        public string RegexValidationExpression
        {
            get { return settingValidationRegex; }
        }

        public string ControlSrc
        {
            get { return controlSrc; }
        }

        public string HelpKey
        {
            get { return helpKey; }
        }

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

		public string Attributes
		{
			get => attributes;
			set => attributes = value;
		}

		public string Options
		{
			get => options;
			set => options = value;
		}

	}
}
