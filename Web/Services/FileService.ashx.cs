// Author:					
// Created:				    2009-12-28
// Last Modified:			2014-11-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;


namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Service endpoint for file operations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FileService : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileService));
        //private bool logAllFileSystemActivity = false;
        IFileSystem fileSystem = null;
        private char displayPathSeparator = '|';
        protected bool overwriteExistingFiles = false;
        private const string DefaultFilePath = "~/Data";
        private string folder = string.Empty;
        private string siteRoot = string.Empty;

        private string cmd = string.Empty;
        private string pipedPath = string.Empty;
        private string pipedSourcePath = string.Empty;
        private string pipedTargetPath = string.Empty;

        private string virtualPath = string.Empty;
        private string virtualSourcePath = string.Empty;
        private string virtualTargetPath = string.Empty;

        HttpPostedFile fileUploaded = null;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

       
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                LoadSettings(context);

                if(WebConfigSettings.FileServiceRejectFishyPosts)
                {
                    if(SiteUtils.IsFishyPost(context.Request))
                    {
                        RenderJsonResult(context, OpResult.Error);
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                RenderJsonResult(context, OpResult.Error);
                return;
            }

            //will be null if user has not permissions
            if ((fileSystem == null)||(!fileSystem.UserHasUploadPermission))
            {
                //log.Info("Could not load file system or user not allowed so blocking access.");
                RenderJsonResult(context, OpResult.Denied);
                return;
            }

            switch (cmd)
            {
                case "download":
                    Download(context);
                    break;

                case "movefile":
                    MoveFile(context);
                    break;

                case "deletefile":
                    DeleteFile(context);
                    break;

                case "listfiles":
                    ListFiles(context);
                    break;

                case "upload":
                    Upload(context);
                    break;

                case "uploadfromeditor":
                    UploadFromDialog(context);
                    break;

                case "uploadskin":
                    UploadSkin(context);
                    break;

                case "uploadfm":
                    UploadFromFileManager(context);
                    break;

                case "createfolder":
                    CreateFolder(context);
                    break;

                case "movefolder":
                    MoveFolder(context);
                    break;

                case "deletefolder":
                    DeleteFolder(context);
                    break;

                case "listfolders":
                    ListFolders(context);
                    break;

                default:
                    RenderJsonResult(context, OpResult.Error);
                    break;
            }

        }

        #region File Methods


        private void Download(HttpContext context)
        {
            try
            {
                if (OnFileDownloading(virtualPath))
                {
                    WebFile file = fileSystem.RetrieveFile(virtualPath);
                    
                    if (file != null)
                    {
                        if (!fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(file.Name)))
                        {
                            RenderJsonResult(context, OpResult.FileTypeNotAllowed);
                            //context.Response.StatusCode = 500;
                            return;
                        }

                        if (file.Path != null)
                        {
                            context.Response.AppendHeader("Content-Length", file.Size.ToString(CultureInfo.InvariantCulture));
                            context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name.Trim() + "\"");

                            try
                            {
                                if (WebConfigSettings.DownloadScriptTimeout > -1)
                                {
                                    context.Server.ScriptTimeout = WebConfigSettings.DownloadScriptTimeout;
                                }

                                context.Response.Buffer = false;
                                context.Response.BufferOutput = false;
                                // this won't work for Azure storage
                                //context.Response.TransmitFile(file.Path);
                                // so we use this approach
                                using (System.IO.Stream stream = fileSystem.GetAsStream(virtualPath))
                                {
                                    stream.CopyTo(context.Response.OutputStream);
                                }

                                context.Response.End();
                            }
                            catch (System.Threading.ThreadAbortException) { } // can happen with Response.End
                            catch (HttpException ex)
                            {
                                log.Error(ex);
                                context.Response.StatusCode = 500;
                            }

                            return;
                        }
                        
                        
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //context.Response.StatusCode = 500;
            }

            // Requested file not found
            context.Response.StatusCode = 404;

        }

        private void UploadFromFileManager(HttpContext context)
        {
            if (context.Request.Files.Count == 0)
            {
                log.Info("No files posted so returning 404");
                context.Response.StatusCode = 404;
                return;
            }

            if (WebConfigSettings.DisableFileManager)
            {
                log.Info("File Manager disabled by web.config");
                context.Response.StatusCode = 404;
                return;
            }

            if (fileSystem == null)
            {
                log.Info("FileSystem is null so returning 404");
                context.Response.StatusCode = 404;
                return;
            }

            
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            bool canAccess = (WebUser.IsAdminOrContentAdmin 
                || WebUser.IsInRoles(siteSettings.RolesThatCanDeleteFilesInEditor) 
                || SiteUtils.UserIsSiteEditor());

            if (!canAccess)
            {
                context.Response.StatusCode = 404;
                return;
            }

            virtualPath = fileSystem.VirtualRoot;

            if (context.Request.Params["frmData"] != null)
            {
                virtualPath = context.Request.Params["frmData"].Replace("..", string.Empty);
            }

            if (virtualPath.StartsWith("/")) { virtualPath = "~" + virtualPath; }
            if (!virtualPath.EndsWith("/")) { virtualPath = virtualPath + "/"; }

            if (!virtualPath.StartsWith(fileSystem.VirtualRoot))
            {
                log.Info("returning 404 for upload request to " + virtualPath);
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new System.Collections.Generic.List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();


            for (int f = 0; f < context.Request.Files.Count; f++)
            {
                HttpPostedFile file = context.Request.Files[f];
                bool doUpload = true;

                if (file.ContentLength > fileSystem.Permission.MaxSizePerFile)
                {
                    log.Info("upload rejected due to fileSystem.Permission.MaxSizePerFile");
                    doUpload = false;    
                }
                else if (fileSystem.CountAllFiles() >= fileSystem.Permission.MaxFiles)
                {
                    log.Info("upload rejected due to fileSystem.Permission.MaxFiles");
                    doUpload = false; 
                }
                else if (fileSystem.GetTotalSize() + file.ContentLength >= fileSystem.Permission.Quota)
                {
                    log.Info("upload rejected due to fileSystem.Permission.Quota");
                    doUpload = false;
                }

                if (!fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(file.FileName)))
                {
                    log.Info("upload rejected due to not allowed file extension");
                    doUpload = false;
                }

                string destPath = VirtualPathUtility.Combine(virtualPath, Path.GetFileName(file.FileName));

                string ext = Path.GetExtension(file.FileName);
                string mimeType = IOHelper.GetMimeType(ext);

                if (doUpload)
                {
                    using (Stream s = file.InputStream)
                    {
                        fileSystem.SaveFile(destPath, s, mimeType, true);
                    }

                    r.Add(new UploadFilesResult()
                    {
                        //Thumbnail_url = 
                        Name = file.FileName,
                        Length = file.ContentLength,
                        Type = mimeType
                    });
                }
            }

            var uploadedFiles = new
            {
                files = r.ToArray()
            };

            var jsonObj = js.Serialize(uploadedFiles);
            context.Response.Write(jsonObj.ToString());

        }


        private void UploadSkin(HttpContext context)
        {
            if (context.Request.Files.Count == 0)
            {
                log.Info("No files posted so returning 404");
                context.Response.StatusCode = 404;
                return;
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            if ((!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
            {
                context.Response.StatusCode = 404;
                return;
            }

            bool overwriteFiles = true;

            if (context.Request.Params["frmData"] != null)
            {
                if (context.Request.Params["frmData"] == "off")
                {
                    overwriteFiles = false;
                }

            }

            string destFolder = SiteUtils.GetSiteSystemFolder();

            DirectoryInfo di = new DirectoryInfo(destFolder);
            if (!di.Exists)
            {
                di.Create();
            }

            context.Response.ContentType = "text/plain";//"application/json";
            var r = new List<UploadFilesResult>();
            JavaScriptSerializer js = new JavaScriptSerializer();

            for (int f = 0; f < context.Request.Files.Count; f++)
            {
                HttpPostedFile file = context.Request.Files[f];

                string ext = System.IO.Path.GetExtension(file.FileName).ToLower().Replace(".",string.Empty);

                if (ext == "zip")
                {
                    string destPath = Path.Combine(destFolder,
                        Path.GetFileName(file.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles));

                    if (File.Exists(destPath))
                    {
                        File.Delete(destPath);
                    }

                    file.SaveAs(destPath);

                    // process the .zip, extract files
                    mojoPortal.Web.Components.SkinHelper helper = new mojoPortal.Web.Components.SkinHelper();
                    helper.InstallSkins(SiteUtils.GetSiteSkinFolderPath(), destPath, overwriteFiles);

                    // delete the temporary file
                    File.Delete(destPath);

                    r.Add(new UploadFilesResult()
                    {
                        //Thumbnail_url = 
                        Name = file.FileName,
                        Length = file.ContentLength,
                        Type = file.ContentType
                    });

                }     
            }

            var uploadedFiles = new
            {
                files = r.ToArray()
            };
            var jsonObj = js.Serialize(uploadedFiles);
            context.Response.Write(jsonObj.ToString());


        }

        private void UploadFromDialog(HttpContext context)
        {
            if (context.Request.Files.Count == 0)
            {
                log.Info("No files posted so returning 404");
                context.Response.StatusCode = 404;
                return;
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) 
            {
                log.Info("Null SiteSettings so returning 404");
                context.Response.StatusCode = 404;
                return; 
            }
            if ((!WebUser.IsInRoles(siteSettings.GeneralBrowseAndUploadRoles))
                &&(!WebUser.IsInRoles(siteSettings.UserFilesBrowseAndUploadRoles))
                &&(!WebUser.IsContentAdmin)
                )
            {
                log.Info("user not in allowed upload roles so returning 404");
                context.Response.StatusCode = 404;
                return;
            }

            virtualPath = fileSystem.VirtualRoot;

            if (context.Request.Params["fld"] != null)
            {
                //log.Info("fld was " + context.Request.Params["fld"]);
                virtualPath = context.Request.Params["fld"].Replace("..", string.Empty);
            }

            if (virtualPath.StartsWith("/")) { virtualPath = "~" + virtualPath; }
            if (!virtualPath.EndsWith("/")) { virtualPath = virtualPath + "/"; }

            if (!virtualPath.StartsWith(fileSystem.VirtualRoot))
            {
                log.Info("returning 404 for upload request to " + virtualPath);
                context.Response.StatusCode = 404;
                return;
            }

            int resizeWidth = WebConfigSettings.ResizeImageDefaultMaxWidth;
            int resizeHeight = WebConfigSettings.ResizeImageDefaultMaxHeight;
            bool resize = false;
            bool keepOriginalSize = WebConfigSettings.KeepFullSizeEditorUploadedImages;

            if (context.Request.Params["maxW"] != null)
            {
                //log.Info("maxW was " + context.Request.Params["maxW"]);
                int.TryParse(context.Request.Params["maxW"], out resizeWidth);

            }

            if (context.Request.Params["maxH"] != null)
            {
                //log.Info("maxH was " + context.Request.Params["maxH"]);
                int.TryParse(context.Request.Params["maxH"], out resizeHeight);

            }

            if (context.Request.Params["rz"] != null)
            {
                string rz = context.Request.Params["rz"];
                //log.Info("rz was " + context.Request.Params["rz"]);
                if (rz == "true")
                {
                    resize = true;
                }

            }

            if (context.Request.Params["ko"] != null)
            {
                string ko = context.Request.Params["ko"];
                //log.Info("rz was " + context.Request.Params["rz"]);
                if (ko == "true")
                {
                    keepOriginalSize = true;
                }

            }

            fileUploaded = context.Request.Files[0];

            bool doUpload = true;

            int contentLength = fileUploaded.ContentLength;

            if (contentLength > fileSystem.Permission.MaxSizePerFile)
            {
                log.Info("upload rejected due to fileSystem.Permission.MaxSizePerFile");
                doUpload = false;
                
            }
            else if (fileSystem.CountAllFiles() >= fileSystem.Permission.MaxFiles)
            {
                log.Info("upload rejected due to fileSystem.Permission.MaxFiles");
                doUpload = false;
                

            }
            else if (fileSystem.GetTotalSize() + contentLength >= fileSystem.Permission.Quota)
            {
                log.Info("upload rejected due to fileSystem.Permission.Quota");
                doUpload = false;
               
            }


            if (!fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(fileUploaded.FileName)))
            {
                log.Info("upload rejected due to not allowed file extension");
                doUpload = false;
                
            }

            string newFileName = Path.GetFileName(fileUploaded.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);

            string destPath = VirtualPathUtility.Combine(virtualPath, newFileName);

            string ext = Path.GetExtension(fileUploaded.FileName).ToLower();
            string mimeType = IOHelper.GetMimeType(ext);
            string fullSizeFilePath = destPath.Replace(ext, "-fs" + ext);

            if (doUpload)
            {
                //context.Response.ContentType = "text/plain";//"application/json";
                context.Response.ContentType = "application/json";
                var r = new List<UploadFilesResult>();
                JavaScriptSerializer js = new JavaScriptSerializer();

                try
                {
                    using (fileUploaded.InputStream)
                    {
                        fileSystem.SaveFile(destPath, fileUploaded.InputStream, IOHelper.GetMimeType(ext), true);
                    }

                    if(resize && keepOriginalSize)
                    {
                        fileSystem.CopyFile(destPath, fullSizeFilePath, true);
                    }

                    if (SiteUtils.IsImageFileExtension(ext))
                    {
                        if (resize)
                        {
                            mojoPortal.Web.ImageHelper.ResizeImage(
                                destPath,
                                mimeType,
                                resizeWidth,
                                resizeHeight,
                                WebConfigSettings.DefaultResizeBackgroundColor);
                        }
                    }
                        
                    
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    context.Response.StatusCode = 404;
                }
                if(resize && keepOriginalSize)
                {
                    r.Add(new UploadFilesResult()
                    {
                        //Thumbnail_url = 
                        FileUrl = WebUtils.ResolveUrl(destPath),
                        FullSizeUrl = WebUtils.ResolveUrl(fullSizeFilePath),
                        Name = newFileName,
                        Length = contentLength,
                        Type = mimeType
                    });

                }
                else
                {
                    r.Add(new UploadFilesResult()
                    {
                        //Thumbnail_url = 
                        FileUrl = WebUtils.ResolveUrl(destPath),
                        Name = newFileName,
                        Length = contentLength,
                        Type = mimeType
                    });

                }
                

                var uploadedFiles = new
                {
                    files = r.ToArray()
                };
                var jsonObj = js.Serialize(uploadedFiles);
                context.Response.Write(jsonObj.ToString());

            }

            

        }

        private void Upload(HttpContext context)
        {
            var result = OpResult.Denied;
           
            string jsonResult = serializer.Serialize(result);

            bool doUpload = true;

            if (context.Request.Files.Count > 0)
            {
                fileUploaded = context.Request.Files[0];


                if (fileUploaded.ContentLength > fileSystem.Permission.MaxSizePerFile) 
                {
                    log.Info("upload rejected due to fileSystem.Permission.MaxSizePerFile");
                    doUpload = false;
                    jsonResult = serializer.Serialize(OpResult.FileSizeLimitExceed);
                }
                else if (fileSystem.CountAllFiles() >= fileSystem.Permission.MaxFiles) 
                {
                    log.Info("upload rejected due to fileSystem.Permission.MaxFiles");
                    doUpload = false;
                    jsonResult = serializer.Serialize(OpResult.FileLimitExceed);

                }
                else if (fileSystem.GetTotalSize() + fileUploaded.ContentLength >= fileSystem.Permission.Quota) 
                {
                    log.Info("upload rejected due to fileSystem.Permission.Quota");
                    doUpload = false;
                    jsonResult = serializer.Serialize(OpResult.QuotaExceed);
                }


                if (!fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(fileUploaded.FileName)))
                {
                    log.Info("upload rejected due to not allowed file extension");
                    doUpload = false;
                    jsonResult = serializer.Serialize(OpResult.FileTypeNotAllowed);
                    //RenderJsonResult(context, OpResult.FileTypeNotAllowed);
                    //return;
                }
                    virtualPath = VirtualPathUtility.AppendTrailingSlash(virtualPath);
                    if (doUpload)
                    {
                        try
                        {
                            if (OnFileUploading(virtualPath, fileUploaded, ref result))
                            {
                                result = fileSystem.SaveFile(virtualPath, fileUploaded, overwriteExistingFiles);
                                if (result == OpResult.Succeed)
                                {
                                    jsonResult = serializer.Serialize(
                                        WebFileInfo.FromPostedFile(fileUploaded,
                                        System.IO.Path.GetFileName(fileUploaded.FileName).ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles)).ToJson());

                                }
                                else
                                {
                                    jsonResult = serializer.Serialize(result);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            result = OpResult.Error;
                            jsonResult = serializer.Serialize(result);
                        }

                    }
            }
            else
            {
                // no file posted
                jsonResult = serializer.Serialize(OpResult.Error);
            }

             //this is supposed to be an html response not a json response
            context.Response.ContentType = "text/html";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;
            context.Response.Write(BuildHtmlWrapper(context, jsonResult));
            
            
        }

        private string BuildHtmlWrapper(HttpContext context, string json)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
            sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            sb.Append("<head>\n");
            sb.Append("<title>Upload Result</title>");
            sb.Append("\n</head>");

            sb.Append("<body><p>");
            sb.Append(context.Server.HtmlEncode(json));

            sb.Append("</p></body>");
            sb.Append("</html>\r\n");

            return sb.ToString();
        }

        private void ListFiles(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFileListing(virtualPath, ref result))
                {
                    var files = fileSystem.GetFileList(virtualPath);

                    context.Response.ContentType = "text/javascript";
                    Encoding encoding = new UTF8Encoding();
                    context.Response.ContentEncoding = encoding;
                    context.Response.Write(serializer.Serialize(files.Select(f => f.ToJson())));
                }
                else
                {
                    RenderJsonResult(context, result);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                RenderJsonResult(context, OpResult.Error);
            }

        }

        private void DeleteFile(HttpContext context)
        {
            var result = OpResult.Denied;

            if (fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(virtualPath)))
            {
                try
                {
                    if (OnFileDeleting(virtualPath, ref result))
                    {
                        result = fileSystem.DeleteFile(virtualPath);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = OpResult.Error;
                }
            }

            RenderJsonResult(context, result);

        }

        private void MoveFile(HttpContext context)
        {
            var result = OpResult.Denied;

            if (
                (fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(virtualSourcePath)))
                && (fileSystem.Permission.IsExtAllowed(VirtualPathUtility.GetExtension(virtualTargetPath)))
                )
            {
                try
                {
                    if (OnFileMoving(virtualSourcePath, virtualTargetPath, ref result))
                    {
                        result = fileSystem.MoveFile(virtualSourcePath, virtualTargetPath, false);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = OpResult.Error;
                }
            }

            RenderJsonResult(context, result);

        }

        #endregion

        #region Folder Methods

        private void CreateFolder(HttpContext context)
        {
            var result = OpResult.Denied;

            if (fileSystem.CountFolders() <= fileSystem.Permission.MaxFolders)
            {
                try
                {
                    virtualPath = VirtualPathUtility.AppendTrailingSlash(virtualPath);

                    if (OnFolderCreating(virtualPath, ref result))
                    {
                        result = fileSystem.CreateFolder(virtualPath);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = OpResult.Error;
                }
            }
            else
            { 
                result = OpResult.FolderLimitExceed; 
            }

            RenderJsonResult(context, result);
        }

        private void ListFolders(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                if (OnFolderListing(ref result))
                {
                    var folders = fileSystem.GetAllFolders();
                    context.Response.ContentType = "text/javascript";
                    Encoding encoding = new UTF8Encoding();
                    context.Response.ContentEncoding = encoding;
                    context.Response.Write(serializer.Serialize(folders.Select(f => f.ToJson())));

                }
                else
                {
                    RenderJsonResult(context, result);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

        }

        private void DeleteFolder(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            {
                virtualPath = VirtualPathUtility.AppendTrailingSlash(virtualPath);

                if (OnFolderDeleting(virtualPath, ref result))
                {
                    result = fileSystem.DeleteFolder(virtualPath);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);

        }

        private void MoveFolder(HttpContext context)
        {
            var result = OpResult.Denied;

            try
            { 
                virtualSourcePath = VirtualPathUtility.AppendTrailingSlash(virtualSourcePath);
                virtualTargetPath = VirtualPathUtility.AppendTrailingSlash(virtualTargetPath);

                if (OnFolderMoving(virtualSourcePath, virtualTargetPath, ref result))
                {
                    result = fileSystem.MoveFolder(virtualSourcePath, virtualTargetPath);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = OpResult.Error;
            }

            RenderJsonResult(context, result);

        }

        #endregion


        private void RenderJsonResult(HttpContext context, OpResult result)
        {
            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            context.Response.Write("{");
            if (result == OpResult.Succeed)
            {
                context.Response.Write("\"succeed\":true");
            }
            else
            {
                context.Response.Write("\"succeed\":false");
            }

            context.Response.Write(",\"status\" : \"" + result.ToString() + "\"");


            context.Response.Write("}");
        }

        private void LoadSettings(HttpContext context)
        {
            //logAllFileSystemActivity = WebConfigSettings.LogAllFileManagerActivity;

            if (context.Request.QueryString["cmd"] != null) { cmd = context.Request.QueryString["cmd"]; }
            if (context.Request.Params.Get("path") != null) { pipedPath = context.Request.Params.Get("path"); }
            if (context.Request.QueryString["srcPath"] != null) { pipedSourcePath = context.Request.QueryString["srcPath"]; }
            if (context.Request.QueryString["destPath"] != null) { pipedTargetPath = context.Request.QueryString["destPath"]; }

            overwriteExistingFiles = WebConfigSettings.FileManagerOverwriteFiles;

            if (WebConfigSettings.RequireFileSystemServiceToken)
            {
                Guid fileSystemToken = WebUtils.ParseGuidFromQueryString("t", Guid.Empty);
                if (fileSystemToken != Global.FileSystemToken)
                {
                    log.Info("Invalid token received by FileService.ashx so blocking access");
                    return;
                }
            }

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

            virtualPath = GetVirtualPath(SanitizePath(pipedPath));
            virtualSourcePath = GetVirtualPath(SanitizePath(pipedSourcePath));
            virtualTargetPath = GetVirtualPath(SanitizePath(pipedTargetPath));

            if (context.Request.Params["fld"] != null)
            {
                virtualTargetPath = context.Request.Params["fld"].Replace("..",string.Empty);
            }

            //log.Info(context.Request.RawUrl);
            if (WebConfigSettings.LogAllFileServiceRequests)
            {
                log.Info("virtualPath = " + virtualPath + " virtualSourcePath = " + virtualSourcePath + " virtualTargetPath = " + virtualTargetPath);
            }

        }

        private string GetVirtualPath(string pipedPath)
        {
            //string virtualPath = "/";
            //foreach (var segment in segments)
            //    virtualPath = VirtualPathUtility.Combine(virtualPath, segment.Replace(displayPathSeparator, '/'));
            //return virtualPath;
            return fileSystem.VirtualRoot + pipedPath.Replace(displayPathSeparator, '/');
        }

        private string SanitizePath(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            // not expecting / or ../ or \ or that kind of thing so remove it to prevent hacks 
            return input.Replace("..", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty).Trim();
        }

        
        protected virtual bool OnFileDownloading(string path)
        {
            return true;
        }

        protected virtual bool OnFolderCreating(string path, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFolderMoving(string srcPath, string destPath, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFolderDeleting(string path, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFolderListing(ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileListing(string path, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileUploading(string path, HttpPostedFile fileUploaded, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileMoving(string srcPath, string destPath, ref OpResult result)
        {
            return true;
        }

        protected virtual bool OnFileDeleting(string path, ref OpResult result)
        {
            return true;
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
