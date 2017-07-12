/*!
 * QtFile Online File Manager v1.0
 * http://qtfile.codeplex.com/
 *
 * Copyright (c) 2009 Zhifeng Lin (fszlin[at]gmail.com)
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 
 Adopted/Forked For mojoPortal by  2009-12-25
 Last Modified: 2009-12-28
 
 
 
 */
 
 
var
// Status flags
	ButtonOK = 'OK',
	ButtonCancel = 'Cancel',
	StatusError = 'Error',
	StatusSucceed = 'Succeed',
	StatusDenied = 'Denied',
	StatusNotFound = 'NotFound',
	StatusFolderNotFound = 'FolderNotFound',
	StatusAlreadyExist = 'AlreadyExist',
	StatusFolderLimitExceed = 'FolderLimitExceed',
	StatusFileSizeLimitExceed = 'FileSizeLimitExceed',
	StatusFileLimitExceed = 'FileLimitExceed',
	StatusQuotaExceed = 'QuotaExceed',
	StatusFileTypeNotAllowed = 'FileTypeNotAllowed',
	// String contants
	TextLinkIntro = 'Copy the URL to access this file:',
	TextGetLink = 'Get Download URL',
	TextDownload = "Download",
	TextRename = "Rename",
	TextMove = "Move",
	TextDelete = "Delete", 
	TextFileNameInvalid = 'The file name entered is invalid.', 
	TextOperationDenied = 'Operation denied.',
	TextFolderInvalidFolderName = 'The folder name entered is invalid.',
	// Messages for folder creating
	TextFolderCreateSucceed = 'Folder created.',
	TextFolderCreateFolderLimitExceed = 'Maximum number of folders exceed.',
	TextFolderCreateAlreadyExist = 'The folder trying to create already exists.',
	TextFolderCreateError = 'Error occurred while creating folder.',
	TextFolderCreateInProgress = 'Creating new folder.',
	TextFolderCreateWait = 'Please wait while a new folder is being created.',
	TextFolderCreateNewFolder = 'New Folder',
	TextFolderCreateQuestion = 'Please enter the name of the new folder:',
	// Messages for folder renaming
	TextFolderRenameSucceed = 'Folder renamed.',
	TextFolderRenameInProgress = 'Renaming folder.',
	TextFolderRenameWait = 'Please wait while a folder is being renamed.',
	TextFolderRenameQuestion = 'Please enter the new name of the folder:',
	TextFolderRenameError = 'Error occurred while renaming folder.',
	TextFolderRenameNotFound = 'The source folder not found, please refresh and try again.',
	TextFolderRenameFolderNotFound = 'The destination folder not found, please refresh and try again.',
	TextFolderRenameAlreadyExist = 'A nameske folder already exists in destination folder.',
	// Messages for folder moving
	TextFolderMoveSucceed = 'Folder Moved.',
	TextFolderMoveError = 'Error occurred while renaming folder.',
	TextFolderMoveSubfolderSelected = 'The destination folder is a subfolder of the source folder.',
	TextFolderMoveCurrentSelected = 'The source folder selected.',
	TextFolderMoveInProgress = 'Please select destination folder.',
	TextFolderMoveWait = 'Please wait while a folder is being moved.',
	TextFolderMoveNotFound = 'The source folder not found, please refresh and try again.',
	TextFolderMoveFolderNotFound = 'The destination folder not found, please refresh and try again.',
	TextFolderMoveAlreadyExist = 'A nameske folder already exists in destination folder.',
	// Messages for folder deleting
	TextFolderDeleteSucceed = 'Folder deleted.',
	TextFolderDeleteInProgress = 'Deleting folder.',
	TextFolderDeleteWait = 'Please wait while a folder is being deleted.',
	TextFolderDeleteQuestion = 'Are you sure you want to pemanently delete this folder and all the files in it?',
	TextFolderDeleteError = 'Error occurred while deleting folder.',
	TextFolderDeleteNotFound = 'The folder not found, please refresh and try again.',
	// Messages for folder listing
	TextFolderListSucceed = 'Folder list loaded.',
	TextFolderListInProgress = 'Loading folder list.',
	TextFolderListWait = 'Please wait while loading folder list.',
	TextFolderListError = 'Error occurred while loading folder list.',
	// Messages for file listing
	TextFileListInProgress = 'Loading file list.',
	TextFileListWait = 'Please wait while loading file list.',
	TextFileListSucceed = 'File list loaded.',
	TextFileListError = 'Error occurred while loading file list.',
	// Messages for file deleting
	TextFileDeleteSucceed = 'File deleted.',
	TextFileDeleteInProgress = 'Deleting file.',
	TextFileDeleteNotFound = 'The file not found, please refresh and try again.',
	TextFileDeleteWait = 'Please wait while a file is being deleted.',
	TextFileDeleteQuestion = 'Are you sure you want to pemanently delete this file?',
	TextFileDeleteError = 'Error occurred while deleting file.',
	// Messages for file moving
	TextFileMoveSucceed = 'File moved.', 
	TextFileMoveError = 'Error occurred while renaming file.', 
	TextFileMoveFileExisting = 'There is already a file with the same name in this folder.', 
	TextFileMoveFileInProgress = 'Moving file.',
	TextFileMoveNotFound = 'The file not found, please refresh and try again.',
	TextFileMoveFolderNotFound = 'The destination folder not found, please refresh and try again.',
	TextFileMoveAlreadyExist = 'A namesake file already exists in destionation folder.',
	TextFileMoveWait = 'Please wait while a file is being Moved.',
	TextFileMoveInProgress = 'Please select destination folder.',
	// Messages for file renaming
	TextFileRenameSucceed = 'File renamed.', 
	TextFileRenameError = 'Error occurred while renaming file.', 
	TextFileRenameFileExisting = 'There is already a file with the same name in this folder.', 
	TextFileRenameFileInProgress = 'Renaming file.',
	TextFileRenameNotFound = 'The file not found, please refresh and try again.',
	TextFileRenameFolderNotFound = 'The destination folder not found, please refresh and try again.',
	TextFileRenameAlreadyExist = 'A namesake file already exists in destionation folder.',
	TextFileRenameWait = 'Please wait while a file is being renamed.',
	TextFileRenameQuestion = 'Please enter the new name of the file:',
	// Messages for file uploading
	TextFileUploadSucceed = 'File uploaded.',
	TextFileUploadFileInProgress = 'Uploading file.',
	TextFileUploadWait = 'Please wait while a file is being uploaded.',
	TextFileUploadError = 'Error occurred while uploading file.',
	TextFileUploadFileSizeLimitExceed = 'Maximum allowed file size exceed.',
	TextFileUploadFileLimitExceed = 'Maximum number of files exceed.',
	TextFileUploadQuotaExceed = 'Disk space quota exceed.',
	TextFileUploadFolderNotFound = 'The destination folder not found, please refresh and try again.',
	TextFileUploadAlreadyExist = 'A namesake file already exists in destionation folder.',
	TextFileUploadFileTypeNotAllowed = 'This type of file is not allowed.'
	