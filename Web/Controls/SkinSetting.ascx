<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SkinSetting.ascx.cs" Inherits="mojoPortal.Web.UI.SkinSetting" %>

<asp:DropDownList ID="dd" runat="server" DataValueField="Name" DataTextField="Name" EnableTheming="false" CssClass="forminput skinsetting"></asp:DropDownList>
<asp:HyperLink id="lnkPreview" runat="server" CssClass="skinpreviewlink" />
<asp:Literal id="litHiddenPreviewLinks" runat="server" />
