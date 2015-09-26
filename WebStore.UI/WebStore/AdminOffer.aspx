<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminOffer.aspx.cs" Inherits="WebStore.UI.AdminOfferPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreadminoffer">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <mp:mojoGridView ID="grdOffer" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false"  
            DataKeyNames="Guid">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkEdit" runat="server" Text='<%# Resources.WebStoreResources.OfferEditButton %>'
                            ToolTip='<%# Resources.WebStoreResources.OfferEditButton %>' NavigateUrl='<%# SiteRoot + "/WebStore/AdminOfferEdit.aspx" + BuildOfferQueryString(Eval("Guid").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("IsVisible") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("IsSpecial") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mp:mojoGridView>
        <div class="settingrow">
            <asp:HyperLink ID="lnkAddNew" runat="server" />
        </div>
        <portal:mojoCutePager ID="pgrOffer" runat="server" />
        <portal:EmptyPanel id="EmptyPanel1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom id="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
