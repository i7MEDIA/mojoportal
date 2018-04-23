/// Author:					Dean Brettle
/// Created:				03/26/2006
/// Last Modified:			2018-03-28

using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Brettle.Web.NeatHtml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
	
	public partial class ContentValidation : Page
	{
		protected Button btnValidate;
        //protected string EditAltText = String.Empty;
        
        private string editContentImage = ConfigurationManager.AppSettings["EditContentImage"];
	
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) WebUtils.ForceSsl();

            //EditAltText = Resource.EditImageAltText;
			
			if (IsPostBack)
			{
				ShowInvalidContent("mp_GalleryImages", "description", "/GalleryImageEdit.aspx?ItemID={0}&mid={1}", "ItemID", "ModuleID");
				ShowInvalidContent("mp_CalendarEvents", "description", "/EventCalendarEdit.aspx?ItemID={0}&mid={1}", "ItemID", "ModuleID");
				ShowInvalidContent("mp_Blogs", "description", "/BlogEdit.aspx?ItemID={0}&mid={1}", "ItemID", "ModuleID");
				ShowInvalidContent("mp_BlogComments", "comment", "/BlogView.aspx?ItemID={0}&mid={1}", "ItemID", "ModuleID");
				ShowInvalidContent("mp_ForumPosts", "post", "/ForumPostEdit.aspx?postid={0}&thread={1}", "PostID", "ThreadID");
			}
		}
		
		private void ShowInvalidContent(string tableName, string contentFieldName, string linkFormat, params string[] fieldNames)
		{
			string schemaFolder = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/NeatHtml/schema" );
			string schemaFile = Path.Combine(schemaFolder, "NeatHtml.xsd");
			
            //XssFilter filter = XssFilter.GetForSchema(schemaFile);
            Filter filter = Filter.DefaultFilter;
				
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				tableName,
				String.Empty);
			tableResults.BorderWidth = Unit.Pixel(1);
			tableResults.BorderColor = Color.Black;
			tableResults.BorderStyle = BorderStyle.Solid;
			foreach(DataRow row in dataTable.Rows)
			{
				string htmlString = row[contentFieldName].ToString();
				try
				{
					filter.FilterUntrusted(htmlString);
				}
				catch (Exception ex)
				{
					HyperLink editLink = new HyperLink();
					editLink.ImageUrl = WebUtils.GetSiteRoot() + "/Data/SiteImages/" + editContentImage;
                    editLink.Text = Resource.EditImageAltText;
					string[] fieldValues = new string[fieldNames.Length];
					for (int i = 0; i < fieldNames.Length; i++)
					{
						fieldValues[i] = row[fieldNames[i]].ToString();
					}
					
					editLink.NavigateUrl = WebUtils.GetSiteRoot() + String.Format(linkFormat, fieldValues);
					
					TableRow tableRow = new TableRow();
					TableCell contentCell = new TableCell();
					contentCell.BorderWidth = Unit.Pixel(1);
					contentCell.BorderColor = Color.Black;
					contentCell.BorderStyle = BorderStyle.Solid;
					contentCell.Controls.Add(editLink);
					contentCell.Controls.Add(new LiteralControl(String.Format(@"<span style=""color: #ff0000;"">{0}</span>:<br>{1}",
			                         HttpUtility.HtmlEncode(ex.Message), HttpUtility.HtmlEncode(htmlString))));
					tableRow.Cells.Add(contentCell);
					this.tableResults.Rows.Add(tableRow);
				}
			}
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

	}
}
