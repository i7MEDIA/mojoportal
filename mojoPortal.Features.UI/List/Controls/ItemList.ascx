<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ItemList.ascx.cs" Inherits="mojoPortal.Web.LinksUI.ItemList" %>
<%@ Register Namespace="mojoPortal.Web.LinksUI" Assembly="mojoPortal.Features.UI" TagPrefix="list" %>
<list:ListDisplaySettings ID="displaySettings" runat="server" />
<div class="listintro"><asp:Literal id="litIntro" runat="server" /></div>
<asp:UpdatePanel ID="updPnl" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                    <asp:Repeater ID="rptLinks" runat="server">
                        <HeaderTemplate>
                            <ul class="linkitem">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li class="linkitem">
                                <div id="div1" runat="server" enableviewstate="false" class="linkdesc" Visible="<%# Config.UseDescription && displaySettings.DescriptionAboveLink %>">
                                <asp:Literal ID="Literal1" runat="server"  Text='<%#  Eval("Description").ToString() %>' EnableViewState="false" />
                                </div>
                                <%# CreateLink(Eval("Title").ToString(), Eval("Url").ToString(), Eval("Description").ToString(), Eval("Target").ToString())%>
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# FormatEditUrl(Convert.ToInt32(Eval("ItemID"))) %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <div id="divDescBottom" runat="server" enableviewstate="false" class="linkdesc" Visible="<%# Config.UseDescription && !displaySettings.DescriptionAboveLink %>">
                                <asp:Literal ID="lblDescription2" runat="server"  Text='<%#  Eval("Description").ToString() %>' EnableViewState="false" />
                                </div>

                            </li>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <li class="linkaltitem">
                                <div id="div2" runat="server" enableviewstate="false" class="linkdesc" Visible="<%# Config.UseDescription && displaySettings.DescriptionAboveLink %>">
                                <asp:Literal ID="Literal2" runat="server"  Text='<%#  Eval("Description").ToString() %>' EnableViewState="false" />
                                </div>
                                <%# CreateLink(Eval("Title").ToString(), Eval("Url").ToString(), Eval("Description").ToString(), Eval("Target").ToString())%>
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text='<%# Resources.LinkResources.LinksEditLink %>'
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# FormatEditUrl(Convert.ToInt32(Eval("ItemID"))) %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>   
                                <div id="divDescBottom" runat="server" enableviewstate="false" class="linkdesc" Visible="<%# Config.UseDescription && !displaySettings.DescriptionAboveLink %>">
                                <asp:Literal ID="lblDescription2" runat="server"  Text='<%#  Eval("Description").ToString() %>' EnableViewState="false" />
                                </div>

                            </li>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </ul></FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptDescription" runat="server" Visible="false">
                        <ItemTemplate>
                            <div class="linkdesc">
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# FormatEditUrl(Convert.ToInt32(Eval("ItemID"))) %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <asp:Literal ID="lblDescription2" runat="server" Visible="<%# Config.UseDescription %>" Text='<%# Eval("Description").ToString()%>'
                                    EnableViewState="false" />
                            </div>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <div class="linkdesc linkdescalt">
                                <asp:HyperLink ID="editLink2" runat="server" EnableViewState="false" Text="<%# Resources.LinkResources.LinksEditLink %>"
                                    ToolTip="<%# Resources.LinkResources.LinksEditLink %>" NavigateUrl='<%# FormatEditUrl(Convert.ToInt32(Eval("ItemID"))) %>'
                                    Visible="<%# IsEditable%>" ImageUrl="<%# LinkImage %>"></asp:HyperLink>
                                <asp:Literal ID="lblDescription2" runat="server" Visible="<%# Config.UseDescription%>" Text='<%# Eval("Description").ToString()%>'
                                    EnableViewState="false" />
                            </div>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <portal:mojoCutePager ID="pgr" runat="server" Visible="false" />
                <portal:EmptyPanel id="divFooter" runat="server" CssClass="modulefooter" SkinID="modulefooter"></portal:EmptyPanel>
            </ContentTemplate>
        </asp:UpdatePanel>
