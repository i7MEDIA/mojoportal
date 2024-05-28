using System.Collections.Generic;
using System.Configuration;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business;

/// <summary>
/// Represents a site folder used to demark a sub site in a multi site installation using folder based child sites.
/// </summary>
public class SiteFolder
{

	#region Constructors

	public SiteFolder()
	{ }


	public SiteFolder(Guid guid)
	{
		GetSiteFolder(guid);
	}

	#endregion

	#region Public Properties

	public Guid @Guid { get; set; } = Guid.Empty;
	public Guid SiteGuid { get; set; } = Guid.Empty;
	public string FolderName { get; set; } = string.Empty;

	#endregion

	#region Private Methods

	private void GetSiteFolder(Guid guid)
	{
		using IDataReader reader = DBSiteFolder.GetOne(guid);
		if (reader.Read())
		{
			Guid = new Guid(reader["Guid"].ToString());
			SiteGuid = new Guid(reader["SiteGuid"].ToString());
			FolderName = reader["FolderName"].ToString();
		}
	}

	private bool Create() => DBSiteFolder.Add(Guid.NewGuid(), SiteGuid, FolderName) > 0;

	private bool Update() => DBSiteFolder.Update(Guid, SiteGuid, FolderName);


	#endregion

	#region Public Methods


	public bool Save()
	{
		if (!IsAllowedFolder(FolderName))
		{
			throw new ArgumentException("Invalid Folder Name");
		}

		if (HasInvalidChars(FolderName))
		{
			throw new ArgumentException("Invalid Folder Name");
		}

		if (Guid != Guid.Empty)
		{
			return Update();
		}
		else
		{
			return Create();
		}
	}

	#endregion

	#region Static Methods

	public static Guid GetSiteGuid(string folderName) => DBSiteFolder.GetSiteGuid(folderName);

	public static bool Delete(Guid guid) => DBSiteFolder.Delete(guid);

	public static bool Exists(string folderName) => DBSiteFolder.Exists(folderName);


	public static bool IsAllowedFolder(string folderName)
	{
		bool result = true;
		if (ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"] is not null)
		{
			string[] disallowedNames
				= ConfigurationManager.AppSettings["DisallowedVirtualFolderNames"].Split([';']);

			foreach (string disallowedName in disallowedNames)
			{
				if (string.Equals(folderName, disallowedName, StringComparison.InvariantCultureIgnoreCase))
				{
					result = false;
				}
			}
		}

		return result;
	}


	public static bool HasInvalidChars(string folderName)
	{
		var result = false;
		char[] nameChars = folderName.ToCharArray();
		foreach (char c in nameChars)
		{
			if (!Char.IsLetterOrDigit(c) && (c != '-')) // allow dashes 2014-01-10
			{
				result = true;
			}
		}

		return result;
	}


	public static List<SiteFolder> GetBySite(Guid siteGuid)
	{
		var siteFolderList = new List<SiteFolder>();

		using (IDataReader reader = DBSiteFolder.GetBySite(siteGuid))
		{
			while (reader.Read())
			{
				var siteFolder = new SiteFolder
				{
					Guid = new Guid(reader["Guid"].ToString()),
					SiteGuid = new Guid(reader["SiteGuid"].ToString()),
					FolderName = reader["FolderName"].ToString()
				};
				siteFolderList.Add(siteFolder);
			}
		}

		return siteFolderList;
	}


	#endregion

	#region Comparison Methods

	public static int CompareByFolderName(SiteFolder siteFolder1, SiteFolder siteFolder2) => siteFolder1.FolderName.CompareTo(siteFolder2.FolderName);

	#endregion
}
