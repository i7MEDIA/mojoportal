<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RejectedContent.aspx.cs" 
    MasterPageFile="~/App_MasterPages/layout.Master" Inherits="mojoPortal.Web.AdminUI.RejectedContentPage" %>
    
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:AdminCrumbContainer ID="pnlAdminCrumbs" runat="server" CssClass="breadcrumbs">
        <asp:HyperLink ID="lnkAdminMenu" runat="server" NavigateUrl="~/Admin/AdminMenu.aspx" /><portal:AdminCrumbSeparator id="litLinkSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkContentWorkFlow" runat="server"  NavigateUrl="~/Admin/ContentWorkflow.aspx" /><portal:AdminCrumbSeparator id="AdminCrumbSeparator1" runat="server" Text="&nbsp;&gt;" EnableViewState="false" />
        <asp:HyperLink ID="lnkCurrentPage" runat="server" CssClass="selectedcrumb" />
 </portal:AdminCrumbContainer>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />    
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper admin workflow">
<portal:HeadingControl id="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
   <asp:literal id="ltlIntroduction" runat="server"></asp:literal>
   <mp:mojoGridView ID="grdRejectedContent" runat="server" AllowPaging="false" AllowSorting="false"
        AutoGenerateColumns="false"  DataKeyNames="Guid">
        <Columns>            
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("ModuleTitle")%>
                </ItemTemplate>                                
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("RecentActionByUserName")%>
                </ItemTemplate>                                
            </asp:TemplateField>
            <asp:TemplateField>
	            <ItemTemplate>
	                <%# DateTimeHelper.GetTimeZoneAdjustedDateTimeString(Eval("RecentActionOn"), timeOffset)%>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Server.HtmlEncode(Eval("Notes").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Repeater ID="rptPageLinks" runat="server">
                        <ItemTemplate>
                            <a href='<%# SiteRoot + Eval("PageUrl").ToString().Replace("~/","/") + "?vm=WorkInProgess"%>'><%# Eval("PageName")%></a>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
        <EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
    </mp:mojoGridView>
    <portal:mojoCutePager ID="pgrRejectedContent" runat="server" />
    </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
<portal:SessionKeepAliveControl id="ka1" runat="server" />

</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />

