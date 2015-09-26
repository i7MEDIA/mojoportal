<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="RecentContentModule.ascx.cs" Inherits="mojoPortal.Web.ContentUI.RecentContentModule" %>

<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper recentcontent">
<portal:ModuleTitleControl EditText="Edit" runat="server" id="TitleControl" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<portal:RecentContentDisplaySettings id="displaySettings" runat="server" />
<portal:NoFollowHyperlink id="lnkFeedTop" runat="server" SkinID="RecentContent" CssClass="feedlink rssfeed" Visible="false" EnableViewstate="false" />
<asp:Repeater ID="rptResults" runat="server" EnableViewState="False">
    <HeaderTemplate>
        <ol class="searchresultlist">
    </HeaderTemplate>
    <ItemTemplate>
        <li class="searchresult">
            <NeatHtml:UntrustedContent ID="UntrustedContent1" runat="server" TrustedImageUrlPattern='<%# mojoPortal.Web.Framework.SecurityHelper.RegexRelativeImageUrlPatern %>'
                ClientScriptUrl="~/ClientScript/NeatHtml.js">
                <<%# displaySettings.ItemHeadingElement %>>
                    <asp:HyperLink ID="Hyperlink1" runat="server" 
                        NavigateUrl='<%# BuildUrl((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>'
                        Text='<%# FormatLinkText(Eval("PageName").ToString(), Eval("ModuleTitle").ToString(), Eval("Title").ToString())  %>' />
		            </<%# displaySettings.ItemHeadingElement %>>
                    <div id="divExcerpt" runat="server" visible='<%# config.ShowExcerpt && displaySettings.ShowExcerpt %>' class="searchresultdesc">
                        <%# Eval("ContentAbstract").ToString() %>
                    </div>
                <%# FormatAuthor(Eval("Author").ToString()) %>
                <%# FormatCreatedDate((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>
                <%# FormatModifiedDate((mojoPortal.SearchIndex.IndexItem)Container.DataItem) %>
            </NeatHtml:UntrustedContent>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ol>
    </FooterTemplate>
</asp:Repeater>
<portal:NoFollowHyperlink id="lnkFeedBottom" runat="server" SkinID="RecentContent" CssClass="feedlink rssfeed" Visible="false" EnableViewstate="false" />
<portal:mojoLabel ID="lblError" runat="server" CssClass="txterror info" />
</portal:InnerBodyPanel>
</portal:OuterBodyPanel>
<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
