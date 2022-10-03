<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionalContentSetting.ascx.cs" Inherits="mojoPortal.Web.Controls.AdditionalContentSetting" %>
<%-- 
	Need:
		(1) ListBox showing all global content
		(2) ListBox for each Location (Top, Bottom, Navigation Top, Navigation Bottom)
			(2a) Sorting of each Location ListBox

	Logic:
		Selecting item in global content list box and moving it to a Location ListBox will publish the content in that location with the set SortOrder
	--%>


<%--<asp:QueryableFilterRepeater ID="it" runat="server" >
	<ItemTemplate>
		
	</ItemTemplate>
</asp:QueryableFilterRepeater>--%>

<asp:Literal ID="litGlobalContentList" runat="server"/>
<asp:HiddenField ID="hdnChosenModules" runat="server" />
