<%@ Page Language="C#" AutoEventWireup="false" MasterPageFile="~/App_MasterPages/layout.Master" CodeBehind="Drafts.aspx.cs" Inherits="mojoPortal.Web.BlogUI.BlogDraftsPage" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<blog:BlogDisplaySettings ID="displaySettings" runat="server" />
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogdrafts">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">

		<asp:repeater id="rptDrafts" runat="server"  EnableViewState="False" >
            <HeaderTemplate><ul class="simplelist bdrafts"></HeaderTemplate>
			<ItemTemplate>
			    <li>
				<asp:HyperLink id="Title" runat="server"  SkinID="plain"
				    Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Heading").ToString()) %>' 
				    NavigateUrl='<%# SiteRoot + "/Blog/EditPost.aspx?pageid=" + PageId.ToString() + "&ItemID=" + DataBinder.Eval(Container.DataItem,"ItemID") + "&mid=" + ModuleId %>'>
				</asp:HyperLink>&#160;
                    <span class="blogdate">
                                   <span id="spnAuthor" runat="server" enableviewstate="false" visible='<%# ShowAuthor %>' class="blogauthor">
                                       <%# FormatPostAuthor(Eval("Name").ToString(),Eval("FirstName").ToString(),Eval("LastName").ToString())%></span>
                         <asp:Literal ID="litPubInfo" runat="server" Text='<%# Resources.BlogResources.BlogToBePublishedLabel + FormatBlogDate(Convert.ToDateTime(Eval("StartDate"))) %>' 
				    Visible='<%# Convert.ToBoolean(Eval("IsPublished")) %>' />
                               </span>
                    
			    </li>	
			</ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
		</asp:repeater>
        <div class="blogpager">
            <portal:mojoCutePager ID="pgr" runat="server" />
        </div>
   </portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />