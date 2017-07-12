/// Author:				        
/// Created:			        2004-08-15
///	Last Modified:              2017-03-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.BlogUI
{
	public partial class BlogView : mojoBasePage
	{
		#region OnInit

		protected override void OnPreInit(EventArgs e)
		{
			AllowSkinOverride = true;
			base.OnPreInit(e);
		}

		override protected void OnInit(EventArgs e)
		{
			Load += new EventHandler(Page_Load);
			base.OnInit(e);

			if (BlogConfiguration.BlogViewSuppressPageMenu)
			{
				SuppressPageMenu();
			}
		}

		#endregion


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
		}

		//private int moduleId = -1;

		private void Page_Load(object sender, EventArgs e)
		{
			//moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			//pnlContainer.ModuleId = moduleId;

			if (SiteUtils.SslIsAvailable() && (siteSettings.UseSslOnAllPages || CurrentPage.RequireSsl))
			{
				SiteUtils.ForceSsl();
			}
			else
			{
				SiteUtils.ClearSsl();
			}

			if ((CurrentPage != null) && (CurrentPage.BodyCssClass.Length > 0))
			{
				AddClassToBody(CurrentPage.BodyCssClass);
			}

			AddClassToBody("blogviewpost");
		}


		protected override void OnError(EventArgs e)
		{
			Exception lastError = Server.GetLastError();
			if ((lastError != null) && (lastError is NullReferenceException) && Page.IsPostBack)
			{
				if (lastError.StackTrace.Contains("Recaptcha"))
				{
					Server.ClearError();
					WebUtils.SetupRedirect(this, Request.RawUrl);
				}
			}
		}
	}
}
