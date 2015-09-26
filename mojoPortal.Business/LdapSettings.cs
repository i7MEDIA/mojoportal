// Author:					TJ Fontaine
// Created:				    2005-09-30
// Last Modified:			2009-01-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;

namespace mojoPortal.Business
{
	/// <summary>
	///
	/// </summary>
	public class LdapSettings
	{
		private string rootDN = String.Empty;
		private string server = String.Empty;

        // TODO: get from db
        private String domain = String.Empty;
		private int port = 389;
		// in open ldap this would be uid, for AD CN
		private String userDNKey = "CN";

		public LdapSettings()
		{
		}

		public string Server
		{
			get { return server; }
			set { server = value;}
		}

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

		public int Port
		{
			get { return port; }
			set { port = value;}
		}

		
		public string RootDN
		{
			get { return rootDN; }
			set { rootDN = value;}
		}

		public string UserDNKey
		{
			get { return userDNKey; }
			set { userDNKey = value;}
		}

		
	}
}
