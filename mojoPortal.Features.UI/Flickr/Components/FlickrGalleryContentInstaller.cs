// Author:					Joe Audette
// Created:				    2011-04-03
// Last Modified:			2011-04-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.IO;
using System.Xml;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI
{
    public class FlickrGalleryContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            XmlDocument xml = new XmlDocument();

            using (StreamReader stream = File.OpenText(HostingEnvironment.MapPath(configInfo)))
            {
                xml.LoadXml(stream.ReadToEnd());
            }

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name == "moduleSetting")
                {
                    XmlAttributeCollection settingAttributes = node.Attributes;

                    if ((settingAttributes["settingKey"] != null) && (settingAttributes["settingKey"].Value.Length > 0))
                    {
                        string key = settingAttributes["settingKey"].Value;
                        string val = string.Empty;
                        if (settingAttributes["settingValue"] != null)
                        {
                            val = settingAttributes["settingValue"].Value;
                        }

                        ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, key, val);
                    }
                }
            }

        }
    }
}