/*!
 * QtFile Online File Manager v1.0
 * http://qtfile.codeplex.com/
 *
 * Copyright (c) 2009 Zhifeng Lin (fszlin[at]gmail.com)
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 
 Adopted/Forked For mojoPortal by  2009-12-25
 Last Modified: 2009-12-29
 
 */
 
 
 
; (function() {

    var 
	window = this,
	undefined,
	jQuery = window.jQuery,
	alert = window.alert,
	prompt = window.prompt,
	confirm = window.confirm,
	Ul = 'ul',
	Li = 'li',
	Span = 'span',
	AttrTitle = 'title',
	HtmlTagUl = '<ul></ul>',
	HtmlTagLi = '<li></li>',
	HtmlTagSpan = '<span></span>',
	DataKeyFileName = 'name',
	DataKeyFolderPath = 'path',
    // Generates a random integer between 0 to (max - 1)
	rand = function(max) {
	    return Math.floor(Math.random() * max);
	}, // End of rand()
    // Generates a random string with specified length
	genRandomId = function(len) {
	    var id = '_';
	    for (var i = 1; i < len; ++i) {
	        var num = rand(26 + 26 + 10);
	        if (num < 26)
	            id += 'a' + num;
	        else if (num < 26 + 26)
	            id += 'A' + num - 26;
	        else
	            id += '0' + num - 26 - 26;
	    }
	    return id;
	}, // End of genRandomId
    // Embeds parameters into the given url
    // NOTE: The used parameters will be removed
	formatUrl = function(url, params) {
	    for (var k in params) {
	        var argName = '{' + k + '}';
	        if (url.indexOf(argName) >= 0) {
	            url = url.replace(argName, escape(params[k]));
	            delete params[k];
	        }
	    }
	    return url;
	}, // End of formatUrl
    // Converts a file size in bytes to a string
	fileSizeToString = function(size) {
	    var names = ['KB', 'MB', 'GB', 'TB'], ind;
	    size /= 1024;
	    for (ind = 0; size > 1024 && ind < names.length; ++ind)
	        size /= 1024;
	    return (Math.floor(size * 100) / 100.0) + ' ' + names[ind];
	}, // End of fileSizeToString()
    // Decodes html entities from a string
	decodeHtml = function(html) {
	    return html
			.replace(/&amp;/g, '&')
			.replace(/&lt;/g, '<')
			.replace(/&gt;/g, '>');
	}, // End of decodeHtml
	parseJSON = function(data) {
	    return window['eval']("(" + data + ")");
	},
	getUrlRoot = function() {
	    var urlSegments =
			/^(?:([^:\/?#]+):)?(?:\/\/((?:(([^:@]*):?([^:@]*))?@)?([^:\/?#]*)(?::(\d*))?))?((((?:[^?#\/]*\/)*)([^?#]*))(?:\?([^#]*))?(?:#(.*))?)/
			.exec(window.location);
	    return urlSegments[1] + '://' + urlSegments[2];
	},
    // jQuery.offset cannot proper calculate offset in Chrome 2?
	calcOffset = function($elem) {
	    var donElem = $elem.get(0), curleft = 0, curtop = 0;
	    if (donElem && donElem.offsetParent) {

	        do {
	            curleft += donElem.offsetLeft;
	            curtop += donElem.offsetTop;
	        } while (donElem = donElem.offsetParent);
	    }
	    return { left: curleft, top: curtop };
	},
	getExt = function(filename) {
	    var dot = filename.lastIndexOf('.');
	    return dot < 0 ? '' : filename.substr(dot + 1);
	};
    getNameOnly = function(filename) {
        var dot = filename.lastIndexOf('.');
        return dot < 1 ? filename : filename.substr(0, dot);
    };

    var 
    // moved labels into en.qtfile.js so this can be localized with other languages
    // Helper methods for translate status to message
	translateFileUploadStatus = function(status) {
	    switch (status) {
	        case StatusDenied:
	            return TextOperationDenied;
	        case StatusFileSizeLimitExceed:
	            return TextFileUploadFileSizeLimitExceed;
	        case StatusFileLimitExceed:
	            return TextFileUploadFileLimitExceed;
	        case StatusQuotaExceed:
	            return TextFileUploadQuotaExceed;
	        case StatusFolderNotFound:
	            return TextFileUploadFolderNotFound;
	        case StatusAlreadyExist:
	            return TextFileUploadAlreadyExist;
	        case StatusFileTypeNotAllowed:
	            return TextFileUploadFileTypeNotAllowed;
	        default: // error
	            return TextFileUploadError;
	    }
	},
	translateFileListStatus = function(status) {
	    switch (status) {
	        case StatusDenied:
	            return TextOperationDenied;
	        default: // error
	            return TextFileListError;
	    }
	},
	translateFileDeleteStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFileDeleteSucceed;
	        case StatusDenied:
	            return TextOperationDenied;
	        case StatusNotFound:
	            return TextFileDeleteNotFound;
	        default: // error
	            return TextFileDeleteError;
	    }
	},
	translateFileMoveStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFileMoveSucceed;
	        case StatusDenied:
	            return TextOperationDenied;
	        case StatusNotFound:
	            return TextFileMoveNotFound;
	        case StatusFolderNotFound:
	            return TextFileMoveFolderNotFound;
	        case StatusAlreadyExist:
	            return TextFileMoveAlreadyExist;
	        default: // error
	            return TextFileMoveError;
	    }
	},
	translateFileRenameStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFileRenameSucceed;
	        case StatusDenied:
	            return TextOperationDenied;
	        case StatusNotFound:
	            return TextFileRenameNotFound;
	        case StatusFolderNotFound:
	            return TextFileRenameFolderNotFound;
	        case StatusAlreadyExist:
	            return TextFileRenameAlreadyExist;
	        default: // error
	            return TextFileRenameError;
	    }
	},
	translateFolderCreateStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFolderCreateSucceed;
	        case StatusFolderLimitExceed:
	            return TextFolderCreateFolderLimitExceed;
	        case StatusAlreadyExist:
	            return TextFolderCreateAlreadyExist;
	        case StatusDenied:
	            return TextOperationDenied;
	        default: // error
	            return TextFolderCreateError;
	    }
	},
	translateFolderRenameStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFolderRenameSucceed;
	        case StatusNotFound:
	            return TextFolderRenameNotFound;
	        case StatusFolderNotFound:
	            return TextFolderRenameFolderNotFound;
	        case StatusAlreadyExist:
	            return TextFolderRenameAlreadyExist;
	        case StatusDenied:
	            return TextOperationDenied;
	        default: // error
	            return TextFileListError;
	    }
	},
	translateFolderMoveStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFolderMoveSucceed;
	        case StatusNotFound:
	            return TextFolderMoveNotFound;
	        case StatusFolderNotFound:
	            return TextFolderMoveFolderNotFound;
	        case StatusAlreadyExist:
	            return TextFolderMoveAlreadyExist;
	        case StatusDenied:
	            return TextOperationDenied;
	        default: // error
	            return TextFileListError;
	    }
	},
	translateFolderDeleteStatus = function(status) {
	    switch (status) {
	        case StatusSucceed:
	            return TextFolderDeleteSucceed;
	        case StatusDenied:
	            return TextOperationDenied;
	        case StatusNotFound:
	            return TextFolderDeleteNotFound;
	        default: // error
	            return TextFolderDeleteError;
	    }
	},
	translateFolderListStatus = function(status) {
	    switch (status) {
	        case StatusDenied:
	            return TextOperationDenied;
	        default: // error
	            return TextFolderListError;
	    }
	};

    var 
	qtfile = function(selector, options) {
	    this.init(selector, options);
	},
	fn = qtfile.prototype = {};

    jQuery.extend(qtfile, {
        setOptions: function(options) {
            jQuery.extend(qtfile.prototype, options);
        }
    });

    // Builds file browser with provided elements.
    // options - Options for file browser
    jQuery.fn.qtfile = function(options) {
        return this.each(function() {
            new qtfile(this, options);
        });
    };

    jQuery.qtfile = function(selector, options) {
        jQuery(selector).qtfile(options);
    };

    jQuery.extend(
	fn, {
	    // Default parameters of file browser
	    options: {
	        // The username of current user,
	        // will be load from the container's title attribute if not provided via options
	        //username: undefined,
	        // hard coded username because we really are not using it
	        username: "user",
	        // The default name of root folder,
	        // may be overwritten by title attribute of folder list element
	        rootFolder: "My Files",
	        // Separator of file path
	        folderSeparator: '|',
	        // Html for folder names
	        folderNameHtml: '<span class="icon-folder"></span><span>{folderName}</span>',
	        // Regex to validate folder name
	        folderName: /^[^\\\/\:\*?\<\>\|]+$/,
	        // Regex to validate file name
	        fileName: /^[^\\\/\:\*?\<\>\|]+$/,
	        // Timeout for ajax request, in ms
	        ajaxTimeout: 10000,
	        // Selector for the folder list, should be a DOM element containing the list
	        folderList: '#folderListPanel',
	        // Selector for the file list, should be a DOM element containing the list
	        fileList: '#fileListPanel',
	        // Selector for the create folder button
	        buttonCreateFolder: '#buttonCreateFolder',
	        // Selector for the rename folder button
	        buttonRenameFolder: '#buttonRenameFolder',
	        // Selector for the delete folder button
	        buttonDeleteFolder: '#buttonDeleteFolder',
	        // Selector for the move folder button
	        buttonMoveFolder: '#buttonMoveFolder',
	        // Selector for the refresh folders button
	        buttonRefreshFolders: '#buttonRefreshFolderList',
	        // Selector for the refresh files button
	        buttonRefreshFiles: '#buttonRefreshFileList',
	        // Selector for the upload file button
	        buttonUploadFile: '#buttonUploadFile',
	        // Selector for the container of message
	        statusMessage: "#statusMessage",
	        // Status consider as an error
	        errorStatus: ['Error', 'NotFound',
				'AlreadyExist', 'FolderNotFound',
				'FolderLimitExceed', 'FileLimitExceed',
				'QuotaExceed', 'FileSizeLimitExceed'],
	        // Settings for file operations
	        file: {
	            // Settings for download file
	            download: {
	                // Url to request for downloading file
	                url: '/UserFile/Download?username={username}&path={path}',
	                // Additional data to send with the request
	                data: {}
	            }, // End of download
	            // Settings for moving file
	            move: {
	                // Url to request for moving file
	                url: '/UserFile/FileMove?srcPath={srcPath}&destPath={destPath}',
	                // Method for ajax request
	                method: 'GET',
	                // Additional data to send with the request
	                data: {}
	            }, // End of move
	            // Settings for deleting file
	            del: {
	                // Url to request for deleting file
	                url: '/UserFile/FileDelete?path={path}',
	                // Method for ajax request
	                method: 'GET',
	                // Additional data to send with the request
	                data: {}
	            }, // End of delete
	            // Settings for listing file under a folder
	            list: {
	                // Url to request for listing file.
	                // path is the folde path to rquest
	                url: '/UserFile/FileList?path={path}',
	                // Method for ajax request
	                method: 'GET',
	                //Additional data to send with the request
	                data: {}
	            }, // End of listing
	            // Settings for uploading file
	            upload: {
	                // Url to request for uploading file
	                url: '/UserFile/FileUpload',
	                // The name of the file input
	                fileName: 'fileUploaded',
	                // The name of the hidden iframe,
	                // a random id will be used if unset
	                frameName: undefined,
	                // Additional data to send with the request
	                data: {}
} // End of upload
	            }, // End of file
	            // Settings for folder operations
	            folder: {
	                // Settings for creating folder
	                create: {
	                    // Url to request for creating folder
	                    url: '/UserFile/FolderCreate?path={path}',
	                    // Method for ajax request
	                    method: 'GET',
	                    // Additional data to send with the request
	                    data: {}
	                }, // End of move
	                // Settings for moving folder
	                move: {
	                    // Url to request for moving folder
	                    url: '/UserFile/FolderMove?srcPath={srcPath}&destPath={destPath}',
	                    // Method for ajax request
	                    method: 'GET',
	                    // Additional data to send with the request
	                    data: {}
	                }, // End of move
	                // Settings for deleting folder
	                del: {
	                    // Url to request for deleting folder
	                    url: '/UserFile/FolderDelete?path={path}',
	                    // Method for ajax request
	                    method: 'GET',
	                    // Additional data to send with the request
	                    data: {}
	                }, // End of delete
	                // Settings for listing folder
	                list: {
	                    // Url to request for listing folder
	                    url: '/UserFile/FolderList',
	                    // Method for ajax request
	                    method: 'GET',
	                    //Additional data to send with the request
	                    data: {}
} // End of listing
	                }, // End of folder
	                // CSS classes used by the script
	                classes: {
	                    // The class for current folder
	                    folderSelected: 'current-folder',
	                    // The class for disabled button
	                    buttonDisabled: 'button-disabled',
	                    // The class for container of file list
	                    fileList: 'file-list',
	                    // The class for container of file list while the list is empty
	                    fileListEmpty: 'file-list-empty',
	                    // The class for the icon of file name
	                    fileNameIcon: 'icon-file-name',
	                    // The class for file name
	                    fileName: 'file-name',
	                    // The class for file size
	                    fileSize: 'file-size',
	                    // The class for file actions
	                    fileActions: 'file-actions',
	                    // The class for file link button
	                    fileActionLink: 'file-action-link',
	                    // The class for download file button
	                    fileActionDownload: 'file-action-download',
	                    // The class for rename file button
	                    fileActionRename: 'file-action-rename',
	                    // The class for move file button
	                    fileActionMove: 'file-action-move',
	                    // The class for delete file button
	                    fileActionDelete: 'file-action-delete',
	                    // The class for container of folder list
	                    folderList: 'folder-list',
	                    // The class for the text of folder item
	                    folderNameText: 'folder-name-text',
	                    // The class for the icon of folder item
	                    folderIcon: 'icon-folder',
	                    // The class for the icon in the end of folder item
	                    folderTailIcon: 'icon-folder-end',
	                    // The class for the bullet of folder item
	                    folderBullet: 'folder-bullet',
	                    // The class for the bullet of opened folder item
	                    folderBulletOpened: 'folder-bullet-opened',
	                    // The class for the bullet of closed folder item
	                    folderBulletClosed: 'folder-bullet-closed',
	                    // The class for folder name
	                    folderName: 'folder-name',
	                    // The class for folder name of current folder
	                    folderNameSelected: 'folder-name-selected',
	                    // The class for normal status message
	                    statusInfo: 'status-info',
	                    // The class for error status message
	                    statusError: 'status-error',
	                    // The class for warning status message
	                    statusWarn: 'status-warn',
	                    // The class for status message icon
	                    statusIcon: 'icon-status'
	                },
	                texts: {
	            }
	        }, // End of settings
	        init: function(elem, options) {
	            var qtfile = this,
	            // Apply user options
			options = jQuery.extend(qtfile.options, options);

	            qtfile.context = jQuery(elem);

	            // Retrieve username if needed
	            options.username = options.username ||
				jQuery.trim(qtfile.context.attr(AttrTitle));

	            // Not enough information to build file browser
	            if (qtfile.context.size() != 1 || !options.username)
	                return;

	            qtfile.context.removeAttr(AttrTitle);

	            options.errorStatus = '|' + options.errorStatus.join('|') + '|';

	            qtfile.initFolders();
	            qtfile.initFiles();
	            qtfile.initButtons();

	            qtfile.initFileDetails();
	        },
	        find: function(selector) {
	            return this.context.find(selector);
	        },
	        loc: function(text) {
	            return this.options.texts[text] || text;
	        },
	        // Combines two parts of patg
	        combinePath: function(part1, part2) {
	            if (part1.length > 0)
	                return part1 + this.options.folderSeparator + part2;
	            return part2;
	        }, // End of combinePath()
	        splitPath: function(path) {
	            var lastSp = path.lastIndexOf(this.options.folderSeparator);
	            var name = path.substr(lastSp + 1);
	            var parent = lastSp < 0 ? '' : path.substr(0, lastSp);
	            return {
	                parent: parent,
	                name: name
	            };
	        },
	        // Shows a status message
	        showStatus: function(message, cssClassName, count) {
	            var qtfile = this, options = qtfile.options,
				msgText = qtfile.loc(message),
				className = cssClassName || options.classes.statusInfo,
				msg = jQuery(HtmlTagSpan).addClass(className)
					.html(msgText)
					.prepend(jQuery(HtmlTagSpan).addClass(options.classes.statusIcon));

	            qtfile.find(options.statusMessage).empty()
				.append(msg);
	        }, // End of showStatusMessage()
	        // Trys to lock the file browser
	        tryLock: function(statusMsg, lockingMsg) {
	            var qtfile = this;
	            if (qtfile.isLocking) {
	                alert(qtfile.lockingMessage);
	                return false;
	            }
	            qtfile.isLocking = true;
	            qtfile.showStatus(statusMsg);
	            qtfile.lockingMessage = lockingMsg;
	            return true;
	        }, // End of tryLock()
	        // Unlocks the file browser
	        unlock: function(statusMsg, className) {
	            var qtfile = this;
	            qtfile.isLocking = false;
	            qtfile.lockingMessage = null;
	            if (statusMsg) {
	                qtfile.showStatus(statusMsg, className);
	            } else {
	                qtfile.find(qtfile.options.statusMessage).empty();
	            }
	        }, // End of unlock()
	        // Sends an ajax request
	        request: function(requestInfo, data,
			onSuccess, // function(data, textStatus) { }
			onError, // function(XMLHttpRequest, textStatus, errorThrown) { }
			onComplete // function(XMLHttpRequest, textStatus) { }
		) {
	            var qtfile = this, options = qtfile.options,

	            // Combine request data
	            //				combinedData = jQuery.extend({ }, requestInfo.data, {
	            //						username: options.username
	            //					}, data),

                combinedData = jQuery.extend({}, requestInfo.data, {
            }, data),

				url = formatUrl(requestInfo.url, combinedData);
	            //url = formatUrl(requestInfo.url, data);

	            jQuery.ajax({
	                url: url,
	                data: combinedData,
	                type: requestInfo.method,
	                success: onSuccess,
	                error: onError,
	                complete: onComplete,
	                dataType: 'json',
	                cache: false,
	                timeout: options.ajaxTimeout
	            });
	        }, // End of request()
	        statusClass: function(data) {
	            var qtfile = this, classes = qtfile.options.classes;
	            return data.succeed ? classes.statusInfo :
				qtfile.options.errorStatus.indexOf('|' + data.status + '|') < 0 ?
				classes.statusWarn : classes.statusError;
	        }
	    });

    jQuery.extend(fn, {
        // Builds initial file list
        initFiles: function() {
            var qtfile = this, files = [];
            qtfile.find(qtfile.options.fileList + ' > ul > li').each(function() {
                files.push(parseJSON(jQuery(this).text()));
            });
            qtfile.resetFiles(files);
        },
        resetFiles: function(files) {

            var qtfile = this, options = qtfile.options,
			classes = options.classes;

            // File list not empty
            if (files.length) {
                var 
				$filePanel = qtfile.find(options.fileList);
                // Get the file list ul
                $fileList = $filePanel.children(Ul);

                if ($fileList.size() == 0) {
                    $filePanel.removeClass(classes.fileListEmpty);
                    $fileList = jQuery(HtmlTagUl)
					.appendTo($filePanel)
					.addClass(classes.fileList);
                }

                // Remove old file items
                $fileList.children(Li).remove();

                // Rebuild each li
                jQuery.each(files, function() {
                    qtfile.createFileItem(this).appendTo($fileList);
                });
                qtfile.updateFileList();

            } else {
                // No file in current folder
                qtfile.find(options.fileList)
				.addClass(classes.fileListEmpty)
				.children(Ul).remove();
            }
        },
        createFileItem: function(fileInfo) {
            var 
			qtfile = this,
            // Short cur of class names
			classNames = qtfile.options.classes,

			$fileItem = jQuery(HtmlTagLi)
				.data('info', fileInfo)
				.data(DataKeyFileName, fileInfo.name);

            // File name
            jQuery(HtmlTagSpan)
			.addClass(classNames.fileName)
			.text(fileInfo.name)
			.attr('title', fileInfo.name)
			.prepend(jQuery(HtmlTagSpan).addClass(classNames.fileNameIcon).addClass('icon-file-' + getExt(fileInfo.name)))
			.appendTo($fileItem)
			.click(function() {
			    qtfile.showFileDetails(fileInfo);
			});

            // File size
            jQuery(HtmlTagSpan)
			.addClass(classNames.fileSize)
			.text(fileSizeToString(fileInfo.size))
			.appendTo($fileItem);

            var $fileActions = jQuery(HtmlTagSpan)
			.addClass(classNames.fileActions)
			.appendTo($fileItem);

            jQuery(HtmlTagSpan)
			.addClass('file-action-preview')
			.html(qtfile.loc("Preview"))
			.attr('title', qtfile.loc("Preview"))
			.click(function() {
			    qtfile.showFileDetails(fileInfo);
			})
			.appendTo($fileActions);

            // Link button
//            jQuery(HtmlTagSpan)
//			.addClass(classNames.fileActionLink)
//			.html(qtfile.loc(TextGetLink))
//			.attr('title', qtfile.loc(TextGetLink))
//			.click(function() {
//			    
//			    function pLoadedcallback() {
//			        $('#namePrompt').focus();
//			        $('#namePrompt').select();
//			    }

//			    var p = qtfile.loc(TextLinkIntro) + '<br /> <input type="text" id="namePrompt" name="namePrompt" class="widetextbox" value="' + qtfile.getUrl(qtfile.currentFolder, jQuery(this).parent(Span).parent(Li)) + '" />';
//			    var buttonSet = '{' + ButtonOK + ': false }';
//			    buttonSet = parseJSON(buttonSet);
//			    jQuery.prompt(p, { loaded: pLoadedcallback, buttons: buttonSet });

//			})
//			.appendTo($fileActions);

            // Download button
            jQuery(HtmlTagSpan)
			.addClass(classNames.fileActionDownload)
			.html(qtfile.loc(TextDownload))
			.attr('title', qtfile.loc(TextDownload))
			.click(function() {
			    qtfile.downloadFile(qtfile.currentFolder, jQuery(this).parent(Span).parent(Li));
			})
			.appendTo($fileActions);

            // Rename button
            jQuery(HtmlTagSpan)
			.addClass(classNames.fileActionRename)
			.html(qtfile.loc(TextRename))
			.attr('title', qtfile.loc(TextRename))
			.click(function() {
			    qtfile.renameFile(qtfile.currentFolder, jQuery(this).parent(Span).parent(Li));
			})
			.appendTo($fileActions);

            // Move button
            jQuery(HtmlTagSpan)
			.addClass(classNames.fileActionMove)
			.html(qtfile.loc(TextMove))
			.attr('title', qtfile.loc(TextMove))
			.click(function() {
			    qtfile.moveFileBegin(qtfile.currentFolder, jQuery(this).parent(Span).parent(Li));
			})
			.appendTo($fileActions);

            // Delete button
            jQuery(HtmlTagSpan)
			.addClass(classNames.fileActionDelete)
			.html(qtfile.loc(TextDelete))
			.attr('title', qtfile.loc(TextDelete))
			.click(function() {
			    qtfile.deleteFile(qtfile.currentFolder, jQuery(this).parent(Span).parent(Li));
			})
			.appendTo($fileActions);

            return $fileItem;
        },
        initFileDetails: function() {
            var qtfile = this,
			$preview = qtfile.find('.file-preview'),
			loadingHtml = $preview.find('.file-preview-image').html();
            if (!loadingHtml) {
                loadingHtml = 'Loading image...';
            }
            $preview.data('loading', loadingHtml);
            $preview.find('.file-preview-close').click(function() {
                if (!$preview.is(':hidden')) {
                    $preview.hide();
                }
            });
        },
        showFileDetails: function(fileInfo) {
            var qtfile = this,
			$preview = qtfile.find('.file-preview');
            $preview.find('.file-preview-name').text(fileInfo.name);
            $preview.find('.file-preview-size').text(fileSizeToString(fileInfo.size));
            $preview.find('.file-preview-content-type').text(fileInfo.contentType);
            var modified = new Date(fileInfo.modified);
            $preview.find('.file-preview-modified').text(modified.toLocaleDateString() + ' ' + modified.toLocaleTimeString());

            if (fileInfo.contentType.substr(0, 6) == 'image/') {
                $preview.find('.file-preview-image').html(
				$preview.data('loading')
			).show();


                var img = jQuery('<img alt="' + fileInfo.name + '" src="' + qtfile.getUrl2(qtfile.currentFolder, fileInfo) + '" />')
				.load(function() {
				    $preview.find('.file-preview-image').html(img);
				});
            } else {
                $preview.find('.file-preview-image').hide();
            }

            if ($preview.is(':hidden')) {
                $preview.show();
            }
        },
        updateFileList: function() {
            var $fileList = this.find(this.options.fileList).children('ul');
            $fileList.children('li:odd').removeClass('even');
            $fileList.children('li:even').addClass('even');
        },
        getUrl2: function($folderItem, fileInfo) {
            var qtfile = this,
			options = qtfile.options,
			filePath = qtfile.combinePath($folderItem.data(DataKeyFolderPath), fileInfo.name),
			url = formatUrl(options.file.download.url, jQuery.extend({}, options.file.download.data, {
			    username: options.username,
			    path: filePath
			}));

            return url;
        },
        getUrl: function($folderItem, $fileItem) {
            var qtfile = this,
			options = qtfile.options,
			filePath = qtfile.combinePath($folderItem.data(DataKeyFolderPath), $fileItem.data(DataKeyFileName)),
			url = formatUrl(options.file.download.url, jQuery.extend({}, options.file.download.data, {
			    username: options.username,
			    path: filePath
			}));

            return url;
        },
        // Opens the request file in a new window
        downloadFile: function($folderItem, $fileItem) {
            window.open(this.getUrl($folderItem, $fileItem));
        }, // End of downloadFile()		
        // Adds a new file into current folder
        addFile: function(file) {
            var qtfile = this, options = qtfile.options,
            // Get the file list ul
			$fileList = qtfile
				.find(options.fileList)
				.removeClass(options.classes.fileListEmpty)
				.children(Ul);

            if ($fileList.size() == 0) {
                $fileList = jQuery(HtmlTagUl)
				.appendTo(qtfile.find(options.fileList))
				.addClass(options.classes.fileList);
            }

            qtfile.createFileItem(file).appendTo($fileList);
            qtfile.updateFileList();
        }, // End of addFile()
        uploadFile: function($fileInput) {
            var qtfile = this, options = qtfile.options,
			classes = options.classes,
			$currentFolder = qtfile.currentFolder;

            // A file is selected
            if ($fileInput.val()) {
                if (qtfile.tryLock(TextFileUploadFileInProgress, TextFileUploadWait)) {

                    var frameName =
					options.file.upload.frameName ||
					genRandomId(20);

                    // Create a hidden iframe to receive server response
                    var $uploadFrame = jQuery('<iframe id="' + frameName + '" name="' + frameName + '"></iframe>')
					.hide()
					.appendTo('body')
					.load(function() {

					    var $frame = jQuery(this), data,
							resultText = $frame.contents().find('body').text();

					    if (resultText) {
					        try {
					            data = parseJSON(resultText);
					        } catch (err) {
					            data = { status: StatusError };
					        }

					        // File object not returned
					        if (data.status) {

					            qtfile.unlock(translateFileUploadStatus(data.status),
								qtfile.statusClass(data));
					        } else {
					            qtfile.addFile(data);

					            qtfile.unlock(TextFileUploadSucceed);
					        }
					    } else {
					        qtfile.unlock(TextFileUploadError, classes.statusError);
					    }

					    setTimeout(function() {
					        $frame.attr('src', 'about:blank');
					        $frame.remove();
					        frameName = $frame = null;
					    }, 1);
					});

                    // Upload form
                    var $uploadFrom =
					jQuery('<form method="post" enctype="multipart/form-data"></form>')
					.attr({
					    action: options.file.upload.url,
					    target: $uploadFrame.attr('name')
					})
					.appendTo('body');

                    // Embed data
                    var data = jQuery.extend({ path: $currentFolder.data(DataKeyFolderPath) }, options.file.upload.data);
                    for (var k in data) {
                        jQuery('<input type="hidden" />')
						.appendTo($uploadFrom)
						.attr({
						    name: k,
						    value: data[k]
						});
                    }

                    // Submit form
                    $uploadFrom
					.append($fileInput)
					.submit();

                    // For new upload file
                    $fileInput.appendTo(qtfile.uploadWraper)
                    $uploadFrom.remove();

                    var uploadChecker = setInterval(
					function() {
					    try {

					        // Upload frame has not been loaded
					        if (frameName) {

					            // If invalid page is loaded,
					            // this will cause an error in IE or FireFox,
					            // and Safari will still fire the load event with empty body.
					            // NOTE: If script debug is enabled in IE, an error dialog will popup.
					            jQuery('#' + frameName).contents();
					        } else {
					            clearInterval(uploadChecker);
					        }
					    } catch (e) {
					        clearInterval(uploadChecker);
					        jQuery('#' + frameName).remove();
					        frameName = null;
					        qtfile.unlock(TextFileUploadError, classes.statusError);
					    }
					},
				500);

                    // For new upload file
                    $fileInput.appendTo(qtfile.uploadWraper);
                    $uploadFrom.remove();
                }
            }
        },
        // Stores the information for moving file
        moveFileBegin: function($folderItem, $fileItem) {
            var qtfile = this;

            // Lock the file browser
            if (qtfile.tryLock(TextFileMoveInProgress, TextFileMoveWait)) {

                // Store the file path
                qtfile.fileMovingPath = qtfile.combinePath($folderItem.data(DataKeyFolderPath), $fileItem.data(DataKeyFileName));
            }
        }, // End of moveFileBegin()
        // Moves the specified file
        moveFileEnd: function($folderItem) {
            var qtfile = this, options = qtfile.options, fileMovingPath = qtfile.fileMovingPath;

            if (!fileMovingPath)
                return;

            var folderPath = $folderItem.data(DataKeyFolderPath),
			destPath = qtfile.combinePath(folderPath, qtfile.splitPath(fileMovingPath).name);

            // Send request to server
            qtfile.request(
			options.file.move,
			{
			    srcPath: fileMovingPath,
			    destPath: destPath
			},
			function(data) {
			    if (data.succeed) {

			        // Remove the file since it has been moved
			        qtfile.removeFile(qtfile.splitPath(fileMovingPath).name);
			    }
			    qtfile.unlock(translateFileMoveStatus(data.status),
					qtfile.statusClass(data));
			},
			function() {
			    qtfile.unlock(TextFileMoveError, options.classes.statusError);
			},
			function() { // onComplete
			    qtfile.fileMovingPath = null;
			}
		);
        }, // End of moveFileEnd()
        // Renames the specified file
        renameFile: function($folderItem, $fileItem) {
            var qtfile = this, options = qtfile.options;

            // Lock the file browser
            if (qtfile.tryLock(TextFileRenameFileInProgress, TextFileRenameWait)) {

                function pcallback(v, m, f) {

                    if (!v) { qtfile.unlock(); return false; }

                    if (f.namePrompt == "") { qtfile.unlock(); return false; }

                    var newFileName = f.namePrompt;
                    //remove spaces to make sure it can work in file browser dialog
                    newFileName = newFileName.split(' ').join('').toLowerCase().replace("'", "").replace('"', "");

                    // Folder path of the file
                    var path = $folderItem.data(DataKeyFolderPath);

                    var orgFileName = $fileItem.data(DataKeyFileName);

                    if (newFileName && newFileName != orgFileName) {

                        newFileName = newFileName + '.' + getExt(orgFileName);
                        // Test the file name
                        if (options.fileName.test(newFileName)) {

                            var srcPath = qtfile.combinePath(path, orgFileName);
                            var destPath = qtfile.combinePath(path, newFileName);

                            // Send request to server
                            qtfile.request(
						options.file.move,
						{
						    srcPath: srcPath,
						    destPath: destPath
						},
						function(data) {
						    if (data.succeed) {

						        // Update file name
						        var fileInfo = $fileItem.data('info');
						        fileInfo.name = newFileName;
						        qtfile.createFileItem(fileInfo).insertAfter($fileItem);
						        $fileItem.remove();
						        $fileItem = null;
						        qtfile.updateFileList();
						    }
						    qtfile.unlock(translateFileRenameStatus(data.status), qtfile.statusClass(data));
						},
						function() {
						    qtfile.unlock(TextFileRenameError, options.classes.statusError);
						}, null // onComplete
					);

                        } else {

                            // File name invalid
                            alert(TextFileNameInvalid);

                            qtfile.unlock();
                        } // End if
                    } else {
                        qtfile.unlock();
                    }
                }
            }

            function pLoadedcallback() {
                $('#namePrompt').focus();
            }

            // Ask for new name
            var p = TextFileRenameQuestion + '<br /> <input type="text" id="namePrompt" name="namePrompt" class="widetextbox" value="' + getNameOnly($fileItem.data(DataKeyFileName)) + '" />';
            var buttonSet = '{' + ButtonOK + ': true,' + ButtonCancel + ': false }';
            buttonSet = parseJSON(buttonSet);
            jQuery.prompt(p, { callback: pcallback, loaded: pLoadedcallback, buttons: buttonSet });

        }, // End of renameFile()
        // Deletes the specified file
        deleteFile: function($folderItem, $fileItem) {
            var qtfile = this, options = qtfile.options;

            // Lock the file browser
            if (qtfile.tryLock(TextFileDeleteInProgress, TextFileDeleteWait)) {

                // Folder path of the file
                var folderPath = $folderItem.data(DataKeyFolderPath),
				fileName = $fileItem.data(DataKeyFileName);

                if (!confirm(TextFileDeleteQuestion + ' ' + fileName)) {
                    qtfile.unlock();
                    return;
                }


                // File path
                var path = qtfile.combinePath(folderPath, fileName);

                // Send request to server
                qtfile.request(
				options.file.del,
				{
				    path: path
				},
				function(data) {
				    if (data.succeed) {
				        qtfile.removeFile($fileItem.data(DataKeyFileName));
				    }
				    qtfile.unlock(translateFileDeleteStatus(data.status), qtfile.statusClass(data));
				},
				function() {
				    qtfile.unlock(TextFileDeleteError, options.classes.statusError);
				}
			);

            }
        }, // End of deleteFile
        // Removes a file from current file list
        removeFile: function(name) {
            var qtfile = this, options = qtfile.options;
            qtfile.find(options.fileList + ' > ul > li').each(function() {
                var $item = jQuery(this);
                if ($item.data(DataKeyFileName) == name) {
                    if (qtfile.find(options.fileList + ' li').size() == 1) {
                        qtfile.find(options.fileList).addClass(options.classes.fileListEmpty);
                    }
                    $item.remove();
                    qtfile.updateFileList();
                }
            });
        } // End of removeFile
    });

    jQuery.extend(fn, {
        // Builds the initial folder list with data in the page
        initFolders: function() {
            var qtfile = this, options = qtfile.options, rootFolderName,
            // Root ul element of folder list
    		$folderList = qtfile.find(options.folderList + ' > ul'),
            // Folder path -> folder <li>
			folderPaths = qtfile.folderPaths = [];

            // Folder list not exists
            if ($folderList.size() == 0) {
                $folderList = jQuery(HtmlTagUl).appendTo(qtfile.find(options.folderList));
            }

            qtfile.folderList = $folderList;

            rootFolderName = jQuery.trim($folderList.attr(AttrTitle) || options.rootFolder);

            $folderList.removeAttr(AttrTitle).addClass(options.classes.folderList);

            // Save all folder paths
            $folderList.children(Li).each(function() {
                var $this = jQuery(this),
				path = parseJSON($this.text()).path;
                folderPaths[path] = qtfile.createFolderItem(path)
                // Copy css, e.g. class of selected folder
				.addClass($this.attr('class'));
                $this.remove();
            });

            // Empty to indicate root folder of current user
            qtfile.rootFolder = qtfile.createFolderItem('', rootFolderName).appendTo($folderList);

            qtfile.resetFolders();
        },
        // Creates a li element for a folder
        createFolderItem: function(fullPath, name) {
            var qtfile = this, classes = qtfile.options.classes,
			folderName = name || qtfile.splitPath(fullPath).name,

			$folderItem = jQuery(HtmlTagLi)
				.attr(AttrTitle, folderName)
				.data(DataKeyFolderPath, fullPath);

            // Folder bullet
            jQuery(HtmlTagSpan)
			.addClass(classes.folderBullet)
			.appendTo($folderItem);

            // Folder name
            jQuery(HtmlTagSpan)
			.addClass(classes.folderName)
			.click(function() {
			    qtfile.selectFolder(jQuery(this).parent(Li));
			})
            // Folder icon
			.append(jQuery(HtmlTagSpan).addClass(classes.folderIcon))
            // Folder name text
			.append(jQuery(HtmlTagSpan).addClass(classes.folderNameText).text(folderName))
            // Tail icon
			.append(jQuery(HtmlTagSpan).addClass(classes.folderTailIcon))
			.appendTo($folderItem);

            return $folderItem;
        }, // End of createFolderItem()
        // Handler for a folder is being selected
        selectFolder: function($folderItem) {
            var qtfile = this;
            if (qtfile.folderMovingPath)
                qtfile.moveFolderEnd($folderItem);
            else if (qtfile.fileMovingPath)
                qtfile.moveFileEnd($folderItem);
            else
                qtfile.changeFolder($folderItem);
        }, // End of onFolderNameClicked()
        resetFolders: function() {
            var qtfile = this,
			$rootFolder = qtfile.rootFolder,
			$currentFolder = qtfile.currentFolder,
			$folderList = qtfile.folderList;

            $rootFolder.children(Ul).remove();

            for (var path in qtfile.folderPaths) {
                qtfile.addFolder(path);
            }

            // Set root folder as current folder if none is current selected
            $currentFolder = $folderList.find(
			'.' + qtfile.options.classes.folderSelected);
            if ($currentFolder.size() == 0) {
                $currentFolder = $rootFolder;
            }

            qtfile.currentFolder = $currentFolder;
            qtfile.updateFolder($currentFolder);
        },
        addFolder: function(path) {
            var qtfile = this,
			$this = qtfile.folderPaths[path],

            // Extract folder path
			folder = qtfile.splitPath(path),
			folderName = folder.name,
			parentPath = folder.parent;

            qtfile.appendFolder($this, parentPath ?
			qtfile.folderPaths[parentPath] :
			qtfile.rootFolder);
        },
        appendFolder: function($folderItem, $parent) {
            if ($parent == undefined) { return; }
            if ($parent.children(Ul).size() == 0) {
                $parent.append(HtmlTagUl);
            }
            $folderItem.appendTo($parent.children(Ul));

            this.updateFolderIcon($parent);
        }, // End of appendFolder()
        updateFolderIcon: function($folder) {
            var classes = this.options.classes;
            if ($folder.children(Ul).children(Li).size()) {

                // add close/open folder list event
                $folder.children('.' + classes.folderBullet)
				.addClass(classes.folderBulletOpened)
				.unbind('click')
				.click(function() {
				    jQuery(this)
						.toggleClass(classes.folderBulletOpened)
						.toggleClass(classes.folderBulletClosed)
						.siblings(Ul)
				    // Open/Close subfolders
						.slideToggle('slow');
				});

            } else {
                $folder.children('.' + classes.folderBullet)
				.removeClass(classes.folderBulletOpened)
				.removeClass(classes.folderBulletClosed)
				.unbind('click');
                $folder.children(Ul).remove();
            }
        },
        // Updates current to a new folder
        updateFolder: function($newCurrentFolder) {
            var qtfile = this,
			$currentFolder = qtfile.currentFolder,
			classes = qtfile.options.classes;

            $currentFolder
			.removeClass(classes.folderSelected)
			.children('.' + classes.folderName)
			.removeClass(classes.folderNameSelected);

            qtfile.currentFolder =
			$currentFolder =
			$newCurrentFolder.addClass(classes.folderSelected);

            $currentFolder
			.children('.' + classes.folderName)
			.addClass(classes.folderNameSelected);
        }, // End of updateCurrentFolder()
        // Changes current folder and loads the file list
        changeFolder: function($newFolder, enforce) {
            var qtfile = this, options = qtfile.options;

            // Enforce reload or The folder is not currently selected
            if (enforce || !$newFolder.hasClass(options.classes.folderSelected)) {

                if (qtfile.tryLock(TextFileListInProgress, TextFileListWait)) {

                    // Send request to server
                    qtfile.request(
					options.file.list,
					{
					    path: $newFolder.data(DataKeyFolderPath)
					},
					function(data) {

					    // Make sure a file list is returned
					    if (jQuery.isArray(data)) {
					        var files = [];
					        jQuery.each(data, function() {
					            files.push(this);
					        });

					        // update current folder
					        qtfile.updateFolder($newFolder);

					        qtfile.resetFiles(files);

					        qtfile.unlock(TextFileListSucceed);
					    } else {
					        qtfile.unlock(translateFileListStatus(data.status), qtfile.statusClass(data));
					    }
					},
					function() {
					    qtfile.unlock(TextFileListError, options.classes.statusError);
					},
					function() {
					    qtfile.updateButtons();
					}
				);
                } // End if
            } // End if
        }, // End of changeCurrentFolder()
        // Creates a new folder
        createFolder: function($parentFolder) {
            //var qtfile = this, options = qtfile.options, folderName;
            var qtfile = this, options = qtfile.options;

            if (qtfile.tryLock(TextFolderCreateInProgress, TextFolderCreateWait)) {

                // Ask for folder name
                //folderName = prompt(TextFolderCreateQuestion, TextFolderCreateNewFolder);


                function pcallback(v, m, f) {

                    //alert(f.folderNamePrompt);
                    if (!v) { qtfile.unlock(); return false; }

                    if (f.namePrompt == "") { qtfile.unlock(); return false; }

                    var folderName = f.namePrompt;
                    //remove spaces to make sure it can work in file browser dialog
                    folderName = folderName.split(' ').join('').toLowerCase().replace("'", "").replace('"', "");

                    if (folderName) {
                        if (options.folderName.test(folderName)) {

                            isLoading = true;

                            var newFolderPath = qtfile.combinePath($parentFolder.data(DataKeyFolderPath), folderName);

                            // Send request to server
                            qtfile.request(
						options.folder.create,
						{
						    path: newFolderPath
						},
						function(data) {

						    if (data.succeed) {

						        // Append new folder to folder list
						        qtfile.folderPaths[newFolderPath] = qtfile.createFolderItem(newFolderPath);
						        qtfile.addFolder(newFolderPath);
						    }
						    qtfile.unlock(translateFolderCreateStatus(data.status), qtfile.statusClass(data));
						},
						function() {
						    qtfile.unlock(TextFolderCreateError, options.classes.statusError);
						}
					);

                        } else {
                            alert(TextFolderInvalidFolderName);
                            qtfile.unlock();
                        }
                    } else {
                        qtfile.unlock();
                    }
                }

            }

            function pLoadedcallback() {
                $('#namePrompt').focus();
            }

            var p = TextFolderCreateQuestion + '<br /> <input type="text" id="namePrompt" name="namePrompt" class="widetextbox" />';
            var buttonSet = '{' + ButtonOK + ': true,' + ButtonCancel + ': false }';
            buttonSet = parseJSON(buttonSet);
            jQuery.prompt(p, { callback: pcallback, loaded: pLoadedcallback, buttons: buttonSet });


        }, // End of createFolder
        // Renames a fodler
        renameFolder: function($folderItem) {
            var qtfile = this, $currentFolder = qtfile.currentFolder,
			options = qtfile.options,
			path = $currentFolder.data(DataKeyFolderPath);

            // Not root folder
            if (path.length) {

                if (qtfile.tryLock(TextFolderRenameInProgress, TextFolderRenameWait)) {

                    function pcallback(v, m, f) {

                        if (!v) { qtfile.unlock(); return false; }

                        if (f.namePrompt == "") { qtfile.unlock(); return false; }

                        var folderName = f.namePrompt;
                        //remove spaces to make sure it can work in file browser dialog
                        folderName = folderName.split(' ').join('').toLowerCase().replace("'", "").replace('"', "");

                        var currentName = qtfile.splitPath(path);

                        if (!folderName) {
                            qtfile.unlock();
                            return;
                        }

                        // Test folder name
                        if (!options.folderName.test(folderName)) {
                            alert(TextFolderInvalidFolderName);
                            qtfile.unlock();
                            return;
                        }

                        var destPath = qtfile.combinePath(currentName.parent, folderName);

                        // Send request to server
                        qtfile.request(
					options.folder.move,
					{
					    srcPath: path,
					    destPath: destPath
					},
					function(data) {
					    if (data.succeed) {

					        qtfile.updateFolderPath($currentFolder, destPath);

					        // update folder to new name
					        $currentFolder
								.children('.' + options.classes.folderName)
								.children('.' + options.classes.folderNameText)
								.text(folderName);
					    }

					    qtfile.unlock(translateFolderRenameStatus(data.status), qtfile.statusClass(data));
					},
					function() {
					    qtfile.unlock(TextFolderRenameError, options.classes.statusError);
					}
				); // End of request
                    } // End if
                } // End if


            }

            function pLoadedcallback() {
                $('#namePrompt').focus();
            }

            var currentFolderName = qtfile.splitPath(path).name;
            var p = TextFolderRenameQuestion + '<br /> <input type="text" id="namePrompt" name="namePrompt" class="widetextbox" value="' + currentFolderName + '" />';
            var buttonSet = '{' + ButtonOK + ': true,' + ButtonCancel + ': false }';
            buttonSet = parseJSON(buttonSet);
            jQuery.prompt(p, { callback: pcallback, loaded: pLoadedcallback, buttons: buttonSet });

        }, // End of renameFolder
        // Deletes the current folder
        deleteFolder: function($folderItem) {
            var qtfile = this, $currentFolder = qtfile.currentFolder,
			options = qtfile.options,
			path = $currentFolder.data(DataKeyFolderPath);

            // not root folder
            if (path.length) {

                if (qtfile.tryLock(TextFolderDeleteInProgress, TextFolderDeleteWait)) {

                    var currentFolderName = qtfile.splitPath(path).name;
                    if (!confirm(TextFolderDeleteQuestion + ' ' + currentFolderName)) {
                        qtfile.unlock();
                        return;
                    }

                    // Send request to server
                    qtfile.request(
					options.folder.del,
					{
					    path: path
					},
					function(data) {
					    if (data.succeed) {

					        var $parent = $currentFolder.parent(Ul).parent(Li);
					        $currentFolder.remove();
					        qtfile.updateFolderIcon($parent);

					        qtfile.unlock(TextFolderDeleteSucceed);

					        // Move to root folder, since current has been deleted
					        qtfile.changeFolder(qtfile.rootFolder);
					    } else {
					        qtfile.unlock(translateFolderDeleteStatus(data.status), qtfile.statusClass(data));
					    }

					},
					function() {
					    qtfile.unlock(TextFolderDeleteError, options.classes.statusError);
					}
				);

                } // End if
            } // End if
        }, // End of deleteFolder
        // Reload folder list
        reloadFolders: function() {
            var qtfile = this,
			options = qtfile.options;
            if (qtfile.tryLock(TextFolderListInProgress, TextFolderListWait)) {

                // Send request to server
                qtfile.request(
				options.folder.list,
				{},
				function(data) {

				    if (jQuery.isArray(data)) {
				        // Reset folder path
				        qtfile.folderPaths = [];

				        jQuery.each(data, function() {
				            qtfile.folderPaths[this.path] = qtfile.createFolderItem(this.path);
				        });

				        // Get original folder item before refresh folder list
				        var orgPath = qtfile.currentFolder.data(DataKeyFolderPath),
							$newCurrentFolder = orgPath.length > 0 ?
							qtfile.folderPaths[qtfile.currentFolder.data(DataKeyFolderPath)] :
							qtfile.rootFolder;

				        qtfile.resetFolders();
				        qtfile.unlock(TextFolderListSucceed);

				        if ($newCurrentFolder) {
				            // Update current folder since it has been rebuild
				            qtfile.updateFolder($newCurrentFolder);
				        }
				        else {
				            // Original current folder has been removed,
				            // move to root folder
				            qtfile.changeFolder(qtfile.rootFolder, true);
				        }
				    } else {
				        qtfile.unlock(data, qtfile.statusClass(data));
				    }
				},
				function() {
				    qtfile.unlock(TextFolderListError, options.classes.statusError);
				}
			); // End request
            }
        },
        // Updates the path info of a folder and its children
        updateFolderPath: function($folder, newPath) {
            var folderPaths = this.folderPaths,

			oldPath = $folder.data(DataKeyFolderPath);
            $folder.find(Li).andSelf().each(function() {
                $this = jQuery(this);

                // Make new path
                var oldSubfolderPath = $this.data(DataKeyFolderPath);
                var newSubfolderPath = oldSubfolderPath.replace(oldPath, newPath);

                // Update pate data
                $this.data(DataKeyFolderPath, newSubfolderPath);

                folderPaths[newSubfolderPath] = $this;
                delete folderPaths[oldSubfolderPath];

            });
        }, // End of updateFolderPath()
        // Marks a file as the file being moved
        moveFolderBegin: function($folderItem) {
            var qtfile = this;
            if (qtfile.tryLock(TextFolderMoveInProgress, TextFolderMoveWait)) {
                qtfile.folderMovingPath = $folderItem.data(DataKeyFolderPath);
            }
        },
        // Moves previous selected file to destination folder
        moveFolderEnd: function($newParent) {
            var qtfile = this, folderMovingPath = qtfile.folderMovingPath,
			options = qtfile.options, folderPaths = qtfile.folderPaths,
			sp = options.folderSeparator;
            if (!folderMovingPath)
                return;

            var newParentPath = $newParent.data(DataKeyFolderPath);

            // parent, current of subfolder selected
            if (folderMovingPath == newParentPath ||
			qtfile.splitPath(folderMovingPath).parent == newParentPath ||
			(sp + newParentPath + sp).indexOf(sp + folderMovingPath) == 0) {

                qtfile.folderMovingPath = null;

                var newLen = newParentPath.length,
				oldLen = folderMovingPath.length;

                if (newLen > oldLen) // Subfolder selected
                    qtfile.unlock(TextFolderMoveSubfolderSelected,
					options.classes.statusError);
                else if (newLen < oldLen) // Parent folder selected
                    qtfile.unlock(TextFolderMoveAlreadyExist,
					options.classes.statusError);
                else // Current folder selected
                    qtfile.unlock(TextFolderMoveCurrentSelected,
					options.classes.statusError);
                return;
            }

            var orgName = qtfile.splitPath(folderMovingPath),
			destPath = qtfile.combinePath(newParentPath, orgName.name);

            // Send request to server
            qtfile.request(
			options.folder.move,
			{
			    srcPath: folderMovingPath,
			    destPath: destPath
			},
			function(data) {
			    if (data.succeed) {

			        qtfile.updateFolderPath(qtfile.currentFolder, destPath);

			        qtfile.appendFolder(folderPaths[destPath], $newParent);

			        // Update old parent since it maybe empty
			        qtfile.updateFolderIcon(orgName.parent ? folderPaths[orgName.parent] : qtfile.rootFolder);
			    }
			    qtfile.unlock(translateFolderMoveStatus(data.status), qtfile.statusClass(data));

			},
			function() {
			    qtfile.unlock(TextFolderMoveError, options.classes.statusError);
			},
			function() {
			    qtfile.folderMovingPath = null;
			}); // End request
        }
    });

    jQuery.extend(fn, {
        // Builds buttons and binds event handlers to them
        initButtons: function() {
            var qtfile = this, options = qtfile.options;

            qtfile.find(options.buttonCreateFolder).click(function() {
                // Create new folder under current folder
                qtfile.createFolder(qtfile.currentFolder);
            });
            qtfile.find(options.buttonRenameFolder).click(function() {
                // Rename current folder
                qtfile.renameFolder(qtfile.currentFolder);
            });
            qtfile.find(options.buttonDeleteFolder).click(function() {
                // Delete current folder
                qtfile.deleteFolder(qtfile.currentFolder);
            });
            qtfile.find(options.buttonMoveFolder).click(function() {
                // Not root folder
                if (qtfile.currentFolder.data(DataKeyFolderPath).length) {
                    // Start to move current folder
                    qtfile.moveFolderBegin(qtfile.currentFolder);
                }
            });
            qtfile.find(options.buttonRefreshFolders).click(function() {
                qtfile.reloadFolders()
            });
            qtfile.find(options.buttonRefreshFiles).click(function() {
                // Enfoce reload
                qtfile.changeFolder(qtfile.currentFolder, true);
            });

            qtfile.initUploadInput();

            // Update button status according to current folder
            qtfile.updateButtons();
        }, // End of initButtons
        // Updates the buttons for folder operations
        updateButtons: function() {
            var qtfile = this, options = qtfile.options,
			disabledClass = options.classes.buttonDisabled;
            // Root folder
            if (qtfile.currentFolder.data(DataKeyFolderPath).length) {
                qtfile.find(options.buttonRenameFolder).removeClass(disabledClass);
                qtfile.find(options.buttonDeleteFolder).removeClass(disabledClass);
                qtfile.find(options.buttonMoveFolder).removeClass(disabledClass);
            } else {
                qtfile.find(options.buttonRenameFolder).addClass(disabledClass);
                qtfile.find(options.buttonDeleteFolder).addClass(disabledClass);
                qtfile.find(options.buttonMoveFolder).addClass(disabledClass);
            }
        }, // End of updateButtons
        // Initializes the button for upload file
        // A container wraps the upload button,
        // and hidden file input used to capture user input.
        initUploadInput: function() {
            var qtfile = this, options = qtfile.options,
			$uploadButton = qtfile.find(options.buttonUploadFile),
			createWraper = $uploadButton.is('input'),

			$uploadWraper = qtfile.uploadWraper =
				createWraper ? jQuery(HtmlTagSpan) : $uploadButton
				.mousemove(function(e) {

				    // Make sure pointer is inside button area
				    var buttonOffset = calcOffset($uploadButton);
				    if (e.pageX >= buttonOffset.left &&
						e.pageX < buttonOffset.left + $uploadButton.outerWidth() &&
						e.pageY >= buttonOffset.top &&
						e.pageY < buttonOffset.top + $uploadButton.outerHeight()) {

				        $uploadInput.show();

				        // Move the input with the mouse, so the user can't misclick it	
				        var offset = calcOffset(jQuery(this));
				        $uploadInput.css({
				            top: e.pageY - offset.top - 5 + 'px',
				            left: e.pageX - offset.left - 170 + 'px'
				        });
				    } else {
				        $uploadInput.hide();
				    }
				});

            if (createWraper) {
                $uploadWraper
                // Replace the original button
				.insertAfter($uploadButton)
				.append($uploadButton);

                // Delay to let firefox to calculate the size
                setTimeout(function() {

                    $uploadWraper
					.attr('class', $uploadButton.attr('class'))
					.css({
					    position: 'relative',
					    overflow: 'hidden',
					    padding: 0,

					    // Copy the margin and display to wraper
					    display: $uploadButton.css('display'),
					    'margin-top': $uploadButton.css('margin-top'),
					    'margin-right': $uploadButton.css('margin-right'),
					    'margin-bottom': $uploadButton.css('margin-bottom'),
					    'margin-left': $uploadButton.css('margin-left')
					});

                    $uploadButton
                    // Margin has been moved to wrapper
					.css({ 'margin': 0 });
                }, 1);
            } else {
                $uploadButton
				.css({ position: 'relative' });
            }

            // The actual file input
            $uploadInput = jQuery('<input type="file" />')
			.attr('name', options.file.upload.fileName)
			.attr('class', $uploadButton.attr('class'))
            // Set a fixed size, so that we can make sure 
            // the user will hit the upload button by
            // tracing mouse position
			.css({
			    position: 'absolute',
			    margin: 0,
			    padding: 0,
			    opacity: 0,
			    top: 0,
			    left: 0,
			    width: '220px',
			    height: '10px'
			})
			.appendTo($uploadWraper)
			.hide()
            // Bind upload handler
			.change(function() {
			    qtfile.uploadFile(jQuery(this));
			});
        } // End of buildUploadInput
    });

})();
