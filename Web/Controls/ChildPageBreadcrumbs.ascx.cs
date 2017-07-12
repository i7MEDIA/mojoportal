///		Author:				
///		Created:			2005-05-21
///		Last Modified:		2007-11-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{

	/// <summary>
	/// 8/20/2005 this control is now deprecated and disabled, Child breadcrumbs is now	
	/// integrated with Breadcrumbs control so these elements can be styled together
	/// </summary>
	public partial class ChildPageBreadCrumbs : System.Web.UI.UserControl
	{
		
        private Literal crumbs = new Literal();
        private string siteRoot = WebUtils.GetSiteRoot();
		private string cssClass = "txtmed";

		public string CssClass
		{	
			get {return cssClass;}
			set {cssClass = value;}
		}



		protected void Page_Load(object sender, System.EventArgs e)
		{
			//			siteSettings = (SiteSettings) HttpContext.Current.Items["SiteSettings"];
			//
			//			if(siteSettings.ActivePage.ShowChildPageBreadcrumbs)
			//			{
			//				isAdmin = WebUser.IsAdmin;
			//				isContentAdmin = WebUser.IsContentAdmin;
			//
			//				StringBuilder stringBuilder = new StringBuilder();
			//				stringBuilder.Append("&nbsp;>&nbsp;");
			//				string separator = string.Empty;
			//
			//				IDataReader reader = PageSettings.GetChildPagesByPageID(siteSettings.ActivePage.PageID);
			//			
			//				while(reader.Read())
			//				{
			//					string allowRoles = reader["AuthorizedRoles"].ToString();
			//					if((isAdmin)
			//						||((isContentAdmin)&&(allowRoles != "Admins;"))
			//						||(WebUser.IsInRoles(allowRoles))
			//						)
			//					{
			//						stringBuilder.Append(separator + "<a class='" + this.cssClass + "' href='" + siteRoot 
			//							+ "/Default.aspx?pageid=" + reader["PageID"].ToString() + "&pageindex=" 
			//							+ siteSettings.ActivePage.PageIndex.ToString() + "'>"
			//							+ reader["PageName"].ToString() + "</a>");
			//
			//						separator = "&nbsp;-&nbsp;";
			//					}
			//
			//				}
			//				reader.Close();
			//
			//				crumbs.Text = stringBuilder.ToString();
			//				if(crumbs.Text != "&nbsp;>&nbsp;")
			//				{
			//					this.Controls.Add(crumbs);
			//				}
			//			}



		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
