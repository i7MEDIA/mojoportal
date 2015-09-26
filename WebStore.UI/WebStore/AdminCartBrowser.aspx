<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminCartBrowser.aspx.cs" Inherits="WebStore.UI.AdminCartBrowserPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper cart">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdCart" runat="server" 
    AllowPaging="false" 
    AllowSorting="false"
    AutoGenerateColumns="false" 
    CssClass="" 
    DataKeyNames="CartGuid">
     <Columns>
     <asp:TemplateField>
        <ItemTemplate>
            <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel8" runat="server" CssClass="settinglabel" ConfigKey="SiteUserLabel" ResourceFile="WebStoreResources" />
            <%# Eval("Name") %> <%# Eval("Email") %>
            </div>
            <div class="settingrow">
            <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="IPUser" ResourceFile="WebStoreResources" />
             <%# Eval("IPUser") %>
            </div>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("OrderTotal"))) %><br />
                <a href='<%# Eval("CartGuid", detailUrlFormat)%>'><%# Resources.WebStoreResources.CartBrowserViewCartLink %></a>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("Created")), timeZone, "g", timeOffset) %><br />
                <a class='cblink' title='<%# Eval("CreatedFromIP") %>' href='http://whois.arin.net/rest/ip/<%# Eval("CreatedFromIP") %>.txt'><%# Eval("CreatedFromIP")%></a>
            </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("LastModified")), timeZone, "g", timeOffset) %>
            </ItemTemplate>
     </asp:TemplateField>
    </Columns>
</mp:mojoGridView>

<div class="settingrow">
	<br /><asp:HyperLink ID="lnkAddNew" runat="server" />
</div>
<portal:mojoCutePager ID="pgrCart" runat="server" />
<br class="clear" />
<div class="settingrow">
<asp:CheckBox ID="chkOnlyAnonymous" runat="server" />
<portal:mojoButton ID="btnDelete" runat="server" />
<asp:TextBox ID="txtDaysOld" runat="server" CssClass="smalltextbox" /> <asp:Literal ID="litDays" runat="server" />

</div>

</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
