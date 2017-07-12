// Author:					
// Created:				    2009-12-26
// Last Modified:			2009-12-30
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Text;
using System.Web.UI;
using mojoPortal.Web.UI;
using mojoPortal.FileSystem;
using log4net;
using Resources;


namespace mojoPortal.Web.AdminUI
{
	public partial class AdvFileManager : UserControl
	{

		private static readonly ILog log = LogManager.GetLogger(typeof(AdvFileManager));
		private IFileSystem fileSystem = null;
		protected FileMgrViewData Model = new FileMgrViewData();
		private string siteRoot = string.Empty;
		protected string displayPath = string.Empty;
		//private char displayPathSeparator = '|';


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Visible) { return; }

			LoadSettings();
			//will be null if user has no permissions
			if (fileSystem == null)
			{
				pnlFile.Visible = false;
				lblDisabledMessage.Text = Resource.FileManagerDisabledMessage;
				return;
			}

			SetupScripts();
		}

		private void LoadSettings()
		{
			siteRoot = SiteUtils.GetNavigationSiteRoot();

			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
				return;
			}

			fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
				return;

			}


			displayPath = fileSystem.VirtualRoot.Replace("~/", "/").TrimEnd('/'); 

		  
			Model = new FileMgrViewData()
			{
				Folders = fileSystem.GetAllFolders(),
				CurrentFolder = fileSystem.VirtualRoot,
				Files = fileSystem.GetFileList(fileSystem.VirtualRoot ?? String.Empty)
			};

		}

		private void SetupScripts()
		{

			if (Page.Master != null)
			{
				ScriptLoader scriptLoader = Page.Master.FindControl("ScriptLoader1") as ScriptLoader;
				if (scriptLoader != null) { scriptLoader.IncludeQtFile = true; }


			}

			string fileSystemToken = Global.FileSystemToken.ToString();

			StringBuilder script = new StringBuilder();

			script.Append(" $('#qtfile').qtfile(");

			//options
			script.Append("{");

			script.Append("rootFolder:\"/\"");

			//begin file
			script.Append(",file: {");

			script.Append("download: {");
			//script.Append("url: '/UserFile/Download?username={username}&path={path}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=download&path={path}&t=" + fileSystemToken + "'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",move: {");
			//script.Append("url: '/UserFile/FileMove?srcPath={srcPath}&destPath={destPath}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=movefile&srcPath={srcPath}&destPath={destPath}&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",del: {");
			//script.Append("url: '/UserFile/FileDelete?path={path}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=deletefile&path={path}&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",list: {");
			//script.Append("url: '/UserFile/FileList?path={path}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=listfiles&path={path}&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",upload: {");
			//script.Append("url: '/UserFile/FileUpload'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=upload&t=" + fileSystemToken + "'");
			script.Append(",fileName: 'fileUploaded'");
			script.Append(",frameName: undefined");
			script.Append(",data: { }");
			script.Append("}");

			//end file
			script.Append("}");

			//begin folder
			script.Append(",folder: {");

			script.Append("create: {");
			//script.Append("url: '/UserFile/FolderCreate?path={path}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=createfolder&path={path}&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",move: {");
			//script.Append("url: '/UserFile/FolderMove?srcPath={srcPath}&destPath={destPath}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=movefolder&srcPath={srcPath}&destPath={destPath}&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",del: {");
			//script.Append("url: '/UserFile/FolderDelete?path={path}'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=deletefolder&path={path}&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			script.Append(",list: {");
			//script.Append("url: '/UserFile/FolderList'");
			script.Append("url:'" + siteRoot + "/Services/FileService.ashx?cmd=listfolders&t=" + fileSystemToken + "'");
			script.Append(",method: 'GET'");
			script.Append(",data: { }");
			script.Append("}");

			//end folder
			script.Append("}");
			

			//end options
			script.Append("}");

			script.Append("); ");

			Page.ClientScript.RegisterStartupScript(typeof(Page),
			   "qtfile-init", "\n<script type=\"text/javascript\" >"
			   + script.ToString() + "</script>");

		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
		}
	}
}