<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="AdminDiscounts.aspx.cs" Inherits="WebStore.UI.AdminDiscountsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" EnableViewState="false"  />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper ">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<mp:mojoGridView ID="grdDiscount" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false" 
            DataKeyNames="DiscountGuid">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" Text='<%# Resources.WebStoreResources.DiscountEditLink %>'
                            ToolTip='<%# Resources.WebStoreResources.DiscountEditLink %>' NavigateUrl='<%# SiteRoot + "/WebStore/AdminDiscountEdit.aspx" + BuildQueryString(Eval("DiscountGuid").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("DiscountCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mp:mojoGridView>
           <portal:mojoCutePager ID="pgrDiscounts" runat="server" />
        <br class="clear" />
        <div class="settingrow">
            <asp:HyperLink ID="lnkNewDiscount" runat="server" />
        </div>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel> 
<mp:CornerRounderBottom id="cbottom1" runat="server" EnableViewState="false" />	
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
