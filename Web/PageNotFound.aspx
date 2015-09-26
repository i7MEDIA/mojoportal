<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    CodeBehind="PageNotFound.aspx.cs" Inherits="mojoPortal.Web.PageNotFoundPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop ID="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper pagenotfound ">
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <div class="pnfmsgdiv">
            <span class="txterror pnfmsg">
                <asp:Literal ID="litErrorMessage" runat="server" EnableViewState="false" />
            </span>
        </div>
        <asp:Panel ID="pnlGoogle404Enhancement" runat="server" CssClass="pnfgoogdiv">
        
        <script type="text/javascript">
            var GOOG_FIXURL_LANG = '<%= CultureCode %>';
            var GOOG_FIXURL_SITE = '<%= SiteNavigationRoot %>';

            var searchInput = $(".searchpanel input[type=text]");
            var searchButton = $(".searchpanel input[type=submit]");
            if ((searchInput) && (searchButton)) {
                searchButton.click(function () {
                    window.location = GOOG_FIXURL_SITE + "/SearchResults.aspx?q=" + searchInput.val();
                    return false;
                });
            
            }
            
        </script>
        <script type="text/javascript" src="https://linkhelp.clients.google.com/tbproxy/lh/wm/fixurl.js"></script>
        
        </asp:Panel>
    </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
