using System;
using System.Collections.Generic;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using SuperFlexiBusiness;

namespace SuperFlexiUI;

public partial class FeaturedImageSetting : mojoUserControl, ICustomField, InterfaceControl, mojoPortal.Web.UI.ISettingControl
{
	public ModuleConfiguration config = new();
	private Field controlField = new();
	protected string removeImageText = SuperFlexiResources.RemoveImage;
	private IDictionary<string, string> attributes = new Dictionary<string, string>();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		browse.Text = SuperFlexiResources.BrowseFeaturedImage;
		browse.ToolTip = SuperFlexiResources.BrowseFeaturedImageToolTip;
		browse.TextBoxClientId = imageUrl.ClientID;
		browse.PreviewImageClientId = imagePreview.ClientID;
		browse.EmptyImageUrl = config.FeaturedImageEmptyUrl;

		if (attributes.ContainsKey("$EmptyImageUrl"))
		{
			browse.EmptyImageUrl = attributes["$EmptyImageUrl"].ToString();
		}
		if (attributes.ContainsKey("$BrowseText"))
		{
			browse.Text = attributes["$BrowseText"].ToString();
		}
		if (attributes.ContainsKey("$ToolTip"))
		{
			browse.ToolTip = attributes["$ToolTip"].ToString();
		}
		if (attributes.ContainsKey("$RemoveImageText"))
		{
			removeImageText = attributes["$RemoveImageText"].ToString();
		}

		if (String.IsNullOrWhiteSpace(imageUrl.Text))
		{
			imagePreview.ImageUrl = config.FeaturedImageEmptyUrl;
		}
		else
		{
			imagePreview.ImageUrl = imageUrl.Text.Replace("&deleted&", "");
		}

		imageUrl.CssClass = controlField.EditPageControlCssClass;
		//imageUrl.Required = controlField.Required;
		foreach (string key in attributes.Keys)
		{
			if (!key.StartsWith("$"))
			{
				imageUrl.Attributes.Add(key, (string)attributes[key]);
			}
		}

	}

	private void LoadSettings()
	{
		int moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
		//moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
		Module module = new Module(moduleId);
		config = new ModuleConfiguration(module);

		if (!String.IsNullOrWhiteSpace(controlField.Attributes))
		{
			attributes = UIHelper.GetDictionaryFromString(controlField.Attributes);
		}
	}

	#region ISettingControl

	public string GetValue()
	{
		// send the value from the control back to mojo
		return imageUrl.Text;
	}

	public void SetValue(string val)
	{
		// set the control to the value from the database 
		imageUrl.Text = val;
	}

	#endregion

	#region InterfaceControl

	public void ControlField(Field field)
	{
		controlField = field;
	}
	public new void Attributes(IDictionary<string, string> attribs)
	{
		//AttributeCollection attribCol = imageUrl.Attributes;

		//FieldUtils.GetFieldAttributes(attribs, out attribCol);

		//foreach (string key in attribCol.Keys)
		//{
		//    imageUrl.Attributes.Add(key, (string)attribCol[key]);
		//}

		foreach (KeyValuePair<string, string> pair in attribs)
		{
			imageUrl.Attributes.Add(pair.Key, pair.Value);
		}
	}
	#endregion
}