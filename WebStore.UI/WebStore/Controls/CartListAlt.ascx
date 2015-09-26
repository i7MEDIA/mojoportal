<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartListAlt.ascx.cs" Inherits="WebStore.UI.Controls.CartListAlt" %>

<asp:Repeater ID="rptCartItems" runat="server">
    <HeaderTemplate>
			<ul class="cartlist">	
    </HeaderTemplate>
    <ItemTemplate>
        <li class="cartitem">
            <div class="cartitem"><%# Eval("Name") %></div>
            <div class="cartprice"><%# string.Format(CurrencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></div>
            <div class="cartquantity">
            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="smalltextbox" />
           
                <portal:mojoButton ID="btnUpdateQuantity" runat="server" Text='<%# Resources.WebStoreResources.UpdateQuantityButton %>' CommandName="updateQuantity" CommandArgument='<%# Eval("ItemGuid") %>' CausesValidation="false" CssClass="cartbutton" />
                <portal:mojoButton ID="btnDelete" runat="server" CssClass="cartbutton" CommandArgument='<%# Eval("ItemGuid") %>'
                    CommandName="delete" Text='<%# Resources.WebStoreResources.DeleteCartItemButton %>' CausesValidation="false" />
            </div>
        </li>
    </ItemTemplate>
	<alternatingItemTemplate>
        <li class="cartitem cartaltitem">
            <div class="cartitem"><%# Eval("Name") %></div>
            <div class="cartprice"><%# string.Format(CurrencyCulture, "{0:c}", Convert.ToDecimal(Eval("OfferPrice")))%></div>
            <div class="cartquantity">
            <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="smalltextbox" />
           
                <portal:mojoButton ID="btnUpdateQuantity" runat="server" Text='<%# Resources.WebStoreResources.UpdateQuantityButton %>' CommandName="updateQuantity" CommandArgument='<%# Eval("ItemGuid") %>' CausesValidation="false" CssClass="cartbutton" />
                <portal:mojoButton ID="btnDelete" runat="server" CssClass="cartbutton" CommandArgument='<%# Eval("ItemGuid") %>'
                    CommandName="delete" Text='<%# Resources.WebStoreResources.DeleteCartItemButton %>' CausesValidation="false" />
            </div>
        </li>					
	</alternatingItemTemplate>
    <FooterTemplate>
		</ul>	
    </FooterTemplate>
</asp:Repeater>