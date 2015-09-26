<%@ Page Language="c#" CodeBehind="HtmlEdit.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master"
    AutoEventWireup="false" Inherits="mojoPortal.Web.ContentUI.EditHtml" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
   <portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper editpage htmlmodule">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
        <asp:Panel ID="pnlEdit" runat="server"  DefaultButton="btnUpdate">
                <mpe:EditorControl ID="edContent" runat="server">
                </mpe:EditorControl>
				<div id="divExcludeFromRecentContent" runat="server" class="settingrow">
                <mp:SiteLabel ID="Sitelabel12" runat="server" ForControl="chkExcludeFromRecentContent" CssClass="settinglabel"
                    ConfigKey="ExcludeFromRecentContent"></mp:SiteLabel>
                <asp:CheckBox ID="chkExcludeFromRecentContent" runat="server" CssClass="forminput"></asp:CheckBox>
                <portal:mojoHelpLink ID="MojoHelpLink8" runat="server" HelpKey="ExcludeFromRecentContent-help" />
            </div>
                <div class="settingrow">
                </div>
                <div class="settingrow">
                    <portal:mojoButton ID="btnUpdateDraft" runat="server" Text="" Visible="false" />&nbsp;
                    <portal:mojoButton ID="btnPublishDraft" runat="server" Text="" Visible="false" />&nbsp;
                    <portal:mojoButton ID="btnUpdate" runat="server" Text="" />&nbsp;
                    <portal:mojoButton ID="btnDelete" runat="server" Text="" CausesValidation="False" />&nbsp;
                    <asp:HyperLink id="lnkCompareDraft" runat="server" CssClass="cblink" Visible="false" />
                    <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancellink" />&nbsp;
                    <portal:mojoHelpLink ID="MojoHelpLink11" runat="server" HelpKey="htmlcontentedithelp" />
                    <asp:HiddenField ID="hdnHxToRestore" runat="server" />
                    <asp:ImageButton ID="btnRestoreFromGreyBox" runat="server" />
                </div>
                <asp:Panel ID="pnlWorkflowNotice" runat="server" Visible="false" CssClass="wf-noticepanel" EnableViewState="false">
                    <mp:SiteLabel ID="lblWorkflowNotice" runat="server" CssClass="txterror warning" ConfigKey="ContentEditsInProgressNotice" />
                </asp:Panel>
                <asp:Panel ID="pnlWorkflowStatus" runat="server" Visible="false" CssClass="wf-statuspanel">
                    <div class="settingrow wf-status">
                        <mp:SiteLabel ID="lblContentStatusLabel" runat="server" CssClass="settinglabel" ConfigKey="ContentStatusLabel">
                        </mp:SiteLabel>	
                        <asp:Literal ID="litWorkflowStatus" runat="server"></asp:Literal>
                    </div>
                    <div class="settingrow wf-lastmodifiedby">
                        <mp:SiteLabel ID="lblRecentActionBy" runat="server" CssClass="settinglabel" ConfigKey="RejectedBy">
                        </mp:SiteLabel>
                        <asp:Literal ID="litRecentActionBy" runat="server"></asp:Literal>
                    </div>
                    <div class="settingrow wf-lastmodifieddate">
                        <mp:SiteLabel ID="lblRecentActionOn" runat="server" CssClass="settinglabel" ConfigKey="RejectedOn">
                        </mp:SiteLabel>
                        <asp:Literal ID="litRecentActionOn" runat="server"></asp:Literal>
                    </div>
                    <div id="divRejection" runat="server" class="wf-rejectedpanel">
                        <div class="settingrow wf-rejectedby">
                            <mp:SiteLabel ID="SiteLabel2" runat="server" CssClass="settinglabel" ConfigKey="ContentLastEditBy">
                            </mp:SiteLabel>
                            <asp:Literal ID="litCreatedBy" runat="server"></asp:Literal>
                        </div>
                        <div class="settingrow wf-rejectionnote">
                            <mp:SiteLabel ID="lblRejectionReason" runat="server" CssClass="settinglabel" ConfigKey="RejectionCommentLabel">
                            </mp:SiteLabel>
                            <asp:Literal ID="ltlRejectionReason" runat="server"></asp:Literal>
                        </div>
                    </div>
                </asp:Panel>
                <asp:UpdatePanel ID="updHx" UpdateMode="Conditional" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="grdHistory" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnlHistory" runat="server" Visible="false">
                            <h2 class="heading versionheading">
                                <asp:Literal ID="litVersionHistory" runat="server" EnableViewState="false" /></h2>
                            <mp:mojoGridView ID="grdHistory" runat="server" CssClass="" AutoGenerateColumns="false"
                                DataKeyNames="Guid">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        <asp:Literal ID="litCreated" runat="server" EnableViewState="false" Text='<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("CreatedUtc")), timeZone, "g", timeOffset) %>' />   
                                            <br />
                                            <asp:Literal ID="litUserName" runat="server" EnableViewState="false" Text='<%# Eval("UserName") %>' />    
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        <asp:Literal ID="litHxDate" runat="server" EnableViewState="false" Text='<%# DateTimeHelper.Format(Convert.ToDateTime(Eval("HistoryUtc")), timeZone, "g", timeOffset) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>  
                                            <asp:HyperLink id="lnkcompare" runat="server" CssClass="cblink" EnableViewState="false" NavigateUrl='<%# SiteRoot + "/HtmlCompare.aspx?pageid=" + pageId + "&mid=" + moduleId + "&h=" + Eval("Guid") %>'
                                                Text='<%$ Resources:Resource, HtmlCompareHistoryToCurrentLink %>' ToolTip='<%$ Resources:Resource, HtmlCompareHistoryToCurrentLink %>' />
                                            <asp:Button ID="btnRestoreToEditor" runat="server" Text='<%$ Resources:Resource, RestoreToEditorButton %>'
                                                CommandName="RestoreToEditor" CommandArgument='<%# Eval("Guid") %>' />
                                            <asp:Button ID="btnDelete" runat="server" CommandName="DeleteHistory" CommandArgument='<%# Eval("Guid") %>'
                                                Visible='<%# isAdmin %>' Text='<%$ Resources:Resource, DeleteHistoryButton %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                        <p class="nodata"><asp:Literal id="litempty" runat="server" EnableViewState="false" Text="<%$ Resources:Resource, GridViewNoData %>" /></p>
                                </EmptyDataTemplate>
                            </mp:mojoGridView>
                            <portal:mojoCutePager ID="pgrHistory" runat="server" />
                            <div id="divHistoryDelete" runat="server" class="settingrow">
                                <portal:mojoButton ID="btnDeleteHistory" runat="server" Text="" />
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            
        </asp:Panel>
        <asp:HiddenField ID="hdnReturnUrl" runat="server" />
   </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
    <portal:SessionKeepAliveControl ID="ka1" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server">
</asp:Content>
