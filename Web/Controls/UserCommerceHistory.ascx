<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UserCommerceHistory.ascx.cs" Inherits="mojoPortal.Web.UI.UserCommerceHistory" %>
<div class="yui-skin-sam">
<asp:UpdatePanel ID="updItems" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<mp:mojoGridView ID="grdUserItems" runat="server"
     AllowPaging="false"
     AllowSorting="false" 
	 AutoGenerateColumns="false">
     <Columns>
		<asp:TemplateField>
			<ItemTemplate>
			    <%# Eval("ModuleTitle") %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			    <span id="spnAdmin" runat="server" visible='<%# ShowAdminOrderLink %>'>
			    <a href='<%# SiteRoot + Eval("AdminOrderLink") %>'><%# Eval("ItemName") %></a>
			    </span>
			    <span id="Span1" runat="server" visible='<%# !ShowAdminOrderLink %>'>
			    <a href='<%# SiteRoot + Eval("UserOrderLink") %>'><%# Eval("ItemName") %></a>
			    </span>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			    <%# string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(Eval("SubTotal"))) %>
            </ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
            <ItemTemplate>
                <%# DateTimeHelper.Format(Convert.ToDateTime(Eval("OrderDateUtc")), timeZone, "g", timeOffset) %>
            </ItemTemplate>
     </asp:TemplateField>
		
</Columns>
<EmptyDataTemplate>
            <p class="nodata"><asp:Literal id="litempty" runat="server" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
    </EmptyDataTemplate>
</mp:mojoGridView>
<portal:mojoCutePager ID="pgrItems" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>

</div>