using System;
using System.Collections.Generic;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace SuperFlexiUI;

public partial class UrlSelector : mojoUserControl, ICustomField
{
	//private Hashtable moduleSettings;
	protected ModuleConfiguration config = new ModuleConfiguration();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		browse.Text = SuperFlexiResources.Browse;

		browse.TextBoxClientId = txtUrl.ClientID;

	}

	private void LoadSettings()
	{
		int moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
		//moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
		Module module = new Module(moduleId);
		config = new ModuleConfiguration(module);
	}

	#region ICustomField

	public string GetValue()
	{
		// send the value from the control back to mojo
		return txtUrl.Text;
	}

	public void SetValue(string val)
	{
		// set the control to the value from the database 
		txtUrl.Text = val;
	}

	public new void Attributes(IDictionary<string, string> attribs)
	{
		foreach (KeyValuePair<string, string> pair in attribs)
		{
			txtUrl.Attributes.Add(pair.Key, pair.Value);
		}
	}
	#endregion
}