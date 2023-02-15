// Created:				    2007-08-05
// Last Modified:			2017-10-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Xml;
using System.Text;
using mojoPortal.Web.Framework;
namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ContentFeatureSetting
    {
        private ContentFeatureSetting()
        { }


        private string resourceFile = string.Empty;
        private string resourceKey = string.Empty;
        private string defaultValue = string.Empty;
        private string controlType = "TextBox";
        private string regexValidationExpression = string.Empty;
        private string controlSrc = string.Empty;
        private string helpKey = string.Empty;
        private string groupNameKey = string.Empty;
        private string attributes = string.Empty;
        private string options = string.Empty;
		private int sortOrder = 100;

		public string GroupNameKey
        {
            get { return groupNameKey; }
        }

        public string ResourceFile
        {
          get{return resourceFile;}
        }

        public string ResourceKey
        {
            get { return resourceKey; } 
        }

        public string DefaultValue
        {
            get { return defaultValue; }
        }

        public string ControlType
        {
            get { return controlType; }
        }

        public string ControlSrc
        {
            get { return controlSrc; }
        }

        public string HelpKey
        {
            get { return helpKey; }
        }

        public int SortOrder
        {
            get { return sortOrder; }
        }

        public string RegexValidationExpression
        {
            get { return regexValidationExpression; }
        }

		public string Attributes
		{
			get { return attributes; }
		}

		public string Options
		{
			get { return options; }
		}

		public static void LoadFeatureSetting(
            ContentFeature feature,
            XmlNode featureSettingNode)
        {
            if (feature == null) return;
            if (featureSettingNode == null) return;

            if (featureSettingNode.Name == "featureSetting")
            {
                ContentFeatureSetting featureSetting = new ContentFeatureSetting();

                XmlAttributeCollection attributeCollection = featureSettingNode.Attributes;

                if (attributeCollection["resourceFile"] != null)
                {
                    featureSetting.resourceFile = attributeCollection["resourceFile"].Value;
                }

                if (attributeCollection["resourceKey"] != null)
                {
                    featureSetting.resourceKey = attributeCollection["resourceKey"].Value;
                }

                if (attributeCollection["grouNameKey"] != null)
                {
                    featureSetting.groupNameKey = attributeCollection["grouNameKey"].Value;
                }

                if (attributeCollection["groupNameKey"] != null)
                {
                    featureSetting.groupNameKey = attributeCollection["groupNameKey"].Value;
                }

                if (attributeCollection["defaultValue"] != null)
                {
                    featureSetting.defaultValue = attributeCollection["defaultValue"].Value;
                }

                if (attributeCollection["controlType"] != null)
                {
                    featureSetting.controlType = attributeCollection["controlType"].Value;
                }

                if (attributeCollection["controlSrc"] != null)
                {
                    featureSetting.controlSrc = attributeCollection["controlSrc"].Value;
                }

                if (attributeCollection["helpKey"] != null)
                {
                    featureSetting.helpKey = attributeCollection["helpKey"].Value;
                }

                if (attributeCollection["sortOrder"] != null)
                {
                    try
                    {
                        featureSetting.sortOrder = Convert.ToInt32(attributeCollection["sortOrder"].Value);
                    }
                    catch (System.FormatException) { }
                    catch (System.OverflowException) { }
                }

                if (attributeCollection["regexValidationExpression"] != null)
                {
                    featureSetting.regexValidationExpression = attributeCollection["regexValidationExpression"].Value;
                }

				StringBuilder attributes = new StringBuilder();
				StringBuilder options = new StringBuilder();
				foreach (XmlNode subNode in featureSettingNode)
				{
					StringBuilder sb = Core.Helpers.XmlHelper.GetKeyValuePairsAsStringBuilder(subNode.ChildNodes);

					switch (subNode.Name)
					{
						case "Options":
							featureSetting.options = sb.ToString();
							break;
						case "Attributes":
							featureSetting.attributes = sb.ToString();
							break;
						//case "PreTokenString":
						//	field.PreTokenString = subNode.InnerText.Trim();
						//	break;
						//case "PostTokenString":
						//	field.PostTokenString = subNode.InnerText.Trim();
						//	break;
					}
				}
				feature.Settings.Add(featureSetting);
            }
        }
    }
}
