//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
// System.Web.UI.WebControls.AdCreatedEventArgs.cs
//
// Author:
//	Jackson Harper (jackson@ximian.com)
//
// 2008-10-13  added to this project from the Mono project, changed namespace
//2008-10-13  added support for OnClientClick

using System;
using System.Collections;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{

	// CAS
	[AspNetHostingPermission (SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission (SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AdCreatedEventArgs : EventArgs {

		private IDictionary properties;

		private string alt_text;
		private string img_url;
		private string nav_url;
        private string client_click;

		public AdCreatedEventArgs (IDictionary adProperties)
		{
			properties = adProperties;

			if (properties != null) {
				alt_text = (string) properties ["AlternateText"];
				img_url = (string) properties ["ImageUrl"];
				nav_url = (string) properties ["NavigateUrl"];
                client_click = (string)properties["OnClientClick"];
			}
		}

		public IDictionary AdProperties {
			get { return properties; }
		}

		public string AlternateText {
			get { return alt_text; }
			set { alt_text = value; }
		}

		public string ImageUrl {
			get { return img_url; }
			set { img_url = value; }
		}

		public string NavigateUrl {
			get { return nav_url; }
			set { nav_url = value; }
		}

        public string OnClientClick
        {
            get { return client_click; }
            set { client_click = value; }
        }
	}
}

