using System;
using System.Collections.Generic;
using System.Web.UI;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.Controls
{
	public partial class FilePathSetting : UserControl, ICustomField
	{
		//private Hashtable moduleSettings;
		//protected ModuleConfiguration config = new ModuleConfiguration();
		//string type = "file";
		protected void Page_Load(object sender, EventArgs e)
		{
			//LoadSettings();

			browse.Text = Resource.Browse;
			browse.TextBoxClientId = txtPath.ClientID;

		}

		private void LoadSettings()
		{
			//int moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			////moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
			//Module module = new Module(moduleId);
			//config = new ModuleConfiguration(module);
		}

		#region ICustomField

		public string GetValue()
		{
			// send the value from the control back to mojo
			return txtPath.Text;
		}

		public void SetValue(string val)
		{
			// set the control to the value from the database 
			txtPath.Text = val;
		}

		public void Attributes(IDictionary<string, string> attribs)
		{

			foreach (KeyValuePair<string, string> pair in attribs)
			{
				if (pair.Key == "type")
				{
					//type = pair.Value;
					browse.BrowserType = pair.Value;
					if (pair.Value == "folder")
					{
						browse.Editor = "folderpicker";
					}
				}
				else if (pair.Key == "startFolder")
				{
					browse.StartFolder = pair.Value;
				}
				else if (pair.Key == "returnFullPath" && pair.Value.ToLower() == "false")
				{
					browse.ReturnFullPath = false;
				}
				else
				{
					txtPath.Attributes.Add(pair.Key, pair.Value);
				}
			}
		}
		#endregion
	}
}