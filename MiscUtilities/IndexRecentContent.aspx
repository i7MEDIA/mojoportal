<%@ Page Language="C#" ClassName="IndexRecentContent.aspx" Inherits="System.Web.UI.Page" %>

<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.SearchIndex" %>
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
<%@ Import Namespace="Resources" %>

<script runat="server">
	
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    private static readonly ILog log = LogManager.GetLogger(typeof(Page));

    private SiteSettings siteSettings = null;
    protected string SiteRoot = string.Empty;
    private int pageNumber = 1;
    private int pageSize = WebConfigSettings.SearchResultsPageSize;
    private int totalHits = 0;
    private int totalPages = 1;
    private DateTime modifiedSinceDate = DateTime.UtcNow.AddDays(-30);
    private int maxItems = 20;
    private Guid featureGuid = Guid.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUser.IsAdmin)
        {
            SiteUtils.RedirectToAccessDeniedPage(this);
            return;
        }
        
        if(SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }

        LoadSettings();
        if (!IsPostBack)
        {
            BindIndex();


            
            BindFeatureList();
            ddFeatureList.Items.Insert(0, new ListItem(Resource.SearchAllContentItem, Guid.Empty.ToString()));
            if (ddFeatureList.Items.Count > 0)
            {
                ListItem item = ddFeatureList.Items.FindByValue(featureGuid.ToString());
                if (item != null)
                {
                    ddFeatureList.ClearSelection();
                    item.Selected = true;

                }
            }
            else
            {
                ddFeatureList.Visible = false;
            }
            
        }


    }



    

    private void BindIndex()
    {
        

        List<IndexItem> searchResults = IndexHelper.GetRecentContent(
            siteSettings.SiteId,
            featureGuid,
            modifiedSinceDate,
            maxItems);

        
        this.rptResults.DataSource = searchResults;
        this.rptResults.DataBind();
		
		

    }

    private void BindFeatureList()
    {
        using (IDataReader reader = ModuleDefinition.GetSearchableModules(siteSettings.SiteId))
        {
            ListItem listItem;

            // this flag tells it to look first for a web config setting for the resource string
            // corresponding to SearchListName value
            // it allows you to customize searchlist names wheeas by default they are just localized
            bool useConfigOverrides = true;

            while (reader.Read())
            {
                string featureid = reader["Guid"].ToString();

                if (!WebConfigSettings.SearchableFeatureGuidsToExclude.Contains(featureid))
                {
                    listItem = new ListItem(
                        ResourceHelper.GetResourceString(
                        reader["ResourceFile"].ToString(),
                        reader["SearchListName"].ToString(),
                        useConfigOverrides),
                        featureid);

                    ddFeatureList.Items.Add(listItem);
                }

            }

        }

    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (beginDate.Text.Length > 0)
        {
            DateTime.TryParse(beginDate.Text, out modifiedSinceDate);
        }

        int.TryParse(txtMaxItems.Text, out maxItems);

        if (ddFeatureList.SelectedValue.Length == 36)
        {
            featureGuid = new Guid(ddFeatureList.SelectedValue);
        }
        
        string searchUrl = SiteRoot
                + "/IndexRecentContent.aspx?p=1"
                + "&bd=" + modifiedSinceDate.Date.ToString("s")
                + "&limit=" + maxItems.ToInvariantString()
                + "&f=" + featureGuid.ToString();

        WebUtils.SetupRedirect(this, searchUrl);

    }


    

    protected string FormatLinkText(string pageName, string moduleTtile, string itemTitle)
    {
       
        if (itemTitle.Length > 0)
        {
            return pageName + " &gt; " + itemTitle;
        }


        return pageName;


    }

    public string BuildUrl(IndexItem indexItem)
    {
        if (indexItem.UseQueryStringParams)
        {
            return SiteRoot + "/" + indexItem.ViewPage
                + "?pageid="
                + indexItem.PageId.ToInvariantString()
                + "&mid="
                + indexItem.ModuleId.ToInvariantString()
                + "&ItemID="
                + indexItem.ItemId.ToInvariantString()
                + indexItem.QueryStringAddendum;

        }
        else
        {
            return SiteRoot + "/" + indexItem.ViewPage;
        }

    }



    private void LoadSettings()
    {

        siteSettings = CacheHelper.GetCurrentSiteSettings();
        pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);
        maxItems = WebUtils.ParseInt32FromQueryString("limit", maxItems);

        modifiedSinceDate = WebUtils.ParseDateFromQueryString("bd", modifiedSinceDate).Date;
        
        featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);

        if (!IsPostBack)
        {
            if (modifiedSinceDate.Date > DateTime.MinValue.Date)
            {
                beginDate.Text = modifiedSinceDate.ToShortDateString();
            }

            txtMaxItems.Text = maxItems.ToInvariantString();

        }
        

    }

    protected string FormatAuthor(string author)
    {
        if (string.IsNullOrEmpty(author)) { return string.Empty; }

        return "<br />Author: " + author;
    }

    protected string FormatMetaDesc(string metaDesc)
    {
        if (string.IsNullOrEmpty(metaDesc)) { return string.Empty; }

        return "<br />Page Meta Desc: " + metaDesc;
    }

    protected string FormatMetaKeywords(string keyWords)
    {
        if (string.IsNullOrEmpty(keyWords)) { return string.Empty; }

        return "<br />Page Meta Keywords: " + keyWords;
    }

    
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Index Recent Content Utility</title>
    <link rel='stylesheet' type='text/css' href='//ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/themes/base/jquery-ui.css' />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript" ></script>
<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/jquery-ui.min.js" type="text/javascript" ></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                Top <asp:TextBox ID="txtMaxItems" runat="server" Text="20" /> Items
                <asp:DropDownList ID="ddFeatureList" runat="server" CssClass="searchfeatures"></asp:DropDownList>
                Modified Since <portal:jDatePicker ID="beginDate" runat="server" />
                <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
                <a href='/IndexRecentContent.aspx'>Clear Filter</a>
            </div>
            
            <asp:Panel ID="pnlSearchResults" runat="server"  CssClass="settingrow searchresults">
                <asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
                    <HeaderTemplate>
                        <ul class="searchresultlist">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="searchresult">
                                <h3>
                                    <asp:HyperLink ID="Hyperlink1" runat="server" NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'>
		                                        <%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %></asp:HyperLink></h3>
                                <div class="searchresultdesc">
                                    <%# Eval("Intro").ToString() %>
                                    <%# FormatAuthor(Eval("Author").ToString()) %>  
                                    <br />Page View Roles:     <%# Eval("ViewRoles").ToString() %>
                                    <br />Module View Roles:   <%# Eval("ModuleViewRoles").ToString() %>
                                    <%# FormatMetaDesc(Eval("PageMetaDescription").ToString()) %>
                                    <%# FormatMetaKeywords(Eval("PageMetaKeywords").ToString()) %>
                                    <br />CreatedUtc:          <%# Eval("CreatedUtc").ToString() %>
                                    <br />ModifiedUtc:         <%# Eval("LastModUtc").ToString() %>
                                </div>
                            
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
               
            </asp:Panel>
            


        </div>
    </form>
</body>
</html>
