<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GCheckoutLogList.ascx.cs" Inherits="mojoPortal.Web.UI.GCheckoutLogList" %>

<asp:Panel ID="pnlGoogleCheckoutLog" runat="server" CssClass="googlecheckoutlog yui-skin-sam">
    <h2 class="heading"><asp:Literal ID="litHeading" runat="server" /></h2>
    <mp:mojoGridView ID="grdGoogleCheckoutLog" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        CssClass="" 
        AutoGenerateColumns="false"
        DataKeyNames="RowGuid">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("NotificationType") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("SerialNumber") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("OrderNumber") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("BuyerId") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("FullfillState") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("FinanceState") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("CreatedUtc") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
    </mp:mojoGridView>
    <portal:mojoCutePager ID="pgrGoogleCheckoutLog" runat="server" />
    <br class="clear" />
</asp:Panel>