// Author:					TJ Fontaine
// Created:				    2005-09-30
// Last Modified:			2013-11-20
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
using System.Configuration;
using Mono.Security.X509;
using log4net;
using Novell.Directory.Ldap;
using System.DirectoryServices;

namespace mojoPortal.Business
{
    /// <summary>
    ///
    /// </summary>
    public sealed class LdapHelper
    {
        private LdapHelper()
        {

        }

        private static readonly ILog log = LogManager.GetLogger(typeof(LdapHelper));

        #region Static Methods

        private static LdapConnection GetConnection(LdapSettings ldapSettings)
        {
            LdapConnection conn = new LdapConnection();

            bool useSsl = false; 
            if (ConfigurationManager.AppSettings["UseSslForLdap"] != null)
            {
                useSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSslForLdap"]);
            }

            if (useSsl)
            {
                // make this support ssl/tls
                //http://stackoverflow.com/questions/386982/novell-ldap-c-novell-directory-ldap-has-anybody-made-it-work
                conn.SecureSocketLayer = true;
                conn.UserDefinedServerCertValidationDelegate += new CertificateValidationCallback(LdapSSLHandler);

            }

            conn.Connect(ldapSettings.Server, ldapSettings.Port);
            
            return conn;
        }

        public static bool LdapSSLHandler(System.Security.Cryptography.X509Certificates.X509Certificate certificate, int[] certificateErrors)
        {

#if !MONO
            X509Store store = null;
            X509Stores stores = X509StoreManager.LocalMachine;
            store = stores.TrustedRoot;
            X509Certificate x509 = null;
            
            byte[] data = certificate.GetRawCertData();
            if (data != null) { x509 = new X509Certificate(data); }

            if (x509 != null)
            {
                //coll.Add(x509);
                if (!store.Certificates.Contains(x509))
                {
                    
                    store.Import(x509);
                }
                
            }
#endif
            
            return true;
        }

        private static LdapEntry GetOneUserEntry(
            LdapConnection conn, 
            LdapSettings ldapSettings, 
            string search)
        {

            LdapSearchConstraints constraints = new LdapSearchConstraints();
            
            LdapSearchQueue queue = null;
            queue = conn.Search(
                ldapSettings.RootDN,
                LdapConnection.SCOPE_SUB,
                ldapSettings.UserDNKey + "=" + search, 
                null,
                false, 
                (LdapSearchQueue)null, 
                (LdapSearchConstraints)null);

            LdapEntry entry = null;

            if (queue != null)
            {
                LdapMessage message = queue.getResponse();
                if (message != null)
                {
                    if (message is LdapSearchResult)
                    {
                        entry = ((LdapSearchResult)message).Entry;
                    }
                }
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
                    string msg = "Login failure for user: " + uid + ". Exception: ";
                    log.Error(msg, ex);
                }
            }

            if ((conn != null) && (conn.Connected))
            {
                LdapEntry entry = null;

                try
                {
                    entry = GetOneUserEntry(conn, ldapSettings, uid);
                    if (entry != null)
                    {
                        LdapConnection authConn = GetConnection(ldapSettings);
                        authConn.Bind(entry.DN, password);
                        authConn.Disconnect();
                        success = true;

                    }
                }
                catch (Novell.Directory.Ldap.LdapException ex)
                {
                    if (log.IsErrorEnabled)
                    {
                        //log.Error("login failure", ex);
                        string msg = "Login failure for user: " + uid + ". Exception: ";
                        log.Error(msg, ex);
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
            if (ConfigurationManager.AppSettings["UseRootDNWithActiveDirectory"] != null)
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["UseRootDNWithActiveDirectory"]);
            }

            return false;
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
                    adentry = new DirectoryEntry("LDAP://" + ldapSettings.Server + "/" + ldapSettings.RootDN, ldapSettings.Domain + "\\" + uid, password);
                }
                else
                {
                    adentry = new DirectoryEntry("LDAP://" + ldapSettings.Server, ldapSettings.Domain + "\\" + uid, password);
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (log.IsErrorEnabled)
                {
                    //log.Error("couldn't connect to ldap server ", ex);
                    string msg = "Login failure for user: " + uid + ". Exception: ";
                    log.Error(msg, ex);
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
                        DirectorySearcher ds = new DirectorySearcher(adentry);
                        ds.Filter = "(&(sAMAccountName=" + uid + "))";
                        SearchResult result = ds.FindOne();
                        if (result != null)
                        {
                            //log.Error("successful authentication to ldap server in OU with Server: " + ldapSettings.Server + "; RootDN: " + ldapSettings.RootDN + "; UID=" + uid);
                            user = new LdapUser(adentry, uid, ldapSettings);
                        }
                        else
                        {
                            log.Info("failed authentication to ldap server in OU with Server: " + ldapSettings.Server + "; RootDN: " + ldapSettings.RootDN + "; UID=" + uid);
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
            bool result = false;

            LdapUser testUser = LdapLogin(ldapSettings, uid, password);
            if (testUser != null)
            {
                result = true;
            }

            return result;
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
}
