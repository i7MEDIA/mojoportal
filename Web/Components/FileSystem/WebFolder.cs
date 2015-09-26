/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */

using System;
using System.Web.Script.Serialization;

namespace mojoPortal.FileSystem
{
    /// <summary>
    /// Represents a user folder.
    /// </summary>
    public class WebFolder : AbstractFileSystemItem
    {
        /// <summary>
        /// Gets or sets the full path related to the root folder of a user.
        /// </summary>
        /// <remarks>
        /// The path should be able to embed in url as a query parameter.
        /// Usually, this can be achieved by replacing directory separator and url encoding.
        /// </remarks>
        /// <value>
        /// The piped path related to the root folder of a user for use in javascript qtfile.
        /// </value>
        public string Path { get; set; }

        //public string Name { get; set; }

        public virtual object ToJson()
        {
            return new { path = Path };
        }

        /// <summary>
        /// determines if the virtualChildPath is decendent to the virtualRootPath
        /// </summary>
        /// <param name="virtualRootPath"></param>
        /// <param name="virtualChildPath"></param>
        /// <returns></returns>
        public static bool IsDecendentVirtualPath(string virtualRootPath, string virtualChildPath)
        {
            string virtualChild;
            if (virtualChildPath.StartsWith("/"))
            {
                virtualChild = "~" + virtualChildPath;
            }
            else
            {
                virtualChild = virtualChildPath;
            }

            string sanitizedChildPath = SanitizePath(virtualChild);


            if (!string.Equals(sanitizedChildPath, virtualChild, StringComparison.InvariantCultureIgnoreCase)) { return false; }

            string virtualRoot;
            if (virtualRootPath.StartsWith("/"))
            {
                virtualRoot = "~" + virtualRootPath;
            }
            else
            {
                virtualRoot = virtualRootPath;
            }

            if (!virtualChild.StartsWith(virtualRoot)) { return false; }

            return true;
        }

        private static string SanitizePath(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }
            // not expecting ../ or \ or that kind of thing so remove it to prevent hacks 
            return input.Replace("..", string.Empty).Replace("\\", string.Empty).Trim();
        }

        
    }
}
