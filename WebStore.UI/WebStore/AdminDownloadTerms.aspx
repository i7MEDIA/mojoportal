<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master"
    Codebehind="AdminDownloadTerms.aspx.cs" Inherits="WebStore.UI.AdminDownloadTermsPage" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
    <mp:CornerRounderTop id="ctop1" runat="server" />
    <portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper webstore webstoreadmindownloadterms">
        <portal:HeadingControl ID="heading" runat="server" />
        <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <mp:mojoGridView ID="grdFullfillDownloadTerms" runat="server" 
            AllowPaging="false" 
            AllowSorting="false"
            AutoGenerateColumns="false"  
            DataKeyNames="Guid">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CssClass="buttonlink" Text='<%# Resources.WebStoreResources.DownloadTermsGridEditButton %>' />
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel19" runat="server" CssClass="settinglabel" ConfigKey="DownloadTermsGridNameHeader"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtName"  Text='<%# Eval("Name") %>' runat="server"
                                MaxLength="255" CssClass="widetextbox forminput" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="DownloadTermsGridDescriptionHeader"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDescription" Columns="40" Rows="5" Text='<%# Eval("Description") %>'
                                runat="server" TextMode="MultiLine" CssClass="forminput" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="DownloadTermsGridDownloadsAllowedHeader"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtDownloadsAllowed"  Text='<%# Eval("DownloadsAllowed") %>'
                                runat="server" MaxLength="4" CssClass="smalltextbox forminput" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel3" runat="server" CssClass="settinglabel" ConfigKey="DownloadTermsGridExpireAfterDaysHeader"
                                ResourceFile="WebStoreResources" />
                            <asp:TextBox ID="txtExpireAfterDays" Text='<%# Eval("ExpireAfterDays") %>'
                                runat="server" MaxLength="4" CssClass="smalltextbox forminput" />
                        </div>
                        <div class="settingrow">
                            <mp:SiteLabel ID="SiteLabel4" runat="server" CssClass="settinglabel" ConfigKey="DownloadTermsGridCountAfterDownloadHeader"
                                ResourceFile="WebStoreResources" />
                            <asp:CheckBox ID="chkCountAfterDownload" Checked='<%# Eval("CountAfterDownload") %>'
                                runat="server" CssClass="forminput" />
                        </div>
                        <div class="forminput">
                            <asp:Button ID="btnGridUpdate" runat="server" Text='<%# Resources.WebStoreResources.DownloadTermsGridUpdateButton %>'
                                CommandName="Update" />
                            <asp:Button ID="btnGridDelete" runat="server" Text='<%# Resources.WebStoreResources.DownloadTermsGridDeleteButton %>'
                                CommandName="Delete" />
                            <asp:Button ID="btnGridCancel" runat="server" Text='<%# Resources.WebStoreResources.DownloadTermsGridCancelButton %>'
                                CommandName="Cancel" />
                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </mp:mojoGridView>
        <div class="settingrow">
            <portal:mojoButton ID="btnAddNew" runat="server" />
        </div>
        <portal:mojoCutePager ID="pgrDownloadTerms" runat="server" />
        <portal:EmptyPanel id="EmptyPanel1" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
        </portal:InnerBodyPanel>
        </portal:OuterBodyPanel>
        <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
    </portal:InnerWrapperPanel>
    <mp:CornerRounderBottom id="cbottom1" runat="server" />
    </portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
