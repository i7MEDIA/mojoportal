<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CommentsModule.ascx.cs" Inherits="mojoPortal.Features.UI.CommentsModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
	<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper Comments">
		<portal:ModuleTitleControl EditText="Edit" EditUrl="~/Comments/CommentsEdit.aspx" runat="server" ID="TitleControl" />
		<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
			<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
				<portal:CommentsWidget ID="InternalCommentSystem" runat="server" />
			</portal:InnerBodyPanel>
		</portal:OuterBodyPanel>
		<portal:EmptyPanel ID="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
	</portal:InnerWrapperPanel>
</portal:OuterWrapperPanel>
