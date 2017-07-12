//  Author:                     
//  Created:                    2009-08-16
//	Last Modified:              2010-03-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Services.Protocols;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using log4net;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Returns html fragments representing folders and files.
    /// Used for populating the jQueryFileTree using the FileDialog.aspx page which is used in TinyMCE editor
    /// http://plugins.jquery.com/project/filetree
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class jqueryFileTreeMediaBrowser : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(jqueryFileTreeMediaBrowser));
        private SiteSettings siteSettings = null;
        private SiteUser currentUser = null;
        private string rootDir = string.Empty;
        private string currentDir = string.Empty;
        private string type = "image";
        private string serverName = string.Empty;
        private bool canView = false;
        private string allowedExtensions = string.Empty;
        private Page page = new Page();
        IFileSystem fileSystem = null;

        public void ProcessRequest(HttpContext context)
        {
            LoadSettings(context);

            if ((!canView)||(fileSystem == null))
            {
                context.Response.Write("<span class='txterror'>" + context.Server.HtmlEncode(Resource.AccessDeniedLabel) + "</span>");
                return;
            }
            
            RenderFileTree(context);

        }

        private void LoadSettings(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            //this is only used to resolve the paths since httphandler does not have it built in
            
            page.AppRelativeVirtualPath = context.Request.AppRelativeCurrentExecutionFilePath;

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

            rootDir = fileSystem.VirtualRoot.Replace("~",string.Empty);

            if ((WebUser.IsAdminOrContentAdmin)||(SiteUtils.UserIsSiteEditor()))
            {
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                canView = true;
            }
            else if (WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
            {
                allowedExtensions = WebConfigSettings.AllowedUploadFileExtensions;
                canView = true;

            }
            else if (WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
            {
                currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null) { return; }
                allowedExtensions = WebConfigSettings.AllowedLessPriveledgedUserUploadFileExtensions;
                canView = true;
            }

            if (!canView) { return; }

            currentDir = rootDir;

            if (context.Request.Params.Get("dir") != null)
            {
                string requestedDir = context.Server.UrlDecode(context.Request.Params.Get("dir"));
                
                if (requestedDir == "/Pages/")
                {
                    currentDir = requestedDir;
                }
                else
                {
                    if (IsChildDirectory(context, requestedDir)) { currentDir = requestedDir; ; }
                }
            }

            ResolveType(context);

        }

        private void RenderFileTree(HttpContext context)
        {
            
            context.Response.Write("<ul class=\"jqueryFileTree\" style=\"display: none;\">\n");

            if (type == "file")
            {
                //this allows browsingpages in the links browser
                if (currentDir == "/Pages/")
                {
                    RenderPages(context);
                }
                else if((currentDir == rootDir)||(currentDir == "~" + rootDir))
                {
                    context.Response.Write("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + "/Pages" + "/\">" + context.Server.HtmlEncode(Resource.PageBrowseNode) + "</a></li>\n");
                }
            }

            if (!(currentDir == "/Pages/"))
            {
                IEnumerable<WebFolder> folders = fileSystem.GetFolderList(currentDir);
                foreach (WebFolder folder in folders)
                {
                    context.Response.Write("\t<li class=\"directory collapsed\"><a href=\"#\" rel=\"" + currentDir.Replace("~", string.Empty) + folder.Name + "/\">" + folder.Name + "</a></li>\n");
                }

                if(type != "folder")
                {
                    IEnumerable<WebFile> files = fileSystem.GetFileList(currentDir);
                    foreach (WebFile fileInfo in files)
                    {
                        if ((type == "image") && (!fileInfo.IsWebImageFile())) { continue; }

                        if ((type == "media") && (!fileInfo.IsAllowedMediaFile())) { continue; }

                        if ((type == "audio") && (!fileInfo.IsAllowedMediaFile())) { continue; }

                        if ((type == "video") && (!fileInfo.IsAllowedMediaFile())) { continue; }

                        if ((type == "audio") && (!fileInfo.IsAudioFile())) { continue; }

                        if ((type == "video") && (!fileInfo.IsVideoFile())) { continue; }

                        if ((type == "file") && (!fileInfo.IsAllowedUploadBrowseFile(allowedExtensions))) { continue; }

                        string ext = "";
                        if (fileInfo.Extension.Length > 1) { ext = fileInfo.Extension.Substring(1).ToLower(); }
                        context.Response.Write("\t<li class=\"file ext_" + ext + "\"><a href=\"#\" rel=\"" + fileSystem.FileBaseUrl + currentDir.Replace("~", string.Empty) + fileInfo.Name + "\">" + fileInfo.Name + "</a></li>\n");
                    }

                }
                

            }
            
            context.Response.Write("</ul>");
           
        }

        private void RenderPages(HttpContext context)
        {
            serverName = WebUtils.GetHostName();
            string serverPort = context.Request.ServerVariables["SERVER_PORT"];
            if ((serverPort != "80") && (serverPort != "443"))
            {
                serverName += ":" + serverPort;
            }

            SiteMapDataSource siteMapDataSource = new SiteMapDataSource();

            siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            RenderSiteMapNodes(context, siteMapNode, string.Empty);

        }

        private void RenderSiteMapNodes(
            HttpContext context,
            SiteMapNode siteMapNode,
            string pagePrefix)
        {
            mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

            if (!mojoNode.IsRootNode)
            {
                if (WebUser.IsInRoles(mojoNode.ViewRoles))
                {
                    if (mojoNode.ParentId > -1) pagePrefix += "-";
                    context.Response.Write("\t<li class=\"file ext_txt\"><a href=\"#\" rel=\"" + page.ResolveUrl(mojoNode.Url) + "\">" + pagePrefix + mojoNode.Title + "</a></li>\n");

                }
            }

            foreach (SiteMapNode childNode in mojoNode.ChildNodes)
            {
                //recurse to populate children
                RenderSiteMapNodes(context, childNode, pagePrefix);

            }

        }

        private bool IsChildDirectory(HttpContext context, string requestedDirectory)
        {
            return WebFolder.IsDecendentVirtualPath(fileSystem.VirtualRoot, requestedDirectory);
        }

        private void ResolveType(HttpContext context)
        {
            string requestedType = "image";
            if (context.Request.QueryString["type"] != null)
            {
                requestedType = context.Request.QueryString["type"];
            }

            switch (requestedType)
            {
                case "media":
                    type = "media";
                    break;

                case "audio":
                    type = "audio";
                    break;

                case "video":
                    type = "video";
                    break;

                case "file":
                    type = "file";
                    break;

                case "folder":
                    type = "folder";
                    break;

                case "image":
                default:
                    type = "image";
                    break;

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
