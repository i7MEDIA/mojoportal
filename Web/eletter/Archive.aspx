<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Archive.aspx.cs" Inherits="mojoPortal.Web.ELetterUI.ArchivePage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
    <asp:HyperLink ID="lnkNewsletters" runat="server" CssClass="unselectedcrumb" />
</portal:AdminCrumbContainer>
<portal:mojoPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<asp:Panel id="pnlLetter" runat="server" CssClass="art-Post-inner panelwrapper letter">
<asp:Panel ID="pnlGrid" runat="server">
<h2 class="moduletitle heading"><asp:Literal ID="litHeading" runat="server" /></h2>
<portal:mojoPanel ID="MojoPanel1" runat="server" ArtisteerCssClass="art-PostContent">
<div class="modulecontent yui-skin-sam">
<mp:mojoGridView ID="grdLetter" runat="server"
	 CssClass=""
	 AutoGenerateColumns="false"
     DataKeyNames="LetterGuid">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
                <asp:HyperLink ID="lnkLetterView" runat="server" CssClass="cblink" NavigateUrl='<%# SiteRoot + "/eletter/LetterView.aspx?l=" + Eval("LetterInfoGuid") + "&letter=" + Eval("LetterGuid") %>'  Text='<%# Eval("Subject") %>' ToolTip='<%# Eval("Subject") %>' />
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("SendClickedUTC")), timeZone, "g", timeOffset) %>
            </ItemTemplate>
		</asp:TemplateField>	
</Columns>
<EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
    <portal:mojoCutePager ID="pgrLetter" runat="server" />
</div>
</portal:mojoPanel>
<div class="cleared"></div>
</asp:Panel>
<div class="modulecontent">
<asp:Label ID="lblMessage" runat="server"  />
</div>


</asp:Panel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:mojoPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
