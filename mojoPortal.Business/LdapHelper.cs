using log4net;
using mojoPortal.Core.Configuration;
using Novell.Directory.Ldap;
using System.DirectoryServices;
using System.Security.Cryptography.X509Certificates;

namespace mojoPortal.Business;

public sealed class LdapHelper
{
	private LdapHelper() { }

	private static readonly ILog log = LogManager.GetLogger(typeof(LdapHelper));


	#region Static Methods

	private static LdapConnection GetConnection(LdapSettings ldapSettings)
	{
		var conn = new LdapConnection();

		if (ConfigHelper.GetBoolProperty("UseSslForLdap", false))
		{
			// make this support ssl/tls
			// http://stackoverflow.com/questions/386982/novell-ldap-c-novell-directory-ldap-has-anybody-made-it-work
			conn.SecureSocketLayer = true;
			conn.UserDefinedServerCertValidationDelegate += new CertificateValidationCallback(LdapSSLHandler);
		}

		conn.Connect(ldapSettings.Server, ldapSettings.Port);

		return conn;
	}


	public static bool LdapSSLHandler(X509Certificate certificate, int[] certificateErrors)
	{
		// 2024-10-04 Removed Mono.Security.X509 dependency.
		// The DLL was part of the Npgsql library in _libs and was
		// removed when migrating the packages to nuget instead of locally.
		var store = new X509Store(StoreLocation.LocalMachine);
		X509Certificate x509 = null;
		var data = certificate.GetRawCertData();

		if (data is not null)
		{
			x509 = new X509Certificate(data);
		}

		if (x509 is not null)
		{
			if (!store.Certificates.Contains(x509))
			{
				store.Certificates.Add(x509);
			}
		}

		return true;
	}


	private static LdapEntry GetOneUserEntry(LdapConnection conn, LdapSettings ldapSettings, string search)
	{
		var constraints = new LdapSearchConstraints();

		LdapSearchQueue queue = null;
		queue = conn.Search(
			ldapSettings.RootDN,
			LdapConnection.SCOPE_SUB,
			$"{ldapSettings.UserDNKey}={search}",
			null,
			false,
			null,
			null
		);

		LdapEntry entry = null;

		if (queue == null)
		{
			return entry;
		}

		LdapMessage message = queue.getResponse();

		if (message != null && message is LdapSearchResult result)
		{
			entry = result.Entry;
		}

		return entry;
	}


	public static LdapUser LdapLogin(LdapSettings ldapSettings, string uid, string password)
	{
		if (ldapSettings.UserDNKey == "uid") //OpenLDAP
		{
			return LdapStandardLogin(ldapSettings, uid, password);
		}
		else //Active Directory
		{
			return ActiveDirectoryLogin(ldapSettings, uid, password);
		}
	}


	private static LdapUser LdapStandardLogin(LdapSettings ldapSettings, string uid, string password)
	{
		bool success = false;
		LdapUser user = null;

		LdapConnection conn = null;
		try
		{
			conn = GetConnection(ldapSettings);
		}
		catch (System.Net.Sockets.SocketException ex)
		{
			if (log.IsErrorEnabled)
			{
				//log.Error("couldn't connect to ldap server ", ex);
				log.Error($"Login failure for user: {uid}. Exception: ", ex);
			}
		}

		if ((conn != null) && conn.Connected)
		{
			LdapEntry entry = null;

			try
			{
				entry = GetOneUserEntry(conn, ldapSettings, uid);
				if (entry != null)
				{
					var authConn = GetConnection(ldapSettings);
					authConn.Bind(entry.DN, password);
					authConn.Disconnect();
					success = true;
				}
			}
			catch (LdapException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error($"Login failure for user: {uid}. Exception: ", ex);
				}
				success = false;
			}

			if (success)
			{
				if (entry != null)
				{
					user = new LdapUser(entry);
				}
			}

			conn.Disconnect();
		}

		return user;
	}


	private static bool UseRootDNWithActiveDirectory()
	{
		return ConfigHelper.GetBoolProperty("UseRootDNWithActiveDirectory", false);
	}


	private static LdapUser ActiveDirectoryLogin(LdapSettings ldapSettings, string uid, string password)
	{
		bool success = false;
		LdapUser user = null;
		DirectoryEntry adentry = null;

		//Note: Not necessary to check SSL. Default authentication type for .NET 2.0+ is "Secure"
		try
		{
			if (UseRootDNWithActiveDirectory())
			{
				adentry = new DirectoryEntry($"LDAP://{ldapSettings.Server}/{ldapSettings.RootDN}", $"{ldapSettings.Domain}\\{uid}", password);
			}
			else
			{
				adentry = new DirectoryEntry($"LDAP://{ldapSettings.Server}", $"{ldapSettings.Domain}\\{uid}", password);
			}
		}
		catch (System.Runtime.InteropServices.COMException ex)
		{
			if (log.IsErrorEnabled)
			{
				log.Error($"Login failure for user: {uid}. Exception: ", ex);
			}
		}

		if (adentry != null)
		{
			//Bind to the native AdsObject to force authentication.
			try
			{
				object testobj = adentry.NativeObject;
				success = true;
			}
			catch (System.Runtime.InteropServices.COMException ex)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("login failure", ex);
				}
				success = false;
			}

			if (success && adentry != null)
			{
				if (UseRootDNWithActiveDirectory())
				{
					var ds = new DirectorySearcher(adentry)
					{
						Filter = $"(&(sAMAccountName={uid}))"
					};

					if (ds.FindOne() != null)
					{
						//log.Error("successful authentication to ldap server in OU with Server: " + ldapSettings.Server + "; RootDN: " + ldapSettings.RootDN + "; UID=" + uid);
						user = new LdapUser(adentry, uid, ldapSettings);
					}
					else
					{
						log.Info($"failed authentication to ldap server in OU with Server: {ldapSettings.Server}; RootDN: {ldapSettings.RootDN}; UID={uid}");
						//potentially look in the security group
					}
				}
				else
				{
					user = new LdapUser(adentry, uid, ldapSettings);
				}
			}
		}

		return user;
	}

	//public static LdapUser LdapLogin(LdapSettings ldapSettings, string uid, string password)
	//{
	//    LdapConnection conn = null;
	//    try
	//    {
	//        conn = GetConnection(ldapSettings);
	//    }
	//    catch (System.Net.Sockets.SocketException ex)
	//    {
	//        log.Error("couldn't connect to ldap server ", ex);
	//    }

	//    bool success = false;
	//    LdapUser user = null;

	//    if ((conn != null)&&(conn.Connected))
	//    {
	//        LdapEntry entry = null;

	//        try
	//        {
	//            // open ldap uses uid
	//            if(ldapSettings.UserDNKey == "uid")
	//            {
	//                entry = GetOneUserEntry(conn, ldapSettings, uid);
	//                if(entry != null)
	//                {
	//                    LdapConnection authConn = GetConnection(ldapSettings);
	//                    authConn.Bind(entry.DN, password);
	//                    authConn.Disconnect();
	//                    success = true;

	//                }

	//            }
	//            else
	//            {
	//                // Active Directory uses CN

	//                // might need this if other Ldap Servers besides Active Directory use CN
	//                //conn.Bind(
	//                //    ldapSettings.UserDNKey + "=" + uid + "," + ldapSettings.RootDN, password);


	//                // this works with Active Directory
	//                conn.Bind(uid + "@" + ldapSettings.Domain, password);
	//                success = conn.Bound;
	//                entry = GetOneUserEntry(conn, ldapSettings, uid);

	//            }


	//        }
	//        catch (Novell.Directory.Ldap.LdapException ex)
	//        {
	//            if (log.IsErrorEnabled)
	//            {
	//                log.Error("login failure", ex);
	//            }
	//            success = false;
	//        }

	//        if (success)
	//        {
	//            if (entry != null)
	//            {
	//                user = new LdapUser(entry);
	//            }
	//            else
	//            {
	//                user = new LdapUser(ldapSettings, uid);
	//            }

	//        }

	//        conn.Disconnect();
	//    }

	//    return user;
	//}

	public static bool TestUser(LdapSettings ldapSettings, string uid, string password)
	{
		if (LdapLogin(ldapSettings, uid, password) != null)
		{
			return true;
		}
		return false;
	}


	//        public static LdapUser GetUser(LdapSettings ldapSettings, string uid)
	//        {
	//            LdapConnection conn = GetConnection(ldapSettings);
	//            LdapUser user = null;
	//
	//            if (conn.Connected)
	//            {
	//
	//                SiteSettings siteSettings = (SiteSettings)HttpContext.Current.Items["SiteSettings"];
	//                SiteUser siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
	//			
	//                conn.Bind(ldapSettings.UserDNKey + "=" +siteUser.UserID+ "," + ldapSettings.RootDN, siteUser.Password);
	//
	//                if (conn.Bound)
	//                {
	//                    LdapEntry entry = GetOneUserEntry(conn, ldapSettings, uid);
	//                    user = new LdapUser(entry);
	//                }
	//
	//                conn.Disconnect();
	//            }
	//            return user;
	//        }

	//		public static bool UpdateUser(LdapDetails LdapSettings, LdapUser User)
	//		{
	//			bool success = false;
	//
	//			LdapConnection conn = GetConnection(LdapSettings, true);
	//
	//			try
	//			{
	//				conn.Modify(User.DN, User.Modifications);
	//				success = true;
	//			}
	//			catch
	//			{
	//				success = false;
	//			}
	//
	//			return success;
	//		}

	/* Left over LDAP Code
    private static LdapEntry GetOneGroupEntry(LdapConnection conn, LdapDetails ld, string GidNumber)
    {
        LdapSearchQueue queue = conn.Search(ld.adminemail,
            LdapConnection.SCOPE_ONE,
            "gidNumber="+GidNumber, null, false, (LdapSearchQueue)null, (LdapSearchConstraints)null);
        LdapMessage message = queue.getResponse();
        LdapEntry entry = null;

        if(message != null && message is LdapSearchResult)
            entry = ((LdapSearchResult)message).Entry;

        return entry;
    }

    public static string[] GetGroups(LdapDetails ld, string email)
    {
        ArrayList groups = new ArrayList();
        if(classConn == null || classConn.Connected)
            classConn = GetConnection(ld, true);

        LdapEntry entry = GetOneUserEntry(classConn, ld, email);
        string entryUID = ((LdapAttribute)entry.getAttribute("uid")).StringValue;

        //primary group
        string gidNumber = ((LdapAttribute)entry.getAttribute("gidNumber")).StringValue;
        LdapAttribute gidName = ((LdapEntry)GetOneGroupEntry(classConn, ld, gidNumber)).getAttribute("cn");
        groups.Add(gidName.StringValue);

        LdapSearchQueue queue = classConn.Search(ld.adminemail,
            LdapConnection.SCOPE_ONE, 
            "memberUid="+entryUID,
            null, 
            false, 
            (LdapSearchQueue)null, (LdapSearchConstraints)null);

        LdapMessage message;
        LdapAttribute attr;
        while((message = queue.getResponse()) != null && message is LdapSearchResult)
        {
            entry = ((LdapSearchResult)message).Entry;
            attr = entry.getAttribute("uid");
            groups.Add(attr.StringValue);
        }

        classConn.Disconnect();

        string[] ret = new string[groups.Count];
        ret = (string[])groups.ToArray(typeof(string));
        return ret;
    }

    public static int UserCount(LdapDetails ld)
    {
        if(classConn == null || !classConn.Connected)
            classConn = GetConnection(ld, true);

        LdapSearchQueue queue = classConn.Search(ld.UserBaseDN, LdapConnection.SCOPE_ONE, "uid=*", null, false, (LdapSearchQueue)null, (LdapSearchConstraints)null);

        int temp = 0;
        LdapMessage message = queue.getResponse();

        while(message != null && message is LdapSearchResult)
        {
            temp++;
            message = queue.getResponse();
        }

        classConn.Disconnect();

        return temp;
    }

    public static DataSet GetUserListPage(LdapDetails ld, int PageNumber, int PageSize, string NameBeginsWith)
    {
        DataTable dt = new DataTable();
			
        dt.Columns.Add("UserID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("WebSiteUrl", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("TotalPosts", typeof(int));

        if(classConn == null || !classConn.Connected)
            classConn = GetConnection(ld, true);

        NameBeginsWith = NameBeginsWith.ToLower();

        LdapSearchQueue queue = classConn.Search(ld.UserBaseDN, LdapConnection.SCOPE_ONE, "uid="+NameBeginsWith+"*", null, false, (LdapSearchQueue)null, (LdapSearchConstraints)null);

        int counted = 0;
        int messid = (PageNumber-1)*PageSize;
        LdapMessage message = queue.getResponse();

        DataRow dr;
        LdapEntry entry;
        LdapAttributeSet las;
        while(message != null && message is LdapSearchResult && counted < PageSize)
        {
            dr = dt.NewRow();
            entry = ((LdapSearchResult)message).Entry;

            las = entry.getAttributeSet();
            foreach(LdapAttribute a in las)
            {
                switch(a.Name)
                {
                    case "cn":
                        dr["Name"] = a.StringValue;
                        break;
                    case "uidNumber":
                        dr["UserID"] = Convert.ToInt32(a.StringValue);
                        break;
                    case "mail":
                        dr["Email"] = a.StringValue;
                        break;
                    case "url":
                        dr["WebSiteUrl"] = a.StringValue;
                        break;
                }
            }

            dt.Rows.Add(dr);

            messid++;
            counted++;
            message = queue.getResponse();
        }

        classConn.Disconnect();
			
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);

        return ds;
    }
    */

	#endregion
}