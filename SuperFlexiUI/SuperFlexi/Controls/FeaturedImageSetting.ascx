<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeaturedImageSetting.ascx.cs" Inherits="SuperFlexiUI.FeaturedImageSetting" %>
<%@ Register Assembly="SuperFlexiUI" Namespace="SuperFlexiUI" TagPrefix="SuperFlexi" %>
<div class="image-selector">
    <span class="featured-image-preview">
        <asp:Image ID="imagePreview" runat="server" CssClass="image-preview" />
        <button type="button" id="<% HttpContext.Current.Response.Write(this.ClientID.ToString()); %>_btnClearImage" 
            class="btn btn-danger image-remove" 
            onclick="var a = document.getElementById('<% HttpContext.Current.Response.Write(imageUrl.ClientID.ToString()); %>'); a.value=''; a.blur();
                ChangeImagePreview('<% HttpContext.Current.Response.Write(imagePreview.ClientID.ToString()); %>', '<% HttpContext.Current.Response.Write(browse.EmptyImageUrl.ToString()); %>'); ">
            <%= removeImageText %></button>
        <asp:TextBox ID="imageUrl" runat="server" />
    </span>

    <portal:FileBrowserTextBoxExtender ID="browse" runat="server" BrowserType="image" CssClass="btn btn-link" TabIndex="10"/>
<style>
    [src*='<%= config.FeaturedImageEmptyUrl %>'].image-preview + button { display: none; }
</style>
</div>