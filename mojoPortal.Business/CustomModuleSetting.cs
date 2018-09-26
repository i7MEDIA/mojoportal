// Author:				
// Created:			    2004-12-26
// Last Modified:		2018-09-25

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
            this.FeatureGuid = featureGuid;
            this.DefSettingId = defSettingId;
            this.ResourceFile = resourceFile;
            this.SettingName = settingName;
            this.SettingValue = defaultValue;
            this.SettingControlType = controlType;
            this.SettingValidationRegex = settingValidationRegex;
            this.ControlSrc = controlSrc;
            this.HelpKey = helpKey;
            this.SortOrder = sortOrder;
			this.Attributes = attributes;
			this.Options = options;
		}

		public int DefSettingId { get; set; } = -1;

		public int SortOrder { get; set; } = 100;

		public Guid FeatureGuid { get; set; } = Guid.Empty;

		public string ResourceFile { get; set; } = "Resource";

		public string SettingName { get; } = string.Empty;

		public string SettingValue { get; } = string.Empty;

		public string SettingControlType { get; } = string.Empty;

		public string SettingValidationRegex { get; } = string.Empty;

		public string ControlType
        {
            get { return SettingControlType; }
        }

        public string RegexValidationExpression
        {
            get { return SettingValidationRegex; }
        }

		public string ControlSrc { get; } = string.Empty;

		public string HelpKey { get; } = string.Empty;

		public string GroupName { get; set; } = string.Empty;

		public string Attributes { get; set; } = string.Empty;

		public string Options { get; set; } = string.Empty;

	}
}
