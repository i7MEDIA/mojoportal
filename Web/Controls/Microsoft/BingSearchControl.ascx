<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BingSearchControl.ascx.cs"
    Inherits="mojoPortal.Web.UI.BingSearchControl" %>
<asp:UpdatePanel ID="upBing" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlBingSearch" runat="server" DefaultButton="btnSearch" CssClass="settingrow search bingsearch">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="verywidetextbox" />
            <portal:mojoButton ID="btnSearch" runat="server" />
            <asp:Label ID="lblPoweredByBing" runat="server" CssClass="bingpowered" />
        </asp:Panel>
        <div class="settingrow searchresults bingresults">
            <asp:Repeater ID="rptResults" runat="server" EnableViewState="false">
                <HeaderTemplate>
                    <ol class="searchresultlist">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="searchresult">
                        <h3>
                            <a href="<%# Eval("Url") %>">
                                <%# Eval("Title") %></a></h3>
                        <div class="searchresultdesc">
                            <%# Eval("Description") %></div>
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ol>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <asp:Panel ID="pnlNoResults" runat="server" Visible="false" CssClass="noresults">
            <asp:Label ID="lblNoResults" runat="server"></asp:Label>
        </asp:Panel>
        <div class="settingrow">
            <portal:mojoButton ID="btnFirst" runat="server" Visible="false" />
            <portal:mojoButton ID="btnPrevious" runat="server" Visible="false" />
            <portal:mojoButton ID="btnNext" runat="server" Visible="false" />
            <asp:HiddenField ID="hdnOffset" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSHC" runat="server" Value="" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
