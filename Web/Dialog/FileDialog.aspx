<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="FileDialog.aspx.cs" Inherits="mojoPortal.Web.Dialog.FileDialog" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <portal:StyleSheetCombiner ID="StyleSheetCombiner" runat="server" />
    <portal:ScriptLoader ID="ScriptInclude" runat="server" IncludejQueryFileTree="true"  />
</head>
<body class="filedialog">
    <form id="form1" runat="server">
	    <div id="filewrapper">
	    	<div class="fileoperations">
	    		<div class="foldercreation">
	    			<span class="operationheading"><asp:Literal ID="litCreateFolder" runat="server" /></span>
	    			<div class="operationpanel">
		    			<span class="operationinstructions"><asp:Literal ID="litFolderInstructions" runat="server" /></span>
		                <asp:Panel ID="pnlUpload" runat="server" Visible="false">
		                    <asp:Panel ID="pnlNewFolder" runat="server" CssClass="settingrow" DefaultButton="btnNewFolder">
		                        <asp:TextBox ID="txtNewDirectory" runat="server" Style="width: 150px" MaxLength="150"></asp:TextBox>
		                        <asp:Button ID="btnNewFolder" runat="server" ValidationGroup="newfolder" />
		                        <div class="errorpanel">
			                        <asp:RequiredFieldValidator ID="requireFolder" runat="server" ControlToValidate="txtNewDirectory"
			                            Display="Dynamic" ValidationGroup="newfolder" CssClass="txterror error" />
			                        <asp:RegularExpressionValidator ID="regexFolder" runat="server" ControlToValidate="txtNewDirectory"
			                            Display="Dynamic" ValidationGroup="newfolder" CssClass="txterror error" />
			                        <portal:mojoLabel ID="lblError" runat="server" CssClass="txterror error" />
		                        </div>
		                    </asp:Panel>
		                </asp:Panel>
	                </div>
	    		</div>
	    		<div id="divFileUpload" runat="server" class="fileupload">
	    			<span class="operationheading"><asp:Literal ID="litUpload" runat="server" /></span>
	    			<span class="operationinstructions"><asp:Literal ID="litUploadInstructions" runat="server" /></span>
		            <div class="imageresizeoptions">
		                <asp:CheckBox ID="chkConstrainImageSize" runat="server"  CssClass="imageresizecheckbox" /> 
		                <mp:SiteLabel ID="lbl1" ForControl="txtMaxWidth" runat="server" ConfigKey="FileBrowserMaxWidth" EnableViewState="false" CssClass="boldtext"></mp:SiteLabel>
		                <asp:TextBox ID="txtMaxWidth" runat="server" CssClass="smalltextbox" />
		                <mp:SiteLabel ID="SiteLabel3" ForControl="txtMaxHeight" runat="server" ConfigKey="FileBrowserMaxHeight" EnableViewState="false" CssClass="boldtext"></mp:SiteLabel>
		                <asp:TextBox ID="txtMaxHeight" runat="server" CssClass="smalltextbox" />
		            </div>
	                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnUpload" CssClass="settingrow">     
                        <portal:jQueryFileUpload ID="uploader" runat="server" />
	                    <asp:Button ID="btnUpload" runat="server" Text="Upload" ValidationGroup="upload"></asp:Button>
	                </asp:Panel>
                    <div class="errorpanel">
	                    <asp:RegularExpressionValidator ID="regexFile" ControlToValidate="uploader" Display="Dynamic"
	                        EnableClientScript="True" runat="server" ValidationGroup="upload" CssClass="txterror error" />
	                    <asp:RequiredFieldValidator ID="reqFile" runat="server" ControlToValidate="uploader"
	                        Display="Dynamic" ValidationGroup="upload" CssClass="txterror error" />
                    </div>	                

	    		</div>
	    		<div class="clearpanel">&nbsp;</div>
	    	</div>
		    <div class="fileselection">
            	<div class="filetreewrapper">
                    <asp:Literal ID="litFileBrowser" runat="server" />
	                <ul class="rootfolder">
	                    <li class="expanded">
	                        <asp:HyperLink ID="lnkRoot" runat="server"></asp:HyperLink>
	                        <asp:Panel ID="pnlFileTree" runat="server" />
	                    </li>
	                </ul>
                </div>
	            <div class="filepreview">
                    <asp:Literal ID="litPreview" runat="server" />
	            	<div class="operationpanel">
		                <asp:Literal ID="litHeading" Visible="false" runat="server" />
		                <span class="operationinstructions"><asp:Literal ID="litFileSelectInstructions" runat="server" /> </span>
		                <div class="settingrow">
		                    <asp:Button ID="btnSubmit" runat="server" Text="Select File" Width="90px" />
		                    <asp:TextBox ID="txtSelection" runat="server" Style="width: 350px; border: 0px" Enabled="false" />&nbsp;
		                </div>
		                <div class="settingrow">
		                    <mp:SiteLabel ID="SiteLabel1" runat="server" CssClass="settinglabel" ConfigKey="spacer" EnableViewState="false"></mp:SiteLabel>
		                </div>
		                <div id="GalleryPreview">
		                    <div id="GalleryPreview_VerticalFix">
		                        <asp:Image ID="imgPreview" runat="server" ImageUrl="~/Data/SiteImages/1x1.gif" /><br />
		                        <asp:HyperLink id="lnkImageCropper" runat="server" />
		                        <asp:LinkButton ID="btnDelete" runat="server" />
		                    </div>
		                </div>
	                </div>
	            </div>
		    </div>
		    <div class="clearpanel">
		        <asp:HiddenField ID="hdnFolder" runat="server" />
		        <asp:HiddenField ID="hdnFileUrl" runat="server"  />
		    </div>
	    </div>  
	    <portal:mojoGoogleAnalyticsScript ID="mojoGoogleAnalyticsScript1" runat="server" />
    </form>
</body>
</html>
