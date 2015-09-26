<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="MyPage.aspx.cs"  Inherits="mojoPortal.Features.UI.MyPage" %>

<%@ Register Assembly="mojoPortal.Features.MyPage.UI" Namespace="mojoPortal.Features.UI" TagPrefix="mpw" %>

<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
  <mp:CornerRounderTop id="ctop1" runat="server" />
	<asp:Panel id="pnlMyPage" runat="server" CssClass="mypage">
    <asp:LoginView ID="lvMessage" runat="server">
        <AnonymousTemplate>
        <strong><mp:SiteLabel id="lbl1" runat="server" ConfigKey="MyPageAnonymousMessage" ResourceFile="MyPageResources" UseLabelTag="false"></mp:SiteLabel></strong>
        </AnonymousTemplate>
        <LoggedInTemplate></LoggedInTemplate>
    </asp:LoginView>
    
    <div class="userpagemenucontainer">
    <asp:ImageButton ID="cmdCatalogView" runat="server" CssClass="ImageButton"  /> 
    <asp:ImageButton ID="cmdResetPersonalization" runat="server" CssClass="ImageButton"  />                               
    <asp:ImageButton ID="cmdPersonalizationModeToggle" CssClass="ImageButton" runat="server" />
    <portal:mojoHelpLink ID="MojoHelpLink11" runat="server" HelpKey="mypagehelp" />
    <mpw:mojoWebPartManager ID="WebPartManager1" runat="server" Personalization-Enabled="true" />
        <asp:Repeater ID="rptUserPageMenu" runat="server">
                    <HeaderTemplate>
                        <ul class="userpagemenu">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class='<%# GetCssClass(Eval("PagePath").ToString()) %>'>
                            <asp:Button ID="lnkUserPage" runat="server" CssClass="buttonlink"
                                Text='<%# Eval("PageName") %>' 
                                CommandName="selectpage" 
                                CommandArgument='<%# Eval("PagePath") %>'
                                />
                                <ul>
                                    <li class="userpagemenu-submenu">
					                    <asp:Button ID="LinkButton2" runat="server" CssClass="buttonlink"
                                            Text='<%# Resources.Resource.MyPageChangeNameText %>' 
                                            CommandName="changename" 
                                            CommandArgument='<%# Eval("UserPageID") %>'
                                            />
					                </li>
					                <li class="userpagemenu-submenu" id="liMoveRight" runat="server" visible='<%# CanMoveRight %>'>
					                    <asp:Button ID="LinkButton1" runat="server" CssClass="buttonlink"
                                            Text='<%# Resources.Resource.MyPageMoveRightText %>'
                                            Visible='<%# CanMoveRight %>'
                                            CommandName="moveright" 
                                            CommandArgument='<%# Eval("UserPageID") %>'
                                            />
					                </li>
					                <li class="userpagemenu-submenu" id="liMoveLeft" runat="server" visible='<%# CanMoveLeft %>'>
					                    <asp:Button ID="LinkButton3" runat="server" CssClass="buttonlink"
                                            Text='<%# Resources.Resource.MyPageMoveLeftText %>'
                                            Visible='<%# CanMoveLeft %>'
                                            CommandName="moveleft" 
                                            CommandArgument='<%# Eval("UserPageID") %>'
                                            />
					                </li>
					                <li class="userpagemenu-submenu" id="liDelete" runat="server" visible='<%# CanDeletePage %>'>
					                    <asp:Button ID="LinkButton4" runat="server" CssClass="buttonlink"
                                            Text='<%# Resources.Resource.MyPageRemoveText %>' 
                                            Visible='<%# CanDeletePage %>'
                                            CommandName="remove" 
                                            CommandArgument='<%# Eval("UserPageID") %>'
                                            />
					                </li>
					            </ul>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                            <li class="newuserpage">
                            <asp:Button ID="lnkMyPageAdd" runat="server" CssClass="buttonlink"
                                Text='New Page...' 
                                CommandName="addpage" 
                                CommandArgument=''
                                />
                            </li>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               
    </div>
    <asp:Panel ID="pnlAddPage" runat="server" Visible="false" CssClass="mypageminibox">
    <br />
         <asp:TextBox ID="txtNewPage" runat="server" CssClass="mypageminibox" />
         <asp:Button ID="btnNewPage" runat="server" CssClass="mypageminibox" />
         <asp:Button ID="btnCancelAddPage" runat="server" CssClass="mypageminibox" />
    </asp:Panel>
    <asp:Panel ID="pnlChangeName" runat="server" Visible="false" CssClass="mypageminibox">
    <br />
         <asp:TextBox ID="txtCurrentPageName" runat="server" CssClass="mypageminibox" />
         <asp:Button ID="btnChangeName" runat="server" CssClass="mypageminibox" />
         <asp:Button ID="btnCancelChangeName" runat="server" CssClass="mypageminibox" />
    </asp:Panel>
                
    <table width="99%" style="clear:both;" class="mypagewebparts">
        
        <tr>
            <td valign="top">
                <asp:WebPartZone ID="LeftWebPartZone" runat="server" 
                    Width="99%"
                    MenuVerbStyle-Font-Names="Verdana, Helvetica, sans-serif"
                    MenuVerbStyle-Font-Size="X-Small"
                    PartTitleStyle-CssClass="WebPartTitleStyle" 
                    
                    >
                    <ZoneTemplate></ZoneTemplate>
                </asp:WebPartZone>
            </td>
            <td valign="top">
                <asp:WebPartZone ID="CenterWebPartZone" runat="server" 
                    Width="99%"
                    MenuVerbStyle-Font-Names="Verdana, Helvetica, sans-serif"
                    MenuVerbStyle-Font-Size="X-Small"
                    PartTitleStyle-CssClass="WebPartTitleStyle"
                    >
                    <ZoneTemplate></ZoneTemplate>
                </asp:WebPartZone>
            </td>
            <td valign="top">
                <asp:EditorZone ID="EditorZone1" runat="server" Width="99%" >
                    <ZoneTemplate>
                        <asp:PropertyGridEditorPart ID="PropertyGridEditorPart1" runat="server" Title="<%$ Resources:Resource, WebPartPropertyGridEditorTitle %>" />
                        <asp:AppearanceEditorPart ID="AppearanceEditorPart1" runat="server" Title="<%$ Resources:Resource, WebPartAppearanceEditorTitle %>" Visible="false" />
                        <asp:LayoutEditorPart ID="LayoutEditorPart1" runat="server" Title="<%$ Resources:Resource, WebPartLayoutEditorPartTitle %>" Visible="false" />
                        <asp:BehaviorEditorPart ID="BehaviorEditorPart1" runat="server" Title="<%$ Resources:Resource, WebPartBehaviorEditorTitle %>" Visible="false"  />
                    </ZoneTemplate>
                </asp:EditorZone>
                <asp:CatalogZone ID="CatalogZone1" runat="server">
                    <ZoneTemplate>
                        <mpw:mojoUserCatalogPart ID="mojoUserCatalogPart1" runat="server" />
                        <asp:PageCatalogPart ID="PageCatalogPart1" runat="server" />
                    </ZoneTemplate>
                </asp:CatalogZone>
                <asp:ConnectionsZone ID="ConnectionZone1" runat="server">
                </asp:ConnectionsZone>
                <asp:WebPartZone ID="RightWebPartZone" runat="server" BorderStyle="solid" 
                    Width="98%"
                    MenuVerbStyle-Font-Names="Verdana, Helvetica, sans-serif"
                    MenuVerbStyle-Font-Size="X-Small"
                    PartTitleStyle-CssClass="WebPartTitleStyle"
                    >
                    <ZoneTemplate></ZoneTemplate>
                </asp:WebPartZone>
            </td>
        </tr>
    </table>
    </asp:Panel>
	<mp:CornerRounderBottom id="cbottom1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server"></asp:Content>
