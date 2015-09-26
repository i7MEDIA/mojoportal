<%@ Page language="c#" Codebehind="EditCategory.aspx.cs" MasterPageFile="~/App_MasterPages/layout.Master" AutoEventWireup="false" Inherits="mojoPortal.Web.BlogUI.BlogCategoryEdit" %>

<asp:Content ContentPlaceHolderID="leftContent" ID="MPLeftPane" runat="server" />
<asp:Content ContentPlaceHolderID="mainContent" ID="MPContent" runat="server">
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper blogcategoryedit">
<portal:HeadingControl ID="heading" runat="server" />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
		<asp:Panel id="pnlBlog" runat="server" DefaultButton="btnAddCategory">
		    <div class="settingrow">
		        <asp:Label ID="lblError" Runat="server" CssClass="txterror"></asp:Label>
		    </div>
		    <div class="settingrow">
		        <portal:mojoButton  runat="server" id="btnAddCategory"></portal:mojoButton>
				<asp:TextBox ID="txtNewCategoryName" Runat="server" MaxLength="50" Columns="50"></asp:TextBox>
		    </div>
		    <div class="settingrow">
		        <portal:mojoDataList id="dlCategories" DataKeyField="CategoryID" runat="server">
					<ItemTemplate>
						<asp:ImageButton runat="server" ID="btnEdit"
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + EditContentImage %>'
						    CommandName="edit" ToolTip="<%# Resources.BlogResources.EditImageAltText%>"
						    AlternateText="<%# Resources.BlogResources.EditImageAltText%>" 
						        />
						<asp:ImageButton runat="server" ID="btnDelete"
						    ImageUrl='<%# ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage %>'
						    CommandName="delete" ToolTip="<%# Resources.BlogResources.BlogEditDeleteButton%>"
						    AlternateText="<%# Resources.BlogResources.BlogEditDeleteButton%>" 
						 />
						&nbsp;&nbsp;
						<%# DataBinder.Eval(Container.DataItem, "Category") %>
					</ItemTemplate>
					<EditItemTemplate>
					    <div >
						<asp:Textbox id="CategoryName" runat="server"
						    MaxLength="50" 
						    CssClass="widetextbox"
						    Text='<%# DataBinder.Eval(Container.DataItem, "Category") %>'  />&nbsp;
						<asp:Button Text="<%# Resources.BlogResources.BlogEditUpdateButton%>" ToolTip="<%# Resources.BlogResources.BlogEditUpdateButton%>"
						    CommandName="apply" runat="server" ID="Button1" />
						</div>
					</EditItemTemplate>
				</portal:mojoDataList>
		    </div>
		</asp:Panel>
		</portal:InnerBodyPanel>	
	</portal:OuterBodyPanel>
	<portal:EmptyPanel id="divCleared" runat="server" CssClass="cleared" SkinID="cleared"></portal:EmptyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>
	</asp:Content>
<asp:Content ContentPlaceHolderID="rightContent" ID="MPRightPane" runat="server" />
<asp:Content ContentPlaceHolderID="pageEditContent" ID="MPPageEdit" runat="server" />
