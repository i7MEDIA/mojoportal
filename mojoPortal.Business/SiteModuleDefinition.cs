using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mojoPortal.Business
{
    public class SiteModuleDefinition
    {
        public SiteModuleDefinition()
        {

        }

        private int modueDefId = -1;

        public int ModueDefId
        {
            get { return modueDefId; }
            set { modueDefId = value; }
        }

        private Guid featureGuid = Guid.Empty;

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        private string featureName = string.Empty;

        public string FeatureName
        {
            get { return featureName; }
            set { featureName = value; }
        }

        private string controlSrc = string.Empty;

        public string ControlSrc
        {
            get { return controlSrc; }
            set { controlSrc = value; }
        }

        private string authorizedRoles = string.Empty;

        public string AuthorizedRoles
        {
            get { return authorizedRoles; }
            set { authorizedRoles = value; }
        }
    }
}
