namespace mojoPortal.Business;

public class CustomModuleSetting
{
	public int DefSettingId { get; set; } = -1;
	public int SortOrder { get; set; } = 100;
	public Guid FeatureGuid { get; set; } = Guid.Empty;
	public string ResourceFile { get; set; } = "Resource";
	public string SettingName { get; } = string.Empty;
	public string SettingValue { get; } = string.Empty;
	public string SettingControlType { get; } = string.Empty;
	public string SettingValidationRegex { get; } = string.Empty;
	public string ControlType => SettingControlType;
	public string RegexValidationExpression => SettingValidationRegex;
	public string ControlSrc { get; } = string.Empty;
	public string HelpKey { get; } = string.Empty;
	public string GroupName { get; set; } = string.Empty;
	public string Attributes { get; set; } = string.Empty;
	public string Options { get; set; } = string.Empty;


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
		FeatureGuid = featureGuid;
		DefSettingId = defSettingId;
		ResourceFile = resourceFile;
		SettingName = settingName;
		SettingValue = defaultValue;
		SettingControlType = controlType;
		SettingValidationRegex = settingValidationRegex;
		ControlSrc = controlSrc;
		HelpKey = helpKey;
		SortOrder = sortOrder;
		Attributes = attributes;
		Options = options;
	}
}
