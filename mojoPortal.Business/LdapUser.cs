using System.DirectoryServices;
using Novell.Directory.Ldap;

namespace mojoPortal.Business;

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


	public LdapUser() { }


	public LdapUser(DirectoryEntry adentry, String userName, LdapSettings ldapSettings)
	{
		userid = new LdapAttribute("userid", userName);
		
		var ds = new DirectorySearcher(adentry)
		{
			Filter = "(&(sAMAccountName=" + userName + "))"
		};

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

		foreach (LdapAttribute a in entry.getAttributeSet())
		{
			switch (a.Name)
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


	public string Email => ConvNull2Blank(email);

	public string UserId => ConvNull2Blank(userid);

	public string CommonName => ConvNull2Blank(commonname);

	public string Password => ConvNull2Blank(password);

	public string DN => dn;

	public string FirstName => firstName;

	public string LastName => lastName;

	public string ConvNull2Blank(LdapAttribute str) => str == null ? " " : str.StringValue;
}