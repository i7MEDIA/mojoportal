// Author:					i7MEDIA (joe davis)
// Created:				    2015-03-31
// Last Modified:			2017-06-16
//
// You must not remove this notice, or any other, from this software.
//
using System;
using System.Collections;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Web.UI;
using System.Collections.Generic;

namespace SuperFlexiUI
{
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
			//AttributeCollection attribCol = txtUrl.Attributes;

			//FieldUtils.GetFieldAttributes(attribs, out attribCol);

			//foreach (string key in attribCol.Keys)
			//{
			//    txtUrl.Attributes.Add(key, (string)attribCol[key]);                
			//}
			foreach (KeyValuePair<string, string> pair in attribs)
			{
				txtUrl.Attributes.Add(pair.Key, pair.Value);
			}
		}
        #endregion
    }
}