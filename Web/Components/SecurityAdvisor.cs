using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Configuration;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;

namespace mojoPortal.Web
{
	public class SecurityAdvisor
	{
		//private List<string> writableFolders = new List<string>();

		//public List<string> WritableFolders
		//{
		//	get { return writableFolders; }
		//}

		static readonly object lockObject = new object();


		public SecurityAdvisor()
		{ }


		public bool UsingCustomMachineKey()
		{
			var configSection = ConfigHelper.GetMachineKeySection();
			var oldValidationKey = "55BA53B475CCAE0992D6BF9FE463A5E97F00C6C16DA3D7DF9202E560078AB501643C15514785FEE30FEF26FC27F5CE594B42FFCA55452EF90E8A056B4DAE9F39";
			var oldDecryptionKey = "939232D527AC4CD3E449441FE887DA110A16C1A36924C424CBAAE3F00282436C";
			var currentValidationKey = "BC4BACC8639CA465C9EEB8ED7C1718B9DD8D29C26CD4D3600DD6AFB5443C436A";
			var currentDecryptionKey = "81F52D8FFA4F6934328032C3A6667A724FF3CD338F99E13B";

			if (
				oldValidationKey == configSection.ValidationKey &&
				oldDecryptionKey == configSection.DecryptionKey ||
				currentValidationKey == configSection.ValidationKey &&
				currentDecryptionKey == configSection.DecryptionKey
			)
			{
				return false;
			}

			return true;

			//mojoMembershipProvider mojoMembership = Membership.Provider as mojoMembershipProvider;

			//if (mojoMembership != null)
			//{
			//	var encoded = mojoMembership.EncodePassword(WebConfigSettings.TestDecryptedValueForDefaultMachineKey, MembershipPasswordFormat.Encrypted);

			//	if (encoded == WebConfigSettings.TestEncryptedValueForDefaultMachineKey)
			//	{
			//		return false;
			//	}
			//}

			//return true;
		}


		public List<string> GetWritableFolders()
		{
			var cacheKey = "installationwritablefolders";

			if (HttpRuntime.Cache[cacheKey] == null)
			{
				lock (lockObject) //only 1 thread at a time should be able to execute this code
				{
					if (HttpRuntime.Cache[cacheKey] != null)
					{
						return HttpRuntime.Cache[cacheKey] as List<string>;
					}

					var writableFolders = LoadWritableFolders();

					HttpRuntime.Cache.Insert(cacheKey, writableFolders, null, DateTime.UtcNow.AddSeconds(600), TimeSpan.Zero);
				}
			}

			return HttpRuntime.Cache[cacheKey] as List<string>;
		}


		/// <summary>
		/// Determine if default admin account and password are in use
		/// </summary>
		/// <returns>(userExists, passwordIsDefault)</returns>
		public (bool userExists, bool passwordIsDefault) DefaultAdmin()
		{
			var defaultEmail = "admin@admin.com";
			var defaultPassword = "admin";
			var encodedPassword = string.Empty;
			var defaultAdminUser = SiteUser.GetByEmail(CacheHelper.GetCurrentSiteSettings(), defaultEmail);

			if (defaultAdminUser == null)
			{
				return (false, false);
			}

			if (Membership.Provider is mojoMembershipProvider membershipProvider)
			{
				encodedPassword = membershipProvider.EncodePassword(defaultPassword, defaultAdminUser.PasswordSalt, MembershipPasswordFormat.Encrypted);
			}

			return (true, defaultAdminUser.Password == defaultPassword || defaultAdminUser.Password == encodedPassword);
		}


		private List<string> LoadWritableFolders()
		{
			var writableFolders = new List<string>();
			var rootDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~/"));

			TestWriteToDirectory(writableFolders, rootDirectory, Resource.RootDirectory, false);

			var subDirectories = rootDirectory.GetDirectories();

			foreach (var subDirectory in subDirectories)
			{
				if (IsExpectedWritableFolder(subDirectory.Name))
				{
					continue;
				}

				TestWriteToDirectory(writableFolders, subDirectory, string.Empty, true);
			}

			return writableFolders;
		}


		private bool IsExpectedWritableFolder(string folderName)
		{
			if (string.Equals(folderName, "App_Data", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			if (string.Equals(folderName, "Data", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}


		private void TestWriteToDirectory(List<string> writableFolders, DirectoryInfo directory, string label, bool deleteTestFile)
		{
			var fileName = "securityadvisortest_" + Guid.NewGuid().ToString() + " .txt";
			var fullPath = directory.FullName + Path.DirectorySeparatorChar + fileName;

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

				if (deleteTestFile)
				{
					File.Delete(fullPath);
				}
			}
			catch
			{ }
		}
	}
}
