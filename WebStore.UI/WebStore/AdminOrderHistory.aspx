<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" Codebehind="AdminOrderHistory.aspx.cs" Inherits="WebStore.UI.AdminOrderHistoryPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper cart">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdOrders" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="OrderGuid">
     <Columns>
     <asp:TemplateField>
        <ItemTemplate>
                <%# Eval("CustomerFirstName") %> <%# Eval("CustomerLastName") %><br />
                <%# Eval("CustomerEmail") %><br />
                <%# Eval("CompletedFromIP") %>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OrderTotal"))) %><br />
                <a href='<%# Eval("OrderGuid", detailUrlFormat)%>'><%# Resources.WebStoreResources.OrderHistoryViewDetailLink%></a>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("Completed")), timeZone, "g", timeOffset) %>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# GetOrderStatus(Eval("StatusGuid").ToString())%>
            </ItemTemplate>
     </asp:TemplateField>
    </Columns>
</mp:mojoGridView>
<portal:mojoCutePager ID="pgrOrders" runat="server" />
<portal:EmptyPanel id="EmptyPanel1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
