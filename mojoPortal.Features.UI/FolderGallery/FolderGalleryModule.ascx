<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="FolderGalleryModule.ascx.cs" Inherits="mojoPortal.Web.GalleryUI.FolderGalleryModule" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper FolderGallery">
<portal:ModuleTitleControl  runat="server"  id="TitleControl"  />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:Panel ID="pnlFolderGallery" runat="server" CssClass="foldergallery">
        <mp:album runat="server" ID="Album1" EnableViewState="false" >
            <FolderModeTemplate>
                <h3 class="center"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>' Visible='<%# (Eval("ParentFolder") != null) %>' /></h3>
                <div>
                    <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl='<%# Eval("ParentFolder.Link") %>'
                        Visible='<%# (Eval("ParentFolder") != null) %>'
                        CssClass='<%# Eval("ImageDivCssClass") %>'>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ParentFolder.IconUrl") %>'
                            AlternateText='<%# Eval("BackToParentTooltip") %>' /><br />
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("BackToParentText") %>' />
                    </asp:HyperLink>
                    <asp:Repeater runat="server" ID="SubFolders" DataSource='<%# Eval("SubFolders") %>'>
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("Link") %>' CssClass='<%# Eval("Owner.ImageDivCssClass") %>'>
                                <asp:Image ID="Image2" runat="server" ImageUrl='<%# Eval("IconUrl") %>'
                                    AlternateText='<%# Eval("Name", (string)Eval("Owner.OpenFolderTooltipFormatString")) %>' /><br />
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name") %>' />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="Images" DataSource='<%# Eval("Images") %>'>
                        <ItemTemplate>
                            <div id="Div1" runat="server" class='<%# Eval("Owner.ImageDivCssClass") %>'>
                                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl='<%# Eval("Link") %>'>
                                    <asp:Image ID="Image3" runat="server" ImageUrl='<%# Eval("IconUrl") %>'
                                        ToolTip='<%# Eval("Owner.DisplayImageTooltip") %>'
                                        AlternateText='<%# Eval("Caption") %>' />
                                </asp:HyperLink><br />&nbsp;
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </FolderModeTemplate>
            <PageModeTemplate>
                <h3 class="center"><asp:Label ID="Label4" runat="server" Text='<%# Eval("Title") %>' /></h3>
                <table><tr>
                    <td valign="top">
                        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl='<%# Eval("ParentFolder.Link") %>'>
                            <asp:Image ID="Image4" runat="server" ImageUrl='<%# Eval("ParentFolder.IconUrl") %>'
                                AlternateText='<%# Eval("BackToParentTooltip") %>' />
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl='<%# Eval("PreviousImage.Link") %>' Visible='<%# Eval("PreviousImage") != null %>'>
                            <asp:Image ID="Image5" runat="server" ImageUrl='<%# Eval("PreviousImage.IconUrl") %>'
                                AlternateText='<%# Eval("PreviousImage.Caption") %>' ToolTip='<%# Eval("PreviousImageTooltip") %>' />
                        </asp:HyperLink><br />
                        <asp:Panel ID="pnlDetails" runat="server" Visible='<%# ShowMetaDetailsSetting %>'>
                        <a href="javascript:void(0)" onclick="photoAlbumDetails(&quot;&quot;)" class="albumDetailsLink"><%# Page.Server.HtmlEncode(Resources.FolderGalleryResources.DetailsLink) %></a>
                        <div id="_details" style="display:none">
                            <asp:Repeater runat="server" ID="MetaData" DataSource='<%# Eval("Image.MetaData") %>'>
                                <HeaderTemplate><table></HeaderTemplate>
                                <ItemTemplate>
                                    <tr><td colspan="2" class="albumMetaSectionHead"><%# Eval("Key") %></td></tr>
                                    <asp:Repeater runat="server" ID="Tags" DataSource='<%# Eval("Value") %>'>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="albumMetaName"><%# Eval("Key") %></td>
                                                <td class="albumMetaValue"><%# Eval("Value") %></td>
                                            </tr>
                                        </ItemTemplate>
                                   </asp:Repeater>
                                </ItemTemplate>
                                <FooterTemplate></table></FooterTemplate>
                            </asp:Repeater>
                        </div>
                        </asp:Panel>
                    </td>
                    <td valign="top" rowspan="2">
                        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl='<%# Eval("Image.Url") %>' Target="_blank">
                            <asp:Image ID="Image6" runat="server" AlternateText='<%# Eval("DisplayFullResolutionTooltip") %>'
                                ImageUrl='<%# Eval("Image.PreviewUrl") %>' />
                        </asp:HyperLink><br />
                        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl='<%# Eval("PermaLink") %>' Visible='<%# ShowPermaLinksSetting %>'
                            Text='<%# Page.Server.HtmlEncode(Resources.FolderGalleryResources.PermaLinkLink) %>' />
                    </td>
                    <td id="Td1" runat="server" valign="top" rowspan="2" visible='<%# Eval("NextImage") != null %>'>
                        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl='<%# Eval("NextImage.Link") %>'>
                            <asp:Image ID="Image7" runat="server" ImageUrl='<%# Eval("NextImage.IconUrl") %>'
                                AlternateText='<%# Eval("NextImage.Caption") %>' ToolTip='<%# Eval("NextImageTooltip") %>' />
                        </asp:HyperLink>
                    </td>
                </tr></table>
            </PageModeTemplate>
        </mp:album>
        <mp:album runat="server" ID="Album2" EnableViewState="false" Visible="false" >
            <FolderModeTemplate>
                <h3 class="center"><asp:Label ID="Label1" runat="server" Text='<%# Eval("Title") %>' Visible='<%# (Eval("ParentFolder") != null) %>' /></h3>
                <div>
                    <asp:HyperLink ID="HyperLink1" runat="server"
                        NavigateUrl='<%# Eval("ParentFolder.Link") %>'
                        Visible='<%# (Eval("ParentFolder") != null) %>'
                        CssClass='<%# Eval("ImageDivCssClass") %>'>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ParentFolder.IconUrl") %>'
                            AlternateText='<%# Eval("BackToParentTooltip") %>' /><br />
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("BackToParentText") %>' />
                    </asp:HyperLink>
                    <asp:Repeater runat="server" ID="SubFolders" DataSource='<%# Eval("SubFolders") %>'>
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("Link") %>' CssClass='<%# Eval("Owner.ImageDivCssClass") %>'>
                                <asp:Image ID="Image2" runat="server" ImageUrl='<%# Eval("IconUrl") %>'
                                    AlternateText='<%# Eval("Name", (string)Eval("Owner.OpenFolderTooltipFormatString")) %>' /><br />
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Name") %>' />
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="Images" DataSource='<%# Eval("Images") %>'>
                        <ItemTemplate>
                            <div id="Div1" runat="server" class='<%# Eval("Owner.ImageDivCssClass") %>'>
                                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl='<%# Eval("Link") %>'>
                                    <asp:Image ID="Image3" runat="server" ImageUrl='<%# Eval("IconUrl") %>'
                                        ToolTip='<%# Eval("Owner.DisplayImageTooltip") %>'
                                        AlternateText='<%# Eval("Caption") %>' />
                                </asp:HyperLink><br />&nbsp;
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </FolderModeTemplate>
            <PageModeTemplate>
                <h3 class="center"><asp:Label ID="Label4" runat="server" Text='<%# Eval("Title") %>' /></h3>
                
                        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl='<%# Eval("ParentFolder.Link") %>'>
                            <asp:Image ID="Image4" runat="server" ImageUrl='<%# Eval("ParentFolder.IconUrl") %>'
                                AlternateText='<%# Eval("BackToParentTooltip") %>' />
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl='<%# Eval("PreviousImage.Link") %>' Visible='<%# Eval("PreviousImage") != null %>'>
                            <asp:Image ID="Image5" runat="server" ImageUrl='<%# Eval("PreviousImage.IconUrl") %>'
                                AlternateText='<%# Eval("PreviousImage.Caption") %>' ToolTip='<%# Eval("PreviousImageTooltip") %>' />
                        </asp:HyperLink><br />
                        <asp:Panel ID="pnlDetails" runat="server" Visible='<%# ShowMetaDetailsSetting %>'>
                        <a href="javascript:void(0)" onclick="photoAlbumDetails(&quot;&quot;)" class="albumDetailsLink"><%# Page.Server.HtmlEncode(Resources.FolderGalleryResources.DetailsLink) %></a>
                        <div id="_details" style="display:none">
                            <asp:Repeater runat="server" ID="MetaData" DataSource='<%# Eval("Image.MetaData") %>'>
                               
                                <ItemTemplate>
                                    <span class="albumMetaSectionHead"><%# Eval("Key") %></span>
                                    <asp:Repeater runat="server" ID="Tags" DataSource='<%# Eval("Value") %>'>
                                        <ItemTemplate>
                                           
                                                <span class="albumMetaName"><%# Eval("Key") %></span>
                                                <span class="albumMetaValue"><%# Eval("Value") %></span>
                                      
                                        </ItemTemplate>
                                   </asp:Repeater>
                                </ItemTemplate>
                                
                            </asp:Repeater>
                        </div>
                        </asp:Panel>
                    
                        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl='<%# Eval("Image.Url") %>' Target="_blank">
                            <asp:Image ID="Image6" runat="server" AlternateText='<%# Eval("DisplayFullResolutionTooltip") %>'
                                ImageUrl='<%# Eval("Image.PreviewUrl") %>' />
                        </asp:HyperLink><br />
                        <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl='<%# Eval("PermaLink") %>' Visible='<%# ShowPermaLinksSetting %>'
                            Text='<%# Page.Server.HtmlEncode(Resources.FolderGalleryResources.PermaLinkLink) %>' />
                    
                    <div id="Td1" runat="server" valign="top" rowspan="2" visible='<%# Eval("NextImage") != null %>'>
                        <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl='<%# Eval("NextImage.Link") %>'>
                            <asp:Image ID="Image7" runat="server" ImageUrl='<%# Eval("NextImage.IconUrl") %>'
                                AlternateText='<%# Eval("NextImage.Caption") %>' ToolTip='<%# Eval("NextImageTooltip") %>' />
                        </asp:HyperLink>
                    </div>
                
            </PageModeTemplate>
        </mp:album>
<div style="clear:both;">&nbsp;</div>
</asp:Panel>
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
