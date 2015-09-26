<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AdvFileManager.ascx.cs" Inherits="mojoPortal.Web.AdminUI.AdvFileManager" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="mojoPortal.FileSystem" %>
<%@ Import Namespace="mojoPortal.Web.Framework" %>
<%@ Import Namespace="Resources" %>

<portal:mojoLabel ID="lblDisabledMessage" runat="server" CssClass="txterror info" />
<asp:Panel ID="pnlFile" runat="server">
<div id="qtfile" class="ui-helper-reset ui-widget qtfile" >
	<h3 class="ui-helper-reset ui-widget-header ui-corner-top qtfile-header">
		<%= Server.HtmlEncode(displayPath)%>
	</h3>
	<div class="ui-widget-content">
		<div class="menu-bar">
			<span id="buttonCreateFolder" class="button folder-create" title='<%= Resources.Resource.FolderCreateNewTooltip %>'>
				<span class="icon"></span><%= Resources.Resource.FolderNewButton %></span> <span id="buttonRenameFolder" class="button folder-rename"
					title='<%= Resources.Resource.FolderRenameTooltip %>'><span class="icon"></span><%= Resources.Resource.FileManagerRename %></span> <span id="buttonMoveFolder"
						class="button folder-move" title='<%= Resources.Resource.FolderMoveTooltip %>'><span class="icon"></span>
						<%= Resources.Resource.FolderMove %></span> <span id="buttonDeleteFolder" class="button folder-delete" title='<%= Resources.Resource.FolderDeleteTooltip %>'>
							<span class="icon"></span><%= Resources.Resource.FileManagerDelete %></span> <span id="buttonUploadFile" class="button file-upload"
								title='<%= Resources.Resource.FileUploadTooltip %>'><span class="icon"></span><%= Resources.Resource.FileManagerUploadButton %></span>
			<span id="buttonRefreshFileList" class="button file-refresh" title='<%= Resources.Resource.FolderRefreshTooltip %>'>
				<span class="icon"></span></span>
		</div>
		<div class="ui-helper-clearfix qtfile-content">
			<div class="folder-wraper">
				<div class="ui-state-default folder-header">
					<span><%= Resources.Resource.Folders %></span><span id="buttonRefreshFolderList" class="button folder-refresh"
						title='<%= Resources.Resource.FolderRefreshTooltip %>'><span class="icon"></span></span></div>
				<div id="folderListPanel" class="folder-panel">
					<% if (Model.Folders.Count() > 0)
		{ %>
					<ul>
						<% foreach (var folder in Model.Folders)
		 {
			 if (folder.VirtualPath == Model.CurrentFolder)
			 { %>
						<li class="current-folder">
							<%= Server.HtmlEncode(StringHelper.ToJsonString(folder.ToJson()))%></li>
						<% }
			 else
			 { %>
						<li>
							<%= Server.HtmlEncode(StringHelper.ToJsonString(folder.ToJson()))%></li>
						<% }
		 } %></ul>
					<% } %>
				</div>
			</div>
			<div class="file-wraper">
				<div class="ui-state-default ui-helper-clearfix file-header">
					<span class="ui-state-default file-name-header"><%= Resources.Resource.FileManagerFileNameLabel %></span><span class="ui-state-default file-size-header"><%= Resources.Resource.FileSize %></span>
					<span class="ui-state-default file-actions-header"><%= Resources.Resource.FileActions %></span>
				</div>
				<div id="fileListPanel" class="ui-helper-clearfix ui-widget-content file-panel">
					<% if (Model.Files.Count() > 0)
		{ %>
					<ul class="file-list">
						<% foreach (var file in Model.Files)
		 {%>
						<li>
							<%= Server.HtmlEncode(StringHelper.ToJsonString(file.ToJson()))%></li>
						<%  } %></ul>
					<% } %>
				</div>
			</div>
			<div class="ui-dialog ui-widget-content ui-corner-top file-preview">
				<div class="ui-dialog-titlebar ui-state-default ui-corner-all ui-helper-clearfix">
					<span class="ui-dialog-title"><%= Resources.Resource.FileDetails %></span> <span class="ui-dialog-titlebar-close ui-corner-all file-preview-close">
						<span class="ui-icon ui-icon-closethick"><%= Resources.Resource.FileClosePreview %></span> </span>
				</div>
				<div class="ui-dialog-content ui-widget-content">
					<p><span class="file-preview-field-name"><%= Resources.Resource.FileManagerFileNameLabel %></span> <span class="file-preview-name"></span></p>
					<p><span class="file-preview-field-name"><%= Resources.Resource.FileSize %></span> <span class="file-preview-size"></span></p>
					<p><span class="file-preview-field-name"><%= Resources.Resource.FileContentType %></span> <span class="file-preview-content-type"></span></p>
					<p><span class="file-preview-field-name"><%= Resources.Resource.FileModified %></span> <span class="file-preview-modified"></span></p>
					<p class="file-preview-image"><span class="file-preview-image-loading"></span></p>
				</div>
			</div>
		</div>
	</div>
	<div class="ui-helper-clearfix ui-state-default ui-corner-bottom qtfile-footer">
		<span id="statusMessage" class="status-message"></span> <span class="credit">&nbsp;</span>
	</div>
</div>
</asp:Panel>
