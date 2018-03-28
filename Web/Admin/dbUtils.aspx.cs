/// Author:					
/// Created:				01/12/2006
/// Last Modified:			2018-03-28

using System;
using System.Data;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.AdminUI
{
	
	public partial class DBUtils : Page
	{
	
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

			this.lblResults.Text = String.Empty;
			this.lblHtmlResults.Text = String.Empty;
			this.lblBlogResults.Text = String.Empty;
			this.lblGalleryResults.Text = String.Empty;
			this.lblEventsResults.Text = String.Empty;
			this.lblForumResults.Text = String.Empty;

			this.lblLinksREsults.Text = String.Empty;

		}
		
		private bool IsEncoded(string s)
		{
			return (s.IndexOf("&lt;") > -1 && s.IndexOf("<") == -1);
		}
		
		private string Decode(string encoded)
		{
			return Server.HtmlDecode(encoded).Replace("scriptnotallowed", "script");
		}

		protected void btnDecodeHtml_Click(object sender, EventArgs e)
		{
			UpdateHtml();

			this.lblHtmlResults.Text = "Completed with no errors";
			
		}

		protected void btnDecodeBlog_Click(object sender, EventArgs e)
		{
			UpdateBlogs();
			UpdateBlogComments();

			this.lblBlogResults.Text = "Completed with no errors";
		
		}

		protected void btnDecodeGallery_Click(object sender, EventArgs e)
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_GalleryImages",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				
				if(IsEncoded(row["description"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_GalleryImages",
						"itemid",
						row["itemid"].ToString(),
						"description",
						Decode(row["description"].ToString()),
						String.Empty);
					
				}

				
				
				
			}

			this.lblGalleryResults.Text = "Completed with no errors";
		
		}

		protected void btnDecodeEvents_Click(object sender, EventArgs e)
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_CalendarEvents",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				if(IsEncoded(row["description"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_CalendarEvents",
						"itemid",
						row["itemid"].ToString(),
						"description",
						Decode(row["description"].ToString()),
						String.Empty);
						
				}

				
				
	
					
			}

			this.lblEventsResults.Text = "Completed with no errors";
			
		
		}

		protected void btnDecodeForums_Click(object sender, EventArgs e)
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_ForumPosts",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				if(IsEncoded(row["post"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_ForumPosts",
						"postid",
						row["postid"].ToString(),
						"post",
						Decode(row["post"].ToString()),
						String.Empty);
						
				}

				
				
	
					
			}

			this.lblForumResults.Text = "Completed with no errors";
		
		}

		protected void btnDecodeLinks_Click(object sender, EventArgs e)
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_Links",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				if(IsEncoded(row["description"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_Links",
						"itemid",
						row["itemid"].ToString(),
						"description",
						Decode(row["description"].ToString()),
						String.Empty);
						
				}
	
				
				
					
			}

			this.lblLinksREsults.Text = "Completed with no errors";
		
		}

		protected void btnEncodeDecode_Click(object sender, EventArgs e)
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				this.txtTableName.Text,
				this.txtWhereClause.Text);

			foreach(DataRow row in dataTable.Rows)
			{
				if(this.rbHtmlDecode.Checked)
				{
					if(IsEncoded(row[this.txtFieldName.Text].ToString()))
					{
						DatabaseHelper.UpdateTableField(
							this.txtConnectionString.Text,
							this.txtTableName.Text,
							this.txtPKField.Text,
							row[this.txtPKField.Text].ToString(),
							this.txtFieldName.Text,
							Decode(row[this.txtFieldName.Text].ToString()),
							this.txtWhereClause.Text);
						
					}

					
				}
				else
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						this.txtTableName.Text,
						this.txtPKField.Text,
						row[this.txtPKField.Text].ToString(),
						this.txtFieldName.Text,
						Server.HtmlEncode(row[this.txtFieldName.Text].ToString()),
						this.txtWhereClause.Text);
					
				}

				
				

			}

			this.lblResults.Text = "Completed with no errors";

		}

        protected void btnSetGuids_Click(object sender, EventArgs e)
        {
            DataTable dataTable = DatabaseHelper.GetTable(
                this.txtConnectionString.Text,
                "mp_Users",
                String.Empty);

            foreach (DataRow row in dataTable.Rows)
            {

               
                    DatabaseHelper.UpdateTableField(
                        this.txtConnectionString.Text,
                        "mp_Users",
                        "userid",
                        row["userid"].ToString(),
                        "userguid",
                        Guid.NewGuid().ToString(),
                        String.Empty);

            }

            dataTable = DatabaseHelper.GetTable(
                this.txtConnectionString.Text,
                "mp_Sites",
                String.Empty);

            foreach (DataRow row in dataTable.Rows)
            {


                DatabaseHelper.UpdateTableField(
                    this.txtConnectionString.Text,
                    "mp_Sites",
                    "siteid",
                    row["siteid"].ToString(),
                    "siteguid",
                    Guid.NewGuid().ToString(),
                    String.Empty);

            }

            this.lblResults.Text = "Completed with no errors";

        }

		

		private void UpdateHtml()
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_HtmlContent",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				if(IsEncoded(row["body"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_HtmlContent",
						"itemid",
						row["itemid"].ToString(),
						"body",
						Decode(row["body"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["excerpt"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_HtmlContent",
						"itemid",
						row["itemid"].ToString(),
						"excerpt",
						Decode(row["excerpt"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["title"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_HtmlContent",
						"itemid",
						row["itemid"].ToString(),
						"title",
						Decode(row["title"].ToString()),
						String.Empty);
					
				}

				
				

				
			}

			
				
			
		}

		private void UpdateBlogs()
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_Blogs",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				if(IsEncoded(row["title"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_Blogs",
						"itemid",
						row["ItemID"].ToString(),
						"title",
						Decode(row["title"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["excerpt"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_Blogs",
						"itemid",
						row["itemid"].ToString(),
						"excerpt",
						Decode(row["excerpt"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["description"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_Blogs",
						"itemid",
						row["itemid"].ToString(),
						"description",
						Decode(row["description"].ToString()),
						String.Empty);
					
				}

				
				

				
			}

			
				
			
		}

		private void UpdateBlogComments()
		{
			DataTable dataTable = DatabaseHelper.GetTable(
				this.txtConnectionString.Text,
				"mp_BlogComments",
				String.Empty);

			foreach(DataRow row in dataTable.Rows)
			{
				
				if(IsEncoded(row["Title"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_BlogComments",
						"blogcommentid",
						row["blogcommentid"].ToString(),
						"title",
						Decode(row["Title"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["Comment"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_BlogComments",
						"blogcommentid",
						row["blogcommentid"].ToString(),
						"comment",
						Decode(row["comment"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["name"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_BlogComments",
						"blogcommentid",
						row["blogcommentid"].ToString(),
						"name",
						Decode(row["name"].ToString()),
						String.Empty);
					
				}

				if(IsEncoded(row["url"].ToString()))
				{
					DatabaseHelper.UpdateTableField(
						this.txtConnectionString.Text,
						"mp_BlogComments",
						"blogcommentid",
						row["blogcommentid"].ToString(),
						"url",
						Decode(row["url"].ToString()),
						String.Empty);
					
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
