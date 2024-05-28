using System;
using System.Data.Common;
using System.Web;

namespace mojoPortal.Business.WebHelpers;

public static class VirtualFolderEvaluator
{
	/// <summary>
	/// find first level folder name after site root
	/// </summary>
	/// <returns></returns>
	public static string VirtualFolderName()
	{
		if (HttpContext.Current == null)
		{
			return string.Empty;
		}

		if (HttpContext.Current.Items["VirtualFolderName"] is not string folderName)
		{
			folderName = GetVirtualFolderName();

			if (folderName is not null)
			{
				HttpContext.Current.Items["VirtualFolderName"] = folderName;
			}
		}

		return folderName;
	}

	private static string GetVirtualFolderName()
	{
		if (HttpContext.Current == null)
		{
			return string.Empty;
		}

		// find first level folder name after site root
		var folderName = string.Empty;

		var requestPath = HttpContext.Current.Request.RawUrl.Replace("https://", string.Empty).Replace("http://", string.Empty);

		if (requestPath == "/")
		{
			return folderName;
		}

		requestPath = requestPath.TrimStart('/');

		if (requestPath.IndexOf("/") > -1)
		{
			requestPath = requestPath.Substring(0, requestPath.IndexOf("/"));
		}

		try
		{
			if (SiteFolder.Exists(requestPath))
			{
				folderName = requestPath;
			}
		}
		catch (DbException)
		{
			// folderName = string.Empty;
		}
		catch (InvalidOperationException)
		{
			// occurs when db tables and procs haven't been created yet
			// in MS SQL
			// folderName = string.Empty;
		}

		//}

		return folderName;
	}
}
