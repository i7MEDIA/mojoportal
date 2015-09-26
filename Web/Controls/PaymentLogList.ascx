<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentLogList.ascx.cs" Inherits="mojoPortal.Web.UI.PaymentLogList" %>
<asp:Panel ID="pnlCheckoutLog" runat="server" CssClass="checkoutlog yui-skin-sam">
    <h2 class="heading"><asp:Literal ID="litHeading" runat="server" /></h2>
    <mp:mojoGridView ID="grdCheckoutLog" runat="server" 
        AllowPaging="false" 
        AllowSorting="false"
        CssClass="" 
        AutoGenerateColumns="false"
        DataKeyNames="RowGuid">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TransactionType") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("TransactionId") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Method")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("ResponseCode") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("Reason") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%# Eval("AuthCode") %>
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
    <portal:mojoCutePager ID="pgrCheckoutLog" runat="server" />
</asp:Panel>
