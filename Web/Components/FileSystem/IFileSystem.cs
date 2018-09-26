/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */
//  Based on QtFile, Forked/Refactored by 
//
// Author:					
// Created:				    2009-12-30
// Last Modified:			2017-09-16
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


//http://msdn.microsoft.com/en-us/library/system.web.virtualpathutility.aspx

using System;
using System.IO;
using System.Web;
using System.Collections.Generic;


namespace mojoPortal.FileSystem
{
	public interface IFileSystem
	{
		IFileSystemPermission Permission { get; }

		bool UserHasUploadPermission { get; } 
		bool UserHasBrowsePermission { get; }
		/// <summary>
		/// The root folder 
		/// </summary>
		string VirtualRoot { get; }

		/// <summary>
		/// the base url for uploaded files such as images
		/// for local file storage will return empty string so that relative urls are used
		/// whereas other file system providers (such as Azure Blob Storage) may return an actual base url
		/// </summary>
		string FileBaseUrl { get; }

		///// <summary>
		///// The root folder 
		///// </summary>
		//string RootFolder { get; }

		///// <summary>
		///// If you don't want to display the actual file path but instead an alternate such as a virtual url style path you can populate this
		///// </summary>
		//string RootFolderDisplayAlias { get; }

		//string GetPath(params string[] segments);

		/// <summary>
		/// Saves a file to new location.
		/// </summary>
		/// <param name="folderPath">
		/// The related path of the destination folder.
		/// </param>
		/// <param name="file">
		/// The file to save.
		/// </param>
		/// <param name="overWrite">
		/// Whether overwrite if namesake file exists.
		/// </param>
		/// <returns>
		/// One of following results:
		/// <list type="bullet">
		/// <item><see cref="OpResult.Succeed"/> - file saved;</item>
		/// <item><see cref="OpResult.FolderNotFound"/> - the destination folder not found;</item>
		/// <item><see cref="OpResult.AlreadyExist"/> - a namesake file alreay exists.</item>
		/// </list>
		/// </returns>
		OpResult SaveFile(string folderVirtualPath, HttpPostedFile file, bool overWrite);

		OpResult SaveFile(string folderVirtualPath, System.IO.Stream stream, string contentType, bool overWrite);

		Stream GetWritableStream(string path);

		OpResult CopyFile(string sourceVirtualPath, string targetVirtualPath, bool overwrite);

		//string GetFileName(string path);
		//string GetFileExtension(string path);

		//string CombinePaths(string path1, string path2);


		Stream GetAsStream(string path);
		WebFile RetrieveFile(string path);

		/// <summary>
		/// Moves a file to new location.
		/// </summary>
		/// <param name="srcPath">
		/// The related path of the source file.
		/// </param>
		/// <param name="destPath">
		/// The related path of the destination folder.
		/// </param>
		/// <param name="overWrite">
		/// Whether overwrite if namesake file exists.
		/// </param>
		/// <returns>
		/// One of following results:
		/// <list type="bullet">
		/// <item><see cref="OpResult.Succeed"/> - file moved;</item>
		/// <item><see cref="OpResult.NotFound"/> - the source file not found;</item>
		/// <item><see cref="OpResult.FolderNotFound"/> - the destination folder not found;</item>
		/// <item><see cref="OpResult.AlreadyExist"/> - a namesake file alreay exists.</item>
		/// </list>
		/// </returns>
		OpResult MoveFile(string srcVirtualPath, string destVirtualPath, bool overWrite);

		OpResult DeleteFile(string virtualPath);

		bool FileExists(string virtualPath);

		bool FolderExists(string virtualPath);

		IEnumerable<WebFile> GetFileList(string virtualPath);

		int CountAllFiles();

		long GetTotalSize();

		/// <summary>
		/// Creates a new folder.
		/// </summary>
		/// <param name="path">
		/// The related path of the new folder.
		/// </param>
		/// <returns>
		/// One of following results:
		/// <list type="bullet">
		/// <item><see cref="OpResult.Succeed"/> - folder created;</item>
		/// <item><see cref="OpResult.AlreadyExist"/> - The folder with specified path already exists.</item>
		/// </list>
		/// </returns>
		OpResult CreateFolder(string virtualPath);

		/// <summary>
		/// Moves a folder to new location.
		/// </summary>
		/// <param name="srcPath">
		/// The related path of the source folder.
		/// </param>
		/// <param name="destPath">
		/// The related path of the destination folder.
		/// </param>
		/// <returns>
		/// One of following results:
		/// <list type="bullet">
		/// <item><see cref="OpResult.Succeed"/> - folder moved;</item>
		/// <item><see cref="OpResult.NotFound"/> - the source folder not found;</item>
		/// <item><see cref="OpResult.FolderNotFound"/> - parent of the destination folder not found;</item>
		/// <item><see cref="OpResult.AlreadyExist"/> - a namesake folder alreay exists.</item>
		/// </list>
		/// </returns>
		OpResult MoveFolder(string srcVirtualPath, string destVirtualPath);

		/// <summary>
		/// Deletes a folder.
		/// </summary>
		/// <param name="path">
		/// The related path of the folder.
		/// </param>
		/// <returns>
		/// One of following results:
		/// <list type="bullet">
		/// <item><see cref="OpResult.Succeed"/> - folder deleted;</item>
		/// <item><see cref="OpResult.NotFound"/> - the folder not found.</item>
		/// </list>
		/// </returns>
		OpResult DeleteFolder(string virtualPath);

		IEnumerable<WebFolder> GetAllFolders();

		IEnumerable<WebFolder> GetFolderList(string virtualPath);

		int CountFolders();
		
		string CombinePath(params string[] paths);
	}
}
