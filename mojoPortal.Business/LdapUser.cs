// Author:					TJ Fontaine
// Created:				    2005-09-30
// Last Modified:			2012-06-07
// 
// 1/7/2006 Haluk Eryuksel added convNull2Blank
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 
// 2011-02-05 added changes from Jamie Eubanks to better support Active Directory

using System;
using Novell.Directory.Ldap;
using System.DirectoryServices;

namespace mojoPortal.Business
{
	/// <summary>
	///
	/// </summary>
	public class LdapUser
	{
		private LdapAttribute email = null;
		private LdapAttribute commonname = null;
		private LdapAttribute password = null;
		private LdapAttribute userid = null;
		private LdapAttribute uidNumber = null;
		private string dn = String.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;

		public LdapUser() {}

        
        public LdapUser(DirectoryEntry adentry, String userName, LdapSettings ldapSettings)
        {
            userid = new LdapAttribute("userid", userName);
            DirectorySearcher ds = new DirectorySearcher(adentry);
            ds.Filter = "(&(sAMAccountName=" + userName + "))";
            SearchResult result = ds.FindOne();
            DirectoryEntry ent = null;

            if (result != null)
            {
                ent = result.GetDirectoryEntry();
            }

            if (ent != null)
            {
                if (ent.Properties["cn"].Value != null)
                {
                    commonname = new LdapAttribute("commonname", ent.Properties["cn"].Value.ToString());
                }
                else
                {
                    commonname = new LdapAttribute("commonname", userName);
                }
                if (ent.Properties["mail"].Value != null)
                {
                    email = new LdapAttribute("email", ent.Properties["mail"].Value.ToString());
                }
                else
                {
                    email = new LdapAttribute("email", userName + "@" + ldapSettings.Domain);
                }
            }
        }

        //public LdapUser(LdapSettings ldapSettings, String userName)
        //{
        //    // in some cases with Active Directory
        //    // we can't actually retrieve ldap entries
        //    // we really just need to create a mojoportal user
        //    // from the ldap user so if we can't read it, just create an ldap user
        //    // with the properties we do have
        //    // Active Directory allows us to bind a connection for authentication
        //    // even if we can't query for entries

        //    email = new LdapAttribute("email", userName + "@" + ldapSettings.Domain);
        //    commonname = new LdapAttribute("commonname", userName);
        //    userid = new LdapAttribute("userid", userName);

        //}

		public LdapUser(LdapEntry entry)
		{
			dn = entry.DN;

			LdapAttributeSet las = entry.getAttributeSet();
			
			foreach(LdapAttribute a in las)
			{
				switch(a.Name)
				{
					case "mail":
						this.email = a;
						break;
					case "cn":
						this.commonname = a;
						break;
					case "userPassword":
						this.password = a;
						break;
					case "uidNumber":
						this.uidNumber = a;
						break;
					case "uid":
						this.userid = a;
                        break;
                    case "sAMAccountName":
                        this.userid = a;
						break;
                    case "givenName":
                        this.firstName = a.StringValue;
                        break;
                    case "sn":
                        this.lastName = a.StringValue;
                        break;
				}
			}
		}

		public string Email
		{
            get { return ConvNull2Blank(email); }
		}

		public string UserId
		{
			get { return ConvNull2Blank(userid); }
		}

		public string CommonName
		{
            get { return ConvNull2Blank(commonname); }

		}

		public string Password
		{
            get { return ConvNull2Blank(password); }
		}

		public string DN
		{
			get { return dn; }
		}

        public string FirstName
        {
            get { return firstName; }
        }

        public string LastName
        {
            get { return lastName; }
        }

        public String ConvNull2Blank(LdapAttribute str)
        {
            if (str == null)
                return " ";
            else
                return str.StringValue;
        }

	}
}
