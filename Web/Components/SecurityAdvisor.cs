// Author:					
// Created:				    2010-09-19
// Last Modified:			2010-09-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.Hosting;
using Resources;

namespace mojoPortal.Web
{
    public class SecurityAdvisor
    {
        //private List<string> writableFolders = new List<string>();

        //public List<string> WritableFolders
        //{
        //    get { return writableFolders; }
        //}

        static readonly object lockObject = new object();

        public SecurityAdvisor()
        { }

        public bool UsingCustomMachineKey()
        {
            mojoMembershipProvider mojoMembership = Membership.Provider as mojoMembershipProvider;
            if (mojoMembership != null)
            {
                string encoded = mojoMembership.EncodePassword(WebConfigSettings.TestDecryptedValueForDefaultMahineKey, MembershipPasswordFormat.Encrypted);
                if (encoded == WebConfigSettings.TestEncryptedValueForDefaultMahineKey) { return false; }
            }

            return true;
        }

        public List<string> GetWritableFolders()
        {
            string cacheKey = "installationwritablefolders";

            if (HttpRuntime.Cache[cacheKey] == null)
            {
                lock (lockObject) //only 1 thread at a time should be able to execute this code
                {
                    if (HttpRuntime.Cache[cacheKey] != null)
                    {
                        return HttpRuntime.Cache[cacheKey] as List<string>;
                    }

                    List<string> writableFolders = LoadWritableFolders();

                    HttpRuntime.Cache.Insert(cacheKey, writableFolders, null, DateTime.UtcNow.AddSeconds(600), TimeSpan.Zero);

                }
            }

            return HttpRuntime.Cache[cacheKey] as List<string>;

        }

        private List<string> LoadWritableFolders()
        {
            List<string> writableFolders = new List<string>();

            DirectoryInfo rootDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/"));
            TestWriteToDirectory(writableFolders, rootDirectory, Resource.RootDirectory, false);

            DirectoryInfo[] subDirectories = rootDirectory.GetDirectories();

            foreach (DirectoryInfo d in subDirectories)
            {
                if (IsExpectedWritableFolder(d.Name)) { continue; }

                TestWriteToDirectory(writableFolders, d, string.Empty, true);
            }

            return writableFolders;
        }

        private bool IsExpectedWritableFolder(string folderName)
        {
            if (string.Equals(folderName, "App_Data", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(folderName, "Data", StringComparison.InvariantCultureIgnoreCase)) { return true; }

            return false;
        }

        private void TestWriteToDirectory(List<string> writableFolders, DirectoryInfo directory, string label, bool deleteTestFile)
        {
            string fileName = "securityadvisortest_" + Guid.NewGuid().ToString() + " .txt";
            string fullPath = directory.FullName + Path.DirectorySeparatorChar + fileName;

            try
            {
                using (StreamWriter streamWriter = File.CreateText(fullPath))
                { }
                //StreamWriter streamWriter = File.CreateText(fullPath);
                //streamWriter.Close();

                if (string.IsNullOrEmpty(label))
                {
                    writableFolders.Add(directory.Name);
                }
                else
                {
                    writableFolders.Add(label);
                }

                if (deleteTestFile) { File.Delete(fullPath); }

            }
            catch { }

        }
    }
}
