<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/DialogMaster.Master"
    CodeBehind="ContentRatingsDialog.aspx.cs" Inherits="mojoPortal.Web.AdminUI.ContentRatingsDialog" %>

<asp:Content ContentPlaceHolderID="phHead" ID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="phMain" ID="MainContent" runat="server">
    
    <div style="padding: 5px 5px 5px 5px;" class="yui-skin-sam">
        <mp:mojoGridView ID="grdContentRating" runat="server" CssClass="" AutoGenerateColumns="false"
            DataKeyNames="RowGuid">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("EmailAddress") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("IpAddress") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("CreatedUtc")), timeZone, "g", timeOffset) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Rating") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" EnableViewState="false"
                            ClientScriptUrl="~/ClientScript/NeatHtml.js">
                            <%# Eval("Comments") %>
                        </NeatHtml:UntrustedContent>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnDeleteRating" runat="server" Text='<%# Resources.Resource.DeleteButton %>'
                            CssClass="buttonlink" CommandArgument='<%# Eval("RowGuid") %>' CommandName='DeleteRating' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mp:mojoGridView>
        <portal:mojoCutePager ID="pgr" runat="server" />
    </div>
</asp:Content>
