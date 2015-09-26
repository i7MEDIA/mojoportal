<%@ Page Language="C#" ClassName="IndexBrowser.aspx" Inherits="System.Web.UI.Page" %>

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
<%@ Import Namespace="System.Threading" %>
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
    private DateTime modifiedBeginDate = DateTime.MinValue;
    private DateTime modifiedEndDate = DateTime.MaxValue;
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
            try
            {
                BindIndex();
                lblMessage.Text = string.Empty;
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
            catch (IOException ex)
            {
                lblMessage.Text = "This error happens when the search index has not been created yet. You could click the Rebuild Index button but if you do you should wait about 5 - 10 minutes before reloading the page.  " 
                + ex.Message + "   " + ex.StackTrace;
            }
            
        }


    }



    

    private void BindIndex()
    {
        
        IndexItemCollection searchResults = IndexHelper.Browse(
            siteSettings.SiteId,
            featureGuid,
            modifiedBeginDate,
            modifiedEndDate,
            pageNumber, 
            pageSize, 
            out totalHits);

       

        totalPages = 1;
       
        if (pageSize > 0) { totalPages = totalHits / pageSize; }

        if (totalHits <= pageSize)
        {
            totalPages = 1;
        }
        else
        {
            int remainder;
            Math.DivRem(totalHits, pageSize, out remainder);
            if (remainder > 0)
            {
                totalPages += 1;
            }
        }
        
        string searchUrl = SiteRoot
                + "/IndexBrowser.aspx?p={0}"
                + "&amp;bd=" + modifiedBeginDate.Date.ToString("s")
                + "&amp;ed=" + modifiedEndDate.Date.ToString("s")
                + "&amp;f=" + featureGuid.ToString();

        pgrTop.PageURLFormat = searchUrl;
        pgrTop.ShowFirstLast = true;
        pgrTop.CurrentIndex = pageNumber;
        pgrTop.PageSize = pageSize;
        pgrTop.PageCount = totalPages;
        pgrTop.Visible = (totalPages > 1);

        pgrBottom.PageURLFormat = searchUrl;
        pgrBottom.ShowFirstLast = true;
        pgrBottom.CurrentIndex = pageNumber;
        pgrBottom.PageSize = pageSize;
        pgrBottom.PageCount = totalPages;
        pgrBottom.Visible = (totalPages > 1);

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
            DateTime.TryParse(beginDate.Text, out modifiedBeginDate);
        }

        if (endDate.Text.Length > 0)
        {
            DateTime.TryParse(endDate.Text, out modifiedEndDate);
        }

        if (ddFeatureList.SelectedValue.Length == 36)
        {
            featureGuid = new Guid(ddFeatureList.SelectedValue);
        }
        
        string searchUrl = SiteRoot
                + "/IndexBrowser.aspx?p=1"
                + "&bd=" + modifiedBeginDate.Date.ToString("s")
                + "&ed=" + modifiedEndDate.Date.ToString("s")
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

    protected void rptResults_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (siteSettings == null) { return; }
        if (e.CommandName == "delete")
        {
            string indexKey = e.CommandArgument.ToString();
            IndexHelper.DeleteIndexDoc(siteSettings.SiteId, indexKey);
        }

        // get out of postback and refresh the list
        Response.Redirect("IndexBrowser.aspx");

    }

    protected void btnRebuildSearchIndex_Click(object sender, EventArgs e)
    {
        IndexingQueue.DeleteAll();
        IndexHelper.DeleteSearchIndex(siteSettings);
        IndexHelper.VerifySearchIndex(siteSettings);

        lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
        Thread.Sleep(1000); //wait 1 seconds
        SiteUtils.QueueIndexing();
    }



    private void LoadSettings()
    {

        siteSettings = CacheHelper.GetCurrentSiteSettings();
        pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);

        modifiedBeginDate = WebUtils.ParseDateFromQueryString("bd", DateTime.MinValue).Date;
        modifiedEndDate = WebUtils.ParseDateFromQueryString("ed", DateTime.MaxValue).Date;
        featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);

        if (!IsPostBack)
        {
            if (modifiedBeginDate.Date > DateTime.MinValue.Date)
            {
                beginDate.Text = modifiedBeginDate.ToShortDateString();
            }

            if (modifiedEndDate.Date < DateTime.MaxValue.Date)
            {
                endDate.Text = modifiedEndDate.ToShortDateString();
            }

        }

        btnRebuildSearchIndex.Text = Resource.SearchRebuildIndexButton;
        UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, Resource.SearchRebuildIndexWarning);
        

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

<!DOCTYPE html>
<html lang="en-US">
<head id="Head1" runat="server">
    <title>Index Browser Utility</title>
    <link rel='stylesheet' type='text/css' href='//ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/themes/base/jquery-ui.css' />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js" type="text/javascript" ></script>
<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.2/jquery-ui.min.js" type="text/javascript" ></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>
                <asp:DropDownList ID="ddFeatureList" runat="server" CssClass="searchfeatures"></asp:DropDownList>
                Modified Between <portal:jDatePicker ID="beginDate" runat="server" />
                and <portal:jDatePicker ID="endDate" runat="server" />
                <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
                <a href='/IndexBrowser.aspx'>Clear Filter</a>
            </div>
            <div>
                <portal:mojoButton ID="btnRebuildSearchIndex" runat="server" OnClick="btnRebuildSearchIndex_Click" />
                
            </div>
            <div>
                <portal:mojoLabel ID="lblMessage" runat="server" />
            </div>
            

            <asp:Panel ID="pnlSearchResults" runat="server"  CssClass="settingrow searchresults">
                <portal:mojoCutePager ID="pgrTop" runat="server" Visible="false" />
                <asp:Repeater ID="rptResults" runat="server" EnableViewState="true" OnItemCommand="rptResults_ItemCommand">
                    <HeaderTemplate>
                        <ul class="searchresultlist">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="searchresult">
                                <h3>
								<%# Eval("DocKey").ToString() %>
                                    <asp:HyperLink ID="Hyperlink1" runat="server" EnableViewState="false" 
                                        NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'
                                        Text='<%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %>' />
		                                        </h3>
                                <div class="searchresultdesc">
                                    <asp:Literal id="litIntro" runat="server" EnableViewState="false" Text='<%# Eval("ContentAbstract").ToString() %>' />
                                    <asp:Literal id="litAuthor" runat="server" EnableViewState="false" Text='<%# FormatAuthor(Eval("Author").ToString()) %>' />  
                                    <br />Page View Roles:     <asp:Literal id="litViewRoles" runat="server" EnableViewState="false" Text='<%# Eval("ViewRoles").ToString() %>' />
                                    <br />Module View Roles:   <asp:Literal id="litModuleRoles" runat="server" EnableViewState="false" Text='<%# Eval("ModuleViewRoles").ToString() %>' />
                                    <asp:Literal id="litMeta" runat="server" EnableViewState="false" Text='<%# FormatMetaDesc(Eval("PageMetaDescription").ToString()) %>' />
                                    <asp:Literal id="litKeywords" runat="server" EnableViewState="false" Text='<%# FormatMetaKeywords(Eval("PageMetaKeywords").ToString()) %>' />
                                    <br />CreatedUtc:          <asp:Literal id="litCreated" runat="server" EnableViewState="false" Text='<%# Eval("CreatedUtc").ToString() %>' />
                                    <br />ModifiedUtc:         <asp:Literal id="litMod" runat="server" EnableViewState="false" Text='<%# Eval("LastModUtc").ToString() %>' />
                                    <br /><portal:mojoButton id="btnDelete" runat="server" Text="Delete" 
                                        CommandName="delete" CommandArgument='<%# Eval("DocKey").ToString() %>' />
                                </div>
                            
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                <div>
                    <br />
                </div>
                <portal:mojoCutePager ID="pgrBottom" runat="server" Visible="false" />
            </asp:Panel>
           
        </div>
    </form>
</body>
</html>
