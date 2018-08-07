//  Author:
//  Created:       2013-04-01
//	Last Modified: 2018-07-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.SharedFilesUI;
using mojoPortal.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace mojoPortal.Features.UI.SharedFiles
{
	/// <summary>
	/// Handles file uploads for the Shared Files feature, called from jQueryFileUpload.cs
	/// </summary>
	public class upload : BaseContentUploadHandler, IHttpHandler
	{
		private string virtualSourcePath = string.Empty;
		private string virtualHistoryPath = string.Empty;
		private SharedFilesConfiguration config = new SharedFilesConfiguration();
		private int itemId = -1;
		private int currentFolderId = -1;
		private Module module = null;

		public void ProcessRequest(HttpContext context)
		{
			Initialize(context);

			if (!UserCanEditModule(ModuleId, SharedFile.FeatureGuid))
			{
				log.Info("User has no edit permission so returning 404");
				Response.StatusCode = 404;

				return;
			}

			if (CurrentSite == null)
			{
				log.Info("CurrentSite is null so returning 404");
				Response.StatusCode = 404;

				return;
			}

			if (CurrentUser == null)
			{
				log.Info("CurrentUser is null so returning 404");
				Response.StatusCode = 404;

				return;
			}

			if (FileSystem == null)
			{
				log.Info("FileSystem is null so returning 404");
				Response.StatusCode = 404;

				return;
			}

			if (Request.Files.Count == 0)
			{
				log.Info("Posted File Count is zero so returning 404");
				Response.StatusCode = 404;

				return;
			}

			if (Request.Files.Count > SharedFilesConfiguration.MaxFilesToUploadAtOnce)
			{
				log.Info("Posted File Count is greater than allowed amount so returning 404");
				Response.StatusCode = 404;

				return;
			}

			module = GetModule(ModuleId, SharedFile.FeatureGuid);

			if (module == null)
			{
				log.Info("Module is null so returning 404");
				Response.StatusCode = 404;

				return;
			}

			itemId = WebUtils.ParseInt32FromQueryString("ItemID", itemId);
			currentFolderId = WebUtils.ParseInt32FromQueryString("frmData", currentFolderId);

			virtualSourcePath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString() + "/SharedFiles/";
			virtualHistoryPath = "~/Data/Sites/" + CurrentSite.SiteId.ToInvariantString() + "/SharedFiles/History/";

			Hashtable moduleSettings = ModuleSettings.GetModuleSettings(ModuleId);
			config = new SharedFilesConfiguration(moduleSettings);

			context.Response.ContentType = "text/plain";

			List<UploadFilesResult> r = new List<UploadFilesResult>();
			JavaScriptSerializer js = new JavaScriptSerializer();

			if (!FileSystem.FolderExists(virtualSourcePath))
			{
				FileSystem.CreateFolder(virtualSourcePath);
			}

			for (int f = 0; f < Request.Files.Count; f++)
			{
				HttpPostedFile file = Request.Files[f];

				string fileName = Path.GetFileName(file.FileName);

				SharedFile sharedFile;

				if ((itemId > -1) && (Request.Files.Count == 1))
				{
					// updating an existing file
					sharedFile = new SharedFile(ModuleId, itemId);

					if (config.EnableVersioning)
					{
						bool historyCreated = SharedFilesHelper.CreateHistory(sharedFile, FileSystem, virtualSourcePath, virtualHistoryPath);

						if (historyCreated)
						{
							sharedFile.ServerFileName = $"{Guid.NewGuid().ToString()}.config";
						}
					}
				}
				else
				{   // new file
					sharedFile = new SharedFile();
					sharedFile.ViewRoles = "All Users";
				}

				sharedFile.ModuleId = ModuleId;
				sharedFile.ModuleGuid = module.ModuleGuid;
				sharedFile.OriginalFileName = fileName;
				sharedFile.FriendlyName = fileName;
				sharedFile.SizeInKB = (file.ContentLength / 1024);
				sharedFile.FolderId = currentFolderId;

				if (currentFolderId > -1)
				{
					SharedFileFolder folder = new SharedFileFolder(ModuleId, currentFolderId);
					sharedFile.FolderGuid = folder.FolderGuid;
					sharedFile.ViewRoles = folder.ViewRoles;
				}

				sharedFile.UploadUserId = CurrentUser.UserId;
				sharedFile.UserGuid = CurrentUser.UserGuid;
				sharedFile.UploadDate = DateTime.UtcNow;
				sharedFile.ContentChanged += new ContentChangedEventHandler(sharedFile_ContentChanged);

				if (sharedFile.Save())
				{
					string destPath = VirtualPathUtility.Combine(virtualSourcePath, sharedFile.ServerFileName);

					using (Stream s = file.InputStream)
					{
						FileSystem.SaveFile(destPath, s, IOHelper.GetMimeType(Path.GetExtension(sharedFile.FriendlyName).ToLower()), true);
					}
				}

				r.Add(new UploadFilesResult()
				{
					Name = fileName,
					Length = file.ContentLength,
					Type = file.ContentType
				});
			}

			CurrentPage.UpdateLastModifiedTime();
			CacheHelper.ClearModuleCache(ModuleId);
			SiteUtils.QueueIndexing();

			var uploadedFiles = new
			{
				files = r.ToArray()
			};

			var jsonObj = js.Serialize(uploadedFiles);
			context.Response.Write(jsonObj.ToString());
		}


		void sharedFile_ContentChanged(object sender, ContentChangedEventArgs e)
		{
			IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["SharedFilesIndexBuilderProvider"];

			if (indexBuilder != null)
			{
				indexBuilder.ContentChangedHandler(sender, e);
			}
		}


		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}