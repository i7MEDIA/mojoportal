// Author:					
// Created:				    2007-04-24
// Last Modified:		    2008-08-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Security.Principal;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.Security
{
    /// <summary>
    ///
    /// </summary>
    public class mojoPrincipal :IPrincipal
    {
        private IPrincipal innerPrincipal;
        private mojoIdentity identity = new mojoIdentity();
        
        //private string userRoles = string.Empty;

        public mojoPrincipal(IPrincipal innerPricipal)
        {
            this.innerPrincipal = innerPricipal;
            identity = new mojoIdentity(this.innerPrincipal.Identity);
            
        }

        

        public bool IsInRole(string role)
        {
            bool result = false;
            if (this.innerPrincipal != null)
            {
                result = this.innerPrincipal.IsInRole(role);
            }

            
            if (WebConfigSettings.UseFolderBasedMultiTenants)
            {
                string virtualFolder = VirtualFolderEvaluator.VirtualFolderName();

                if(virtualFolder.Length > 0)result = false;
            }

            return result;
        }

        public IIdentity Identity
        {
            get { return identity; }
        }

        
            
    }
}
