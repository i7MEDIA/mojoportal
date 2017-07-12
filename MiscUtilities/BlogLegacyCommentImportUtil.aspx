<%@ Page Language="C#" ClassName="NewsletterImporter.aspx" Inherits="System.Web.UI.Page"   %>
<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Data" %>
<%@ Import Namespace="mojoPortal.Web" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="mojoPortal.Web.Controls" %>
<%@ Import Namespace="mojoPortal.Web.UI" %>
<%@ Import Namespace="mojoPortal.Web.Editor" %>
<%@ Import Namespace="mojoPortal.Net" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="log4net" %>

<script runat="server">
	// Author:					
	// Created:					2012-08-27
	// Last Modified:			2012-08-27
	// 
	// This is example code for migrating comments from the legacy blog comment system into the new replacement comment system provided by mojoPortal core.
	// YOU ONLY NEED THIS IF YOU ARE NOT USING MS SQL -  we have automated migration for ms sql server as part of the upgrade script.
	// The reason we need an import tool for other db platforms is because we they don't have an equivalent data type as uniqueidentifier that can be automatically // // // populated during insert of rows by setting a default value as newid()
    // 
	// So without an easy way to generate the guid ids in those other platforms we use this utility which can migrate the data and generate a guid from .net code
	// to pass in for the id fields.
	//
	// NOTE: if you have any trouble migrating you "could" continue to use the older comment system that was part of the blog
	// by adding this in user.config:
	// <add key="Blog:UseLegacyCommentSystem" value="true"/>

    /// <summary>
    /// 
    /// </summary>
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    private static readonly ILog log = LogManager.GetLogger(typeof(Page));
    private CommentRepository repository = new CommentRepository();
    private SiteSettings siteSettings = null;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUser.IsAdmin)
        {
            SiteUtils.RedirectToAccessDeniedPage(this);
            return;
        }
        
        LoadSettings();

        // this is updating all sites so only allow it to be run from the master site
        
        if (!siteSettings.IsServerAdminSite)
        {
            SiteUtils.RedirectToAccessDeniedPage(this);
            return;
        }
        
        PopulateControls();
		
        
    }

   
          
    void btnMigrate_Click(object sender, EventArgs e)
    {

        DoMigration();

    }

    private void DoMigration()
    {
        DataTable dataTable = new DataTable();
        using (IDataReader reader = DatabaseHelper.GetReader(ConnectionString.GetReadConnectionString(), BuildSelectFromBlogComments()))
        {
            dataTable.Load(reader);
        }
        
        foreach(DataRow row in dataTable.Rows)
        {
            Comment comment = new Comment();
            comment.SiteGuid = new Guid(row["SiteGuid"].ToString());
            comment.FeatureGuid = new Guid(row["FeatureGuid"].ToString());
            comment.ModuleGuid = new Guid(row["Guid"].ToString());
            comment.ContentGuid = new Guid(row["BlogGuid"].ToString());
            comment.Title = row["Title"].ToString();
            comment.UserComment = row["Comment"].ToString();
            comment.UserName = row["Name"].ToString();
            comment.UserUrl = row["Url"].ToString();
            comment.CreatedUtc = Convert.ToDateTime(row["DateCreated"]);
            comment.LastModUtc = comment.CreatedUtc;

            repository.Save(comment);
        }

        lblMessage.Text = "Found and migrated " + dataTable.Rows.Count.ToInvariantString() + " rows.";

    }

    

	private void PopulateControls()
    {
        lblMessage.Text = string.Empty;
    }

    private string BuildSelectFromBlogComments()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT ");
        sql.Append("m.SiteGuid, ");
        sql.Append("m.FeatureGuid, ");
        sql.Append("m.Guid, ");
        sql.Append("b.BlogGuid, ");
        sql.Append("bc.Title, ");
        sql.Append("bc.Comment, ");
        sql.Append("bc.Name, ");
        sql.Append("bc.URL, ");
        sql.Append("bc.DateCreated, ");
        sql.Append("bc.DateCreated ");
        
        sql.Append("FROM mp_BlogComments bc ");
        
        sql.Append("JOIN mp_Blogs b ");
        sql.Append("ON b.ItemID = bc.ItemID ");
        
        sql.Append("JOIN mp_Modules m ");
        sql.Append("ON m.ModuleID = bc.ModuleID ");

        // make sure we don't migrate more than once if the button is clicked again
        sql.Append("LEFT OUTER JOIN mp_Comments c ");
        sql.Append("ON c.ContentGuid = b.BlogGuid ");
        sql.Append("WHERE ");
        sql.Append("c.ContentGuid IS NULL ");
        
        sql.Append(";");
        

        return sql.ToString();

    }


    
    private void LoadSettings()
    {

        siteSettings = CacheHelper.GetCurrentSiteSettings();

    }

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>mojoPortal Legacy Blog Comment Import Utility</title> 
</head>
<body>
 <form id="form1" runat="server">
<div>
<div>

Author:					
<br />Created:					2012-08-27
<br />Last Modified:			2012-08-27
<br />
<br />This is example code for migrating comments from the legacy blog comment system into the new replacement comment system provided by mojoPortal core.
	<br />YOU ONLY NEED THIS IF YOU ARE NOT USING MS SQL -  we have automated migration for ms sql server as part of the upgrade script.
<br />The reason we need an import tool for other db platforms is because we they don't have an equivalent data type as uniqueidentifier that can be automatically populated during insert of rows by setting a default value as newid()
    <p>
 So without an easy way to generate the guid ids in those other platforms we use this utility which can migrate the data and generate a guid from .net code
to pass in for the id fields.
        </p>
<p>
NOTE: if you have any trouble migrating you "could" continue to use the older comment system that was part of the blog
by adding this in user.config:
<br /> &lt;add key="Blog:UseLegacyCommentSystem" value="true"/&gt;
    </p>
  
</div>
<asp:Panel ID="pnlMigrate" runat="server">

<asp:Button ID="btnMigrate" runat="server" OnClick="btnMigrate_Click" Text="Import" />
<div>
<asp:Label runat="server" ID="lblMessage" />
</div>

</asp:Panel>


</div>
</form>
</body>
</html>