/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */


namespace mojoPortal.FileSystem
{
	/// <summary>
	/// Represents a user file.
	/// </summary>
	/// <remarks>
	/// One and only one of <see cref="Stream"/>, <see cref="Path"/> 
	/// or <see cref="Data"/> should be populated.
	/// </remarks>
	public class WebFile : WebFileInfo
	{
	   

		/// <summary>
		/// Gets or sets the path of the file.
		/// </summary>
		/// <value>
		/// The path of the file.
		/// </value>
		public string Path
		{
			get;
			set;
		}

		public string FolderVirtualPath
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the binary data of the file.
		/// </summary>
		/// <value>
		/// The binary data of the file.
		/// </value>
		public byte[] Data
		{
			get;
			set;
		}

		public string Extension
		{
			get { return System.IO.Path.GetExtension(Name); }
		}
	}
}
