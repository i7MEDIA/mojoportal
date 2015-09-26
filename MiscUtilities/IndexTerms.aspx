<%@ Page Language="C#" ClassName="IndexUtil.aspx" Inherits="System.Web.UI.Page" %>

<%@ Import Namespace="mojoPortal.Business" %>
<%@ Import Namespace="mojoPortal.Business.WebHelpers" %>
<%@ Import Namespace="mojoPortal.Data" %>
<%@ Import Namespace="mojoPortal.Web" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="mojoPortal.Web.Controls" %>
<%@ Import Namespace="mojoPortal.Web.UI" %>
<%@ Import Namespace="mojoPortal.Web.Editor" %>
<%@ Import Namespace="mojoPortal.Net" %>
<%@ Import Namespace="mojoPortal.SearchIndex" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="log4net" %>

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
    private int minFrequencyToInclude = 1;
    private int maxFrequencyToInclude = -1; //no maximum


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!WebUser.IsAdmin)
        {
            SiteUtils.RedirectToAccessDeniedPage(this);
            return;
        }

        LoadSettings();


        BindTerms();
       


    }



    

    private void BindTerms()
    {
        List<IndexTerm> indexTerms 
            = mojoPortal.SearchIndex.IndexHelper.GetIndexTerms(
            siteSettings.SiteId, 
            minFrequencyToInclude,
            maxFrequencyToInclude);

        string searchUrl = SiteRoot + "/IndexTermDocs.aspx?p={0}";

        //pgrTop.PageURLFormat = searchUrl;
        //pgrTop.ShowFirstLast = true;
        //pgrTop.CurrentIndex = pageNumber;
        //pgrTop.PageSize = pageSize;
        //pgrTop.PageCount = totalPages;
        //pgrTop.Visible = (totalPages > 1);

        //pgrBottom.PageURLFormat = searchUrl;
        //pgrBottom.ShowFirstLast = true;
        //pgrBottom.CurrentIndex = pageNumber;
        //pgrBottom.PageSize = pageSize;
        //pgrBottom.PageCount = totalPages;
        pgrBottom.Visible = (totalPages > 1); //false for now

        rptResults.DataSource = indexTerms;
        rptResults.DataBind();

    }



    
    

    public string BuildUrl(mojoPortal.SearchIndex.IndexItem indexItem)
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

    }

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Index Terms Utility</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <asp:Panel ID="pnlSearchResults" runat="server"  CssClass="settingrow searchresults">
                <portal:mojoCutePager ID="pgrTop" runat="server" Visible="false" />
                <asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
                    <HeaderTemplate>
                        <ul class="searchresultlist">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="searchresult">      
		                    Term: <%# Eval("Term").ToString()  %> Frequency: <%# Eval("Frequency") %>       
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                <div>
                    &nbsp;
                </div>
                <portal:mojoCutePager ID="pgrBottom" runat="server" Visible="false" />
            </asp:Panel>
            <asp:Panel ID="pnlNoResults" runat="server" Visible="False">
                <asp:Label ID="lblNoResults" runat="server"></asp:Label>
            </asp:Panel>


        </div>
    </form>
</body>
</html>
