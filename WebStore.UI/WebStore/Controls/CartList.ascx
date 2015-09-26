<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CartList.ascx.cs" Inherits="WebStore.UI.Controls.CartList" %>

<asp:Repeater ID="rptCartItems" runat="server">
    <HeaderTemplate>
        <table class="cartgrid">
			<thead>
				<tr>
					<th class="cartitems">
						<%# Resources.WebStoreResources.CartItemsHeading%>
					</th>
					<th class="cartprice">
						<%# Resources.WebStoreResources.CartPriceHeading%>
					</th>
					<th class="cartquantity">
						<%# Resources.WebStoreResources.CartQuantityHeading%>
					</th>
					<th class="cartactions">&nbsp;</th>
				</tr>
			</thead>
			<tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td class="cartitems"><%# Eval("Name") %></td>
            <td class="cartprice"><%# string.Format(CurrencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></td>
            <td class="cartquantity"><asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' Columns="4" /></td>
            <td class="cartactions">
                <portal:mojoButton ID="btnUpdateQuantity" runat="server" Text='<%# Resources.WebStoreResources.UpdateQuantityButton %>' CommandName="updateQuantity" CommandArgument='<%# Eval("ItemGuid") %>' CausesValidation="false" CssClass="cartbutton" />
                <portal:mojoButton ID="btnDelete" runat="server" CssClass="cartbutton" CommandArgument='<%# Eval("ItemGuid") %>'
                    CommandName="delete" Text='<%# Resources.WebStoreResources.DeleteCartItemButton %>' CausesValidation="false" />
            </td>
        </tr>
    </ItemTemplate>
	<alternatingItemTemplate>
        <tr class="cartgrid-altrow">
            <td class="cartitems"><%# Eval("Name") %></td>
            <td class="cartprice"><%# string.Format(CurrencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></td>
            <td class="cartquantity"><asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' Columns="4" /></td>
            <td class="cartactions">
                <portal:mojoButton ID="btnUpdateQuantity" runat="server" Text='<%# Resources.WebStoreResources.UpdateQuantityButton %>' CommandName="updateQuantity" CommandArgument='<%# Eval("ItemGuid") %>' CausesValidation="false" CssClass="cartbutton" />
                <portal:mojoButton ID="btnDelete" runat="server" CssClass="cartbutton" CommandArgument='<%# Eval("ItemGuid") %>'
                    CommandName="delete" Text='<%# Resources.WebStoreResources.DeleteCartItemButton %>' CausesValidation="false" />
            </td>
        </tr>					
	</alternatingItemTemplate>
    <FooterTemplate>
		</tbody>
    </table>
    </FooterTemplate>
</asp:Repeater>
