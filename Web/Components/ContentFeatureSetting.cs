using System;
using System.Text;
using System.Xml;

namespace mojoPortal.Web;

public class ContentFeatureSetting
{
	private ContentFeatureSetting()
	{ }

	public string GroupNameKey { get; private set; } = string.Empty;
	public string ResourceFile { get; private set; } = string.Empty;
	public string ResourceKey { get; private set; } = string.Empty;
	public string DefaultValue { get; private set; } = string.Empty;
	public string ControlType { get; private set; } = "TextBox";
	public string ControlSrc { get; private set; } = string.Empty;
	public string HelpKey { get; private set; } = string.Empty;
	public int SortOrder { get; private set; } = 100;
	public string RegexValidationExpression { get; private set; } = string.Empty;
	public string Attributes { get; private set; } = string.Empty;
	public string Options { get; private set; } = string.Empty;
	public string Roles { get; private set; } = string.Empty;
	public bool ShowToUnauthorized { get; set; } = false;


	public static void LoadFeatureSetting(ContentFeature feature, XmlNode featureSettingNode)
	{
		if (feature == null || featureSettingNode == null) return;
	
		if (featureSettingNode.Name == "featureSetting")
		{
			var featureSetting = new ContentFeatureSetting();
			var attributeCollection = featureSettingNode.Attributes;

			if (attributeCollection["resourceFile"] != null)
			{
				featureSetting.ResourceFile = attributeCollection["resourceFile"].Value;
			}

			if (attributeCollection["resourceKey"] != null)
			{
				featureSetting.ResourceKey = attributeCollection["resourceKey"].Value;
			}

			if (attributeCollection["grouNameKey"] != null)
			{
				featureSetting.GroupNameKey = attributeCollection["grouNameKey"].Value;
			}

			if (attributeCollection["groupNameKey"] != null)
			{
				featureSetting.GroupNameKey = attributeCollection["groupNameKey"].Value;
			}

			if (attributeCollection["defaultValue"] != null)
			{
				featureSetting.DefaultValue = attributeCollection["defaultValue"].Value;
			}

			if (attributeCollection["controlType"] != null)
			{
				featureSetting.ControlType = attributeCollection["controlType"].Value;
			}

			if (attributeCollection["controlSrc"] != null)
			{
				featureSetting.ControlSrc = attributeCollection["controlSrc"].Value;
			}

			if (attributeCollection["helpKey"] != null)
			{
				featureSetting.HelpKey = attributeCollection["helpKey"].Value;
			}

			if (attributeCollection["sortOrder"] != null)
			{
				try
				{
					featureSetting.SortOrder = Convert.ToInt32(attributeCollection["sortOrder"].Value);
				}
				catch (FormatException) { }
				catch (OverflowException) { }
			}

			if (attributeCollection["regexValidationExpression"] != null)
			{
				featureSetting.RegexValidationExpression = attributeCollection["regexValidationExpression"].Value;
			}

			foreach (XmlNode subNode in featureSettingNode)
			{
				StringBuilder sb = XmlHelper.GetKeyValuePairsAsStringBuilder(subNode.ChildNodes);

				switch (subNode.Name)
				{
					case "Options":
						featureSetting.Options = sb.ToString();
						break;
					case "Attributes":
						featureSetting.Attributes = sb.ToString();
						break;
						//case "PreTokenString":
						//	field.PreTokenString = subNode.InnerText.Trim();
						//	break;
						//case "PostTokenString":
						//	field.PostTokenString = subNode.InnerText.Trim();
						//	break;
				}
			}

			if (attributeCollection["roles"] != null)
			{
				featureSetting.Roles = attributeCollection["roles"].Value;
			}

			if (attributeCollection["showToUnauthorized"] != null)
			{
				try
				{
					featureSetting.ShowToUnauthorized = Convert.ToBoolean(attributeCollection["showToUnauthorized"].Value);
				}
				catch (FormatException) { }
				catch (OverflowException) { }
			}

			feature.Settings.Add(featureSetting);
		}
	}
}
