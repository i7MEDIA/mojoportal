<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ArchiveViewControl.ascx.cs"Inherits="mojoPortal.Web.BlogUI.ArchiveViewControl" %>

<%@ Register TagPrefix="blog" TagName="NavControl" Src="~/Blog/Controls/BlogNav.ascx" %>
<%@ Register Namespace="mojoPortal.Web.BlogUI" Assembly="mojoPortal.Features.UI" TagPrefix="blog" %>
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" cssclass="panelwrapper blogmodule blogcategories blogarchive ">
    <portal:HeadingControl ID="heading" runat="server" />
    <portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
        <portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
            <asp:panel id="pnlBlog" runat="server" cssclass="blogwrapper">
                <blog:BlogDisplaySettings ID="displaySettings" runat="server" />
                <blog:NavControl ID="navTop" runat="server" ShowCalendar="false" />
                <asp:panel id="divblog" runat="server" cssclass="blogcenter-rightnav">
                    <asp:repeater id="dlArchives" runat="server" enableviewstate="False">
                        <itemtemplate>
			            <h3 class="blogtitle">
				        <asp:HyperLink id="Title" runat="server" 
				            Text='<%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem,"Heading").ToString()) %>' 
				            NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>'>
				        </asp:HyperLink></h3>
				        <div class="blogcontent">
                        <blog:BlogDatePanel ID="pnlBottomDate" runat="server" CssClass="blogdate barchivedate">
                        <%# FormatBlogDate(Convert.ToDateTime(Eval("StartDate"))) %>
                        <asp:HyperLink id="Hyperlink2" runat="server" CssClass="barchivefeedbacklink" Text='<%# FeedBackLabel + "(" + DataBinder.Eval(Container.DataItem,"CommentCount") + ")" %>' 
				        Visible='<%# AllowComments %>' 
				        NavigateUrl='<%# FormatBlogUrl(DataBinder.Eval(Container.DataItem,"ItemUrl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem,"ItemID"))) %>'>
				        </asp:HyperLink>
                        </blog:BlogDatePanel>
				        
				        </div>
			        </itemtemplate>
                    </asp:repeater>
                    <asp:label id="lblCopyright" runat="server" cssclass="txtcopyright"></asp:label>
                </asp:panel>
                <blog:NavControl ID="navBottom" runat="server" ShowCalendar="false" Visile="false" />
            </asp:panel>
            
        </portal:InnerBodyPanel>
    </portal:OuterBodyPanel>
    <portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
