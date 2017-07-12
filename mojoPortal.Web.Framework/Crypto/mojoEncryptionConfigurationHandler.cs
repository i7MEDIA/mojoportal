using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace mojoPortal.Web.Framework
{
    /// <summary>
    /// Author:              
    /// Created:            2/6/2006
    /// Last Modified:    2/6/2006
    /// </summary>
    public class mojoEncryptionConfigurationHandler : IConfigurationSectionHandler
    {


        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode node)
        {
            mojoEncryptionConfiguration config = new mojoEncryptionConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }

        #endregion


    }
}
